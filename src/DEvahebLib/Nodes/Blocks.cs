using System;
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
    }

    public class Affect : BlockNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { EntityName, Type };

        public Node EntityName { get; set; }

        public Node Type { get; set; }

        public Affect()
            : this(new StringValue("DEFAULT"), new FloatValue((float)AFFECT_TYPE.INSERT))
        {
        }

        public Affect(Node name, Node type)
            : base(name: "affect")
        {
            EntityName = name;
            Type = type;
        }
    }

    public class Else : BlockNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>();

        public Else()
            : base(name: "else")
        {
        }
    }

    public class Loop : BlockNode
    {
        public override IEnumerable<Node> Arguments => new List<Node>() { Count };

        public Node Count { get; set; }

        public AFFECT_TYPE Type { get; set; }

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

            if (Count is FloatValue floatValue)
            {
                Count = new IntegerValue((Int32)floatValue.Float);
            }
            if (!(Count is IntegerValue) && !(Count is FunctionNode))
            {
                throw new Exception("Loop argument is not Int or Function");
            }
        }
    }
}
