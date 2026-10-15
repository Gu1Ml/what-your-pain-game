# TRIAGEM — Briefing técnico e de design da Alpha Demo

**Versão do documento:** 1.0 — reestruturação técnica do briefing v0.1 e de `dia-1.md` v0.2  
**Engine-alvo:** Godot 4.6.3 .NET  
**Linguagem:** C#  
**Plataforma inicial:** Windows desktop  
**Modo:** single-player, offline  
**Escopo desta entrega:** alpha demo jogável do Dia 1  
**Público-alvo:** programador Godot 4.6.3 + C# e designer de níveis/UI  
**Estado:** especificação de planejamento; sem implementação de código neste documento

> **Regra de leitura:** este documento consolida o conteúdo enviado e as decisões fechadas nesta conversa. Ele não autoriza completar lacunas por suposição. Itens `[PROPOSTA]` continuam sendo sugestões; itens `[EM ABERTO]` exigem decisão de design; itens `[FECHADO]` são compromissos já definidos. Requisitos técnicos são identificados como `[REQUISITO TÉCNICO]`.

## 1. Contexto prévio e visão geral do jogo

### 1.1 Premissa

O jogador trabalha como triador em um hospital. Os pacientes chegam ao guichê com sintomas, documentos e histórias que podem ser verdadeiras, exageradas ou absurdas. O jogador tem poucos minutos para conferir as informações, aplicar o protocolo do dia e decidir quem é atendido, espera, é encaminhado ou tem atendimento recusado.

O jogo busca provocar **pressão de tempo, pequenas injustiças e riso nervoso**. O humor deve surgir dos personagens, dos diálogos e da burocracia, no espírito de jogos de decisão documental como *Papers, Please*. Casos emocionalmente sérios não devem virar piada; o contraste entre humor cotidiano e dilemas humanos dá peso ao epílogo.

### 1.2 Loop central

Para cada paciente:

1. O paciente chega ao guichê.
2. O jogador lê a ficha e o relato do paciente.
3. O jogador confere documentos, sintomas e informações adicionais disponíveis por interação.
4. O protocolo do dia é usado para avaliar a decisão.
5. O jogador escolhe um carimbo/ação.
6. O jogo apresenta feedback imediato, aplica consequências financeiras e narrativas e registra a decisão.
7. O paciente segue para seu destino visual e o próximo paciente chega.

**Objetivo técnico da alpha demo:** provar esse loop ponta a ponta com o conteúdo do Dia 1: chegada do paciente, leitura de ficha/documento, validação contra o protocolo, escolha de ação, consequência imediata, atualização das variáveis e próximo paciente.

### 1.3 Fantasia e tensão principal

A tensão central é **ganhar mais atendendo rápido versus decidir de acordo com as regras ou com a consciência**. A comissão por decisões corretas, as multas, as despesas fixas e a dívida tornam a pressão financeira concreta. Reputação e consciência acumulam o peso das decisões ao longo dos sete dias.

### 1.4 Campanha

- `[FECHADO]` A campanha planejada tem **7 dias**.
- `[FECHADO]` Reputação e consciência começam em um ponto neutro.
- `[FECHADO]` No fim do Dia 7, uma métrica é considerada alta quando está acima do ponto neutro inicial. Os limiares poderão ser ajustados depois de testar a campanha.
- `[FECHADO]` Dinheiro pressiona o jogador e aparece no epílogo, mas não decide diretamente o final.
- `[PROPOSTA]` Quatro finais a partir das combinações de reputação alta/baixa e consciência alta/baixa: “plantonista de confiança”, “promovido, mas à custa dos outros”, “desligado, mas com a cabeça erguida” e “desligado sem ter feito diferença”.
- `[FECHADO]` O desfecho do garoto integra o epílogo de cada final. Ralel não tem desfecho próprio.

### 1.5 Tom e apresentação

- Ambientação hospitalar e burocrática, humor por diálogos curtos e situações excêntricas.
- Casos sérios são tratados com empatia e sem transformar sofrimento em piada.
- Epílogo emocional: a consequência das escolhas deve ser reconhecível no texto, não apenas em um número.
- Arte inicial enxuta: guichê fixo, papéis/fichas como interface e retratos de pacientes em pixel art própria ou peças combináveis.
- `[PROPOSTA]` Evitar animações complexas durante a alpha.

## 2. Prompt real / especificação da alpha demo

### 2.1 Objetivo

Entregar uma alpha demo executável do Dia 1, com começo, meio e fim. O jogador deve conseguir iniciar o plantão, receber pacientes narrativos e rotineiros, examinar informações, escolher uma ação, ver o resultado, fechar o dia, consultar o resumo financeiro/narrativo, tomar a decisão do cachorro e salvar o progresso.

A alpha deve comprovar a arquitetura e as regras, não representar o jogo inteiro. O conteúdo dos Dias 2–7 permanece documentado como campanha futura, mas não é requisito de conteúdo jogável nesta primeira fatia vertical.

### 2.2 Requisitos de execução

- `[REQUISITO TÉCNICO]` Projeto criado com a edição **Godot 4.6.3 .NET** e C#.
- `[REQUISITO TÉCNICO]` Build local para Windows.
- `[REQUISITO TÉCNICO]` Jogo single-player e offline; nenhum backend ou serviço online.
- `[REQUISITO TÉCNICO]` Regras e conteúdo narrativo carregados de arquivos externos, não codificados diretamente nos componentes de UI.
- `[REQUISITO TÉCNICO]` A lógica de domínio deve ser testável sem precisar clicar na cena principal.
- `[REQUISITO TÉCNICO]` Salvar/carregar dados locais; não é exigido retomar no meio do atendimento de um paciente.

### 2.3 Escopo funcional obrigatório

1. Tela inicial simples com opção de iniciar/continuar quando houver save válido.
2. Passagem de turno de Ralel e apresentação do folheto do Dia 1.
3. Cena fixa do guichê.
4. Interface de ficha, documento, sintomas e protocolo do dia.
5. Cinco pacientes narrativos do Dia 1: Márcio, Camila, Lucas, Helena e Roberto.
6. Gerador de pacientes de rotina por combinação de sintoma, problema de documento e traço de personalidade.
7. Relógio com base de cinco minutos reais.
8. Controles de ação para ATENDER, FILA, ENCAMINHAR e RECUSAR, conforme o escopo técnico solicitado. **A correspondência entre esses quatro controles e o carimbo composto RECOMENDAR/FILA do rascunho original continua `[EM ABERTO]`; ver seção 17.**
9. Avaliação da decisão contra a matriz de regras do Dia 1.
10. Consequências imediatas: fala de saída, destino visual, comissão, multa, registro de erro e métricas de campanha quando aplicável.
11. Fim do turno após o término do tempo base e a passagem de todos os pacientes narrativos pendentes.
12. Resumo de fim do dia com contagens, comissão, multas, custo de escolhas éticas, despesas fixas, saldo, reputação e consciência.
13. Evento do cachorro entre os turnos, com escolha de levar ou deixar o filhote.
14. Save/load básico do estado do Dia 1 e das decisões narrativas relevantes.
15. Interface inicial de campanha preparada para carregar os dias seguintes por dados, sem exigir o conteúdo completo desses dias na alpha.

### 2.4 Fora do escopo da alpha

Preservam-se as exclusões do briefing original:

- Sem editor de conteúdo.
- Sem multiplayer.
- Sem 3D.
- Sem vozes/dublagem.
- Sem simulação médica realista; as regras de triagem são ficcionais.
- Sem mais de uma sala/cenário jogável principal. O evento entre turnos pode ser uma tela narrativa simples, sem construir uma segunda área explorável.
- Sem backend.
- Sem necessidade de retomada no meio do atendimento atual.

## 3. Escopo mínimo jogável — Dia 1

### 3.1 Sequência da alpha

1. **Menu:** iniciar novo jogo ou continuar de um checkpoint válido.
2. **Passagem de turno:** Ralel apresenta o posto e alerta sobre o protocolo.
3. **Folheto:** o jogador pode consultar as três regras de validação do Dia 1.
4. **Guichê:** os cinco pacientes narrativos aparecem na ordem fixa indicada em `dia-1.md`, intercalados com pacientes de rotina.
5. **Inspeção:** ficha, documento e sintomas ficam disponíveis; Lucas tem uma interação de termômetro que revela 36,6 °C.
6. **Decisão:** o jogador escolhe uma ação. O motor avalia a decisão, aplica consequências e registra o resultado.
7. **Continuidade:** o paciente sai/é encaminhado/entra no fluxo visual correspondente; o próximo paciente aparece.
8. **Relógio:** pacientes de rotina deixam de ser gerados ao terminar os cinco minutos-base. Pacientes narrativos que ainda não passaram continuam entrando.
9. **Resumo:** exibe resultados do plantão e calcula saldo do dia.
10. **Evento entre turnos:** exibe o filhote e permite levá-lo para casa ou continuar andando.
11. **Checkpoint:** salva o estado concluído do Dia 1 e as decisões narrativas.

### 3.2 Tempo e fila

- `[FECHADO]` O relógio base é de **5 minutos reais**; a experiência total costuma durar de 5 a 7 minutos.
- `[FECHADO]` Pacientes de rotina são gerados até o fim do relógio-base.
- `[FECHADO]` O turno não termina até que todos os pacientes de história definidos para aquele dia tenham passado.
- `[FECHADO]` Depois do fim do tempo-base, entram apenas pacientes de história ainda pendentes; não são gerados novos pacientes de rotina.
- `[PROPOSTA]` Atendimento lento/turno estendido aplica −1 de reputação no resumo do dia.
- `[EM ABERTO]` O algoritmo exato de intercalação dos pacientes de rotina entre os cinco pacientes narrativos não está definido. O gerador não pode alterar a ordem relativa de Márcio, Camila, Lucas, Helena e Roberto.
- `[EM ABERTO]` O comportamento de pausa do relógio ao abrir diálogos, folheto ou inspeções ainda precisa ser definido. Não assumir pausa automática.

### 3.3 Pacientes de rotina

Os pacientes de rotina não têm dilema ético. Devem usar combinações coerentes dos seguintes componentes:

- Nome e idade sorteados de uma lista de conteúdo.
- Sintoma pertencente a um dos grupos do protocolo do Dia 1.
- Documento válido, vencido ou ausente/incompleto.
- Traço de personalidade com uma fala curta (por exemplo: apressado, desconfiado, simpático, culpa o sistema).
- Resultado esperado e consequências definidos de acordo com as regras aplicáveis.

`[PROPOSTA]` A proporção inicial é aproximadamente 60% de pacientes cuja decisão correta é ATENDER e 40% em que se deve recusar ou encaminhar. Ajustar depois de testar.

**Regra técnica de segurança de conteúdo:** o gerador não deve criar combinações contraditórias sem que exista uma regra de precedência definida. Como a precedência entre documento inválido e sintoma agudo ainda está `[EM ABERTO]`, o conjunto de combinações da alpha deve evitar esse conflito até a decisão ser tomada.

## 4. Estrutura de cenas Godot

### 4.1 Convenção recomendada

A experiência é predominantemente uma interface 2D fixa. Portanto, a cena do guichê pode usar `Control` como raiz, com `TextureRect`, `PanelContainer`, `MarginContainer`, `HBoxContainer`, `VBoxContainer`, `RichTextLabel`, `TextureButton` e `ProgressBar` para compor o posto e a interface. `Node2D` fica reservado a uma camada de cenário que precise de posicionamento 2D livre; não é obrigatório para todo componente visual.

As cenas devem ser componentes reutilizáveis. A lógica de regras e economia não deve depender da hierarquia visual nem ficar espalhada nos sinais dos botões.

### 4.2 Árvore de cenas sugerida

```text
res://scenes/
├── MainMenu.tscn
├── TriageStation.tscn
├── PatientQueue.tscn
├── Patient.tscn
├── PatientCardUI.tscn
├── DocumentPanel.tscn
├── ProtocolPanel.tscn
├── StampController.tscn
├── DialogueUI.tscn
├── DaySummaryUI.tscn
├── NarrativeEventUI.tscn
└── RoutinePatientGenerator.tscn   # opcional; pode ser somente uma classe C#
```

### 4.3 Responsabilidade por cena

| Cena | Raiz/nodes recomendados | Responsabilidade e conexões |
|---|---|---|
| `MainMenu.tscn` | `Control`, título, botões Novo Jogo/Continuar/Sair, `ConfirmationDialog` opcional | Inicia `GameState`, verifica save com `SaveManager` e abre `TriageStation`. Não contém regras de triagem. |
| `TriageStation.tscn` | `Control`, `TextureRect`/fundo, cabeçalho, painel central, subpainéis, overlay de diálogo | Cena central do plantão. Escuta sinais de `DayManager`, `PatientManager`, `TriageController` e atualiza os componentes visuais. |
| `PatientQueue.tscn` | `Control`, `VBoxContainer` ou área de fila, retratos/ícones | Representa pacientes em espera e destino visual de FILA. Não decide se o paciente está correto. |
| `Patient.tscn` | `Control` composto por retrato, nome, idade e animação simples opcional | Representação visual de um paciente. Recebe `PatientData`; não é fonte de verdade para dados ou decisão. |
| `PatientCardUI.tscn` | `PanelContainer`, `Label`/`RichTextLabel` e botão de inspeção | Exibe sintomas declarados, histórico e dados visíveis. Pede ao controlador os detalhes liberados por interação. |
| `DocumentPanel.tscn` | `PanelContainer`, retrato/ícone de documento, campos e botão ampliar | Apresenta tipo, nome, número e validade do documento. Deve permitir distinguir dados sempre visíveis de dados que exigem clique/inspeção. |
| `ProtocolPanel.tscn` | `PanelContainer`, regras em `RichTextLabel`/lista e botão abrir/fechar | Exibe o protocolo do dia a partir de `ProtocolData`. É uma representação consultável dos dados; texto não deve ser duplicado no código. |
| `StampController.tscn` | `Control`, botões/carimbos de ação e feedback | Emite `ActionSelected(actionId)`. Os quatro botões solicitados são exibidos na alpha, mas a semântica de FILA versus ENCAMINHAR deve permanecer configurável e marcada como pendente. |
| `DialogueUI.tscn` | `PanelContainer`, nome do interlocutor, texto e botão continuar/escolha | Exibe diálogos em nós e opções. Notifica `DialogueManager` ao avançar ou escolher uma opção. |
| `DaySummaryUI.tscn` | `Control`, painéis de estatísticas, finanças, reputação e consciência | Exibe o resultado retornado por `DayManager`/`EconomyManager`. Não recalcula regras independentes na UI. |
| `NarrativeEventUI.tscn` | `Control`, ilustração de fundo, texto e botões | Usada para eventos entre turnos, iniciando pelo cachorro. Cada escolha atualiza flags/contas por métodos do `GameState`. |
| `RoutinePatientGenerator.tscn` | Se necessário, `Node` ou `Control` sem interface | A geração pode ser só uma classe C# (`RoutinePatientGenerator.cs`). Não criar cena se isso não trouxer benefício de composição visual. |

### 4.4 Navegação entre cenas

Fluxo da alpha:

`MainMenu` → passagem de turno/folheto na `TriageStation` → ciclo de pacientes na `TriageStation` → `DaySummaryUI` → `NarrativeEventUI` (cachorro) → checkpoint concluído do Dia 1.

`[REQUISITO TÉCNICO]` A mudança de fase deve ser comandada por `DayManager`/`GameState`, não por referências diretas entre botões distantes. A UI emite eventos; os controladores validam e alteram estado.

## 5. Arquitetura técnica C#

### 5.1 Princípios

- Separar apresentação, aplicação/fluxo, domínio/regras, conteúdo e persistência.
- Um conjunto pequeno de classes explícitas é preferível a um framework de eventos ou injeção de dependência pesado para este projeto solo.
- A UI não determina ações corretas; solicita uma avaliação ao domínio e apresenta o resultado recebido.
- Nenhuma regra de paciente deve depender de strings de texto exibidas na tela. Usar IDs estáveis.
- Todas as alterações persistentes de estado passam por métodos de `GameState`, para facilitar debug, save/load e QA.
- Os dados narrativos e regras ficam em JSON; a C# implementa o interpretador dos dados, não uma versão codificada de cada paciente.

### 5.2 Divisão lógica

1. **Presentation:** cenas Godot e componentes UI.
2. **Application:** fluxo do turno, fila, início/fim do dia e ligação dos sistemas.
3. **Domain:** protocolo, avaliação das decisões, economia, reputação/consciência e condições narrativas.
4. **Content:** leitura, desserialização e validação de JSON.
5. **Persistence:** snapshots JSON em `user://`.
6. **Tests:** testes automatizados da matriz, economia, gerador e leitura dos dados.

### 5.3 Classes e métodos públicos essenciais

Os métodos a seguir são contratos de intenção, não implementação completa. Assinaturas exatas podem ser ajustadas ao padrão C# do projeto, desde que o comportamento público permaneça equivalente.

#### `GameState.cs`

**Papel:** estado persistente central. Recomenda-se registrar como Autoload/Singleton (classe C# derivada de `Node`) para acesso entre cenas.

**Campos/estado:** campanha atual, dia, fase, relógio, saldo/dívida, totais financeiros do dia, métricas, erros, pacientes processados, flags narrativas, cachorro, hora extra e cansaço.

**Métodos:**

- `SetFlag(string key, object value)` — registra uma flag narrativa compatível com serialização JSON.
- `GetFlag<T>(string key, T fallback)` — lê uma flag tipada com valor padrão.
- `BeginDay(int dayNumber)` — zera os contadores diários sem apagar a campanha.
- `ApplyTriageResult(TriageResult result)` — aplica em um único ponto as consequências persistentes da triagem.
- `ApplyDailyFinancials(DailyFinancialResult result)` — consolida a conta diária sem duplicar lançamentos.
- `SetPhase(string phaseId)` — altera a fase do fluxo.
- `CreateSnapshot()` — produz um DTO serializável para save.
- `RestoreSnapshot(GameSaveData data)` — restaura estado validado.

`[REQUISITO TÉCNICO]` Não expor os campos persistentes como mutáveis por qualquer componente. O estado é lido por propriedades e alterado por métodos. Flags persistidas devem conter valores serializáveis (booleanos, números, strings ou estruturas DTO), não referências de nós Godot.

#### `DayManager.cs`

**Papel:** conduz o dia, relógio, regra de parar a geração de rotina, extensão para os pacientes narrativos restantes e transições.

**Métodos:**

- `StartDay(int dayNumber)`
- `StartShiftClock()`
- `OnClockExpired()`
- `CanSpawnRoutinePatient()`
- `CanEndShift()`
- `FinishDay()`
- `BuildDaySummary()`
- `ContinueToBetweenShiftEvent()`

`CanEndShift()` só é verdadeiro quando o tempo-base terminou e todos os pacientes de história exigidos pelo conteúdo do dia já passaram. O relógio/base exato quando diálogos estão abertos fica `[EM ABERTO]`.

#### `PatientManager.cs`

**Papel:** controla ordem dos pacientes narrativos, inserção/intercalação de rotina, paciente atual e destino visual depois da decisão.

**Métodos:**

- `LoadPatientSequence(DayConfig day)`
- `GetCurrentPatient()`
- `AdvanceToNextPatient()`
- `MarkStoryPatientPassed(string patientId)`
- `TryQueueRoutinePatient()`
- `MovePatientToDestination(string patientId, string destinationId)`
- `AreAllStoryPatientsPassed()`

Preserva a ordem relativa dos cinco pacientes de história do Dia 1. A regra exata de intercalação de rotina ainda é `[EM ABERTO]`.

#### `RoutinePatientGenerator.cs`

**Papel:** cria instâncias de `PatientData` a partir de pools e regras de geração externas.

**Métodos:**

- `GeneratePatient(int dayNumber, int seed)`
- `ValidateGeneratedCombination(PatientData patient, ProtocolData protocol)`
- `GetExpectedAction(PatientData patient, ProtocolData protocol)`

Deve usar uma seed registrada no estado para permitir reprodução de bugs. Se for usada uma distribuição 60/40, ela permanece marcada `[PROPOSTA]`. Não criar combinações em que o resultado depende de precedência ainda não decidida.

#### `ProtocolManager.cs`

**Papel:** aplica as regras declarativas do protocolo a documentos, sintomas e evidências reveladas.

**Métodos:**

- `LoadProtocol(int dayNumber)`
- `ValidateDocument(DocumentData document)`
- `EvaluateSymptoms(PatientData patient)`
- `ValidatePatient(PatientData patient)`
- `GetApplicableRules(PatientData patient)`

Deve devolver resultados estruturados com IDs e motivos, não apenas um booleano. A precedência em casos com documento inválido e sintoma agudo continua `[EM ABERTO]`.

#### `TriageController.cs`

**Papel:** recebe ação do jogador, compara a ação escolhida com a matriz de decisão, cria resultado e comanda feedback/avanço.

**Métodos:**

- `ProcessAction(string patientId, string action)`
- `EvaluateAction(string patientId, string action)`
- `CommitResult(TriageResult result)`
- `GetFeedback(TriageResult result)`

**Regra importante:** a avaliação retorna classificação `CORRECT`, `PERMITTED`, `ERROR`, `ETHICAL_EXCEPTION` ou `UNRESOLVED`. A classificação `UNRESOLVED` bloqueia a aplicação silenciosa de consequências; o jogo deve apresentar estado de desenvolvimento/validação ou impedir a build de release da alpha até que o mapeamento pendente seja configurado.

#### `EconomyManager.cs`

**Papel:** calcula os efeitos financeiros da decisão e do dia, sem alterar diretamente campos do `GameState`.

**Métodos:**

- `CalculateTriageFinancials(TriageResult result)`
- `ApplyResult(TriageResult result)`
- `CalculateDailyExpenses(int dayNumber, CampaignState state)`
- `CalculateDailyBalance(DailyFinancialInput input)`
- `ApplyCarryForward(DailyFinancialResult result)`

Valores de origem: comissão de $5; multa de $5 por paciente com erro; despesas fixas de $25 por dia; escolhas éticas custam $10. **A relação exata entre saldo negativo e variável separada de dívida precisa seguir a decisão financeira que fechar a divergência identificada na seção 17.**

#### `ReputationManager.cs`

**Papel:** calcula alterações em reputação e consciência separadas das consequências monetárias.

**Métodos:**

- `CalculateMetricChanges(TriageResult result)`
- `ApplyDayCompletionBonus(DaySummary summary)`
- `GetEndingBand(float metric, float neutralBaseline)`

A escolha ética de Helena dá reputação −1 e consciência +1; seguir o protocolo no dilema dá reputação +1 e consciência +0. O bônus de reputação +1 por dia sem erros genéricos foi aprovado nesta conversa. A punição de −1 por turno estendido continua `[PROPOSTA]`.

#### `DialogueManager.cs`

**Papel:** carrega e executa linhas, escolhas e condições narrativas.

**Métodos:**

- `LoadDialogueSet(int dayNumber)`
- `StartDialogue(string dialogueId)`
- `AdvanceDialogue()`
- `ChooseDialogueOption(string optionId)`
- `GetAvailableOptions(string dialogueId)`

O fluxo de diálogo deve ser dirigido por IDs. O avanço das falas não deve alterar comissão ou reputação por efeitos colaterais escondidos; as consequências ficam na matriz/ações de conteúdo.

#### `NarrativeFlagManager.cs`

**Papel:** registra marcas para continuidade e epílogo.

**Métodos:**

- `SetNarrativeFlag(string key, string value)`
- `GetNarrativeFlag(string key)`
- `EvaluateFlagConditions(string conditionGroupId)`

Flags conhecidas incluem `lucas_visita_1`, `helena=recusada`, `helena=ajudada` e `dog_adopted`. Usar IDs estáveis, sem depender do texto localizado.

#### `SaveManager.cs`

**Papel:** serializa snapshots validados no armazenamento local.

**Métodos:**

- `SaveGame()`
- `LoadGame()`
- `HasValidSave()`
- `DeleteSave()`
- `ValidateSave(GameSaveData data)`

Salvamento em JSON em `user://`. Usar schema version e defaults/migração explícita para campos novos. Em C#, abrir arquivos Godot por `FileAccess` e liberar o recurso corretamente.

#### `DataLoader.cs`

**Papel:** ler arquivos JSON, desserializar tipos e validar referências/IDs.

**Métodos:**

- `LoadDay(int dayNumber)`
- `LoadProtocol(int dayNumber)`
- `LoadPatients(int dayNumber)`
- `LoadDecisionMatrix(int dayNumber)`
- `LoadDialogues(int dayNumber)`
- `ValidateContent(ContentBundle bundle)`

O carregamento do dia deve falhar de maneira explícita e útil quando falta arquivo, há ID duplicado, campo obrigatório ausente ou ação sem regra resolvida.

#### `CampaignManager.cs` / `EndingEvaluator.cs` — infraestrutura para a campanha

**Papel:** preparar o estado da campanha e avaliar o final após o Dia 7; não precisam executar finais completos na alpha, mas os dados/IDs devem ser compatíveis com a arquitetura.

**Métodos sugeridos:**

- `CampaignManager.InitializeCampaign()`
- `CampaignManager.CompleteDay(int dayNumber)`
- `EndingEvaluator.EvaluateEnding(CampaignState state)`
- `EndingEvaluator.BuildEpilogue(CampaignState state, string endingId)`

Os quatro finais continuam `[PROPOSTA]` conforme o briefing original. O sistema deve permitir que flags de Lucas e do cachorro alterem o epílogo sem o dinheiro determinar o final.

## 6. Dados externos e contratos de conteúdo

### 6.1 Estrutura de arquivos

Sugestão de arquivos para o conteúdo, regras e consequências:

```text
res://content/
├── campaign.json
├── protocols/
│   └── day_1_protocol.json
├── patients/
│   ├── day_1_patients.json
│   ├── day_1_narrative_patients.json
│   └── routine_patient_rules.json
├── decisions/
│   └── day_1_decision_matrix.json
├── dialogues/
│   └── day_1_dialogue.json
├── events/
│   └── dog_event.json
├── summaries/
│   └── day_summaries.json
└── endings/
    └── endings.json
```

Nomes são uma proposta de organização técnica; IDs de conteúdo devem ser estáveis. `day_1_patients.json` pode conter pools gerais e `day_1_narrative_patients.json` os cinco casos escritos à mão. Evitar duplicar o mesmo registro em dois arquivos sem necessidade.

### 6.2 Tipos C# recomendados

- `DayConfig`: número do dia, tempo-base, ordem dos pacientes de história, protocolo e IDs de conteúdo.
- `PatientData`: ID, nome, idade, traços visuais, fatos visíveis, documento, sintomas, diálogos e flags.
- `DocumentData`: tipo, nome impresso, identificador visível, validade/estado e detalhes exibíveis.
- `ProtocolData`: ID, título, regras e mensagens exibidas.
- `ProtocolRule`: ID, condição declarativa, prioridade se aprovada, grupo e decisão de referência.
- `TriageAction`: ID estável de ação e metadados de interface.
- `DecisionCase`: paciente, condições avaliadas e resultado esperado por ação.
- `ActionConsequence`: classificação, comissões/multas/custos, mudanças de métricas, flags, diálogo de saída, destino e log.
- `DialogueNode`: ID, interlocutor, texto, próxima linha, opções e condições.
- `TriageResult`: paciente, ação, classificação, motivos, efeitos de domínio calculados e destino.
- `DaySummary`: estatísticas, contas, métricas depois do fechamento e próximo evento.
- `GameSaveData`: schema version e snapshot de estado persistente.

### 6.3 Convenções dos dados

- Usar IDs sem acentos e estáveis: `marcio_souza`, `camila_torres`, `lucas_mendes`, `helena_rios`, `roberto_paiva`.
- Usar enums/constantes tipadas na lógica C# para ações e classificações; evitar comparar rótulos de tela.
- Manter os textos exibidos em conteúdo externo.
- As condições declarativas permitidas devem ser limitadas a comparadores simples de campos tipados, por exemplo `document.status == "expired"`; não executar código vindo do JSON.
- Validar referências cruzadas entre pacientes, diálogos, regras, destinos e flags no carregamento.
- Dados faltantes não devem gerar consequências financeiras padrão silenciosas.

### 6.4 Exemplo de protocolo: `day_1_protocol.json`

Exemplo estrutural ilustrativo. O formato final pode ser refinado sem alterar as regras de design:

```json
{
  "id": "day_1",
  "title": "Protocolo de Triagem - Dia 1",
  "base_duration_seconds": 300,
  "commission_amount": 5,
  "error_fine_amount": 5,
  "daily_living_expense": 25,
  "rules": [
    {
      "id": "document_validity",
      "group": "document",
      "text": "Deve conter Nome + RG/CPF visível e dentro da validade.",
      "condition": "document.name_visible == true && document.id_visible == true && document.valid == true"
    },
    {
      "id": "acute_symptoms",
      "group": "immediate_care",
      "text": "Febre acima de 38°C; dores intensas/agudas no abdômen, peito ou cabeça; ferimentos com sangramento ativo."
    },
    {
      "id": "no_immediate_care",
      "group": "refer_or_refuse",
      "text": "Pedido de atestado sem sintoma agudo; tontura leve; cansaço; sintomas com mais de 3 dias; receita vencida ou sem assinatura."
    }
  ]
}
```

O exemplo serve como esquema, não autoriza a implementação de uma prioridade entre regras conflitantes. Essa precedência continua `[EM ABERTO]`.

## 7. Variáveis centrais e flags

### 7.1 Estado de campanha/turno

Concentrar a versão autoritativa das variáveis em `GameState.cs`.

| Nome | Tipo sugerido | Descrição |
|---|---|---|
| `current_day` | `int` | Dia atual da campanha (1–7). |
| `day_phase` | enum/string ID | Menu, handover, shift, summary, between-shifts, completed. |
| `time_remaining_seconds` | `float` | Tempo-base restante; não representa sozinho a condição de encerrar o turno. |
| `turn_extended` | `bool` | Marca se o tempo-base acabou antes de todos os pacientes narrativos passarem. |
| `money` | `decimal`/`double` | Saldo disponível conforme o modelo financeiro final. A representação exata em relação à dívida ainda depende da pendência da seção 17. |
| `debt` | `decimal`/`double` | Dívida acumulada, se o modelo final a separar do saldo. |
| `commission_gross_today` | `decimal`/`double` | Comissão bruta reconhecida no dia. |
| `fines_today` | `decimal`/`double` | Multas totais do dia. |
| `ethical_costs_today` | `decimal`/`double` | Custos monetários de escolhas éticas. |
| `daily_expenses` | `decimal`/`double` | Despesa fixa de $25 e eventuais despesas narrativas recorrentes. |
| `reputation` | `int`/`float` | Métrica administrativa da campanha; começa neutra. |
| `conscience` | `int`/`float` | Métrica de consciência; começa neutra. |
| `errors_today` | `int` | Número de pacientes com erro genérico; um paciente com vários motivos conta como um erro para a multa e para o bônus diário. |
| `patients_processed_today` | `int` | Número de pacientes que receberam uma decisão final. A contagem de “atendidos” no resumo precisa seguir definição aprovada. |
| `patients_correct_today` | `int` | Quantidade de decisões classificadas como corretas/permitidas conforme matriz. |
| `patient_flags` | mapa/dicionário tipado | Marcas por paciente quando exigidas. |
| `narrative_flags` | mapa/dicionário tipado | Flags de continuidade/epílogo. |
| `dog_adopted` | `bool` | Se o jogador levou o filhote para casa. |
| `dog_food_cost` | valor monetário | Despesa recorrente do cão. Valor final entre $2 e $5 está `[EM ABERTO]`; o `dia-1.md` usa $2/dia. |
| `overtime_accepted` | `bool` | Se o jogador aceitou o turno dobrado no Dia 5. |
| `fatigue_level` | `int`/`float` | Intensidade/estado do efeito visual de cansaço; o sistema completo é futuro e o efeito afeta leitura apenas. |
| `routine_rng_seed` | `int`/`long` | Seed para reproduzir a sequência de pacientes de rotina. `[REQUISITO TÉCNICO]` |
| `campaign_state` | DTO/enum | Estado resumido da campanha e flags persistentes. |
| `save_schema_version` | `int` | Versão do formato do save. |

### 7.2 Flags narrativas conhecidas

| Flag | Quando é gravada | Estado |
|---|---|---|
| `lucas_visita_1` | Após processar a primeira visita do Lucas no Dia 1 | Presente na matriz do Dia 1. |
| `helena_outcome` = `refused` | Helena segue o protocolo: recusar ou recomendar reconsulta | Definido pela matriz atual. |
| `helena_outcome` = `helped` | Helena recebe a liberação excepcional | Definido pela matriz atual. |
| `dog_adopted` | Jogador leva o filhote para casa | Decisão entre turnos. |
| `overtime_accepted` | Oferta do Dia 5 aceita | Campanha futura. |
| `lucas_final_choice` | Decisão do jogador sobre o garoto no Dia 7 | Campanha futura; regras finais herdadas do briefing. |
| `elder_followup_state` | Arco do idoso e consequência de uma decisão anterior | Campanha futura; dia de retorno ainda contém proposta no briefing. |

### 7.3 Alteração de estado

Nenhuma cena deve alterar diretamente `money`, `reputation`, `conscience`, `errors_today` ou flags persistentes. A sequência é:

1. Controlador recebe intenção do jogador.
2. Domínio valida e calcula `TriageResult`.
3. `GameState.ApplyTriageResult(...)` registra os efeitos aprovados.
4. A UI recebe o resultado e apresenta feedback.
5. `PatientManager` move o paciente e avança.

Isso reduz divergências entre UI, resumo e save/load.

## 8. Sistema de decisões e consequências — Dia 1

### 8.1 Classificações internas

Use classificações tipadas, distintas do texto do botão:

- `CORRECT`: ação requerida pelo protocolo.
- `PERMITTED`: alternativa expressamente permitida pela matriz.
- `ERROR`: ação que viola o protocolo e recebe multa genérica.
- `ETHICAL_EXCEPTION`: escolha de dilema com consequência própria, sem multa genérica adicional no caso da Helena.
- `UNRESOLVED`: mapeamento necessário ainda não foi decidido. Não aplicar penalidades ou comissões por omissão.

### 8.2 Regras financeiras e de métrica aprovadas nesta conversa

- `[APROVADO]` Um erro de triagem perde a comissão do paciente e gera **uma única multa de $5 por paciente**, mesmo se o log contiver mais de um motivo de erro.
- `[APROVADO]` O log pode incluir vários IDs de regras violadas, por exemplo documento vencido e sintoma sem direito a atendimento imediato.
- `[APROVADO]` Helena ajudada: recebe comissão de $5 e tem custo de $10, líquido −$5; não recebe multa genérica adicional.
- `[APROVADO]` A escolha ética da Helena não conta como erro genérico para o bônus diário.
- `[APROVADO]` Reputação +1 ao concluir o dia sem erros genéricos.
- `[APROVADO]` FILA é tratada como decisão final e tem destino visual próprio; isso não resolve a semântica global de FILA versus ENCAMINHAR.
- Comissão de $5 por decisão correta aparece na matriz do Dia 1 enviada pelo usuário. O briefing geral original diz “comissão por paciente atendido”; essa diferença está explicitamente `[EM ABERTO]` na seção 17.

### 8.3 Matriz resumida

O texto abaixo consolida o rascunho `dia-1.md` e a revisão da matriz fornecida na conversa. A equivalência entre o rótulo provisório RECOMENDAR/FILA e os controles separados FILA/ENCAMINHAR permanece pendente.

#### Paciente 1 — Márcio Souza

- **ID:** `marcio_souza`
- **Dados visíveis:** homem, 40 anos; queda de escada; dor aguda no joelho direito; inchaço visível; RG válido, emitido em 2022.
- **Informação/interação:** conferência do documento e dos sintomas.
- **Regra:** `acute_symptoms`; documento regular.
- **Ação correta:** ATENDER.
- **Alternativas sem erro:** nenhuma.
- **Consequências:** ATENDER +$5; sem impacto direto em reputação ou consciência. Outras ações: sem comissão e multa única −$5.
- **Fala correta:** “Obrigado, doutor.”
- **Destinos:** atendido → hospital; fila → cadeiras de espera; recusa → saída.
- **Log de erro:** “Dor aguda deve ser atendida na hora (regra 2)”.
- **Marca de epílogo:** ainda não definida (`[EM ABERTO]`).
- **[PROPOSTA]** Se errar, mostrar seta/tutorial apontando a regra 2 do folheto.

#### Paciente 2 — Camila Torres

- **ID:** `camila_torres`
- **Dados visíveis:** mulher, 28 anos; tontura leve desde ontem; RG vencido há quatro anos; olhando o celular e mascando chiclete.
- **Diálogo-chave:** precisa ver um médico; quando a validade é apontada, diz que seria apenas para uma receita de vitamina.
- **Regras:** `document_validity` e `no_immediate_care`.
- **Ação correta na matriz atual:** RECUSAR.
- **Alternativa sem erro na matriz atual:** RECOMENDAR/FILA; encaminhamento para renovação é descrito como alternativa no `dia-1.md`.
- **Consequências:** decisão sem erro +$5; ATENDER é erro, sem comissão e multa de $5, com dois motivos registrados no log.
- **Fala de saída correta:** “Que burocracia ridícula. Vou no posto do bairro.”
- **Destino:** saída em recusa/encaminhamento; hospital caso seja atendida incorretamente.
- **Log de erro:** “RG vencido (regra 1) e sintoma sem direito a atendimento imediato (regra 3)”.
- **Marca de epílogo:** ainda não definida (`[EM ABERTO]`).
- **[PROPOSTA]** Fala alternativa específica caso o jogador recomende renovação.

#### Paciente 3 — Lucas Mendes

- **ID:** `lucas_mendes`
- **Dados visíveis:** menino, 11 anos; mochila escolar; RG válido; ficha declara “febre alta repentina e dor no corpo todo”; solicita atestado escolar.
- **Interação:** ao consultar o termômetro, o jogador revela 36,6 °C.
- **Regra:** `no_immediate_care` — pedido de atestado sem sintoma agudo; a leitura normal contradiz a febre relatada.
- **Ação correta na matriz atual:** RECUSAR.
- **Alternativa sem erro na matriz atual:** RECOMENDAR/FILA, conforme a matriz em revisão.
- **Consequências:** decisão sem erro +$5; ATENDER é erro, sem comissão e multa de $5.
- **Fala correta:** “Poxa... valeu a tentativa. Volto depois de amanhã.” (sussurrando).
- **Destino:** saída em recusa/recomendação; hospital se atendido incorretamente.
- **Log de erro:** “Atestado sem sintoma agudo e temperatura normal (regra 3)”.
- **Marca:** `lucas_visita_1`.
- **Continuidade:** na matriz mais recente, Lucas retorna nos Dias 3, 5 e 7 independentemente da decisão do Dia 1, pois o motivo real — bullying — persiste. A distribuição de detalhes das visitas continua dependente do conteúdo futuro.
- **[PROPOSTA]** Fala alternativa para encaminhamento à enfermaria escolar. Essa fala e esse destino não são decisão fechada.

#### Paciente 4 — Helena Rios

- **ID:** `helena_rios`
- **Dados visíveis:** mulher, 32 anos; sacola de mercado; RG válido; precisa retirar medicação contínua para asma do filho; receita vencida há dois dias.
- **Revelação narrativa:** é mãe solteira, trabalha em dois empregos e levou três semanas para conseguir folga; a bombinha do filho acabou.
- **Regra:** receita vencida (`no_immediate_care`).
- **Seguir protocolo:** RECUSAR ou recomendar reconsulta.
- **Exceção ética:** ATENDER/LIBERAR. É uma exceção narrativa reconhecida, não um `ERROR` genérico para fins de multa/bônus diário.
- **RECUSAR:** +$5; reputação +1; consciência +0; fala: “Compreendo... regras são regras. Vou ver se consigo emprestado com a vizinha...”; marca `helena_outcome=refused`.
- **RECOMENDAR reconsulta:** +$5; reputação +1; consciência +0; fala alternativa `[PROPOSTA]`: “Marcar consulta... vou tentar. Se eu conseguir folga de novo...”; marca `helena_outcome=refused`.
- **ATENDER/LIBERAR:** comissão +$5, custo ético −$10, líquido −$5; reputação −1 pela auditoria; consciência +1; fala: “Muito obrigada, moço... você não imagina o alívio que isso me dá.”; marca `helena_outcome=helped`.
- **Log de auditoria:** “Receita vencida liberada”, apenas quando ajudada.
- **Destinos:** farmácia do hospital se ajudada; saída em recusa/reconsulta.
- **Retorno:** Helena não volta como paciente, mas aparece no epílogo.

#### Paciente 5 — Roberto Paiva

- **ID:** `roberto_paiva`
- **Dados visíveis:** homem, 50 anos; camisa amassada; cheiro de cigarro; dor de dente há duas semanas; CNH válida.
- **Regra:** sintoma com mais de três dias e setor inadequado para odontologia.
- **Ação correta/alternativas no rascunho:** ENCAMINHAR ou RECUSAR; na matriz revisada, RECUSAR é registrada como ação correta e RECOMENDAR/FILA como alternativa sem erro.
- **Consequências:** decisão sem erro +$5; ATENDER é erro, sem comissão e multa de $5.
- **Fala correta:** “Perdi meu tempo aqui...”
- **Destino:** saída quando recusado/encaminhado; hospital se atendido incorretamente.
- **Log de erro:** “Triagem em setor errado: dor crônica vai para a Odontologia”.
- **Marca de epílogo:** ainda não definida (`[EM ABERTO]`).
- **[PROPOSTA]** Fala alternativa do encaminhamento: “Posto Central... e como eu chego lá?”.

### 8.4 Matriz técnica em formato estruturado (esboço para JSON)

A estrutura abaixo é intencionalmente um **esboço de migração**. Não é um JSON de produção pronto para ser carregado: as decisões dependentes da semântica de ação são marcadas como `UNRESOLVED` e não devem ser convertidas silenciosamente em FILA ou ENCAMINHAR.

```json
{
  "day_id": "day_1",
  "schema_version": 1,
  "financial_rules": {
    "commission_per_correct_decision": 5,
    "error_fine_per_patient": 5,
    "daily_living_expense": 25,
    "ethical_exception_cost": 10,
    "commission_policy_conflict": "OPEN"
  },
  "day_completion": {
    "reputation_bonus_if_no_generic_errors": 1,
    "ethical_exception_counts_as_generic_error": false,
    "slow_shift_reputation_penalty": "PROPOSAL"
  },
  "action_model": {
    "ui_actions_requested": ["ATTEND", "QUEUE", "REFER", "REFUSE"],
    "legacy_day_1_label": "RECOMMEND_QUEUE",
    "queue_vs_refer_semantics": "UNRESOLVED"
  },
  "patients": [
    {
      "id": "marcio_souza",
      "expected_action": "ATTEND",
      "actions": {
        "ATTEND": {"classification": "CORRECT", "commission": 5},
        "QUEUE": {"classification": "ERROR", "commission": 0, "fine": 5},
        "REFER": {"classification": "ERROR", "commission": 0, "fine": 5},
        "REFUSE": {"classification": "ERROR", "commission": 0, "fine": 5}
      },
      "exit_dialogue_id": "marcio_thanks",
      "audit_log_ids": [],
      "narrative_flags": []
    },
    {
      "id": "camila_torres",
      "expected_action": "REFUSE",
      "legacy_permitted_action": "RECOMMEND_QUEUE",
      "actions": {
        "ATTEND": {"classification": "ERROR", "commission": 0, "fine": 5, "audit_log_ids": ["expired_id", "no_immediate_care"]},
        "QUEUE": {"classification": "UNRESOLVED", "reason": "Legacy RECOMMEND_QUEUE label does not specify whether queue is valid"},
        "REFER": {"classification": "PERMITTED", "commission": 5, "reason": "Forward for identity renewal; destination details remain content-defined"},
        "REFUSE": {"classification": "CORRECT", "commission": 5}
      },
      "exit_dialogue_id": "camila_refusal",
      "narrative_flags": []
    },
    {
      "id": "lucas_mendes",
      "expected_action": "REFUSE",
      "legacy_permitted_action": "RECOMMEND_QUEUE",
      "revealed_facts": ["temperature_celsius=36.6"],
      "actions": {
        "ATTEND": {"classification": "ERROR", "commission": 0, "fine": 5, "audit_log_ids": ["false_fever_attestation"]},
        "QUEUE": {"classification": "UNRESOLVED", "reason": "Legacy RECOMMEND_QUEUE label does not specify whether queue is valid"},
        "REFER": {"classification": "PERMITTED", "commission": 5, "reason": "Referral is permitted by the current matrix; the destination still needs definition"},
        "REFUSE": {"classification": "CORRECT", "commission": 5}
      },
      "exit_dialogue_id": "lucas_refusal",
      "narrative_flags": ["lucas_visita_1"]
    },
    {
      "id": "helena_rios",
      "expected_action": "REFUSE_OR_REFER_RECONSULTATION",
      "ethical_exception_action": "ATTEND_RELEASE",
      "actions": {
        "ATTEND_RELEASE": {"classification": "ETHICAL_EXCEPTION", "commission": 5, "ethical_cost": 10, "generic_error_fine": 0, "reputation_delta": -1, "conscience_delta": 1, "audit_log_ids": ["expired_prescription_released"], "flag": "helena_outcome=helped"},
        "REFUSE": {"classification": "CORRECT", "commission": 5, "reputation_delta": 1, "conscience_delta": 0, "flag": "helena_outcome=refused"},
        "REFER_RECONSULTATION": {"classification": "PERMITTED", "commission": 5, "reputation_delta": 1, "conscience_delta": 0, "flag": "helena_outcome=refused"}
      }
    },
    {
      "id": "roberto_paiva",
      "expected_action": "REFUSE",
      "legacy_permitted_action": "RECOMMEND_QUEUE_OR_REFER",
      "actions": {
        "ATTEND": {"classification": "ERROR", "commission": 0, "fine": 5, "audit_log_ids": ["chronic_dental_pain_wrong_department"]},
        "QUEUE": {"classification": "UNRESOLVED", "reason": "Queue is not equivalent to the explicit dental referral in the current content"},
        "REFER": {"classification": "PERMITTED", "commission": 5, "reason": "Referral to the Central Post dental service is permitted"},
        "REFUSE": {"classification": "CORRECT", "commission": 5}
      }
    }
  ]
}
```

**Nota:** alguns aliases presentes nesse esboço (por exemplo `ATTEND_RELEASE` e `REFER_RECONSULTATION`) são intenções específicas de conteúdo, não os quatro botões globais definitivos. A lista final de `actionId` precisa ser normalizada quando a decisão pendente for fechada.

## 9. Regras de economia e resumo do plantão

### 9.1 Valores conhecidos

| Item | Valor/regra | Estado |
|---|---|---|
| Comissão do Dia 1 | $5 | Valor fechado; política de quando exatamente ganha está em divergência entre os arquivos. |
| Multa por erro | $5, sem comissão do paciente | Fechado; multa única por paciente com erro aprovada nesta conversa. |
| Custo de vida diário | $25 | Fechado no briefing e no folheto. |
| Escolha ética de Helena | $10 | Fechado na matriz da conversa; comissão +$5 e custo −$10; sem multa genérica. |
| Saldo negativo/dívida | Acompanha de um dia para outro | Regra geral fechada; representação exata entre `money` e `debt` precisa ser formalizada. |
| Exibir dívida final no epílogo | `[PROPOSTA]` | Não tratar como requisito até aprovar. |
| Ração do cachorro | $2/dia em `dia-1.md`; briefing geral menciona $2 ou $5 | `[EM ABERTO]` por divergência. |
| Móvel destruído após hora extra com cão adotado | −$12 | Definido no briefing para o Dia 5, condicionado a aceitar hora extra e ter adotado o cão. |
| Hora extra do Dia 5 | +$20 | Definido no briefing. |

### 9.2 Resumo obrigatório

O resumo deve exibir pelo menos:

- Pacientes processados e número de decisões corretas, permitidas e erros.
- Comissão bruta reconhecida.
- Número de pacientes com erro e total de multas (uma multa por paciente com erro).
- Quantidade e custo das escolhas éticas.
- Se o turno foi estendido.
- Despesas fixas diárias e despesas narrativas aplicáveis.
- Saldo do dia e saldo acumulado/dívida.
- Reputação e consciência após as alterações do dia.
- Evento entre turnos disponível.

Os números “cerca de 15 pacientes”, “$75 de comissão” e “saldo de +$50 / +$40” em `dia-1.md` são exemplos de um plantão típico, não metas rígidas ou resultados fixos.

### 9.3 Regra de lançamento financeiro

Cada decisão produz um `TriageResult` com lançamentos separados para comissão, multa e custo ético. O resumo agrega os lançamentos registrados; não refaz a lógica paciente a paciente a partir do texto da interface. Isso evita que uma escolha ética receba a multa genérica duas vezes.

## 10. Sistemas e mecânicas interativas

### 10.1 Inspeção de documento

- Exibir nome, identificador e validade.
- Distinguir visualmente documento válido, vencido ou incompleto.
- O estado de validade vem do dado do paciente, não de interpretação livre do texto pela UI.
- O Dia 1 exige Nome + RG/CPF visível e dentro da validade.
- `[EM ABERTO]` Se CNH substitui RG/CPF para a regra geral e como documentos sem data explícita de validade são representados. Roberto tem CNH válida conforme o caso escrito.

### 10.2 Inspeção de sintomas

- Exibir sintoma declarado na ficha e no diálogo.
- O protocolo do Dia 1 aceita para atendimento imediato: febre acima de 38 °C; dor intensa/aguda no abdômen, peito ou cabeça; ferimento com sangramento ativo.
- O protocolo do Dia 1 não dá atendimento imediato a: pedido de atestado sem sintoma agudo; tontura leve; cansaço; sintomas com mais de três dias; receita vencida ou sem assinatura.
- A regra de temperatura de Lucas é informação ficcional de gameplay, não simulador ou conselho médico.
- `[EM ABERTO]` Precedência entre sintoma urgente e documento inválido, além de eventuais empates de regra.

### 10.3 Carimbos/ações

- A alpha deve desenhar os quatro botões de ação pedidos: ATENDER, FILA, ENCAMINHAR e RECUSAR.
- O rascunho de Dia 1 ainda apresenta o carimbo composto `RECOMENDAR/FILA`; a semântica independente entre FILA e ENCAMINHAR está `[EM ABERTO]`.
- Feedback visual proposto: carimbo aplicado sobre a ficha, breve destaque do resultado, som correspondente e atualização das variáveis.
- Não usar cores como único meio de comunicação. O rascunho associa ATENDER a vermelho, RECOMENDAR/FILA a verde e RECUSAR a azul; a paleta do quarto botão e os rótulos finais permanecem pendentes. Cor deve vir acompanhada de texto/ícone.

### 10.4 Gerador de rotina

- Carregar pools de nome, idade, sintoma, documento e traço de personalidade.
- Combinar dados e calcular a ação esperada antes de inserir o paciente na fila.
- Não gerar combinações inválidas sem uma regra resolvida.
- Registrar seed para testes reprodutíveis.
- `[PROPOSTA]` Meta inicial aproximada de 60% de casos para ATENDER e 40% para recusar/encaminhar.

### 10.5 Relógio e continuidade

- O relógio conta o tempo real de jogo equivalente a cinco minutos de plantão.
- Ao zerar, o gerador não cria pacientes de rotina novos.
- Os pacientes narrativos restantes continuam chegando até serem processados.
- O turno só se encerra depois que os cinco pacientes narrativos do Dia 1 passaram.
- `[PROPOSTA]` Se o turno foi prolongado, registrar a condição no resumo e aplicar −1 reputação.

### 10.6 Evento do cachorro

Cena narrativa curta no caminho para casa, sob chuva fina. Um filhote está encolhido numa caixa de papelão molhada.

Texto original a preservar: “No caminho para casa, perto do ponto de ônibus, você escuta um ganido fraco. Um filhote de cachorro está encolhido sob a chuva, tremendo de frio.”

- **Levar o filhote:** liga `dog_adopted`; gera despesa recorrente a partir do Dia 2; habilita a consequência de ansiedade em caso de hora extra e a linha do mascote no epílogo, esta última `[PROPOSTA]` no briefing geral.
- **Continuar caminhando:** sem despesa adicional; o cão não volta a aparecer.
- `[EM ABERTO]` O custo recorrente final é $2 ou $5 por dia, pois os dois documentos não são consistentes.

### 10.7 Hora extra e cansaço (campanha futura)

- A hora extra aparece apenas no Dia 5.
- Aceitar: +$20; consome o tempo livre.
- Se o cão tiver sido adotado, a ansiedade culmina na destruição de um móvel: −$12.
- No plantão seguinte há um efeito visual de cansaço: a tela começa ligeiramente esmaecida e o efeito aumenta ao longo do turno; afeta a leitura, sem penalidade mecânica adicional definida.
- Recusar: a rotina segue normalmente.
- `[PROPOSTA]` O Dia 6 como dia de consequências sem dilema novo aparece como sugestão em uma parte do briefing, embora outra passagem chame “Dia 6 sem dilema novo” de decidido; tratar esta datação como pendente de reconciliação.
- `[PROPOSTA]` Oferecer opção de acessibilidade para reduzir a intensidade do efeito visual.

A hora extra não faz parte do loop jogável do Dia 1; nesta alpha, os dados e flags podem ser preparados, mas não há necessidade de simular os Dias 5–6.

## 11. UI, UX, transições e áudio

### 11.1 Tela inicial

- Título do jogo e botões Novo Jogo, Continuar (se houver save válido) e Sair.
- Erros de leitura de save precisam gerar mensagem útil, com opção de iniciar nova campanha; nunca bloquear permanentemente a entrada no jogo por save inválido.

### 11.2 Guichê

Áreas propostas de layout:

1. Cabeçalho: dia, fase, relógio e saldo visível se decidido que o saldo fica sempre à vista.
2. Área do paciente: retrato, nome, idade, balão de fala e fala em andamento.
3. Área de documentos: ficha/documento ampliável e interações.
4. Área de protocolo: resumo das regras do dia, acessível durante o turno.
5. Área de decisão: os quatro botões solicitados, com estado desabilitado quando a cena estiver em diálogo ou processando consequência.
6. Área de feedback: mensagem curta de decisão correta/erro, motivos e resultado financeiro.
7. Área de espera/destino: cadeiras de espera e marcadores visuais dos destinos dos pacientes.

A geometria definitiva e a escala pixel-art podem ser ajustadas durante a montagem de cena. Nenhuma ação importante pode depender exclusivamente de hover ou cor.

### 11.3 Transições

- Passagem de turno → guichê aberto.
- Paciente atual → decisão → feedback curto → destino do paciente → próximo paciente.
- Timer-base expirado → parar geração de rotina; continuar só com narrativa pendente.
- Último paciente narrativo concluído + timer-base expirado → resumo do plantão.
- Resumo → evento narrativo do cachorro → checkpoint do Dia 1.

### 11.4 Feedback visual e sonoro mínimo

- Papel/ficha sendo colocada na mesa.
- Inspeção/manuseio de documento.
- Clique de UI.
- Quatro carimbos ou feedback visual equivalente.
- Chegada e saída de paciente.
- Erro de triagem.
- Decisão correta.
- Ambiente hospitalar discreto.
- Indicação sutil de pressão temporal.

Áudios são camadas de feedback, não requisitos para calcular uma decisão. Se o áudio não estiver disponível, o jogo continua totalmente funcional.

## 12. NPCs, diálogos e integração de conteúdo

### 12.1 Ralel

- 27 anos, bem-humorado, personagem secundário sem arco/desfecho próprio.
- Faz triagem em outra cabine e pertence ao turno anterior.
- Aparece no início de cada dia para transferir o posto e informar as novas validações.
- Dia 1: três falas do rascunho devem ser preservadas no conteúdo de diálogo:
  - “Pronto. O posto é seu. O turno da noite foi um caos... duas brigas de bar e um idoso que engoliu a prótese dentária.”
  - “Deixei o folheto do protocolo do dia em cima da mesa. Lembre-se: confere o nome na ficha, o tipo de sintoma e se o documento tá no prazo. A diretoria tá de olho nas falhas.”
  - “Boa sorte. Vou tentar dormir seis horas seguidas se o barulho da obra deixar.”
- `[PROPOSTA]` Um aviso dele pode ser incompleto/errado em algum dia porque está cansado. Não implementar como surpresa no Dia 1 sem conteúdo explícito.

### 12.2 Garoto do atestado — Lucas Mendes

- Dia 1: 11 anos, RG válido, pede atestado e alega febre, mas a leitura do termômetro é 36,6 °C.
- A primeira tentativa é engraçada; as visitas posteriores revelam gradualmente o motivo real relacionado ao bullying.
- Na matriz revisada, retorna nos Dias 3, 5 e 7 mesmo que a decisão do Dia 1 varie.
- O briefing original define no Dia 7 duas escolhas: aceitar o afastamento e acionar o serviço social para chamar a família, ou recusar/não encaminhar para obter atestado; nesse segundo caminho, a família é chamada e fica subentendido que o garoto volta à escola com dores. O jogo não moraliza nem mostra a consequência da recusa diretamente.
- `[PROPOSTA]` Plantar pistas nos dias anteriores ligadas ao recreio, educação física e saída da escola.
- `[PROPOSTA]` Ouvir o garoto consome segundos do turno e reduz o número de pacientes de rotina/comissão.

### 12.3 Helena Rios

É o dilema do Dia 1. A medicação contínua para asma é para o filho; o documento médico está vencido há dois dias. O protocolo é seguido ao recusar ou recomendar reconsulta; liberá-la é a exceção ética com os efeitos consolidados na seção 8.3.

### 12.4 Outros dilemas da campanha — conteúdo de referência

Os casos abaixo são parte do briefing de campanha e devem ser preservados para implementação futura. Não fazem parte do conteúdo jogável obrigatório da alpha do Dia 1.

- **Operário no limite da exaustão:** trabalha na construção civil, 14 horas por dia, pede dois dias de afastamento e revela calos extremos/tremedeira por falta de sono. Protocolo veda atestado para sintomas leves sem laudo de especialista. Burlar gera atestado falso por “gastroenterite aguda”; a auditoria detecta e reputação cai.
- **Idoso e receita de remédio controlado:** receita de alto custo exige carimbo presencial do médico responsável; médico titular está de licença e a fila para nova consulta é de três semanas; o idoso só tem medicação para 24 horas. Ele chega sozinho de ônibus, com filha em outro estado. Burlar é liberar por conta própria. Se seguir o protocolo, há uma consequência posterior: reaparece em pior estado. `[PROPOSTA]` O retorno no Dia 6.
- **Jovem sem RG:** exames rápidos exigem identidade original com foto; a bolsa foi furtada, restou uma foto borrada do RG e carteirinha de estudante rasurada. Burlar é validar com esses documentos. A auditoria não detecta a decisão; ela só rende consciência, sem queda de reputação.
- **Pai e criança com febre:** pediatria sem vagas; febre abaixo de 39 °C é redirecionada a posto a 5 km. Pai chega com filha de 5 anos a 38,5 °C sem dinheiro para ônibus; o posto fechou o portão e a menina já teve delírio febril na rua. Burlar é registrar temperatura de 39,5 °C para forçar encaixe.
- `[PROPOSTA]` A distribuição dos dilemas listados é: Dia 1 Helena; Dia 2 jovem sem RG; Dia 3 operário; Dia 4 idoso; Dia 5 pai e criança; Dia 6 sem dilema novo; Dia 7 garoto do atestado. A inconsistência do status do Dia 6 está registrada na seção 10.7.

### 12.5 Tom dos diálogos

- Falas curtas no ciclo principal.
- Informações escondidas podem aparecer ao inspecionar ou conversar, conforme conteúdo do caso.
- O jogador não deve receber automaticamente toda a informação narrativa se o design do caso exige interação para descobri-la.
- O texto de saída deve variar por decisão apenas quando esse caminho tiver fala especificada ou aprovada; falas alternativas marcadas `[PROPOSTA]` podem ficar como texto provisório.

## 13. Save/checkpoint

### 13.1 Política de checkpoint

- `[FECHADO]` Não é necessário retomar no meio de um plantão.
- `[REQUISITO TÉCNICO]` Autosave após o resumo/finalização do dia.
- `[REQUISITO TÉCNICO]` Persistir a escolha do evento narrativo do cachorro depois da decisão.
- `[REQUISITO TÉCNICO]` Salvar em JSON com snapshot das variáveis centrais do `GameState.cs`.
- Save no meio de um turno não faz parte da alpha. Se uma ação narrativa puder ocorrer fora do resumo e tiver consequência irreversível no estado persistente, salvá-la após a confirmação é `[PROPOSTA TÉCNICA]` para proteger a continuidade; não implica permitir retomar no meio do turno.

### 13.2 Estrutura sugerida de save

```json
{
  "schema_version": 1,
  "campaign_id": "main_campaign",
  "current_day": 1,
  "day_phase": "between_shifts_complete",
  "money": 50,
  "debt": 0,
  "reputation": 1,
  "conscience": 0,
  "dog_adopted": false,
  "overtime_accepted": false,
  "fatigue_level": 0,
  "errors_today": 0,
  "patients_processed_today": 15,
  "narrative_flags": {
    "lucas_visita_1": true,
    "helena_outcome": "refused"
  },
  "routine_rng_seed": 12345
}
```

Os números são exemplos estruturais, não um resultado obrigatório do Dia 1. Valores reais devem ser obtidos de `GameState`. O par `money`/`debt` será formalizado quando a representação da dívida for resolvida.

### 13.3 Robustez

- Validar `schema_version` e campos essenciais antes de aplicar o save.
- Se o save não existir, iniciar nova campanha.
- Se o save estiver inválido, informar o problema e permitir recuperação sem travar o jogo.
- Não salvar referências a Nodes, cenas carregadas ou objetos visuais.
- Usar caminho `user://` para dados graváveis; os arquivos de conteúdo empacotados são somente leitura durante o jogo.

## 14. Arte e som

### 14.1 Arte mínima

- Guichê fixo em 2D.
- Retrato de cada paciente narrativo.
- Retratos combináveis ou conjunto reduzido de retratos para pacientes de rotina.
- Ficha, documento, folheto e carimbos como objetos de interface.
- Fundo simples do evento do cachorro sob chuva.
- Marcadores visuais dos destinos: hospital, cadeiras de espera, serviço externo/encaminhamento e saída.
- Pixel art própria desde o início, mas a alpha pode usar formas/arte temporária enquanto o loop e os dados são validados.

`[PROPOSTA]` Sem animações complexas: transições curtas e pequenas reações são suficientes para provar a experiência.

### 14.2 Áudio mínimo

- Papel sendo colocado/manuseado.
- Documento sendo inspecionado.
- Clique de interface.
- Carimbo de cada ação.
- Chegada/saída de paciente.
- Feedback de decisão correta e erro.
- Ambiente hospitalar discreto.
- Pressão temporal sutil.

Não é necessário voice acting. Evitar sons punitivos excessivamente agressivos; o feedback deve comunicar estado sem distrair a leitura do documento.

## 15. Critérios de aceitação da alpha

A alpha é considerada funcional quando os itens obrigatórios abaixo passam em testes manuais ou automatizados.

### 15.1 Inicialização e fluxo

- [ ] O jogador consegue iniciar uma campanha nova.
- [ ] A passagem de turno de Ralel e o protocolo do Dia 1 aparecem.
- [ ] O jogador consegue abrir/consultar o folheto do dia.
- [ ] A cena do guichê permanece jogável de ponta a ponta sem travar.
- [ ] Os cinco pacientes narrativos são apresentados na ordem relativa Márcio → Camila → Lucas → Helena → Roberto, intercalados conforme política de rotina ainda a definir.
- [ ] Pacientes de rotina continuam sendo gerados antes do fim do relógio-base.
- [ ] Ao expirar o tempo-base, a geração de pacientes de rotina para.
- [ ] Pacientes narrativos pendentes ainda entram mesmo depois do fim dos cinco minutos.
- [ ] O turno não termina até que os cinco pacientes narrativos tenham passado.
- [ ] Não existe caminho em que um diálogo/modal impeça permanentemente o avanço do turno.

### 15.2 Inspeção e regras

- [ ] A ficha exibe os dados definidos para cada paciente.
- [ ] O documento exibe o estado de validade correto.
- [ ] A interação do termômetro revela 36,6 °C para Lucas.
- [ ] O painel de protocolo mostra exatamente as regras carregadas de `day_1_protocol.json`.
- [ ] Márcio: ATENDER é correto; RECUSAR é erro; consequências financeiras aplicadas corretamente.
- [ ] Camila: RECUSAR gera decisão sem erro; ATENDER gera um erro e uma multa total de $5 com os dois motivos no log.
- [ ] Lucas: RECUSAR gera decisão sem erro; ATENDER gera um erro e multa única de $5; `lucas_visita_1` fica registrada.
- [ ] Helena: seguir o protocolo dá +$5, reputação +1 e consciência +0; ajudar dá comissão $5, custo $10, reputação −1, consciência +1, sem multa genérica e marca `helena_outcome=helped`.
- [ ] Roberto: ATENDER gera erro e multa única de $5; a recusa/encaminhamento segue o mapeamento que for formalmente aprovado antes da build de aceitação.
- [ ] Nenhuma ação com semântica ainda não decidida recebe uma classificação definitiva por padrão. Essa validação deve falhar ou sinalizar explicitamente `UNRESOLVED`.

### 15.3 Economia e resumo

- [ ] Decisão correta/permitida reconhece a comissão segundo a política que vier a ser formalizada.
- [ ] Cada paciente com erro gera no máximo uma multa de $5, mesmo com múltiplos motivos.
- [ ] A escolha ética da Helena é contabilizada como custo de $10 e comissão de $5; não recebe multa genérica adicional.
- [ ] O bônus de reputação +1 é aplicado se o dia terminar sem erros genéricos, incluindo o caso em que Helena foi ajudada.
- [ ] O resumo mostra pacientes processados, decisões, comissão, multas, custos éticos, despesas fixas, saldo e métricas.
- [ ] A despesa fixa de $25 é aplicada uma única vez ao final do dia.
- [ ] A conta financeira não duplica lançamentos entre resolução da ação e montagem do resumo.

### 15.4 Evento e save

- [ ] O evento do cachorro apresenta as duas escolhas.
- [ ] Adotar define `dog_adopted=true`; não adotar não gera custo recorrente.
- [ ] O checkpoint é gravado após o resumo e a decisão narrativa.
- [ ] O estado salvo pode ser carregado depois de fechar e reabrir o jogo.
- [ ] Um save ausente/corrompido não impede iniciar uma campanha nova.
- [ ] O estado de `lucas_visita_1`, desfecho da Helena e adoção do cachorro sobrevive ao save/load.

### 15.5 Bloqueios de aceitação

A implementação pode progredir com os componentes independentes, mas o aceite completo das decisões depende de resolver as pendências de ação e comissão listadas na seção 17. Não registrar uma build como validada enquanto casos relevantes ainda retornarem `UNRESOLVED`.

## 16. Entregáveis priorizados

### 16.1 Core — essencial para a alpha

1. Projeto Godot 4.6.3 .NET compilando em Windows.
2. `MainMenu.tscn`.
3. `TriageStation.tscn` e composição do guichê.
4. UI de paciente, ficha, documentos e protocolo.
5. `StampController.tscn` com os quatro slots de ação solicitados, mantendo o mapeamento não resolvido configurável.
6. Cinco pacientes narrativos do Dia 1.
7. Gerador de pacientes de rotina com seed reproduzível.
8. Relógio e regras de fim do turno.
9. `GameState.cs`, `DayManager.cs`, `PatientManager.cs`, `TriageController.cs`, `ProtocolManager.cs` e `EconomyManager.cs`.
10. `ReputationManager.cs`, `DialogueManager.cs`, `NarrativeFlagManager.cs`, `DataLoader.cs` e `SaveManager.cs`.
11. Arquivos externos de protocolo, pacientes, diálogos e matriz do Dia 1.
12. Log de auditoria com IDs/motivos de erro.
13. Resumo de fim do plantão.
14. Evento narrativo do cachorro.
15. Save/load básico pós-turno.
16. Testes unitários do motor de regras e cálculos financeiros.

### 16.2 Nice-to-have

- Variações adicionais de falas de rotina.
- Mais pools de nomes, idades, documentos e traços.
- Efeitos visuais e animações extras.
- Animações de fila.
- Mais camadas de áudio.
- Polimento de retratos e cenário.
- Ferramenta de QA/debug para exibir ID da regra aplicada e os lançamentos de um paciente (apenas desenvolvimento; não é editor de conteúdo para jogador).

### 16.3 Ordem técnica sugerida

1. **Fundação:** projeto, cenas vazias, Autoload, DTOs e carregamento/validação de JSON.
2. **Domínio puro:** protocolo, matriz de ação, consequências e testes sem UI.
3. **Loop de guichê:** exibição do paciente, inspeção e botões de ação.
4. **Fila e relógio:** ordem de narrativos, geração rotineira e fim do turno.
5. **Economia e resumo:** comissão, multas, despesas, saldo e métricas.
6. **Diálogo/evento:** Ralel, falas de saída e cachorro.
7. **Persistência:** salvar após conclusão e carregar checkpoint.
8. **Validação alpha:** passar pelos critérios da seção 15 e registrar itens ainda bloqueados.

## 17. Decisões fechadas, propostas e pendências

### 17.1 Fechado no briefing ou nesta conversa

- Experiência central de triador em um plantão hospitalar, com pressão de tempo, burocracia, humor e dilemas humanos.
- Loop: chegada → inspeção → decisão → consequência → próximo paciente.
- Campanha planejada de sete dias.
- Engine solicitada nesta tarefa: Godot 4.6.3 .NET; linguagem C#; primeiro alvo Windows; jogo offline sem backend.
- Turno de cinco minutos-base; duração prática aproximada de cinco a sete minutos.
- Parar geração de rotina quando acabar o tempo-base, mas processar todos os pacientes narrativos pendentes.
- Comissão base de $5, multa de $5 por erro e despesa fixa de $25 por dia.
- Saldo é acumulado entre dias e resultados negativos entram no sistema de dívida; representação interna exata ainda precisa ser formalizada.
- Reputação e consciência começam neutras; alta no final é acima do ponto neutro inicial.
- Dinheiro não decide diretamente o final; aparece como pressão e no epílogo.
- Um dilema de peso por dia no design da campanha como estrutura-base; conteúdo de cada dia não precisa estar implementado na alpha.
- Ralel aparece antes de cada turno e não tem arco/desfecho próprio.
- O garoto do atestado retorna nos Dias 3, 5 e 7 segundo a matriz recente; a revelação de bullying culmina no Dia 7.
- No Dia 7, a escolha sobre o garoto tem os dois caminhos descritos na seção 12.2; o jogo não moraliza a recusa nem mostra diretamente a consequência negativa.
- A hora extra está disponível no Dia 5; aceitar gera +$20; se o cão foi adotado, destrói um móvel com custo de $12; cansaço visual ocorre no plantão seguinte e afeta leitura apenas.
- As decisões entre turnos na versão atual são adotar/deixar o cachorro e aceitar/recusar hora extra.
- No Dia 1 existem cinco pacientes narrativos definidos e a matriz desta conversa fornece as ações/resultados de Márcio, Camila, Lucas, Helena e Roberto.
- Multa genérica única de $5 por paciente com erro; log pode registrar motivos múltiplos.
- Helena ajudada: comissão $5, custo ético $10, líquido −$5, sem multa genérica adicional, reputação −1, consciência +1.
- Helena seguindo o protocolo: reputação +1 e consciência +0; recusar e recomendar reconsulta guardam `helena_outcome=refused`; ajudar guarda `helena_outcome=helped`.
- FILA é uma decisão final e tem destino visual de cadeiras de espera.
- Bônus diário de reputação +1 se não houver erros genéricos; a exceção ética de Helena não elimina o bônus.
- `lucas_visita_1` é a marca inicial do arco do garoto.

### 17.2 Itens que continuam `[PROPOSTA]`

- Punição de −1 reputação se o turno estender além do tempo-base.
- Quatro finais específicos por combinação de reputação/consciência e seus títulos.
- 28 pacientes escritos à mão no total (quatro por dia), mais personagens fixos; o Dia 1 atual contém cinco pacientes narrativos, por isso o volume global requer revisão de contagem.
- Cerca de 15 pacientes/turno com aproximadamente dez de rotina.
- Proporção inicial 60% de decisões corretas para atender e 40% para recusar/encaminhar.
- Avisos incompletos ou incorretos de Ralel em algum dia.
- Pistas sutis nas visitas anteriores de Lucas.
- O diálogo com Lucas consumindo segundos do turno.
- A linha do cachorro no epílogo, embora adotar garanta a continuidade narrativa na matriz do Dia 1.
- Distribuição de dilemas pelos Dias 1–7 conforme seção 12.4, exceto o que estiver claramente fechado após revisão.
- Retorno do idoso no Dia 6 como consequência de seguir protocolo.
- Redução de intensidade do efeito de cansaço por opção de acessibilidade.
- Ausência de animação complexa.
- Exibir a dívida final no epílogo.
- Custos/efeitos de cansaço precisam ser equilibrados depois de testar.

### 17.3 Decisões em aberto que devem permanecer explícitas

**A. Modelo de ações / carimbos**

- O rascunho do Dia 1 tem três rótulos visuais: `ATENDER`, `RECOMENDAR/FILA`, `RECUSAR`.
- A mecânica central do briefing e a especificação técnica da alpha pedem quatro ações: `ATENDER`, `FILA`, `ENCAMINHAR`, `RECUSAR`.
- O usuário ainda não aprovou se o modelo final terá quatro ações independentes ou se RECOMENDAR/FILA será uma ação agrupada.
- O fato de FILA ser decisão final com destino próprio foi aprovado, mas **isso não fecha** a distinção global entre FILA e ENCAMINHAR.
- Até haver decisão, a interface pode ser construída com quatro slots, mas a matriz deve indicar `UNRESOLVED` onde a ação do rascunho não possa ser mapeada sem suposição.

**B. Política de comissão**

- O briefing geral diz “comissão por paciente atendido”.
- O folheto e a matriz do Dia 1 dizem “comissão por paciente correto/decisão correta”, inclusive para recusar ou encaminhar.
- O valor de $5 é consistente; o gatilho da comissão ainda precisa ser resolvido entre os documentos.

**C. Precedência do protocolo**

- O que ocorre se o sintoma pedir atendimento imediato, mas o documento estiver inválido?
- Como resolver outros conflitos entre regras? Não há política de prioridade fechada.

**D. Documentos aceitos**

- A regra cita RG/CPF visível e válido; Roberto tem CNH válida. Não está formalmente definido se a CNH substitui RG/CPF na validação geral.
- Como representar documento sem data explícita de vencimento também precisa de definição.

**E. Consequência genérica de reputação por erro**

- O briefing diz que reputação sobe com boas decisões e sem erros.
- A matriz define que um erro elimina o bônus diário de +1, mas não define uma penalidade imediata geral de reputação por todo erro. Não adicionar essa penalidade sem decisão.

**F. Dinheiro e dívida**

- A dívida acumula se o saldo diário é negativo e afeta dias seguintes; o formato matemático interno de `money`/`debt` precisa ser definido para não contar a mesma falta duas vezes.
- `[PROPOSTA]` Mostrar a dívida final no epílogo.

**G. Custo do cachorro**

- O briefing geral cita ração de $2 ou $5/dia; `dia-1.md` define $2/dia a partir do Dia 2. O valor final está pendente.

**H. Atendimento lento / fim do turno**

- −1 reputação por turno estendido continua `[PROPOSTA]`.
- O comportamento do relógio durante diálogo e inspeção está por definir.
- A intercalação exata da fila de rotina não foi especificada.

**I. Contagem de conteúdo narrativo**

- O briefing geral propõe 28 pacientes escritos à mão (quatro por dia), mas `dia-1.md` contém cinco casos narrativos no Dia 1. Preservar os cinco no protótipo e rever a contagem de campanha antes de fechar o planejamento dos sete dias.

**J. Dia 6 e consequência da hora extra**

- O briefing chama o Dia 6 de dia sem dilema novo em uma passagem, mas a distribuição por dias está marcada `[PROPOSTA]`. A datação final deve ser reconciliada.

**K. Marcas de epílogo de Márcio, Camila e Roberto**

- Na matriz atual, os IDs/efeitos das marcas desses três casos não foram definidos. Não inventar flags só para preencher o esquema.

**L. Destino e falas em ações alternativas**

- As falas alternativas de Márcio, Camila, Lucas e Roberto assinaladas como `[PROPOSTA]` ainda não são definitivas.
- Destinos que dependem de uma distinção entre FILA e ENCAMINHAR devem permanecer pendentes.

**M. Escala de reputação/consciência e finais**

- O ponto neutro inicial e a regra “alta = acima do neutro no Dia 7” estão fechados.
- Os quatro finais e os limiares adicionais além dessa regra são `[PROPOSTA]`/conteúdo ainda a revisar. Não fixar faixas numéricas adicionais sem teste/decisão.

### 17.4 Política de resolução de pendências

- Não substituir `[EM ABERTO]` por um valor default apenas para o JSON carregar.
- No conteúdo, representar pendências com status explícito; o validador pode permitir execução de trechos independentes, mas deve impedir que decisões não resolvidas sejam apresentadas como comportamentos finais testados.
- Manter um `docs/decisions.md` com ID da decisão, pergunta, opções discutidas, responsável, estado e data de resolução.
- Ao fechar uma pendência, atualizar este documento, a matriz estruturada, o conteúdo JSON e seus testes na mesma mudança.

## 18. Dicas técnicas para Godot 4.6.3 + C#

1. Instalar e usar a **edição .NET** do editor Godot na mesma versão-alvo do projeto; um editor padrão sem suporte .NET não compila scripts C#.
2. Usar uma IDE externa (por exemplo, Visual Studio ou VS Code) para navegação, compilação e depuração C#.
3. Manter a lógica de domínio em classes C# separadas da UI. Os scripts ligados diretamente a Nodes devem atuar como adaptadores para sinais e atualizações de apresentação.
4. Autoload `GameState` como uma classe derivada de `Node`, registrando-a em Project Settings → Autoload. Evitar transformar todo manager em singleton; só o estado persistente global deve ser necessário entre cenas.
5. Cenas UI podem emitir sinais e chamar métodos de controladores; não conectar cada botão diretamente à economia, às flags e ao save ao mesmo tempo.
6. Usar `System.Text.Json` ou serialização tipada compatível com o projeto para DTOs; testar desserialização de campos ausentes, enum inválido e IDs inexistentes. Não serializar diretamente objetos `Node`.
7. Para arquivos empacotados de conteúdo, usar caminhos `res://content/...` para leitura. Para save e configuração mutável, usar `user://...`.
8. Em C#, usar `FileAccess` conforme API Godot e descartar/liberar o arquivo corretamente; lidar com retorno nulo/erro de abertura.
9. Validar os JSONs no início do dia e reportar nome do arquivo, ID e campo com erro. Um conteúdo malformado não pode virar erro genérico de triagem em gameplay.
10. Criar testes unitários de `ProtocolManager`, `TriageController`, `EconomyManager` e `RoutinePatientGenerator`. Cada linha da matriz é um cenário de teste.
11. Usar IDs estáveis para ações, pacientes, regra, diálogos e flags; texto é conteúdo de exibição e pode ser alterado sem quebrar o save.
12. Manter um seed para geração de rotina, para reproduzir defeitos.
13. Evitar depender de hot reload para validar estado: reinicializar e retestar save/load em uma build independente.
14. C# no Godot 4 não é alvo de exportação Web conforme a documentação da engine; esta alpha é Windows desktop, por isso a limitação não afeta o alvo atual. Se o alvo mudar, reavaliar a linguagem/engine antes de assumir suporte Web.
15. Consultar a documentação oficial da versão 4.6, em particular [C# basics](https://docs.godotengine.org/en/4.6/tutorials/scripting/c_sharp/c_sharp_basics.html), [linguagens de scripting](https://docs.godotengine.org/en/4.6/getting_started/step_by_step/scripting_languages.html) e [FileAccess](https://docs.godotengine.org/en/4.6/classes/class_fileaccess.html). Confirmar que o pacote instalado corresponde à versão-alvo 4.6.3 .NET.

## 19. Pitch curto para README

**Triagem** é um jogo narrativo de decisão burocrática ambientado em um hospital. Durante plantões curtos, o jogador confere documentos e sintomas, aplica regras que mudam a cada dia e escolhe quem atender, deixar esperando, encaminhar ou recusar. Cada decisão movimenta o dinheiro, a reputação e a consciência — e os casos que parecem absurdos no guichê podem ganhar outro peso no epílogo.

**Alpha demo:** Dia 1 jogável, feito em Godot 4.6.3 + C#, com pacientes narrativos e rotineiros, protocolo externo em JSON, resumo financeiro e evento do cachorro.

## 20. Observações finais para desenvolvimento

- Este arquivo substitui o briefing v0.1 como documento técnico de referência da alpha, mas não apaga os arquivos fonte. `dia-1.md` continua sendo a fonte dos textos e da sequência narrativa do Dia 1 até que os dados JSON sejam validados.
- A alpha deve primeiro provar o loop, a matriz de decisões e a contabilidade; arte final e polimento vêm depois.
- Não criar um sistema médico realista. O conteúdo e as regras são ficcionais.
- Não implementar decisões ainda em aberto como se fossem decisões de design aprovadas.
- A especificação dos quatro botões pode avançar em UI, mas testes de comportamento para FILA/ENCAMINHAR dependem do fechamento da semântica.
- Uma decisão de conteúdo deve ficar no JSON; uma regra geral deve ficar no motor C#; uma animação/feedback deve ficar na apresentação.
- Ao descobrir uma inconsistência, registrar em `docs/decisions.md` e cobrir com teste para evitar regressão.

---

## Referências de origem

- `briefing-triagem.md` — **Triagem — Briefing (v0.1)**, fonte da premissa, campanha, personagens fixos, dilemas, economia geral, evento do cachorro, hora extra, cansaço, arte mínima e fora de escopo.
- `dia-1.md` — **Dia 1 — O Primeiro Plantão (v0.2)**, fonte do protocolo de Dia 1, falas de Ralel, cinco pacientes narrativos, pacientes de rotina, turno, resumo e evento do cachorro.
- Matriz de decisões do Dia 1 e deliberações posteriores nesta conversa — fonte das classificações de Camila/Lucas/Helena/Roberto, multa única por paciente, exceção ética de Helena, FILA como ação final, bônus diário sem erros genéricos e semântica de ações ainda pendente.
- [Documentação oficial Godot 4.6 — C# basics](https://docs.godotengine.org/en/4.6/tutorials/scripting/c_sharp/c_sharp_basics.html)
- [Documentação oficial Godot 4.6 — FileAccess](https://docs.godotengine.org/en/4.6/classes/class_fileaccess.html)
