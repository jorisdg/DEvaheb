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

    public class EnumValue : ValueNode
    {
        ValueNode valueNode = null;

        public Type ValueNodeType => valueNode.GetType();

        public Type EnumType { get; protected set; }

        Dictionary<string, int> enumValues;

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

        public string Text
        {
            get
            {
                string text = null;

                if (Value is Single s)
                {
                    text = (from e in enumValues where e.Value == (int)s select e.Key).FirstOrDefault();
                }
                else if (Value is Int32 i)
                {
                    text = (from e in enumValues where e.Value == i select e.Key).FirstOrDefault();
                }

                return text ?? valueNode.ToString();
            }
            // TODO Set?
            //set
            //{
            //}
        }

        public override int Size => valueNode.Size;

        public EnumValue(ValueNode valueNode, Type enumType)
            : this(valueNode)
        {
            if (!enumType.IsEnum)
                throw new Exception("EnumValue node has to be instantiated with an type that is an enumeration"); // TODO specific exception

            EnumType = enumType;

            enumValues = new Dictionary<string, int>();
            var values = EnumType.GetEnumValues();
            for (int i = 0; i < values.Length; i++)
            {
                var value = (int)values.GetValue(i);

                enumValues.Add(EnumType.GetEnumName(value), value);
            }
        }

        public EnumValue(ValueNode valueNode, Dictionary<string, int> enumValues)
            : this(valueNode)
        {
            if (enumValues == null)
                throw new Exception("Cannot instantiate EnumValue without a list of enumeration values"); // TODO specific exception

            this.enumValues = enumValues;
        }

        private EnumValue(ValueNode valueNode)
            : base()
        {
            if (valueNode == null)
                throw new Exception("Cannot instantiate EnumValue without a ValueNode instance"); // TODO specific exception

            this.valueNode = valueNode;
        }

        public override string ToString()
        {
            return $"EnumValue : {Text} =  {valueNode.ToString()}";
        }

        public static Node CreateOrPassThrough(Node enumNode, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new Exception("EnumValue node has to be instantiated with an type that is an enumeration"); // TODO specific exception

            Node finalNode = null;

            if (enumNode is ValueNode valueNode)
            {
                finalNode = new EnumValue(valueNode, enumType);
            }
            else //if (enumNode is FunctionNode && !(enumNode is BlockNode))
            {
                finalNode = enumNode;
            }

            return finalNode;
        }
    }
}
