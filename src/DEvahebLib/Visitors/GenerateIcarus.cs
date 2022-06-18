using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Enums;
using DEvahebLib.Exceptions;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public enum SourceCodeParity
    {
        BehavED = 0
    }

    public class GenerateIcarus : StackVisitorBase
    {
        public SourceCodeParity Parity { get; set; } = SourceCodeParity.BehavED;

        public string Indentation { get; set; } = "\t";

        public StringBuilder SourceCode { get; protected set; } = new StringBuilder();

        protected string indent = String.Empty;

        public override void Visit(Node node)
        {
            if (blockStack.Count == 0) // in case of indent at root level
            {
                if (Parity == SourceCodeParity.BehavED)
                {
                    if (node is BlockNode) // new empty line before block node
                    {
                        SourceCode.AppendLine();
                    }
                }

                SourceCode.Append(indent);
            }

            base.Visit(node);
        }

        public override void VisitValueNode(ValueNode node)
        {
            if (node is VectorValue vectorValue)
            {
                if (vectorValue.Values[0] == null)
                    throw new MissingArgumentException("Vector is missing first value argument");
                if (vectorValue.Values[1] == null)
                    throw new MissingArgumentException("Vector is missing second value argument");
                if (vectorValue.Values[2] == null)
                    throw new MissingArgumentException("Vector is missing third value argument");

                SourceCode.Append("<");

                base.VisitValueNode(node);

                SourceCode.Append(" >");
            }
            else
            {
                base.VisitValueNode(node);

                if (node is FloatValue floatValue)
                {
                    if (floatValue.Float == null)
                        throw new MissingArgumentException("Float is missing a value");

                    SourceCode.Append(((float)floatValue.Float).ToString("0.000"));
                }
                else if (node is IntegerValue integerValue)
                {
                    if (integerValue.Integer == null)
                        throw new MissingArgumentException("Integer is missing a value");

                    SourceCode.Append(integerValue.Integer.ToString());
                }
                else if (node is IdentifierValue identifierValue)
                {
                    if (string.IsNullOrWhiteSpace(identifierValue.String))
                        throw new MissingArgumentException("String is missing a value");

                    SourceCode.Append(identifierValue.String);
                }
                else if (node is StringValue stringValue)
                {
                    if (stringValue.String == null)
                        throw new MissingArgumentException("String is missing a value");

                    SourceCode.Append($"\"{stringValue.String}\"");
                }
                else if (node is CharValue charValue)
                {
                    if (charValue.Char == null)
                        throw new MissingArgumentException("Char is missing a value");

                    SourceCode.Append(charValue.Char);
                }
                else if (node is EnumValue enumValue)
                {
                    if (Parity == SourceCodeParity.BehavED &&
                        (!(argumentStack.Peek().Item1 is Tag) && !(argumentStack.Peek().Item1 is Get)))
                    {
                        // TODO fix for set signatures "varname" versus SET_Types enum
                        if (argumentStack.Peek().Item1.Name != "set" || enumValue.KnowsValue(enumValue.Value))
                        {
                            SourceCode.Append($"/*@{enumValue.Name}*/ ");
                        }
                    }

                    SourceCode.Append(enumValue.Text);
                }
            }
        }

        public override void VisitVectorValue(VectorValue vector, Node node)
        {
            SourceCode.Append(" ");
            base.VisitVectorValue(vector, node);
        }

        public override void VisitBlockNode(BlockNode node)
        {
            SourceCode.Append($"{node.Name}");

            base.VisitBlockNode(node);
        }

        public override void VisitBlockNodeArguments(BlockNode node)
        {
            SourceCode.Append(" (");

            if (Parity == SourceCodeParity.BehavED && node.Arguments?.FirstOrDefault() == null)
            {
                SourceCode.Append(" ");
            }

            base.VisitBlockNodeArguments(node);

            SourceCode.AppendLine(" )");
        }

        public override void VisitBlockNodeSubNodes(BlockNode node)
        {
            SourceCode.Append(indent);
            SourceCode.AppendLine("{");

            int length = indent.Length;
            indent += Indentation ?? "\t";

            base.VisitBlockNodeSubNodes(node);

            indent = indent.Substring(0, length);

            SourceCode.Append(indent);
            SourceCode.AppendLine("}");
            
            if (Parity == SourceCodeParity.BehavED)
            {
                SourceCode.AppendLine();
            }
        }

        public override void VisitBlockNodeSubNode(BlockNode blockNode, Node subNode)
        {
            if (Parity == SourceCodeParity.BehavED)
            {
                if (subNode is BlockNode) // new empty line before block node
                {
                    SourceCode.AppendLine();
                }
            }

            SourceCode.Append(indent);

            base.VisitBlockNodeSubNode(blockNode, subNode);

            if (!(subNode is BlockNode))
            {
                SourceCode.AppendLine(";");
            }
        }

        public override void VisitOperatorNode(OperatorNode node)
        {
            char op = '=';

            switch (node.Operator)
            {
                case Operator.Gt:
                    op = '>';
                    break;
                case Operator.Lt:
                    op = '<';
                    break;
                case Operator.Eq:
                    op = '=';
                    break;
                case Operator.Ne:
                    op = '!';
                    break;
                default: // defensive coding?
                    throw new ArgumentOutOfRangeException("Operator value is invalid");
            }

            SourceCode.Append(op.ToString());
        }

        public override void VisitFunctionArgument(FunctionNode function, Node argument)
        {
            if (argumentStack.Peek().Item2.Count > 0) // if we're not the first argument
            {
                SourceCode.Append(",");
            }

            SourceCode.Append(" ");

            if (function is If)
            {
                // NOTE:
                // $-signs around values don't seem to be required:
                //      - Behaved saves to source files like that, but can read fine without
                //      - IBIZE compiles with or without
                // For the If operators (=, !, >, <) behaved can NOT read without $, but IBIZE compiles fine

                SourceCode.Append("$");
                base.VisitFunctionArgument(function, argument);
                SourceCode.Append("$");
            }
            else if (argument is Get || argument is Tag || argument is Nodes.Random)
            {
                SourceCode.Append("$");
                base.VisitFunctionArgument(function, argument);
                SourceCode.Append("$");
            }
            else
            {
                base.VisitFunctionArgument(function, argument);
            }
        }

        public override void VisitFunctionNode(FunctionNode node)
        {
            if (Parity == SourceCodeParity.BehavED &&
                (node is Tag || node is Get || node is Nodes.Random))
            {
                SourceCode.Append($"{node.Name}(");
            }
            else
            {
                SourceCode.Append($"{node.Name} (");
            }

            if (Parity == SourceCodeParity.BehavED && node.Arguments?.FirstOrDefault() == null)
            {
                SourceCode.Append(" ");
            }

            base.VisitFunctionNode(node);

            if (Parity == SourceCodeParity.BehavED && (node is Get || node is Tag))
            {
                SourceCode.Append(")");
            }
            else
            {
                SourceCode.Append(" )");
            }

            if (blockStack.Count == 0 && argumentStack.Count == 0) // root level, inside blocks is handled by visitblocknodes
            {
                SourceCode.AppendLine(";");
            }
        }

        public override void VisitMisteryNode(Node node)
        {
            base.VisitMisteryNode(node);

            SourceCode.Append($"**MISTERY**");
        }
    }
}
