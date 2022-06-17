using System;
using System.Collections.Generic;
using System.IO;
using DEvahebLib.Enums;
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
        public Node ReadToken(BinaryReader reader)
        {
            return ReadToken(reader, IBIToken.UNKNOWN);
        }

        protected Node ReadToken(BinaryReader reader, IBIToken parent)
        {
            var b = reader.ReadInt32();

            IBIToken t = Enum.IsDefined(typeof(IBIToken), (Int32)b) ? (IBIToken)b : IBIToken.UNKNOWN;

            var size = reader.ReadInt32();

            //if (b > 7)
            if (parent == IBIToken.UNKNOWN)
            {
                b = reader.ReadByte(); // TODO functions have 1 extra byte here? values do not?
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
                    var str = new string(reader.ReadChars(size));
                    str = str.Substring(0, str.Length - 1);
                    newNode = new StringValue(str);
                    break;
                case IBIToken.Identifier:
                    newNode = new StringValue(new string(reader.ReadChars(size)));
                    break;
                case IBIToken.Float:
                    newNode = new FloatValue(reader.ReadSingle());
                    break;
                case IBIToken.Vector:
                    newNode = new VectorValue(x: ReadToken(reader, t), y: ReadToken(reader, t), z: ReadToken(reader, t));
                    break;

                case IBIToken.Gt:
                case IBIToken.Lt:
                case IBIToken.Eq:
                case IBIToken.Ne:
                    newNode = new OperatorNode((Operator)t);
                    break;

                case IBIToken.@if:
                    newNode = new If(expression1: ReadToken(reader, t), operatorNode: (OperatorNode)ReadToken(reader, t), expression2: ReadToken(reader, t));
                    break;
                case IBIToken.@else:
                    newNode = new Else();
                    break;
                case IBIToken.random:
                    newNode = new Nodes.Random(min: ReadToken(reader, t), max: ReadToken(reader, t));
                    break;
                case IBIToken.task:
                    newNode = new Task(name: ReadToken(reader, t));
                    break;
                case IBIToken.affect:
                    newNode = new Affect(name: ReadToken(reader, t), type: ReadToken(reader, t));
                    break;
                case IBIToken.loop:
                    newNode = new Loop(count: ReadToken(reader, t));
                    break;

                case IBIToken.tag:
                    newNode = new Tag(tagName: ReadToken(reader, t), tagType: ReadToken(reader, t));
                    break;
                case IBIToken.get:
                    newNode = new Get(type: ReadToken(reader, t), variableName: ReadToken(reader, t));
                    break;
                case IBIToken.camera:
                    parms = new List<Node>();
                    for (int i = 0; i < size; i++)
                    {
                        var node = ReadToken(reader, t);
                        if (node != null)
                        {
                            parms.Add(node);

                            i += node.Size - 1;
                        }
                    }

                    newNode = new Camera(parms);
                    break;
                case IBIToken.declare:
                    newNode = new Declare(type: ReadToken(reader, t), variableName: ReadToken(reader, t));
                    break;
                case IBIToken.sound:
                    newNode = new Sound(channel: ReadToken(reader, t), filename: ReadToken(reader, t));
                    break;

                default:
                    parms = new List<Node>();

                    if (size > 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            var node = ReadToken(reader, t);

                            parms.Add(node);

                            i += node.Size - 1;
                        }
                    }
                    newNode = new GenericFunction(t.ToString(), parms);
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
                        blockNode.SubNodes.Add(blockChild);
                    }
                }
                while (!blockEnd);
            }

            return newNode;
        }
    }
}
