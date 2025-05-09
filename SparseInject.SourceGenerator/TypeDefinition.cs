using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SparseInject.SourceGenerator;

public class TypeDefinition
{
    public INamedTypeSymbol Symbol { get; }
    public List<string> GenericArgs { get; }
    public string TypeName { get; }
    public string FullTypeName { get; }

    public IMethodSymbol? Constructor { get; }
    public (string paramType, string paramName)[] ConstructorParameters { get; }
    public bool IsPrivate => Symbol.DeclaredAccessibility == Accessibility.Private;

    readonly TypeDeclarationSyntax syntax;

    public TypeDefinition(INamedTypeSymbol symbol, List<string> genericArgs, TypeDeclarationSyntax syntax)
    {
        Symbol = symbol;
        GenericArgs = genericArgs;
        this.syntax = syntax;

        TypeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        FullTypeName = symbol.ToDisplayString();

        Constructor = GetConstructor();
        ConstructorParameters = GetConstructorParameters(Constructor);
    }

    public Location GetLocation()
    {
        return syntax?.Identifier.GetLocation() ??
               Symbol.Locations.FirstOrDefault() ??
               Location.None;
    }

    private IMethodSymbol? GetConstructor()
    {
        return Symbol.InstanceConstructors
            .Where(x => !x.IsImplicitlyDeclared)
            .OrderByDescending(ctor => ctor.Parameters.Length)
            .FirstOrDefault();
    }

    private (string paramType, string paramName)[] GetConstructorParameters(IMethodSymbol? constructorSymbol)
    {
        var parameters = constructorSymbol != null ? constructorSymbol.Parameters
            .Select(param =>
            {
                var paramType = param.Type.ToDisplayString();
                var paramName = param.Name;
                return (paramType, paramName);
            })
            .ToArray() : Array.Empty<(string paramType, string paramName)>();

        return parameters;
    }
}