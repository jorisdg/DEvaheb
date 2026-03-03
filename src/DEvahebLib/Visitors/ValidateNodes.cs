using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class ValidateNodes : StackVisitorBase
    {
        public List<Diagnostic> Diagnostics { get; } = new();

        public static List<Diagnostic> Validate(Node node)
        {
            var validator = new ValidateNodes();
            validator.Visit(node);

            return validator.Diagnostics;
        }

        public static List<Diagnostic> Validate(List<Node> nodes)
        {
            var validator = new ValidateNodes();
            validator.Visit(nodes);

            return validator.Diagnostics;
        }

        public override void VisitFunctionNode(FunctionNode node)
        {
            ValidateNode(node);

            base.VisitFunctionNode(node);
        }

        public override void VisitBlockNode(BlockNode node)
        {
            ValidateNode(node);

            base.VisitBlockNode(node);
        }

        private void ValidateNode(FunctionNode node)
        {
            var args = node.Arguments.ToList();
            int expected = node.ExpectedArgCount;

            if (args.Count != expected)
            {
                Diagnostics.Add(Diagnostic.ERR001_InvalidArgumentCount(node, expected, args.Count));
            }

            foreach (var arg in args)
            {
                if (arg is FunctionNode function && function.ValueType == typeof(VoidValue))
                {
                    Diagnostics.Add(Diagnostic.ERR002_FunctionArgumentIsVoid(node, args.IndexOf(arg)));
                }
            }

            if (node is If && args.Count > 1 && !(args[1] is OperatorNode))
            {
                Diagnostics.Add(Diagnostic.ERR003_IfSecondArgumentNotOperator(node));
            }
            else if (node is Task && HasParentBlockOfType<Loop>())
            {
                blockStack.Any(block => block is Loop && 
                    (
                        (((Loop)block).Count is FloatValue floatCount && floatCount.Float != 0)
                        ||
                        (((Loop)block).Count is IntegerValue intCount && intCount.Integer != 0)
                    ));
                Diagnostics.Add(Diagnostic.WARN001_TaskInsideLoop(node));
            }
        }
    }
}
