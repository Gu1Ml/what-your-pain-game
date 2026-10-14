# Plano de Implementação — Alpha Demo de Triagem: Dia 1

**Feature:** `001-triagem-alpha-dia-1`  
**Data:** 2026-10-14  
**Especificação de origem:** [`spec.md`](spec.md)  
**Estado:** plano técnico para revisão; pontos de conteúdo não resolvidos continuam bloqueados

## Resumo

Implementar uma alpha desktop offline do Dia 1 de Triagem. O projeto usa uma UI 2D fixa, dados de pacientes e protocolos carregados de JSON, lógica de domínio testável fora das cenas, fluxo de turno controlado por managers pequenos e save JSON local depois do resumo/evento entre turnos.

O desenvolvimento será incremental: fundação e validação de conteúdo; domínio e matriz; interface do guichê; relógio/fila; economia/resumo; evento do cachorro; persistência; build e aceite. Não codificar comportamentos de design ainda em aberto.

## Contexto técnico

- **Linguagem/versão:** C# com Godot 4.6.3 .NET.
- **Dependência principal:** Godot Engine 4.6.3 .NET.
- **Bibliotecas externas de runtime:** nenhuma prevista.
- **Serialização:** `System.Text.Json` para DTOs simples de conteúdo/save; revisar compatibilidade de opções de enum e campos opcionais durante a fundação.
- **Persistência:** arquivos JSON locais via APIs da Godot em `user://`.
- **Conteúdo de leitura:** JSON empacotado em `res://content/`.
- **Testes:** testes unitários de domínio e validação de conteúdo. `[PROPOSTA TÉCNICA]` Criar projeto de testes .NET separado com xUnit; confirmar compatibilidade com o SDK/.NET instalado antes da configuração definitiva.
- **Plataforma-alvo:** Windows desktop.
- **Tipo de projeto:** jogo desktop 2D, single-player, offline.
- **Meta funcional:** concluir o Dia 1 sem travamento, com cinco pacientes narrativos e rotina gerada até o fim do tempo-base.
- **Escala:** uma cena de guichê principal, cinco pacientes de história no Dia 1 e um conjunto enxuto de variantes de rotina.

## Restrições de implementação

1. Sem backend ou chamadas de rede.
2. Sem simulação clínica real; todas as regras são ficcionais e vêm do briefing.
3. UI não calcula classificação, multas, reputação ou consciência.
4. Conteúdo e regras são identificados por IDs estáveis; nunca usar a fala/texto da UI como regra de negócio.
5. Um `TriageResult` deve guardar a classificação, a lista de motivos, os lançamentos calculados, destino, diálogo e flags relevantes.
6. Uma decisão não resolvida deve retornar `UNRESOLVED`; não usar default implícito.
7. O save contém DTOs/valores serializáveis, nunca referências a `Node`, `Resource` visual ou objetos de cena carregados.
8. A ordem relativa dos pacientes narrativos do Dia 1 não pode mudar.
9. A geração de rotina deve evitar combinações que dependam da precedência ainda não aprovada.
10. A arquitetura precisa permitir a campanha de sete dias, mas a alpha precisa conter conteúdo jogável somente do Dia 1.

## Verificação da Constituição

| Princípio | Como o plano o cumpre |
|---|---|
| Consequências compreensíveis | `TriageResult`, feedback imediato e resumo com lançamentos explícitos |
| Protocolo/matriz como fonte de verdade | `ProtocolManager` e arquivos JSON dedicados |
| Conteúdo separado da lógica | `content/` para JSON; regras gerais em classes de domínio |
| Incerteza explícita | estados `PROPOSAL`, `OPEN` ou `UNRESOLVED` em decisões/matriz e `docs/decisions.md` |
| Contabilidade rastreável | resultado calculado uma vez e agregado no resumo |
| Dilema ético separado do erro | classificação `ETHICAL_EXCEPTION` e fluxo de efeitos próprios |
| Continuidade narrativa | IDs/flags persistentes no `GameState` |
| Tom humano | conteúdo preservado da fonte; sem sistema que converta casos sérios em gag |
| Entrega incremental | fases em `tasks.md` e `IMPLEMENT.md` |
| Testabilidade/legibilidade | lógica de domínio sem dependência de UI; testes por linha da matriz |
| Offline/save local | conteúdo `res://`; saves em `user://` |
| Fidelidade ao briefing | decisões abertas e propostas registradas sem preenchimento automático |

**Resultado:** plano compatível com a Constituição em draft. O aceite integral do loop é bloqueado até decidir a correspondência das ações e a política global de comissão.

## Arquitetura proposta

### Camadas

1. **Presentation (`scenes/`, `scripts/presentation/`)** — renderiza o estado e envia intenções do jogador.
2. **Application (`scripts/application/`)** — controla fases, relógio, fila, transições e coordena os sistemas.
3. **Domain (`scripts/domain/`)** — protocolo, classificação, economia, reputação/consciência e regras narrativas sem dependência de nós visuais.
4. **Content (`scripts/content/`, `content/`)** — leitura, desserialização e validação de arquivos externos.
5. **Persistence (`scripts/persistence/`)** — snapshot, validação e gravação/carregamento em JSON.
6. **Tests (`tests/`)** — cenários de domínio, economia, dados e gerador.

### Classes e responsabilidades

| Classe | Responsabilidade | API essencial (intenção) |
|---|---|---|
| `GameState.cs` | Estado persistente da campanha e do dia; única porta de alteração persistente | `SetFlag`, `BeginDay`, `ApplyTriageResult`, `ApplyDailyFinancials`, `CreateSnapshot`, `RestoreSnapshot` |
| `DayManager.cs` | Relógio, fase do turno, fim do tempo-base, conclusão quando todos os narrativos passaram | `StartDay`, `StartShiftClock`, `OnClockExpired`, `CanSpawnRoutinePatient`, `CanEndShift`, `FinishDay`, `BuildDaySummary` |
| `PatientManager.cs` | Ordem fixa dos narrativos, rotina intercalada, paciente atual e destino | `LoadPatientSequence`, `GetCurrentPatient`, `AdvanceToNextPatient`, `MarkStoryPatientPassed`, `AreAllStoryPatientsPassed` |
| `RoutinePatientGenerator.cs` | Geração determinística a partir de pools; rejeita combinações conflitantes | `GeneratePatient`, `ValidateGeneratedCombination`, `GetExpectedAction` |
| `ProtocolManager.cs` | Avalia regras de documento/sintoma e retorna IDs dos motivos | `LoadProtocol`, `ValidateDocument`, `EvaluateSymptoms`, `ValidatePatient`, `GetApplicableRules` |
| `TriageController.cs` | Avalia ação, produz/commita resultado, coordena feedback e avanço | `ProcessAction`, `EvaluateAction`, `CommitResult`, `GetFeedback` |
| `EconomyManager.cs` | Calcula comissão, multa, custo ético, despesa fixa, saldo e carry-forward | `CalculateTriageFinancials`, `ApplyResult`, `CalculateDailyExpenses`, `CalculateDailyBalance`, `ApplyCarryForward` |
| `ReputationManager.cs` | Alterações de reputação/consciência e bônus diário | `CalculateMetricChanges`, `ApplyDayCompletionBonus`, `GetEndingBand` |
| `DialogueManager.cs` | Carrega e avança diálogos e escolhas, sem efeitos econômicos escondidos | `LoadDialogueSet`, `StartDialogue`, `AdvanceDialogue`, `ChooseDialogueOption`, `GetAvailableOptions` |
| `NarrativeFlagManager.cs` | Atualiza e consulta flags de continuidade | `SetNarrativeFlag`, `GetNarrativeFlag`, `EvaluateFlagConditions` |
| `SaveManager.cs` | Valida e persiste snapshot local | `SaveGame`, `LoadGame`, `HasValidSave`, `DeleteSave`, `ValidateSave` |
| `DataLoader.cs` | Carrega JSON e valida IDs, campos e referências | `LoadDay`, `LoadProtocol`, `LoadPatients`, `LoadDecisionMatrix`, `LoadDialogues`, `ValidateContent` |
| `CampaignManager.cs` | Inicializa campanha e avança dia (infraestrutura para dias futuros) | `InitializeCampaign`, `CompleteDay` |
| `EndingEvaluator.cs` | Avalia finais/epílogo quando a campanha completa existir | `EvaluateEnding`, `BuildEpilogue`; não é necessário ter finais jogáveis na alpha |

Evitar criar autoloads para todos os managers. `[PROPOSTA TÉCNICA]` Somente `GameState` deve ser Autoload; os demais objetos devem ser criados/gerenciados pelo fluxo da aplicação, salvo necessidade demonstrada.

## Estrutura de diretórios

```text
what-your-pain-game/
├── project.godot
├── WhatYourPain.csproj
├── README.md
├── .specify/
│   └── memory/constitution.md
├── specs/
│   └── 001-triagem-alpha-dia-1/
│       ├── spec.md
│       ├── plan.md
│       ├── tasks.md
│       ├── data-model.md
│       ├── quickstart.md
│       └── contracts/
├── scenes/
│   ├── MainMenu.tscn
│   ├── TriageStation.tscn
│   ├── PatientQueue.tscn
│   ├── Patient.tscn
│   ├── PatientCardUI.tscn
│   ├── DocumentPanel.tscn
│   ├── ProtocolPanel.tscn
│   ├── StampController.tscn
│   ├── DialogueUI.tscn
│   ├── DaySummaryUI.tscn
│   └── NarrativeEventUI.tscn
├── scripts/
│   ├── application/
│   ├── domain/
│   ├── content/
│   ├── generation/
│   ├── persistence/
│   └── presentation/
├── content/
│   ├── campaign.json
│   ├── protocols/day_1_protocol.json
│   ├── patients/day_1_patients.json
│   ├── patients/day_1_narrative_patients.json
│   ├── patients/routine_patient_rules.json
│   ├── decisions/day_1_decision_matrix.json
│   ├── dialogues/day_1_dialogue.json
│   ├── events/dog_event.json
│   └── summaries/day_summaries.json
├── tests/
│   ├── domain/
│   ├── economy/
│   ├── content/
│   └── generation/
└── docs/
    └── decisions.md
```

A estrutura é uma proposta técnica de organização; nomes podem ser ajustados sem mudar contratos de domínio. `RoutinePatientGenerator` pode ser somente uma classe C#, sem cena própria.

## Cenas e composição

- `MainMenu.tscn`: iniciar/continuar; chama `SaveManager` e `CampaignManager`.
- `TriageStation.tscn`: raiz `Control`; guichê, cabeçalho, relógio, painel de paciente e subpainéis.
- `PatientQueue.tscn`: representação visual de fila/destinos; não avalia regras.
- `Patient.tscn`: retrato e identificação de apresentação; recebe `PatientData`.
- `PatientCardUI.tscn`: ficha e sintomas visíveis.
- `DocumentPanel.tscn`: documento e estado de validade.
- `ProtocolPanel.tscn`: folheto carregado do protocolo externo.
- `StampController.tscn`: quatro slots requeridos pela alpha; mapeamento de ação pendente não pode ser presumido.
- `DialogueUI.tscn`: falas e opções narrativas dirigidas por IDs.
- `DaySummaryUI.tscn`: exibe `DaySummary`; não recalcula o domínio.
- `NarrativeEventUI.tscn`: evento do cachorro como tela narrativa simples.

Usar `Control`/containers para interface fixa. Reservar `Node2D` para camada de cenário que realmente necessite posicionamento livre.

## Contratos de conteúdo

### Arquivos

- `content/protocols/day_1_protocol.json`
- `content/patients/day_1_patients.json`
- `content/patients/day_1_narrative_patients.json`
- `content/patients/routine_patient_rules.json`
- `content/decisions/day_1_decision_matrix.json`
- `content/dialogues/day_1_dialogue.json`
- `content/events/dog_event.json`
- `content/summaries/day_summaries.json`

IDs são estáveis. Texto e tradução não podem ser usados como identificador de regra. Cada ação em cada `DecisionCase` deve conter status, motivos, efeitos e destino quando resolvida; casos abertos usam um status explícito sem efeitos definitivos.

### Tipos tipados

`DayConfig`, `PatientData`, `DocumentData`, `ProtocolData`, `ProtocolRule`, `TriageAction`, `DecisionCase`, `ActionConsequence`, `DialogueNode`, `TriageResult`, `DailyFinancialResult`, `DaySummary`, `NarrativeFlag` e `GameSaveData`. Definição de campos em [`data-model.md`](data-model.md).

### Validação na carga

A carga do Dia 1 deve falhar com mensagem explicativa se houver arquivo obrigatório ausente, JSON inválido, ID duplicado, referência desconhecida, campo obrigatório em falta ou ação com consequência incompleta. Conteúdo ainda não aprovado pode existir com `status: "OPEN"` ou `classification: "UNRESOLVED"` e não deve ser tratado como conteúdo de produção aprovado.

## Estado e persistência

`GameState` deve expor propriedades de leitura e métodos para alterar estado. Campos planejados: dia/fase atual, tempo restante, estado de extensão de turno, dinheiro/dívida, comissões, multas, despesas, reputação, consciência, erros, contagem de pacientes, flags de pacientes/narrativa, adoção do cachorro, hora extra, fadiga e seed de rotina.

Salvar snapshot JSON em `user://`. Autosave após conclusão do resumo e após a decisão do cachorro. Não há retomada no meio do atendimento. O modelo matemático definitivo de dinheiro/dívida aguarda resolução da pendência registrada em `docs/decisions.md`.

## Plano de testes

- Cada linha/variante resolvida da matriz deve gerar teste de `TriageController`/`ProtocolManager`.
- Testar a multa única mesmo com múltiplos `audit_log_ids`.
- Testar o cálculo financeiro de Helena e a exclusão da multa genérica.
- Testar o bônus diário com e sem erros genéricos e com a exceção ética.
- Testar que ações `UNRESOLVED` não produzem comissão, multa ou mudança de métrica por default.
- Testar relógio/fila com seed fixa e garantia de todos os narrativos.
- Testar validação de conteúdo para arquivos ausentes, IDs duplicados e referência quebrada.
- Testar round-trip do snapshot e recuperação de save inválido.

## Riscos e contenções

| Risco | Contenção |
|---|---|
| Ação ou comissão indefinida impede o loop completo | Isolar com `UNRESOLVED`; avançar em módulos independentes; fechar antes do aceite integral |
| Paciente de rotina é logicamente contraditório | Gerador valida e rejeita combinações que dependem de precedência não definida |
| UI vira fonte de verdade | Controladores chamam domínio e apresentam resultado imutável |
| Saldo/dívida duplica valores | Centralizar lançamentos e fechar uma única regra de carry-forward antes do saldo final |
| JSON inválido vira erro de gameplay | Validar conteúdo no carregamento e mostrar erro de desenvolvimento claramente |
| Escopo da alpha cresce para os sete dias | Dados futuros preparados apenas quando necessário; conteúdo jogável limitado ao Dia 1 |

## Portões de saída

1. **Fundação:** projeto compila; dados tipados; carregamento/validação; snapshot testável.
2. **Domínio:** matriz e consequências testáveis sem UI; pendências explícitas.
3. **Loop:** inspeção, ação, feedback e próximo paciente funcionam para casos decididos.
4. **Turno:** relógio, rotina e narrativos encerram corretamente.
5. **Resumo/evento:** contabilidade, reputação, consciência, cachorro e flags coerentes.
6. **Alpha:** build Windows executável e critérios de `spec.md` verificados; pendências bloqueadoras reportadas.

## Complexidade justificada

| Decisão | Motivo | Alternativa mais simples rejeitada porque |
|---|---|---|
| Separar domínio da UI | Permite testar decisões e economia isoladamente | Condições diretamente nos botões duplicariam regras e dificultariam QA |
| Dados externos JSON | Protocolo/pacientes mudam por dia e por conteúdo | Hardcode exigiria alterar código a cada mudança de narrativa |
| Flags estáveis | Continuidade de Lucas/Helena/cachorro e epílogo | Derivar estado de texto exibido é frágil |
| Validação de `UNRESOLVED` | Evita decisões inventadas | Default genérico esconderia lacunas de design |
