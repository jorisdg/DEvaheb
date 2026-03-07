using DEvahebLib.Nodes;

namespace DEvahebLib
{
    public enum DiagnosticLevel
    {
        Error,
        Warning,
        Info
    }

    public class Diagnostic
    {
        public DiagnosticLevel Level { get; private set; }

        public int DiagnosticCode { get; private set; }

        public string Message { get; private set; }

        public Node Node { get; private set; }

        private Diagnostic(DiagnosticLevel level, int diagnosticCode, string message, Node node)
        {
            Level = level;
            DiagnosticCode = diagnosticCode;
            Message = message;
            Node = node;
        }

        public static bool operator ==(Diagnostic left, Diagnostic right)
        {
            return left.Level == right.Level && left.DiagnosticCode == right.DiagnosticCode;
        }

        public static bool operator !=(Diagnostic left, Diagnostic right)
        {
            return left.Level != right.Level || left.DiagnosticCode != right.DiagnosticCode;
        }

        protected static string NodeName(Node node)
        {
            return (node is FunctionNode) ? (node as FunctionNode).Name : node?.GetType().Name;
        }

        // ERRORS
        public static Diagnostic ERR001_InvalidArgumentCount(FunctionNode node, int expected, int actual)
            => new Diagnostic(DiagnosticLevel.Error, 1, $"'{node?.Name}' requires {expected} argument{(expected == 1 ? "" : "s")}, got {actual}", node);

        public static Diagnostic ERR002_FunctionArgumentIsVoid(FunctionNode node, int argIndex)
            => new Diagnostic(DiagnosticLevel.Error, 2, $"Argument '{argIndex + 1}()' of function '{node?.Name}' is a void function, which is not allowed", node);

        public static Diagnostic ERR003_IfSecondArgumentNotOperator(FunctionNode node)
            => new Diagnostic(DiagnosticLevel.Error, 3, $"The second argument of an 'if()' must be an operator", node);

        public static Diagnostic ERR004_UnexpectedEndOfLine(Node node)
            => new Diagnostic(DiagnosticLevel.Error, 4, $"Unexpected end of line in '{NodeName(node)}'", node);

        public static Diagnostic ERR005_UnexpectedCharacter(Node node)
            => new Diagnostic(DiagnosticLevel.Error, 5, $"Unexpected character in '{NodeName(node)}'", node);

        public static Diagnostic ERR006_UnexpectedEndOfLine(Node node, char expected, char found)
            => new Diagnostic(DiagnosticLevel.Error, 6, $"Expected '{expected}' but found '{found}' in '{NodeName(node)}'", node);

        // WARNINGS
        public static Diagnostic WARN001_TaskInsideLoop(Node node)
            => new Diagnostic(DiagnosticLevel.Warning, 1, $"Defining a task inside a loop is not allowed", node);

        public static Diagnostic WARN002_NoSemiColonFound(Node node)
            => new Diagnostic(DiagnosticLevel.Warning, 2, $"No semi-colon found at the end of expression '{NodeName(node)}'", node);

        public static Diagnostic WARN003_UnknownTask(Node node, string entityName, string taskName)
            => new Diagnostic(DiagnosticLevel.Warning, 3, $"Task '{taskName}' {(!string.IsNullOrEmpty(entityName) ? $"for entity '{entityName}' " : "")}is not defined in this script", node);
    }
}
