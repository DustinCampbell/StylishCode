using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StylishCode
{
    [ExportCodeFixProvider("Add blank line after block", LanguageNames.CSharp)]
    internal class AddBlankLineAfterBlockCodeFixProvider : ICodeFixProvider
    {
        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            yield return DiagnosticIds.ClosingCurlyBraceMustBeFollowedByBlankLine;
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var token = root.FindToken(span.Start);
            var block = token.Parent as BlockSyntax;

            if (block == null || !StyleHelpers.NeedsTrailingBlankLine(block))
            {
                return null;
            }

            var newBlock = StyleHelpers.AddTrailingBlankLine(block);
            var newRoot = root.ReplaceNode(block, newBlock);

            return new[] { CodeAction.Create("Add blank line", document.WithSyntaxRoot(newRoot)) };
        }
    }
}
