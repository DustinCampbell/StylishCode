using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace StylishCode.Tests
{
    public partial class MarkupCode
    {
        public class ParseResult
        {
            private readonly string input;
            private readonly string output;
            private readonly int? position;
            private readonly ImmutableDictionary<string, ImmutableArray<TextSpan>> spanMap;

            public ParseResult(string input, string output, int? position, ImmutableDictionary<string, ImmutableArray<TextSpan>> spanMap)
            {
                this.input = input;
                this.output = output;
                this.position = position;
                this.spanMap = spanMap;
            }

            public string Input
            {
                get { return this.input; }
            }

            public string Output
            {
                get { return this.output; }
            }

            public int? Position
            {
                get { return this.position; }
            }

            public ImmutableArray<TextSpan> GetSpans(string name = null)
            {
                return this.spanMap[name ?? string.Empty];
            }

            public IEnumerable<string> GetSpanNames()
            {
                return this.spanMap.Keys;
            }
        }
    }
}
