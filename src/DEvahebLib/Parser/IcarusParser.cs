using System;
using System.Collections.Generic;
using System.Globalization;
using DEvahebLib.Enums;
using DEvahebLib.Nodes;

namespace DEvahebLib.Parser
{
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

        public List<Node> Parse(string sourceText, bool convertComments = false)
        {
            lines = sourceText.Replace("\r\n", "\n").Split('\n');
            lineIndex = -1;
            currentLine = string.Empty;
            pos = 0;

            this.convertComments = convertComments;
            var nodes = new List<Node>();

            while (ReadNextLine())
            {
                SkipWhitespaceAndComments(nodes);

                while (pos < currentLine.Length)
                {
                    var stmtNodes = ParseStatement();
                    if (stmtNodes == null)
                        break;

                    nodes.AddRange(stmtNodes);

                    SkipWhitespaceAndComments(nodes);
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

            if (FunctionMap.TryGetValue(keyword, out var factory))
                return new List<Node> { ParseMappedFunction(factory) };

            return new List<Node> { ParseMappedFunction(args => new GenericFunction(keyword, args)) };
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

            Expect('{');

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
            var args = ParseArgList();

            var node = factory(args);

            if (node is BlockNode block)
            {
                ParseBlockBody(block);
            }
            else
            {
                ExpectSemicolon();
            }

            return node;
        }

        private Node[] ParseArgList()
        {
            Expect('(');

            var args = new List<Node>();

            SkipWhitespaceAndComments();

            if (pos < currentLine.Length && currentLine[pos] != ')')
            {
                args.Add(ParseArgumentValue());

                while (true)
                {
                    SkipWhitespaceAndComments();

                    if (pos >= currentLine.Length || currentLine[pos] == ')')
                        break;

                    if (currentLine[pos] == ',')
                    {
                        pos++;
                        args.Add(ParseArgumentValue());
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Expect(')');

            return args.ToArray();
        }

        private Node ParseInnerFunction(string name)
        {
            if (FunctionMap.TryGetValue(name, out var factory))
                return factory(ParseArgList());

            return new GenericFunction(name, ParseArgList());
        }

        #endregion

        #region Value Parsing

        private Node ParseArgumentValue()
        {
            SkipWhitespaceAndComments();

            // Handle $-expression wrapper (BehavED format)
            if (pos < currentLine.Length && currentLine[pos] == '$')
            {
                pos++; // consume $

                SkipWhitespaceAndComments();

                Node result;
                string word = PeekIdentifier();

                if (word != null)
                {
                    ReadIdentifier();
                    SkipWhitespaceAndComments();

                    if (pos < currentLine.Length && currentLine[pos] == '(')
                        result = ParseInnerFunction(word);
                    else
                        result = new IdentifierValue(word);
                }
                else
                {
                    result = ParseValue();
                }

                SkipWhitespaceAndComments();

                Expect('$');

                return result;
            }

            return ParseValueWithOptionalEnum();
        }

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

        private Node ParseValueWithOptionalEnum()
        {
            SkipWhitespaceAndComments();

            string enumComment = TryReadEnumComment();

            Node value = ParseValue();

            if (enumComment != null && EnumCommentTypes.TryGetValue(enumComment, out var enumType))
            {
                value = ResolveEnumIdentifier(value, enumType);
                value = EnumValue.CreateOrPassThrough(value, enumType);
            }

            return value;
        }

        private Node ParseValue()
        {
            SkipWhitespaceAndComments();

            if (pos >= currentLine.Length)
                throw new Exception($"Unexpected end of line {lineIndex + 1} while parsing value");

            char c = currentLine[pos];

            // Vector: < x y z > OR operator: < (less-than)
            if (c == '<')
            {
                // Peek ahead to determine if this is a vector or operator
                int next = pos + 1;
                while (next < currentLine.Length && currentLine[next] == ' ') next++;
                if (next < currentLine.Length && (char.IsDigit(currentLine[next]) || currentLine[next] == '-' || currentLine[next] == '.'))
                    return ParseVector();
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
                return ParseString();

            // Number: digit, minus, or dot
            if (char.IsDigit(c) || c == '-' || c == '.')
                return ParseNumber();

            // Identifier: alpha or underscore
            if (char.IsLetter(c) || c == '_')
            {
                string ident = ReadIdentifier();

                SkipWhitespaceAndComments();

                if (pos < currentLine.Length && currentLine[pos] == '(')
                    return ParseInnerFunction(ident);

                return new IdentifierValue(ident);
            }

            throw new Exception($"Unexpected character '{c}' on line {lineIndex + 1}, column {pos + 1}");
        }

        private VectorValue ParseVector()
        {
            Expect('<');

            SkipWhitespaceAndComments();

            var x = ParseNumber();

            SkipWhitespaceAndComments();

            var y = ParseNumber();

            SkipWhitespaceAndComments();

            var z = ParseNumber();

            SkipWhitespaceAndComments();

            Expect('>');

            return new VectorValue(x, y, z);
        }

        private StringValue ParseString()
        {
            Expect('"');

            int start = pos;
            while (pos < currentLine.Length && currentLine[pos] != '"')
            {
                pos++;
            }

            string value = currentLine.Substring(start, pos - start);

            Expect('"');

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
                    if (convertComments && nodes != null)
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

        private string PeekIdentifier()
        {
            int savedPos = pos;
            string result = ReadIdentifier();
            pos = savedPos;

            return result;
        }

        private void Expect(char expected)
        {
            SkipWhitespaceAndComments();

            if (pos >= currentLine.Length)
                throw new Exception($"Expected '{expected}' but reached end of line {lineIndex + 1}");

            if (currentLine[pos] != expected)
                throw new Exception($"Expected '{expected}' but found '{currentLine[pos]}' on line {lineIndex + 1}, column {pos + 1}");

            pos++;
        }

        private bool TryConsume(char c)
        {
            SkipWhitespaceAndComments();

            if (pos < currentLine.Length && currentLine[pos] == c)
            {
                pos++;

                return true;
            }

            return false;
        }

        private void ExpectComma()
        {
            SkipWhitespaceAndComments();

            if (pos < currentLine.Length && currentLine[pos] == ',')
            {
                pos++;
            }
        }

        private void ExpectSemicolon()
        {
            SkipWhitespaceAndComments();

            if (pos < currentLine.Length && currentLine[pos] == ';')
            {
                pos++;
            }
        }

        #endregion
    }
}
