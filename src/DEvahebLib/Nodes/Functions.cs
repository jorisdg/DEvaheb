using System;
using System.Collections.Generic;
using System.Text;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class Tag : FunctionNode
    {
        public Node TagName { get => GetArg(0); set => SetArg(0, value); }

        public Node TagType { get => GetArg(1); set => SetArg(1, value); }

        public override Type ValueType => typeof(VectorValue);

        public override int ExpectedArgCount => 2;

        public Tag()
            : this(new StringValue(""), Enums.TagType.ORIGIN)
        {
        }

        public Tag(Node tagName, TagType tagType)
            : this(tagName, new FloatValue((int)tagType))
        {
        }

        public Tag(params Node[] args) : base("tag", args)
        {
        }
    }

    public class Get : FunctionNode
    {
        public Node Type { get => GetArg(0); set => SetArg(0, value); }

        public Node VariableName { get => GetArg(1); set => SetArg(1, value); }

        public override Type ValueType
        {
            get
            {
                if (Type is EnumFloatValue ef && ef.Name == nameof(Enums.DECLARE_TYPE))
                {
                    switch ((Enums.DECLARE_TYPE)ef.Float)
                    {
                        case Enums.DECLARE_TYPE.FLOAT: return typeof(FloatValue);
                        case Enums.DECLARE_TYPE.STRING: return typeof(StringValue);
                        case Enums.DECLARE_TYPE.VECTOR: return typeof(VectorValue);
                    }
                }
                return typeof(FloatValue);
            }
        }

        public override int ExpectedArgCount => 2;

        public Get()
            : this(Enums.DECLARE_TYPE.FLOAT, new StringValue(""))
        {
        }

        public Get(DECLARE_TYPE type, Node nameExpression)
            : this(new FloatValue((int)type), nameExpression)
        {
        }

        public Get(params Node[] args) : base("get", args)
        {
        }
    }

    public class Set : FunctionNode
    {
        public Node VariableName { get => GetArg(0); set => SetArg(0, value); }

        public Node Value { get => GetArg(1); set => SetArg(1, value); }

        public override int ExpectedArgCount => 2;

        public Set(params Node[] args) : base("set", args)
        {
        }
    }

    public class Random : FunctionNode
    {
        public Node Min { get => GetArg(0); set => SetArg(0, value); }

        public Node Max { get => GetArg(1); set => SetArg(1, value); }

        public override Type ValueType => typeof(FloatValue);

        public override int ExpectedArgCount => 2;

        public Random()
            : this(new FloatValue(0.0f), new FloatValue(0.0f))
        {
        }

        public Random(params Node[] args) : base("random", args)
        {
        }
    }

    public class Declare : FunctionNode
    {
        public Node Type { get => GetArg(0); set => SetArg(0, value); }

        public Node VariableName { get => GetArg(1); set => SetArg(1, value); }

        public override int ExpectedArgCount => 2;

        public Declare()
            : this(Enums.DECLARE_TYPE.FLOAT, new StringValue(""))
        {
        }

        public Declare(Enums.DECLARE_TYPE type, Node nameExpression)
            : this(new FloatValue((int)type), nameExpression)
        {
        }

        public Declare(params Node[] args) : base("declare", args)
        {
        }
    }

    public class Sound : FunctionNode
    {
        public Node Channel { get => GetArg(0); set => SetArg(0, value); }

        public Node Filename { get => GetArg(1); set => SetArg(1, value); }

        public override int ExpectedArgCount => 2;

        public Sound()
            : this(CHANNELS.CHAN_AUTO, new StringValue(""))
        {
        }

        public Sound(CHANNELS channel, Node filename)
            : this(new FloatValue((int)channel), filename)
        {
        }

        public Sound(params Node[] args) : base("sound", args)
        {
        }
    }

    public class Play : FunctionNode
    {
        public Node PlayType { get => GetArg(0); set => SetArg(0, value); }

        public Node Filename { get => GetArg(1); set => SetArg(1, value); }

        public override int ExpectedArgCount => 2;

        public Play()
            : this(PLAY_TYPES.PLAY_ROFF, new StringValue(""))
        {
        }

        public Play(PLAY_TYPES playType, Node filename)
            : this(EnumValue.CreateOrPassThrough(new FloatValue((int)playType), typeof(PLAY_TYPES)), filename)
        {
        }

        public Play(params Node[] args) : base("play", args)
        {
        }
    }

    public class Use : FunctionNode
    {
        public Node Target { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Use(params Node[] args) : base("use", args)
        {
        }
    }

    public class Print : FunctionNode
    {
        public Node Message { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Print(params Node[] args) : base("print", args)
        {
        }
    }

    public class Flush : FunctionNode
    {
        public override int ExpectedArgCount => 0;

        public Flush(params Node[] args) : base("flush", args)
        {
        }
    }

    public class Rotate : FunctionNode
    {
        public Node Angles { get => GetArg(0); set => SetArg(0, value); }

        public Node Duration { get => GetArg(1); set => SetArg(1, value); }

        public override int ExpectedArgCount => 2;

        public Rotate(params Node[] args) : base("rotate", args)
        {
        }
    }

    public class WaitSignal : FunctionNode
    {
        public Node SignalName { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public WaitSignal(params Node[] args) : base("waitsignal", args)
        {
        }
    }

    public class Move : FunctionNode
    {
        public Node Origin { get => arguments.Count >= 3 ? GetArg(0) : null; set { if (arguments.Count >= 3) SetArg(0, value); } }

        public Node Destination { get => GetArg(arguments.Count >= 3 ? 1 : 0); set => SetArg(arguments.Count >= 3 ? 1 : 0, value); }

        public Node Duration { get => GetArg(arguments.Count >= 3 ? 2 : 1); set => SetArg(arguments.Count >= 3 ? 2 : 1, value); }

        public override int ExpectedArgCount => -1;

        public Move(params Node[] args) : base("move", args)
        {
        }
    }

    public class Remove : FunctionNode
    {
        public Node Target { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Remove(params Node[] args) : base("remove", args)
        {
        }
    }

    public class Free : FunctionNode
    {
        public Node Target { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Free(params Node[] args) : base("free", args)
        {
        }
    }

    public class Signal : FunctionNode
    {
        public Node SignalName { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Signal(params Node[] args) : base("signal", args)
        {
        }
    }

    public class Do : FunctionNode
    {
        public Node Target { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Do(params Node[] args) : base("do", args)
        {
        }
    }

    public class Run : FunctionNode
    {
        public Node Filename { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Run(params Node[] args) : base("run", args)
        {
        }
    }

    public class Kill : FunctionNode
    {
        public Node Target { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Kill(params Node[] args) : base("kill", args)
        {
        }
    }

    public class Wait : FunctionNode
    {
        public Node Duration { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Wait(params Node[] args) : base("wait", args)
        {
        }
    }

    public class Rem : FunctionNode
    {
        public Node Comment { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Rem(params Node[] args) : base("rem", args)
        {
        }
    }

    public class DoWait : FunctionNode
    {
        public Node WaitName { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public DoWait(params Node[] args) : base("dowait", args)
        {
        }
    }

    public class BlockEnd : FunctionNode
    {
        public BlockEnd(params Node[] args) : base("blockEnd", args)
        {
        }
    }
}
