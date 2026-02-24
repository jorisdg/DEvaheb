using System;
using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class CompareNodes : Visitor
    {
        private readonly float floatTolerance;
        private readonly bool stopOnFirst;
        private Node currentActual;
        private readonly Stack<string> pathStack = new Stack<string>();
        private bool stopped;

        public List<string> Differences { get; } = new List<string>();

        public CompareNodes(float floatTolerance = 0.001f, bool stopOnFirst = true)
        {
            this.floatTolerance = floatTolerance;
            this.stopOnFirst = stopOnFirst;
            pathStack.Push("root");
        }

        public static List<string> Compare(List<Node> expected, List<Node> actual, float floatTolerance = 0.001f, bool stopOnFirst = true)
        {
            var comparer = new CompareNodes(floatTolerance, stopOnFirst);
            comparer.CompareLists(expected, NormalizeNodes(actual), "root");
            return comparer.Differences;
        }

        private string CurrentPath => string.Join("", pathStack.Reverse());

        private void AddDifference(string message)
        {
            Differences.Add($"{CurrentPath}: {message}");
            if (stopOnFirst) stopped = true;
        }

        private void CompareLists(List<Node> expected, List<Node> actual, string pathPrefix)
        {
            if (stopped) return;

            if (expected.Count != actual.Count)
            {
                AddDifference($"node count mismatch (expected {expected.Count}, got {actual.Count})");
                if (stopped) return;
            }

            int count = Math.Min(expected.Count, actual.Count);
            for (int i = 0; i < count; i++)
            {
                if (stopped) return;

                pathStack.Push($"[{i}]");
                currentActual = actual[i];

                if (expected[i] == null && actual[i] == null)
                {
                    // both null, ok
                }
                else if (expected[i] == null)
                {
                    AddDifference($"expected null, got {actual[i].GetType().Name}");
                }
                else if (actual[i] == null)
                {
                    AddDifference($"expected {expected[i].GetType().Name}, got null");
                }
                else
                {
                    Visit(expected[i]);
                }

                pathStack.Pop();
            }
        }

        public override void VisitValueNode(ValueNode expected)
        {
            if (stopped) return;

            var actual = currentActual;

            if (expected.GetType() != actual.GetType())
            {
                AddDifference($"type mismatch (expected {expected.GetType().Name}, actual {actual.GetType().Name})");
                return;
            }

            if (expected is VectorValue ev)
            {
                var av = (VectorValue)actual;
                for (int i = 0; i < 3; i++)
                {
                    if (stopped) return;
                    pathStack.Push($".vec[{i}]");
                    currentActual = av.Values[i];
                    Visit(ev.Values[i]);
                    pathStack.Pop();
                }
                currentActual = actual;
                return;
            }

            if (expected is EnumValue expectedEnum)
            {
                var actualEnum = (EnumValue)actual;
                if (expectedEnum.Name != actualEnum.Name)
                {
                    AddDifference($"enum name mismatch (expected {expectedEnum.Name}, actual {actualEnum.Name})");
                    return;
                }

                CompareValues(expectedEnum.Value, actualEnum.Value);
                return;
            }

            CompareValues(expected.Value, ((ValueNode)actual).Value);
        }

        private void CompareValues(object expected, object actual)
        {
            if (expected is float ef && actual is float af)
            {
                double tolerance = Math.Max(floatTolerance, Math.Abs((double)ef) * 1e-6);
                if (Math.Abs((double)ef - (double)af) > tolerance)
                    AddDifference($"float mismatch (expected {ef}, actual {af})");
            }
            else if (!object.Equals(expected, actual))
            {
                AddDifference($"value mismatch (expected {expected}, actual {actual})");
            }
        }

        public override void VisitFunctionNode(FunctionNode expected)
        {
            if (stopped) return;

            var actual = currentActual;

            if (expected.GetType() != actual.GetType())
            {
                AddDifference($"type mismatch (expected {expected.GetType().Name}, actual {actual.GetType().Name})");
                return;
            }

            var actualFn = (FunctionNode)actual;
            if (expected.Name != actualFn.Name)
            {
                AddDifference($"function name mismatch (expected '{expected.Name}', actual '{actualFn.Name}')");
                return;
            }

            var eArgs = new List<Node>(expected.Arguments);
            var aArgs = new List<Node>(actualFn.Arguments);

            pathStack.Push(".args");
            CompareLists(eArgs, aArgs, "args");
            pathStack.Pop();

            currentActual = actual;
        }

        public override void VisitBlockNode(BlockNode expected)
        {
            if (stopped) return;

            var actual = currentActual;

            if (expected.GetType() != actual.GetType())
            {
                AddDifference($"type mismatch (expected {expected.GetType().Name}, actual {actual.GetType().Name})");
                return;
            }

            var actualBlock = (BlockNode)actual;
            if (expected.Name != actualBlock.Name)
            {
                AddDifference($"function name mismatch (expected '{expected.Name}', actual '{actualBlock.Name}')");
                return;
            }

            var eArgs = new List<Node>(expected.Arguments);
            var aArgs = new List<Node>(actualBlock.Arguments);

            pathStack.Push(".args");
            CompareLists(eArgs, aArgs, "args");
            pathStack.Pop();

            pathStack.Push(".body");
            CompareLists(expected.SubNodes, actualBlock.SubNodes, "body");
            pathStack.Pop();

            currentActual = actual;
        }

        public override void VisitOperatorNode(OperatorNode expected)
        {
            if (stopped) return;

            var actual = currentActual;

            if (expected.GetType() != actual.GetType())
            {
                AddDifference($"type mismatch (expected {expected.GetType().Name}, actual {actual.GetType().Name})");
                return;
            }

            var actualOp = (OperatorNode)actual;
            if (expected.Operator != actualOp.Operator)
                AddDifference($"operator mismatch (expected {expected.Operator}, actual {actualOp.Operator})");
        }

        private static List<Node> NormalizeNodes(List<Node> nodes)
        {
            var result = new List<Node>();
            foreach (var node in nodes)
            {
                if (node is DoWait dw)
                {
                    result.Add(new Do(dw.Arguments.ToArray()));
                    result.Add(new Wait(dw.Arguments.ToArray()));
                }
                else
                {
                    result.Add(node);
                }
            }
            return result;
        }
    }
}
