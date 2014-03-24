using Microsoft.CodeAnalysis;

namespace StylishCode
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor BracesNeededForControlBlocks =
           new DiagnosticDescriptor(
               id: DiagnosticIds.BracesNeededForControlBlocks,
               description: "Braces needed for control blocks",
               messageFormat: "{0} blocks require braces",
               category: "Code Style",
               defaultSeverity: DiagnosticSeverity.Warning);
    }
}
