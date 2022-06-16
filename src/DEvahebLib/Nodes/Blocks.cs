using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEvahebLib.Nodes
{
    public class If : BlockNode
    {
        public Node Expr1 { get; set; }

        public OperatorNode Operator { get; set; }

        public Node Expr2 { get; set; }

        internal protected If()
            : this(expression1: null, new OperatorNode(Operators.Eq), expression2: null)
        {
        }

        internal protected If(Node expression1, OperatorNode opr, Node expression2)
            : base()
        {
            Expr1 = expression1;
            Operator = opr;
            Expr2 = expression2;
        }

        public override int Size
        {
            get
            {
                int count = 2; // count ourselves and operator

                count += Expr1.Size;
                count += Expr2.Size;

                return count;
            }
        }

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}if ( ");

            if (Expr1 is ValueNode)
                sbuild.Append($"${Expr1.ToString()}$, ");
            else
                sbuild.Append($"{Expr1.ToString()}, ");

            sbuild.Append(Operator.ToString());
            sbuild.Append(", ");

            if (Expr2 is ValueNode)
                sbuild.Append($"${Expr2.ToString()}$");
            else
                sbuild.Append($"{Expr2.ToString()}");

            sbuild.AppendLine(" )");
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            if (ChildNodes.Count > 0)
            {
                sbuild.AppendLine(ChildrenToString(indent + "\t"));
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }

        static public OperatorNode CreateOperator(Operators op)
        {
            return new OperatorNode(op);
        }
    }

    public class Task : BlockNode
    {
        public Node Name { get; set; }

        internal protected Task()
            : this(ValueNode.Create("DEFAULT"))
        {
        }

        internal protected Task(Node name)
            : base()
        {
            Name = name;
        }

        public override int Size => 2;

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}task ( ");

            sbuild.Append(Name.ToString());

            sbuild.AppendLine(" )");
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            if (ChildNodes.Count > 0)
            {
                sbuild.AppendLine(ChildrenToString(indent + "\t"));
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public enum AffectType
    {
        INSERT = 55,
        FLUSH = 56
    }

    public class Affect : BlockNode
    {
        public Node Name { get; set; }

        public AffectType Type { get; set; }

        internal protected Affect()
            : this(ValueNode.Create("DEFAULT"), AffectType.INSERT)
        {
        }

        internal protected Affect(Node name, AffectType type)
            : base()
        {
            Name = name;
            Type = type;
        }

        public override int Size => 3;

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}affect ( ");

            sbuild.Append(Name.ToString());
            sbuild.Append(", ");
            sbuild.Append(Type.ToString());

            sbuild.AppendLine(" )");
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            if (ChildNodes.Count > 0)
            {
                sbuild.AppendLine(ChildrenToString(indent + "\t"));
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class Else : BlockNode
    {
        internal protected Else()
            : base()
        {
        }

        public override int Size => 1;

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}else (  )");

            sbuild.AppendLine();
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            if (ChildNodes.Count > 0)
            {
                sbuild.AppendLine(ChildrenToString(indent + "\t"));
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }

    public class Loop : BlockNode
    {
        public int Count { get; set; }

        public AffectType Type { get; set; }

        internal protected Loop()
            : this(10)
        {
        }

        internal protected Loop(int count)
            : base()
        {
            Count = count;
        }

        public override int Size => 2;

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}loop ( {Count} )");

            sbuild.AppendLine();
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            if (ChildNodes.Count > 0)
            {
                sbuild.AppendLine(ChildrenToString(indent + "\t"));
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }
    }
}
