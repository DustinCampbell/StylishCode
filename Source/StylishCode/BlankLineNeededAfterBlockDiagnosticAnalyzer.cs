using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StylishCode
{
    [ExportDiagnosticAnalyzer("ClosingCurlyBraceMustBeFollowedByBlankLine", LanguageNames.CSharp)]
    public class BlankLineNeededAfterBlockDiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        private static readonly ImmutableArray<DiagnosticDescriptor> descriptors = ImmutableArray.Create(new[]
        {
            DiagnosticDescriptors.ClosingCurlyBraceMustBeFollowedByBlankLine
        });

        private static readonly ImmutableArray<SyntaxKind> syntaxKinds = ImmutableArray.Create(new[]
        {
            SyntaxKind.Block
        });

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return descriptors; }
        }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get { return syntaxKinds; }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            var block = (BlockSyntax)node;

            if (StyleHelpers.NeedsTrailingBlankLine(block))
            {
                addDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.ClosingCurlyBraceMustBeFollowedByBlankLine,
                    block.CloseBraceToken.GetLocation()));
            }
        }
    }
}
