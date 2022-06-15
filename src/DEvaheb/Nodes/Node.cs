using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvaheb.Nodes
{
    public abstract class Node
    {
        abstract public int Size { get; }

        internal protected Node()
        {
        }

        sealed public override string ToString()
        {
            return ToString(string.Empty);
        }

        public abstract string ToString(string indent);

        public abstract byte[] ToBinary();
    }

    public class ReturnFlagNode : Node
    {
        public override int Size => 1;

        public byte B1 { get; set; }

        public byte B2 { get; set; }

        public ReturnFlagNode()
            : this(0, 0)
        {
        }

        public ReturnFlagNode(byte b1, byte b2)
            : base()
        {
            B1 = b1;
            B2 = b2;
        }

        public override string ToString(string indent)
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BlockNode : Node
    {
        public List<Node> ChildNodes = new List<Node>();

        internal protected BlockNode()
            : base()
        {
        }

        protected string ChildrenToString(string indent)
        {
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < ChildNodes.Count; i++)
            {
                build.Append(ChildNodes[i].ToString(indent));

                if (!(ChildNodes[i] is BlockNode))
                {
                    build.Append(";");
                }
                
                if (i < ChildNodes.Count - 1)
                {
                    build.AppendLine("");
                }
            }

            return build.ToString();
        }

        public static If CreateIf(Node expression1, OperatorNode opr, Node expression2)
        {
            return new If(expression1, opr, expression2);
        }

        public static Else CreateElse()
        {
            return new Else();
        }

        public static Task CreateTask(Node name)
        {
            return new Task(name);
        }

        public static Affect CreateAffect(Node name, AffectType type)
        {
            return new Affect(name, type);
        }

        public static Loop CreateLoop(int count)
        {
            return new Loop(count);
        }
    }

    public abstract class FunctionNode : Node
    {
        public string Name { get; protected set; }

        public ReturnFlagNode Return { get; set; }

        abstract public int ArgumentCount { get; }

        internal protected FunctionNode()
            : base()
        {
            Return = null;
        }

        static public FunctionNode CreateGeneric(string name, List<Node> arguments)
        {
            return new GenericFunction(name, arguments);
        }

        static public FunctionNode CreateGeneric(string name, List<Node> arguments, ReturnFlagNode returnNode)
        {
            var node = CreateGeneric(name, arguments);
            node.Return = returnNode;

            return node;
        }

        static public Tag CreateTag(Node name, TagType type)
        {
            return new Tag(name, type);
        }

        static public Get CreateGet(ValueType type, Node variableName)
        {
            return new Get(type, variableName);
        }

        static public Camera CreateCamera(List<Node> arguments)
        {
            return new Camera(arguments);
        }

        static public Random CreateRandom(Node min, Node max)
        {
            return new Random(min, max);
        }

        public static Declare CreateDeclare(ValueType type, Node variableName)
        {
            return new Declare(type, variableName);
        }

        public static Sound CreateSound(string channel, Node filename)
        {
            return new Sound(channel, filename);
        }
    }

    public abstract class ValueNode : Node
    {
        public object? Value { get; set; } = null;

        internal protected ValueNode()
            : base()
        {
        }

        static public ValueNode Create(string stringValue)
        {
            return new StringValue() { String = stringValue };
        }

        static public ValueNode Create(float floatValue)
        {
            return new FloatValue() { Float = floatValue };
        }

        static public ValueNode CreateVector(float x, float y, float z)
        {
            return new VectorValue() { Vector = new Tuple<float, float, float>(x, y, z) };
        }

        static public ValueNode Create(Int16 integerValue)
        {
            return new IntegerValue() { Integer = integerValue };
        }

        static public ValueNode CreateIdentifier(string identifierName)
        {
            return new IdentifierValue() { IdentifierName = identifierName };
        }

        static public ValueNode CreateIdentifier(float identifierNumber)
        {
            return new IdentifierValue() { Float = identifierNumber };
        }
    }

    public enum Operators
    {
        Gt = 15,
        Lt = 16,
        Eq = 17,
        Ne = 18
    }

    public class OperatorNode : Node
    {
        public Operators Operator;

        public override int Size => 1;

        internal protected OperatorNode()
            : this(Operators.Eq)
        {
        }

        internal protected OperatorNode(Operators op)
            : base()
        {
            Operator = op;
        }

        public override string ToString(string indent)
        {
            string op = string.Empty;

            switch (Operator)
            {
                case Operators.Gt:
                    op = ">";
                    break;
                case Operators.Lt:
                    op = "<";
                    break;
                case Operators.Eq:
                    op = "=";
                    break;
                case Operators.Ne:
                    op = "!";
                    break;
            }

            return $"{indent}${op}$";
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }
}
