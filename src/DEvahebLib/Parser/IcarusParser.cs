using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DEvahebLib.Enums;
using DEvahebLib.Nodes;

namespace DEvahebLib.Parser
{
    public class ParseException : Exception
    {
        public Diagnostic Diagnostic { get; private set; }

        public ParseException(Diagnostic diagnostic)
            : base()
        {
            Diagnostic = diagnostic;
        }
    }

    public class IcarusParser
    {
        private static readonly Dictionary<string, Type> EnumCommentTypes = new()
        {
            { "DECLARE_TYPE",    typeof(DECLARE_TYPE) },
            { "CHANNELS",        typeof(CHANNELS) },
            { "PLAY_TYPES",      typeof(PLAY_TYPES) },
            { "AFFECT_TYPE",     typeof(AFFECT_TYPE) },
            { "CAMERA_COMMANDS", typeof(CAMERA_COMMANDS) },
            { "TagType",         typeof(TagType) },
        };

        private static readonly Dictionary<string, Func<Node[], FunctionNode>> FunctionMap = new()
        {
            { "use",        args => new Use(args) },
            { "print",      args => new Print(args) },
            { "flush",      args => new Flush(args) },
            { "rotate",     args => new Rotate(args) },
            { "waitsignal", args => new WaitSignal(args) },
            { "move",       args => new Move(args) },
            { "remove",     args => new Remove(args) },
            { "free",       args => new Free(args) },
            { "signal",     args => new Signal(args) },
            { "do",         args => new Do(args) },
            { "run",        args => new Run(args) },
            { "kill",       args => new Kill(args) },
            { "wait",       args => new Wait(args) },
            { "rem",        args => new Rem(args) },
            { "set",        args => new Set(args) },
            { "declare",    args => new Declare(args) },
            { "sound",      args => new Sound(args) },
            { "play",       args => new Play(args) },
            { "loop",       args => new Loop(args) },
            { "task",       args => new Task(args) },
            { "affect",     args => new Affect(args) },
            { "else",       args => new Else(args) },
            { "if",         args => new If(args) },
            { "camera",     args => new Camera(args) },
            { "dowait",     args => new DoWait(args) },
            { "get",        args => new Get(args) },
            { "tag",        args => new Tag(args) },
            { "random",     args => new Nodes.Random(args) },
        };

        private string[] lines;
        private int lineIndex;
        private string currentLine;
        private int pos;
        private bool convertComments;
        private bool includeRem;

        public List<Diagnostic> Diagnostics { get; } = new();

        public List<Node> Parse(string sourceText, bool convertComments = false, bool includeRem = true)
        {
            Diagnostics.Clear();

            lines = sourceText.Replace("\r\n", "\n").Split('\n'); // TODO clean up this mess
            lineIndex = -1;
            currentLine = string.Empty;
            pos = 0;

            this.convertComments = convertComments;
            this.includeRem = includeRem;
            var nodes = new List<Node>();

            while (ReadNextLine())
            {
                SkipWhitespaceAndComments(nodes);

                while (pos < currentLine.Length)
                {
                    try
                    {
                        var stmtNodes = ParseStatement();
                        if (stmtNodes == null)
                            break;

                        nodes.AddRange(stmtNodes);

                        SkipWhitespaceAndComments(nodes);
                    }
                    catch (ParseException)
                    {
                        continue; // swallow regular parsing error and attempt to continue reading
                    }
                }
            }

            return nodes;
        }

        private bool ReadNextLine()
        {
            lineIndex++;

            if (lineIndex >= lines.Length)
                return false;

            currentLine = lines[lineIndex];
            pos = 0;

            return true;
        }

        private List<Node> ParseStatement()
        {
            string keyword = ReadIdentifier();

            if (string.IsNullOrEmpty(keyword))
                return null;

            List<Node> nodes = new List<Node>();

            try
            {
                if (FunctionMap.TryGetValue(keyword, out var factory))
                {
                    var node = ParseMappedFunction(factory);

                    if (includeRem || !(node is Rem))
                    {
                        nodes.Add(node);
                    }
                }
                else
                {
                    var node = ParseMappedFunction(args => new GenericFunction(keyword, args));
                    nodes.Add(node);
                }
            }
            catch (ParseException ex)
            {
                if (ex.Diagnostic.Node != null)
                {
                    nodes.Add(ex.Diagnostic.Node);
                }

                //throw; // TODO does this make sense?
            }

            return nodes;
        }

        #region Block Parsers

        private void ParseBlockBody(BlockNode block)
        {
            // Look for { on current line or next line
            SkipWhitespaceAndComments();

            if (pos >= currentLine.Length)
            {
                ReadNextLine();

                SkipWhitespaceAndComments();
            }

            Expect('{', block);

            // Parse body: could be on same line or subsequent lines
            SkipWhitespaceAndComments(block.SubNodes);

            while (true)
            {
                if (pos < currentLine.Length && currentLine[pos] == '}')
                {
                    pos++; // consume }
                    break;
                }

                if (pos < currentLine.Length)
                {
                    var stmtNodes = ParseStatement();
                    if (stmtNodes == null)
                        break;

                    foreach (var node in stmtNodes)
                    {
                        block.SubNodes.Add(node);
                    }

                    SkipWhitespaceAndComments(block.SubNodes);
                }
                else
                {
                    if (!ReadNextLine())
                        break;

                    SkipWhitespaceAndComments(block.SubNodes);
                }
            }
        }

        #endregion

        #region Function Parsers

        private Node ParseMappedFunction(Func<Node[], FunctionNode> factory)
        {
            var node = factory(arg: null);

            Node[] args;
            try
            {
                args = ParseArgList(node);
            }
            catch(ParseException ex)
            {
                // TODO SET line/pos : ex.Diagnostic.Node
                throw;
            }

            node = factory(args);

            if (node is BlockNode block)
            {
                ParseBlockBody(block);
            }
            else
            {
                ExpectSemicolon(node);
            }

            return node;
        }

        private Node[] ParseArgList(Node parent)
        {
            Expect('(', parent);

            var args = new List<Node>();
            bool skippedComma = false;

            while (true)
            {
                SkipWhitespaceAndComments();

                // skip $ signs — they're BehavED editor markers, not semantic
                while (pos < currentLine.Length && currentLine[pos] == '$')
                {
                    pos++;
                    SkipWhitespaceAndComments();
                }

                if (pos >= currentLine.Length || currentLine[pos] == ')')
                    break;

                // skip commas between arguments
                if (args.Count > 0 && currentLine[pos] == ',')
                {
                    pos++;
                    skippedComma = true;
                    continue;
                }

                skippedComma = false;

                args.Add(ParseValueWithOptionalEnum(parent));
            }

            // this is technically nonsensical because we would have parsed it,
            // but it will ensure a diagnostic error if there's a trailing comma
            if (skippedComma)
            {
                args.Add(ParseValueWithOptionalEnum(parent));
            }

            Expect(')', parent);

            return args.ToArray();
        }

        private Node ParseInnerFunction(string name, Node parent)
        {
            if (FunctionMap.TryGetValue(name, out var factory))
                return factory(ParseArgList(parent));

            return new GenericFunction(name, ParseArgList(parent));
        }

        #endregion

        #region Value Parsing

        // When source has an identifier like FLOAT, ORIGIN, FLUSH — resolve to FloatValue
        // so CreateOrPassThrough produces EnumFloatValue matching the IBI parser output.
        // Some enum types (CHANNELS, PLAY_TYPES) are stored as identifiers in the IBI binary.
        private static readonly HashSet<Type> IdentifierEnumTypes = new HashSet<Type>
        {
            typeof(CHANNELS),
            typeof(PLAY_TYPES)
        };

        private static Node ResolveEnumIdentifier(Node value, Type enumType)
        {
            if (IdentifierEnumTypes.Contains(enumType))
                return value;

            if (value is IdentifierValue id)
            {
                var table = EnumTableFloat.FromEnum(enumType);

                if (table.HasEnum(id.IdentifierName))
                    return new FloatValue(table.GetValue(id.IdentifierName));
            }

            return value;
        }

        private Node ParseValueWithOptionalEnum(Node parent)
        {
            SkipWhitespaceAndComments();

            string enumComment = TryReadEnumComment();

            Node value = ParseValue(parent);

            if (enumComment != null && EnumCommentTypes.TryGetValue(enumComment, out var enumType))
            {
                value = ResolveEnumIdentifier(value, enumType);
                value = EnumValue.CreateOrPassThrough(value, enumType);
            }

            return value;
        }

        private Node ParseValue(Node parent)
        {
            SkipWhitespaceAndComments();

            if (pos >= currentLine.Length)
            {
                Diagnostics.Add(Diagnostic.ERR004_UnexpectedEndOfLine(parent));
                throw new ParseException(Diagnostics.Last());
            }

            char c = currentLine[pos];

            // Vector: < x y z > OR operator: < (less-than)
            if (c == '<')
            {
                if (IsVectorAhead())
                    return ParseVector(parent);

                pos++;
                return new OperatorNode(Operator.Lt);
            }

            // Operators: >, =, !
            if (c == '>')
            {
                pos++;
                return new OperatorNode(Operator.Gt);
            }
            if (c == '=')
            {
                pos++;
                return new OperatorNode(Operator.Eq);
            }
            if (c == '!')
            {
                pos++;
                return new OperatorNode(Operator.Ne);
            }

            // String: "..."
            if (c == '"')
                return ParseString(parent);

            // Number: digit, minus, or dot
            if (char.IsDigit(c) || c == '-' || c == '.')
                return ParseNumber();

            // Identifier: alpha or underscore
            if (char.IsLetter(c) || c == '_')
            {
                string ident = ReadIdentifier();

                SkipWhitespaceAndComments();

                if (pos < currentLine.Length && currentLine[pos] == '(')
                    return ParseInnerFunction(ident, parent);

                return new IdentifierValue(ident);
            }

            Diagnostics.Add(Diagnostic.ERR005_UnexpectedCharacter(parent));
            throw new ParseException(Diagnostics.Last());
        }

        private VectorValue ParseVector(Node parent)
        {
            Expect('<', parent);

            SkipWhitespaceAndComments();

            var x = ParseNumber();

            SkipWhitespaceAndComments();

            var y = ParseNumber();

            SkipWhitespaceAndComments();

            var z = ParseNumber();

            SkipWhitespaceAndComments();

            Expect('>', parent);

            return new VectorValue(x, y, z);
        }

        private StringValue ParseString(Node parent)
        {
            Expect('"', parent);

            int start = pos;
            while (pos < currentLine.Length && currentLine[pos] != '"')
            {
                pos++;
            }

            string value = currentLine.Substring(start, pos - start);

            Expect('"', parent);

            return new StringValue(value);
        }

        private Node ParseNumber()
        {
            SkipWhitespaceAndComments();

            int start = pos;
            bool hasDot = false;

            if (pos < currentLine.Length && currentLine[pos] == '-')
            {
                pos++;
            }

            while (pos < currentLine.Length && (char.IsDigit(currentLine[pos]) || currentLine[pos] == '.'))
            {
                if (currentLine[pos] == '.')
                {
                    hasDot = true;
                }

                pos++;
            }

            string numStr = currentLine.Substring(start, pos - start);

            if (hasDot)
            {
                float f = float.Parse(numStr, CultureInfo.InvariantCulture);

                return new FloatValue(f);
            }
            else
            {
                // Parse as integer, but return FloatValue for contexts where IBI uses float
                // Node constructors (Loop, Camera) will convert to IntegerValue as needed
                int i = int.Parse(numStr, CultureInfo.InvariantCulture);

                return new FloatValue(i);
            }
        }

        #endregion

        #region Tokenizer Helpers

        private void SkipWhitespaceAndComments()
        {
            SkipWhitespaceAndComments(null);
        }

        private void SkipWhitespaceAndComments(List<Node> nodes)
        {
            while (pos < currentLine.Length)
            {
                char c = currentLine[pos];

                if (c == ' ' || c == '\t')
                {
                    pos++;
                }
                else if (pos + 1 < currentLine.Length && c == '/' && currentLine[pos + 1] == '/')
                {
                    if (convertComments && includeRem && nodes != null)
                    {
                        string commentText = currentLine.Substring(pos + 2).Trim();
                        nodes.Add(new Rem(new StringValue(commentText)));
                    }

                    pos = currentLine.Length;
                }
                else if (pos + 1 < currentLine.Length && c == '/' && currentLine[pos + 1] == '*')
                {
                    // Block comment — could be /*@ENUM*/ or /*!*/
                    // Don't consume here; let TryReadEnumComment handle /*@..*/
                    // But DO consume /*!*/ silently
                    if (pos + 3 < currentLine.Length && currentLine[pos + 2] == '!' && currentLine[pos + 3] == '*')
                    {
                        // /*!*/ — skip
                        pos += 4;
                        if (pos < currentLine.Length && currentLine[pos] == '/')
                        {
                            pos++;
                        }
                    }
                    else if (pos + 2 < currentLine.Length && currentLine[pos + 2] == '@')
                    {
                        // /*@ENUM*/ — don't consume here, let TryReadEnumComment handle it
                        break;
                    }
                    else
                    {
                        // Regular block comment — skip, may span multiple lines
                        pos += 2;
                        while (true)
                        {
                            while (pos + 1 < currentLine.Length && !(currentLine[pos] == '*' && currentLine[pos + 1] == '/'))
                            {
                                pos++;
                            }

                            if (pos + 1 < currentLine.Length)
                            {
                                pos += 2; // consume */
                                break;
                            }

                            if (!ReadNextLine())
                                break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private string TryReadEnumComment()
        {
            SkipWhitespaceAndComments();

            if (pos + 3 < currentLine.Length && currentLine[pos] == '/' && currentLine[pos + 1] == '*' && currentLine[pos + 2] == '@')
            {
                pos += 3; // skip /*@
                int start = pos;

                while (pos + 1 < currentLine.Length && !(currentLine[pos] == '*' && currentLine[pos + 1] == '/'))
                {
                    pos++;
                }

                string enumName = currentLine.Substring(start, pos - start);
                pos += 2; // skip */

                SkipWhitespaceAndComments();

                return enumName;
            }
            return null;
        }

        private string ReadIdentifier()
        {
            SkipWhitespaceAndComments();

            if (pos >= currentLine.Length || !(char.IsLetter(currentLine[pos]) || currentLine[pos] == '_'))
                return null;

            int start = pos;
            while (pos < currentLine.Length && (char.IsLetterOrDigit(currentLine[pos]) || currentLine[pos] == '_'))
            {
                pos++;
            }

            return currentLine.Substring(start, pos - start);
        }

        /// <summary>
        /// Peeks ahead from current pos (which should be at '&lt;') to check if this is a vector: &lt; number number number &gt;
        /// Does not modify pos.
        /// </summary>
        private bool IsVectorAhead()
        {
            int p = pos + 1; // skip '<'

            for (int component = 0; component < 3; component++)
            {
                // skip spaces
                while (p < currentLine.Length && currentLine[p] == ' ') p++;

                if (p >= currentLine.Length)
                    return false;

                // expect a number: optional minus, digits, optional dot+digits
                if (!char.IsDigit(currentLine[p]) && currentLine[p] != '-' && currentLine[p] != '.')
                    return false;

                if (currentLine[p] == '-') p++;

                bool hasDigits = false;
                while (p < currentLine.Length && (char.IsDigit(currentLine[p]) || currentLine[p] == '.'))
                {
                    if (char.IsDigit(currentLine[p])) hasDigits = true;
                    p++;
                }

                if (!hasDigits)
                    return false;
            }

            // skip spaces before '>'
            while (p < currentLine.Length && currentLine[p] == ' ') p++;

            return p < currentLine.Length && currentLine[p] == '>';
        }

        private void Expect(char expected, Node parent)
        {
            SkipWhitespaceAndComments();

            if (pos >= currentLine.Length)
            {
                Diagnostics.Add(Diagnostic.ERR006_UnexpectedEndOfLine(parent, expected));
                throw new ParseException(Diagnostics.Last());
            }

            if (currentLine[pos] != expected)
            {
                Diagnostics.Add(Diagnostic.ERR007_UnexpectedEndOfLine(parent, expected, found: currentLine[pos]));
                throw new ParseException(Diagnostics.Last());
            }

            pos++;
        }

        private void ExpectSemicolon(Node parent)
        {
            SkipWhitespaceAndComments();

            if (pos < currentLine.Length && currentLine[pos] == ';')
            {
                pos++;
            }
            else
            {
                Diagnostics.Add(Diagnostic.WARN002_NoSemiColonFound(parent));
            }
        }

        #endregion
    }
}
