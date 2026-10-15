using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WhatYourPain.Domain.Models;

namespace WhatYourPain.Content;

/// <summary>Conteúdo de um dia já carregado (e, via <see cref="DataLoader.LoadDay"/>, validado).</summary>
public sealed record DayContent
{
    public required int DayNumber { get; init; }
    public required CampaignData Campaign { get; init; }
    public required ProtocolData Protocol { get; init; }
    public required PatientSetData Patients { get; init; }
    public required DecisionMatrix DecisionMatrix { get; init; }
    public required DialogueSetData Dialogues { get; init; }

    /// <summary>Relatório da validação (avisos de pendências; vazio se não validado).</summary>
    public ContentValidationReport Report { get; init; } = new([]);

    public DayConfig? DayConfig => Campaign.Days.FirstOrDefault(d => d.DayNumber == DayNumber);
}

/// <summary>
/// Lê os JSON de conteúdo e os desserializa em DTOs tipados. Falhas informam arquivo,
/// linha/caminho JSON e motivo (plan.md, "Validação na carga").
/// </summary>
public sealed class DataLoader(IContentSource source)
{
    public CampaignData LoadCampaign() => Load<CampaignData>(ContentPaths.Campaign);

    public ProtocolData LoadProtocol(int day) => Load<ProtocolData>(ContentPaths.Protocol(day));

    public PatientSetData LoadPatients(int day) => Load<PatientSetData>(ContentPaths.NarrativePatients(day));

    public DecisionMatrix LoadDecisionMatrix(int day) => Load<DecisionMatrix>(ContentPaths.DecisionMatrix(day));

    public DialogueSetData LoadDialogues(int day) => Load<DialogueSetData>(ContentPaths.Dialogues(day));

    /// <summary>
    /// Carrega todos os arquivos do dia, reunindo as falhas de carga de todos eles, e valida
    /// o conjunto. Lança <see cref="ContentLoadException"/> ou <see cref="ContentValidationException"/>.
    /// </summary>
    public DayContent LoadDay(int day)
    {
        var issues = new List<ContentIssue>();
        var campaign = TryLoad(LoadCampaign, issues);
        var protocol = TryLoad(() => LoadProtocol(day), issues);
        var patients = TryLoad(() => LoadPatients(day), issues);
        var matrix = TryLoad(() => LoadDecisionMatrix(day), issues);
        var dialogues = TryLoad(() => LoadDialogues(day), issues);

        if (issues.Count > 0)
            throw new ContentLoadException(issues);

        var content = new DayContent
        {
            DayNumber = day,
            Campaign = campaign!,
            Protocol = protocol!,
            Patients = patients!,
            DecisionMatrix = matrix!,
            Dialogues = dialogues!,
        };

        var report = ValidateContent(content);
        if (!report.IsValid)
            throw new ContentValidationException(report);
        return content with { Report = report };
    }

    public ContentValidationReport ValidateContent(DayContent content) => ContentValidator.Validate(content);

    private T Load<T>(string relativePath) where T : class
    {
        if (!source.Exists(relativePath))
            throw Fail(IssueCodes.FileMissing, relativePath, null,
                $"arquivo obrigatório ausente em {source.RootDescription}");

        string text;
        try
        {
            text = source.ReadAllText(relativePath);
        }
        catch (Exception ex) when (ex is not ContentLoadException)
        {
            throw Fail(IssueCodes.FileMissing, relativePath, null, $"não foi possível ler o arquivo: {ex.Message}");
        }

        T? data;
        try
        {
            data = JsonSerializer.Deserialize<T>(text, ContentJson.Options);
        }
        catch (JsonException ex)
        {
            throw Fail(Classify(ex), relativePath, DescribeLocation(ex), ex.Message);
        }

        if (data is null)
            throw Fail(IssueCodes.FieldMissing, relativePath, null, "o documento JSON é null");

        var nullFields = NullFieldChecker.FindNullRequiredFields(data);
        if (nullFields.Count > 0)
            throw new ContentLoadException(nullFields
                .Select(path => new ContentIssue(IssueSeverity.Error, IssueCodes.FieldMissing, relativePath, path,
                    "campo obrigatório com valor null"))
                .ToList());

        return data;
    }

    private static string Classify(JsonException ex)
    {
        // Erros de sintaxe vêm do leitor JSON como exceção interna; os demais são de contrato.
        if (ex.InnerException?.GetType().Name == "JsonReaderException")
            return IssueCodes.JsonInvalid;
        if (ex.Message.Contains("missing required properties", StringComparison.OrdinalIgnoreCase))
            return IssueCodes.FieldMissing;
        return IssueCodes.SchemaMismatch;
    }

    private static string? DescribeLocation(JsonException ex)
    {
        var parts = new List<string>();
        if (ex.LineNumber is long line)
            parts.Add($"linha {line + 1}");
        if (!string.IsNullOrEmpty(ex.Path))
            parts.Add(ex.Path);
        return parts.Count == 0 ? null : string.Join(", ", parts);
    }

    private static ContentLoadException Fail(string code, string file, string? location, string message) =>
        new([new ContentIssue(IssueSeverity.Error, code, file, location, message)]);

    private static T? TryLoad<T>(Func<T> load, List<ContentIssue> issues) where T : class
    {
        try
        {
            return load();
        }
        catch (ContentLoadException ex)
        {
            issues.AddRange(ex.Issues);
            return null;
        }
    }
}
