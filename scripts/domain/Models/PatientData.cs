using System.Collections.Generic;

namespace WhatYourPain.Domain.Models;

/// <summary>Documento apresentado pelo paciente (data-model, DocumentData).</summary>
public sealed record DocumentData
{
    public required DocumentType DocumentType { get; init; }

    /// <summary>null = não informado pela fonte; não presumir.</summary>
    public bool? NameVisible { get; init; }

    /// <summary>null = não informado pela fonte; não presumir.</summary>
    public bool? IdentifierVisible { get; init; }

    public required ValidityStatus ValidityStatus { get; init; }
    public int? IssueYear { get; init; }

    /// <summary>Data ISO (yyyy-MM-dd), quando aplicável.</summary>
    public string? ExpiryDate { get; init; }
}

/// <summary>Paciente de história ou de rotina (data-model, PatientData).</summary>
public sealed record PatientData
{
    public required string Id { get; init; }
    public required PatientType PatientType { get; init; }
    public required string Name { get; init; }
    public required int Age { get; init; }
    public string? ProfileText { get; init; }
    public IReadOnlyList<string> DeclaredSymptoms { get; init; } = [];
    public required DocumentData Document { get; init; }
    public IReadOnlyList<string> VisibleFacts { get; init; } = [];
    public IReadOnlyList<string> HiddenFacts { get; init; } = [];
    public string? DialogueId { get; init; }
    public string? DecisionMatrixId { get; init; }
    public string? EpilogueFlag { get; init; }
}

/// <summary>Conteúdo de <c>content/patients/day_N_narrative_patients.json</c>.</summary>
public sealed record PatientSetData
{
    public required string Id { get; init; }
    public required int DayNumber { get; init; }
    public required IReadOnlyList<PatientData> Patients { get; init; }
    public string? Notes { get; init; }
}
