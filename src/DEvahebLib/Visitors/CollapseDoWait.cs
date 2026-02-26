using System;
using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class CollapseDoWait : Visitor
    {
        public static void Collapse(List<Node> nodes)
        {
            var visitor = new CollapseDoWait();
            visitor.Visit(nodes);
        }

        public override void Visit(List<Node> rootNodes)
        {
            base.Visit(rootNodes);

            CollapseList(rootNodes);
        }

        public override void VisitBlockNodeSubNodes(BlockNode node)
        {
            base.VisitBlockNodeSubNodes(node);

            CollapseList(node.SubNodes);
        }

        private static void CollapseList(List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count - 2; i++)
            {
                if (nodes[i] is Do doNode && nodes[i + 1] is Wait waitNode)
                {
                    var doArgs = doNode.Arguments.ToArray();
                    var waitArgs = waitNode.Arguments.ToArray();

                    if (doArgs.Length == 1 && waitArgs.Length == 1 && ArgsEqual(doArgs[0], waitArgs[0]))
                    {
                        nodes[i] = new DoWait(doArgs);
                        nodes.RemoveAt(i + 1);
                    }
                }
            }
        }

        private static bool ArgsEqual(Node a, Node b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.GetType() != b.GetType()) return false;

            if (a is ValueNode va && b is ValueNode vb)
                return object.Equals(va.Value, vb.Value);

            return false;
        }
    }
}
