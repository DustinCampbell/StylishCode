using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StylishCode
{
    [ExportDiagnosticAnalyzer("BracesNeededForControlBlocks", LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : IDiagnosticAnalyzer, ISyntaxNodeAnalyzer<SyntaxKind>
    {
        private static readonly DiagnosticDescriptor[] descriptors = new[]
        {
            DiagnosticDescriptors.BracesNeededForControlBlocks
        };

        private static readonly SyntaxKind[] syntaxKinds = new[]
        {
            SyntaxKind.IfStatement,
            SyntaxKind.WhileStatement,
            SyntaxKind.DoStatement
        };

        public IEnumerable<DiagnosticDescriptor> GetSupportedDiagnostics()
        {
            return descriptors;
        }

        public IEnumerable<SyntaxKind> SyntaxKindsOfInterest
        {
            get { return syntaxKinds; }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            if (TryAnalyzeIfStatement(node, addDiagnostic))
            {
                return;
            }

            if (TryAnalyzeWhileStatement(node, addDiagnostic))
            {
                return;
            }

            if (TryAnalyzeDoStatement(node, addDiagnostic))
            {
                return;
            }
        }

        private static bool TryAnalyzeIfStatement(SyntaxNode node, Action<Diagnostic> addDiagnostic)
        {
            var ifStatement = node as IfStatementSyntax;
            if (ifStatement == null)
            {
                return false;
            }

            if (StyleHelpers.NeedsBraces(ifStatement))
            {
                addDiagnostic(CreateDiagnostic("if", ifStatement.IfKeyword.GetLocation()));
            }

            if (ifStatement.Else != null &&
                StyleHelpers.NeedsBraces(ifStatement.Else))
            {
                addDiagnostic(CreateDiagnostic("else", ifStatement.Else.ElseKeyword.GetLocation()));
            }

            return true;
        }

        private static bool TryAnalyzeWhileStatement(SyntaxNode node, Action<Diagnostic> addDiagnostic)
        {
            var whileStatement = node as WhileStatementSyntax;
            if (whileStatement == null)
            {
                return false;
            }

            if (StyleHelpers.NeedsBraces(whileStatement))
            {
                addDiagnostic(CreateDiagnostic("while", whileStatement.WhileKeyword.GetLocation()));
            }

            return true;
        }

        private static bool TryAnalyzeDoStatement(SyntaxNode node, Action<Diagnostic> addDiagnostic)
        {
            var doStatement = node as DoStatementSyntax;
            if (doStatement == null)
            {
                return false;
            }

            if (StyleHelpers.NeedsBraces(doStatement))
            {
                addDiagnostic(CreateDiagnostic("do", doStatement.DoKeyword.GetLocation()));
            }

            return true;
        }

        private static Diagnostic CreateDiagnostic(string name, Location location)
        {
            return Diagnostic.Create(DiagnosticDescriptors.BracesNeededForControlBlocks, location, name);
        }
    }
}
