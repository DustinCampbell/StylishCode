using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace StylishCode.Tests
{
    public partial class MarkupCode
    {
        private StringBuilder nameBuilder;
        private StringBuilder outputBuilder;
        private int? position;
        private Dictionary<string, List<TextSpan>> nameToSpansMap;
        private Stack<Tuple<int, string>> spanStack;

        private MarkupCode() { }

        private bool IsSpanStackEmpty()
        {
            return this.spanStack == null || this.spanStack.Count == 0;
        }

        private static bool IsPositionDelimiter(char current, char next)
        {
            return current == '$' && next == '$';
        }

        private static bool IsSpanStartDelimiter(char current, char next)
        {
            return current == '[' && next == '|';
        }

        private static bool IsSpanEndDelimiter(char current, char next)
        {
            return current == '|' && next == ']';
        }

        private static bool IsNamedSpanStartDelimiter(char current, char next)
        {
            return current == '{' && next == '|';
        }

        private static bool IsNamedSpanEndDelimiter(char current, char next)
        {
            return current == '|' && next == '}';
        }

        private void PushSpanStart(string name = null)
        {
            if (this.spanStack == null)
            {
                this.spanStack = new Stack<Tuple<int, string>>();
            }

            var startIndex = this.outputBuilder.Length;
            name = name ?? string.Empty;

            var pair = Tuple.Create(startIndex, name);

            this.spanStack.Push(pair);
        }

        private void PopAndAddSpan()
        {
            var pair = this.spanStack.Pop();
            var startIndex = pair.Item1;
            var name = pair.Item2;
            var endIndex = this.outputBuilder.Length;

            var span = TextSpan.FromBounds(startIndex, endIndex);

            List<TextSpan> spanList;

            if (this.nameToSpansMap == null)
            {
                this.nameToSpansMap = new Dictionary<string, List<TextSpan>>();
            }

            if (!this.nameToSpansMap.TryGetValue(name, out spanList))
            {
                spanList = new List<TextSpan>();
                this.nameToSpansMap.Add(name, spanList);
            }

            spanList.Add(span);
        }

        private ParseResult ParseCore(string input)
        {
            this.nameBuilder = null; // Only allocate this when needed.
            this.outputBuilder = new StringBuilder();
            this.position = null;
            this.nameToSpansMap = null; // Only allocate this when needed.
            this.spanStack = null; // Only allocate this when needed.

            var inputIndex = 0;

            while (inputIndex < input.Length - 1)
            {
                var current = input[inputIndex];
                var next = input[inputIndex + 1];

                if (IsPositionDelimiter(current, next))
                {
                    if (position != null)
                    {
                        throw new ArgumentException("input has multiple occurrences of $$.", "input");
                    }

                    position = outputBuilder.Length;
                    inputIndex += 2;
                }
                else if (IsSpanStartDelimiter(current, next))
                {
                    PushSpanStart();
                    inputIndex += 2;
                }
                else if (IsSpanEndDelimiter(current, next))
                {
                    if (IsSpanStackEmpty())
                    {
                        throw new ArgumentException("Encountered span end delimiter with no span start delimiter.", "input");
                    }

                    if (this.spanStack.Peek().Item2.Length > 0)
                    {
                        throw new ArgumentException("Encountered named span end delimiter with no named span start delimiter.", "input");
                    }

                    PopAndAddSpan();

                    inputIndex += 2;
                }
                else if (IsNamedSpanStartDelimiter(current, next))
                {
                    this.nameBuilder = this.nameBuilder == null
                        ? new StringBuilder()
                        : this.nameBuilder.Clear();

                    inputIndex += 2;

                    // Is the next character ':'? If so, throw -- a name is required.
                    if (inputIndex < input.Length && input[inputIndex] == ':')
                    {
                        throw new ArgumentException("Encounted named span with no name.", "input");
                    }

                    // Read name
                    while (inputIndex < input.Length)
                    {
                        var ch = input[inputIndex];

                        if (ch != ':')
                        {
                            this.nameBuilder.Append(ch);
                        }

                        inputIndex++;

                        if (ch == ':')
                        {
                            break;
                        }
                    }

                    PushSpanStart(this.nameBuilder.ToString());
                }
                else if (IsNamedSpanEndDelimiter(current, next))
                {
                    if (IsSpanStackEmpty())
                    {
                        throw new ArgumentException("Encountered named span end delimiter with no named span start delimiter.", "input");
                    }

                    if (this.spanStack.Peek().Item2.Length == 0)
                    {
                        throw new ArgumentException("Encountered span end delimiter with no span start delimiter.", "input");
                    }

                    PopAndAddSpan();

                    inputIndex += 2;
                }
                else
                {
                    outputBuilder.Append(current);
                    inputIndex++;
                }
            }

            if (!IsSpanStackEmpty())
            {
                throw new ArgumentException("input contains spans with no end.", "input");
            }

            if (inputIndex < input.Length)
            {
                outputBuilder.Append(input.Substring(inputIndex));
            }

            return new ParseResult(
                input,
                this.outputBuilder.ToString(),
                this.position,
                this.nameToSpansMap != null
                    ? this.nameToSpansMap.ToImmutableDictionary(
                        keySelector: kvp => kvp.Key,
                        elementSelector: kvp => kvp.Value.ToImmutableArray())
                    : ImmutableDictionary<string, ImmutableArray<TextSpan>>.Empty);
        }

        public static ParseResult Parse(string input)
        {
            return new MarkupCode().ParseCore(input);
        }
    }
}
