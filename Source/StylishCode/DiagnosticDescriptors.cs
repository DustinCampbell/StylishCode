using Microsoft.CodeAnalysis;

namespace StylishCode
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor CurlyBracesNeededForControlBlocks =
           new DiagnosticDescriptor(
               id: DiagnosticIds.CurlyBracesNeededForControlBlocks,
               description: "Braces needed for control blocks",
               messageFormat: "{0} blocks require braces",
               category: "Code Style",
               defaultSeverity: DiagnosticSeverity.Warning);

        public static readonly DiagnosticDescriptor ClosingCurlyBraceMustBeFollowedByBlankLine =
           new DiagnosticDescriptor(
               id: DiagnosticIds.ClosingCurlyBraceMustBeFollowedByBlankLine,
               description: "Closing curly brace must be followed by a blank line",
               messageFormat: "Closing curly brace must be followed by a blank line",
               category: "Code Style",
               defaultSeverity: DiagnosticSeverity.Warning);
    }
}
