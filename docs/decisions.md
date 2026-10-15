# Registro de Decisões — What Your Pain?

Este documento é a referência para separar decisões fechadas, propostas ainda não aprovadas e questões em aberto. Atualize este arquivo quando uma decisão for tomada; depois atualize os artefatos impactados (`spec.md`, `plan.md`, `tasks.md`, `data-model.md`, conteúdo e testes).

## Convenções de estado

- **FECHADA:** decisão explícita do usuário; não deve ser reaberta sem solicitação.
- **[PROPOSTA]:** sugestão que não se torna regra até aprovação explícita.
- **[EM ABERTO]:** precisa de decisão; a implementação não deve inventar um comportamento definitivo.
- **BLOQUEIO DE IMPLEMENTAÇÃO:** parte do código pode ser construída, mas não deve consolidar o comportamento pendente.

## 1. Decisões fechadas de produto e campanha

| ID | Decisão | Estado | Fonte/impacto |
|---|---|---|---|
| D-001 | A campanha tem sete dias. | FECHADA | Briefing `Triagem — Briefing (v0.1)`. |
| D-002 | Reputação e consciência determinam o final; dinheiro pressiona o jogador e aparece no epílogo, mas não decide sozinho o final. | FECHADA | Briefing v0.1. |
| D-003 | Reputação e consciência começam em um ponto neutro; “alta” significa acima do neutro ao fim do Dia 7. Limiares podem ser ajustados depois de jogar. | FECHADA | Briefing v0.1. |
| D-004 | Ralel é colega fixo da passagem de turno, aparece antes do turno e não tem desfecho próprio. | FECHADA | Briefing v0.1. Avisos incompletos/errados continuam proposta, salvo confirmação posterior. |
| D-005 | O garoto do atestado é um menino sozinho e tenta obter atestado em quatro ocasiões; o motivo real de bullying é revelado no Dia 7. | FECHADA quanto ao arco central | O briefing marca a distribuição exata dias 1, 3, 5 e 7 como `[PROPOSTA]`; não promovê-la a regra fechada sem confirmação específica. **Atualização 2026-10-14:** distribuição nos dias 1, 3, 5 e 7 confirmada pelo autor (D-029). |
| D-006 | O Dia 1 contém cinco pacientes narrativos em ordem relativa: Márcio Souza, Camila Torres, Lucas Mendes, Helena Rios e Roberto Paiva. | FECHADA | `dia-1.md` v0.2. Pacientes de rotina podem ser intercalados sem mudar a ordem relativa. |
| D-007 | O turno tem 300 segundos de tempo-base; após o tempo-base, param novos pacientes de rotina, mas pacientes narrativos pendentes ainda aparecem. | FECHADA | `dia-1.md` v0.2. Duração prática de 5–7 minutos decorre da conclusão dos casos de história. |
| D-008 | Despesa diária de subsistência: $25. | FECHADA | Folheto do Dia 1. |
| D-009 | Comissão nominal do folheto do Dia 1: $5. | FECHADA como valor; gatilho global ainda aberto | Divergência entre “por paciente atendido” no briefing e “por paciente correto/decisão correta” na matriz. **Atualização 2026-10-14:** gatilho fechado como atendimento (D-026); conteúdo executável ainda não ajustado (INC-015). **Atualização pós-auditoria (2026-10-14):** a regra vigente do gatilho é D-030 (decisão correta); D-026 foi substituída. |
| D-010 | Erro de triagem: sem comissão do paciente e multa de $5. | FECHADA como regra base | Folheto do Dia 1; aplicar a multa única por paciente conforme decisão abaixo. |
| D-011 | Os dilemas seguem a tensão entre seguir o protocolo e ajudar a pessoa, com reputação e consciência como eixos separados. | FECHADA | Briefing v0.1. Valores/condições específicos seguem as entradas de cada dilema. |
| D-012 | A hora extra aparece no Dia 5; aceitar dá +$20, e, se o cachorro foi adotado, o cão destrói um móvel com custo de $12; no dia seguinte há efeito visual de cansaço que afeta a leitura. | FECHADA conforme seção “Oferta de turno dobrado” | A redução de intensidade visual é proposta no briefing e precisa ser confirmada se ainda não estiver aprovada. |
| D-013 | O efeito de cansaço afeta apenas a leitura por meio de efeito visual. | FECHADA | Briefing v0.1. Não aplicar penalidades mecânicas adicionais sem aprovação. |
| D-014 | O cachorro adotado cria consequência narrativa e custo de ração recorrente, e a hora extra pode afetar sua ansiedade. | FECHADA em conceito | O valor da ração e detalhes de ansiedade permanecem em aberto/proposta conforme abaixo. |

## 2. Decisões fechadas nesta conversa

| ID | Decisão | Estado | Consequência obrigatória |
|---|---|---|---|
| D-015 | Uma multa genérica de $5 por paciente com erro, mesmo se várias regras forem violadas. | FECHADA | O log pode registrar múltiplos motivos, mas a penalidade financeira genérica é aplicada uma única vez por paciente. |
| D-016 | Ajudar Helena é uma exceção ética, não um erro genérico para fins de multa. | FECHADA | Comissão de $5 e custo de $10 (líquido −$5); sem multa genérica adicional. |
| D-017 | Ajudar Helena dá consciência +1 e reputação −1 pela auditoria. | FECHADA | Registrar a exceção e o log “Receita vencida liberada”. |
| D-018 | Seguir o protocolo no caso da Helena dá reputação +1 e consciência +0. | FECHADA | A comissão é $5 conforme a matriz do Dia 1. **Atualização 2026-10-14:** com D-026, a comissão de $5 na recusa depende da definição de atendimento (L-010); ver INC-015. **Atualização pós-auditoria (2026-10-14):** regra vigente do gatilho em D-030. |
| D-019 | Bônus de reputação +1 ao final do dia sem erros genéricos. | FECHADA | A escolha ética da Helena não conta como erro genérico e não elimina esse bônus. |
| D-020 | FILA é uma decisão final de triagem no protótipo, com destino visual próprio (cadeiras de espera). | FECHADA | Não tratar FILA como etapa obrigatória que sempre transita automaticamente para outra ação. |
| D-021 | A proposta de aprovar quatro ações independentes ou agrupar `RECOMENDAR/FILA` não foi decidida. | [EM ABERTO] | Não presumir correspondência definitiva entre os rótulos/controles. A interface pode preparar os quatro controles solicitados, mas ações cuja semântica não está mapeada devem ser bloqueadas/`UNRESOLVED`. |

## 3. Matriz do Dia 1 — decisões conhecidas

### 3.1 Márcio Souza

- Evidências: homem de 40 anos; queda de escada; dor aguda e inchaço no joelho; RG válido (emissão 2022).
- Ação correta: `ATENDER`.
- Alternativas sem erro: nenhuma.
- Comissão: +$5 segundo a matriz do Dia 1.
- Reputação/consciência: sem efeito direto.
- Erro em outra ação: sem comissão, multa única de $5 e log “Dor aguda deve ser atendida na hora (regra 2)”.
- Falas alternativas de erro e tutorial que aponta regra 2 são `[PROPOSTA]`.

### 3.2 Camila Torres

- Evidências: tontura leve desde ontem; RG vencido há quatro anos.
- Ações sem erro na matriz atual: `RECUSAR` ou recomendação para renovar documento.
- `ATENDER`: erro; sem comissão e multa única de $5.
- Log: documento vencido (regra 1) e sintoma sem direito a atendimento imediato (regra 3). Uma multa só, apesar dos dois motivos.
- Sem mudança específica de reputação/consciência definida.
- Falas alternativas são propostas se não constarem literalmente em `dia-1.md`.

### 3.3 Lucas Mendes

- Evidências: menino de 11 anos; RG válido; relata febre, mas termômetro mostra 36,6 °C; pede atestado.
- Ação correta na matriz atual: `RECUSAR`.
- Alternativa sem erro na matriz atual: recomendação/encaminhamento, cujo destino e semântica ainda precisam de definição.
- `ATENDER`: erro; sem comissão e multa única de $5.
- Log: atestado sem sintoma agudo e temperatura normal (regra 3).
- Flag: `lucas_visita_1`.
- A história continua e Lucas volta nos dias 3, 5 e 7 conforme a decisão narrativa mais recente desta conversa; manter a cronologia como parte do arco, sem alterar os motivos do personagem.
- Falas alternativas de erro são propostas quando não aparecem na fonte.

### 3.4 Helena Rios

- Evidências: busca medicação contínua para asma do filho; RG válido; receita vencida há dois dias.
- Seguir o protocolo: `RECUSAR` ou recomendar reconsulta; comissão +$5, reputação +1, consciência +0; flag `helena=recusada`.
- Exceção ética: `ATENDER/LIBERAR`; comissão +$5, custo $10, líquido −$5; reputação −1, consciência +1; sem multa genérica adicional; flag `helena=ajudada`.
- Auditoria: “Receita vencida liberada” apenas quando ajudada.
- Destino: farmácia do hospital quando ajudada; saída ao seguir o protocolo.
- O bônus diário de reputação +1 ainda se aplica se não houver erros genéricos.

### 3.5 Roberto Paiva

- Evidências: dor de dente há duas semanas; CNH válida; caso destinado à Odontologia do Posto Central.
- Ações sem erro na matriz atual: recusar ou encaminhar/recomendar ao serviço odontológico.
- `ATENDER`: erro; sem comissão e multa única de $5.
- Log: “Triagem em setor errado: dor crônica vai para a Odontologia”.
- Sem mudança específica de reputação/consciência definida.
- Aceitação da CNH como documento válido segundo a regra 1 precisa de resolução explícita; não inferir só porque o rascunho a descreve como válida.

## 4. Propostas não aprovadas (continuam propostas)

| ID | Proposta | Estado atual |
|---|---|---|
| P-001 | Aviso de Ralel pode estar incompleto ou errado em algum dia. | [PROPOSTA] |
| P-002 | Quatro finais correspondentes às combinações de reputação alta/baixa e consciência alta/baixa. | [PROPOSTA] na fonte original; nomes descritos são rascunho de finais. |
| P-003 | Distribuição exata de quatro pacientes manuais por dia, além dos dois personagens fixos. | [PROPOSTA] |
| P-004 | Pacientes de rotina: aproximadamente 60% corretos para atender e 40% para recusar/encaminhar. | [PROPOSTA] |
| P-005 | Penalidade de reputação −1 por atendimento lento/turno estendido. | [PROPOSTA] |
| P-006 | Dívida final destacada no epílogo. | [PROPOSTA] |
| P-007 | Escolha ética sempre custa $10 como molde dos dilemas; para Helena, os valores e efeitos específicos foram fechados nesta conversa. | O molde geral vem do briefing; não inventar valores ou efeitos adicionais para outros dilemas. |
| P-008 | O cachorro ganha linha de epílogo. | [PROPOSTA] |
| P-009 | Valor da ração: $2 ou $5 por dia. | [EM ABERTO] na fonte, opções não resolvidas. |
| P-010 | O Dia 6 sem dilema novo concentra as consequências do cansaço. | [PROPOSTA] na fonte. |
| P-011 | Efeito de cansaço tem opção para reduzir intensidade visual. | [PROPOSTA] na fonte, salvo decisão posterior explícita. |
| P-012 | Falas de saída alternativas adicionadas durante a revisão da matriz. | [PROPOSTA] quando não forem texto literal da fonte. |

## 5. Decisões em aberto e bloqueios

| ID | Questão em aberto | Impacto | Conduta até resolver |
|---|---|---|---|
| O-001 | Quatro ações independentes (`ATENDER`, `FILA`, `ENCAMINHAR`, `RECUSAR`) versus agrupamento legado `RECOMENDAR/FILA`. | Interface, matriz, motor de decisão, destinos. | Manter os rótulos/controlos solicitados sem fingir que a correspondência está aprovada; resultado afetado fica `UNRESOLVED`. |
| O-002 | Gatilho global da comissão: paciente atendido ou decisão correta/permitida. | Economia, resumo e testes. | Não fechar a contabilidade global. Implementar ledger e política configurável/testável, sem declarar aceitação financeira completa. **→ FECHADA em 2026-10-14 (D-026): gatilho = atendimento.** Conteúdo e aviso do folheto ainda não alterados (INC-014, INC-015, L-010). **Atualização pós-auditoria (2026-10-14):** D-026 foi substituída por D-030 (comissão por decisão correta). |
| O-003 | Precedência quando documento inválido e sintoma agudo aparecem juntos. | Validador e gerador de rotina. | Rejeitar/evitar combinações conflitantes na geração; registrar bloqueio no teste. |
| O-004 | Se CNH satisfaz a regra documental que pede RG/CPF visível. | Roberto e rotina. | Não assumir equivalência até decisão. |
| O-005 | Validade de documentos sem data de expiração ou com validade não representada. | Modelo documental. | Estado `UNKNOWN`; não assumir válido. |
| O-006 | Modelo exato de saldo negativo/dívida: saldo acumulado negativo ou dívida separada. | Economia/save/epílogo. | Preservar o valor monetário sem criar cobrança/juros não definidos. |
| O-007 | Custo de ração do cachorro: $2 ou $5 por dia. | Economia entre turnos. | Não aplicar débito definitivo até escolha. |
| O-008 | Se inspeção, diálogo e leitura do protocolo consomem tempo do relógio. | Ritmo e UX. | Não adicionar consumo de tempo por interação sem decisão. |
| O-009 | Algoritmo e ordem exatos de intercalamento entre rotina e história. | Fila/gerador. | Garantir ordem relativa dos cinco casos; manter estratégia de intercalamento configurável até aprovação. |
| O-010 | Limiares numéricos de reputação/consciência para finais. | Campanha/finais. | Usar ponto neutro e classificação acima/abaixo no final, sem inventar limiar absoluto. |
| O-011 | Escala de `fatigueLevel`, duração e parâmetros visuais do cansaço. | Dia 6 e acessibilidade. | Não aplicar penalidade além do efeito visual aprovado. |
| O-012 | Flags de epílogo específicas para Márcio, Camila e Roberto. | Epílogo. | Não inventar consequências de campanha. |
| O-013 | Se Lucas deve aparecer exatamente nos dias 1, 3, 5 e 7. | Cronologia da campanha. | O arco central é fechado; a sequência exata foi marcada como proposta no briefing original e deve ser confirmada se houver conflito com a especificação atual. **→ Resolvida em 2026-10-14 por D-029 (fechamento de INC-008).** Confirmação do autor após a auditoria final registrada em 13.2; os diálogos individuais de cada visita continuam em aberto. |
| O-014 | Reputação por erro genérico além da perda do bônus de dia sem erros. | Pontuação. | Não aplicar penalidade geral de reputação não definida. |
| O-015 | O destino/semântica exata de `RECOMENDAR` para Camila e Lucas. | Matriz e UI. | Manter classificação provisória da matriz; bloquear destino/resultado se exigir distinção ainda não aprovada. |

## 6. Regra de mudança

Quando uma pendência for decidida:

1. Atualizar a linha correspondente neste arquivo com decisão, data e resumo da escolha.
2. Atualizar os requisitos e critérios de aceite em `spec.md`.
3. Atualizar contratos e tipos em `data-model.md`.
4. Atualizar a arquitetura/plano se necessário.
5. Atualizar tarefas e testes afetados.
6. Não apagar o histórico: registrar a resolução na tabela abaixo.

## 7. Histórico de resoluções

| Data | ID | Decisão final | Artefatos a revisar |
|---|---|---|---|
| 2026-10-14 | D-015 a D-021 | Multa única por paciente com erro; Helena como exceção ética sem multa genérica; FILA é decisão final; bônus +1 por dia sem erros genéricos; sem decisão final sobre quatro ações versus agrupamento legado. | `spec.md`, `plan.md`, `tasks.md`, `data-model.md`, matriz JSON e testes. |
| 2026-10-14 | D-022 | Helena RECUSAR = CORRECT, confirmado pelo autor (seção 11.1). | Matriz JSON (já coerente), `Day1ContentTests`. `spec.md` e `data-model.md` não exigem alteração. |
| 2026-10-14 | D-023 (L-001, P-013) | Saldo inicial $0, decisão do autor; sem definição nas fontes. | `campaign.json`, `GameState.CreateNew`, testes de conteúdo (pendentes). |
| 2026-10-14 | D-024 (L-004) | Roberto ligado à regra 3, com base no briefing v1.0 §8.3. | `openDecisionIds` de Roberto (pendente). |
| 2026-10-14 | D-025 (P-014, parcial) | `dog_adopted` como booleano; semântica pré-decisão em L-011. | Catálogo de marcas, `GameState`, save (pendentes). |
| 2026-10-14 | D-026 (O-002) | Comissão por atendimento; contradições em INC-014, INC-015 e L-010. | Matriz, folheto, `spec.md` §7.4, economia e testes (pendentes). |
| 2026-10-14 | D-027 (INC-012) | Validade do RG revelada na inspeção documental. | Conteúdo já coerente; T020/T021. |
| 2026-10-14 | D-028 (INC-002) | `helena_outcome=refused/helped`, conforme briefing v1.0. | `openDecisionIds` de Helena (pendente); §3.4 preservada como histórico. |
| 2026-10-14 | D-029 (INC-008, O-013) | Lucas nos Dias 1, 3, 5 e 7, conforme briefing v1.0. | Conteúdo futuro da campanha. |
| 2026-10-14 | D-030 (substitui D-026) | Comissão por decisão correta, não por executar ATENDER; confirmada após a auditoria final. `PERMITTED` e a relação com D-016 não decididas. | Matriz, aviso do folheto, `spec.md` §7.4, economia e testes (pendentes). |
| 2026-10-14 | D-031 (D-025, L-011) | `dog_adopted` booleano mais controle separado da ocorrência do evento do cão; mecanismo técnico não definido. | Catálogo de marcas, `GameState`, save, T038–T040 (pendentes). |
| 2026-10-14 | O-013 → D-029 (confirmação) | O-013 resolvido por referência a D-029, confirmado após a auditoria final; diálogos das visitas em aberto. | Conteúdo futuro da campanha. |

## 8. Formato para novas decisões

Use este formato para registrar uma nova questão ou decisão. Uma questão só passa a **FECHADA** por decisão explícita do autor do projeto; ao fechá-la, siga a regra de mudança da seção 6.

| Campo | Conteúdo |
|---|---|
| ID | `D-xxx` (fechada), `P-xxx` (proposta), `O-xxx` (em aberto), `INC-xxx` (inconsistência documental) ou `L-xxx` (lacuna identificada na implementação). |
| Pergunta | O que precisa ser decidido, em uma frase. |
| Estado | FECHADA, [PROPOSTA], [EM ABERTO] ou BLOQUEIO DE IMPLEMENTAÇÃO. |
| Opções | Alternativas conhecidas, com a fonte de cada uma; não inventar opções. |
| Impacto | Arquivos, sistemas, conteúdo e testes afetados. |
| Conduta até resolver | Como o código e o conteúdo se comportam enquanto a questão está aberta. |

Os IDs `O-xxx`, `L-xxx` e `INC-xxx` são referenciados no campo `openDecisionIds` da matriz (`content/decisions/day_1_decision_matrix.json`). Um teste verifica que todo ID usado no conteúdo existe neste arquivo.

## 9. Inconsistências e lacunas registradas na Fase II (2026-10-14)

Registradas durante T005–T012. Nenhuma foi resolvida por escolha silenciosa: a coluna "Conduta adotada" explica o que a implementação faz até a decisão.

### 9.1 Inconsistências entre documentos

| ID | Inconsistência | Estado | Opções | Impacto | Conduta adotada |
|---|---|---|---|---|---|
| INC-001 | `tasks.md`, `plan.md` e `IMPLEMENT.md` citam `quickstart.md` e `contracts/decision-matrix-contract.md`; `spec.md` e este registro citam `dia-1.md` e o briefing. Nenhum desses arquivos está no repositório. | [EM ABERTO] | Adicionar os arquivos; ou remover as referências. | Contrato da matriz, smoke test (T044) e textos literais (falas, evento do cachorro). | O contrato da matriz foi derivado de `data-model.md`. Textos literais não foram criados (ver L-003). |
| INC-002 | Nome da marca de Helena: `helena=recusada/ajudada` (§3.4 deste registro) versus `helena_outcome=refused/helped` (FR-015). O próprio `spec.md` §7.2 sugere normalizar para `helena_outcome`. | [EM ABERTO] | `helena_outcome` com `refused`/`helped`; ou `helena` com `recusada`/`ajudada`. | Matriz, catálogo de marcas, save e epílogo. | Usado `helena_outcome` com `refused`/`helped` (FR-015). Os resultados de Helena citam `INC-002` em `openDecisionIds`. **→ FECHADA em 2026-10-14 (D-028).** |
| INC-003 | Nomes de tipos: `plan.md`, `tasks.md` (T005) e `spec.md` §11 usam `DecisionCase`, `ActionConsequence` e `GameSaveData`; `data-model.md` usa `DecisionMatrix`, `ActionOutcome` e `GameStateSnapshot`. O exemplo `DecisionMatrix` do `data-model.md` repete o ID `day_1_story_matrix` com um único `patientId`, o que geraria IDs duplicados com cinco pacientes. | [EM ABERTO] (técnica) | Unificar em um dos vocabulários. | Nomes de classes, JSON e testes. | `DecisionMatrix` (arquivo) contém um `DecisionCase` por paciente, e cada caso contém `ActionOutcome` por ação. O snapshot é `GameStateSnapshot`. A correspondência está anotada nos comentários das classes. |
| INC-004 | A regra 2 lista "dores intensas/agudas (abdômen, peito, cabeça)", mas a matriz classifica a dor aguda no **joelho** de Márcio como regra 2. | [EM ABERTO] | A lista é exemplificativa (qualquer dor aguda); ou é exaustiva (o caso de Márcio precisaria de outra justificativa). | `ProtocolManager` (T018) e gerador de rotina (T027). | A matriz de Márcio segue §3.1 (fechada). O texto da regra foi mantido literal. Nenhuma condição declarativa foi escrita para essa regra. |
| INC-005 | Pasta de diálogos: `content/dialogues/` (`plan.md`, T009) versus `content/dialogue/` (`data-model.md` §4). | [EM ABERTO] (técnica) | Qualquer das duas. | Caminho do `DataLoader`. | Usado `content/dialogues/` (dois documentos de execução); o caminho está centralizado em `ContentPaths`. |
| INC-006 | Chave de versão do save: `schema_version` (T039) versus `schemaVersion` (`data-model.md`, `GameStateSnapshot`). | [EM ABERTO] (técnica) | snake_case ou camelCase. | `SaveManager` (T039) e compatibilidade de saves. | Todo o JSON usa camelCase (`schemaVersion`), como no `data-model.md`. |
| INC-007 | As referências visuais foram pedidas como `hud-reference.png` e `main-menu-reference.png`; no repositório estão como `.jpg`. | Informativa | Manter `.jpg`; ou exportar `.png`. | `docs/design/visual-references.md`. | Registrados os caminhos `.jpg` existentes. As imagens não foram alteradas. |
| INC-008 | Retorno de Lucas nos dias 3, 5 e 7: §3.3 o trata como decidido ("decisão narrativa mais recente"); O-013 o mantém em aberto; D-005 diz que a distribuição exata é `[PROPOSTA]`. | [EM ABERTO] | Confirmar os dias 1, 3, 5 e 7; ou manter como proposta. | Conteúdo da campanha (fora da alpha). | Nada implementado. Só a marca `lucas_visita_1` existe. **→ FECHADA em 2026-10-14 (D-029).** |

### 9.2 Lacunas encontradas na implementação

| ID | Pergunta | Estado | Opções | Impacto | Conduta adotada |
|---|---|---|---|---|---|
| L-001 | Qual é o saldo inicial da campanha? | [EM ABERTO] | Nenhuma opção na fonte. | `GameState`, economia, resumo e epílogo. | `campaign.json` tem `startingMoney: null` (o validador emite aviso). `GameState.CreateNew` exige o valor explicitamente, sem default. **→ FECHADA em 2026-10-14 (D-023: $0).** Conteúdo executável ainda com `startingMoney: null`. |
| L-002 | Qual é o destino visual de ATENDER, RECUSAR e dos erros, fora os casos de Helena e da FILA? | [EM ABERTO] | Nenhuma opção na fonte. | Fila/feedback visual (T030). | `destinationId` ausente; os resultados citam `L-002`. |
| L-003 | Textos literais de Ralel, dos pacientes, das falas de saída e do evento do cachorro (`dia-1.md`). | BLOQUEIO DE IMPLEMENTAÇÃO (conteúdo) | Adicionar `dia-1.md` ao repositório. | `day_1_dialogue.json`, T023 e T037. | Arquivo de diálogos criado sem falas. Pacientes sem `dialogueId` (o validador emite aviso). |
| L-004 | O log de Roberto ("dor crônica vai para a Odontologia") não cita o número da regra. | [EM ABERTO] | Regra 3 ("sintomas com mais de três dias"), inferida da duração de duas semanas; ou outra regra a definir. | Matriz e auditoria de Roberto. | Vinculado a `D1_NO_IMMEDIATE_MILD_OR_LASTING`, com `L-004` em `openDecisionIds` para revisão. **→ FECHADA em 2026-10-14 (D-024).** |
| L-005 | A leitura do termômetro de Lucas é obrigatória antes da decisão? (Pendência de `spec.md` §7.2 sem entrada na seção 5.) | [EM ABERTO] | Obrigatória; ou opcional. | Inspeção (T021) e ação. | Só o fato `temperature_celsius=36.6` foi registrado como fato oculto. |

## 10. Auditoria de encerramento da Fase II (2026-10-14)

Detalhes em [`docs/audits/fase-2-rastreabilidade.md`](audits/fase-2-rastreabilidade.md). Esta seção só acrescenta itens; as linhas das seções anteriores não foram alteradas. Nenhuma pendência foi fechada: os itens abaixo aguardam confirmação explícita do autor.

### 10.1 Fontes localizadas fora do repositório

`dia-1.md` (v0.2), `briefing-triagem.md` (v0.1) e `briefing-triagem-alpha-demo-v1.0.md` foram encontrados em `D:/Downloads/` e lidos como fonte. Eles não foram copiados para o repositório. `quickstart.md` e `contracts/decision-matrix-contract.md` não foram encontrados (INC-001 continua aberta para esses dois).

### 10.2 Novas inconsistências

| ID | Inconsistência | Estado | Opções | Impacto | Conduta adotada |
|---|---|---|---|---|---|
| INC-009 | O esboço da matriz do briefing v1.0 §8.4 classifica ENCAMINHAR como `PERMITTED` (+$5) para Camila, Lucas e Roberto, e `REFER_RECONSULTATION` como `PERMITTED` para Helena. D-021 e O-001 mantêm o mapeamento em aberto. O próprio briefing chama §8.4 de "esboço de migração" e, no §17.3 A, mantém a questão em aberto. | [EM ABERTO] | Adotar o esboço §8.4; ou manter UNRESOLVED até decidir O-001. | Matriz, testes e UI dos quatro slots. | Mantido `UNRESOLVED` (segue D-021/O-001). |
| INC-010 | O briefing v1.0 §8.3 define destinos para todos os pacientes (hospital ao atender, cadeiras de espera na fila, saída ao recusar/encaminhar, hospital em atendimento incorreto). O conteúdo só tem os destinos de Helena e da FILA (L-002). | [EM ABERTO] | Adotar os destinos do §8.3; ou manter L-002 aberta. | Matriz e fila visual (T030). | Destinos ausentes até confirmação. Destinos que dependem de FILA × ENCAMINHAR continuam pendentes (briefing §17.3 L). |
| INC-011 | Sintoma de Camila: ficha literal "Tontura **chata** desde ontem." (`dia-1.md`) versus "tontura leve desde ontem" (briefing v1.0, `spec.md` e §3.2 deste registro). | [EM ABERTO] | Texto literal da ficha; ou o texto consolidado. | Ficha exibida e regra 3 ("tontura leve"). | Conteúdo usa "leve". |
| INC-012 | A validade do RG (Márcio e Camila) é revelada por inspeção em `spec.md` US2-2, mas aparece entre os "dados visíveis" no briefing v1.0 §8.3. | [EM ABERTO] | Visível desde a chegada; ou revelada por inspeção. | Inspeção de documento (T020/T021). | Conteúdo segue `spec.md` (fato oculto). **→ FECHADA em 2026-10-14 (D-027): validade revelada na inspeção documental.** |
| INC-013 | O briefing v1.0 §18.4 recomenda `GameState` como Node em Autoload e lista, no §7.1, as fases `menu`, `handover`, `shift`, `summary`, `between-shifts` e `completed`. A implementação tem `GameState` como classe pura e as fases `NotStarted`, `InDay` e `DayClosed`. | [EM ABERTO] (técnica) | Adaptador Node em Autoload envolvendo o `GameState` puro; ou converter a própria classe. | T013 a T015, save (T039). | Nenhuma mudança na Fase II. |

### 10.3 Atualização de itens existentes (evidências encontradas)

| ID | Evidência nova | Estado |
|---|---|---|
| INC-001 | `dia-1.md` e os dois briefings foram localizados (seção 10.1). `quickstart.md` e o contrato da matriz continuam ausentes. | [EM ABERTO] |
| INC-002 | O briefing v1.0 §7.2 e §17.1 (fechados) usam `helena_outcome=refused/helped`, como FR-015. Só a §3.4 deste registro usa `helena=recusada/ajudada`. | [EM ABERTO]; aguardando confirmação |
| INC-008 | O briefing v1.0 §17.1 lista o retorno de Lucas nos Dias 3, 5 e 7 como fechado; `dia-1.md` traz a fala "Volto depois de amanhã". | [EM ABERTO]; aguardando confirmação |
| L-001 | O resumo de exemplo de `dia-1.md` §7 sugere saldo inicial $0, mas o briefing v1.0 §9.2 diz que os números são apenas exemplos. | [EM ABERTO] |
| L-002 | Destinos encontrados no briefing v1.0 §8.3 (ver INC-010). | [EM ABERTO]; aguardando confirmação |
| L-003 | As falas literais de Ralel, dos cinco pacientes e do evento do cachorro existem em `dia-1.md` (fora do repositório). | [EM ABERTO]; aguardando autorização para importar |
| L-004 | O briefing v1.0 §8.3 associa Roberto a "sintoma com mais de três dias e setor inadequado para odontologia", o que apoia o vínculo à regra 3. | [EM ABERTO]; aguardando confirmação |
| L-005 | As fontes só mostram a leitura do termômetro dentro do diálogo; nenhuma diz se ela é obrigatória. | [EM ABERTO] |

## 11. Orientações do autor e revisão do conteúdo (2026-10-14)

Após aprovar a auditoria, o autor deu as orientações abaixo. Nenhuma regra de gameplay foi alterada: classificações, comissões, multas, custos, reputação e consciência da matriz continuam iguais. As mudanças de conteúdo restringem-se a textos literais, destinos e marcação de pendências.

### 11.1 Decisões e propostas

| ID | Decisão / proposta | Estado | Fonte | Conduta |
|---|---|---|---|---|
| D-022 | No caso de Helena, RECUSAR (seguir o protocolo) é classificado como `CORRECT`. | FECHADA | Confirmação explícita do autor em 2026-10-14. Apoio: briefing v1.0 §8.4 (`"REFUSE": {"classification": "CORRECT"}`). | A matriz já usava CORRECT; agora cita D-022. |
| P-013 | Saldo inicial da campanha de **$0**. | [PROPOSTA] do autor, aguardando confirmação final | Proposta do autor (2026-10-14). O resumo de exemplo de `dia-1.md` §7 (saldo do dia igual ao saldo acumulado) é só **evidência de apoio**: o briefing v1.0 §9.2 diz que esses números são exemplos, não definição. | `campaign.json` mantém `startingMoney: null` até a confirmação. Resolve L-001 quando confirmada. **→ Aprovada em 2026-10-14 como D-023.** |
| P-014 | Representar marcas binárias (`dog_adopted`, `lucas_visita_1`) como booleano JSON (`true`/`false`) em vez de texto. | [PROPOSTA] de modelagem | O save de exemplo do briefing v1.0 §13.2 usa `"dog_adopted": false` e `"lucas_visita_1": true`. O `GameStateSnapshot` já tem `dogAdopted` booleano. | Esquema inalterado até aprovação (ver 11.3). **→ Aprovada em 2026-10-14 somente para `dog_adopted` (D-025); `lucas_visita_1` não foi incluída na aprovação.** |

### 11.2 Nova inconsistência e novas lacunas

| ID | Questão | Estado | Opções | Impacto | Conduta adotada |
|---|---|---|---|---|---|
| INC-014 | O folheto literal (`dia-1.md` §2, bloco "[ ATENÇÃO ]") afirma "Comissão: $5 por paciente correto.", o que contradiz "comissão por paciente atendido" (briefing v0.1). É a mesma divergência de O-002, agora no texto exibido ao jogador. | [EM ABERTO] | Exibir o texto literal após decidir O-002; ou exibi-lo com indicação de pendência. Não foi criada redação substituta. | `ProtocolPanel` (T016) e economia. | Texto preservado literalmente como aviso `OPEN` (`D1_NOTICE_COMMISSION`), com `O-002` e `INC-014`. O validador o reporta como `PROTOCOL_NOTICE_BLOCKED` e impede marcá-lo `APPROVED` enquanto houver pendência. **Atualização 2026-10-14:** O-002 foi fechada (D-026), mas a contradição entre o texto literal e a regra aprovada permanece; aviso e conteúdo não alterados. **Atualização pós-auditoria (2026-10-14):** ver D-030 e 13.3; estado inalterado. |
| L-006 | Reputação e consciência nas recusas corretas de Camila, Lucas e Roberto: as fontes só indicam "+$5". | [EM ABERTO] | Confirmar 0; ou definir outros efeitos. | Matriz, `ReputationManager` (T033). | Valores 0 mantidos (sem mudança de gameplay), com `L-006` nesses três resultados. |
| L-007 | Motivos de regra (`reasonRuleIds`) em resultados sem erro, que as fontes não explicitam por resultado. O briefing v1.0 §8.3 dá só a "Regra" de cada paciente. | [EM ABERTO] | Manter como justificativa; ou restringir os motivos a erros e exceções. | Matriz e feedback. | Mantidos, com `L-007` nos resultados CORRECT. |
| L-008 | Divisão do protocolo em 7 regras, com IDs `D1_…` e categorias, versus 3 IDs no briefing v1.0 (`document_validity`, `acute_symptoms`, `no_immediate_care`). | [EM ABERTO] (técnica/design) | 7 sub-regras; ou 3 regras. | Protocolo, matriz e logs. | Mantidas as 7 regras; registrado nas notas do protocolo. |
| L-009 | Representação das marcas narrativas (texto versus booleano/enum tipado). | [EM ABERTO] | Ver P-014. | Conteúdo, `GameState`, save (T039). | Texto mantido. **Atualização 2026-10-14:** resolvida apenas para `dog_adopted` (D-025); as demais marcas continuam em aberto. |

### 11.3 Avaliação de P-014 (marcas booleanas)

- **A favor:** coincide com o exemplo de save do briefing v1.0 §13.2. Elimina a ambiguidade entre `"true"`/`"false"` e marca ausente. Fica alinhado a `GameStateSnapshot.dogAdopted`.
- **Contra / custo:** `flagsToSet` e `narrativeFlags` hoje são `Dictionary<string,string>`, e `helena_outcome` é enumerada (`refused`/`helped`), não binária. Adotar booleanos exige um tipo por marca no catálogo (ex.: `valueType: BOOLEAN | ENUM`) e valores JSON tipados, o que muda o esquema de conteúdo e de save.
- **Para não adotar:** é preciso decidir se "não adotar" grava `false` ou deixa a marca ausente. As fontes só dizem que adotar define `dog_adopted` como verdadeiro e que, sem adoção, "o cão não aparece mais".
- **Ação tomada:** o valor `"false"` foi **removido** de `dog_adopted.allowedValues`, porque era inferência sem justificativa. A marca só aceita `"true"`. Ausência da marca equivale a não adotado em `GameState.DogAdopted`. O esquema não foi alterado.

### 11.4 Atualização de itens existentes

| ID | Atualização | Estado |
|---|---|---|
| INC-001 | `dia-1.md`, `briefing-triagem.md` e `briefing-triagem-alpha-demo-v1.0.md` foram copiados sem alteração para `docs/sources/` (hashes em `docs/sources/README.md`). `quickstart.md` e `contracts/decision-matrix-contract.md` **não existem**; as referências em `tasks.md`, `plan.md` e `IMPLEMENT.md` foram anotadas. A geração desses dois artefatos continua pendente. | Parcialmente resolvida |
| INC-010 / L-002 | Destinos do briefing v1.0 §8.3 aplicados aos resultados resolvidos: hospital (ATENDER e ATENDER incorreto), cadeiras de espera (FILA de Márcio), saída (RECUSAR). Continuam sem destino: ENCAMINHAR de Márcio (L-002) e todos os resultados `UNRESOLVED`. | Parcialmente resolvida |
| L-003 | Falas literais de `dia-1.md` importadas para `content/dialogues/day_1_dialogue.json`: Ralel (3), diálogos e falas de saída dos 5 pacientes, e as duas saídas de Helena. As saídas foram ligadas só aos caminhos com fala literal. Falas alternativas continuam `[PROPOSTA]` (P-012); o evento do cachorro fica para T037. | Resolvida para o Dia 1 (exceto evento do cachorro) |
| INC-011 | Ficha literal de Camila ("Tontura chata desde ontem.") aplicada ao conteúdo exibido. A descrição consolidada "tontura leve", que sustenta o motivo da regra 3, **não** foi resolvida. Os resultados de Camila citam `INC-011`. | [EM ABERTO] |
| L-004 | Justificativa registrada: o briefing v1.0 §8.3 associa Roberto a "sintoma com mais de três dias e setor inadequado para odontologia". O "setor inadequado" não é regra do folheto. Os resultados continuam citando `L-004` até a confirmação explícita. | [EM ABERTO]; justificativa registrada |
| L-001 | Ver P-013. | [EM ABERTO] |
| INC-012 | Visibilidade da validade documental continua em aberto, por orientação do autor. | [EM ABERTO] |

## 12. Decisões aprovadas pelo autor (2026-10-14)

Decisões explícitas do autor em 2026-10-14. Esta atualização alterou **somente este registro**: o conteúdo executável (`content/`), o código, os testes, o folheto e as tarefas **não** foram ajustados, e por isso o conteúdo ainda diverge de algumas destas decisões (ver 12.2). As linhas anteriores foram preservadas e só receberam anotações de atualização.

### 12.1 Decisões fechadas

| ID | Decisão | Origem | Fontes verificadas em `docs/sources/` | Limitação de apoio | Estado do conteúdo executável |
|---|---|---|---|---|---|
| D-023 | Saldo inicial da campanha: **$0**, valor explícito. | Autor (era P-013; resolve L-001). | `dia-1.md` §7 (resumo de exemplo: "SALDO DO DIA: +$50.00" igual a "SALDO ACUMULADO … +$50.00"). | **Nenhuma fonte define o saldo inicial.** O resumo de `dia-1.md` é só evidência de apoio, e o briefing v1.0 §9.2 diz que esses números "são exemplos de um plantão típico, não metas rígidas". A decisão é do autor. | Não aplicado: `campaign.json` mantém `startingMoney: null` (aviso `STARTING_MONEY_UNDEFINED`). |
| D-024 | O motivo do erro de Roberto ao ATENDER (e da recusa correta) é a regra 3, "Sintomas com mais de 3 dias". | Autor (resolve L-004). | Briefing v1.0 §8.3, Paciente 5: "**Regra:** sintoma com mais de três dias e setor inadequado para odontologia." `dia-1.md` §4: "dor de dente há 2 semanas". | O briefing cita também "setor inadequado para odontologia", que **não é regra do folheto**; esse componente segue fora de `reasonRuleIds`. O log literal ("Triagem em setor errado…") não cita o número da regra. | Vínculo já usado na matriz; os resultados de Roberto ainda citam `L-004` em `openDecisionIds` (marca desatualizada). |
| D-025 | A marca `dog_adopted` é representada como booleano `true`/`false`. | Autor (era P-014, só para `dog_adopted`). | Briefing v1.0 §7.1 (`dog_adopted` tipo `bool`), §13.2 (save de exemplo com `"dog_adopted": false`), §15.4 ("Adotar define `dog_adopted=true`; não adotar não gera custo recorrente"). `GameStateSnapshot.dogAdopted` já é booleano. | As fontes não definem o valor antes da decisão do jogador (ver L-011 em 12.2). `lucas_visita_1` e `helena_outcome` não estão cobertas por esta decisão. | Esquema não alterado: o catálogo ainda trata a marca como texto com valor permitido `"true"` (L-009 continua para as demais marcas). **Atualização pós-auditoria (2026-10-14):** confirmada e complementada por D-031 (controle separado da ocorrência do evento). |
| D-026 | O gatilho da comissão é o **atendimento**, não a correção da decisão. | Autor (resolve O-002). | Briefing v0.1, "Economia": "Comissão por paciente atendido: quanto mais atende, mais ganha" e "Comissão: $5 por paciente atendido". O valor de $5 segue D-009. | Contradiz `dia-1.md` §2 ("Comissão: $5 por paciente correto."), o resumo de `dia-1.md` §7 ("Corretos: 15 ($75)"), a matriz do Dia 1 e o briefing v1.0 §8.2/§8.3 (+$5 nas recusas corretas). As fontes **não definem** se "atendimento" é a ação ATENDER ou todo paciente que recebe decisão final (L-010). | Não aplicado: a matriz mantém +$5 nas recusas corretas de Camila, Lucas, Roberto e Helena, e o aviso `D1_NOTICE_COMMISSION` continua `OPEN` citando `O-002` (ver INC-014, INC-015). **SUBSTITUÍDA por D-030 (2026-10-14, após a auditoria final): o gatilho vigente é a decisão correta. Mantida como histórico.** |
| D-027 | A validade do RG é revelada durante a **inspeção documental**. | Autor (resolve INC-012). | `spec.md` US2-2 ("quando o jogador inspeciona o RG de Márcio ou Camila … a validade correspondente é revelada"); `dia-1.md` §4 ("Jogador (ao conferir o documento)", "Jogador (ao apontar a data no RG)"). | O briefing v1.0 §8.3 lista a validade do RG em "Dados visíveis", e `dia-1.md` §2 diz que a ficha "entregue na hora" traz o "documento apresentado". Essas passagens ficam como evidência da divergência anterior. | Já coerente: a validade de Márcio e Camila está em `hiddenFacts`. |
| D-028 | A marca de Helena é `helena_outcome`, com valores `refused` (seguir o protocolo: recusar ou recomendar reconsulta) e `helped` (liberação excepcional). | Autor, conforme briefing v1.0 (fecha INC-002). | Briefing v1.0 §7.2 (tabela de marcas), §8.3, §8.4, §13.2 e §17.1 (lista de fechados); FR-015. | O próprio briefing v1.0 §5.3 (`NarrativeFlagManager.cs`) ainda cita "`helena=recusada`, `helena=ajudada`", assim como a §3.4 deste registro. Essas menções ficam preservadas como histórico e passam a ser consideradas superadas. | Já coerente: a matriz usa `helena_outcome`; os resultados de Helena ainda citam `INC-002` em `openDecisionIds` (marca desatualizada). |
| D-029 | Lucas aparece nos Dias 1, 3, 5 e 7, retornando nos Dias 3, 5 e 7 independentemente da decisão do Dia 1; a revelação do bullying culmina no Dia 7. | Autor, conforme briefing v1.0 (fecha INC-008; resolve O-013). | Briefing v1.0 §8.3 (Paciente 3, "Continuidade"), §12.2 e §17.1. `dia-1.md` §4: "Volto depois de amanhã". | O briefing v0.1 marcava os dias 1, 3, 5 e 7 como `[PROPOSTA]`, e D-005 pedia confirmação específica, agora dada. O briefing v1.0 ressalva que "a distribuição de detalhes das visitas continua dependente do conteúdo futuro": só os dias estão fechados, não o conteúdo de cada visita. | Sem impacto na alpha do Dia 1 (só existe `lucas_visita_1`). |

### 12.2 Contradições preservadas

| ID | Contradição | Estado | Impacto | Conduta |
|---|---|---|---|---|
| INC-014 | O texto literal do folheto, "Comissão: $5 por paciente correto.", contradiz D-026 (atendimento). | [EM ABERTO]: a decisão de design está fechada, mas falta definir como o texto literal será tratado | `ProtocolPanel` (T016), aviso do folheto. | Aviso e conteúdo **não alterados**: continua `OPEN` com `O-002`/`INC-014`. Não foi criada redação substituta. |
| INC-015 | D-026 conflita com o conteúdo e com entradas fechadas anteriores que dão +$5 a recusas corretas: D-018 ("A comissão é $5 conforme a matriz do Dia 1"), §3.2–§3.5 deste registro, `spec.md` §7.2/§7.4, briefing v1.0 §8.3/§8.4 e a matriz JSON. Se "atendimento" significar só a ação ATENDER, essas recusas teriam comissão $0. | [EM ABERTO] | Matriz, `EconomyManager` (T032), resumo, testes de economia. | Conteúdo **não alterado**. O aceite financeiro continua bloqueado até definir L-010 e atualizar matriz, `spec.md` e testes na mesma mudança (seção 6). **Atualização pós-auditoria (2026-10-14):** premissa alterada por D-030; ver 13.3. Estado inalterado. |
| L-010 | O que conta como "atendimento" para a comissão: só a ação ATENDER; ou todo paciente com decisão final, exceto erros (D-010). O briefing v0.1 diz "quanto mais atende, mais ganha"; o briefing v1.0 §7.1 diz que "a contagem de 'atendidos' no resumo precisa seguir definição aprovada"; o resumo de `dia-1.md` §7 conta ~15 "Pacientes atendidos". | [EM ABERTO] | Matriz, economia, resumo. | Nenhuma interpretação aplicada. Helena ajudada (ATENDER) já recebe +$5 por D-016 em qualquer leitura. **Atualização pós-auditoria (2026-10-14):** sem objeto enquanto vigorar D-030; ver 13.3. Estado inalterado. |
| L-011 | Semântica de `dog_adopted = false`: o booleano não distingue "o jogador recusou o filhote" de "o evento ainda não aconteceu" (antes do resumo do Dia 1, ou num save anterior ao evento). **Risco:** código ou save podem tratar `false` como decisão tomada e pular o evento, ou exibir a linha de epílogo de não adoção sem escolha. | [EM ABERTO] (modelagem) | `GameState`, `NarrativeEventController` (T038), `SaveManager` (T039), epílogo. | Esquema **não alterado**. Opções a decidir: `false` só gravado após a confirmação, junto com um indicador separado de evento resolvido; ou marca ausente (indefinida) até a decisão. **→ Resolvida quanto à semântica por D-031 (2026-10-14): controle separado da ocorrência do evento; nome e mecanismo técnico não definidos.** |
| INC-011 | Descrição da tontura de Camila: "Tontura chata desde ontem." (ficha literal de `dia-1.md`) versus "tontura leve desde ontem" (briefing v1.0, `spec.md`, §3.2 deste registro). | [EM ABERTO], mantida por decisão do autor | Ficha exibida e motivo da regra 3. | Sem mudança. |
| INC-001 | `quickstart.md` e `contracts/decision-matrix-contract.md` continuam **ausentes**. Não foram encontrados no repositório, no histórico Git, no GitHub nem no disco local, e não constam das fontes. | [EM ABERTO] | T044 e contrato da matriz. | Não criados. A geração continua pendente. |

### 12.3 Fontes verificadas

Verificadas nesta atualização: `docs/sources/dia-1.md` (v0.2), `docs/sources/briefing-triagem.md` (v0.1), `docs/sources/briefing-triagem-alpha-demo-v1.0.md` (v1.0) e `specs/001-triagem-alpha-dia-1/spec.md`. Não estavam acessíveis, porque não existem: `quickstart.md` e `contracts/decision-matrix-contract.md`. As deliberações da conversa de design citadas pelo briefing v1.0 ("matriz de decisões do Dia 1 e deliberações posteriores") só estão disponíveis indiretamente, por meio do próprio briefing e deste registro.

## 13. Confirmações do autor após a auditoria final da Fase II (2026-10-14)

Confirmações explícitas do autor, dadas depois da auditoria final de fechamento da Fase II. Esta atualização alterou **somente este registro**. Conteúdo executável, código, testes, folheto, especificações e tarefas **não** foram ajustados. As entradas anteriores foram preservadas e só receberam anotações de atualização.

### 13.1 Decisões fechadas

| ID | Decisão | Origem | Relação com o histórico | Limitações e pontos não decididos | Estado do conteúdo executável |
|---|---|---|---|---|---|
| D-030 | A comissão é concedida **por decisão correta**, e não simplesmente por executar a ação `ATENDER`. | Confirmação explícita do autor após a auditoria final (2026-10-14). | **Substitui D-026 como regra vigente do gatilho da comissão.** D-026 (registrada no mesmo dia com o gatilho "atendimento") fica preservada como histórico. O valor de $5 continua definido por D-009, e a ausência de comissão em erro de triagem por D-010. Coerente com o folheto literal (`dia-1.md` §2: "Comissão: $5 por paciente correto.") e com o briefing v1.0 §8.2 ("Comissão de $5 por decisão correta aparece na matriz do Dia 1"). O briefing v0.1 ("Comissão: $5 por paciente atendido") fica registrado como fonte divergente. | (1) **Resultados `PERMITTED`:** não foi decidido se recebem comissão; continua lacuna identificada na auditoria final (`spec.md` §7.4 fala em "decisão correta/permitida"). (2) **D-016** (Helena ajudada, `ETHICAL_EXCEPTION`, comissão de $5) não foi alterada por esta confirmação; a relação entre D-016 e D-030 não foi tratada. (3) Os 8 resultados `UNRESOLVED` de FILA/ENCAMINHAR (O-001, O-015) não mudam. | Não alterado. A matriz já paga +$5 nas decisões `CORRECT` e $0 nos erros, mas os resultados e o aviso `D1_NOTICE_COMMISSION` (ainda `OPEN`) continuam citando `O-002`/`INC-014`. |
| D-031 | `dog_adopted` permanece **booleano** (`true`/`false`), e o sistema deve controlar **separadamente** se o evento do cão já ocorreu, distinguindo o estado de adoção do estado de ocorrência do evento. | Confirmação explícita do autor após a auditoria final (2026-10-14). | **Confirma D-025** (representação booleana) e **responde a L-011**: a ambiguidade de `false` ("recusou" × "evento ainda não ocorreu") é tratada por um controle separado da ocorrência do evento. | Nome, tipo e mecanismo técnico do controle de ocorrência **não foram definidos** e ficam fora desta decisão. As fontes em `docs/sources/` não descrevem esse controle: é decisão do autor. | Não alterado: o catálogo ainda trata `dog_adopted` como texto (`allowedValues ["true"]`) e não existe controle de ocorrência do evento. |

### 13.2 Confirmação de O-013 por referência a D-029

| Item | Confirmação | Origem | Registro |
|---|---|---|---|
| O-013 | O-013 está **resolvido por referência a D-029**: os retornos de Lucas estão definidos para os Dias 1, 3, 5 e 7. Nenhuma decisão nova foi criada. | Confirmação explícita do autor após a auditoria final (2026-10-14). | A entrada original de O-013 (seção 5) e a anotação de resolução foram preservadas. **Os diálogos individuais de cada visita continuam em aberto** e não são requisito definido (ver também a ressalva de D-029 em 12.1). |

### 13.3 Itens relacionados cujo estado não foi alterado

A premissa destes itens vinha da redação de D-026 ("atendimento"). Com D-030 a regra vigente é a decisão correta, mas o **estado deles foi mantido**: não houve autorização explícita para fechá-los.

| ID | Situação após D-030 | Estado |
|---|---|---|
| INC-014 | O texto literal do folheto ("Comissão: $5 por paciente correto.") deixa de contradizer a regra vigente. A forma de exibição do aviso e seu status no conteúdo continuam a decidir. | [EM ABERTO] (inalterado) |
| INC-015 | O conflito descrito (D-026 contra os +$5 das recusas corretas) dependia do gatilho "atendimento"; com D-030 essa premissa não se aplica. | [EM ABERTO] (inalterado) |
| L-010 | A pergunta sobre o que conta como "atendimento" perde o objeto enquanto vigorar D-030. | [EM ABERTO] (inalterado) |
