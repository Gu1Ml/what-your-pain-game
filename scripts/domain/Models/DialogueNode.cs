using System.Collections.Generic;

namespace WhatYourPain.Domain.Models;

/// <summary>Fala dirigida por ID (data-model, DialogueNode).</summary>
public sealed record DialogueNode
{
    public required string Id { get; init; }
    public required string SpeakerId { get; init; }
    public required string Text { get; init; }
    public required DialogueTrigger Trigger { get; init; }
    public IReadOnlyList<RuleCondition> Conditions { get; init; } = [];
    public string? NextDialogueId { get; init; }
}

/// <summary>Conteúdo de <c>content/dialogues/day_N_dialogue.json</c>.</summary>
public sealed record DialogueSetData
{
    public required string Id { get; init; }
    public required int DayNumber { get; init; }
    public required IReadOnlyList<DialogueNode> Nodes { get; init; }
    public string? Notes { get; init; }
}
