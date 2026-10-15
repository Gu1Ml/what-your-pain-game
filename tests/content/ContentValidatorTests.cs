using System.Linq;
using System.Text.Json.Nodes;
using WhatYourPain.Content;
using WhatYourPain.Tests.Support;
using Xunit;

namespace WhatYourPain.Tests.Content;

/// <summary>
/// T012: IDs duplicados, referências quebradas e UNRESOLVED com efeito definitivo, além das
/// regras de consequência completa aprovadas (multa única, exceção ética sem multa).
/// </summary>
public sealed class ContentValidatorTests
{
    private static readonly string Matrix = ContentPaths.DecisionMatrix(1);
    private static readonly string Patients = ContentPaths.NarrativePatients(1);
    private static readonly string Protocol = ContentPaths.Protocol(1);

    // ---- IDs duplicados ----

    [Fact]
    public void DuplicatePatientId_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Patients, p => p["patients"]![1]!["id"] = "marcio_souza");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.DuplicateId, Patients);
    }

    [Fact]
    public void DuplicateRuleId_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Protocol, p => p["rules"]![1]!["id"] = "D1_DOCUMENT_VALID");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.DuplicateId, Protocol);
    }

    [Fact]
    public void DuplicateDecisionCaseId_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Matrix, m => m["cases"]![1]!["id"] = "d1_marcio_souza");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.DuplicateId, Matrix);
    }

    [Fact]
    public void DuplicateStoryPatientInOrder_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(ContentPaths.Campaign, c => c["days"]![0]!["storyPatientOrder"]![4] = "marcio_souza");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.DuplicateId, ContentPaths.Campaign);
        IssueAssert.HasError(ex.Report.Issues, IssueCodes.StoryOrderIncomplete, ContentPaths.Campaign);
    }

    [Fact]
    public void DuplicateActionInCase_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_lucas_mendes", "QUEUE", o => o["action"] = "REFER");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.DuplicateAction, Matrix);
        IssueAssert.HasError(ex.Report.Issues, IssueCodes.MissingAction, Matrix);
    }

    // ---- Referências quebradas ----

    [Fact]
    public void UnknownReasonRule_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_marcio_souza", "ATTEND", o => o["reasonRuleIds"] = new JsonArray("D1_DOES_NOT_EXIST"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        var issue = IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Matrix);
        Assert.Contains("D1_DOES_NOT_EXIST", issue.Message);
    }

    [Fact]
    public void PatientPointingToMissingCase_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Patients, p => p["patients"]![0]!["decisionMatrixId"] = "d1_nobody");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Patients);
    }

    [Fact]
    public void CaseForUnknownPatient_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Matrix, m => m["cases"]![0]!["patientId"] = "ghost_patient");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Matrix);
    }

    [Fact]
    public void StoryOrderWithUnknownPatient_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(ContentPaths.Campaign, c => c["days"]![0]!["storyPatientOrder"]![2] = "lucas_mendez");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, ContentPaths.Campaign);
    }

    [Theory]
    [InlineData("destinationId", "nowhere")]
    [InlineData("exitDialogueId", "no_such_line")]
    public void OutcomeWithUnknownDestinationOrDialogue_IsRejected(string field, string value)
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_marcio_souza", "ATTEND", o => o[field] = value);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Matrix);
    }

    [Fact]
    public void UnknownAuditLog_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_camila_torres", "ATTEND", o => o["auditLogIds"] = new JsonArray("AUD_MISSING"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Matrix);
    }

    [Fact]
    public void FlagOutsideCatalog_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_helena_rios", "REFUSE", o => o["flagsToSet"] = new JsonObject { ["helena"] = "recusada" });

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Matrix);
    }

    [Fact]
    public void FlagValueOutsideAllowedValues_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_helena_rios", "REFUSE", o => o["flagsToSet"] = new JsonObject { ["helena_outcome"] = "maybe" });

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.FlagValueNotAllowed, Matrix);
    }

    [Fact]
    public void ProtocolMismatchBetweenFiles_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Matrix, m => m["protocolId"] = "day_2_protocol");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnknownReference, Matrix);
    }

    // ---- UNRESOLVED com efeito definitivo ----

    [Theory]
    [InlineData("commissionDelta", "0")]
    [InlineData("commissionDelta", "5")]
    [InlineData("fineDelta", "5")]
    [InlineData("specialCostDelta", "10")]
    [InlineData("reputationDelta", "0")]
    [InlineData("conscienceDelta", "1")]
    [InlineData("destinationId", "\"waiting_chairs\"")]
    [InlineData("auditLogIds", "[\"AUD_D1_EXPIRED_RG_AND_NO_IMMEDIATE\"]")]
    [InlineData("flagsToSet", "{\"lucas_visita_1\":\"true\"}")]
    public void UnresolvedOutcomeWithDefinitiveEffect_IsRejected(string field, string valueJson)
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_camila_torres", "QUEUE", o => o[field] = JsonNode.Parse(valueJson));

        var ex = IssueAssert.ThrowsValidation(workspace);

        var issue = IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnresolvedWithEffect, Matrix);
        Assert.Contains(field, issue.Message);
    }

    [Fact]
    public void UnresolvedOutcomeWithoutPendingDecision_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_roberto_paiva", "REFER", o => o.Remove("openDecisionIds"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnresolvedWithoutPending, Matrix);
    }

    [Fact]
    public void MissingActionInCase_IsRejected_InsteadOfDefaulting()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Matrix, m =>
        {
            var outcomes = m["cases"]![2]!["outcomes"]!.AsArray();
            outcomes.Remove(outcomes.Single(o => (string?)o!["action"] == "REFER"));
        });

        var ex = IssueAssert.ThrowsValidation(workspace);

        var issue = IssueAssert.HasError(ex.Report.Issues, IssueCodes.MissingAction, Matrix);
        Assert.Contains("Refer", issue.Message);
    }

    // ---- Consequências incompletas ou contrárias às decisões fechadas ----

    [Theory]
    [InlineData("fineDelta")]
    [InlineData("commissionDelta")]
    [InlineData("reputationDelta")]
    [InlineData("reasonRuleIds")]
    public void ResolvedOutcomeWithMissingEffect_IsIncomplete(string field)
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_camila_torres", "REFUSE", o => o.Remove(field));

        var ex = IssueAssert.ThrowsValidation(workspace);

        var issue = IssueAssert.HasError(ex.Report.Issues, IssueCodes.IncompleteConsequence, Matrix);
        Assert.Contains(field, issue.Message);
    }

    [Fact]
    public void ErrorWithDifferentFine_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_camila_torres", "ATTEND", o => o["fineDelta"] = 10);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.InvalidErrorConsequence, Matrix);
    }

    [Fact]
    public void ErrorWithCommission_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_lucas_mendes", "ATTEND", o => o["commissionDelta"] = 5);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.InvalidErrorConsequence, Matrix);
    }

    [Fact]
    public void EthicalExceptionWithGenericFine_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_helena_rios", "ATTEND", o => o["fineDelta"] = 5);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.InvalidFine, Matrix);
    }

    [Fact]
    public void NegativeMoneyMagnitude_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_helena_rios", "ATTEND", o => o["specialCostDelta"] = -10);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.InvalidValue, Matrix);
    }

    [Fact]
    public void RulePriority_IsRejectedWhilePrecedenceIsOpen()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Protocol, p => p["rules"]![0]!["priority"] = 1);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.PrecedenceUndecided, Protocol);
    }

    [Fact]
    public void InvalidPendingDecisionId_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_camila_torres", "QUEUE", o => o["openDecisionIds"] = new JsonArray("talvez depois"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.InvalidValue, Matrix);
    }

    [Fact]
    public void ValidationErrors_ListEveryProblem_NotOnlyTheFirst()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Patients, p => p["patients"]![1]!["id"] = "marcio_souza");
        workspace.EditOutcome("d1_camila_torres", "QUEUE", o => o["commissionDelta"] = 0);

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.DuplicateId, Patients);
        IssueAssert.HasError(ex.Report.Issues, IssueCodes.UnresolvedWithEffect, Matrix);
    }

    // ---- Avisos do folheto ----

    private static JsonObject Notice(JsonNode protocol, string id) =>
        protocol["notices"]!.AsArray().Single(n => (string?)n!["id"] == id)!.AsObject();

    [Fact]
    public void OpenNoticeWithoutPendingDecision_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Protocol, p => Notice(p, "D1_NOTICE_COMMISSION").Remove("openDecisionIds"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.NoticeWithoutPending, Protocol);
    }

    [Fact]
    public void CommissionNotice_CannotBeMarkedApprovedWhileTriggerIsOpen()
    {
        // Promover o aviso de comissão a APPROVED mantendo O-002 é contraditório.
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Protocol, p => Notice(p, "D1_NOTICE_COMMISSION")["status"] = "APPROVED");

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.NoticeStatusMismatch, Protocol);
    }

    [Fact]
    public void ApprovedNoticeWithoutClosedDecision_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Protocol, p => Notice(p, "D1_NOTICE_DAILY_EXPENSE").Remove("decisionIds"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.NoticeStatusMismatch, Protocol);
    }

    [Fact]
    public void NoticeCitingNonClosedIdAsDecision_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(Protocol, p => Notice(p, "D1_NOTICE_DAILY_EXPENSE")["decisionIds"] = new JsonArray("O-002"));

        var ex = IssueAssert.ThrowsValidation(workspace);

        IssueAssert.HasError(ex.Report.Issues, IssueCodes.InvalidValue, Protocol);
    }
}
