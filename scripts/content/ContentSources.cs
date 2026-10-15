using System.IO;
using System.Text;

namespace WhatYourPain.Content;

/// <summary>Origem dos arquivos de conteúdo, com caminhos relativos à raiz de conteúdo.</summary>
public interface IContentSource
{
    /// <summary>Descrição da raiz, usada nas mensagens (ex.: <c>res://content/</c>).</summary>
    string RootDescription { get; }

    bool Exists(string relativePath);

    string ReadAllText(string relativePath);
}

/// <summary>Lê de uma pasta do sistema de arquivos (testes e ferramentas fora do Godot).</summary>
public sealed class FileSystemContentSource(string rootDirectory) : IContentSource
{
    public string RootDescription => rootDirectory;

    public bool Exists(string relativePath) => File.Exists(Resolve(relativePath));

    public string ReadAllText(string relativePath) => File.ReadAllText(Resolve(relativePath), Encoding.UTF8);

    private string Resolve(string relativePath) => Path.Combine(rootDirectory, relativePath);
}

/// <summary>Caminhos dos arquivos de conteúdo por dia (plan.md, "Contratos de conteúdo").</summary>
public static class ContentPaths
{
    public const string Campaign = "campaign.json";

    public static string Protocol(int day) => $"protocols/day_{day}_protocol.json";

    public static string NarrativePatients(int day) => $"patients/day_{day}_narrative_patients.json";

    public static string DecisionMatrix(int day) => $"decisions/day_{day}_decision_matrix.json";

    public static string Dialogues(int day) => $"dialogues/day_{day}_dialogue.json";
}
