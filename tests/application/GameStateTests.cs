using System;
using System.Collections.Generic;
using System.Text.Json;
using WhatYourPain.Application;
using WhatYourPain.Content;
using WhatYourPain.Domain.Models;
using Xunit;

namespace WhatYourPain.Tests.Application;

/// <summary>T006: mutações controladas, aplicação única e snapshot/restauração.</summary>
public sealed class GameStateTests
{
    private static GameState StartedDay()
    {
        // Saldo inicial arbitrário de teste: o valor real não está definido (L-001).
        var state = GameState.CreateNew(startingMoney: 100m);
        state.BeginDay(1, randomSeed: 42);
        return state;
    }

    private static FinancialEntry Entry(string id, FinancialEntryType type, decimal amount, string? patientId) => new()
    {
        Id = id,
        DayNumber = 1,
        PatientId = patientId,
        EntryType = type,
        Amount = amount,
    };

    private static TriageResult ErrorResult(string patientId) => new()
    {
        PatientId = patientId,
        Action = TriageAction.Attend,
        Classification = TriageClassification.Error,
        ReasonRuleIds = ["D1_DOCUMENT_VALID", "D1_NO_IMMEDIATE_MILD_OR_LASTING"],
        FinancialEntries = [Entry($"{patientId}:fine", FinancialEntryType.Fine, -5m, patientId)],
        AuditEntries = ["AUD_D1_EXPIRED_RG_AND_NO_IMMEDIATE"],
    };

    private static TriageResult HelenaHelped() => new()
    {
        PatientId = "helena_rios",
        Action = TriageAction.Attend,
        Classification = TriageClassification.EthicalException,
        ReasonRuleIds = ["D1_NO_IMMEDIATE_PRESCRIPTION_INVALID"],
        FinancialEntries =
        [
            Entry("helena_rios:commission", FinancialEntryType.Commission, 5m, "helena_rios"),
            Entry("helena_rios:ethical", FinancialEntryType.EthicalCost, -10m, "helena_rios"),
        ],
        ReputationDelta = -1,
        ConscienceDelta = 1,
        DestinationId = "hospital_pharmacy",
        AuditEntries = ["AUD_D1_EXPIRED_PRESCRIPTION_RELEASED"],
        FlagsToSet = new Dictionary<string, string> { ["helena_outcome"] = "helped" },
    };

    private static TriageResult Unresolved(string patientId) => new()
    {
        PatientId = patientId,
        Action = TriageAction.Queue,
        Classification = TriageClassification.Unresolved,
        OpenDecisionIds = ["O-001", "O-015"],
    };

    [Fact]
    public void CreateNew_StartsAtNeutralMetrics_AndNotStarted()
    {
        var state = GameState.CreateNew(startingMoney: 0m);

        Assert.Equal(GameState.NeutralMetricValue, state.Reputation);
        Assert.Equal(GameState.NeutralMetricValue, state.Conscience);
        Assert.Equal(CampaignPhase.NotStarted, state.CampaignPhase);
        Assert.Equal(0, state.CurrentDay);
        Assert.False(state.DogAdopted);
    }

    [Fact]
    public void ApplyTriageResult_RefusesUnresolved_AndLeavesStateUntouched()
    {
        var state = StartedDay();
        var before = JsonSerializer.Serialize(state.CreateSnapshot(), ContentJson.Options);

        var ex = Assert.Throws<InvalidOperationException>(() => state.ApplyTriageResult(Unresolved("camila_torres")));

        Assert.Contains("O-001", ex.Message);
        Assert.Equal(before, JsonSerializer.Serialize(state.CreateSnapshot(), ContentJson.Options));
    }

    [Fact]
    public void UnresolvedResultCarryingEffects_IsInconsistent()
    {
        var withMoney = Unresolved("camila_torres") with
        {
            FinancialEntries = [Entry("camila:commission", FinancialEntryType.Commission, 5m, "camila_torres")],
        };
        var withMetric = Unresolved("camila_torres") with { ReputationDelta = 1 };
        var withoutPending = Unresolved("camila_torres") with { OpenDecisionIds = [] };

        Assert.True(withMoney.HasDefinitiveEffects);
        Assert.Throws<InvalidOperationException>(withMoney.EnsureConsistent);
        Assert.Throws<InvalidOperationException>(withMetric.EnsureConsistent);
        Assert.Throws<InvalidOperationException>(withoutPending.EnsureConsistent);
    }

    [Fact]
    public void ApplyTriageResult_GenericError_CountsErrorAndAppliesSingleFine()
    {
        var state = StartedDay();

        state.ApplyTriageResult(ErrorResult("camila_torres"));

        Assert.Equal(95m, state.Money);
        Assert.Equal(1, state.ErrorsToday);
        Assert.Equal(1, state.PatientsProcessedToday);
        Assert.Single(state.FinancialLedger);
    }

    [Fact]
    public void ResultWithTwoFines_IsRejected()
    {
        var state = StartedDay();
        var twoFines = ErrorResult("camila_torres") with
        {
            FinancialEntries =
            [
                Entry("camila:fine:1", FinancialEntryType.Fine, -5m, "camila_torres"),
                Entry("camila:fine:2", FinancialEntryType.Fine, -5m, "camila_torres"),
            ],
        };

        Assert.Throws<InvalidOperationException>(() => state.ApplyTriageResult(twoFines));
        Assert.Empty(state.FinancialLedger);
    }

    [Fact]
    public void EthicalException_IsNotCountedAsGenericError()
    {
        var state = StartedDay();

        state.ApplyTriageResult(HelenaHelped());

        Assert.Equal(0, state.ErrorsToday);
        Assert.Equal(95m, state.Money);           // +5 −10
        Assert.Equal(5m, state.CommissionToday);
        Assert.Equal(-1, state.Reputation);
        Assert.Equal(1, state.Conscience);
        Assert.Equal("helped", state.NarrativeFlags["helena_outcome"]);
    }

    [Fact]
    public void SamePatientTwiceInADay_IsRejected()
    {
        var state = StartedDay();
        state.ApplyTriageResult(ErrorResult("camila_torres"));

        var again = ErrorResult("camila_torres") with
        {
            FinancialEntries = [Entry("camila_torres:fine:again", FinancialEntryType.Fine, -5m, "camila_torres")],
        };

        Assert.Throws<InvalidOperationException>(() => state.ApplyTriageResult(again));
        Assert.Equal(95m, state.Money);
    }

    [Fact]
    public void LedgerEntryAppliedTwice_IsRejected_WithoutPartialChanges()
    {
        var state = StartedDay();
        state.ApplyTriageResult(ErrorResult("camila_torres"));

        var reusedEntry = ErrorResult("lucas_mendes") with
        {
            FinancialEntries = [Entry("camila_torres:fine", FinancialEntryType.Fine, -5m, "lucas_mendes")],
        };

        Assert.Throws<InvalidOperationException>(() => state.ApplyTriageResult(reusedEntry));
        Assert.Equal(1, state.PatientsProcessedToday);
        Assert.Equal(1, state.ErrorsToday);
    }

    [Fact]
    public void ApplyTriageResult_OutsideADay_IsRejected()
    {
        var state = GameState.CreateNew(startingMoney: 0m);

        Assert.Throws<InvalidOperationException>(() => state.ApplyTriageResult(ErrorResult("camila_torres")));
    }

    [Fact]
    public void DailyExpense_IsAppliedOnlyOnce()
    {
        var state = StartedDay();
        var expense = Entry("day1:expense", FinancialEntryType.DailyExpense, -25m, null);

        state.ApplyDailyFinancials([expense]);

        Assert.True(state.DailyExpenseApplied);
        Assert.Equal(75m, state.Money);
        Assert.Equal(CampaignPhase.DayClosed, state.CampaignPhase);
        Assert.Throws<InvalidOperationException>(
            () => state.ApplyDailyFinancials([Entry("day1:expense:2", FinancialEntryType.DailyExpense, -25m, null)]));
        Assert.Equal(75m, state.Money);
    }

    [Fact]
    public void DailyClose_RequiresExactlyOneDailyExpense()
    {
        var state = StartedDay();

        Assert.Throws<InvalidOperationException>(() => state.ApplyDailyFinancials([]));
        Assert.Throws<InvalidOperationException>(() => state.ApplyDailyFinancials(
        [
            Entry("a", FinancialEntryType.DailyExpense, -25m, null),
            Entry("b", FinancialEntryType.DailyExpense, -25m, null),
        ]));
        Assert.False(state.DailyExpenseApplied);
    }

    [Fact]
    public void DailyFinancialResult_IsDerivedFromLedger_WithoutReapplying()
    {
        var state = StartedDay();
        state.ApplyTriageResult(ErrorResult("camila_torres"));
        state.ApplyTriageResult(HelenaHelped());
        state.ApplyDailyFinancials([Entry("day1:expense", FinancialEntryType.DailyExpense, -25m, null)]);
        var moneyAfterClose = state.Money;

        var result = DailyFinancialResult.FromLedger(1, state.FinancialLedger);

        Assert.Equal(5m, result.CommissionTotal);
        Assert.Equal(-5m, result.FineTotal);
        Assert.Equal(-10m, result.EthicalCostTotal);
        Assert.Equal(-25m, result.DailyExpenseTotal);
        Assert.Equal(-35m, result.DayBalance);
        Assert.Equal(moneyAfterClose, state.Money);
    }

    [Fact]
    public void BeginDay_ResetsDailyCounters_AndRequiresNextDay()
    {
        var state = StartedDay();
        state.ApplyTriageResult(ErrorResult("camila_torres"));
        state.ApplyDailyFinancials([Entry("day1:expense", FinancialEntryType.DailyExpense, -25m, null)]);

        Assert.Throws<InvalidOperationException>(() => state.BeginDay(3));
        state.BeginDay(2);

        Assert.Equal(0, state.ErrorsToday);
        Assert.Equal(0, state.PatientsProcessedToday);
        Assert.Equal(0m, state.CommissionToday);
        Assert.False(state.DailyExpenseApplied);
        Assert.Empty(state.ProcessedPatientIds);
        Assert.Equal(2, state.FinancialLedger.Count); // histórico preservado
    }

    [Fact]
    public void Snapshot_RoundTripsThroughJson()
    {
        var state = StartedDay();
        state.ApplyTriageResult(ErrorResult("camila_torres"));
        state.ApplyTriageResult(HelenaHelped());
        state.SetFlag(GameState.DogAdoptedFlag, "true");

        var json = JsonSerializer.Serialize(state.CreateSnapshot(), ContentJson.Options);
        var restored = GameState.FromSnapshot(JsonSerializer.Deserialize<GameStateSnapshot>(json, ContentJson.Options)!);

        Assert.Equal(json, JsonSerializer.Serialize(restored.CreateSnapshot(), ContentJson.Options));
        Assert.True(restored.DogAdopted);
        Assert.Equal(90m, restored.Money);
        Assert.Equal(42, restored.RandomSeed);
        Assert.Contains("\"schemaVersion\": 1", json);
    }

    [Fact]
    public void RestoreSnapshot_RejectsInvalidSnapshot_WithoutChangingState()
    {
        var state = StartedDay();
        state.ApplyTriageResult(ErrorResult("camila_torres"));
        var good = state.CreateSnapshot();
        var before = JsonSerializer.Serialize(good, ContentJson.Options);

        Assert.Throws<InvalidSnapshotException>(() => state.RestoreSnapshot(good with { SchemaVersion = 99 }));
        Assert.Throws<InvalidSnapshotException>(() => state.RestoreSnapshot(good with { ProcessedPatientIds = ["a", "a"] }));
        Assert.Throws<InvalidSnapshotException>(() => state.RestoreSnapshot(good with { DogAdopted = true }));

        Assert.Equal(before, JsonSerializer.Serialize(state.CreateSnapshot(), ContentJson.Options));
    }

    [Fact]
    public void ExposedCollections_CannotBeMutatedByCasting()
    {
        var state = StartedDay();

        Assert.True(((ICollection<FinancialEntry>)state.FinancialLedger).IsReadOnly);
        Assert.True(((ICollection<string>)state.ProcessedPatientIds).IsReadOnly);
        Assert.True(((ICollection<KeyValuePair<string, string>>)state.NarrativeFlags).IsReadOnly);
        Assert.True(((ICollection<KeyValuePair<string, string>>)state.PatientFlags).IsReadOnly);
    }

    [Fact]
    public void Snapshot_IsACopy_NotALiveView()
    {
        var state = StartedDay();
        var snapshot = state.CreateSnapshot();

        state.ApplyTriageResult(ErrorResult("camila_torres"));

        Assert.Empty(snapshot.FinancialLedger);
        Assert.Empty(snapshot.ProcessedPatientIds);
    }
}
