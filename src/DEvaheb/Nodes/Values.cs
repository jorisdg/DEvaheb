using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvaheb.Nodes
{
    public class CharValue : ValueNode
    {
        public char? Char { get { return (char?)Value; } set { Value = value; } }

        internal protected CharValue()
            : base()
        {
        }

        public override string ToString()
        {
            return Char?.ToString() ?? "0";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class StringValue : ValueNode
    {
        public string? String { get { return (string?)Value; } set { Value = value; } }

        internal protected StringValue()
            : base()
        {
        }

        public override string ToString()
        {
            return String != null ? $"\"{String.ToString()}\"" : "\"\"";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class IntegerValue : ValueNode
    {
        public Int16? Integer { get { return (Int16?)Value; } set { Value = value; } }

        internal protected IntegerValue()
            : base()
        {
        }

        public override string ToString()
        {
            return Integer?.ToString() ?? "0";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class FloatValue : ValueNode
    {
        public float? Float { get { return (float?)Value; } set { Value = value; } }

        internal protected FloatValue()
            : base()
        {
        }

        public override string ToString()
        {
            return Float?.ToString() ?? "0.000";
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

        public override string ToString()
        {
            return IdentifierName?.ToString() ?? "0.000";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    //public class VectorValue : ValueNode
    //{
    //    public float[] Float { get { return (float[])Value; } set { Value = value; } }

    //    internal protected VectorValue()
    //        : base()
    //    {
    //    }

    //    public override string ToString()
    //    {
    //        return (Float != null && Float.Length == 3)
    //            ? $"< {Float[0].ToString("0.000")} {Float[1].ToString("0.000")} {Float[2].ToString("0.000")} >"
    //            : "< 0.000 0.000 0.000 >";
    //    }

    //    public override byte[] ToBinary()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
