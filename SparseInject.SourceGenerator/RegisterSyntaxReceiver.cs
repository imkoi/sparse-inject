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
                    TypesWithGenerator.Add(genericArgumentName);
                }
            }
        }
    }
}