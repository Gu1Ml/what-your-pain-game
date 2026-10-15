using System.Collections.Generic;
using System.Linq;

namespace WhatYourPain.Domain.Models;

/// <summary>
/// Lançamento financeiro (data-model, FinancialEntry). <see cref="Amount"/> é o efeito
/// assinado sobre o saldo: comissão positiva; multa, custo ético e despesa negativos.
/// O <see cref="Id"/> é único no ledger e impede aplicar o mesmo lançamento duas vezes.
/// </summary>
public sealed record FinancialEntry
{
    public required string Id { get; init; }
    public required int DayNumber { get; init; }
    public string? PatientId { get; init; }
    public required FinancialEntryType EntryType { get; init; }
    public required decimal Amount { get; init; }
    public string? Description { get; init; }
}

/// <summary>
/// Visão calculada das finanças de um dia (spec §11). É derivada do ledger e não aplica
/// nada por si só; o modelo de dívida continua em aberto (O-006).
/// </summary>
public sealed record DailyFinancialResult
{
    public required int DayNumber { get; init; }
    public required IReadOnlyList<FinancialEntry> Entries { get; init; }

    public decimal CommissionTotal => Sum(FinancialEntryType.Commission);
    public decimal FineTotal => Sum(FinancialEntryType.Fine);
    public decimal EthicalCostTotal => Sum(FinancialEntryType.EthicalCost);
    public decimal DailyExpenseTotal => Sum(FinancialEntryType.DailyExpense);
    public decimal DayBalance => Entries.Sum(e => e.Amount);

    public static DailyFinancialResult FromLedger(int dayNumber, IEnumerable<FinancialEntry> ledger) => new()
    {
        DayNumber = dayNumber,
        Entries = ledger.Where(e => e.DayNumber == dayNumber).ToList(),
    };

    private decimal Sum(FinancialEntryType type) => Entries.Where(e => e.EntryType == type).Sum(e => e.Amount);
}

/// <summary>
/// Dados já calculados para o resumo do plantão (US4, cenário 5). A UI apenas exibe;
/// a montagem única fica para T034.
/// </summary>
public sealed record DaySummary
{
    public required int DayNumber { get; init; }
    public required int PatientsProcessed { get; init; }
    public required IReadOnlyDictionary<TriageClassification, int> ClassificationCounts { get; init; }
    public required DailyFinancialResult Financials { get; init; }
    public required decimal MoneyAfter { get; init; }

    /// <summary>Representação da dívida em aberto (O-006); não calculada nesta fase.</summary>
    public decimal? DebtAfter { get; init; }

    public required int ReputationAfter { get; init; }
    public required int ConscienceAfter { get; init; }
    public string? NextEventId { get; init; }
}
