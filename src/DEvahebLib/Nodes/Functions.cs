using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Nodes
{
    public class GenericFunction : FunctionNode
    {
        protected List<Node> arguments;

        public IEnumerable<Node> Arguments => arguments;

        public override int ArgumentCount => arguments.Count;

        public override int Size
        { 
            get 
            { 
                int count = 1; // count ourselves

                foreach(var node in arguments)
                {
                    //if (node is FunctionNode func)
                    //    count += func.Size;
                    count += node.Size;
                }

                return count;
            } 
        }

        internal protected GenericFunction(string name, List<Node> arguments)
            : base()
        {
            this.Name = name;
            this.arguments = arguments ?? new List<Node>();
        }

        internal protected GenericFunction(string name)
            : this(name, new List<Node>())
        {
        }

        public override string ToString(string indent)
        {
            var text = new StringBuilder();

            text.Append($"{indent}{Name} ( ");
            if (ArgumentCount > 0)
            {
                var args = Arguments.GetEnumerator();
                if (args.MoveNext())
                    text.Append(args.Current.ToString());

                while (args.MoveNext())
                {
                    text.Append($", {args.Current.ToString()}");
                }
            }
            text.Append($" )");

            return text.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public enum CameraType
    {
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
        FOLLOW = 68
    }

    public class Camera : GenericFunction
    {
        internal protected Camera(List<Node> arguments)
            : base("camera", arguments)
        {
        }

        public override string ToString(string indent)
        {
            var text = new StringBuilder();

            text.Append($"{indent}camera ( ");
            if (ArgumentCount > 0)
            {
                var args = Arguments.GetEnumerator();
                if (args.MoveNext())
                {
                    if (args.Current is FloatValue f)
                        text.Append(((CameraType)f.Float).ToString());
                    else
                        text.Append(args.Current.ToString());
                }

                while (args.MoveNext())
                {
                    text.Append($", {args.Current.ToString()}");
                }
            }
            text.Append($" )");

            return text.ToString();
        }
    }

    public enum TagType
    {
        ANGLES = 53,
        ORIGIN = 54
    }

    public class Tag : FunctionNode
    {
        public Node TagName { get; set; }
        public TagType Type { get; set; }

        public override int ArgumentCount => 2;

        public override int Size => 2 + TagName.Size;

        internal protected Tag()
            : this(ValueNode.Create(""), TagType.ORIGIN)
        {
        }

        internal protected Tag(Node name, TagType type)
            : base()
        {
            Name = "tag";
            TagName = name;
            Type = type;
        }

        public override string ToString(string indent)
        {
            return $"{indent}$tag( {TagName.ToString()}, {Type.ToString()})$";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public enum ValueType
    {
        //CHAR = 3, // Behaved doesn't show this as an option
        STRING = 4,
        //INTEGER = 5, // Behaved doesn't show this as an option
        FLOAT = 6,
        //IDENTIFIER = 7, // Behaved doesn't show this as an option

        VECTOR = 14
    }

    public class Get : FunctionNode
    {
        public Node VariableName { get; set; }
        public ValueType Type { get; set; }

        public override int ArgumentCount => 2;

        public override int Size => 2 + VariableName.Size;

        internal protected Get()
            : this(ValueType.FLOAT, ValueNode.Create(""))
        {
        }

        internal protected Get(ValueType type, Node nameExpression)
            : base()
        {
            Name = "get";
            VariableName = nameExpression;
            Type = type;
        }

        public override string ToString(string indent)
        {
            return $"{indent}$get( {Type.ToString()}, {VariableName.ToString()})$";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class Random : FunctionNode
    {
        public Node Min { get; set; }
        public Node Max { get; set; }

        public override int ArgumentCount => 2;

        public override int Size => 1 + Min.Size + Max.Size;

        internal protected Random()
            : this(ValueNode.Create(0.0f), ValueNode.Create(0.0f))
        {
        }

        internal protected Random(Node min, Node max)
            : base()
        {
            Name = "random";
            Min = min;
            Max = max;
        }

        public override string ToString(string indent)
        {
            return $"{indent}$random( {Min.ToString()}, {Max.ToString()} )$";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class Declare : FunctionNode
    {
        public Node VariableName { get; set; }
        public ValueType Type { get; set; }

        public override int ArgumentCount => 2;

        public override int Size => 3;

        internal protected Declare()
            : this(ValueType.FLOAT, ValueNode.Create(""))
        {
        }

        internal protected Declare(ValueType type, Node nameExpression)
            : base()
        {
            Name = "declare";
            VariableName = nameExpression;
            Type = type;
        }

        public override string ToString(string indent)
        {
            return $"{indent}declare ( {Type.ToString()}, {VariableName.ToString()} )";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    //public enum SoundChannel
    //{
	   // CHAN_AUTO,
    //    CHAN_LOCAL,
    //    CHAN_WEAPON,
    //    CHAN_VOICE,
    //    CHAN_VOICE_ATTEN,
    //    CHAN_VOICE_GLOBAL,
    //    CHAN_ITEM,
    //    CHAN_BODY,
    //    CHAN_AMBIENT,
    //    CHAN_LOCAL_SOUND,
    //    CHAN_ANNOUNCER,
    //    CHAN_LESS_ATTEN,
    //    CHAN_MUSIC
    //}

    public class Sound : FunctionNode
    {
        public Node FileName { get; set; }
        public string Channel { get; set; }

        public override int ArgumentCount => 2;

        public override int Size => 3;

        internal protected Sound()
            : this("CHAN_AUTO", ValueNode.Create(""))
        {
        }

        internal protected Sound(string channel, Node filenameExpression)
            : base()
        {
            Name = "sound";
            FileName = filenameExpression;
            Channel = channel;
        }

        public override string ToString(string indent)
        {
            return $"{indent}sound ( {Channel.ToString()}, {FileName.ToString()} )";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }
}
