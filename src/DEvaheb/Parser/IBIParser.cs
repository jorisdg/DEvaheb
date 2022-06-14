using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvaheb.Nodes;

namespace DEvaheb.Parser
{
    public enum IBIToken
    {
        UNKNOWN = 0,

        // Comment = 1
        // EOL = 2
        Char = 3, // Icarus
        String = 4,
        Int = 5, // Icarus
        Float = 6,
        Identifier = 7,

        TBlockStart = 8, // Icarus
        TBlockEnd = 9, // Icarus
        VectorStart = 10, // Icarus
        VectorEnd = 11, // Icarus
        OpenParenthesis = 12, // Icarus
        CloseParenthesis = 13, // Icarus

        Vector = 14,

        Gt = 15,
        Lt = 16,
        Eq = 17,
        Ne = 18,

        affect = 19,
        sound = 20,
        move = 21,
        rotate = 22,
        wait = 23,
        blockStart = 24, // Icarus
        blockEnd = 25,
        set = 26,
        loop = 27,
        loopEnd = 28, // Icarus
        print = 29,
        use = 30,
        flush = 31,
        runscript = 32,
        kill = 33,
        remove = 34,
        camera = 35,
        get = 36,
        random = 37,
        @if = 38,
        @else = 39,
        rem = 40, // Icarus
        task = 41,
        @do = 42,
        declare = 43,
        free = 44,
        dowait = 45, // Icarus
        signal = 46,
        waitsignal = 47,
        play = 48,
        tag = 49,
    }

    public class IBIParser
    {
        bool debugWrite = false;

        public Node ReadToken(BinaryReader reader)
        {
            var b = reader.ReadByte();
            if (b == 0)
            {
                var i = reader.ReadInt16();
                reader.BaseStream.Seek(-2, SeekOrigin.Current);

                var empty = reader.ReadBytes(2);
                if (debugWrite) Console.WriteLine($"{{ {empty[0].ToString()} {empty[1].ToString()} : {i}}}");

                return null;
            }

            IBIToken t = Enum.IsDefined(typeof(IBIToken), (Int32)b) ? (IBIToken)b : IBIToken.UNKNOWN;

            var bs = reader.ReadBytes(3);
            if (bs[0] != 0 || bs[1] != 0 || bs[2] != 0)
                File.AppendAllText("log.txt", $"Token {b} ({t.ToString()} 1st 3 bytes: {bs[0]}, {bs[1]}, {bs[2]}\r\n");

            var size = reader.ReadByte();
            bs = reader.ReadBytes(3);
            if (bs[0] != 0 || bs[1] != 0 || bs[2] != 0)
                File.AppendAllText("log.txt", $"Token {b} ({t.ToString()} 2nd 3 bytes: {bs[0]}, {bs[1]}, {bs[2]}\r\n");

            if (debugWrite) Console.WriteLine($"{b} ({t.ToString()}) : {size}");

            if (b > 7)
            {
                b = reader.ReadByte(); // TODO functions have 1 extra byte here? values do not?
            }

            Node newNode = null;
            List<Node> parms;

            switch (t)
            {
                case IBIToken.String:
                    newNode = ValueNode.Create(new string(reader.ReadChars(size)));
                    break;
                case IBIToken.Identifier:
                    newNode = ValueNode.Create(new string(reader.ReadChars(size)));
                    break;
                case IBIToken.Float:
                    newNode = ValueNode.Create(reader.ReadSingle());
                    break;
                case IBIToken.Vector:
                    if (ReadToken(reader) != null) throw new Exception("Vector expected 0 token");

                    newNode = ValueNode.CreateVector(x: (float)((FloatValue)ReadToken(reader)).Float, y: (float)((FloatValue)ReadToken(reader)).Float, z: (float)((FloatValue)ReadToken(reader)).Float);
                    break;
                case IBIToken.Gt:
                case IBIToken.Lt:
                case IBIToken.Eq:
                case IBIToken.Ne:
                    if (ReadToken(reader) != null) throw new Exception("operator expected 0 token");

                    newNode = If.CreateOperator((Operators)t);
                    break;
                case IBIToken.@if:
                    newNode = BlockNode.CreateIf(ReadToken(reader), (OperatorNode)ReadToken(reader), ReadToken(reader));
                    break;
                case IBIToken.random:
                    if (ReadToken(reader) != null) throw new Exception("random expected 0 token");

                    newNode = FunctionNode.CreateRandom(ReadToken(reader), ReadToken(reader));
                    break;
                case IBIToken.task:
                    newNode = BlockNode.CreateTask(name: ReadToken(reader));
                    break;
                case IBIToken.affect:
                    newNode = BlockNode.CreateAffect(name: ReadToken(reader), type: (Nodes.AffectType)(float)((FloatValue)ReadToken(reader)).Float);
                    break;
                //case IBIToken.affect:
                //case IBIToken.task:
                //    Output($"{t.ToString()} (");
                //    ReadSubTokens(reader, size);
                //    OutputLine(")");
                //    OutputLine("{");
                //    currentIndent += "  ";
                //    break;
                //case IBIToken.blockEnd:
                //    currentIndent = currentIndent.Length > 2 ? currentIndent.Substring(2) : string.Empty;
                //    OutputLine("}");
                //    break;
                case IBIToken.tag:
                    if (ReadToken(reader) != null) throw new Exception("tag expected 0 token");

                    newNode = FunctionNode.CreateTag(name: ReadToken(reader), type: (Nodes.TagType)(float)((FloatValue)ReadToken(reader)).Float);
                    break;
                case IBIToken.get:
                    if (ReadToken(reader) != null) throw new Exception("get expected 0 token");

                    newNode = FunctionNode.CreateGet(type: (Nodes.ValueType)(float)((FloatValue)ReadToken(reader)).Float, variableName: ReadToken(reader));
                    break;
                case IBIToken.camera:
                    parms = new List<Node>();
                    for (int i = 0; i < size; i++)
                    {
                        if (debugWrite) Console.WriteLine($"- Param {i} ");
                        var node = ReadToken(reader);
                        if (node != null)
                        {
                            parms.Add(node);

                            i += node.Size - 1;
                        }

                        if (debugWrite) Console.WriteLine($"- Param {i}: {node?.Size} ");
                    }

                    newNode = FunctionNode.CreateCamera(parms);
                    break;
                default:
                    parms = new List<Node>();
                    if (size > 0)
                    {
                        if (debugWrite) Console.WriteLine($"Params ({size}): ");
                        for (int i = 0; i < size; i++)
                        {
                            if (debugWrite) Console.WriteLine($"- Param {i} ");
                            var node = ReadToken(reader);
                            if (node != null)
                            {
                                parms.Add(node);

                                i += node.Size - 1;
                            }

                            if (debugWrite) Console.WriteLine($"- Param {i}: {node?.Size } ");
                        }
                    }
                    newNode = FunctionNode.CreateGeneric(t.ToString(), parms);
                    break;
            }

            if (newNode is BlockNode blockNode)
            {
                bool blockEnd = false;
                do
                {
                    var blockChild = ReadToken(reader);

                    if (blockChild is GenericFunction func && func.Name.Equals("blockEnd"))
                    {
                        blockEnd = true;
                    }
                    else
                    {
                        blockNode.ChildNodes.Add(blockChild);
                    }
                }
                while (!blockEnd);
            }

            return newNode;
        }
    }
}
