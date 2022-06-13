using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvaheb.Nodes
{
    public class GenericFunction : FunctionNode
    {
        public string Name { get; protected set; }

        //public ValueNode ReturnType { get; protected set; } // TODO?

        List<Node> arguments;
        public IEnumerable<Node> Arguments { get { return arguments; } }

        public override int ArgumentCount {  get { return arguments.Count; } }

        public override int RecursiveArgumentCount
        { 
            get 
            { 
                int count = arguments.Count;

                foreach(var node in arguments)
                {
                    if (node is FunctionNode func)
                        count += func.RecursiveArgumentCount;
                }

                return count;
            } 
        }

        internal protected GenericFunction(string name, List<Node> arguments)
            : base()
        {
            this.Name = name;
            this.arguments = arguments ?? new List<Node>();
        }

        internal protected GenericFunction(string name)
            : this(name, new List<Node>())
        {
        }

        public override string ToString()
        {
            var text = new StringBuilder();

            text.Append($"{Name} ( ");
            if (ArgumentCount > 0)
            {
                var args = Arguments.GetEnumerator();
                if (args.MoveNext())
                    text.Append(args.Current.ToString());

                while (args.MoveNext())
                {
                    text.Append($", {args.Current.ToString()}");
                }
            }
            text.Append($" )");

            return text.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }
}
