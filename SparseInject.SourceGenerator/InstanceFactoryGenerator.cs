using System;
using Microsoft.CodeAnalysis;

namespace SparseInject.SourceGenerator;

public static class InstanceFactoryGenerator
{
    public static bool TryGenerate(TypeMeta typeMeta,
        CodeWriter codeWriter, GeneratorExecutionContext context, out string resultGeneratorName,
        out string correctedTypeName)
    {
        correctedTypeName = null;
        resultGeneratorName = string.Empty;
        
        if (typeMeta.IsPrivate)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.PrivateTypesNotSupported,
                typeMeta.GetLocation(),
                typeMeta.TypeName));
                
            return false;
        }

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

        using (codeWriter.CreateClass(typeMeta,
                   new []
                   {
                       "int constructorParametersIndex"
                   },
                   new []
                   {
                       "ConstructorParametersIndex = constructorParametersIndex;",
                       $"ConstructorParametersCount = {typeMeta.ConstructorParameters.Length};"
                   },
                   out resultGeneratorName))
        {
            using (codeWriter.Scope("public override object Create(object[] arguments)"))
            {
                var parameters = typeMeta.ConstructorParameters;

                correctedTypeName = typeMeta.FullTypeName;
                
                if (typeMeta.GenericArgs != null)
                {
                    var typeNameSplitted = correctedTypeName.Split('.');
                    var typeNameLast = typeNameSplitted[typeNameSplitted.Length - 1];
                
                    var startIndex = typeNameLast.IndexOf('<');
                    var endIndex = typeNameLast.LastIndexOf('>');
                                
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        var stringToReplace = typeNameLast.Substring(startIndex + 1, endIndex - startIndex - 1);
                        typeNameLast = typeNameLast.Replace(stringToReplace, string.Join(",", typeMeta.GenericArgs));
                    }
                    
                    typeNameSplitted[typeNameSplitted.Length - 1] = typeNameLast;
                    
                    correctedTypeName = string.Join(".", typeNameSplitted);
                }
                
                if (constructorSymbol != null)
                {
                    var args = new string[parameters.Length];
                
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        args[i] = $"({parameters[i].paramType})(arguments[{i}])";
                    }

                    codeWriter.WriteLine($"return new {correctedTypeName}({string.Join(", ", args)});");
                }
                else
                {
                    codeWriter.WriteLine($"return new {correctedTypeName}();");
                }
            }
        }

        return true;
    }
    
    private static IDisposable CreateClass(this CodeWriter writer, TypeMeta typeMeta,
        string[] constructorParameters, string[] constructorLines,
        out string resultGeneratorName)
    {
        var typeSymbol = typeMeta.Symbol;
        
        var generatorName = string.Empty;
        resultGeneratorName = string.Empty;
        
        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
        IDisposable namespaceScope = new EmptyScope();
        
        if (!typeSymbol.ContainingNamespace.IsGlobalNamespace && !string.IsNullOrEmpty(namespaceName))
        {
            resultGeneratorName += namespaceName + ".";

            namespaceScope = writer.Scope($"namespace {namespaceName}");
        }
        
        var containingType = typeSymbol.ContainingType;

        while (containingType != null)
        {
            var containingTypeName = containingType.Name + "_";
            
            generatorName += containingTypeName;
            
            containingType = containingType.ContainingType;
        }
        
        var className = typeSymbol.Name;

        if (typeMeta.GenericArgs != null)
        {
            className += "_" + string.Join("_", typeMeta.GenericArgs);
        }
        
        generatorName += $"{className}_SparseInject_InstanceFactory";
        resultGeneratorName += generatorName;

        writer.WriteLine("#if UNITY_2017_1_OR_NEWER");
        writer.WriteLine("[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]");
        writer.WriteLine("[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]");
        writer.WriteLine("[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]");
        writer.WriteLine("[UnityEngine.Scripting.Preserve]");
        writer.WriteLine("#endif");
        var classScope = writer.Scope($"public class {generatorName} : InstanceFactoryBase");
        
        using (writer.Scope($"public {generatorName}({string.Join(",", constructorParameters)})"))
        {
            foreach (var constructorLine in constructorLines)
            {
                writer.WriteLine(constructorLine);
            }
        }

        return new CompositeScope(namespaceScope, classScope);
    }
}
