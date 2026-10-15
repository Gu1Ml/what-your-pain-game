using Godot;

namespace WhatYourPain.Content;

/// <summary>
/// Lê conteúdo empacotado em <c>res://content/</c> via FileAccess do Godot. É o único arquivo
/// de <c>scripts/content/</c> que depende do Godot (fica fora do projeto de testes).
/// </summary>
public sealed class GodotContentSource(string root = "res://content/") : IContentSource
{
    public string RootDescription => root;

    public bool Exists(string relativePath) => FileAccess.FileExists(root + relativePath);

    public string ReadAllText(string relativePath)
    {
        var path = root + relativePath;
        var text = FileAccess.GetFileAsString(path);
        var error = FileAccess.GetOpenError();
        if (error != Error.Ok)
            throw new System.IO.IOException($"Não foi possível ler '{path}': {error}.");
        return text;
    }
}
