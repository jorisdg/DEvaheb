using System;
using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class ValidateNodes : Visitor
    {
        public List<string> Errors { get; } = new List<string>();

        private Stack<BlockNode> blockStack = new Stack<BlockNode>();

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
            blockStack.Push(node);
            base.VisitBlockNode(node);
            blockStack.Pop();
        }

        private bool IsInsideBlock<T>() where T : BlockNode
        {
            foreach (var block in blockStack)
            {
                if (block is T)
                    return true;
            }
            return false;
        }

        private void ValidateNode(FunctionNode node)
        {
            var args = node.Arguments.ToList();
            int argCount = args.Count;

            int expected = node.ExpectedArgCount;
            if (expected >= 0 && argCount != expected)
            {
                string label = node is Camera camera ? $"camera({camera.GetCommand()})" : $"{node.Name}()";
                Errors.Add($"{label} requires {expected} argument{(expected == 1 ? "" : "s")}, got {argCount}");
            }

            if (node is If && argCount > 1 && !(args[1] is OperatorNode))
                Errors.Add("if() second argument must be an operator");

            if (node is Loop && argCount > 0 && !(args[0] is IntegerValue) && !(args[0] is FloatValue) && !(args[0] is FunctionNode))
                Errors.Add("loop() argument must be an integer, float, or function");

            if (node is Move && argCount != 2 && argCount != 3)
                Errors.Add($"move() requires 2 or 3 arguments, got {argCount}");

            if (node is Move && argCount == 2 && !IsInsideBlock<Affect>())
                Errors.Add("move() with 2 arguments is only valid inside an affect block");

            if (node is Camera && expected < 0)
                Errors.Add("camera() first argument must be a CAMERA_COMMANDS enum value");
        }
    }
}
