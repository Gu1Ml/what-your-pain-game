using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatYourPain.Content;

public enum IssueSeverity
{
    Error,
    Warning,
}

/// <summary>
/// Problema de conteúdo encontrado na carga ou validação. É erro de desenvolvimento/conteúdo,
/// nunca erro de triagem do jogador (constituição, princípio 10).
/// </summary>
public sealed record ContentIssue(IssueSeverity Severity, string Code, string File, string? Location, string Message)
{
    public override string ToString()
    {
        var where = Location is null ? File : $"{File} @ {Location}";
        var tag = Severity == IssueSeverity.Error ? "ERRO" : "AVISO";
        return $"[{tag}] {Code} — {where}: {Message}";
    }
}

/// <summary>Códigos estáveis usados em mensagens e testes.</summary>
public static class IssueCodes
{
    public const string FileMissing = "CONTENT_FILE_MISSING";
    public const string JsonInvalid = "CONTENT_JSON_INVALID";
    public const string FieldMissing = "CONTENT_FIELD_MISSING";
    public const string SchemaMismatch = "CONTENT_SCHEMA_MISMATCH";
    public const string DuplicateId = "DUPLICATE_ID";
    public const string UnknownReference = "UNKNOWN_REFERENCE";
    public const string ReferenceMismatch = "REFERENCE_MISMATCH";
    public const string InvalidValue = "INVALID_VALUE";
    public const string InvalidCondition = "INVALID_CONDITION";
    public const string MissingDayConfig = "MISSING_DAY_CONFIG";
    public const string StoryOrderIncomplete = "STORY_ORDER_INCOMPLETE";
    public const string PrecedenceUndecided = "PRECEDENCE_UNDECIDED";
    public const string DuplicateAction = "DUPLICATE_ACTION";
    public const string MissingAction = "MISSING_ACTION";
    public const string IncompleteConsequence = "INCOMPLETE_CONSEQUENCE";
    public const string InvalidErrorConsequence = "INVALID_ERROR_CONSEQUENCE";
    public const string InvalidFine = "INVALID_FINE";
    public const string UnresolvedWithEffect = "UNRESOLVED_WITH_EFFECT";
    public const string UnresolvedWithoutPending = "UNRESOLVED_WITHOUT_PENDING";
    public const string FlagValueNotAllowed = "FLAG_VALUE_NOT_ALLOWED";
    public const string NoticeWithoutPending = "NOTICE_WITHOUT_PENDING";
    public const string NoticeStatusMismatch = "NOTICE_STATUS_MISMATCH";

    // Avisos: pendências explícitas, não bloqueiam a carga.
    public const string UnresolvedOutcome = "UNRESOLVED_OUTCOME";
    public const string OpenDecisionDependency = "OPEN_DECISION_DEPENDENCY";
    public const string DialoguePending = "DIALOGUE_PENDING";
    public const string StartingMoneyUndefined = "STARTING_MONEY_UNDEFINED";
    public const string ProtocolNoticeBlocked = "PROTOCOL_NOTICE_BLOCKED";
}

/// <summary>Resultado da validação de um conjunto de conteúdo.</summary>
public sealed class ContentValidationReport(IReadOnlyList<ContentIssue> issues)
{
    public IReadOnlyList<ContentIssue> Issues { get; } = issues;
    public IEnumerable<ContentIssue> Errors => Issues.Where(i => i.Severity == IssueSeverity.Error);
    public IEnumerable<ContentIssue> Warnings => Issues.Where(i => i.Severity == IssueSeverity.Warning);
    public bool IsValid => !Errors.Any();
}

/// <summary>Arquivo ausente, JSON inválido ou incompatível com o contrato.</summary>
public sealed class ContentLoadException(IReadOnlyList<ContentIssue> issues)
    : Exception("Falha ao carregar conteúdo:\n" + string.Join("\n", issues))
{
    public IReadOnlyList<ContentIssue> Issues { get; } = issues;
}

/// <summary>Conteúdo carregado, mas reprovado pelo <see cref="ContentValidator"/>.</summary>
public sealed class ContentValidationException(ContentValidationReport report)
    : Exception("Conteúdo inválido:\n" + string.Join("\n", report.Errors))
{
    public ContentValidationReport Report { get; } = report;
}
