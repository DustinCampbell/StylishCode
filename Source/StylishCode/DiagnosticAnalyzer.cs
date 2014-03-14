using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StylishCode
{
    [ExportDiagnosticAnalyzer("NeedToAddBraces", LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : IDiagnosticAnalyzer, ISyntaxNodeAnalyzer<SyntaxKind>
    {
        private const string Id = "SC0001";

        private static readonly DiagnosticDescriptor descriptor =
            new DiagnosticDescriptor(Id, "Compiler", "NeedToAddBraces", "Need to add braces", "Style", DiagnosticSeverity.Warning);

        private static readonly SyntaxKind[] syntaxKinds = new[] { SyntaxKind.IfStatement };

        public IEnumerable<DiagnosticDescriptor> GetSupportedDiagnostics()
        {
            return new[] { descriptor };
        }

        public IEnumerable<SyntaxKind> SyntaxKindsOfInterest
        {
            get { return syntaxKinds; }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            var ifStatement = (IfStatementSyntax)node;
            if (StyleHelpers.NeedsBraces(ifStatement))
            {
                addDiagnostic(Diagnostic.Create(descriptor, ifStatement.IfKeyword.GetLocation()));
            }

            if (ifStatement.Else != null &&
                StyleHelpers.NeedsBraces(ifStatement.Else))
            {
                addDiagnostic(Diagnostic.Create(descriptor, ifStatement.Else.ElseKeyword.GetLocation()));
            }
        }
    }
}
