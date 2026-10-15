using System.Collections.Generic;
using System.Text.Json;

namespace WhatYourPain.Domain.Models;

/// <summary>Condição declarativa simples; sem scripts nem expressões vindas do JSON.</summary>
public sealed record RuleCondition
{
    public required string Field { get; init; }
    public required RuleOperator Operator { get; init; }

    /// <summary>Valor JSON bruto (número, texto ou booleano); ausente para IS_TRUE/IS_FALSE.</summary>
    public JsonElement? Value { get; init; }
}

/// <summary>Regra do protocolo (data-model, ProtocolRule).</summary>
public sealed record ProtocolRule
{
    public required string Id { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public required int RuleGroup { get; init; }
    public IReadOnlyList<RuleCondition> Conditions { get; init; } = [];
    public IReadOnlyList<TriageAction> AppliesToActions { get; init; } = [];

    /// <summary>Deve permanecer null enquanto a precedência estiver em aberto (O-003).</summary>
    public int? Priority { get; init; }
}

/// <summary>Grupo numerado do folheto (as três categorias do Dia 1, FR-002).</summary>
public sealed record ProtocolRuleGroup
{
    public required int Number { get; init; }
    public required string Title { get; init; }
}

/// <summary>Estado de um aviso do folheto em relação ao registro de decisões.</summary>
public enum NoticeStatus
{
    /// <summary>A afirmação corresponde a decisões fechadas (<see cref="ProtocolNotice.DecisionIds"/>).</summary>
    Approved,

    /// <summary>A afirmação depende de pendência; não pode ser apresentada como regra aprovada.</summary>
    Open,
}

/// <summary>
/// Linha literal do bloco de avisos do folheto (ex.: "[ ATENÇÃO ]"). O texto é exibível;
/// o status diz se ele já corresponde a uma regra aprovada.
/// </summary>
public sealed record ProtocolNotice
{
    public required string Id { get; init; }
    public required string Text { get; init; }
    public required NoticeStatus Status { get; init; }

    /// <summary>Decisões fechadas (D-xxx) que sustentam o aviso.</summary>
    public IReadOnlyList<string> DecisionIds { get; init; } = [];

    /// <summary>Pendências (O-xxx, L-xxx, INC-xxx); obrigatório quando OPEN.</summary>
    public IReadOnlyList<string> OpenDecisionIds { get; init; } = [];

    public string? Notes { get; init; }
}

/// <summary>Protocolo diário (conteúdo de <c>content/protocols/day_N_protocol.json</c>).</summary>
public sealed record ProtocolData
{
    public required string Id { get; init; }
    public required int DayNumber { get; init; }

    /// <summary>Título literal do folheto.</summary>
    public string? Title { get; init; }

    /// <summary>Cabeçalho literal da seção de regras.</summary>
    public string? RulesHeading { get; init; }

    public required IReadOnlyList<ProtocolRuleGroup> RuleGroups { get; init; }
    public required IReadOnlyList<ProtocolRule> Rules { get; init; }

    /// <summary>Cabeçalho literal do bloco de avisos.</summary>
    public string? NoticesHeading { get; init; }

    public IReadOnlyList<ProtocolNotice> Notices { get; init; } = [];
    public string? Notes { get; init; }
}
