using System.Collections.Generic;
using System.Text;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class Camera : FunctionNode
    {
        List<Node> arguments;

        public override IEnumerable<Node> Arguments => arguments;

        public Camera(List<Node> arguments)
            : base(name: "camera")
        {
            this.arguments = arguments ?? new List<Node>();
        }

        public override string ToString(string indent)
        {
            var text = new StringBuilder();

            text.Append($"{indent}camera ( ");
            for (int i = 0; i < arguments.Count; i++)
            {
                if (i == 0)
                {
                    if (arguments[i] is FloatValue f)
                    {
                        text.Append(((CameraType)f.Float).ToString());
                    }
                    else
                    {
                        text.Append(arguments[i].ToString());
                    }
                }
                else
                {
                    text.Append(",");
                }

                text.Append(arguments[i].ToString());
            }
            text.Append($" )");

            return text.ToString();
        }
    }

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

        public override string ToString(string indent)
        {
            return $"{indent}$tag( {TagName.ToString()}, {(TagType is FloatValue ? (TagType)((FloatValue)TagType).Float : TagType.ToString())})$";
        }
    }

    public class Get : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Type, VariableName };

        public Node Type { get; set; }

        public Node VariableName { get; set; }

        public Get()
            : this(Enums.Type.FLOAT, new StringValue(""))
        {
        }

        public Get(Type type, Node nameExpression)
            : this(new FloatValue((int)type), nameExpression)
        {
        }

        public Get(Node type, Node variableName)
            : base(name: "get")
        {
            Type = type;
            VariableName = variableName;
        }

        public override string ToString(string indent)
        {
            return $"{indent}$get( {(Type is FloatValue ? (Enums.Type)((FloatValue)Type).Float : Type.ToString())}, {VariableName.ToString()})$";
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

        public override string ToString(string indent)
        {
            return $"{indent}$random( {Min.ToString()}, {Max.ToString()} )$";
        }
    }

    public class Declare : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Type, VariableName };

        public Node Type { get; set; }

        public Node VariableName { get; set; }

        public Declare()
            : this(Enums.Type.FLOAT, new StringValue(""))
        {
        }

        public Declare(Enums.Type type, Node nameExpression)
            : this(new FloatValue((int)type), nameExpression)
        {
        }

        public Declare(Node type, Node variableName)
            : base("declare")
        {
            Type = type;
            VariableName = variableName;
        }

        public override string ToString(string indent)
        {
            return $"{indent}declare ( {(Type is FloatValue ? (Enums.Type)((FloatValue)Type).Float : VariableName.ToString())}, {VariableName.ToString()} )";
        }
    }

    public class Sound : FunctionNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Channel, Filename };

        public Node Channel { get; set; }

        public Node Filename { get; set; }

        public Sound()
            : this(SoundChannel.CHAN_AUTO, new StringValue(""))
        {
        }

        public Sound(SoundChannel channel, Node filename)
            : this(new FloatValue((int)channel), filename)
        {

        }

        public Sound(Node channel, Node filename)
            : base("sound")
        {
            Channel = channel;
            Filename = filename;
        }

        public override string ToString(string indent)
        {
            // TODO this comes from IBI as string, not float
            return $"{indent}sound ( {(Channel is FloatValue ? (SoundChannel)((FloatValue)Channel).Float : Channel.ToString())}, {Filename.ToString()} )";
        }
    }
}
