using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Nodes
{
    public class CharValue : ValueNode
    {
        public char? Char { get { return (char?)Value; } set { Value = value; } }

        public override int Size => 1;

        internal protected CharValue()
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + Char?.ToString() ?? "0";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class StringValue : ValueNode
    {
        public string? String { get { return (string?)Value; } set { Value = value; } }

        public override int Size => 1;

        internal protected StringValue()
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + String != null ? $"\"{String.ToString()}\"" : "\"\"";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class IntegerValue : ValueNode
    {
        public Int16? Integer { get { return (Int16?)Value; } set { Value = value; } }

        public override int Size => 1;

        internal protected IntegerValue()
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + Integer?.ToString() ?? "0";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class FloatValue : ValueNode
    {
        public float? Float { get { return (float?)Value; } set { Value = value; } }

        public override int Size => 1;

        internal protected FloatValue()
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + Float?.ToString("0.000") ?? "0.000";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class IdentifierValue : FloatValue
    {
        public string? IdentifierName { get { return ((float?)Value)?.ToString(); } set { Value = 0; /* TODO */ } }

        internal protected IdentifierValue()
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + IdentifierName?.ToString() ?? "";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class VectorValue : ValueNode
    {
        public Tuple<float, float, float> Vector { get { return (Tuple<float, float, float>)Value; } set { Value = value; } }

        public override int Size => 4;

        internal protected VectorValue()
            : this(0.0f, 0.0f, 0.0f)
        {
        }

        internal protected VectorValue(float x, float y, float z)
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + Vector != null ? $"< {Vector.Item1.ToString("0.000")} {Vector.Item2.ToString("0.000")} {Vector.Item3.ToString("0.000")} >" : "< 0.000 0.000 0.000 >";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }
}
