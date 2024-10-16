using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VContainer.SourceGenerator;

class TypeMeta
{
    public INamedTypeSymbol Symbol { get; }
    public string TypeName { get; }
    public string FullTypeName { get; }

    public IReadOnlyList<IMethodSymbol> Constructors { get; }

    public bool IsGenerics => Symbol.Arity > 0;

    readonly TypeDeclarationSyntax syntax;

    public TypeMeta(INamedTypeSymbol symbol, TypeDeclarationSyntax syntax)
    {
        Symbol = symbol;
        this.syntax = syntax;

        TypeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        FullTypeName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        Constructors = GetConstructors();
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

    IReadOnlyList<IMethodSymbol> GetConstructors()
    {
        return Symbol.InstanceConstructors
            .Where(x => !x.IsImplicitlyDeclared) // remove empty ctor(struct always generate it), record's clone ctor
            .ToArray();
    }

    public bool IsNested()
    {
        return Symbol.ContainingType != null;
    }
}