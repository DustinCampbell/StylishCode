using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StylishCode
{
    [ExportCodeFixProvider("Add braces to control blocks", LanguageNames.CSharp)]
    internal class CodeFixProvider : ICodeFixProvider
    {
        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            yield return DiagnosticIds.BracesNeededForControlBlocks;
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var token = root.FindToken(span.Start);

            switch (token.CSharpKind())
            {
                case SyntaxKind.IfKeyword:
                    var ifStatement = token.Parent as IfStatementSyntax;
                    if (ifStatement != null)
                    {
                        return new[] { CreateCodeAction(document, root, ifStatement, StyleHelpers.AddBraces) };
                    }

                    break;

                case SyntaxKind.ElseKeyword:
                    var elseClause = token.Parent as ElseClauseSyntax;
                    if (elseClause != null)
                    {
                        return new[] { CreateCodeAction(document, root, elseClause, StyleHelpers.AddBraces) };
                    }

                    break;

                case SyntaxKind.WhileKeyword:
                    var whileStatement = token.Parent as WhileStatementSyntax;
                    if (whileStatement != null)
                    {
                        return new[] { CreateCodeAction(document, root, whileStatement, StyleHelpers.AddBraces) };
                    }

                    break;

                case SyntaxKind.DoKeyword:
                    var doStatement = token.Parent as DoStatementSyntax;
                    if (doStatement != null)
                    {
                        return new[] { CreateCodeAction(document, root, doStatement, StyleHelpers.AddBraces) };
                    }

                    break;
            }

            return null;
        }

        private static CodeAction CreateCodeAction<TSyntaxNode>(
            Document document, SyntaxNode root, TSyntaxNode node, Func<TSyntaxNode, TSyntaxNode> nodeUpdater)
            where TSyntaxNode : SyntaxNode
        {
            var newRoot = root.ReplaceNode(node, nodeUpdater(node));
            return CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot));
        }
    }
}
