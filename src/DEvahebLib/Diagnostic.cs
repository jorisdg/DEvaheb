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

        // ERRORS
        public static Diagnostic ERR001_InvalidArgumentCount(FunctionNode node, int expected, int actual)
            => new Diagnostic(DiagnosticLevel.Error, 1, $"{node?.Name} requires {expected} argument{(expected == 1 ? "" : "s")}, got {actual}", node);

        public static Diagnostic ERR002_FunctionArgumentIsVoid(FunctionNode node, int argIndex)
            => new Diagnostic(DiagnosticLevel.Error, 2, $"Argument {argIndex + 1} of {node?.Name} is a void function, which is not allowed", node);

        public static Diagnostic ERR003_IfSecondArgumentNotOperator(FunctionNode node)
            => new Diagnostic(DiagnosticLevel.Error, 3, $"The second argument of an if() must be an operator", node);

        // WARNINGS
        public static Diagnostic WARN001_TaskInsideLoop(FunctionNode node)
            => new Diagnostic(DiagnosticLevel.Warning, 1, $"Defining a task inside a loop is not allowed", node);
    }
}
