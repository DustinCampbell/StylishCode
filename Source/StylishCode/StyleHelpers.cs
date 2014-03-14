using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

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

        public static IfStatementSyntax AddBraces(IfStatementSyntax ifStatement)
        {
            Debug.Assert(ifStatement != null && NeedsBraces(ifStatement));

            return ifStatement
                .WithStatement(SyntaxFactory.Block(ifStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        public static ElseClauseSyntax AddBraces(ElseClauseSyntax elseClause)
        {
            Debug.Assert(elseClause != null && NeedsBraces(elseClause));

            return elseClause
                .WithStatement(SyntaxFactory.Block(elseClause.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }
    }
}
