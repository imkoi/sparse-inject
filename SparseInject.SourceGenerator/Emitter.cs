using Microsoft.CodeAnalysis;

namespace SparseInject.SourceGenerator;

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
        
        var constructorSymbol = typeMeta.Constructor;

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

        var generateTypeName = $"{typeName}_SparseInject_GeneratedInstanceFactory";

        codeWriter.AppendLine("#if UNITY_EDITOR");
        codeWriter.AppendLine("[Il2CppSetOption(Option.NullChecks, false)]");
        codeWriter.AppendLine("[Il2CppSetOption(Option.DivideByZeroChecks, false)]");
        codeWriter.AppendLine("[Il2CppSetOption(Option.ArrayBoundsChecks, false)]");
        codeWriter.AppendLine("[Preserve]");
        codeWriter.AppendLine("#endif");
        using (codeWriter.BeginBlockScope($"class {generateTypeName} : InstanceFactoryBase"))
        {
            using (codeWriter.BeginBlockScope($"public {generateTypeName}(int constructorParametersIndex)"))
            {
                codeWriter.AppendLine("ConstructorParametersIndex = constructorParametersIndex;");
                codeWriter.AppendLine($"ConstructorParametersCount = {typeMeta.ConstructorParameters.Length};");
            }
            
            using (codeWriter.BeginBlockScope("public override object Create(object[] arguments)"))
            {
                var parameters = typeMeta.ConstructorParameters;
                
                if (constructorSymbol != null)
                {
                    var args = new string[parameters.Length];
                
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        args[i] = $"Unsafe.As<{parameters[i].paramType}>(arguments[{i}])";
                    }

                    codeWriter.AppendLine($"return new {typeMeta.TypeName}({string.Join(", ", args)});");
                }
                else
                {
                    codeWriter.AppendLine($"return new {typeMeta.TypeName}();");
                }
            }
        }

        if (!ns.IsGlobalNamespace)
        {
            codeWriter.EndBlock();
        }

        return true;
    }
}
