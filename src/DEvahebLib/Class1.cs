using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DEvahebLib.Nodes;
using DEvahebLib.Parser;

namespace DEvahebLib
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

    //public enum Types
    //{
    //    CHAR = 3,
    //    STRING = 4,
    //    INTEGER = 5,
    //    FLOAT = 6,
    //    IDENTIFIER = 7,

    //    VECTOR = 14,

    //    //Wait types
    //    WAIT_COMPLETE = 51,
    //    WAIT_TRIGGERED = 52,

    //    //Set types
    //    ANGLES = 53,
    //    ORIGIN = 54,

    //    //Affect types
    //    INSERT = 55,
    //    FLUSH = 56,

    //    //Camera types
    //    PAN = 57,
    //    ZOOM = 58,
    //    MOVE = 59,
    //    FADE = 60,
    //    PATH = 61,
    //    ENABLE = 62,
    //    DISABLE = 63,
    //    SHAKE = 64,
    //    ROLL = 65,
    //    TRACK = 66,
    //    DISTANCE = 67,
    //    FOLLOW = 68,

    //    //Variable type
    //    VARIABLE = 69
    //}

    public class IBIReader
    {
        //public string Read(string filename)
        //{
        //    StringBuilder output = new StringBuilder();

        //    try
        //    {
        //        List<Node> nodes = new List<Node>();

        //        using (var file = new FileStream(filename, FileMode.Open))
        //        {
        //            using (var reader = new BinaryReader(file))
        //            {
        //                var header = reader.ReadChars(3);

        //                if (new string(header) != "IBI")
        //                    throw new Exception($"File {filename} is not a valid IBI file");

        //                reader.ReadByte(); // IBI string terminating
        //                Console.WriteLine($"IBI File Version: {reader.ReadSingle()}");

        //                var parser = new IBIParser();
        //                while (reader != null && reader.BaseStream.Position < reader.BaseStream.Length)
        //                {
        //                    var node = parser.ReadIBIBlock(reader);
        //                    nodes.Add(node);
                            
        //                    output.Append(node.ToString());

        //                    if (!(node is BlockNode))
        //                    {
        //                        output.AppendLine(";");
        //                    }
        //                    else
        //                    {
        //                        output.AppendLine();
        //                    }
        //                }
        //            }
        //        }

        //        var icarusText = new Visitors.GenerateIcarus() { Parity = Visitors.SourceCodeParity.BehavED };
        //        icarusText.Visit(nodes);
        //        Console.WriteLine(icarusText.SourceCode.ToString());
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine(ex.ToString());
        //    }

        //    return output.ToString();
        //}

        //private static bool EndOfFile(BinaryReader reader)
        //{
        //    bool eof = reader == null || reader.BaseStream.Position >= reader.BaseStream.Length;

        //    return eof;
        //}
    }
}