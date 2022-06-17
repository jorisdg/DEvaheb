using System.Collections.Generic;
using System.Text;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class If : BlockNode
    {
        public override IEnumerable<Node> Arguments => new List<Node> { Expr1, Operator, Expr2 };

        public Node Expr1 { get; set; }

        public OperatorNode Operator { get; set; }

        public Node Expr2 { get; set; }

        public If()
            : this(expression1: null, operatorNode: new OperatorNode(Enums.Operator.Eq), expression2: null, childNodes: null)
        {
        }

        public If(Node expression1, OperatorNode operatorNode, Node expression2)
            : this(expression1, operatorNode, expression2, childNodes: null)
        {

        }

        public If(Node expression1, OperatorNode operatorNode, Node expression2, List<Node> childNodes)
            : base(name: "if", childNodes)
        {
            Expr1 = expression1;
            Operator = operatorNode;
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
            StringBuilder sbuild = new StringBuilder();

            sbuild.AppendLine();
            sbuild.Append($"{indent}if ( ");

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

            string children = ChildrenToString(indent + "\t");
            if (children.Length > 0)
            {
                sbuild.AppendLine(children);
            }

            sbuild.Append(indent);
            sbuild.AppendLine("}");

            return sbuild.ToString();
        }
    }

    public class Task : BlockNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { TaskName };

        public Node TaskName { get; set; }

        public Task()
            : this(new StringValue("DEFAULT"))
        {
        }

        public Task(Node name)
            : this(name, childNodes: new List<Node>())
        {
        }

        public Task(Node name, List<Node> childNodes)
            : base(name: "task", childNodes)
        {
            TaskName = name;
        }

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.AppendLine();
            sbuild.Append($"{indent}task ( ");

            sbuild.Append(TaskName.ToString());

            sbuild.AppendLine(" )");
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            string children = ChildrenToString(indent + "\t");
            if (children.Length > 0)
            {
                sbuild.AppendLine(children);
            }

            sbuild.Append(indent);
            sbuild.AppendLine("}");

            return sbuild.ToString();
        }
    }

    public class Affect : BlockNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { EntityName, Type };

        public Node EntityName { get; set; }

        public Node Type { get; set; }

        public Affect()
            : this(new StringValue("DEFAULT"), new FloatValue((float)AffectType.INSERT))
        {
        }

        public Affect(Node name, Node type)
            : base(name: "affect")
        {
            EntityName = name;
            Type = type;
        }

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.AppendLine();
            sbuild.Append($"{indent}affect ( ");

            sbuild.Append(EntityName.ToString());
            sbuild.Append(", ");
            sbuild.Append(Type.ToString());

            sbuild.AppendLine(" )");
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            string children = ChildrenToString(indent + "\t");
            if (children.Length > 0)
            {
                sbuild.AppendLine(children);
            }

            sbuild.Append(indent);
            sbuild.AppendLine("}");

            return sbuild.ToString();
        }
    }

    public class Else : BlockNode // TODO newlines before and after?
    {
        public override IEnumerable<Node> Arguments => new List<Node>();

        public Else()
            : base(name: "else")
        {
        }

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}else (  )");

            sbuild.AppendLine();
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            string children = ChildrenToString(indent + "\t");
            if (children.Length > 0)
            {
                sbuild.AppendLine(children);
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }
    }

    public class Loop : BlockNode // TODO newlines before and after?
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Count };

        public Node Count { get; set; }

        public AffectType Type { get; set; }

        public Loop()
            : this(10)
        {
        }

        public Loop(int count)
            : this(new FloatValue(count))
        {
        }

        public Loop(Node count)
            : base(name: "loop")
        {
            Count = count;
        }

        public override string ToString(string indent)
        {
            StringBuilder sbuild = new StringBuilder($"{indent}loop ( {Count} )");

            sbuild.AppendLine();
            sbuild.Append(indent);
            sbuild.AppendLine("{");

            var children = ChildrenToString(indent + "\t");
            if (children.Length > 0)
            {
                sbuild.AppendLine(children);
            }

            sbuild.Append(indent);
            sbuild.Append("}");

            return sbuild.ToString();
        }
    }
}
