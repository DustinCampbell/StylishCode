using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace StylishCode
{
    [ExportCodeFixProvider("Add Braces", LanguageNames.CSharp)]
    public class CodeFixProvider : ICodeFixProvider
    {
        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            yield return "SC0001";
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var token = root.FindToken(span.Start);

            if (token.IsKind(SyntaxKind.IfKeyword))
            {
                var ifStatement = token.Parent as IfStatementSyntax;
                if (ifStatement != null)
                {
                    return new[] { CreateCodeAction(document, root, ifStatement, AddBraces(ifStatement), cancellationToken) };
                }
            }

            if (token.IsKind(SyntaxKind.ElseKeyword))
            {
                var elseClause = token.Parent as ElseClauseSyntax;
                if (elseClause != null)
                {
                    return new[] { CreateCodeAction(document, root, elseClause, AddBraces(elseClause), cancellationToken) };
                }
            }

            return null;
        }

        private static CodeAction CreateCodeAction(
            Document document,
            SyntaxNode root,
            SyntaxNode oldNode,
            SyntaxNode newNode,
            CancellationToken cancellationToken)
        {
            var newRoot = root.ReplaceNode(oldNode, newNode);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return CodeAction.Create("Add braces", newDocument);
        }

        private static IfStatementSyntax AddBraces(IfStatementSyntax ifStatement)
        {
            Debug.Assert(ifStatement != null && StyleHelpers.NeedsBraces(ifStatement));

            return ifStatement
                .WithStatement(SyntaxFactory.Block(ifStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        private static ElseClauseSyntax AddBraces(ElseClauseSyntax elseClause)
        {
            Debug.Assert(elseClause != null && StyleHelpers.NeedsBraces(elseClause));

            return elseClause
                .WithStatement(SyntaxFactory.Block(elseClause.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }
    }
}
