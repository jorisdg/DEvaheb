using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class CharValue : ValueNode
    {
        public char? Char { get { return (char?)Value; } set { Value = value; } }

        public override int Size => 1;

        public CharValue()
            : this('\0')
        {
        }

        public CharValue(char character)
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + Char?.ToString() ?? "0";
        }
    }

    public class StringValue : ValueNode
    {
        public string String { get { return (string)Value; } set { Value = value; } }

        public override int Size => 1;

        public StringValue()
            : this(string.Empty)
        {
        }

        public StringValue(string value)
            : base()
        {
            String = value;
        }

        public override string ToString(string indent)
        {
            return indent + String != null ? $"\"{String.ToString()}\"" : "\"\"";
        }
    }

    public class IntegerValue : ValueNode
    {
        public Int32? Integer { get { return (Int32?)Value; } set { Value = value; } }

        public override int Size => 1;

        public IntegerValue()
            : this(0)
        {
        }

        public IntegerValue(Int32 integer)
            : base()
        {
            Integer = integer;
        }

        public override string ToString(string indent)
        {
            return indent + Integer?.ToString() ?? "0";
        }
    }

    public class FloatValue : ValueNode
    {
        public float? Float { get { return (float?)Value; } set { Value = value; } }

        public override int Size => 1;

        public FloatValue()
            : this(0.0f)
        {
        }

        public FloatValue(float f)
            : base()
        {
            Float = f;
        }

        public override string ToString(string indent)
        {
            return indent + Float?.ToString("0.000") ?? "0.000";
        }
    }

    // TODO: identifier
    public class IdentifierValue : FloatValue
    {
        public string? IdentifierName { get { return ((float?)Value)?.ToString(); } set { Value = 0; } }

        public IdentifierValue()
            : base()
        {
        }

        public override string ToString(string indent)
        {
            return indent + IdentifierName?.ToString() ?? "";
        }
    }

    public class VectorValue : ValueNode
    {
        public override int Size
        {
            get
            {
                int count = 1; // count ourselves

                foreach (var node in Values)
                {
                    count += node.Size;
                }

                return count;
            }
        }

        public Node[] Values { get; protected set; }

        public VectorValue()
            : this(x: new FloatValue(0.0f), y: new FloatValue(0.0f), z: new FloatValue(0.0f))
        {
        }

        public VectorValue(Node x, Node y, Node z)
            : base()
        {
            Values = new Node[3] { x, y, z};
        }

        public override string ToString(string indent)
        {
            return indent + $"< {(Values[0] != null ? Values[0].ToString() : "0.000")} {(Values[1] != null ? Values[1].ToString() : "0.000")} {(Values[2] != null ? Values[2].ToString() : "0.000")} >";
        }
    }

    public class OperatorNode : ValueNode
    {
        public Operator Operator {  get { return (Operator)Value; } set { Value = value; } }

        public override int Size => 1;

        public OperatorNode()
            : this(Operator.Eq)
        {
        }

        public OperatorNode(Operator op)
            : base()
        {
            Operator = op;
        }

        public override string ToString(string indent)
        {
            string op = string.Empty;

            switch (Operator)
            {
                case Operator.Gt:
                    op = ">";
                    break;
                case Operator.Lt:
                    op = "<";
                    break;
                case Operator.Eq:
                    op = "=";
                    break;
                case Operator.Ne:
                    op = "!";
                    break;
            }

            return $"{indent}${op}$";
        }
    }
}
