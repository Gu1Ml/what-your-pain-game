using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatYourPain.Domain.Models;

/// <summary>
/// Registro imutável de uma decisão processada (data-model, TriageResult). A UI consome;
/// não recalcula. Um resultado UNRESOLVED nunca carrega consequência definitiva: isso é
/// verificado na criação e o <c>GameState</c> se recusa a aplicá-lo.
/// </summary>
public sealed record TriageResult
{
    public required string PatientId { get; init; }
    public required TriageAction Action { get; init; }
    public required TriageClassification Classification { get; init; }
    public IReadOnlyList<string> ReasonRuleIds { get; init; } = [];
    public IReadOnlyList<FinancialEntry> FinancialEntries { get; init; } = [];
    public int ReputationDelta { get; init; }
    public int ConscienceDelta { get; init; }
    public string? DestinationId { get; init; }
    public string? ExitDialogueId { get; init; }

    /// <summary>IDs de auditoria; T025 pode trocar o tipo do elemento por AuditLogEntry.</summary>
    public IReadOnlyList<string> AuditEntries { get; init; } = [];

    public IReadOnlyDictionary<string, string> FlagsToSet { get; init; } = new Dictionary<string, string>();

    /// <summary>Pendências que impediram a classificação (obrigatório em UNRESOLVED).</summary>
    public IReadOnlyList<string> OpenDecisionIds { get; init; } = [];

    /// <summary>True quando o resultado carrega algum efeito que mudaria o estado.</summary>
    public bool HasDefinitiveEffects =>
        FinancialEntries.Count > 0
        || ReputationDelta != 0
        || ConscienceDelta != 0
        || FlagsToSet.Count > 0
        || AuditEntries.Count > 0
        || DestinationId is not null;

    /// <summary>Lança se o resultado violar as invariantes do domínio.</summary>
    public void EnsureConsistent()
    {
        if (string.IsNullOrWhiteSpace(PatientId))
            throw new InvalidOperationException("TriageResult sem patientId.");

        if (Classification == TriageClassification.Unresolved)
        {
            if (HasDefinitiveEffects)
                throw new InvalidOperationException(
                    $"TriageResult UNRESOLVED de '{PatientId}' ({Action}) carrega efeitos definitivos.");
            if (OpenDecisionIds.Count == 0)
                throw new InvalidOperationException(
                    $"TriageResult UNRESOLVED de '{PatientId}' ({Action}) não informa a pendência (openDecisionIds).");
        }

        if (FinancialEntries.Count(e => e.EntryType == FinancialEntryType.Fine) > 1)
            throw new InvalidOperationException(
                $"TriageResult de '{PatientId}' tem mais de uma multa; a regra é multa única por paciente (D-015).");

        if (FinancialEntries.Any(e => e.PatientId != PatientId))
            throw new InvalidOperationException(
                $"TriageResult de '{PatientId}' contém lançamento de outro paciente.");
    }
}
