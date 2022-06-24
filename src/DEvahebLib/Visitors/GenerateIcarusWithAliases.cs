using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class GenerateIcarusWithAliases : GenerateIcarus
    {
        string lastDo = string.Empty;
        Int32 lastLength = 0;

        public GenerateIcarusWithAliases()
            : base()
        {
        }

        public GenerateIcarusWithAliases(Variables variables)
            : base(variables)
        {
        }

        public override void Visit(Node node)
        {
            if (argumentStack.Count == 0 && node is FunctionNode doFunction && doFunction.Name == "do")
            {
                lastLength = SourceCode.Length;

                base.Visit(node);

                lastDo = SourceCode.ToString().Substring(lastLength, SourceCode.Length - lastLength);
            }
            else if (argumentStack.Count == 0 && !string.IsNullOrEmpty(lastDo) && node is FunctionNode waitFunction && waitFunction.Name == "wait")
            {
                int origLength = SourceCode.Length;

                base.Visit(node);

                string localIndent = blockStack.Count == 0 ? indent : string.Empty;
                string wait = SourceCode.ToString().Substring(origLength, SourceCode.Length - origLength).Replace(localIndent + "wait", localIndent + "do");

                if (wait == lastDo)
                {
                    SourceCode.Length = lastLength;
                    SourceCode.Append(wait.Replace(localIndent + "do", localIndent + "dowait"));
                }

                lastDo = string.Empty;
            }
            else
            {
                if (argumentStack.Count == 0)
                    lastDo = String.Empty;

                base.Visit(node);
            }
        }

        public override void VisitBlockNodeSubNodes(BlockNode node)
        {
            base.VisitBlockNodeSubNodes(node);

            lastDo = String.Empty;
        }

        public override void VisitFunctionNode(FunctionNode node)
        {
            base.VisitFunctionNode(node);
        }
    }
}
