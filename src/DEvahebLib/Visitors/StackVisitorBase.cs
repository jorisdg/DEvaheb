using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class StackVisitorBase : Visitor
    {
        protected Stack<Node> blockStack = new Stack<Node>();

        protected Stack<Tuple<FunctionNode, Stack<Node>>> argumentStack = new Stack<Tuple<FunctionNode, Stack<Node>>>();

        public override void VisitBlockNode(BlockNode node)
        {
            blockStack.Push(node);

            base.VisitBlockNode(node);

            blockStack.Pop();
        }

        public override void VisitBlockNodeArguments(BlockNode node)
        {
            argumentStack.Push(new Tuple<FunctionNode, Stack<Node>>(node, new Stack<Node>()));

            base.VisitBlockNodeArguments(node);

            argumentStack.Pop();
        }

        public override void VisitFunctionNode(FunctionNode node)
        {
            argumentStack.Push(new Tuple<FunctionNode, Stack<Node>>(node, new Stack<Node>()));

            base.VisitFunctionNode(node);

            argumentStack.Pop();
        }

        public override void VisitFunctionArgument(FunctionNode function, Node argument)
        {
            argumentStack.Peek().Item2.Push(argument);

            base.VisitFunctionArgument(function, argument);
        }
    }
}
