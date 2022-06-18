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

        public Node()
        {
        }

        sealed public override string ToString()
        {
            return ToString(string.Empty);
        }

        public abstract string ToString(string indent);
    }

    public abstract class ValueNode : Node
    {
        public virtual object Value { get; set; } = null;

        public ValueNode()
            : base()
        {
        }
    }

    public abstract class FunctionNode : Node
    {
        public string Name { get; protected set; }

        public abstract IEnumerable<Node> Arguments { get; }

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

        public FunctionNode(string name)
            : base()
        {
            Name = name;
        }

        public override string ToString(string indent)
        {
            var text = new StringBuilder();

            text.Append($"{indent}{Name} ( ");
            bool first = true;
            foreach(var arg in Arguments)
            {
                if (!first)
                {
                    text.Append(", ");
                }
                text.Append(arg.ToString());

                first = false;
            }
            text.Append($" )");

            return text.ToString();
        }
    }

    public class GenericFunction : FunctionNode
    {
        List<Node> arguments;

        public override IEnumerable<Node> Arguments => arguments;

        public GenericFunction(string name, List<Node> arguments)
            : base(name)
        {
            this.arguments = arguments ?? new List<Node>();
        }
    }

    public abstract class BlockNode : FunctionNode
    {
        public List<Node> SubNodes { get; protected set; }

        public BlockNode(string name)
            : this(name, childNodes: new List<Node>())
        {
        }

        public BlockNode(string name, List<Node> childNodes)
            : base(name)
        {
            SubNodes = childNodes ?? new List<Node>();
        }

        protected string ChildrenToString(string indent)
        {
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < SubNodes.Count; i++)
            {
                build.Append(SubNodes[i].ToString(indent));

                if (!(SubNodes[i] is BlockNode))
                {
                    build.Append(";");
                }

                if (i < SubNodes.Count - 1)
                {
                    build.AppendLine("");
                }
            }

            return build.ToString();
        }
    }
}
