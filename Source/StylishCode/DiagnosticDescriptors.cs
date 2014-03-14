using Microsoft.CodeAnalysis;

namespace StylishCode
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor BracesNeededForControlBlocks =
           new DiagnosticDescriptor(
               id: DiagnosticIds.BracesNeededForControlBlocks,
               kind: "Compiler",
               name: "Braces needed for control blocks",
               messageTemplate: "{0} blocks require braces",
               category: "Style",
               severity: DiagnosticSeverity.Warning);
    }
}
