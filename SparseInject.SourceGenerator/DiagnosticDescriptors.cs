using Microsoft.CodeAnalysis;

namespace VContainer.SourceGenerator
{
    static class DiagnosticDescriptors
    {
        const string Category = "SparseInject.SourceGenerator";

        public static readonly DiagnosticDescriptor UnexpectedErrorDescriptor = new(
            id: "SION0001",
            title: "Unexpected error during generation",
            messageFormat: "Unexpected error occurred during code generation: {0}",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor AbstractNotAllow = new(
            id: "SION0002",
            title: "Injectable type must not be abstract/interface",
            messageFormat: "The injectable type of '{0}' is abstract/interface. It is not allowed",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NestedNotSupported = new(
            id: "SION0003",
            title: "Nested type is not support to code generation.",
            messageFormat: "The injectable object '{0}' is a nested type. It cannot support code generation ",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor PrivateConstructorNotSupported = new(
            id: "SION0004",
            title: "The private constructor is not supported to code generation.",
            messageFormat: "The injectable constructor of '{0}' is private. It cannot support source generator.",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor GenericsNotSupported = new(
            id: "SION005",
            title: "The [Inject] constructor or method that require generics argument is not supported to code generation.",
            messageFormat: "[Inject] '{0}' needs generic arguments. It cannot inject by the source generator.",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor AssemblyProcessingStarted = new(
            id: "SION006",
            title: "The Assembly is processing.",
            messageFormat: "Assembly '{0}' processing is started.",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor AssemblySourceGenerationFailed = new(
            id: "SION007",
            title: "The source generation failed.",
            messageFormat: "Assembly '{0}' source generation is failed.",
            category: Category,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
