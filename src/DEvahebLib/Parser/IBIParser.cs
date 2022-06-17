using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Nodes;

namespace DEvahebLib.Parser
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
            return ReadToken(reader, IBIToken.UNKNOWN);
        }

        protected Node ReadToken(BinaryReader reader, IBIToken parent)
        {
            //if (reader.ReadByte() == 0)
            //{
            //    var i = reader.ReadInt16();
            //    reader.BaseStream.Seek(-2, SeekOrigin.Current);

            //    var empty = reader.ReadBytes(2);
            //    if (debugWrite) Console.WriteLine($"{{ {empty[0].ToString()} {empty[1].ToString()} : {i}}}");

            //    return new ReturnFlagNode(empty[0], empty[1]);
            //}

            //reader.BaseStream.Seek(-1, SeekOrigin.Current);
            var b = reader.ReadInt32();

            IBIToken t = Enum.IsDefined(typeof(IBIToken), (Int32)b) ? (IBIToken)b : IBIToken.UNKNOWN;

            var size = reader.ReadInt32();

            if (debugWrite) Console.WriteLine($"{b} ({t.ToString()}) : {size}");

            //if (b > 7)
            if (parent == IBIToken.UNKNOWN)
            {
                b = reader.ReadByte(); // TODO functions have 1 extra byte here? values do not?

                if (b != 0)
                    Console.WriteLine($"{t} flags: {b}");
            }
            else if (b > 7)
            {
                var bts = reader.ReadBytes(size);
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
                    //((StringValue)newNode).String = ((StringValue)newNode).String.Substring(0, ((StringValue)newNode).String.Length - 1);
                    break;
                case IBIToken.Float:
                    newNode = ValueNode.Create(reader.ReadSingle());
                    break;
                case IBIToken.Vector:
                    //if (!(ReadToken(reader) is ReturnFlagNode)) throw new Exception("Vector expected 0 token");
                    //bts = reader.ReadBytes(size);

                    newNode = ValueNode.CreateVector(x: (float)((FloatValue)ReadToken(reader, t)).Float, y: (float)((FloatValue)ReadToken(reader, t)).Float, z: (float)((FloatValue)ReadToken(reader, t)).Float);
                    break;

                case IBIToken.Gt:
                case IBIToken.Lt:
                case IBIToken.Eq:
                case IBIToken.Ne:
                    //if (!(ReadToken(reader) is ReturnFlagNode)) throw new Exception("operator expected 0 token");
                    //bts = reader.ReadBytes(size);

                    newNode = If.CreateOperator((Operators)t);
                    break;

                case IBIToken.@if:
                    newNode = BlockNode.CreateIf(ReadToken(reader, t), (OperatorNode)ReadToken(reader, t), ReadToken(reader, t));
                    break;
                case IBIToken.@else:
                    newNode = BlockNode.CreateElse();
                    break;
                case IBIToken.random:
                    //if (!(ReadToken(reader) is ReturnFlagNode)) throw new Exception("random expected 0 token");
                    //bts = reader.ReadBytes(size);

                    newNode = FunctionNode.CreateRandom(ReadToken(reader, t), ReadToken(reader, t));
                    break;
                case IBIToken.task:
                    newNode = BlockNode.CreateTask(name: ReadToken(reader, t));
                    break;
                case IBIToken.affect:
                    newNode = BlockNode.CreateAffect(name: ReadToken(reader, t), type: (Nodes.AffectType)(float)((FloatValue)ReadToken(reader, t)).Float);
                    break;
                case IBIToken.loop:
                    newNode = BlockNode.CreateLoop(count: (int)((FloatValue)ReadToken(reader, t)).Float);
                    break;

                case IBIToken.tag:
                    //if (!(ReadToken(reader) is ReturnFlagNode)) throw new Exception("tag expected 0 token");
                    //bts = reader.ReadBytes(size);

                    newNode = FunctionNode.CreateTag(name: ReadToken(reader, t), type: (Nodes.TagType)(float)((FloatValue)ReadToken(reader, t)).Float);
                    break;
                case IBIToken.get:
                    //var rflag = ReadToken(reader);
                    //if (!(rflag is ReturnFlagNode)) throw new Exception("get expected 0 token");
                    //bts = reader.ReadBytes(size);

                    newNode = FunctionNode.CreateGet(type: (Nodes.ValueType)(float)((FloatValue)ReadToken(reader, t)).Float, variableName: ReadToken(reader, t));
                    break;
                case IBIToken.camera:
                    parms = new List<Node>();
                    for (int i = 0; i < size; i++)
                    {
                        if (debugWrite) Console.WriteLine($"- Param {i} ");
                        var node = ReadToken(reader, t);
                        if (node != null)
                        {
                            parms.Add(node);

                            i += node.Size - 1;
                        }

                        if (debugWrite) Console.WriteLine($"- Param {i}: {node?.Size} ");
                    }

                    newNode = FunctionNode.CreateCamera(parms);
                    break;
                case IBIToken.declare:
                    newNode = FunctionNode.CreateDeclare(type: (Nodes.ValueType)(float)((FloatValue)ReadToken(reader, t)).Float, variableName: ReadToken(reader, t));
                    break;
                case IBIToken.sound:
                    newNode = FunctionNode.CreateSound(channel: ((StringValue)ReadToken(reader, t)).String, filename: ReadToken(reader, t));
                    break;

                default:
                    parms = new List<Node>();

                    if (size > 0)
                    {
                        if (debugWrite) Console.WriteLine($"Params ({size}): ");
                        for (int i = 0; i < size; i++)
                        {
                            if (debugWrite) Console.WriteLine($"- Param {i} ");
                            var node = ReadToken(reader, t);

                            parms.Add(node);

                            i += node.Size - 1;

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
