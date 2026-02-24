using System;
using System.Collections.Generic;
using System.Text;
using DEvahebLib.Enums;

namespace DEvahebLib.Nodes
{
    public class If : BlockNode
    {
        public Node Expr1 { get => GetArg(0); set => SetArg(0, value); }

        public OperatorNode Operator { get => GetArg<OperatorNode>(1); set => SetArg(1, value); }

        public Node Expr2 { get => GetArg(2); set => SetArg(2, value); }

        public override int ExpectedArgCount => 3;

        public If(Node expression1, OperatorNode operatorNode, Node expression2)
            : this(new Node[] { expression1, operatorNode, expression2 })
        {
        }

        public If(params Node[] args)
            : base("if", args)
        {
        }

        public override int Size
        {
            get
            {
                int count = 2; // count ourselves and operator

                if (Expr1 != null) count += Expr1.Size;
                if (Expr2 != null) count += Expr2.Size;

                return count;
            }
        }
    }

    public class Task : BlockNode
    {
        public Node TaskName { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Task()
            : this(new StringValue("DEFAULT"))
        {
        }

        public Task(params Node[] args)
            : base("task", args)
        {
        }
    }

    public class Affect : BlockNode
    {
        public Node EntityName { get => GetArg(0); set => SetArg(0, value); }

        public Node Type { get => GetArg(1); set => SetArg(1, value); }

        public override int ExpectedArgCount => 2;

        public Affect()
            : this(new StringValue("DEFAULT"), new FloatValue((float)AFFECT_TYPE.INSERT))
        {
        }

        public Affect(params Node[] args)
            : base("affect", args)
        {
        }
    }

    public class Else : BlockNode
    {
        public override int ExpectedArgCount => 0;

        public Else(params Node[] args)
            : base("else", args)
        {
        }
    }

    public class Loop : BlockNode
    {
        public Node Count { get => GetArg(0); set => SetArg(0, value); }

        public override int ExpectedArgCount => 1;

        public Loop()
            : this(10)
        {
        }

        public Loop(int count)
            : this(new FloatValue(count))
        {
        }

        public Loop(params Node[] args)
            : base("loop", args)
        {
        }
    }
}
