using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Nodes
{
    public abstract class Node
    {
        abstract public int Size { get; }

        public virtual Type ValueType => typeof(VoidValue);

        public Node()
        {
        }
    }

    public abstract class ValueNode : Node
    {
        public virtual object Value { get; set; } = null;

        public override Type ValueType => GetType();

        public ValueNode()
            : base()
        {
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} : {(Value?.ToString())}";
        }
    }

    public abstract class FunctionNode : Node
    {
        public string Name { get; protected set; }

        protected List<Node> arguments;

        public virtual IEnumerable<Node> Arguments => arguments;

        protected Node GetArg(int index) => index < arguments.Count ? arguments[index] : null;
        protected T GetArg<T>(int index) where T : Node => index < arguments.Count ? arguments[index] as T : null;
        public void SetArg(int index, Node value) { if (index < arguments.Count) arguments[index] = value; }

        public override int Size
        {
            get
            {
                int count = 1; // count ourselves

                foreach(var arg in Arguments)
                {
                    count += arg.Size;
                }

                return count;
            }
        }

        public FunctionNode(string name, params Node[] args)
            : base()
        {
            Name = name;
            arguments = new List<Node>(args);
        }

        /// <summary>
        /// Expected number of arguments for this node. Returns -1 if unknown/variable.
        /// </summary>
        public virtual int ExpectedArgCount => -1;

        public override string ToString()
        {
            return $"function {Name}()";
        }
    }

    public class GenericFunction : FunctionNode
    {
        public GenericFunction(string name, params Node[] args)
            : base(name, args)
        {
        }
    }

    public abstract class BlockNode : FunctionNode
    {
        public List<Node> SubNodes { get; protected set; }

        public BlockNode(string name, params Node[] args)
            : base(name, args)
        {
            SubNodes = new List<Node>();
        }

        public override string ToString()
        {
            return $"block {Name}() {{ }}";
        }
    }
}
