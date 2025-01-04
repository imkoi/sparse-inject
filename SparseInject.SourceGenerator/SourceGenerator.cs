using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
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
        if (context.SyntaxReceiver is not RegisterSyntaxReceiver receiver)
        {
            return;
        }

        if (!HasReference(context, "SparseInject"))
        {
            return;
        }
        
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.AssemblyProcessingStarted,
            Location.None,
            context.Compilation.Assembly.Name));
        
        var compilation = context.Compilation;

        var generatedClasses = new List<GeneratedInstanceFactory>(receiver.TypesWithGenerator.Count);
        var codeWriter = new CodeWriter(4, new []
        {
            "System",
            "System.Collections.Generic",
            "System.Runtime.CompilerServices",
            "SparseInject"
        });

        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            //var semanticModel = compilation.GetSemanticModel(syntaxTree);

            var typeDeclarations = syntaxTree.GetRoot()
                .DescendantNodes()
                .OfType<TypeDeclarationSyntax>();

            foreach (var typeDeclaration in typeDeclarations)
            {
                var semanticModel = compilation.GetSemanticModel(typeDeclaration.SyntaxTree);
                
                var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                
                if (typeSymbol.TypeKind is TypeKind.Interface or TypeKind.Struct or TypeKind.Enum)
                {
                    continue;
                }
                
                if (typeSymbol.IsAbstract || typeSymbol.IsStatic)
                {
                    continue;
                }

                if (typeSymbol != null)
                {
                    var typeName = typeSymbol.Name;
                    var listGenericArgs = new List<string>();
                    
                    if (typeSymbol is INamedTypeSymbol namedTypeSymbolSymbol && namedTypeSymbolSymbol.Arity > 0)
                    {
                        foreach (var typeNameForGen in receiver.TypesWithGenerator)
                        {
                            if (typeNameForGen.StartsWith(typeName) && typeNameForGen.EndsWith(">") &&
                                typeNameForGen.Contains("<"))
                            {
                                var startIndex = typeNameForGen.IndexOf('<');
                                var endIndex = typeNameForGen.LastIndexOf('>');
                                
                                if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                                {
                                    var substring = typeNameForGen.Substring(startIndex + 1, endIndex - startIndex - 1);
                                    listGenericArgs.Add(substring);
                                }
                            }
                        }

                        if (listGenericArgs.Count == 0)
                        {
                            continue;
                        }
                    }
                    else if (!receiver.TypesWithGenerator.Contains(typeName))
                    {
                        continue;
                    }
                    
                    var typeDeclarationSyntax =
                        typeSymbol.DeclaringSyntaxReferences.First().GetSyntax() as TypeDeclarationSyntax;
                    

                    if (listGenericArgs?.Count > 0)
                    {
                        foreach (var genericArg in listGenericArgs)
                        {
                            var genericArgs = genericArg.Split(',').Select(x => x.Replace(" ", "")).ToList();
                            
                            var typeMeta = TypeAnalyzer.AnalyzeTypeSymbol(typeSymbol, typeDeclarationSyntax, genericArgs);
                            
                            if (InstanceFactoryGenerator.TryGenerate(typeMeta, codeWriter, context, out var generateTypeName, out var correctedTypeName))
                            {
                                generatedClasses.Add(new GeneratedInstanceFactory
                                {
                                    Type = typeSymbol,
                                    TypeName = correctedTypeName,
                                    GenericArgument = genericArg,
                                    GeneratedFactoryName = generateTypeName,
                                    ConstructorParameterTypes = typeMeta.ConstructorParameters
                                });
                            }
                        }
                    }
                    else
                    {
                        var typeMeta = TypeAnalyzer.AnalyzeTypeSymbol(typeSymbol, typeDeclarationSyntax);
                        
                        if (InstanceFactoryGenerator.TryGenerate(typeMeta, codeWriter, context, out var generateTypeName, out var correctedTypeName))
                        {
                            generatedClasses.Add(new GeneratedInstanceFactory
                            {
                                Type = typeSymbol,
                                TypeName = correctedTypeName,
                                GeneratedFactoryName = generateTypeName,
                                ConstructorParameterTypes = typeMeta.ConstructorParameters
                            });
                        }
                    }
                }
            }
        }
        
        codeWriter.WriteLine("#if UNITY_2017_1_OR_NEWER");
        codeWriter.WriteLine("[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]");
        codeWriter.WriteLine("[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]");
        codeWriter.WriteLine("[Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]");
        codeWriter.WriteLine("[UnityEngine.Scripting.Preserve]");
        codeWriter.WriteLine("#endif");
        using (codeWriter.Scope("class SparseInject_ReflectionBakingProvider : IReflectionBakingProvider"))
        {
            codeWriter.WriteLine("public Type[] ConstructorParametersSpan => _constructorParameterTypes;");
            
            codeWriter.WriteLine("private Dictionary<Type, InstanceFactoryBase> _cache;");
            codeWriter.WriteLine("private Type[] _constructorParameterTypes;");
            
            using (codeWriter.Scope("public void Initialize()"))
            {
                var allConstructorParameters = generatedClasses.Sum(d => d.ConstructorParameterTypes.Length);
                
                codeWriter.WriteLine($"_cache = new Dictionary<Type, InstanceFactoryBase>({generatedClasses.Count});");
                codeWriter.WriteLine($"_constructorParameterTypes = new Type[{allConstructorParameters}];");

                var constructorIndex = 0;

                foreach (var data in generatedClasses)
                {
                    var typeName = data.GenericArgument != null && data.GenericArgument.Length > 0 ? data.TypeName : data.Type.ToDisplayString();
                    
                    codeWriter.WriteLine($"_cache.Add(typeof({typeName}), new {data.GeneratedFactoryName}({constructorIndex}));");

                    for (var i = 0; i < data.ConstructorParameterTypes.Length; i++)
                    {
                        codeWriter.WriteLine($"_constructorParameterTypes[{constructorIndex}] = typeof({data.ConstructorParameterTypes[i].paramType});");
                        constructorIndex++;
                    }
                }
            }
            
            using (codeWriter.Scope("public InstanceFactoryBase GetInstanceFactory(Type type)"))
            {
                using (codeWriter.Scope("if (_cache.TryGetValue(type, out var instanceFactory))"))
                {
                    codeWriter.WriteLine("return instanceFactory;");
                }
                
                codeWriter.WriteLine("return null;");
            }
        }

        var generatedCode = codeWriter.Build();

        context.AddSource("SparseInject_GeneratedInstanceFactory.g.cs", SourceText.From(generatedCode, Encoding.UTF8));
    }

    private static bool HasReference(GeneratorExecutionContext context, string reference)
    {
        var referenced = false;

        foreach (var referencedAssembly in context.Compilation.ReferencedAssemblyNames)
        {
            if (referencedAssembly.Name == reference)
            {
                referenced = true;
            }
        }

        return referenced;
    }
}

class GeneratedInstanceFactory
{
    public ITypeSymbol Type;
    public string TypeName;
    public string GenericArgument;
    public string GeneratedFactoryName;
    public (string paramType, string paramName)[] ConstructorParameterTypes;
}
