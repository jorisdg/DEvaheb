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
    }

    public class IdentifierValue : StringValue
    {
        public string IdentifierName { get { return (string)Value; } set { Value = value; } }

        public IdentifierValue()
            : this("")
        {
        }

        public IdentifierValue(string identifierName)
            : base(value: identifierName)
        {
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

        public override string ToString()
        {
            return $"VectorValue : < {(Values[0] != null ? Values[0].ToString() : "0.000")} {(Values[1] != null ? Values[1].ToString() : "0.000")} {(Values[2] != null ? Values[2].ToString() : "0.000")} >";
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
    }

    public abstract class EnumValue : ValueNode
    {
        public abstract string Name { get; }

        protected ValueNode valueNode = null;

        public override int Size => valueNode.Size;

        public override object Value
        {
            get
            {
                return valueNode.Value;
            }
            set
            {
                valueNode.Value = value;
            }
        }

        public abstract string Text { get; set; }

        public EnumValue(ValueNode node)
        {
            valueNode = node;
        }

        public abstract bool KnowsValue(object value);

        public override string ToString()
        {
            return $"EnumValue : {Text} = {valueNode.ToString()}";
        }

        public static Node CreateOrPassThrough(Node enumNode, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("EnumValue node has to be instantiated with an type that is an enumeration"); // TODO specific exception

            Node finalNode = null;

            if (enumNode is FloatValue f)
            {
                finalNode = new EnumFloatValue(f, EnumTableFloat.FromEnum(enumType));
            }
            else if (enumNode is IntegerValue i)
            {
                finalNode = new EnumIntValue(i, EnumTableInt.FromEnum(enumType));
            }
            else if (enumNode is IdentifierValue id) // this inherits from String so check ident first
            {
                finalNode = new EnumIdentifierValue(id, EnumTableString.FromEnum(enumType));
            }
            else if (enumNode is StringValue s)
            {
                finalNode = new EnumStringValue(s, EnumTableString.FromEnum(enumType));
            }
            else //if (enumNode is FunctionNode && !(enumNode is BlockNode))
            {
                finalNode = enumNode;
            }

            return finalNode;
        }
    }

    public class EnumFloatValue : EnumValue
    {
        EnumTableFloat enumTable;

        public override string Name => enumTable.EnumName;

        public float Float
        {
            get
            {
                return (float)valueNode.Value;
            }
            set
            {
                valueNode.Value = value;
            }
        }

        public override string Text
        {
            get
            {
                if (enumTable.HasValue(Float))
                {
                    return enumTable.GetEnum(Float);
                }
                else
                {
                    return Float.ToString("0.000");
                }
            }

            set
            {
                if (enumTable.HasEnum(value))
                {
                    Float = enumTable.GetValue(value);
                }
                // TODO else?!?
                else
                {
                    throw new Exception($"Unknown enum text {value}");
                }
            }
        }

        public EnumFloatValue(ValueNode valueNode, EnumTableFloat enumTableFloat)
            : base(valueNode)
        {
            if (valueNode.Value.GetType() != typeof(float))
                throw new Exception($"EnumFloatValue cannot use value node {valueNode.GetType().Name}");

            enumTable = enumTableFloat;
        }

        public override bool KnowsValue(object value)
        {
            return enumTable.HasValue((float)value);
        }
    }

    public class EnumIntValue : EnumValue
    {
        EnumTableInt enumTable;

        public override string Name => enumTable.EnumName;

        public Int32 Integer
        {
            get
            {
                return (Int32)valueNode.Value;
            }
            set
            {
                valueNode.Value = value;
            }
        }

        public override string Text
        {
            get
            {
                if (enumTable.HasValue(Integer))
                {
                    return enumTable.GetEnum(Integer);
                }
                else
                {
                    return Integer.ToString();
                }
            }

            set
            {
                if (enumTable.HasEnum(value))
                {
                    Integer = enumTable.GetValue(value);
                }
                // TODO else?!?
                else
                {
                    throw new Exception($"Unknown enum text {value}");
                }
            }
        }

        public EnumIntValue(ValueNode valueNode, EnumTableInt enumTableInt)
            : base(valueNode)
        {
            if (valueNode.Value.GetType() != typeof(Int32))
                throw new Exception($"EnumIntValue cannot use value node {valueNode.GetType().Name}");

            enumTable = enumTableInt;
        }

        public override bool KnowsValue(object value)
        {
            return enumTable.HasValue((Int32)value);
        }
    }

    public class EnumStringValue : EnumValue
    {
        protected EnumTableString enumTable;

        public override string Name => enumTable.EnumName;

        public string String
        {
            get
            {
                return (string)valueNode.Value;
            }
            set
            {
                valueNode.Value = value;
            }
        }

        public override string Text
        {
            get
            {
                if (enumTable.HasValue(String))
                {
                    return $"\"{enumTable.GetEnum(String)}\"";
                }
                else
                {
                    return $"\"{String}\"";
                }
            }

            set
            {
                if (enumTable.HasEnum(value))
                {
                    // TODO strip quotes?
                    String = enumTable.GetValue(value);
                }
                // TODO else?!?
                else
                {
                    // TODO could assign String = value ?
                    throw new Exception($"Unknown enum text {value}");
                }
            }
        }

        public EnumStringValue(ValueNode valueNode, EnumTableString enumTableString)
            : base(valueNode)
        {
            if (valueNode.Value.GetType() != typeof(string))
                throw new Exception($"{this.GetType().Name} cannot use value node {valueNode.GetType().Name}");

            enumTable = enumTableString;
        }

        public override bool KnowsValue(object value)
        {
            return enumTable.HasValue((string)value);
        }
    }

    public class EnumIdentifierValue : EnumStringValue
    {
        public override string Text
        {
            get
            {
                if (enumTable.HasValue(String))
                {
                    return enumTable.GetEnum(String);
                }
                else
                {
                    return String;
                }
            }

            set
            {
                if (enumTable.HasEnum(value))
                {
                    String = enumTable.GetValue(value);
                }
                // TODO else?!?
                else
                {
                    // TODO could assign String = value ?
                    throw new Exception($"Unknown enum text {value}");
                }
            }
        }

        public string IdentifierName
        {
            get
            {
                return (string)valueNode.Value;
            }
            set
            {
                valueNode.Value = value;
            }
        }

        public EnumIdentifierValue(ValueNode valueNode, EnumTableString enumTableString)
            : base(valueNode, enumTableString)
        {
        }
    }
}
