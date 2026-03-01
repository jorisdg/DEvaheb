using System;
using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class ValidateNodes : StackVisitorBase
    {
        public List<string> Errors { get; } = new List<string>();

        public static List<string> Validate(Node node)
        {
            var validator = new ValidateNodes();
            validator.Visit(node);

            return validator.Errors;
        }

        public static List<string> Validate(List<Node> nodes)
        {
            var validator = new ValidateNodes();
            validator.Visit(nodes);

            return validator.Errors;
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
                string label = node is Camera camera ? $"camera({camera.GetCommand()})" : $"{node.Name}()";
                Errors.Add($"{label} requires {expected} argument{(expected == 1 ? "" : "s")}, got {args.Count}");
            }

            if (node is If && args.Count > 1 && !(args[1] is OperatorNode))
            {
                Errors.Add("if() second argument must be an operator");
            }
            else if (node is Task && HasParentBlockOfType<Loop>())
            {
                blockStack.Any(block => block is Loop && 
                    (
                        (((Loop)block).Count is FloatValue floatCount && floatCount.Float != 0)
                        ||
                        (((Loop)block).Count is IntegerValue intCount && intCount.Integer != 0)
                    ));
                Errors.Add("Defining a task inside a loop");
            }
        }
    }
}
