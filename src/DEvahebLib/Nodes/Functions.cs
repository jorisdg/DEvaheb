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
            Type = type;
            VariableName = variableName;
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
            Channel = channel;
            Filename = filename;
        }
    }
}
