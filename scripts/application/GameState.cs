using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WhatYourPain.Domain.Models;

namespace WhatYourPain.Application;

/// <summary>
/// Estado persistente da campanha e do dia; única porta de alteração persistente (plan.md).
/// Classe C# pura, sem dependência de Godot, para ser testável fora da cena. O registro
/// como Autoload (proposta técnica do plano) fica para quando houver cena que o use.
/// </summary>
/// <remarks>
/// Esta classe só aplica valores já calculados. Ela não decide classificação, comissão,
/// multa ou bônus (EconomyManager/ReputationManager, fases seguintes).
/// </remarks>
public sealed class GameState
{
    /// <summary>Ponto neutro inicial de reputação e consciência (D-003); limiares em aberto (O-010).</summary>
    public const int NeutralMetricValue = 0;

    public const string DogAdoptedFlag = "dog_adopted";

    private readonly Dictionary<string, string> _patientFlags = new();
    private readonly Dictionary<string, string> _narrativeFlags = new();
    private readonly List<string> _processedPatientIds = new();
    private readonly List<FinancialEntry> _ledger = new();

    private GameState()
    {
        // Visões somente leitura: impedem alterar o estado por cast da coleção exposta.
        PatientFlags = new ReadOnlyDictionary<string, string>(_patientFlags);
        NarrativeFlags = new ReadOnlyDictionary<string, string>(_narrativeFlags);
        ProcessedPatientIds = _processedPatientIds.AsReadOnly();
        FinancialLedger = _ledger.AsReadOnly();
    }

    public int CurrentDay { get; private set; }
    public CampaignPhase CampaignPhase { get; private set; }
    public decimal Money { get; private set; }

    /// <summary>Preservada a partir do snapshot; nunca calculada aqui (O-006).</summary>
    public decimal Debt { get; private set; }

    public int Reputation { get; private set; }
    public int Conscience { get; private set; }
    public int ErrorsToday { get; private set; }
    public int PatientsProcessedToday { get; private set; }
    public decimal CommissionToday { get; private set; }
    public bool DailyExpenseApplied { get; private set; }
    public bool OvertimeAccepted { get; private set; }
    public int FatigueLevel { get; private set; }
    public int? RandomSeed { get; private set; }

    public bool DogAdopted => _narrativeFlags.TryGetValue(DogAdoptedFlag, out var value) && value == "true";

    public IReadOnlyDictionary<string, string> PatientFlags { get; }
    public IReadOnlyDictionary<string, string> NarrativeFlags { get; }
    public IReadOnlyList<string> ProcessedPatientIds { get; }
    public IReadOnlyList<FinancialEntry> FinancialLedger { get; }

    /// <summary>
    /// Cria uma campanha nova. O saldo inicial é obrigatório e explícito porque os documentos
    /// ainda não o definem (L-001); não há valor padrão escondido.
    /// </summary>
    public static GameState CreateNew(decimal startingMoney) => new()
    {
        CurrentDay = 0,
        CampaignPhase = CampaignPhase.NotStarted,
        Money = startingMoney,
        Reputation = NeutralMetricValue,
        Conscience = NeutralMetricValue,
    };

    public static GameState FromSnapshot(GameStateSnapshot snapshot)
    {
        var state = new GameState();
        state.RestoreSnapshot(snapshot);
        return state;
    }

    /// <summary>Inicia o próximo dia e zera os contadores diários.</summary>
    public void BeginDay(int dayNumber, int? randomSeed = null)
    {
        if (CampaignPhase == CampaignPhase.InDay)
            throw new InvalidOperationException($"O dia {CurrentDay} ainda está em andamento.");
        if (dayNumber != CurrentDay + 1)
            throw new InvalidOperationException($"Dia esperado: {CurrentDay + 1}; recebido: {dayNumber}.");

        CurrentDay = dayNumber;
        CampaignPhase = CampaignPhase.InDay;
        ErrorsToday = 0;
        PatientsProcessedToday = 0;
        CommissionToday = 0;
        DailyExpenseApplied = false;
        _processedPatientIds.Clear();
        RandomSeed = randomSeed;
    }

    /// <summary>Define uma marca narrativa conhecida (validação de ID/valor é do conteúdo).</summary>
    public void SetFlag(string flagId, string value)
    {
        if (string.IsNullOrWhiteSpace(flagId))
            throw new ArgumentException("ID de marca vazio.", nameof(flagId));
        ArgumentNullException.ThrowIfNull(value);
        _narrativeFlags[flagId] = value;
    }

    /// <summary>
    /// Aplica uma única vez um resultado já calculado. Recusa UNRESOLVED, paciente repetido no
    /// dia e lançamento já presente no ledger; nada é alterado se a validação falhar.
    /// </summary>
    public void ApplyTriageResult(TriageResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        RequirePhase(CampaignPhase.InDay);
        result.EnsureConsistent();

        if (result.Classification == TriageClassification.Unresolved)
            throw new InvalidOperationException(
                $"Resultado UNRESOLVED de '{result.PatientId}' não pode ser aplicado " +
                $"(pendências: {string.Join(", ", result.OpenDecisionIds)}).");
        if (_processedPatientIds.Contains(result.PatientId))
            throw new InvalidOperationException($"Paciente '{result.PatientId}' já foi processado hoje.");

        EnsureEntriesCanBeApplied(result.FinancialEntries);

        ApplyEntries(result.FinancialEntries);
        Reputation += result.ReputationDelta;
        Conscience += result.ConscienceDelta;
        foreach (var (flagId, value) in result.FlagsToSet)
            _narrativeFlags[flagId] = value;

        _processedPatientIds.Add(result.PatientId);
        PatientsProcessedToday++;
        // Só erro genérico conta; a exceção ética de Helena não (D-016, D-019).
        if (result.Classification == TriageClassification.Error)
            ErrorsToday++;
    }

    /// <summary>
    /// Aplica os lançamentos de fechamento do dia. Exige exatamente uma despesa diária e
    /// impede reaplicá-la (FR-017). Lançamentos de triagem não devem ser repassados aqui.
    /// </summary>
    public void ApplyDailyFinancials(IReadOnlyList<FinancialEntry> closingEntries)
    {
        ArgumentNullException.ThrowIfNull(closingEntries);
        RequirePhase(CampaignPhase.InDay);
        if (DailyExpenseApplied)
            throw new InvalidOperationException($"A despesa diária do dia {CurrentDay} já foi aplicada.");

        var dailyExpenses = closingEntries.Count(e => e.EntryType == FinancialEntryType.DailyExpense);
        if (dailyExpenses != 1)
            throw new InvalidOperationException(
                $"O fechamento deve conter exatamente uma despesa diária; recebidas: {dailyExpenses}.");
        if (closingEntries.Any(e => e.PatientId is not null))
            throw new InvalidOperationException("Lançamentos de fechamento não pertencem a um paciente.");

        EnsureEntriesCanBeApplied(closingEntries);
        ApplyEntries(closingEntries);
        DailyExpenseApplied = true;
        CampaignPhase = CampaignPhase.DayClosed;
    }

    public GameStateSnapshot CreateSnapshot() => new()
    {
        SchemaVersion = GameStateSnapshot.CurrentSchemaVersion,
        CurrentDay = CurrentDay,
        CampaignPhase = CampaignPhase,
        Money = Money,
        Debt = Debt,
        Reputation = Reputation,
        Conscience = Conscience,
        ErrorsToday = ErrorsToday,
        PatientsProcessedToday = PatientsProcessedToday,
        CommissionToday = CommissionToday,
        DailyExpenseApplied = DailyExpenseApplied,
        DogAdopted = DogAdopted,
        OvertimeAccepted = OvertimeAccepted,
        FatigueLevel = FatigueLevel,
        PatientFlags = new Dictionary<string, string>(_patientFlags),
        NarrativeFlags = new Dictionary<string, string>(_narrativeFlags),
        ProcessedPatientIds = _processedPatientIds.ToList(),
        FinancialLedger = _ledger.ToList(),
        RandomSeed = RandomSeed,
    };

    /// <summary>Substitui o estado pelo snapshot, validando-o antes de alterar qualquer campo.</summary>
    public void RestoreSnapshot(GameStateSnapshot snapshot)
    {
        ValidateSnapshot(snapshot);

        CurrentDay = snapshot.CurrentDay;
        CampaignPhase = snapshot.CampaignPhase;
        Money = snapshot.Money;
        Debt = snapshot.Debt;
        Reputation = snapshot.Reputation;
        Conscience = snapshot.Conscience;
        ErrorsToday = snapshot.ErrorsToday;
        PatientsProcessedToday = snapshot.PatientsProcessedToday;
        CommissionToday = snapshot.CommissionToday;
        DailyExpenseApplied = snapshot.DailyExpenseApplied;
        OvertimeAccepted = snapshot.OvertimeAccepted;
        FatigueLevel = snapshot.FatigueLevel;
        RandomSeed = snapshot.RandomSeed;

        Replace(_patientFlags, snapshot.PatientFlags);
        Replace(_narrativeFlags, snapshot.NarrativeFlags);
        _processedPatientIds.Clear();
        _processedPatientIds.AddRange(snapshot.ProcessedPatientIds);
        _ledger.Clear();
        _ledger.AddRange(snapshot.FinancialLedger);
    }

    private static void ValidateSnapshot(GameStateSnapshot? snapshot)
    {
        if (snapshot is null)
            throw new InvalidSnapshotException("Snapshot ausente.");
        if (snapshot.SchemaVersion != GameStateSnapshot.CurrentSchemaVersion)
            throw new InvalidSnapshotException(
                $"schemaVersion {snapshot.SchemaVersion} não suportada (esperada {GameStateSnapshot.CurrentSchemaVersion}).");
        if (snapshot.CurrentDay < 0)
            throw new InvalidSnapshotException("currentDay negativo.");
        if (snapshot.PatientFlags is null || snapshot.NarrativeFlags is null
            || snapshot.ProcessedPatientIds is null || snapshot.FinancialLedger is null)
            throw new InvalidSnapshotException("Coleção obrigatória ausente no snapshot.");
        if (snapshot.ProcessedPatientIds.Distinct().Count() != snapshot.ProcessedPatientIds.Count)
            throw new InvalidSnapshotException("processedPatientIds contém IDs repetidos.");
        if (snapshot.FinancialLedger.Any(e => e is null || string.IsNullOrWhiteSpace(e.Id)))
            throw new InvalidSnapshotException("financialLedger contém lançamento sem ID.");
        if (snapshot.FinancialLedger.Select(e => e.Id).Distinct().Count() != snapshot.FinancialLedger.Count)
            throw new InvalidSnapshotException("financialLedger contém IDs repetidos.");

        var flagSaysAdopted = snapshot.NarrativeFlags.TryGetValue(DogAdoptedFlag, out var v) && v == "true";
        if (flagSaysAdopted != snapshot.DogAdopted)
            throw new InvalidSnapshotException("dogAdopted diverge da marca narrativa 'dog_adopted'.");
    }

    private void EnsureEntriesCanBeApplied(IReadOnlyList<FinancialEntry> entries)
    {
        var known = new HashSet<string>(_ledger.Select(e => e.Id));
        foreach (var entry in entries)
        {
            if (string.IsNullOrWhiteSpace(entry.Id))
                throw new InvalidOperationException("Lançamento sem ID.");
            if (entry.DayNumber != CurrentDay)
                throw new InvalidOperationException($"Lançamento '{entry.Id}' é do dia {entry.DayNumber}, não do dia {CurrentDay}.");
            if (!known.Add(entry.Id))
                throw new InvalidOperationException($"Lançamento '{entry.Id}' já foi aplicado.");
        }
    }

    private void ApplyEntries(IEnumerable<FinancialEntry> entries)
    {
        foreach (var entry in entries)
        {
            _ledger.Add(entry);
            Money += entry.Amount;
            if (entry.EntryType == FinancialEntryType.Commission)
                CommissionToday += entry.Amount;
        }
    }

    private void RequirePhase(CampaignPhase expected)
    {
        if (CampaignPhase != expected)
            throw new InvalidOperationException($"Operação exige a fase {expected}; fase atual: {CampaignPhase}.");
    }

    private static void Replace(Dictionary<string, string> target, IReadOnlyDictionary<string, string> source)
    {
        target.Clear();
        foreach (var (key, value) in source)
            target[key] = value;
    }
}

/// <summary>Snapshot rejeitado na restauração; o estado atual permanece intacto.</summary>
public sealed class InvalidSnapshotException(string message) : Exception(message);
