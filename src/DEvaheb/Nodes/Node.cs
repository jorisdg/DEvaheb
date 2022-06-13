using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvaheb.Nodes
{
    public abstract class Node
    {
        internal protected Node()
        {
        }

        public abstract override string ToString();

        public abstract byte[] ToBinary();
    }

    public abstract class BlockNode : Node
    {
        internal protected BlockNode()
            : base()
        {
        }
    }

    public abstract class FunctionNode : Node
    {
        abstract public int ArgumentCount { get; }

        abstract public int RecursiveArgumentCount { get; }

        internal protected FunctionNode()
            : base()
        {
        }

        static public FunctionNode CreateGeneric(string name, List<Node> arguments)
        {
            return new GenericFunction(name, arguments);
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

        //static public ValueNode CreateVector(float x, float y, float z)
        //{
        //    return new VectorValue() { Float = new float[] { x, y, z } };
        //}

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
}
