using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WhatYourPain.Content;
using WhatYourPain.Domain.Models;
using WhatYourPain.Tests.Support;
using Xunit;

namespace WhatYourPain.Tests.Content;

/// <summary>
/// Verifica o conteúdo real do Dia 1 contra as decisões fechadas de docs/decisions.md, e que
/// toda pendência continua explícita.
/// </summary>
public sealed class Day1ContentTests
{
    private static readonly DayContent Day = new DataLoader(new FileSystemContentSource(RepoPaths.ContentDirectory)).LoadDay(1);

    private static ActionOutcome Outcome(string caseId, TriageAction action) =>
        Day.DecisionMatrix.Cases.Single(c => c.Id == caseId).Outcomes.Single(o => o.Action == action);

    [Fact]
    public void StoryOrder_MatchesD006()
    {
        Assert.Equal(
            ["marcio_souza", "camila_torres", "lucas_mendes", "helena_rios", "roberto_paiva"],
            Day.DayConfig!.StoryPatientOrder);
    }

    [Fact]
    public void DayConfig_MatchesClosedValues()
    {
        var day = Day.DayConfig!;
        Assert.Equal(300, day.BaseDurationSeconds);   // D-007
        Assert.Equal(25m, day.DailyLivingExpense);    // D-008
        Assert.Equal(5m, day.NominalCommission);      // D-009 (valor)
        Assert.Equal(5m, day.GenericErrorFine);       // D-010 / D-015
        Assert.Equal(7, Day.Campaign.TotalDays);      // D-001
    }

    [Fact]
    public void Protocol_HasTheThreeRuleGroups()
    {
        Assert.Equal([1, 2, 3], Day.Protocol.RuleGroups.Select(g => g.Number).Order());
        Assert.All(Day.Protocol.RuleGroups, g => Assert.Contains(Day.Protocol.Rules, r => r.RuleGroup == g.Number));
        Assert.All(Day.Protocol.Rules, r => Assert.Null(r.Priority)); // O-003
    }

    [Fact]
    public void StartingMoney_StaysUndefined_AndIsReported()
    {
        Assert.Null(Day.Campaign.StartingMoney); // L-001
        Assert.Contains(Day.Report.Warnings, w => w.Code == IssueCodes.StartingMoneyUndefined);
    }

    [Fact]
    public void EveryUnresolvedOutcome_HasNoEffects_AndNamesItsPendingDecision()
    {
        var unresolved = Day.DecisionMatrix.Cases.SelectMany(c => c.Outcomes)
            .Where(o => o.Classification == TriageClassification.Unresolved)
            .ToList();

        Assert.NotEmpty(unresolved);
        Assert.All(unresolved, o =>
        {
            Assert.Null(o.CommissionDelta);
            Assert.Null(o.FineDelta);
            Assert.Null(o.SpecialCostDelta);
            Assert.Null(o.ReputationDelta);
            Assert.Null(o.ConscienceDelta);
            Assert.Null(o.DestinationId);
            Assert.Null(o.ExitDialogueId);
            Assert.Empty(o.AuditLogIds);
            Assert.Empty(o.FlagsToSet);
            Assert.NotEmpty(o.OpenDecisionIds);
        });
        Assert.Equal(unresolved.Count, Day.Report.Warnings.Count(w => w.Code == IssueCodes.UnresolvedOutcome));
    }

    [Fact]
    public void QueueAndRefer_StayUnresolved_WhereTheMappingIsOpen()
    {
        // O-001/O-015: a correspondência RECOMENDAR legado x FILA/ENCAMINHAR não foi decidida.
        foreach (var caseId in new[] { "d1_camila_torres", "d1_lucas_mendes", "d1_helena_rios", "d1_roberto_paiva" })
        foreach (var action in new[] { TriageAction.Queue, TriageAction.Refer })
        {
            var outcome = Outcome(caseId, action);
            Assert.Equal(TriageClassification.Unresolved, outcome.Classification);
            Assert.Contains("O-001", outcome.OpenDecisionIds);
        }
    }

    [Fact]
    public void Marcio_OnlyAttendIsWithoutError()
    {
        Assert.Equal(TriageClassification.Correct, Outcome("d1_marcio_souza", TriageAction.Attend).Classification);
        foreach (var action in new[] { TriageAction.Queue, TriageAction.Refer, TriageAction.Refuse })
            Assert.Equal(TriageClassification.Error, Outcome("d1_marcio_souza", action).Classification);
    }

    [Fact]
    public void GenericErrors_HaveSingleFiveDollarFine_AndNoCommission()
    {
        var errors = Day.DecisionMatrix.Cases.SelectMany(c => c.Outcomes)
            .Where(o => o.Classification == TriageClassification.Error)
            .ToList();

        Assert.NotEmpty(errors);
        Assert.All(errors, o =>
        {
            Assert.Equal(5m, o.FineDelta);
            Assert.Equal(0m, o.CommissionDelta);
            Assert.Equal(0, o.ReputationDelta); // O-014: sem penalidade geral definida
        });
        // Camila tem dois motivos e continua com uma única multa (D-015).
        Assert.Equal(2, Outcome("d1_camila_torres", TriageAction.Attend).ReasonRuleIds.Count);
    }

    [Fact]
    public void Helena_EthicalException_MatchesD016AndD017()
    {
        var helped = Outcome("d1_helena_rios", TriageAction.Attend);

        Assert.Equal(TriageClassification.EthicalException, helped.Classification);
        Assert.Equal(5m, helped.CommissionDelta);
        Assert.Equal(10m, helped.SpecialCostDelta);
        Assert.Equal(0m, helped.FineDelta);
        Assert.Equal(-1, helped.ReputationDelta);
        Assert.Equal(1, helped.ConscienceDelta);
        Assert.Equal("hospital_pharmacy", helped.DestinationId);
        Assert.Equal("helped", helped.FlagsToSet["helena_outcome"]);
        Assert.Contains("AUD_D1_EXPIRED_PRESCRIPTION_RELEASED", helped.AuditLogIds);
    }

    [Fact]
    public void Helena_FollowingProtocol_MatchesD018()
    {
        var refused = Outcome("d1_helena_rios", TriageAction.Refuse);

        Assert.Equal(5m, refused.CommissionDelta);
        Assert.Equal(1, refused.ReputationDelta);
        Assert.Equal(0, refused.ConscienceDelta);
        Assert.Equal(0m, refused.FineDelta);
        Assert.Equal("refused", refused.FlagsToSet["helena_outcome"]);
        Assert.Empty(refused.AuditLogIds); // auditoria só quando ajudada
    }

    [Fact]
    public void Lucas_ThermometerFact_AndVisitFlag()
    {
        var lucas = Day.Patients.Patients.Single(p => p.Id == "lucas_mendes");
        Assert.Contains("temperature_celsius=36.6", lucas.HiddenFacts);
        Assert.Equal("lucas_visita_1", lucas.EpilogueFlag);
    }

    [Fact]
    public void OnlyDocumentedFlagsExist()
    {
        // FR-015: não inventar outras marcas.
        Assert.Equal(["dog_adopted", "helena_outcome", "lucas_visita_1"],
            Day.Campaign.NarrativeFlags.Select(f => f.Id).Order());
    }

    [Fact]
    public void EveryDecisionIdUsedInContent_IsRegisteredInDecisionsDocument()
    {
        var registry = File.ReadAllText(RepoPaths.DecisionsDocument);
        var used = Day.DecisionMatrix.Cases.SelectMany(c => c.Outcomes).SelectMany(o => o.OpenDecisionIds)
            .Concat(Day.Protocol.Notices.SelectMany(n => n.OpenDecisionIds.Concat(n.DecisionIds)))
            .Distinct();

        Assert.All(used, id => Assert.Matches(new Regex($@"\|\s*{Regex.Escape(id)}\s*\|"), registry));
    }

    // ---- Revisão de 2026-10-14: fidelidade às fontes em docs/sources ----

    [Fact]
    public void DisplayedTexts_AreLiteralExcerptsOfTheSources()
    {
        var sources = string.Join("\n", Directory.GetFiles(RepoPaths.SourcesDirectory, "*.md")
            .Where(f => !f.EndsWith("README.md"))
            .Select(File.ReadAllText));

        var protocol = Day.Protocol;
        var texts = new[] { protocol.Title, protocol.RulesHeading, protocol.NoticesHeading }
            .Concat(protocol.RuleGroups.Select(g => g.Title))
            .Concat(protocol.Rules.Select(r => r.Description))
            .Concat(protocol.Notices.Select(n => n.Text))
            .Concat(Day.Patients.Patients.SelectMany(p =>
                new[] { p.ProfileText }.Concat(p.DeclaredSymptoms).Concat(p.VisibleFacts)
                    .Concat(p.HiddenFacts.Where(f => !f.Contains('=')))))
            .Concat(Day.Dialogues.Nodes.Select(n => n.Text))
            .Concat(Day.DecisionMatrix.AuditLogs.Select(a => a.Text))
            .Where(t => t is not null)
            .ToList();

        Assert.NotEmpty(texts);
        Assert.All(texts, t => Assert.True(sources.Contains(t!, System.StringComparison.Ordinal), $"Texto sem origem literal: '{t}'"));
    }

    [Fact]
    public void CommissionNotice_IsPreservedButBlockedUntilO002()
    {
        var notice = Day.Protocol.Notices.Single(n => n.Id == "D1_NOTICE_COMMISSION");

        Assert.Equal("Comissão: $5 por paciente correto.", notice.Text);
        Assert.Equal(NoticeStatus.Open, notice.Status);
        Assert.Contains("O-002", notice.OpenDecisionIds);
        Assert.Contains(Day.Report.Warnings, w => w.Code == IssueCodes.ProtocolNoticeBlocked && w.Message.Contains("D1_NOTICE_COMMISSION"));
        Assert.Equal("[ ATENÇÃO ]", Day.Protocol.NoticesHeading);
        Assert.Equal(3, Day.Protocol.Notices.Count);
    }

    [Fact]
    public void Destinations_FollowBriefingSection83_OnlyWhereTheActionIsResolved()
    {
        Assert.Equal("hospital", Outcome("d1_marcio_souza", TriageAction.Attend).DestinationId);
        Assert.Equal("waiting_chairs", Outcome("d1_marcio_souza", TriageAction.Queue).DestinationId);
        Assert.Equal("exit", Outcome("d1_marcio_souza", TriageAction.Refuse).DestinationId);
        Assert.Null(Outcome("d1_marcio_souza", TriageAction.Refer).DestinationId); // L-002

        foreach (var caseId in new[] { "d1_camila_torres", "d1_lucas_mendes", "d1_roberto_paiva" })
        {
            Assert.Equal("hospital", Outcome(caseId, TriageAction.Attend).DestinationId);
            Assert.Equal("exit", Outcome(caseId, TriageAction.Refuse).DestinationId);
        }
    }

    [Fact]
    public void ExitLines_OnlyOnPathsWithALiteralLine()
    {
        var withExit = Day.DecisionMatrix.Cases.SelectMany(c => c.Outcomes.Select(o => (c.Id, o)))
            .Where(x => x.o.ExitDialogueId is not null)
            .ToList();

        Assert.Equal(6, withExit.Count); // 5 caminhos corretos + Helena ajudada
        Assert.All(withExit, x => Assert.NotEqual(TriageClassification.Error, x.o.Classification)); // falas de erro são [PROPOSTA]
    }

    [Fact]
    public void CamilaError_UsesSingleLiteralLog_WithTwoReasons()
    {
        var attend = Outcome("d1_camila_torres", TriageAction.Attend);

        Assert.Equal(["AUD_D1_EXPIRED_RG_AND_NO_IMMEDIATE"], attend.AuditLogIds);
        Assert.Equal(2, attend.ReasonRuleIds.Count);
        Assert.Contains("INC-011", attend.OpenDecisionIds);
    }

    [Fact]
    public void DogAdoptedFlag_OnlyAcceptsTheDocumentedValue()
    {
        // O valor gravado ao não adotar não é definido pelas fontes (P-014).
        Assert.Equal(["true"], Day.Campaign.NarrativeFlags.Single(f => f.Id == "dog_adopted").AllowedValues);
    }
}
