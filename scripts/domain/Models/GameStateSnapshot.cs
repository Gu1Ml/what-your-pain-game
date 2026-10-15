using System.Collections.Generic;

namespace WhatYourPain.Domain.Models;

/// <summary>Fase serializável da campanha (nunca uma referência a Node).</summary>
public enum CampaignPhase
{
    NotStarted,
    InDay,
    DayClosed,
}

/// <summary>
/// Snapshot persistível do estado (data-model, GameStateSnapshot; "GameSaveData" em
/// plan.md/tasks.md — ver INC-003). Só valores serializáveis.
/// </summary>
public sealed record GameStateSnapshot
{
    public const int CurrentSchemaVersion = 1;

    public required int SchemaVersion { get; init; }
    public required int CurrentDay { get; init; }
    public required CampaignPhase CampaignPhase { get; init; }
    public required decimal Money { get; init; }

    /// <summary>Representação separada de dívida ainda em aberto (O-006); preservada, não calculada.</summary>
    public required decimal Debt { get; init; }

    public required int Reputation { get; init; }
    public required int Conscience { get; init; }
    public required int ErrorsToday { get; init; }
    public required int PatientsProcessedToday { get; init; }
    public required decimal CommissionToday { get; init; }
    public required bool DailyExpenseApplied { get; init; }
    public required bool DogAdopted { get; init; }
    public required bool OvertimeAccepted { get; init; }

    /// <summary>Escala ainda não especificada (O-011); sem efeito nesta fase.</summary>
    public required int FatigueLevel { get; init; }

    public required IReadOnlyDictionary<string, string> PatientFlags { get; init; }
    public required IReadOnlyDictionary<string, string> NarrativeFlags { get; init; }
    public required IReadOnlyList<string> ProcessedPatientIds { get; init; }
    public required IReadOnlyList<FinancialEntry> FinancialLedger { get; init; }
    public int? RandomSeed { get; init; }
}
