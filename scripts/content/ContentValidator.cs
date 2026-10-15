using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WhatYourPain.Domain.Models;

namespace WhatYourPain.Content;

/// <summary>
/// Valida a integridade de um dia de conteúdo: IDs únicos, referências existentes,
/// consequências completas e pendências explícitas (data-model §5). Não decide regras de
/// triagem; apenas impede que conteúdo incompleto vire comportamento silencioso.
/// </summary>
public static class ContentValidator
{
    private static readonly Regex OpenDecisionIdPattern = new(@"^(O|L|INC)-\d{3}$", RegexOptions.CultureInvariant);
    private static readonly Regex ClosedDecisionIdPattern = new(@"^D-\d{3}$", RegexOptions.CultureInvariant);

    private static readonly TriageAction[] AllActions = Enum.GetValues<TriageAction>();

    public static ContentValidationReport Validate(DayContent content)
    {
        var v = new Run(content);
        v.Execute();
        return new ContentValidationReport(v.Issues);
    }

    private sealed class Run(DayContent content)
    {
        private readonly string _campaignFile = ContentPaths.Campaign;
        private readonly string _protocolFile = ContentPaths.Protocol(content.DayNumber);
        private readonly string _patientsFile = ContentPaths.NarrativePatients(content.DayNumber);
        private readonly string _matrixFile = ContentPaths.DecisionMatrix(content.DayNumber);
        private readonly string _dialogueFile = ContentPaths.Dialogues(content.DayNumber);

        private HashSet<string> _ruleIds = [];
        private HashSet<string> _dialogueIds = [];
        private Dictionary<string, NarrativeFlag> _flags = [];

        public List<ContentIssue> Issues { get; } = [];

        public void Execute()
        {
            _flags = ValidateCampaign();
            _ruleIds = ValidateProtocol();
            _dialogueIds = ValidateDialogues();
            var patients = ValidatePatients();
            ValidateDayConfig(patients);
            ValidateMatrix(patients);
        }

        private Dictionary<string, NarrativeFlag> ValidateCampaign()
        {
            var campaign = content.Campaign;
            RequireId(campaign.Id, _campaignFile, "id");

            CheckUnique(campaign.Days.Select((d, i) => (d.DayNumber.ToString(CultureInfo.InvariantCulture), $"days[{i}]")),
                _campaignFile, "dayNumber");

            if (campaign.StartingMoney is null)
                Warn(IssueCodes.StartingMoneyUndefined, _campaignFile, "startingMoney",
                    "saldo inicial não definido nos documentos (L-001); GameState exige valor explícito");

            var flags = new Dictionary<string, NarrativeFlag>();
            for (var i = 0; i < campaign.NarrativeFlags.Count; i++)
            {
                var flag = campaign.NarrativeFlags[i];
                var at = $"narrativeFlags[{i}]";
                if (!RequireId(flag.Id, _campaignFile, at + ".id"))
                    continue;
                if (!flags.TryAdd(flag.Id, flag))
                    Error(IssueCodes.DuplicateId, _campaignFile, at, $"marca '{flag.Id}' duplicada");
                if (flag.AllowedValues is null || flag.AllowedValues.Count == 0)
                    Error(IssueCodes.FieldMissing, _campaignFile, at + ".allowedValues", $"marca '{flag.Id}' sem valores permitidos");
            }

            return flags;
        }

        private HashSet<string> ValidateProtocol()
        {
            var protocol = content.Protocol;
            RequireId(protocol.Id, _protocolFile, "id");
            if (protocol.DayNumber != content.DayNumber)
                Error(IssueCodes.ReferenceMismatch, _protocolFile, "dayNumber",
                    $"protocolo do dia {protocol.DayNumber} carregado para o dia {content.DayNumber}");

            CheckUnique(protocol.RuleGroups.Select((g, i) => (g.Number.ToString(CultureInfo.InvariantCulture), $"ruleGroups[{i}]")),
                _protocolFile, "grupo de regra");
            var groups = protocol.RuleGroups.Select(g => g.Number).ToHashSet();

            var ids = new HashSet<string>();
            for (var i = 0; i < protocol.Rules.Count; i++)
            {
                var rule = protocol.Rules[i];
                var at = $"rules[{i}]";
                if (RequireId(rule.Id, _protocolFile, at + ".id") && !ids.Add(rule.Id))
                    Error(IssueCodes.DuplicateId, _protocolFile, at, $"regra '{rule.Id}' duplicada");
                RequireText(rule.Category, _protocolFile, at + ".category");
                RequireText(rule.Description, _protocolFile, at + ".description");
                if (!groups.Contains(rule.RuleGroup))
                    Error(IssueCodes.UnknownReference, _protocolFile, at + ".ruleGroup",
                        $"regra '{rule.Id}' aponta para o grupo inexistente {rule.RuleGroup}");
                if (rule.Priority is not null)
                    Error(IssueCodes.PrecedenceUndecided, _protocolFile, at + ".priority",
                        $"regra '{rule.Id}' define prioridade, mas a precedência entre regras está em aberto (O-003)");
                ValidateConditions(rule.Conditions, _protocolFile, at);
            }

            ValidateNotices(protocol.Notices);
            return ids;
        }

        private void ValidateNotices(IReadOnlyList<ProtocolNotice> notices)
        {
            var ids = new HashSet<string>();
            for (var i = 0; i < notices.Count; i++)
            {
                var notice = notices[i];
                var at = $"notices[{i}]";
                if (RequireId(notice.Id, _protocolFile, at + ".id") && !ids.Add(notice.Id))
                    Error(IssueCodes.DuplicateId, _protocolFile, at, $"aviso '{notice.Id}' duplicado");
                RequireText(notice.Text, _protocolFile, at + ".text");

                foreach (var decision in notice.DecisionIds)
                    if (!ClosedDecisionIdPattern.IsMatch(decision))
                        Error(IssueCodes.InvalidValue, _protocolFile, at + ".decisionIds", $"'{decision}' não é um ID de decisão fechada (D-xxx)");
                foreach (var pending in notice.OpenDecisionIds)
                    if (!OpenDecisionIdPattern.IsMatch(pending))
                        Error(IssueCodes.InvalidValue, _protocolFile, at + ".openDecisionIds", $"'{pending}' não é um ID de pendência");

                if (notice.Status == NoticeStatus.Open)
                {
                    if (notice.OpenDecisionIds.Count == 0)
                        Error(IssueCodes.NoticeWithoutPending, _protocolFile, at + ".openDecisionIds",
                            $"aviso '{notice.Id}' está OPEN sem indicar a pendência que o bloqueia");
                    else
                        Warn(IssueCodes.ProtocolNoticeBlocked, _protocolFile, at,
                            $"aviso '{notice.Id}' não pode ser apresentado como regra aprovada ({string.Join(", ", notice.OpenDecisionIds)})");
                }
                else if (notice.OpenDecisionIds.Count > 0 || notice.DecisionIds.Count == 0)
                    Error(IssueCodes.NoticeStatusMismatch, _protocolFile, at,
                        $"aviso '{notice.Id}' APPROVED deve citar decisões fechadas e nenhuma pendência");
            }
        }

        private HashSet<string> ValidateDialogues()
        {
            var set = content.Dialogues;
            RequireId(set.Id, _dialogueFile, "id");
            if (set.DayNumber != content.DayNumber)
                Error(IssueCodes.ReferenceMismatch, _dialogueFile, "dayNumber",
                    $"diálogos do dia {set.DayNumber} carregados para o dia {content.DayNumber}");

            var ids = new HashSet<string>();
            for (var i = 0; i < set.Nodes.Count; i++)
            {
                var node = set.Nodes[i];
                var at = $"nodes[{i}]";
                if (RequireId(node.Id, _dialogueFile, at + ".id") && !ids.Add(node.Id))
                    Error(IssueCodes.DuplicateId, _dialogueFile, at, $"fala '{node.Id}' duplicada");
                RequireText(node.SpeakerId, _dialogueFile, at + ".speakerId");
                RequireText(node.Text, _dialogueFile, at + ".text");
                ValidateConditions(node.Conditions, _dialogueFile, at);
            }

            for (var i = 0; i < set.Nodes.Count; i++)
            {
                var next = set.Nodes[i].NextDialogueId;
                if (next is not null && !ids.Contains(next))
                    Error(IssueCodes.UnknownReference, _dialogueFile, $"nodes[{i}].nextDialogueId", $"fala '{next}' não existe");
            }

            return ids;
        }

        private Dictionary<string, PatientData> ValidatePatients()
        {
            var set = content.Patients;
            RequireId(set.Id, _patientsFile, "id");
            if (set.DayNumber != content.DayNumber)
                Error(IssueCodes.ReferenceMismatch, _patientsFile, "dayNumber",
                    $"pacientes do dia {set.DayNumber} carregados para o dia {content.DayNumber}");

            var patients = new Dictionary<string, PatientData>();
            for (var i = 0; i < set.Patients.Count; i++)
            {
                var p = set.Patients[i];
                var at = $"patients[{i}]";
                if (!RequireId(p.Id, _patientsFile, at + ".id"))
                    continue;
                if (!patients.TryAdd(p.Id, p))
                    Error(IssueCodes.DuplicateId, _patientsFile, at, $"paciente '{p.Id}' duplicado");

                RequireText(p.Name, _patientsFile, at + ".name");
                if (p.Age < 0)
                    Error(IssueCodes.InvalidValue, _patientsFile, at + ".age", "idade negativa");

                if (p.Document is null)
                    Error(IssueCodes.FieldMissing, _patientsFile, at + ".document", $"paciente '{p.Id}' sem documento");
                else if (p.Document.ExpiryDate is { } expiry
                         && !DateOnly.TryParseExact(expiry, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    Error(IssueCodes.InvalidValue, _patientsFile, at + ".document.expiryDate", $"data '{expiry}' não está no formato ISO yyyy-MM-dd");

                if (p.PatientType == PatientType.Story && string.IsNullOrWhiteSpace(p.DecisionMatrixId))
                    Error(IssueCodes.FieldMissing, _patientsFile, at + ".decisionMatrixId", $"paciente de história '{p.Id}' sem matriz de decisão");

                if (p.DialogueId is null)
                {
                    if (p.PatientType == PatientType.Story)
                        Warn(IssueCodes.DialoguePending, _patientsFile, at + ".dialogueId",
                            $"paciente '{p.Id}' sem diálogo: fonte dia-1.md ausente do repositório (L-003)");
                }
                else if (!_dialogueIds.Contains(p.DialogueId))
                    Error(IssueCodes.UnknownReference, _patientsFile, at + ".dialogueId", $"diálogo '{p.DialogueId}' não existe");

                if (p.EpilogueFlag is not null && !_flags.ContainsKey(p.EpilogueFlag))
                    Error(IssueCodes.UnknownReference, _patientsFile, at + ".epilogueFlag", $"marca '{p.EpilogueFlag}' não está no catálogo de campaign.json");
            }

            return patients;
        }

        private void ValidateDayConfig(Dictionary<string, PatientData> patients)
        {
            var day = content.DayConfig;
            if (day is null)
            {
                Error(IssueCodes.MissingDayConfig, _campaignFile, "days", $"dia {content.DayNumber} não configurado");
                return;
            }

            var at = $"days[dayNumber={day.DayNumber}]";
            if (day.ProtocolId != content.Protocol.Id)
                Error(IssueCodes.UnknownReference, _campaignFile, at + ".protocolId",
                    $"protocolo '{day.ProtocolId}' não corresponde ao arquivo carregado ('{content.Protocol.Id}')");
            if (day.BaseDurationSeconds <= 0)
                Error(IssueCodes.InvalidValue, _campaignFile, at + ".baseDurationSeconds", "duração deve ser positiva");
            foreach (var (name, value) in new[]
                     {
                         ("dailyLivingExpense", day.DailyLivingExpense),
                         ("nominalCommission", day.NominalCommission),
                         ("genericErrorFine", day.GenericErrorFine),
                     })
                if (value < 0)
                    Error(IssueCodes.InvalidValue, _campaignFile, $"{at}.{name}", "valor deve ser uma magnitude não negativa");

            CheckUnique(day.StoryPatientOrder.Select((id, i) => (id, $"{at}.storyPatientOrder[{i}]")), _campaignFile, "paciente na ordem");
            foreach (var id in day.StoryPatientOrder)
                if (!patients.TryGetValue(id, out var p) || p.PatientType != PatientType.Story)
                    Error(IssueCodes.UnknownReference, _campaignFile, at + ".storyPatientOrder", $"paciente de história '{id}' não existe");

            foreach (var p in patients.Values.Where(p => p.PatientType == PatientType.Story))
                if (!day.StoryPatientOrder.Contains(p.Id))
                    Error(IssueCodes.StoryOrderIncomplete, _campaignFile, at + ".storyPatientOrder", $"paciente de história '{p.Id}' fora da ordem do dia");
        }

        private void ValidateMatrix(Dictionary<string, PatientData> patients)
        {
            var matrix = content.DecisionMatrix;
            RequireId(matrix.Id, _matrixFile, "id");
            if (matrix.ProtocolId != content.Protocol.Id)
                Error(IssueCodes.UnknownReference, _matrixFile, "protocolId",
                    $"protocolo '{matrix.ProtocolId}' não corresponde ao arquivo carregado ('{content.Protocol.Id}')");

            var destinations = CollectIds(matrix.Destinations.Select(d => d.Id), "destinations", "destino");
            var audits = CollectIds(matrix.AuditLogs.Select(a => a.Id), "auditLogs", "registro de auditoria");
            for (var i = 0; i < matrix.AuditLogs.Count; i++)
                RequireText(matrix.AuditLogs[i].Text, _matrixFile, $"auditLogs[{i}].text");

            var cases = new Dictionary<string, DecisionCase>();
            for (var i = 0; i < matrix.Cases.Count; i++)
            {
                var c = matrix.Cases[i];
                var at = $"cases[{i}]";
                if (!RequireId(c.Id, _matrixFile, at + ".id"))
                    continue;
                if (!cases.TryAdd(c.Id, c))
                    Error(IssueCodes.DuplicateId, _matrixFile, at, $"caso '{c.Id}' duplicado");
                if (!patients.ContainsKey(c.PatientId))
                    Error(IssueCodes.UnknownReference, _matrixFile, at + ".patientId", $"paciente '{c.PatientId}' não existe");

                ValidateOutcomes(c, at, destinations, audits);
            }

            foreach (var p in patients.Values.Where(p => p.DecisionMatrixId is not null))
            {
                if (!cases.TryGetValue(p.DecisionMatrixId!, out var c))
                    Error(IssueCodes.UnknownReference, _patientsFile, $"patients[id={p.Id}].decisionMatrixId", $"caso '{p.DecisionMatrixId}' não existe na matriz");
                else if (c.PatientId != p.Id)
                    Error(IssueCodes.ReferenceMismatch, _patientsFile, $"patients[id={p.Id}].decisionMatrixId",
                        $"caso '{c.Id}' pertence a '{c.PatientId}', não a '{p.Id}'");
            }
        }

        private void ValidateOutcomes(DecisionCase c, string caseAt, HashSet<string> destinations, HashSet<string> audits)
        {
            var seen = new HashSet<TriageAction>();
            for (var i = 0; i < c.Outcomes.Count; i++)
            {
                var o = c.Outcomes[i];
                var at = $"{caseAt}.outcomes[{i}]";
                if (!seen.Add(o.Action))
                    Error(IssueCodes.DuplicateAction, _matrixFile, at, $"ação {o.Action} repetida no caso '{c.Id}'");

                ValidateOutcomeReferences(o, at, destinations, audits);
                ValidateOutcomeEffects(c, o, at);
            }

            foreach (var missing in AllActions.Where(a => !seen.Contains(a)))
                Error(IssueCodes.MissingAction, _matrixFile, caseAt,
                    $"caso '{c.Id}' não define a ação {missing}; ações sem decisão devem constar como UNRESOLVED");
        }

        private void ValidateOutcomeReferences(ActionOutcome o, string at, HashSet<string> destinations, HashSet<string> audits)
        {
            foreach (var ruleId in o.ReasonRuleIds)
                if (!_ruleIds.Contains(ruleId))
                    Error(IssueCodes.UnknownReference, _matrixFile, at + ".reasonRuleIds", $"regra '{ruleId}' não existe no protocolo");
            if (o.DestinationId is not null && !destinations.Contains(o.DestinationId))
                Error(IssueCodes.UnknownReference, _matrixFile, at + ".destinationId", $"destino '{o.DestinationId}' não existe");
            if (o.ExitDialogueId is not null && !_dialogueIds.Contains(o.ExitDialogueId))
                Error(IssueCodes.UnknownReference, _matrixFile, at + ".exitDialogueId", $"diálogo '{o.ExitDialogueId}' não existe");
            foreach (var auditId in o.AuditLogIds)
                if (!audits.Contains(auditId))
                    Error(IssueCodes.UnknownReference, _matrixFile, at + ".auditLogIds", $"registro de auditoria '{auditId}' não existe");
            foreach (var (flagId, value) in o.FlagsToSet)
            {
                if (!_flags.TryGetValue(flagId, out var flag))
                    Error(IssueCodes.UnknownReference, _matrixFile, at + ".flagsToSet", $"marca '{flagId}' não está no catálogo de campaign.json");
                else if (!flag.AllowedValues.Contains(value))
                    Error(IssueCodes.FlagValueNotAllowed, _matrixFile, at + ".flagsToSet",
                        $"valor '{value}' não permitido para '{flagId}' ({string.Join(", ", flag.AllowedValues)})");
            }
            foreach (var pending in o.OpenDecisionIds)
                if (!OpenDecisionIdPattern.IsMatch(pending))
                    Error(IssueCodes.InvalidValue, _matrixFile, at + ".openDecisionIds",
                        $"'{pending}' não é um ID de pendência (O-xxx, L-xxx ou INC-xxx de docs/decisions.md)");
        }

        private void ValidateOutcomeEffects(DecisionCase c, ActionOutcome o, string at)
        {
            var label = $"'{c.Id}' {o.Action}";
            if (o.Classification == TriageClassification.Unresolved)
            {
                var effects = DescribeEffects(o);
                if (effects.Count > 0)
                    Error(IssueCodes.UnresolvedWithEffect, _matrixFile, at,
                        $"{label} é UNRESOLVED mas define efeitos ({string.Join(", ", effects)}); deixe-os ausentes até a decisão");
                if (o.OpenDecisionIds.Count == 0)
                    Error(IssueCodes.UnresolvedWithoutPending, _matrixFile, at + ".openDecisionIds",
                        $"{label} é UNRESOLVED sem indicar a pendência que o bloqueia");
                else
                    Warn(IssueCodes.UnresolvedOutcome, _matrixFile, at,
                        $"{label} permanece UNRESOLVED ({string.Join(", ", o.OpenDecisionIds)})");
                return;
            }

            var missing = new List<string>();
            if (o.CommissionDelta is null) missing.Add("commissionDelta");
            if (o.FineDelta is null) missing.Add("fineDelta");
            if (o.SpecialCostDelta is null) missing.Add("specialCostDelta");
            if (o.ReputationDelta is null) missing.Add("reputationDelta");
            if (o.ConscienceDelta is null) missing.Add("conscienceDelta");
            if (o.ReasonRuleIds.Count == 0) missing.Add("reasonRuleIds");
            if (missing.Count > 0)
                Error(IssueCodes.IncompleteConsequence, _matrixFile, at,
                    $"{label} é {o.Classification} mas não define: {string.Join(", ", missing)}");

            foreach (var (name, value) in new[] { ("commissionDelta", o.CommissionDelta), ("fineDelta", o.FineDelta), ("specialCostDelta", o.SpecialCostDelta) })
                if (value < 0)
                    Error(IssueCodes.InvalidValue, _matrixFile, $"{at}.{name}", $"{label}: valores monetários são magnitudes não negativas");

            var genericFine = content.DayConfig?.GenericErrorFine;
            switch (o.Classification)
            {
                case TriageClassification.Error:
                    if (o.CommissionDelta is not null and not 0)
                        Error(IssueCodes.InvalidErrorConsequence, _matrixFile, at + ".commissionDelta", $"{label}: erro de triagem não recebe comissão (D-010)");
                    if (o.FineDelta is not null && genericFine is not null && o.FineDelta != genericFine)
                        Error(IssueCodes.InvalidErrorConsequence, _matrixFile, at + ".fineDelta",
                            $"{label}: erro genérico tem multa única de {genericFine} por paciente (D-015)");
                    break;
                case TriageClassification.EthicalException:
                case TriageClassification.Correct:
                case TriageClassification.Permitted:
                    if (o.FineDelta is not null and not 0)
                        Error(IssueCodes.InvalidFine, _matrixFile, at + ".fineDelta",
                            $"{label}: só erro genérico gera multa (exceção ética sem multa genérica, D-016)");
                    break;
            }

            if (o.OpenDecisionIds.Count > 0)
                Warn(IssueCodes.OpenDecisionDependency, _matrixFile, at,
                    $"{label} ({o.Classification}) ainda depende de {string.Join(", ", o.OpenDecisionIds)}");
        }

        private static List<string> DescribeEffects(ActionOutcome o)
        {
            var effects = new List<string>();
            if (o.CommissionDelta is not null) effects.Add("commissionDelta");
            if (o.FineDelta is not null) effects.Add("fineDelta");
            if (o.SpecialCostDelta is not null) effects.Add("specialCostDelta");
            if (o.ReputationDelta is not null) effects.Add("reputationDelta");
            if (o.ConscienceDelta is not null) effects.Add("conscienceDelta");
            if (o.DestinationId is not null) effects.Add("destinationId");
            if (o.ExitDialogueId is not null) effects.Add("exitDialogueId");
            if (o.AuditLogIds.Count > 0) effects.Add("auditLogIds");
            if (o.FlagsToSet.Count > 0) effects.Add("flagsToSet");
            return effects;
        }

        private void ValidateConditions(IReadOnlyList<RuleCondition> conditions, string file, string at)
        {
            for (var i = 0; i < conditions.Count; i++)
            {
                var cond = conditions[i];
                var cAt = $"{at}.conditions[{i}]";
                RequireText(cond.Field, file, cAt + ".field");
                var unary = cond.Operator is RuleOperator.IsTrue or RuleOperator.IsFalse;
                if (unary && cond.Value is not null)
                    Error(IssueCodes.InvalidCondition, file, cAt + ".value", $"{cond.Operator} não aceita valor");
                if (!unary && cond.Value is null)
                    Error(IssueCodes.InvalidCondition, file, cAt + ".value", $"{cond.Operator} exige valor");
            }
        }

        private HashSet<string> CollectIds(IEnumerable<string> ids, string at, string what)
        {
            var set = new HashSet<string>();
            var i = 0;
            foreach (var id in ids)
            {
                if (RequireId(id, _matrixFile, $"{at}[{i}].id") && !set.Add(id))
                    Error(IssueCodes.DuplicateId, _matrixFile, $"{at}[{i}]", $"{what} '{id}' duplicado");
                i++;
            }
            return set;
        }

        private void CheckUnique(IEnumerable<(string Id, string Location)> items, string file, string what)
        {
            var seen = new HashSet<string>();
            foreach (var (id, location) in items)
                if (!seen.Add(id))
                    Error(IssueCodes.DuplicateId, file, location, $"{what} '{id}' duplicado");
        }

        private bool RequireId(string? id, string file, string at)
        {
            if (!string.IsNullOrWhiteSpace(id))
                return true;
            Error(IssueCodes.FieldMissing, file, at, "ID obrigatório ausente ou vazio");
            return false;
        }

        private void RequireText(string? text, string file, string at)
        {
            if (string.IsNullOrWhiteSpace(text))
                Error(IssueCodes.FieldMissing, file, at, "campo obrigatório ausente ou vazio");
        }

        private void Error(string code, string file, string? at, string message) =>
            Issues.Add(new ContentIssue(IssueSeverity.Error, code, file, at, message));

        private void Warn(string code, string file, string? at, string message) =>
            Issues.Add(new ContentIssue(IssueSeverity.Warning, code, file, at, message));
    }
}
