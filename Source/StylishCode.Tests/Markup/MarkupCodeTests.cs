using System;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace StylishCode.Tests.Markup
{
    [TestFixture]
    public class MarkupCodeTests
    {
        [Test]
        public void NoPositionOrSpans()
        {
            const string input = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(input));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.Empty);
        }

        [Test]
        public void PositionAtStart()
        {
            const string input = @"$$class C { void M() { int x = 0; } }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.EqualTo(0));
            Assert.That(result.GetSpanNames(), Is.Empty);
        }

        [Test]
        public void PositionAtEnd()
        {
            const string input = @"class C { void M() { int x = 0; } }$$";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.EqualTo(output.Length));
            Assert.That(result.GetSpanNames(), Is.Empty);
        }

        [Test]
        public void PositionInside()
        {
            const string input = @"class C { void M() {$$ int x = 0; } }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.EqualTo(20));
            Assert.That(result.GetSpanNames(), Is.Empty);
        }

        [Test]
        public void EmptySpanAtStart()
        {
            const string input = @"[||]class C { void M() { int x = 0; } }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, 0) }));
        }

        [Test]
        public void EmptySpanAtEnd()
        {
            const string input = @"class C { void M() { int x = 0; } }[||]";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(output.Length, output.Length) }));
        }

        [Test]
        public void EmptySpanInside()
        {
            const string input = @"class C { void M() {[||] int x = 0; } }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(20, 20) }));
        }

        [Test]
        public void SpanSurroundingInput()
        {
            const string input = @"[|class C { void M() { int x = 0; } }|]";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length) }));
        }

        [Test]
        public void SpanInside()
        {
            const string input = @"class C { [|void M() { int x = 0; }|] }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(10, 33) }));
        }

        [Test]
        public void NestedSpans()
        {
            const string input = @"[|class C { [|void M() { int x = 0; }|] }|]";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length), TextSpan.FromBounds(10, 33) }));
        }

        [Test]
        public void NestedSpansAndPosition()
        {
            const string input = @"[|class C { [|void M() {$$ int x = 0; }|] }|]";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.EqualTo(20));
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length), TextSpan.FromBounds(10, 33) }));
        }

        [Test]
        public void NamedSpanWithNoNameThrows()
        {
            const string input = @"{|:|}class C { void M() { int x = 0; } }";

            Assert.Throws<ArgumentException>(() => MarkupCode.Parse(input));
        }

        [Test]
        public void EmptyNamedSpanAtStart()
        {
            const string input = @"{|foo:|}class C { void M() { int x = 0; } }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, 0) }));
        }

        [Test]
        public void EmptyNamedSpanAtEnd()
        {
            const string input = @"class C { void M() { int x = 0; } }{|foo:|}";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(output.Length, output.Length) }));
        }

        [Test]
        public void EmptyNamedSpanInside()
        {
            const string input = @"class C { void M() {{|foo:|} int x = 0; } }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(20, 20) }));
        }

        [Test]
        public void NamedSpanSurroundingInput()
        {
            const string input = @"{|foo:class C { void M() { int x = 0; } }|}";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length) }));
        }

        [Test]
        public void NamedSpanInside()
        {
            const string input = @"class C { {|foo:void M() { int x = 0; }|} }";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(10, 33) }));
        }

        [Test]
        public void NestedNamedSpans()
        {
            const string input = @"{|foo:class C { {|foo:void M() { int x = 0; }|} }|}";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length), TextSpan.FromBounds(10, 33) }));
        }

        [Test]
        public void NestedNamedSpansAndPosition()
        {
            const string input = @"{|foo:class C { {|foo:void M() {$$ int x = 0; }|} }|}";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.EqualTo(20));
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length), TextSpan.FromBounds(10, 33) }));
        }

        [Test]
        public void NestedNamedAndNonNamedSpans()
        {
            const string input = @"{|foo:class C { [|void M() { int x = 0; }|] }|}";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty, "foo" }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(10, 33) }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length) }));
        }

        [Test]
        public void NestedNamedAndNonNamedSpansAndPosition()
        {
            const string input = @"{|foo:class C { [|void M() {$$ int x = 0; }|] }|}";
            const string output = @"class C { void M() { int x = 0; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.EqualTo(20));
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty, "foo" }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(10, 33) }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(0, output.Length) }));
        }

        [Test]
        public void HandlePotentialAmbiguity1()
        {
            const string input = @"class C { void M() { [|int[|]] x = null; } }";
            const string output = @"class C { void M() { int[] x = null; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { string.Empty }));
            Assert.That(result.GetSpans(), Is.EquivalentTo(new[] { TextSpan.FromBounds(21, 25) }));
        }

        [Test]
        public void HandlePotentialAmbiguity2()
        {
            const string input = @"class C { void M() { {|foo:int[|}] x = null; } }";
            const string output = @"class C { void M() { int[] x = null; } }";

            var result = MarkupCode.Parse(input);

            Assert.That(result.Input, Is.EqualTo(input));
            Assert.That(result.Output, Is.EqualTo(output));
            Assert.That(result.Position, Is.Null);
            Assert.That(result.GetSpanNames(), Is.EquivalentTo(new[] { "foo" }));
            Assert.That(result.GetSpans("foo"), Is.EquivalentTo(new[] { TextSpan.FromBounds(21, 25) }));
        }
    }
}
