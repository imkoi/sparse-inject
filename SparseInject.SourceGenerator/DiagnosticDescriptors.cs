using Microsoft.CodeAnalysis;

namespace SparseInject.SourceGenerator;

internal static class DiagnosticDescriptors
{
    private const string Category = "SparseInject.SourceGenerator";

    public static readonly DiagnosticDescriptor PrivateConstructorNotSupported = new(
        id: "SION0004",
        title: "The private constructor is not supported to code generation.",
        messageFormat: "Constructor of '{0}' is private. It cannot support source generator.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor GenericsConstructorNotSupported = new(
        id: "SION005",
        title: "Constructor that require generics argument is not supported to code generation.",
        messageFormat: "Constructor '{0}' use generic arguments.",
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

    public static readonly DiagnosticDescriptor PrivateTypesNotSupported = new(
        id: "SION0008",
        title: "The private types is not supported to code generation.",
        messageFormat: "The registered type '{0}' is private. It cannot support source generator.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
}