using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DEvahebLib.Nodes;
using DEvahebLib.Parser;

namespace DEvahebLib.Visitors
{
    public class GenerateIBI : Visitor
    {
        private BinaryWriter writer;
        private float version;
        private bool isNested = false;

        public GenerateIBI(BinaryWriter writer, float version = 1.57f)
        {
            this.writer = writer;
            this.version = version;
        }

        public override void Visit(List<Node> rootNodes)
        {
            writer.Write("IBI\0".ToCharArray());
            writer.Write(version);

            base.Visit(rootNodes);
        }

        private void WriteTokenHeader(IBIToken token, int size)
        {
            writer.Write((int)token);

            if (!isNested)
            {
                writer.Write(size);
                writer.Write((byte)0);
            }
            else if ((int)token > 7)
            {
                // IBI flat format: nested members with token > 7 have a 4-byte data
                // payload containing float(token_value) for functions, or float(0) for operators.
                writer.Write(4);
                if (token >= IBIToken.Gt && token <= IBIToken.Ne)
                {
                    writer.Write(0.0f);
                }
                else
                {
                    writer.Write((float)(int)token);
                }
            }
            else
            {
                writer.Write(size);
            }
        }

        private void WriteBlockEnd()
        {
            writer.Write((int)IBIToken.blockEnd);
            writer.Write(0);
            writer.Write((byte)0);
        }

        private int GetArgumentSizeSum(FunctionNode node)
        {
            int count = 0;

            foreach (var arg in node.Arguments)
            {
                count += arg.Size;
            }

            return count;
        }

        private IBIToken GetFunctionToken(FunctionNode node)
        {
            if (node is If) return IBIToken.@if;
            if (node is Else) return IBIToken.@else;
            if (node is Task) return IBIToken.task;
            if (node is Affect) return IBIToken.affect;
            if (node is Loop) return IBIToken.loop;
            if (node is Set) return IBIToken.set;
            if (node is Get) return IBIToken.get;
            if (node is Tag) return IBIToken.tag;
            if (node is Nodes.Random) return IBIToken.random;
            if (node is Declare) return IBIToken.declare;
            if (node is Sound) return IBIToken.sound;
            if (node is Play) return IBIToken.play;
            if (node is Camera) return IBIToken.camera;
            if (node is Use) return IBIToken.use;
            if (node is Print) return IBIToken.print;
            if (node is Flush) return IBIToken.flush;
            if (node is Rotate) return IBIToken.rotate;
            if (node is WaitSignal) return IBIToken.waitsignal;
            if (node is Move) return IBIToken.move;
            if (node is Remove) return IBIToken.remove;
            if (node is Free) return IBIToken.free;
            if (node is Signal) return IBIToken.signal;
            if (node is Do) return IBIToken.@do;
            if (node is Run) return IBIToken.run;
            if (node is Kill) return IBIToken.kill;
            if (node is Wait) return IBIToken.wait;
            if (node is Rem) return IBIToken.rem;
            if (node is BlockEnd) return IBIToken.blockEnd;
            if (node is GenericFunction gf)
            {
                if (Enum.TryParse<IBIToken>(gf.Name, out var token))
                    return token;

                throw new Exception($"Unknown generic function name: {gf.Name}");
            }

            throw new Exception($"Unknown function node type: {node.GetType().Name}");
        }

        public override void VisitValueNode(ValueNode node)
        {
            if (node is VectorValue vector)
            {
                WriteVector(vector);
            }
            else if (node is EnumValue enumValue)
            {
                WriteEnumValue(enumValue);
            }
            else if (node is IntegerValue intVal)
            {
                // IBI format stores all numbers as floats;
                // IntegerValues are parser-side conversions (Loop, Camera)
                WriteTokenHeader(IBIToken.Float, 4);
                writer.Write((float)(int)intVal.Integer);
            }
            else if (node is FloatValue floatVal)
            {
                WriteTokenHeader(IBIToken.Float, 4);
                writer.Write((float)floatVal.Float);
            }
            else if (node is IdentifierValue idVal)
            {
                int len = idVal.String.Length + 1;
                WriteTokenHeader(IBIToken.Identifier, len);
                writer.Write(idVal.String.ToCharArray());
                writer.Write('\0');
            }
            else if (node is StringValue strVal)
            {
                int len = strVal.String.Length + 1;
                WriteTokenHeader(IBIToken.String, len);
                writer.Write(strVal.String.ToCharArray());
                writer.Write('\0');
            }
            else if (node is CharValue charVal)
            {
                WriteTokenHeader(IBIToken.Char, 1);
                writer.Write((char)charVal.Char);
            }
        }

        private void WriteVector(VectorValue vector)
        {
            WriteTokenHeader(IBIToken.Vector, 0);

            bool saved = isNested;
            isNested = true;

            foreach (var component in vector.Values)
            {
                Visit(component);
            }

            isNested = saved;
        }

        private void WriteEnumValue(EnumValue enumValue)
        {
            if (enumValue is EnumFloatValue efv)
            {
                WriteTokenHeader(IBIToken.Float, 4);
                writer.Write(efv.Float);
            }
            else if (enumValue is EnumIdentifierValue eiv)
            {
                int len = eiv.IdentifierName.Length + 1;
                WriteTokenHeader(IBIToken.Identifier, len);
                writer.Write(eiv.IdentifierName.ToCharArray());
                writer.Write('\0');
            }
            else if (enumValue is EnumStringValue esv)
            {
                int len = esv.String.Length + 1;
                WriteTokenHeader(IBIToken.String, len);
                writer.Write(esv.String.ToCharArray());
                writer.Write('\0');
            }
            else if (enumValue is EnumIntValue eintv)
            {
                WriteTokenHeader(IBIToken.Float, 4);
                writer.Write((float)eintv.Integer);
            }
        }

        public override void VisitOperatorNode(OperatorNode node)
        {
            IBIToken token = (IBIToken)(int)node.Operator;
            WriteTokenHeader(token, 0);
        }

        public override void VisitFunctionNode(FunctionNode node)
        {
            if (node is DoWait)
            {
                VisitFunctionNode(new Do(node.Arguments.ToArray()));
                VisitFunctionNode(new Wait(node.Arguments.ToArray()));
                return;
            }

            IBIToken token = GetFunctionToken(node);
            int size = GetArgumentSizeSum(node);

            WriteTokenHeader(token, size);

            bool saved = isNested;
            isNested = true;
            base.VisitFunctionNode(node);
            isNested = saved;
        }

        public override void VisitBlockNode(BlockNode node)
        {
            IBIToken token = GetFunctionToken(node);
            int size = GetArgumentSizeSum(node);

            WriteTokenHeader(token, size);

            bool saved = isNested;
            isNested = true;
            VisitBlockNodeArguments(node);
            isNested = saved;

            VisitBlockNodeSubNodes(node);

            WriteBlockEnd();
        }

        public override void VisitMisteryNode(Node node)
        {
            throw new Exception("Cannot write unknown node type to IBI");
        }
    }
}
