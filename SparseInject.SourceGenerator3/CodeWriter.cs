using System;
using System.Text;

namespace SparseInject.SourceGenerator;

public class CodeWriter
{
    private readonly int _scopeIdentSize;
    private readonly StringBuilder _builder = new StringBuilder(1024);
    
    private int _indent;
    private bool _previousScopeJustClosed;

    public CodeWriter(int scopeIdentSize, params string[] usingLiterals)
    {
        _scopeIdentSize = scopeIdentSize;
        foreach (var usingLiteral in usingLiterals)
        {
            _builder.Append("using ").Append(usingLiteral).AppendLine(";");
        }
        
        _builder.AppendLine();
    }

    public void Write(string text, bool ensureEmptyLineAfterScope = true)
    {
        if (ensureEmptyLineAfterScope)
        {
            EnsureEmptyLineAfterScope();
        }
        
        var ident = GetIdent();
        
        _builder.Append(ident).Append(text);
    }

    public void WriteLine(string text, bool ensureEmptyLineAfterScope = true)
    {
        if (ensureEmptyLineAfterScope)
        {
            EnsureEmptyLineAfterScope();
        }
        
        var ident = GetIdent();
        
        _builder.Append(ident).AppendLine(text);
    }
    
    public void WriteEmptyLine()
    {
        var ident = GetIdent();
        
        _builder.AppendLine(ident);
    }

    public IDisposable Scope(string code, string? afterScopeCode = default, bool placeEmptyLineAfterScope = true)
    {
        EnsureEmptyLineAfterScope();
        
        return new CodeScope(this, code, afterScopeCode, placeEmptyLineAfterScope);
    }

    public void AddIdent()
    {
        _indent += _scopeIdentSize;
    }

    public void RemoveIdent()
    {
        _indent -= _scopeIdentSize;
    }

    public string Build()
    {
        var result = _builder.ToString();

        return result;
    }
    
    public void NotifyScopeClosed(bool placeEmptyLineAfterScope)
    {
        if (placeEmptyLineAfterScope)
        {
            _previousScopeJustClosed = true;
        }
    }

    private void EnsureEmptyLineAfterScope()
    {
        if (_previousScopeJustClosed)
        {
            WriteEmptyLine();
            _previousScopeJustClosed = false;
        }
    }

    private string GetIdent()
    {
        var idents = new char[_indent];
        for (var i = 0; i < _indent; i++)
        {
            idents[i] = ' ';
        }
        
        return new string(idents);
    }
}

public class CodeScope : IDisposable
{
    private readonly CodeWriter _writer;
    private readonly string? _afterScopeCode;
    private readonly bool _placeEmptyLineAfterScope;

    public CodeScope(CodeWriter writer, string code, string? afterScopeCode, bool placeEmptyLineAfterScope)
    {
        _writer = writer;
        _afterScopeCode = afterScopeCode;
        _placeEmptyLineAfterScope = placeEmptyLineAfterScope;

        _writer.WriteLine(code, false);
        _writer.WriteLine("{", false);
        
        _writer.AddIdent();
    }
    
    public void Dispose()
    {
        _writer.RemoveIdent();
        
        if (string.IsNullOrEmpty(_afterScopeCode))
        {
            _writer.Write("}\n", false);
        }
        else
        {
            _writer.Write("}" + _afterScopeCode + "\n", false);
        }

        _writer.NotifyScopeClosed(_placeEmptyLineAfterScope);
    }
}

public class CompositeScope : IDisposable
{
    private readonly IDisposable[] _scopes;

    public CompositeScope(params IDisposable[] scopes)
    {
        _scopes = scopes;
    }
    
    public void Dispose()
    {
        for (var i = _scopes.Length - 1; i >= 0; i--)
        {
            _scopes[i].Dispose();
        }
    }
}

public class EmptyScope : IDisposable
{
    public void Dispose()
    {
        
    }
}