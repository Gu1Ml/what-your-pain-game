# Modelo de Dados — Alpha Demo de Triagem: Dia 1

**Feature:** `001-triagem-alpha-dia-1`  
**Estado:** contrato inicial de dados; campos marcados como `[EM ABERTO]` não podem receber comportamento definitivo por inferência.

## 1. Princípios

- Os dados de conteúdo ficam separados da lógica de domínio.
- Cada paciente, regra, diálogo, evento e flag persistente usa um ID estável.
- O JSON descreve conteúdo e resultados; não contém código executável.
- Dados inválidos ou combinações que dependem de regras em aberto devem gerar erro de validação ou estado `UNRESOLVED`, nunca uma decisão silenciosa.
- O modelo suporta a campanha de sete dias, embora a alpha implemente o Dia 1.
- Os quatro controles visuais pedidos para a alpha são `ATTEND`, `QUEUE`, `REFER` e `REFUSE`. A semântica final de `QUEUE` versus `REFER` e a correspondência com o rótulo legado `RECOMENDAR/FILA` continuam `[EM ABERTO]`.

## 2. Enums e identificadores

### 2.1 Ação de triagem

```json
[
  "ATTEND",
  "QUEUE",
  "REFER",
  "REFUSE"
]
```

A lista representa os controles solicitados, não uma declaração de que a semântica de todos já foi aprovada.

### 2.2 Classificação do resultado

```json
[
  "CORRECT",
  "PERMITTED",
  "ERROR",
  "ETHICAL_EXCEPTION",
  "UNRESOLVED"
]
```

- `CORRECT`: ação correta segundo a matriz aprovada.
- `PERMITTED`: alternativa explicitamente permitida sem erro.
- `ERROR`: ação incorreta com consequências genéricas aprovadas.
- `ETHICAL_EXCEPTION`: escolha narrativa que viola o protocolo, mas tem regras próprias, como ajudar Helena.
- `UNRESOLVED`: falta uma decisão de design necessária para classificar ou aplicar o resultado.

### 2.3 Tipos de dado

Usar tipos C# explícitos: `string` para IDs e textos; `int` para contagens e métricas inteiras; `decimal` para dinheiro; `float`/`double` para tempo e temperatura; `bool` para estados binários; listas e dicionários tipados para coleções.

## 3. Entidades C# recomendadas

### `DayConfig`

| Campo | Tipo | Descrição |
|---|---|---|
| `dayNumber` | `int` | Dia da campanha. Alpha: `1`. |
| `protocolId` | `string` | ID do protocolo do dia. |
| `baseDurationSeconds` | `int` | 300 segundos no Dia 1. |
| `storyPatientOrder` | `List<string>` | IDs dos cinco pacientes de história na ordem relativa. |
| `routineGenerationEnabled` | `bool` | Permite gerar rotina antes de expirar o tempo-base. |
| `dailyLivingExpense` | `decimal` | $25 por dia, conforme o folheto. |

### `PatientData`

| Campo | Tipo | Descrição |
|---|---|---|
| `id` | `string` | ID estável, por exemplo `marcio_souza`. |
| `patientType` | `string`/enum | `STORY` ou `ROUTINE`. |
| `name` | `string` | Nome apresentado. |
| `age` | `int` | Idade. |
| `profileText` | `string` | Descrição visual curta. |
| `declaredSymptoms` | `List<string>` | Sintomas relatados. |
| `document` | `DocumentData` | Documento apresentado. |
| `visibleFacts` | `List<string>` | Fatos visíveis inicialmente. |
| `hiddenFacts` | `List<string>` | Fatos revelados por interação/diálogo. |
| `dialogueId` | `string` | Referência ao conjunto de falas. |
| `decisionMatrixId` | `string` | Matriz específica do paciente/caso. |
| `epilogueFlag` | `string` opcional | Marca narrativa, se definida. |

### `DocumentData`

| Campo | Tipo | Descrição |
|---|---|---|
| `documentType` | `string` | RG, CPF, CNH ou tipo futuro explicitamente definido. |
| `nameVisible` | `bool` | Nome visível. |
| `identifierVisible` | `bool` | RG/CPF visível conforme regra do Dia 1. |
| `validityStatus` | `string`/enum | `VALID`, `EXPIRED`, `MISSING`, `UNKNOWN`. |
| `issueYear` | `int` opcional | Ano de emissão, quando fornecido. |
| `expiryDate` | `string` opcional | Data ISO, quando aplicável. |

**[EM ABERTO]** O briefing não define plenamente a aceitação de CNH como documento equivalente ao RG/CPF nem a regra para documentos sem validade explícita. Não inferir aceitação a partir do tipo do documento.

### `ProtocolRule`

| Campo | Tipo | Descrição |
|---|---|---|
| `id` | `string` | ID estável da regra. |
| `category` | `string` | `DOCUMENT`, `SYMPTOM`, `ATTESTATION`, etc. |
| `description` | `string` | Texto de regra para o sistema e/ou folheto. |
| `ruleGroup` | `int` | Grupo numerado do folheto, quando aplicável. |
| `conditions` | `List<RuleCondition>` | Condições declarativas permitidas. |
| `appliesToActions` | `List<string>` | Ações que a regra restringe, se definidas. |
| `priority` | `int` opcional | Não definir prioridade enquanto a precedência estiver em aberto. |

### `RuleCondition`

Condição declarativa simples com `field`, `operator` e `value`. Operadores devem ser de uma lista permitida, por exemplo `EQUALS`, `NOT_EQUALS`, `GREATER_THAN`, `LESS_THAN`, `IS_TRUE`, `IS_FALSE`, `CONTAINS`. Sem scripts, expressões arbitrárias ou execução de código vindo do JSON.

### `ActionOutcome`

| Campo | Tipo | Descrição |
|---|---|---|
| `action` | `string`/enum | Ação escolhida. |
| `classification` | `string`/enum | Uma das cinco classificações acima. |
| `reasonRuleIds` | `List<string>` | Todas as regras violadas ou satisfeitas relevantes. |
| `commissionDelta` | `decimal` | Comissão aplicada a este resultado, se resolvida. |
| `fineDelta` | `decimal` | Multa genérica; no erro genérico é uma multa única de $5 por paciente. |
| `specialCostDelta` | `decimal` | Custo excepcional, como $10 para ajudar Helena. |
| `reputationDelta` | `int` | Alteração explicitamente definida. |
| `conscienceDelta` | `int` | Alteração explicitamente definida. |
| `destinationId` | `string` | Destino visual, se definido. |
| `exitDialogueId` | `string` | Fala de saída, se definida. |
| `auditLogIds` | `List<string>` | Registros de auditoria gerados. |
| `flagsToSet` | `Dictionary<string,string>` | Marcas persistentes, quando aplicáveis. |

`ActionOutcome` não deve obrigar todos os casos a possuir consequências definidas. Se um campo depender de decisão aberta, o resultado é `UNRESOLVED` até decisão ser registrada.

### `DecisionMatrix`

```json
{
  "id": "day_1_story_matrix",
  "patientId": "marcio_souza",
  "protocolId": "day_1_protocol",
  "outcomes": [
    {
      "action": "ATTEND",
      "classification": "CORRECT",
      "reasonRuleIds": ["D1_SYMPTOM_ACUTE"],
      "commissionDelta": 5,
      "fineDelta": 0,
      "specialCostDelta": 0,
      "reputationDelta": 0,
      "conscienceDelta": 0,
      "destinationId": "hospital"
    }
  ]
}
```

Este exemplo é ilustrativo e parcial; a matriz completa deve refletir cada paciente e não deve preencher alternativas ainda pendentes.

### `DialogueNode`

| Campo | Tipo | Descrição |
|---|---|---|
| `id` | `string` | ID estável. |
| `speakerId` | `string` | Personagem que fala. |
| `text` | `string` | Fala. |
| `trigger` | `string` | Entrada, inspeção, ação ou saída. |
| `conditions` | `List<RuleCondition>` | Condições declarativas opcionais. |
| `nextDialogueId` | `string` opcional | Próxima fala. |

### `NarrativeEventData`

Contém `id`, `trigger`, `text`, `options` e as flags/efeitos explicitamente associados. No evento do cachorro, `dog_adopted=true` quando o jogador adota; a despesa diária da ração permanece configurável/pendente entre $2 e $5, conforme o briefing original.

### `TriageResult`

Resultado imutável de uma decisão processada: `patientId`, `action`, `classification`, `reasonRuleIds`, `financialEntries`, `reputationDelta`, `conscienceDelta`, `destinationId`, `exitDialogueId`, `auditEntries` e `flagsToSet`. A UI consome o resultado; não recalcula consequências.

### `FinancialEntry`

Cada lançamento tem `id`, `dayNumber`, `patientId` opcional, `entryType`, `amount` decimal e `description`. Tipos sugeridos: `COMMISSION`, `FINE`, `ETHICAL_COST`, `DAILY_EXPENSE`, `OVERTIME_BONUS`, `DOG_EXPENSE`, `DAMAGE_COST`. Um lançamento já aplicado não pode ser aplicado novamente durante a exibição do resumo.

### `GameStateSnapshot`

| Campo | Tipo | Observação |
|---|---|---|
| `schemaVersion` | `int` | Versão do formato de save. |
| `currentDay` | `int` | Dia da campanha. |
| `campaignPhase` | `string` | Fase serializável, não referência a Node. |
| `money` | `decimal` | Saldo disponível/acumulado conforme modelo aprovado. |
| `debt` | `decimal` | Dívida separada, se adotada a representação separada. **[EM ABERTO]** |
| `reputation` | `int` | Começa no ponto neutro definido pelo briefing. |
| `conscience` | `int` | Começa no ponto neutro definido pelo briefing. |
| `errorsToday` | `int` | Contagem de pacientes com erro genérico. |
| `patientsProcessedToday` | `int` | Contagem de pacientes processados. |
| `commissionToday` | `decimal` | Agregado do dia, sem reaplicar lançamentos. |
| `dailyExpenseApplied` | `bool` | Impede aplicar despesa diária duas vezes. |
| `dogAdopted` | `bool` | Flag persistente do evento do cachorro. |
| `overtimeAccepted` | `bool` | Estado de hora extra, quando o dia 5 for implementado. |
| `fatigueLevel` | `int` | Valor e escala ainda precisam de especificação antes do dia 6. |
| `patientFlags` | `Dictionary<string,string>` | Marcas por paciente. |
| `narrativeFlags` | `Dictionary<string,string>` | Marcas de campanha/epílogo. |
| `processedPatientIds` | `List<string>` | Protege contra processamento duplicado. |
| `financialLedger` | `List<FinancialEntry>` | Histórico necessário ao resumo e QA. |
| `randomSeed` | `int` opcional | Para reproduzir a geração de rotina. |

Não salvar referências a nós Godot, delegates, objetos de cena ou instâncias runtime.

## 4. Arquivos JSON sugeridos

```text
content/
├── protocols/day_1_protocol.json
├── patients/day_1_patients.json
├── patients/day_1_narrative_patients.json
├── patients/routine_patient_rules.json
├── decisions/day_1_decision_matrix.json
├── dialogue/day_1_dialogue.json
├── events/dog_event.json
└── summaries/day_summaries.json
```

## 5. Validação de conteúdo

Ao carregar o Dia 1, validar:

1. IDs únicos dentro de cada domínio e referências existentes entre arquivos.
2. Todos os cinco pacientes narrativos presentes e na ordem relativa correta.
3. Cada ação de cada matriz é única; não existem resultados duplicados para a mesma ação.
4. Todos os IDs de regras, diálogos, destinos e flags referenciados existem.
5. Nenhuma combinação de rotina depende de precedência ainda não aprovada.
6. Erros genéricos geram no máximo uma multa por paciente, mesmo com vários `reasonRuleIds`.
7. A exceção de Helena não recebe multa genérica adicional.
8. Campos financeiros usam valores numéricos decimais e não strings localizadas.
9. Dados incompletos que dependam de uma decisão aberta são reportados claramente e não recebem defaults silenciosos.

## 6. Decisões que este modelo não fecha

- Semântica final de FILA, ENCAMINHAR e rótulo legado `RECOMENDAR/FILA`.
- Gatilho global da comissão (paciente atendido versus decisão correta/permitida).
- Precedência entre documento inválido e sintoma agudo.
- Aceitação de CNH e documentos sem validade explícita.
- Representação exata de saldo negativo/dívida.
- Custo de ração do cachorro ($2 ou $5 por dia).
- Se inspeções consomem o relógio.
- Regras completas para gerar rotina e intercalar casos sem colisões.
- Limiares numéricos finais de reputação/consciência e escala de cansaço.
