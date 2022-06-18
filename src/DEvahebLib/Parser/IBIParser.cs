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
        public Node ReadIBIBlock(BinaryReader reader)
        {
            return ReadIBIBlock(reader, IBIToken.UNKNOWN);
        }

        protected Node ReadIBIBlock(BinaryReader reader, IBIToken parent)
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
                    var identifier = new string(reader.ReadChars(size));
                    identifier = identifier.Substring(0, identifier.Length - 1);
                    newNode = new IdentifierValue(identifier);
                    break;
                case IBIToken.Float:
                    newNode = new FloatValue(reader.ReadSingle());
                    break;
                case IBIToken.Vector:
                    newNode = new VectorValue(x: ReadIBIBlock(reader, t), y: ReadIBIBlock(reader, t), z: ReadIBIBlock(reader, t));
                    break;

                case IBIToken.Gt:
                case IBIToken.Lt:
                case IBIToken.Eq:
                case IBIToken.Ne:
                    newNode = new OperatorNode((Operator)t);
                    break;

                case IBIToken.@if:
                    newNode = new If(expression1: ReadIBIBlock(reader, t), operatorNode: (OperatorNode)ReadIBIBlock(reader, t), expression2: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.@else:
                    newNode = new Else();
                    break;
                case IBIToken.random:
                    newNode = new Nodes.Random(min: ReadIBIBlock(reader, t), max: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.task:
                    newNode = new Task(name: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.affect:
                    newNode = new Affect(name: ReadIBIBlock(reader, t), type: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(AFFECT_TYPE)));
                    break;
                case IBIToken.loop:
                    newNode = new Loop(count: ReadIBIBlock(reader, t));
                    break;

                case IBIToken.tag:
                    newNode = new Tag(tagName: ReadIBIBlock(reader, t), tagType: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), enumType: typeof(TagType)));
                    break;
                case IBIToken.get:
                    newNode = new Get(type: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(Enums.DECLARE_TYPE)), variableName: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.camera:
                    parms = new List<Node>();

                    parms.Add(EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(CAMERA_COMMANDS)));

                    for (int i = 1; i < size; i++)
                    {
                        var node = ReadIBIBlock(reader, t);
                        if (node != null)
                        {
                            parms.Add(node);

                            i += node.Size - 1;
                        }
                    }

                    newNode = new Camera(parms);
                    break;
                case IBIToken.declare:
                    newNode = new Declare(type: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(Enums.DECLARE_TYPE)), variableName: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.sound:
                    newNode = new Sound(channel: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(Enums.CHANNELS)), filename: ReadIBIBlock(reader, t));
                    break;

                default:
                    parms = new List<Node>();

                    if (size > 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            var node = ReadIBIBlock(reader, t);

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
                    var blockChild = ReadIBIBlock(reader);

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
