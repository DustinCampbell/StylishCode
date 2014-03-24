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

        public static bool NeedsBraces(ForStatementSyntax forStatement)
        {
            if (forStatement == null)
            {
                throw new ArgumentNullException("forStatement");
            }

            return forStatement.Statement != null
                && !forStatement.Statement.IsKind(SyntaxKind.Block);
        }

        public static bool NeedsBraces(LockStatementSyntax lockStatement)
        {
            if (lockStatement == null)
            {
                throw new ArgumentNullException("lockStatement");
            }

            return lockStatement.Statement != null
                && !lockStatement.Statement.IsKind(SyntaxKind.Block);
        }

        public static bool NeedsBraces(ForEachStatementSyntax forEachStatement)
        {
            if (forEachStatement == null)
            {
                throw new ArgumentNullException("forEachStatement");
            }

            return forEachStatement.Statement != null
                && !forEachStatement.Statement.IsKind(SyntaxKind.Block);
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

        public static ForStatementSyntax AddBraces(ForStatementSyntax forStatement)
        {
            Debug.Assert(forStatement != null && NeedsBraces(forStatement));

            return forStatement
                .WithStatement(SyntaxFactory.Block(forStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        public static ForEachStatementSyntax AddBraces(ForEachStatementSyntax forEachStatement)
        {
            Debug.Assert(forEachStatement != null && NeedsBraces(forEachStatement));

            return forEachStatement
                .WithStatement(SyntaxFactory.Block(forEachStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        public static LockStatementSyntax AddBraces(LockStatementSyntax lockStatement)
        {
            Debug.Assert(lockStatement != null && NeedsBraces(lockStatement));

            return lockStatement
                .WithStatement(SyntaxFactory.Block(lockStatement.Statement))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        public static bool NeedsTrailingBlankLine(BlockSyntax block)
        {
            if (block == null)
            {
                throw new ArgumentNullException("block");
            }

            var closeBrace = block.CloseBraceToken;

            // Check to see if the closing brace is really there.
            if (closeBrace.IsKind(SyntaxKind.None))
            {
                return false;
            }

            var lines = block.GetText().Lines;
            var closeBraceLine = lines.GetLineFromPosition(closeBrace.Span.End);

            // If the close brace line is the last line, it doesn't need a trailing blank line.
            if (closeBraceLine.LineNumber + 1 == lines.Count)
            {
                return false;
            }

            // If the next token is also a close brace token and is on the next line,
            // we don't need a trailing blank line.
            var nextToken = closeBrace.GetNextToken();
            if (nextToken.IsKind(SyntaxKind.CloseBraceToken))
            {
                var nextTokenLine = lines.GetLineFromPosition(nextToken.Span.Start);
                if (nextTokenLine.LineNumber - 1 == closeBraceLine.LineNumber)
                {
                    return false;
                }
            }

            // If the line after the closing brace is not whitespace, we need a trailing line.
            var nextLine = lines[closeBraceLine.LineNumber + 1];

            return !string.IsNullOrWhiteSpace(nextLine.ToString());
        }

        public static BlockSyntax AddTrailingBlankLine(BlockSyntax block)
        {
            Debug.Assert(block != null && NeedsTrailingBlankLine(block));

            return block
                .WithTrailingTrivia(block.GetTrailingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }
    }
}
