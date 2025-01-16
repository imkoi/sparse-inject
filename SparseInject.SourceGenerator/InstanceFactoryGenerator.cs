using System;
using Microsoft.CodeAnalysis;

namespace SparseInject.SourceGenerator;

public static class InstanceFactoryGenerator
{
    public static int GeneratorIndex { get; set; }
    
    public static bool TryGenerate(TypeDefinition typeDefinition,
        CodeWriter codeWriter, GeneratorExecutionContext context, out string resultGeneratorName,
        out string correctedTypeName)
    {
        correctedTypeName = null;
        resultGeneratorName = string.Empty;
        
        if (typeDefinition.IsPrivate)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.PrivateTypesNotSupported,
                typeDefinition.GetLocation(),
                typeDefinition.TypeName));
                
            return false;
        }

        var constructorSymbol = typeDefinition.Constructor;

        if (constructorSymbol != null)
        {
            if (!(constructorSymbol.DeclaredAccessibility >= Accessibility.Internal)) // can be called from internal
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.PrivateConstructorNotSupported,
                    typeDefinition.GetLocation(),
                    typeDefinition.TypeName));
                
                return false;
            }

            if (constructorSymbol.Arity > 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.GenericsConstructorNotSupported,
                    typeDefinition.GetLocation(),
                    typeDefinition.TypeName));
                return false;
            }
        }

        using (codeWriter.CreateClass(typeDefinition,
                   new []
                   {
                       "int constructorParametersIndex"
                   },
                   new []
                   {
                       "ConstructorParametersIndex = constructorParametersIndex;",
                       $"ConstructorParametersCount = {typeDefinition.ConstructorParameters.Length};"
                   },
                   out resultGeneratorName))
        {
            using (codeWriter.Scope("public override object Create(object[] arguments)"))
            {
                var parameters = typeDefinition.ConstructorParameters;

                correctedTypeName = typeDefinition.FullTypeName;
                
                if (typeDefinition.GenericArgs != null)
                {
                    var typeNameSplitted = correctedTypeName.Split('.');
                    var typeNameLast = typeNameSplitted[typeNameSplitted.Length - 1];
                
                    var startIndex = typeNameLast.IndexOf('<');
                    var endIndex = typeNameLast.LastIndexOf('>');
                                
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        var stringToReplace = typeNameLast.Substring(startIndex + 1, endIndex - startIndex - 1);
                        typeNameLast = typeNameLast.Replace(stringToReplace, string.Join(",", typeDefinition.GenericArgs));
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
    
    private static IDisposable CreateClass(this CodeWriter writer, TypeDefinition typeDefinition,
        string[] constructorParameters, string[] constructorLines,
        out string resultGeneratorName)
    {
        var typeSymbol = typeDefinition.Symbol;
        
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
        
        var className = GeneratorIndex.ToString();
        GeneratorIndex++;

        generatorName += $"SparseInject_InstanceFactory_{className}";
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
