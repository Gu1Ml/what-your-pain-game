# Tasks: Alpha Demo de Triagem — Dia 1

**Entrada:** `spec.md`, `plan.md`, `data-model.md`, `quickstart.md`, `contracts/decision-matrix-contract.md` e `.specify/memory/constitution.md`.  
**Regra:** executar na ordem; itens `[P]` podem ser paralelizados quando seus arquivos forem independentes e as dependências anteriores estiverem concluídas.  
**Atenção:** tarefas que dependam de decisões marcadas `[EM ABERTO]` podem preparar estrutura/validação, mas não devem implementar uma interpretação definitiva.

## Phase 1 — Setup do projeto

**Objetivo:** criar a base executável e a estrutura de diretórios sem implementar gameplay.

- [ ] T001 Criar/inicializar o projeto Godot 4.6.3 .NET em `project.godot` e `WhatYourPain.csproj`; confirmar build C# mínima.
- [ ] T002 [P] Criar as pastas `scenes/`, `scripts/application/`, `scripts/domain/`, `scripts/content/`, `scripts/generation/`, `scripts/persistence/`, `scripts/presentation/`, `content/`, `tests/` e `docs/`.
- [ ] T003 [P] Adicionar à raiz o `README.md` do jogo, incluindo objetivo da alpha, requisitos locais e referência aos documentos Spec-Kit.
- [ ] T004 Registrar uma configuração de build Windows e confirmar que uma cena vazia pode ser executada/exportada antes de adicionar gameplay.

## Phase 2 — Foundational: contratos e estado

**Objetivo:** concluir as dependências que todas as histórias usam. Nenhuma história deve duplicar dados de domínio em componentes visuais.

- [ ] T005 Criar os DTOs iniciais em `scripts/domain/Models/` para `DayConfig`, `PatientData`, `DocumentData`, `ProtocolData`, `ProtocolRule`, `DecisionCase`, `ActionConsequence`, `TriageResult`, `DailyFinancialResult`, `DaySummary`, `DialogueNode` e `GameSaveData`, seguindo `data-model.md`.
- [ ] T006 Implementar `GameState.cs` em `scripts/application/GameState.cs` com leitura de estado, `SetFlag`, `BeginDay`, `ApplyTriageResult`, `ApplyDailyFinancials`, `CreateSnapshot` e `RestoreSnapshot`; restringir mutações diretas.
- [ ] T007 Implementar `DataLoader.cs` em `scripts/content/DataLoader.cs` para ler JSON de `res://content/`, desserializar DTOs e informar arquivo/campo/ID em erros.
- [ ] T008 Implementar validação de conteúdo em `scripts/content/ContentValidator.cs`, incluindo IDs duplicados, referências inexistentes e ações incompletas/`UNRESOLVED`.
- [ ] T009 [P] Criar os arquivos iniciais de conteúdo `content/protocols/day_1_protocol.json`, `content/patients/day_1_narrative_patients.json`, `content/dialogues/day_1_dialogue.json` e `content/decisions/day_1_decision_matrix.json` com os conteúdos já suportados pela fonte; não preencher lacunas de design por suposição.
- [ ] T010 [P] Criar `docs/decisions.md` no repositório do jogo copiando o registro de decisões desta distribuição e preparar formato de decisão com ID, pergunta, estado, opções e impacto.
- [ ] T011 Criar projeto de teste isolado em `tests/`; `[PROPOSTA TÉCNICA]` usar xUnit após confirmar compatibilidade com o SDK .NET local.
- [ ] T012 Criar testes de validação de conteúdo em `tests/content/` para arquivo ausente, JSON inválido, ID duplicado, referência quebrada e ação `UNRESOLVED` com efeito definitivo.

## Phase 3 — US1: Iniciar o plantão e consultar o protocolo

**Objetivo da história:** começar o Dia 1 com orientação clara antes da primeira decisão.  
**Teste independente:** novo jogo → passagem de Ralel → folheto → guichê.

- [ ] T013 [US1] Criar `scenes/MainMenu.tscn` e o script de apresentação correspondente em `scripts/presentation/MainMenuController.cs` para iniciar campanha nova e, quando aplicável, continuar save válido.
- [ ] T014 [P] [US1] Criar `scenes/TriageStation.tscn` como UI fixa 2D com cabeçalho, relógio, painel de paciente, folheto e área dos carimbos.
- [ ] T015 [US1] Implementar a fase inicial de `DayManager.cs` em `scripts/application/DayManager.cs`, incluindo passagem de turno e carregamento de `day_1`.
- [ ] T016 [US1] Criar `scenes/ProtocolPanel.tscn` e conectar a exibição ao `ProtocolData` carregado, sem duplicar texto do protocolo no script.
- [ ] T017 [US1] Criar teste de fluxo para inicialização do Dia 1 e disponibilidade das três categorias de regra em `tests/integration/DayStartTests.cs`.

## Phase 4 — US2: Inspecionar pacientes e aplicar a matriz de decisão

**Objetivo da história:** testar o loop central de cada paciente sem depender da geração de rotina.  
**Teste independente:** carregar uma fixture de paciente, inspecionar dados/revelações e comparar cada ação com sua consequência.

- [ ] T018 [US2] Implementar `ProtocolManager.cs` em `scripts/domain/ProtocolManager.cs` para documento, sintomas e motivos de regra, sem inventar precedência em conflitos abertos.
- [ ] T019 [US2] Implementar `TriageController.cs` em `scripts/application/TriageController.cs` para avaliar/commitar `TriageResult` com `CORRECT`, `PERMITTED`, `ERROR`, `ETHICAL_EXCEPTION` e `UNRESOLVED`.
- [ ] T020 [P] [US2] Criar `scenes/Patient.tscn`, `scenes/PatientCardUI.tscn` e `scenes/DocumentPanel.tscn` para apresentar retrato, ficha e documento sem conter regras de decisão.
- [ ] T021 [US2] Criar a interação de termômetro de Lucas e expor o fato `temperature_celsius=36.6` por meio do controlador de inspeção em `scripts/presentation/PatientInspectionController.cs`.
- [ ] T022 [US2] Criar `scenes/StampController.tscn` com os quatro slots ATENDER, FILA, ENCAMINHAR e RECUSAR; impedir commit definitivo em ações sem mapeamento aprovado.
- [ ] T023 [US2] Criar apresentação de falas de saída e feedback em `scenes/DialogueUI.tscn` e `scripts/presentation/DialogueController.cs`, consumindo IDs de diálogo e resultado.
- [ ] T024 [US2] Criar testes da matriz de Márcio, Camila, Lucas, Helena e Roberto em `tests/domain/TriageDecisionTests.cs`, incluindo multa única, exceção da Helena e casos `UNRESOLVED`.
- [ ] T025 [US2] Criar logs de auditoria com IDs de regras em `scripts/domain/AuditLogEntry.cs` e integrar os logs a `TriageResult` sem duplicar multa por motivo.

## Phase 5 — US3: Rotina, fila e relógio

**Objetivo da história:** simular a pressão do turno e garantir que os casos narrativos não sejam perdidos.  
**Teste independente:** executar fila com seed conhecida e confirmar expiração, bloqueio de rotina e término após os cinco pacientes de história.

- [ ] T026 [US3] Implementar `PatientManager.cs` em `scripts/application/PatientManager.cs` para preservar a ordem relativa de `marcio_souza`, `camila_torres`, `lucas_mendes`, `helena_rios` e `roberto_paiva`.
- [ ] T027 [US3] Implementar `RoutinePatientGenerator.cs` em `scripts/generation/RoutinePatientGenerator.cs` com seed reproduzível, pools externos e validação contra combinações que dependem de precedência aberta.
- [ ] T028 [US3] Criar `content/patients/day_1_patients.json` e `content/patients/routine_patient_rules.json` com pools de rotina apoiados pelo protocolo. Manter 60/40 como `[PROPOSTA]` e configurável.
- [ ] T029 [US3] Implementar relógio de 300 segundos e condições `CanSpawnRoutinePatient`/`CanEndShift` em `scripts/application/DayManager.cs`.
- [ ] T030 [P] [US3] Criar `scenes/PatientQueue.tscn` e o feedback visual de destinos aprovados; não presumir destino de `UNRESOLVED`.
- [ ] T031 [US3] Criar testes de seed, ordem narrativa, fim de rotina ao expirar o tempo e conclusão após narrativos pendentes em `tests/generation/PatientQueueTests.cs`.

## Phase 6 — US4: Consequências financeiras, reputação e resumo

**Objetivo da história:** fechar a conta do turno e mostrar consequências coerentes.  
**Teste independente:** aplicar uma lista conhecida de resultados e comparar resumo/ledger com valores esperados.

- [ ] T032 [US4] Implementar `EconomyManager.cs` em `scripts/domain/EconomyManager.cs` para calcular comissão por resultado, multa única, custos éticos e despesas diárias; manter o gatilho global de comissão marcado como bloqueio enquanto a divergência de briefing não for resolvida.
- [ ] T033 [US4] Implementar `ReputationManager.cs` em `scripts/domain/ReputationManager.cs` com os efeitos fechados da Helena, consciência e bônus +1 por dia sem erros genéricos; não adicionar penalidade geral por erro comum.
- [ ] T034 [US4] Implementar agregação única do `DaySummary` em `scripts/application/DayManager.cs`, evitando reaplicar lançamentos durante a renderização do resumo.
- [ ] T035 [P] [US4] Criar `scenes/DaySummaryUI.tscn` para apresentar contagens, finanças, métricas e evento seguinte usando dados já calculados.
- [ ] T036 [US4] Criar testes financeiros em `tests/economy/EconomyManagerTests.cs`, incluindo Helena, multa única de $5, despesa fixa de $25 e prevenção de lançamentos duplicados.

## Phase 7 — US5: Evento do cachorro e persistência local

**Objetivo da história:** concluir o Dia 1 e preservar as escolhas relevantes.  
**Teste independente:** concluir turno, escolher adoção/não adoção, salvar, reiniciar e validar o estado restaurado.

- [ ] T037 [US5] Criar `content/events/dog_event.json` com o texto e as duas opções documentadas no `dia-1.md`; manter custo de ração como decisão aberta sem efetuar débito no Dia 1.
- [ ] T038 [US5] Criar `scenes/NarrativeEventUI.tscn` e o controlador em `scripts/presentation/NarrativeEventController.cs` para registrar `dog_adopted` após confirmação.
- [ ] T039 [US5] Implementar `SaveManager.cs` em `scripts/persistence/SaveManager.cs` para salvar/carregar snapshot JSON em `user://`, validar `schema_version` e lidar com save ausente/inválido.
- [ ] T040 [US5] Acionar autosave após resumo e após confirmação do evento do cachorro; atualizar a fase de campanha em `GameState`.
- [ ] T041 [US5] Criar testes de round-trip de save/load e recuperação de save inválido em `tests/persistence/SaveManagerTests.cs`.

## Phase 8 — Polish, QA e build da alpha

**Objetivo:** verificar o escopo funcional, a build e as pendências documentadas.

- [ ] T042 [P] Adicionar feedback mínimo de papel, documento, clique e carimbo, sem bloquear a legibilidade, em `scenes/` e `assets/audio/`.
- [ ] T043 [P] Incluir assets visuais provisórios para guichê, retratos de história, documentos e evento do cachorro em `assets/`.
- [ ] T044 Executar o smoke test de `quickstart.md` e registrar resultados em `docs/alpha-acceptance.md`.
- [ ] T045 Verificar compilação e exportação Windows; documentar versão exata do editor .NET e SDK usados em `README.md`.
- [ ] T046 Revisar todas as pendências de `docs/decisions.md`; marcar bloqueios e não declarar a matriz completa como aprovada enquanto ações/comissão relevantes estiverem `UNRESOLVED`.

## Dependências e ordem de execução

- Phase 1 precede todas as fases.
- Phase 2 precede qualquer fase de história; dados e validação são fundação.
- US1 cria o ciclo inicial e a UI base; US2 implementa a decisão por paciente.
- US3 usa o loop de paciente de US2.
- US4 consome `TriageResult` de US2 e contagens de US3.
- US5 consome resumo de US4 e estado persistente de Phase 2.
- Phase 8 depende de todas as fases funcionais.

## Estratégia de implementação

Entregar primeiro um loop vertical mínimo (um paciente carregado por dados, uma inspeção, uma decisão mapeada e um resultado), ampliar para os cinco pacientes da história, depois incluir rotina/tempo, resumo e cachorro. Implementar em paralelo somente tarefas que não escrevam no mesmo arquivo nem dependam de uma decisão aberta. Um resultado `UNRESOLVED` é resultado de validação, não comportamento final de gameplay.
