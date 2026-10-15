using System.Text.Json.Nodes;
using WhatYourPain.Content;
using WhatYourPain.Tests.Support;
using Xunit;

namespace WhatYourPain.Tests.Content;

/// <summary>T012: arquivo ausente, JSON inválido e incompatibilidades de contrato na carga.</summary>
public sealed class DataLoaderTests
{
    [Fact]
    public void LoadDay_RealContent_LoadsWithoutErrors()
    {
        using var workspace = new ContentWorkspace();

        var day = workspace.Loader.LoadDay(1);

        Assert.True(day.Report.IsValid);
        Assert.Equal(5, day.Patients.Patients.Count);
        Assert.Equal(5, day.DecisionMatrix.Cases.Count);
    }

    [Fact]
    public void LoadDay_MissingFile_ReportsFileAndCode()
    {
        using var workspace = new ContentWorkspace();
        workspace.Delete(ContentPaths.DecisionMatrix(1));

        var ex = IssueAssert.ThrowsLoad(workspace);

        IssueAssert.HasError(ex.Issues, IssueCodes.FileMissing, ContentPaths.DecisionMatrix(1));
    }

    [Fact]
    public void LoadDay_SeveralMissingFiles_ReportsAllOfThem()
    {
        using var workspace = new ContentWorkspace();
        workspace.Delete(ContentPaths.Protocol(1));
        workspace.Delete(ContentPaths.Dialogues(1));

        var ex = IssueAssert.ThrowsLoad(workspace);

        IssueAssert.HasError(ex.Issues, IssueCodes.FileMissing, ContentPaths.Protocol(1));
        IssueAssert.HasError(ex.Issues, IssueCodes.FileMissing, ContentPaths.Dialogues(1));
    }

    [Fact]
    public void LoadDay_InvalidJsonSyntax_ReportsLine()
    {
        using var workspace = new ContentWorkspace();
        workspace.WriteText(ContentPaths.Protocol(1), "{\n  \"id\": \"day_1_protocol\",\n  \"dayNumber\": 1,\n  \"rules\": [ \n");

        var ex = IssueAssert.ThrowsLoad(workspace);

        var issue = IssueAssert.HasError(ex.Issues, IssueCodes.JsonInvalid, ContentPaths.Protocol(1));
        Assert.Contains("linha", issue.Location);
    }

    [Fact]
    public void LoadDay_MissingRequiredField_ReportsFieldMissing()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(ContentPaths.DecisionMatrix(1), m => m.AsObject().Remove("protocolId"));

        var ex = IssueAssert.ThrowsLoad(workspace);

        var issue = IssueAssert.HasError(ex.Issues, IssueCodes.FieldMissing, ContentPaths.DecisionMatrix(1));
        Assert.Contains("protocolId", issue.Message);
    }

    [Fact]
    public void LoadDay_ExplicitNullInRequiredField_ReportsPath()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(ContentPaths.NarrativePatients(1), p => p["patients"]![2]!["name"] = null);

        var ex = IssueAssert.ThrowsLoad(workspace);

        var issue = IssueAssert.HasError(ex.Issues, IssueCodes.FieldMissing, ContentPaths.NarrativePatients(1));
        Assert.Equal("$.patients[2].name", issue.Location);
    }

    [Fact]
    public void LoadDay_NullListOfRequiredCollection_ReportsFieldMissing()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditJson(ContentPaths.DecisionMatrix(1), m => m["cases"]![0]!["outcomes"] = null);

        var ex = IssueAssert.ThrowsLoad(workspace);

        var issue = IssueAssert.HasError(ex.Issues, IssueCodes.FieldMissing, ContentPaths.DecisionMatrix(1));
        Assert.Equal("$.cases[0].outcomes", issue.Location);
    }

    [Fact]
    public void LoadDay_UnknownField_IsRejectedInsteadOfIgnored()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_marcio_souza", "ATTEND", o => o["comissionDelta"] = 5);

        var ex = IssueAssert.ThrowsLoad(workspace);

        IssueAssert.HasError(ex.Issues, IssueCodes.SchemaMismatch, ContentPaths.DecisionMatrix(1));
    }

    [Fact]
    public void LoadDay_MoneyAsText_IsRejected()
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_marcio_souza", "ATTEND", o => o["commissionDelta"] = "5,00");

        var ex = IssueAssert.ThrowsLoad(workspace);

        IssueAssert.HasError(ex.Issues, IssueCodes.SchemaMismatch, ContentPaths.DecisionMatrix(1));
    }

    [Theory]
    [InlineData("\"MAYBE\"")]
    [InlineData("4")]
    public void LoadDay_UnknownOrNumericClassification_IsRejected(string classificationJson)
    {
        using var workspace = new ContentWorkspace();
        workspace.EditOutcome("d1_marcio_souza", "ATTEND", o => o["classification"] = JsonNode.Parse(classificationJson));

        var ex = IssueAssert.ThrowsLoad(workspace);

        IssueAssert.HasError(ex.Issues, IssueCodes.SchemaMismatch, ContentPaths.DecisionMatrix(1));
    }

    [Fact]
    public void LoadDay_LoadErrors_AreContentErrors_NotTriageResults()
    {
        using var workspace = new ContentWorkspace();
        workspace.Delete(ContentPaths.Campaign);

        var ex = IssueAssert.ThrowsLoad(workspace);

        Assert.All(ex.Issues, i => Assert.Equal(IssueSeverity.Error, i.Severity));
        Assert.Contains(ContentPaths.Campaign, ex.Message);
        Assert.Single(ex.Issues, i => i.File == ContentPaths.Campaign);
    }
}
