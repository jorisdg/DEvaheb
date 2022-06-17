using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public abstract class Visitor
    {
        virtual public void Visit(List<Node> rootNodes)
        {
            for (int i = 0; i < rootNodes.Count; i++)
            {
                Visit(rootNodes[i]);
            }
        }

        virtual public void Visit(Node node)
        {
            if (node is OperatorNode opr) // OperatorNode inherits from ValueNode so check first
            {
                VisitOperatorNode(opr);
            }
            else if (node is ValueNode value)
            {
                VisitValueNode(value);
            }
            else if (node is BlockNode block) // BlockNode inherits from FunctionNode so check first
            {
                VisitBlockNode(block);
            }
            else if (node is FunctionNode function)
            {
                VisitFunctionNode(function);
            }
            else
            {
                VisitMisteryNode(node);
            }
        }

        virtual public void VisitValueNode(ValueNode node)
        {
            if (node is VectorValue vectorValue)
            {
                for (int i = 0; i < vectorValue.Values.Length; i++)
                {
                    VisitVectorValue(vectorValue, vectorValue.Values[i]);
                }
            }
        }

        virtual public void VisitVectorValue(VectorValue vector, Node node)
        {
            Visit(node);
        }

        virtual public void VisitFunctionArgument(FunctionNode function, Node argument)
        {
            Visit(argument);
        }

        virtual public void VisitFunctionNode(FunctionNode node)
        {
            foreach (var arg in node.Arguments)
            {
                VisitFunctionArgument(node, arg);
            }
        }

        virtual public void VisitBlockNode(BlockNode node)
        {
            VisitBlockNodeArguments(node);

            VisitBlockNodeSubNodes(node);
        }

        virtual public void VisitBlockNodeArguments(BlockNode node)
        {
            foreach (var arg in node.Arguments)
            {
                VisitFunctionArgument(node, arg);
            }
        }
        virtual public void VisitBlockNodeSubNodes(BlockNode node)
        {
            foreach (var subNode in node.SubNodes)
            {
                VisitBlockNodeSubNode(blockNode: node, subNode);
            }
        }

        virtual public void VisitBlockNodeSubNode(BlockNode blockNode, Node subNode)
        {
            Visit(subNode);
        }

        virtual public void VisitOperatorNode(OperatorNode node)
        {
        }

        virtual public void VisitMisteryNode(Node node)
        {
        }
    }
}
