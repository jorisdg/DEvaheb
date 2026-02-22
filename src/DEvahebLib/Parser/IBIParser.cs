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
        Char = 3,
        String = 4,
        Int = 5,
        Float = 6,
        Identifier = 7,

        TBlockStart = 8,
        TBlockEnd = 9,
        VectorStart = 10,
        VectorEnd = 11,
        OpenParenthesis = 12,
        CloseParenthesis = 13,

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
        blockStart = 24,
        blockEnd = 25,
        set = 26,
        loop = 27,
        loopEnd = 28,
        print = 29,
        use = 30,
        flush = 31,
        run = 32,
        kill = 33,
        remove = 34,
        camera = 35,
        get = 36,
        random = 37,
        @if = 38,
        @else = 39,
        rem = 40,
        task = 41,
        @do = 42,
        declare = 43,
        free = 44,
        dowait = 45,
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

            if (parent == IBIToken.UNKNOWN) // top level block (no parent), i.e. a function
            {
                b = reader.ReadByte(); // read flags byte
                // TODO is there a reason we should store this?
            }
            else if (b > 7) // block has a parent, but it's not just a basic type value
            {
                var bts = reader.ReadBytes(size); // read data
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
                    newNode = new Get(type: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(DECLARE_TYPE)), variableName: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.camera:
                    parms = new List<Node>();

                    parms.Add(EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(CAMERA_COMMANDS)));

                    // camera has several overloads, just read all arguments like a generic function
                    for (int i = 1; i < size; i++)
                    {
                        var node = ReadIBIBlock(reader, t);
                        if (node != null)
                        {
                            parms.Add(node);

                            i += node.Size - 1;
                        }
                    }

                    // Determine overload and check the arguments
                    newNode = Camera.CreateCameraOverload(parms);
                    break;
                case IBIToken.declare:
                    newNode = new Declare(type: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(DECLARE_TYPE)), variableName: ReadIBIBlock(reader, t));
                    break;
                case IBIToken.sound:
                    newNode = new Sound(channel: EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(CHANNELS)), filename: ReadIBIBlock(reader, t));
                    break;

                case IBIToken.set:
                    newNode = new Set(ReadIBIBlock(reader, t), ReadIBIBlock(reader, t));
                    break;

                case IBIToken.play:
                    parms = new List<Node>();

                    Node playNode = EnumValue.CreateOrPassThrough(ReadIBIBlock(reader, t), typeof(PLAY_TYPES));
                    
                    newNode = new Play(playNode, ReadIBIBlock(reader, t));
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

            // if new node is a block, read subnodes but stop at blockend
            if (newNode is BlockNode blockNode)
            {
                bool blockEnd = false;
                do
                {
                    // Handle some existing game IBIs that don't have an blockEnd token for some reason and just end the file
                    if (reader.BaseStream.Position >= reader.BaseStream.Length)
                        break;

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
