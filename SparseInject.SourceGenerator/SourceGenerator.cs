using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SparseInject.SourceGenerator;

[Generator]
public class SourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new RegisterSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var sparseInjectReferenced = false;

        foreach (var referencedAssembly in context.Compilation.ReferencedAssemblyNames)
        {
            if (referencedAssembly.Name == "SparseInject")
            {
                sparseInjectReferenced = true;
            }
        }

        if (!sparseInjectReferenced)
        {
            return;
        }
        
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.AssemblyProcessingStarted,
            Location.None,
            context.Compilation.Assembly.Name));
        
        if (context.SyntaxReceiver is not RegisterSyntaxReceiver receiver)
            return;
        
        var compilation = context.Compilation;

        var progress = 0f;
        var index = 0;
        var sw = Stopwatch.StartNew();
        var sw1 = Stopwatch.StartNew();
        sw1.Stop();
        var codeWriter = new CodeWriter();
        
        var generatedClasses = new List<GeneratedInstanceFactory>();
        
        codeWriter.AppendLine("using System;");
        codeWriter.AppendLine("using System.Collections.Generic;");
        codeWriter.AppendLine("using System.Runtime.CompilerServices;");
        codeWriter.AppendLine("using SparseInject;");
        
        codeWriter.AppendLine("#if UNITY_2018_1_OR_NEWER");
        codeWriter.AppendLine("using Unity.IL2CPP.CompilerServices;");
        codeWriter.AppendLine("using UnityEngine.Scripting;");
        codeWriter.AppendLine("#endif");
        
        codeWriter.AppendLine();
        
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            sw1.Start();
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            sw1.Stop();

            var typeDeclarations = syntaxTree.GetRoot()
                .DescendantNodes()
                .OfType<TypeDeclarationSyntax>();

            foreach (var typeDeclaration in typeDeclarations)
            {
                sw1.Start();
                var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                sw1.Stop();
                
                if (typeSymbol.TypeKind is TypeKind.Interface or TypeKind.Struct or TypeKind.Enum)
                {
                    continue;
                }
                
                if (typeSymbol.IsAbstract || typeSymbol.IsStatic)
                {
                    continue;
                }
                
                //TODO: check not inherit from unity object
                
                if (typeSymbol != null && receiver.TypesInContainer.Contains(typeSymbol.Name))
                {
                    index++;
                    progress = (index * 1.0f / receiver.TypesInContainer.Count * 1.0f) * 100f;
                        
                    if (sw.Elapsed.TotalSeconds >= 5f)
                    {
                        Console.WriteLine($"Compiled: {index} of {receiver.RegisterCalls.Count} == {progress} %");
                        Console.WriteLine($"GetSemanticModel time: {sw1.Elapsed.TotalSeconds} seconds");
                
                        sw.Restart();
                    }
                    
                    var typeDeclarationSyntax =
                        typeSymbol.DeclaringSyntaxReferences.First().GetSyntax() as TypeDeclarationSyntax;
                    var typeMeta = Analyzer.AnalyzeTypeSymbol(typeSymbol, typeDeclarationSyntax);
                    
                    if (Emitter.TryEmitGeneratedInjector(typeMeta, codeWriter, context))
                    {
                        var typeName = typeMeta.TypeName
                            .Replace("global::", "")
                            .Replace("<", "_")
                            .Replace(">", "_");
                        
                        var generateTypeName = $"{typeName}_SparseInject_GeneratedInstanceFactory";
                        
                        generatedClasses.Add(new GeneratedInstanceFactory()
                        {
                            ClassName = typeName,
                            GeneratedFactoryName = generateTypeName,
                            ConstructorParameterTypes = typeMeta.ConstructorParameters
                        });
                    }
                }
            }
        }
        
        codeWriter.AppendLine("#if UNITY_2018_1_OR_NEWER");
        codeWriter.AppendLine("[Il2CppSetOption(Option.NullChecks, false)]");
        codeWriter.AppendLine("[Il2CppSetOption(Option.DivideByZeroChecks, false)]");
        codeWriter.AppendLine("[Il2CppSetOption(Option.ArrayBoundsChecks, false)]");
        codeWriter.AppendLine("[Preserve]");
        codeWriter.AppendLine("#endif");
        using (codeWriter.BeginBlockScope("class SparseInject_ReflectionBakingProvider : IReflectionBakingProvider"))
        {
            codeWriter.AppendLine("public Type[] ConstructorParametersSpan => _constructorParameterTypes;");
            
            codeWriter.AppendLine("private Dictionary<Type, InstanceFactoryBase> _cache;");
            codeWriter.AppendLine("private Type[] _constructorParameterTypes;");
            
            using (codeWriter.BeginBlockScope("public void Initialize()"))
            {
                var allConstructorParameters = generatedClasses.Sum(d => d.ConstructorParameterTypes.Length);
                
                codeWriter.AppendLine($"_cache = new Dictionary<Type, InstanceFactoryBase>({generatedClasses.Count});");
                codeWriter.AppendLine($"_constructorParameterTypes = new Type[{allConstructorParameters}];");

                var constructorIndex = 0;

                foreach (var data in generatedClasses)
                {
                    codeWriter.AppendLine($"_cache.Add(typeof({data.ClassName}), new {data.GeneratedFactoryName}({constructorIndex}));");

                    for (var i = 0; i < data.ConstructorParameterTypes.Length; i++)
                    {
                        codeWriter.AppendLine($"_constructorParameterTypes[{constructorIndex}] = typeof({data.ConstructorParameterTypes[i].paramType});");
                        constructorIndex++;
                    }
                }
            }
            
            using (codeWriter.BeginBlockScope("public InstanceFactoryBase GetInstanceFactory(Type type)"))
            {
                using (codeWriter.BeginBlockScope(
                           "if (_cache.TryGetValue(type, out var instanceFactory))"))
                {
                    codeWriter.AppendLine("return instanceFactory;");
                }
                
                codeWriter.AppendLine("return null;");
            }
        }
        
        context.AddSource("SparseInject_GeneratedInstanceFactory.g.cs",
            SourceText.From(codeWriter.ToString(), Encoding.UTF8));
    }
}

class GeneratedInstanceFactory
{
    public string ClassName;
    public string GeneratedFactoryName;
    public (string paramType, string paramName)[] ConstructorParameterTypes;
}

class RegisterSyntaxReceiver : ISyntaxReceiver
{
    public DateTime StartTime { get; private set; } = DateTime.Now;
    public List<InvocationExpressionSyntax> RegisterCalls { get; } = new List<InvocationExpressionSyntax>();
    public HashSet<string> TypesInContainer { get; } = new HashSet<string>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not IdentifierNameSyntax)
        {
            return;
        }

        var genericArgumentName = (syntaxNode as IdentifierNameSyntax).Identifier.Text;
        syntaxNode = syntaxNode.Parent;
        
        if (syntaxNode is not TypeArgumentListSyntax)
        {
            return;
        }
        
        syntaxNode = syntaxNode.Parent;
        
        if (syntaxNode is GenericNameSyntax genericNameSyntax && genericNameSyntax.Identifier.Text.StartsWith("Register"))
        {
            syntaxNode = syntaxNode.Parent;
            
            if (syntaxNode is not MemberAccessExpressionSyntax)
            {
                return;
            }
            
            syntaxNode = syntaxNode.Parent;
            
            if (syntaxNode is not InvocationExpressionSyntax)
            {
                return;
            }

            while (syntaxNode is not CompilationUnitSyntax)
            {
                syntaxNode = syntaxNode.Parent;
            }

            foreach (var usingDirective in (syntaxNode as CompilationUnitSyntax).Usings)
            {
                if (usingDirective.Name is IdentifierNameSyntax identifierNameSyntax && identifierNameSyntax.Identifier.Text == "SparseInject")
                {
                    TypesInContainer.Add(genericArgumentName);
                    RegisterCalls.Add(syntaxNode as InvocationExpressionSyntax);
                }
            }
        }
    }
}
