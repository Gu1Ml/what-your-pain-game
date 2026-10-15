using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using WhatYourPain.Content;
using Xunit;

namespace WhatYourPain.Tests.Support;

/// <summary>
/// Cópia temporária do conteúdo real (<c>content/</c>) para injetar um defeito por teste
/// sem tocar nos arquivos do repositório.
/// </summary>
public sealed class ContentWorkspace : IDisposable
{
    public ContentWorkspace()
    {
        Root = Path.Combine(Path.GetTempPath(), "wyp-content-" + Guid.NewGuid().ToString("N"));
        CopyDirectory(RepoPaths.ContentDirectory, Root);
    }

    public string Root { get; }

    public DataLoader Loader => new(new FileSystemContentSource(Root));

    public void Delete(string relativePath) => File.Delete(Path.Combine(Root, relativePath));

    public void WriteText(string relativePath, string text) => File.WriteAllText(Path.Combine(Root, relativePath), text);

    /// <summary>Edita um arquivo JSON da cópia como árvore mutável.</summary>
    public void EditJson(string relativePath, Action<JsonNode> edit)
    {
        var path = Path.Combine(Root, relativePath);
        var node = JsonNode.Parse(File.ReadAllText(path))!;
        edit(node);
        File.WriteAllText(path, node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

    /// <summary>Edita um resultado da matriz do Dia 1, localizado por caso e ação.</summary>
    public void EditOutcome(string caseId, string action, Action<JsonObject> edit) =>
        EditJson(ContentPaths.DecisionMatrix(1), matrix => edit(FindOutcome(matrix, caseId, action)));

    public static JsonObject FindOutcome(JsonNode matrix, string caseId, string action)
    {
        var @case = matrix["cases"]!.AsArray().Single(c => (string?)c!["id"] == caseId)!;
        return @case["outcomes"]!.AsArray().Single(o => (string?)o!["action"] == action)!.AsObject();
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(Root, recursive: true);
        }
        catch (IOException)
        {
            // Pasta temporária; falha de limpeza não invalida o teste.
        }
    }

    private static void CopyDirectory(string source, string target)
    {
        Directory.CreateDirectory(target);
        foreach (var file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
        {
            var destination = Path.Combine(target, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(file, destination);
        }
    }
}

/// <summary>Localiza a raiz do repositório (pasta com <c>project.godot</c>).</summary>
public static class RepoPaths
{
    public static string Root { get; } = FindRoot();

    public static string ContentDirectory => Path.Combine(Root, "content");

    public static string DecisionsDocument => Path.Combine(Root, "docs", "decisions.md");

    public static string SourcesDirectory => Path.Combine(Root, "docs", "sources");

    private static string FindRoot()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
            if (File.Exists(Path.Combine(dir.FullName, "project.godot")))
                return dir.FullName;
        throw new InvalidOperationException("Raiz do repositório (project.godot) não encontrada.");
    }
}

public static class IssueAssert
{
    public static ContentIssue HasError(IEnumerable<ContentIssue> issues, string code, string? file = null)
    {
        var list = issues.ToList();
        var match = list.FirstOrDefault(i => i.Severity == IssueSeverity.Error && i.Code == code && (file is null || i.File == file));
        Assert.True(match is not null,
            $"Esperado erro {code}{(file is null ? "" : $" em {file}")}; encontrados:\n{string.Join("\n", list)}");
        return match!;
    }

    public static ContentValidationException ThrowsValidation(ContentWorkspace workspace) =>
        Assert.Throws<ContentValidationException>(() => workspace.Loader.LoadDay(1));

    public static ContentLoadException ThrowsLoad(ContentWorkspace workspace) =>
        Assert.Throws<ContentLoadException>(() => workspace.Loader.LoadDay(1));
}
