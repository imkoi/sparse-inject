using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SparseInject.SourceGenerator;

class TypeMeta
{
    public INamedTypeSymbol Symbol { get; }
    public string TypeName { get; }
    public string FullTypeName { get; }

    public IMethodSymbol? Constructor { get; }
    public (string paramType, string paramName)[] ConstructorParameters { get; }

    public bool IsGenerics => Symbol.Arity > 0;

    readonly TypeDeclarationSyntax syntax;

    public TypeMeta(INamedTypeSymbol symbol, TypeDeclarationSyntax syntax)
    {
        Symbol = symbol;
        this.syntax = syntax;

        TypeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        FullTypeName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        Constructor = GetConstructor();
        ConstructorParameters = GetConstructorParameters(Constructor);
    }

    public Location GetLocation()
    {
        return syntax?.Identifier.GetLocation() ??
               Symbol.Locations.FirstOrDefault() ??
               Location.None;
    }

    public bool InheritsFrom(INamedTypeSymbol baseSymbol)
    {
        var baseName = baseSymbol.ToString();
        var symbol = Symbol;
        while (true)
        {
            if (symbol.ToString() == baseName)
            {
                return true;
            }
            if (symbol.BaseType != null)
            {
                symbol = symbol.BaseType;
                continue;
            }
            break;
        }
        return false;
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
                var paramType =
                    param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                var paramName = param.Name;
                return (paramType, paramName);
            })
            .ToArray() : Array.Empty<(string paramType, string paramName)>();

        return parameters;
    }

    public bool IsNested()
    {
        return Symbol.ContainingType != null;
    }
}