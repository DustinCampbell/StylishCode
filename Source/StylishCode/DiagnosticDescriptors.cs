using Microsoft.CodeAnalysis;

namespace StylishCode
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor BracesNeededForControlBlocks =
           new DiagnosticDescriptor(
               id: DiagnosticIds.BracesNeededForControlBlocks,
               kind: "Style",
               name: "Braces needed for control blocks",
               messageTemplate: "{0} blocks require braces",
               category: "Code Style",
               severity: DiagnosticSeverity.Warning);
    }
}
