using System;
using System.Collections.Generic;
using System.Globalization;
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
        BehavED = 0,
        BareExpressions = 1
    }

    public class GenerateIcarus : StackVisitorBase
    {
        public Variables Variables { get; protected set; }

        public SourceCodeParity Parity { get; set; } = SourceCodeParity.BehavED;

        public string Indentation { get; set; } = "\t";

        public StringBuilder SourceCode { get; protected set; } = new StringBuilder();

        protected string indent = String.Empty;

        public GenerateIcarus()
            : this(variables: null)
        {
        }

        public GenerateIcarus(Variables variables)
            : base()
        {
            Variables = variables;
        }

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
            if (Parity == SourceCodeParity.BehavED)
            {
                if (argumentStack.Peek().Item1 is Set set && set.VariableName is StringValue variableName)
                {
                    // if we're handling the second argument of the set function
                    if (node == set.Value)
                    {
                        if (Variables.Exists(variableName.String))
                        {
                            var typeName = Variables.GetVariableType(variableName.String);

                            if (typeName.StartsWith("\"") && typeName.EndsWith("\""))
                            {
                                SourceCode.Append($"/*@{typeName.Substring(1, typeName.Length - 2)}*/ ");
                            }
                            else if (typeName == "INT" && node is FloatValue f)
                            {
                                node = new IntegerValue((Int32)f.Float);
                            }
                        }
                    }
                    // if we're handling the first argument of the set function
                    else if (node == set.VariableName)
                    {
                        if (Variables.Exists(variableName.String))
                        {
                            SourceCode.Append($"/*@SET_TYPES*/ ");
                        }
                    }
                }
            }

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

                    if (argumentStack.Peek().Item1 is Nodes.Random && Parity == SourceCodeParity.BehavED)
                    {
                        // weirdly BehavED doesn't format random float with 3 decimal points by default
                        SourceCode.Append(((float)floatValue.Float).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        SourceCode.Append(((float)floatValue.Float).ToString("0.000", CultureInfo.InvariantCulture));
                    }
                }
                else if (node is IntegerValue integerValue)
                {
                    if (integerValue.Integer == null)
                        throw new MissingArgumentException("Integer is missing a value");

                    SourceCode.Append(((int)integerValue.Integer).ToString(CultureInfo.InvariantCulture));
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
                    if (Parity == SourceCodeParity.BehavED)
                    {
                        // skip types for second argument of tag (tag type) and first argument of get (declare type)
                        if (!(argumentStack.Peek().Item1 is Tag skipTag && node == skipTag.TagType)
                            && !(argumentStack.Peek().Item1 is Get skipGet && node == skipGet.Type))
                        {
                            string enumName = enumValue.Name;

                            // if we don't know the enum name but we're trying to handle the variable name for get()
                            if (string.IsNullOrEmpty(enumName) && argumentStack.Peek().Item1 is Get get
                                && node == get.VariableName && get.VariableName is StringValue getVariable)
                            {
                                // Check if this variable name is in the set types list
                                var typeName = Variables.GetVariableType(getVariable.String);

                                // only care if the set type is an enum type (which is in quotes)
                                if (typeName.StartsWith("\"") && typeName.EndsWith("\""))
                                {
                                    enumName = typeName.Substring(1, typeName.Length - 2);
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(enumName))
                            {
                                SourceCode.Append($"/*@{enumValue.Name}*/ ");
                            }
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
            }

            SourceCode.Append(op.ToString());
        }

        public override void VisitFunctionArgument(FunctionNode function, Node argument)
        {
            if (argumentStack.Peek().Item2.Count > 0) // if we're not the first argument
            {
                if (Parity == SourceCodeParity.BehavED || !(function is If)) // output all IF arguments as one expression if not for behaved
                {
                    SourceCode.Append(",");
                }
            }

            SourceCode.Append(" ");

            if (function is If)
            {
                // NOTE:
                // $-signs around values and function calls are to aid BehavED to replace text expressions with dropdown selections
                // The IBIZE compiler can handle with or without
                // An expression can be split up into comma-separated parts, each inside $-signs
                // For the If operators the expression can split up into 3 parts: expr1, operator, expr2

                if (Parity == SourceCodeParity.BehavED) SourceCode.Append("$");
                base.VisitFunctionArgument(function, argument);
                if (Parity == SourceCodeParity.BehavED) SourceCode.Append("$");
            }
            else if (argument is Get || argument is Tag || argument is Nodes.Random)
            {
                if (Parity == SourceCodeParity.BehavED) SourceCode.Append("$");
                base.VisitFunctionArgument(function, argument);
                if (Parity == SourceCodeParity.BehavED) SourceCode.Append("$");
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
