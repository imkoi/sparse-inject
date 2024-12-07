using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SparseInject.SourceGenerator;

public class RegisterSyntaxReceiver : ISyntaxReceiver
{
    public HashSet<string> TypesWithGenerator { get; }

    public RegisterSyntaxReceiver()
    {
        TypesWithGenerator = new HashSet<string>();
    }

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
        
        if (syntaxNode is GenericNameSyntax genericNameSyntax)
        {
            var identifierName = genericNameSyntax.Identifier.Text;
            
            if (identifierName == "Register" || identifierName == "RegisterScope")
            {
                TypesWithGenerator.Add(genericArgumentName);
            }
        }
    }
}