namespace WhatYourPain.Domain.Models;

// Os nomes em JSON seguem o data-model.md em SNAKE_CASE maiúsculo (ex.: ETHICAL_EXCEPTION),
// convertidos por ContentJson. Valores inteiros não são aceitos no JSON.

/// <summary>
/// Os quatro controles pedidos para a alpha (data-model §2.1). A lista não declara
/// que a semântica de QUEUE/REFER está aprovada (O-001, O-015).
/// </summary>
public enum TriageAction
{
    Attend,
    Queue,
    Refer,
    Refuse,
}

/// <summary>Classificação de um resultado (data-model §2.2).</summary>
public enum TriageClassification
{
    Correct,
    Permitted,
    Error,
    EthicalException,
    Unresolved,
}

public enum PatientType
{
    Story,
    Routine,
}

/// <summary>RG, CPF ou CNH. A aceitação da CNH pela regra documental continua em aberto (O-004).</summary>
public enum DocumentType
{
    Rg,
    Cpf,
    Cnh,
}

/// <summary>UNKNOWN é usado quando a validade não está representada (O-005); nunca presumir VALID.</summary>
public enum ValidityStatus
{
    Valid,
    Expired,
    Missing,
    Unknown,
}

/// <summary>Operadores permitidos em condições declarativas (data-model, RuleCondition).</summary>
public enum RuleOperator
{
    Equals,
    NotEquals,
    GreaterThan,
    LessThan,
    IsTrue,
    IsFalse,
    Contains,
}

/// <summary>Momento de uma fala: entrada, inspeção, ação ou saída (data-model, DialogueNode).</summary>
public enum DialogueTrigger
{
    Entry,
    Inspection,
    Action,
    Exit,
}

/// <summary>Tipos sugeridos de lançamento (data-model, FinancialEntry).</summary>
public enum FinancialEntryType
{
    Commission,
    Fine,
    EthicalCost,
    DailyExpense,
    OvertimeBonus,
    DogExpense,
    DamageCost,
}
