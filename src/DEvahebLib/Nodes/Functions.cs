using System;
using System.Collections.Generic;
using System.Text;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class Tag : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { TagName, TagType };

        public Node TagName { get; set; }

        public Node TagType { get; set; }

        public Tag()
            : this(new StringValue(""), Enums.TagType.ORIGIN)
        {
        }

        public Tag(Node tagName, TagType tagType)
            : this(tagName, new FloatValue((int)tagType))
        {

        }

        public Tag(Node tagName, Node tagType)
            : base(name: "tag")
        {
            if (tagName == null | tagType == null)
                throw new Exception("Arguments for 'tag' function cannot be null");

            TagName = tagName;
            TagType = tagType;
        }
    }

    public class Get : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Type, VariableName };

        public Node Type { get; set; }

        public Node VariableName { get; set; }

        public Get()
            : this(Enums.DECLARE_TYPE.FLOAT, new StringValue(""))
        {
        }

        public Get(DECLARE_TYPE type, Node nameExpression)
            : this(new FloatValue((int)type), nameExpression)
        {
        }

        public Get(Node type, Node variableName)
            : base(name: "get")
        {
            if (type == null || variableName == null)
                throw new Exception("Arguments for 'get' function cannot be null");

            Type = type;
            VariableName = variableName;
        }
    }

    public class Set : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { VariableName, Value };

        public Node Value { get; set; }

        public Node VariableName { get; set; }

        public Set(Node nameExpression, Node valueExpression)
            : base(name: "set")
        {
            if (nameExpression == null || valueExpression == null)
                throw new Exception("Arguments for 'set' function cannot be null");

            VariableName = nameExpression;
            Value = valueExpression;
        }
    }

    public class Random : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Min, Max };

        public Node Min { get; set; }

        public Node Max { get; set; }

        public Random()
            : this(new FloatValue(0.0f), new FloatValue(0.0f))
        {
        }

        public Random(Node min, Node max)
            : base("random")
        {
            if (min == null || max == null)
                throw new Exception("Arguments for 'random' function cannot be null");

            Min = min;
            Max = max;
        }
    }

    public class Declare : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Type, VariableName };

        public Node Type { get; set; }

        public Node VariableName { get; set; }

        public Declare()
            : this(Enums.DECLARE_TYPE.FLOAT, new StringValue(""))
        {
        }

        public Declare(Enums.DECLARE_TYPE type, Node nameExpression)
            : this(new FloatValue((int)type), nameExpression)
        {
        }

        public Declare(Node type, Node variableName)
            : base("declare")
        {
            if (type == null || variableName == null)
                throw new Exception("Arguments for 'declare' function cannot be null");

            Type = type;
            VariableName = variableName;
        }
    }

    public class Sound : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Channel, Filename };

        public Node Channel { get; set; }

        public Node Filename { get; set; }

        public Sound()
            : this(CHANNELS.CHAN_AUTO, new StringValue(""))
        {
        }

        public Sound(CHANNELS channel, Node filename)
            : this(new FloatValue((int)channel), filename)
        {

        }

        public Sound(Node channel, Node filename)
            : base("sound")
        {
            if (channel == null || filename == null)
                throw new Exception("Arguments for 'sound' function cannot be null");

            Channel = channel;
            Filename = filename;
        }
    }

    public class Play : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { PlayType, Filename };

        public Node PlayType { get; set; }

        public Node Filename { get; set; }

        public Play()
            : this(PLAY_TYPES.PLAY_ROFF, new StringValue(""))
        {
        }

        public Play(PLAY_TYPES playType, Node filename)
            : this(EnumValue.CreateOrPassThrough(new FloatValue((int)playType), typeof(PLAY_TYPES)), filename)
        {

        }

        public Play(Node playType, Node filename)
            : base("play")
        {
            if (playType == null || filename == null)
                throw new Exception("Arguments for 'play' function cannot be null");

            PlayType = playType;
            Filename = filename;
        }
    }

    // TODO functions that still use GenericFunction:
    // use
    // blockend
    // print
    // flush
    // rotate
    // waitsignal
    // move
    // remove
    // free
    // signal
    // do
    // run
    // kill
    // wait
}
