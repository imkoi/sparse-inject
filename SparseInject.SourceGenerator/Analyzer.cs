using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SparseInject.SourceGenerator
{
    static class Analyzer
    {
        public static TypeMeta? AnalyzeTypeSymbol(
            ITypeSymbol symbol,
            TypeDeclarationSyntax syntax,
            List<string> genericArgs = null)
        {
            if (symbol is not INamedTypeSymbol typeSymbol)
            {
                return null;
            }

            if (typeSymbol.TypeKind is TypeKind.Interface or TypeKind.Struct or TypeKind.Enum)
            {
                return null;
            }

            if (typeSymbol.IsAbstract || typeSymbol.IsStatic || typeSymbol.SpecialType != SpecialType.None)
            {
                return null;
            }

            var moduleName = typeSymbol.ContainingModule.Name;
            if (moduleName is "VContainer" or "VContainer.Standalone" ||
                moduleName.StartsWith("Unity.") ||
                moduleName.StartsWith("UnityEngine.") ||
                moduleName.StartsWith("System."))
            {
                return null;
            }


            return new TypeMeta(typeSymbol, genericArgs, syntax);
        }
    }
}