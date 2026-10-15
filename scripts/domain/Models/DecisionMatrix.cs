using System.Collections.Generic;

namespace WhatYourPain.Domain.Models;

/// <summary>
/// Consequência de uma ação em um caso (data-model, ActionOutcome; chamado de
/// "ActionConsequence" em plan.md/tasks.md — ver INC-003 em docs/decisions.md).
/// </summary>
/// <remarks>
/// Valores monetários são magnitudes não negativas; o sinal é aplicado ao gerar
/// <see cref="FinancialEntry"/>. Campos null significam "não definido": um resultado
/// resolvido precisa de todos os efeitos; um UNRESOLVED não pode ter nenhum.
/// </remarks>
public sealed record ActionOutcome
{
    public required TriageAction Action { get; init; }
    public required TriageClassification Classification { get; init; }
    public IReadOnlyList<string> ReasonRuleIds { get; init; } = [];
    public decimal? CommissionDelta { get; init; }
    public decimal? FineDelta { get; init; }
    public decimal? SpecialCostDelta { get; init; }
    public int? ReputationDelta { get; init; }
    public int? ConscienceDelta { get; init; }
    public string? DestinationId { get; init; }
    public string? ExitDialogueId { get; init; }
    public IReadOnlyList<string> AuditLogIds { get; init; } = [];
    public IReadOnlyDictionary<string, string> FlagsToSet { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Extensão técnica: IDs de docs/decisions.md (O-xxx, L-xxx) dos quais este resultado
    /// depende. Obrigatório em UNRESOLVED; em resultado resolvido indica pendência parcial.
    /// </summary>
    public IReadOnlyList<string> OpenDecisionIds { get; init; } = [];

    public string? Notes { get; init; }
}

/// <summary>
/// Matriz de um paciente: uma consequência por ação (chamado de "DecisionCase" em
/// plan.md/tasks.md — ver INC-003).
/// </summary>
public sealed record DecisionCase
{
    public required string Id { get; init; }
    public required string PatientId { get; init; }
    public required IReadOnlyList<ActionOutcome> Outcomes { get; init; }
    public string? Notes { get; init; }
}

/// <summary>Destino visual conhecido.</summary>
public sealed record DestinationData
{
    public required string Id { get; init; }
    public required string Label { get; init; }
    public string? Source { get; init; }
}

/// <summary>Registro de auditoria conhecido (texto exibido ao jogador/QA).</summary>
public sealed record AuditLogData
{
    public required string Id { get; init; }
    public required string Text { get; init; }
    public string? Source { get; init; }
}

/// <summary>Conteúdo de <c>content/decisions/day_N_decision_matrix.json</c> (data-model, DecisionMatrix).</summary>
public sealed record DecisionMatrix
{
    public required string Id { get; init; }
    public required string ProtocolId { get; init; }
    public required IReadOnlyList<DecisionCase> Cases { get; init; }
    public IReadOnlyList<DestinationData> Destinations { get; init; } = [];
    public IReadOnlyList<AuditLogData> AuditLogs { get; init; } = [];
    public string? Notes { get; init; }
}
