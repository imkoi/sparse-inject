using System;
using System.Linq;
using Microsoft.CodeAnalysis;
#if ROSLYN3
using SourceProductionContext = Microsoft.CodeAnalysis.GeneratorExecutionContext;
#endif

namespace VContainer.SourceGenerator;

static class Emitter
{
    public static bool TryEmitGeneratedInjector(
        TypeMeta typeMeta,
        CodeWriter codeWriter,
        GeneratorExecutionContext context)
    {
        if (typeMeta.IsGenerics)
        {
            return false; // TODO:
        }

        var ns = typeMeta.Symbol.ContainingNamespace;
        if (!ns.IsGlobalNamespace)
        {
            codeWriter.AppendLine($"namespace {ns}");
            codeWriter.BeginBlock();
        }

        var typeName = typeMeta.TypeName
            .Replace("global::", "")
            .Replace("<", "_")
            .Replace(">", "_");

        var generateTypeName = $"{typeName}_SparseInject_GeneratedInstanceFactory";

        codeWriter.AppendLine($"#if UNITY_EDITOR");
        codeWriter.AppendLine("[Preserve]");
        codeWriter.AppendLine("#endif");
        using (codeWriter.BeginBlockScope($"class {generateTypeName} : IInstanceFactory"))
        {
            if (!TryEmitGetConstructorParameters(typeMeta, codeWriter, context))
            {
                return false;
            }
            
            if (!TryEmitCreateInstanceMethod(typeMeta, codeWriter, context))
            {
                return false;
            }
        }

        if (!ns.IsGlobalNamespace)
        {
            codeWriter.EndBlock();
        }

        return true;
    }
    
    private static bool TryEmitGetConstructorParameters(
        TypeMeta typeMeta,
        CodeWriter codeWriter,
        GeneratorExecutionContext context)
    {
        var constructorSymbol = typeMeta.Constructors.OrderByDescending(ctor => ctor.Parameters.Length).FirstOrDefault();

        if (constructorSymbol != null)
        {
            if (!constructorSymbol.CanBeCallFromInternal())
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.PrivateConstructorNotSupported,
                    typeMeta.GetLocation(),
                    typeMeta.TypeName));
                return false;
            }

            if (constructorSymbol.Arity > 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.GenericsNotSupported,
                    typeMeta.GetLocation(),
                    typeMeta.TypeName));
                return false;
            }
        }
        
        var parameters = constructorSymbol != null ? constructorSymbol.Parameters
            .Select(param =>
            {
                var paramType =
                    param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                var paramName = param.Name;
                return (paramType, paramName);
            })
            .ToArray() : Array.Empty<(string paramType, string paramName)>();
        
        
        codeWriter.AppendLine($"public int ConstructorParametersCount => {parameters.Length};");
        codeWriter.AppendLine();

        using (codeWriter.BeginBlockScope("public Type[] GetConstructorParameters()"))
        {
            if (constructorSymbol != null)
            {
                var args = new string[parameters.Length];
                
                for (var i = 0; i < parameters.Length; i++)
                {
                    args[i] = $"typeof({parameters[i].paramType})";
                }

                var argsArray = "{" + string.Join(", ", args) + "}";

                codeWriter.AppendLine($"return new Type[]{argsArray};");
            }
            else
            {
                codeWriter.AppendLine("return null;");
            }
        }
        
        return true;
    }

    private static bool TryEmitCreateInstanceMethod(
        TypeMeta typeMeta,
        CodeWriter codeWriter,
        GeneratorExecutionContext context)
    {
        var constructorSymbol = typeMeta.Constructors.OrderByDescending(ctor => ctor.Parameters.Length).FirstOrDefault();

        if (constructorSymbol != null)
        {
            if (!constructorSymbol.CanBeCallFromInternal())
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.PrivateConstructorNotSupported,
                    typeMeta.GetLocation(),
                    typeMeta.TypeName));
                return false;
            }

            if (constructorSymbol.Arity > 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.GenericsNotSupported,
                    typeMeta.GetLocation(),
                    typeMeta.TypeName));
                return false;
            }
        }
        
        var parameters = constructorSymbol != null ? constructorSymbol.Parameters
            .Select(param =>
            {
                var paramType =
                    param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                var paramName = param.Name;
                return (paramType, paramName);
            })
            .ToArray() : Array.Empty<(string paramType, string paramName)>();

        using (codeWriter.BeginBlockScope("public object Create(object[] arguments)"))
        {
            if (constructorSymbol != null)
            {
                var args = new string[parameters.Length];
                
                for (var i = 0; i < parameters.Length; i++)
                {
                    args[i] = $"({parameters[i].paramType})arguments[{i}]";
                }

                codeWriter.AppendLine($"return new {typeMeta.TypeName}({string.Join(", ", args)});");
            }
            else
            {
                codeWriter.AppendLine($"return new {typeMeta.TypeName}();");
            }
        }
        
        return true;
    }
}
