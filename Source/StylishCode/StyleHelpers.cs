using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StylishCode
{
    internal static class StyleHelpers
    {
        public static bool NeedsBraces(IfStatementSyntax ifStatement)
        {
            if (ifStatement == null)
            {
                throw new ArgumentNullException("ifStatement");
            }

            return ifStatement.Statement != null
                && !ifStatement.Statement.IsKind(SyntaxKind.Block);
        }

        public static bool NeedsBraces(ElseClauseSyntax elseClause)
        {
            if (elseClause == null)
            {
                throw new ArgumentNullException("ifStatement");
            }

            return elseClause.Statement != null
                && !elseClause.Statement.IsKind(SyntaxKind.Block)
                && !elseClause.Statement.IsKind(SyntaxKind.IfStatement);
        }
    }
}
