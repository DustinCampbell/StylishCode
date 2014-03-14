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
                throw new ArgumentNullException("elseClause");
            }

            return elseClause.Statement != null
                && !elseClause.Statement.IsKind(SyntaxKind.Block)
                && !elseClause.Statement.IsKind(SyntaxKind.IfStatement);
        }

        public static bool NeedsBraces(WhileStatementSyntax whileStatement)
        {
            if (whileStatement == null)
            {
                throw new ArgumentNullException("whileStatement");
            }

            return whileStatement.Statement != null
                && !whileStatement.Statement.IsKind(SyntaxKind.Block);
        }

        public static bool NeedsBraces(DoStatementSyntax doStatement)
        {
            if (doStatement == null)
            {
                throw new ArgumentNullException("doStatement");
            }

            return doStatement.Statement != null
                && !doStatement.Statement.IsKind(SyntaxKind.Block);
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

        public static WhileStatementSyntax AddBraces(WhileStatementSyntax whileStatement)
        {
            Debug.Assert(whileStatement != null && NeedsBraces(whileStatement));

            return whileStatement
                .WithStatement(SyntaxFactory.Block(whileStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        public static DoStatementSyntax AddBraces(DoStatementSyntax doStatement)
        {
            Debug.Assert(doStatement != null && NeedsBraces(doStatement));

            return doStatement
                .WithStatement(SyntaxFactory.Block(doStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }
    }
}
