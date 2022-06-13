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
        public Node ReadToken(BinaryReader reader)
        {
            var b = reader.ReadByte();
            if (b == 0)
            {
                var i = reader.ReadInt16();
                reader.BaseStream.Seek(-2, SeekOrigin.Current);

                var empty = reader.ReadBytes(2);
                //Output($"{{ {empty[0].ToString()} {empty[1].ToString()} : {i}}}");
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

                    parms = new List<Node>();
                    parms.Add(ReadToken(reader));
                    parms.Add(ReadToken(reader));
                    parms.Add(ReadToken(reader));

                    newNode = FunctionNode.CreateGeneric("vector", parms);
                    
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
                case IBIToken.get:
                    if (ReadToken(reader) != null) throw new Exception("tag/vector expected 0 token");

                    parms = new List<Node>();
                    parms.Add(ReadToken(reader));
                    parms.Add(ReadToken(reader));

                    newNode = FunctionNode.CreateGeneric(t.ToString(), parms);
                    break;
                //case IBIToken.camera:
                //    Output($"{t.ToString()} ( ");
                //    ReadSubTokens(reader, size);
                //    Output(")");
                //    if (root) OutputLine(";");
                //    break;
                default:
                    parms = new List<Node>();
                    if (size > 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            var node = ReadToken(reader);
                            if (node != null)
                            {
                                parms.Add(node);

                                if (node is FunctionNode func)
                                {
                                    i += func.RecursiveArgumentCount;
                                }
                            }
                        }
                    }
                    newNode = FunctionNode.CreateGeneric(t.ToString(), parms);
                    break;
            }

            return newNode;
        }
    }
}
