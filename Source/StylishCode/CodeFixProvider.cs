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
    [ExportCodeFixProvider("Add Braces", LanguageNames.CSharp)]
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
                        var newRoot = root.ReplaceNode(ifStatement, StyleHelpers.AddBraces(ifStatement));
                        return new[] { CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot)) };
                    }

                    break;

                case SyntaxKind.ElseKeyword:
                    var elseClause = token.Parent as ElseClauseSyntax;
                    if (elseClause != null)
                    {
                        var newRoot = root.ReplaceNode(elseClause, StyleHelpers.AddBraces(elseClause));
                        return new[] { CodeAction.Create("Add braces", document.WithSyntaxRoot(newRoot)) };
                    }

                    break;
            }

            return null;
        }
    }
}
