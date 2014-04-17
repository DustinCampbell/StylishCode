using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using StylishCode.Tests.Markup;

namespace StylishCode.Tests
{
    public abstract class SyntaxNodeAnalyzerTestFixture<TSyntaxKind>
    {
        private static readonly MetadataReference mscorlib = new MetadataFileReference(typeof(int).Assembly.Location);

        protected abstract string Language { get; }
        protected abstract ISyntaxNodeAnalyzer<TSyntaxKind> CreateAnalyzer();

        protected void Exists(string markupCode, Func<dynamic, dynamic> nodeLocator)
        {
            var result = MarkupCode.Parse(markupCode);

            var workspace = new CustomWorkspace();

            var project = workspace.CurrentSolution.AddProject("Test", "Test", this.Language);
            project = project.AddMetadataReference(mscorlib);

            var document = project.AddDocument("Test.cs", result.Output);

            var semanticModel = document.GetSemanticModelAsync(CancellationToken.None).Result;
            var syntaxRoot = semanticModel.SyntaxTree.GetRoot(CancellationToken.None);

            var analyzer = CreateAnalyzer();
            var node = (SyntaxNode)nodeLocator(syntaxRoot);
            var diagnosticList = new List<Diagnostic>();

            analyzer.AnalyzeNode(node, semanticModel, d => diagnosticList.Add(d), CancellationToken.None);

            var diagnosticsById = diagnosticList.GroupBy(d => d.Id);

            foreach (var diagnostics in diagnosticsById)
            {
                var spans = result.GetSpans(diagnostics.Key).OrderBy(span => span.Start).ToArray();
                var diagnosticSpans = diagnostics.Select(d => d.Location.SourceSpan).OrderBy(span => span.Start).ToArray();

                Assert.That(diagnosticSpans, Is.EquivalentTo(spans));
            }
        }
    }
}
