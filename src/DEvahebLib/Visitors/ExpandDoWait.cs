using System;
using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class ExpandDoWait : Visitor
    {
        public static void Expand(List<Node> nodes)
        {
            var visitor = new ExpandDoWait();
            visitor.Visit(nodes);
        }

        public override void Visit(List<Node> rootNodes)
        {
            ExpandList(rootNodes);

            base.Visit(rootNodes);
        }

        public override void VisitBlockNodeSubNodes(BlockNode node)
        {
            ExpandList(node.SubNodes);

            base.VisitBlockNodeSubNodes(node);
        }

        private static void ExpandList(List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is DoWait dw)
                {
                    var doNode = new Do(dw.Arguments.ToArray());
                    var waitNode = new Wait(dw.Arguments.ToArray());
                    nodes[i] = doNode;
                    nodes.Insert(i + 1, waitNode);
                    i++;
                }
            }
        }
    }
}
