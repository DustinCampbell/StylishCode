using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace StylishCode.Tests
{
    [TestFixture]
    public class CurlyBracesNeededDiagnosticAnalyzerTests : SyntaxNodeAnalyzerTestFixture<SyntaxKind>
    {
        protected override string Language
        {
            get { return LanguageNames.CSharp; }
        }

        protected override ISyntaxNodeAnalyzer<SyntaxKind> CreateAnalyzer()
        {
            return new CurlyBracesNeededDiagnosticAnalyzer();
        }

        [Test]
        public void IfWithoutCurlies()
        {
            const string code = @"
class C
{
    void M(int x)
    {
        {|SC0001:if|} (x > 0)
            return;
    }
}";

            Exists(code, root => root.Members[0].Members[0].Body.Statements[0]);
        }
    }
}
