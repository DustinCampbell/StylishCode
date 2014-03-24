using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StylishCode
{
    [ExportDiagnosticAnalyzer("CurlyBracesNeededForControlBlocks", LanguageNames.CSharp)]
    public class CurlyBracesNeededDiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        private static readonly ImmutableArray<DiagnosticDescriptor> descriptors = ImmutableArray.Create(new[]
        {
            DiagnosticDescriptors.CurlyBracesNeededForControlBlocks
        });

        private static readonly ImmutableArray<SyntaxKind> syntaxKinds = ImmutableArray.Create(new[]
        {
            SyntaxKind.IfStatement,
            SyntaxKind.WhileStatement,
            SyntaxKind.DoStatement,
            SyntaxKind.ForStatement,
            SyntaxKind.ForEachStatement,
            SyntaxKind.LockStatement
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

            if (TryAnalyzeForStatement(node, addDiagnostic))
            {
                return;
            }

            if (TryAnalyzeForEachStatement(node, addDiagnostic))
            {
                return;
            }

            if (TryAnalyzeLockStatement(node, addDiagnostic))
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

        private static bool TryAnalyzeForStatement(SyntaxNode node, Action<Diagnostic> addDiagnostic)
        {
            var forStatement = node as ForStatementSyntax;
            if (forStatement == null)
            {
                return false;
            }

            if (StyleHelpers.NeedsBraces(forStatement))
            {
                addDiagnostic(CreateDiagnostic("for", forStatement.ForKeyword.GetLocation()));
            }

            return true;
        }

        private static bool TryAnalyzeForEachStatement(SyntaxNode node, Action<Diagnostic> addDiagnostic)
        {
            var forEachStatement = node as ForEachStatementSyntax;
            if (forEachStatement == null)
            {
                return false;
            }

            if (StyleHelpers.NeedsBraces(forEachStatement))
            {
                addDiagnostic(CreateDiagnostic("foreach", forEachStatement.ForEachKeyword.GetLocation()));
            }

            return true;
        }

        private static bool TryAnalyzeLockStatement(SyntaxNode node, Action<Diagnostic> addDiagnostic)
        {
            var lockStatement = node as LockStatementSyntax;
            if (lockStatement == null)
            {
                return false;
            }

            if (StyleHelpers.NeedsBraces(lockStatement))
            {
                addDiagnostic(CreateDiagnostic("lock", lockStatement.LockKeyword.GetLocation()));
            }

            return true;
        }

        private static Diagnostic CreateDiagnostic(string name, Location location)
        {
            return Diagnostic.Create(DiagnosticDescriptors.CurlyBracesNeededForControlBlocks, location, name);
        }
    }
}
