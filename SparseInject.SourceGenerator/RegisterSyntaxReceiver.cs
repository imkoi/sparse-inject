using System;
using System.Collections.Generic;
using System.Linq;
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
        if (syntaxNode is InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess && memberAccess.Name is GenericNameSyntax genericName && 
                (genericName.Identifier.Text == "Register" || genericName.Identifier.Text == "RegisterScope"))
            {
                var typeArguments = genericName.TypeArgumentList.Arguments;

                foreach (var typeArgument in typeArguments)
                {
                    if (typeArgument is GenericNameSyntax genericType)
                    {
                        var fullTypeName = $"{genericType.Identifier.Text}<{string.Join(",", genericType.TypeArgumentList.Arguments.Select(a => a.ToString()))}>";
                        
                        TypesWithGenerator.Add(fullTypeName);
                    }
                    else if (typeArgument is IdentifierNameSyntax identifierName)
                    {
                        TypesWithGenerator.Add(identifierName.Identifier.Text);
                    }
                }
            }
        }

        // if (syntaxNode is not IdentifierNameSyntax)
        // {
        //     return;
        // }
        //
        // var genericArgumentName = (syntaxNode as IdentifierNameSyntax).Identifier.Text;
        // syntaxNode = syntaxNode.Parent;
        //
        // if (syntaxNode is not TypeArgumentListSyntax)
        // {
        //     return;
        // }
        //
        // syntaxNode = syntaxNode.Parent;
        //
        // if (syntaxNode is GenericNameSyntax genericNameSyntax)
        // {
        //     var identifierName = genericNameSyntax.Identifier.Text;
        //     
        //     if (identifierName == "Register" || identifierName == "RegisterScope")
        //     {
        //         TypesWithGenerator.Add(genericArgumentName);
        //     }
        // }
    }
}