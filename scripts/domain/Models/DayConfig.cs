using System.Collections.Generic;

namespace WhatYourPain.Domain.Models;

/// <summary>Configuração de um dia da campanha (data-model, DayConfig).</summary>
public sealed record DayConfig
{
    public required int DayNumber { get; init; }
    public required string ProtocolId { get; init; }
    public required int BaseDurationSeconds { get; init; }

    /// <summary>IDs dos pacientes de história na ordem relativa obrigatória (D-006).</summary>
    public required IReadOnlyList<string> StoryPatientOrder { get; init; }

    public required bool RoutineGenerationEnabled { get; init; }
    public required decimal DailyLivingExpense { get; init; }

    /// <summary>
    /// Extensão técnica: valor nominal de comissão do folheto (D-009). Só o valor está fechado;
    /// o gatilho global da comissão continua em aberto (O-002).
    /// </summary>
    public required decimal NominalCommission { get; init; }

    /// <summary>Extensão técnica: multa genérica única por paciente com erro (D-010, D-015).</summary>
    public required decimal GenericErrorFine { get; init; }
}

/// <summary>Marca narrativa conhecida e os valores que ela pode assumir.</summary>
public sealed record NarrativeFlag
{
    public required string Id { get; init; }
    public required IReadOnlyList<string> AllowedValues { get; init; }
    public string? Description { get; init; }
}

/// <summary>Conteúdo de <c>content/campaign.json</c>.</summary>
public sealed record CampaignData
{
    public required string Id { get; init; }
    public required int TotalDays { get; init; }

    /// <summary>Saldo inicial da campanha. Não definido nos documentos: null até decisão (L-001).</summary>
    public decimal? StartingMoney { get; init; }

    public required IReadOnlyList<DayConfig> Days { get; init; }
    public required IReadOnlyList<NarrativeFlag> NarrativeFlags { get; init; }
    public string? Notes { get; init; }
}
