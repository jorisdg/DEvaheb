using System.Collections.Generic;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLib.Visitors
{
    public class ValidateNodes : StackVisitorBase
    {
        public List<Diagnostic> Diagnostics { get; } = new();

        Stack<string> entityContext = new(["__THIS"]);

        Dictionary<string, SymbolTable> entitySymbols;

        public ValidateNodes(Dictionary<string, SymbolTable> symbols = null)
        {
            entitySymbols = symbols != null ? new(symbols) : new();
        }

        public static List<Diagnostic> Validate(Node node, Dictionary<string, SymbolTable> symbols = null)
        {
            var validator = new ValidateNodes(symbols);
            validator.Visit(node);

            return validator.Diagnostics;
        }

        public static List<Diagnostic> Validate(List<Node> nodes, Dictionary<string, SymbolTable> symbols = null)
        {
            var validator = new ValidateNodes(symbols);
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

            if (node is Affect affect)
            {
                entityContext.Push((affect.EntityName as StringValue)?.String ?? "__UNKNOWN");
            }
            else if ( node is Task task)
            {
                string currentEntity = entityContext.Peek();

                if (!entitySymbols.ContainsKey(currentEntity))
                {
                    entitySymbols[currentEntity] = new SymbolTable();
                }

                entitySymbols[currentEntity].AddTask((task.TaskName as StringValue).String);
            }

            base.VisitBlockNode(node);

            if (node is Affect)
            {
                // since we don't know what entity this was for, let's make sure we don't remember any tasks
                // so any subsequent unknown entities don't assume they are defined
                if (entityContext.Peek().Equals("__UNKNOWN"))
                {
                    entitySymbols.Remove(entityContext.Peek());
                }

                entityContext.Pop();
            }
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
            else if (node is Task task && HasParentBlockOfType<Loop>())
            {
                blockStack.Any(block => block is Loop && 
                    (
                        (((Loop)block).Count is FloatValue floatCount && floatCount.Float != 0)
                        ||
                        (((Loop)block).Count is IntegerValue intCount && intCount.Integer != 0)
                    ));
                Diagnostics.Add(Diagnostic.WARN001_TaskInsideLoop(node));
            }
            else if (node is Do || node is DoWait)
            {
                string taskName = (node is Do doNode) ? (doNode.Target as StringValue)?.String : ((node as DoWait).WaitName as StringValue)?.String;

                string currentEntity = entityContext.Peek();
                if (!entitySymbols.ContainsKey(currentEntity) || !entitySymbols[currentEntity].TaskExists(taskName))
                {
                    Diagnostics.Add(Diagnostic.WARN003_UnknownTask(node, currentEntity.StartsWith("__") ? null : currentEntity, taskName));
                }
            }
        }
    }
}