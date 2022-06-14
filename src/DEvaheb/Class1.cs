using DEvaheb.Parser;

namespace DEvaheb
{
    //https://jkhub.org/forums/topic/10085-icarus-scripting/

    public enum Token
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
        rem =  40, // Icarus
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

    public enum Types // Icarus
    {
        CHAR = 3,
        STRING = 4,
        INTEGER = 5,
        FLOAT = 6,
        IDENTIFIER = 7,

        VECTOR = 14,

        //Wait types
        WAIT_COMPLETE = 51,
        WAIT_TRIGGERED = 52,

        //Set types
        ANGLES = 53,
        ORIGIN = 54,

        //Affect types
        INSERT = 55,
        FLUSH = 56,

        //Camera types
        PAN = 57,
        ZOOM = 58,
        MOVE = 59,
        FADE = 60,
        PATH = 61,
        ENABLE = 62,
        DISABLE = 63,
        SHAKE = 64,
        ROLL = 65,
        TRACK = 66,
        DISTANCE = 67,
        FOLLOW = 68,

        //Variable type
        VARIABLE = 69
    }

    public class IBIReader
    {
        string currentIndent = string.Empty;

        private int ReadToken(BinaryReader reader, bool root = false)
        {
            int numCommandsRead = 0;

            var b = reader.ReadByte();
            if (b == 0)
            {
                var i = reader.ReadInt16();
                reader.BaseStream.Seek(-2, SeekOrigin.Current);

                var empty = reader.ReadBytes(2);
                //Output($"{{ {empty[0].ToString()} {empty[1].ToString()} : {i}}}");
                return -1; // TODO don't count this as a token in a loop
            }

            Token t = Enum.IsDefined(typeof(Token), (Int32)b) ? (Token)b : Token.UNKNOWN;

            var bs = reader.ReadBytes(3);
            if (bs[0] != 0 || bs[1] != 0 || bs[2] != 0)
                File.AppendAllText("log.txt", $"Token {b} ({t.ToString()} 1st 3 bytes: {bs[0]}, {bs[1]}, {bs[2]}\r\n");
                
            var size = reader.ReadByte();
            bs = reader.ReadBytes(3);
            if (bs[0] != 0 || bs[1] != 0 || bs[2] != 0)
                File.AppendAllText("log.txt", $"Token {b} ({t.ToString()} 2nd 3 bytes: {bs[0]}, {bs[1]}, {bs[2]}\r\n");

            if (b > 7)
            {
                b = reader.ReadByte(); // TODO functions have 1 extra byte here? values not?
                numCommandsRead = size;
            }


            switch (t)
            {
                case Token.String:
                    Output($"\"{new string(reader.ReadChars(size))}\"");
                    break;
                case Token.Identifier:
                    Output($"{new string(reader.ReadChars(size))}");
                    break;
                case Token.Float:
                    Output($"{reader.ReadSingle().ToString("0.000")}");
                    break;
                case Token.Vector:
                    Output($"<");
                    ReadSubTokens(reader, 3);
                    Output(">");
                    if (root) OutputLine(";");
                    numCommandsRead = 3;
                    break;
                case Token.affect:
                case Token.task:
                    Output($"{t.ToString()} (");
                    ReadSubTokens(reader, size);
                    OutputLine(")");
                    OutputLine("{");
                    currentIndent += "  ";
                    break;
                case Token.blockEnd:
                    currentIndent = currentIndent.Length > 2 ? currentIndent.Substring(2) : string.Empty;
                    OutputLine("}");
                    break;
                case Token.tag:
                case Token.get:
                    Output($"${t.ToString()}(");
                    ReadSubTokens(reader, 2);
                    Output(")$");
                    if (root) OutputLine(";");
                    numCommandsRead = 2;
                    break;
                case Token.camera:
                    Output($"{t.ToString()} ( ");
                    ReadSubTokens(reader, size);
                    Output(")");
                    if (root) OutputLine(";");
                    break;
                default:
                    Output($"{t.ToString()} (");
                    ReadSubTokens(reader, size);
                    Output(")");
                    if (root) OutputLine(";");
                    break;
            }

            return numCommandsRead;
        }

        private void ReadSubTokens(BinaryReader reader, int size)
        {
            if (size > 0)
            {
                Output(" ");
                for (int i = 0; i < size; i++)
                {
                    if (i > 0) Output(", ");
                    i += ReadToken(reader);
                }
                Output(" ");
            }
        }

        bool newLine = true;
        private void Output(string text)
        {
            if (newLine)
            {
                Console.Write(currentIndent);
                newLine = false;
            }
            Console.Write(text);
        }

        private void OutputLine(string text)
        {
            if (newLine)
            {
                Console.Write(currentIndent);
            }

            newLine = true;
            Console.WriteLine(text);
        }

        public void Read(string filename)
        {
            currentIndent = string.Empty;

            try
            {
                using (var file = new FileStream(filename, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        var header = reader.ReadChars(3);

                        if (new string(header) != "IBI")
                            throw new Exception($"File {filename} is not a valid IBI file");

                        reader.ReadBytes(5); // unknown header details

                        var parser = new IBIParser();
                        while (!EndOfFile(reader))
                        {
                            var node = parser.ReadToken(reader);
                            
                            Output(node.ToString());

                            if (!(node is Nodes.BlockNode))
                            {
                                OutputLine(";");
                            }
                            else
                            {
                                OutputLine("");
                            }

                            //ReadToken(reader, root: true);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
            }
        }

        private static bool EndOfFile(BinaryReader reader)
        {
            bool eof = reader == null || reader.BaseStream.Position >= reader.BaseStream.Length;

            return eof;
        }
    }
}