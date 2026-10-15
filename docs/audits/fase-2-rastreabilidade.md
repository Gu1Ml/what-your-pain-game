# Auditoria de rastreabilidade — encerramento da Fase II (T005–T012)

**Data:** 2026-10-14
**Escopo:** conteúdo de `content/` e decisões embutidas no código da Fase II, comparados às fontes de design.
**Resultado:** auditoria documental. Nenhum código, conteúdo JSON ou regra foi alterado nesta etapa. As correções sugeridas aguardam revisão de design.

> **Ações posteriores (2026-10-14):** a auditoria foi aprovada como registro de rastreabilidade. As orientações do autor e as correções aplicadas estão em [`docs/decisions.md` §11](../decisions.md). As fontes estão em [`docs/sources/`](../sources/README.md). Este documento é mantido como estava no momento da auditoria.

## 1. Fontes consultadas

### 1.1 Recuperadas (fora do repositório)

Encontradas em `D:/Downloads/`; não foram copiadas para o repositório.

| Documento | Caminho | Tamanho | SHA-256 | Papel |
|---|---|---|---|---|
| Briefing v0.1 | `D:/Downloads/briefing-triagem.md` | 12 777 B | `5e1f00c6…f4f5` | Briefing original (premissa, economia geral, campanha). |
| Dia 1 v0.2 | `D:/Downloads/dia-1.md` | 9 156 B | `2104dd1e…abcc6` | Fonte literal do protocolo, das falas e dos cinco pacientes do Dia 1. |
| Briefing técnico v1.0 | `D:/Downloads/briefing-triagem-alpha-demo-v1.0.md` | 80 063 B | `850141a1…1ef2` | Consolida v0.1, `dia-1.md` e as decisões da conversa; é a fonte citada em `spec.md`. |

O briefing v1.0 §20 declara que `dia-1.md` "continua sendo a fonte dos textos e da sequência narrativa do Dia 1".

### 1.2 Ainda ausentes

| Caminho esperado | Citado em | Tentativas | Impacto |
|---|---|---|---|
| `specs/001-triagem-alpha-dia-1/quickstart.md` | `tasks.md` (entrada, T044), `plan.md`, `IMPLEMENT.md` | Árvore do repositório; histórico Git (`--all`, nomes em todos os commits); stash; objetos inalcançáveis; refs remotas no GitHub (`ls-remote`: só `main` = `ff08ebe`); busca por nome em `D:/` (profundidade 7) e nas pastas Downloads, Documents, Desktop e OneDrive do usuário; conteúdo dos três briefings. | Sem roteiro de smoke test para T044. Não bloqueia a Fase III. |
| `specs/001-triagem-alpha-dia-1/contracts/decision-matrix-contract.md` | `tasks.md` (entrada), `plan.md` | Mesmas tentativas. | O contrato implementado deriva de `data-model.md`. O esboço mais próximo é o briefing v1.0 §6 e §8.4, que diverge em pontos listados abaixo. |

Esses dois arquivos parecem ser artefatos do Spec-Kit que nunca foram gerados. **É preciso pedir os originais** ao autor, ou decidir que eles não existem e remover as referências.

## 2. Legenda de origem

| Código | Significado |
|---|---|
| **APR** | Decisão explicitamente aprovada (`D-xxx` em `decisions.md`, ou `[FECHADO]`/`[APROVADO]` no briefing v1.0). |
| **LIT** | Conteúdo literal de uma fonte (`dia-1.md`, briefing v0.1/v1.0, `spec.md`). |
| **PROP** | Proposta ainda não aprovada (`[PROPOSTA]`). |
| **INF** | Inferência do agente: escolha técnica ou interpretação sem fonte explícita. |
| **ABERTO** | Decisão em aberto (`O-xxx`, `L-xxx`, `INC-xxx`). |

## 3. Rastreabilidade por item

### 3.1 Configuração do dia e campanha (`content/campaign.json`)

| Item | Valor implementado | Origem | Fonte / observação |
|---|---|---|---|
| Campanha de 7 dias | `totalDays: 7` | APR | D-001; briefing v0.1 "Decisões fechadas". |
| Ordem dos pacientes de história | Márcio → Camila → Lucas → Helena → Roberto | APR | D-006; `dia-1.md` §3. |
| Tempo-base | `300` s | APR | D-007; `dia-1.md` §6. |
| Geração de rotina no Dia 1 | `routineGenerationEnabled: true` | APR | D-007 (rotina gerada até o fim do relógio). |
| Despesa diária | `25` | APR | D-008; folheto do Dia 1. |
| Valor da comissão | `nominalCommission: 5` | APR (valor) / ABERTO (gatilho) | D-009; gatilho em O-002. |
| Multa genérica | `genericErrorFine: 5` | APR | D-010, D-015. |
| **Saldo inicial** | `startingMoney: null` | ABERTO | L-001. Ver §4, item 19. |
| Marca `lucas_visita_1` | valores `["true"]` | LIT (ID) / INF (tipo) | Briefing v1.0 §7.2; o save de exemplo do §13.2 usa booleano `true`, não texto. |
| Marca `helena_outcome` | `refused`/`helped` | LIT | FR-015; briefing v1.0 §7.2 e §17.1 (lista de fechados). Diverge de `decisions.md` §3.4 (INC-002). |
| Marca `dog_adopted` | `["true","false"]` | LIT (ID) / **INF** (valor `false`) | Nenhuma fonte define o valor gravado ao não adotar. |
| Localização dos valores econômicos | em `DayConfig` | INF | O exemplo do briefing v1.0 §6.4 os coloca no protocolo (`commission_amount`, etc.). |

### 3.2 Protocolo (`content/protocols/day_1_protocol.json`)

| Item | Implementado | Origem | Fonte literal (`dia-1.md` §2) |
|---|---|---|---|
| Três grupos | títulos "Documento", "Sintomas para atendimento imediato", "Sintomas sem direito a atendimento imediato" | LIT de `spec.md` §7.1 | O folheto usa "[ REGRAS BÁSICAS DE VALIDAÇÃO ]", "SINTOMAS ACEITOS PARA ATENDIMENTO IMEDIATO" e "SINTOMAS SEM DIREITO A ATENDIMENTO IMEDIATO **(Encaminhar/Recusar)**". |
| Regra 1 | "Documento: deve conter nome e RG/CPF visível e estar dentro da validade." | LIT de `spec.md` (paráfrase do folheto) | "DOCUMENTO: Deve conter Nome + RG/CPF visível e DENTRO DA VALIDADE." |
| Regra 2 (3 itens) | textos de `spec.md` | LIT de `spec.md` | "Febre acima de 38°C"; "Dores intensas/Agudas (abdômen, peito, cabeça)"; "Ferimentos com sangramento ativo". |
| Regra 3 (3 itens) | "Pedido de atestado…", "Receita vencida…" | LIT de `spec.md` | "Pedidos de atestado **médico** sem sintoma agudo"; "Tontura leve / Cansaço / Sintomas com mais de 3 dias"; "Receita **médica** vencida ou sem assinatura". |
| Bloco "[ ATENÇÃO ]" do folheto | ausente | — | "Erros de triagem: sem comissão do paciente + multa de $5." / "Comissão: $5 por paciente correto." / "Custo diário de subsistência: $25." |
| Divisão em 7 regras e seus IDs (`D1_…`) | 7 regras | **INF** | O briefing v1.0 §6.4/§8.3 usa 3 IDs: `document_validity`, `acute_symptoms`, `no_immediate_care`. |
| Categorias (`DOCUMENT`, `SYMPTOM`, `ATTESTATION`, `PRESCRIPTION`) | por regra | **INF** | O briefing usa os grupos `document`, `immediate_care`, `refer_or_refuse`. |
| `priority` ausente | — | ABERTO | O-003 (o validador rejeita prioridade). |
| `conditions` vazias | — | INF (adiamento técnico) | Ficam para T018. |

### 3.3 Pacientes (`content/patients/day_1_narrative_patients.json`)

| Paciente | Campo | Implementado | Origem | Fonte literal / divergência |
|---|---|---|---|---|
| Márcio | idade, nome, ID | 40, "Márcio Souza", `marcio_souza` | LIT | `dia-1.md` §4; briefing v1.0 §6.3. |
| | perfil | "Homem de 40 anos." | LIT (resumido) | "homem de 40 anos, segurando o joelho direito." |
| | sintomas | "Dor aguda no joelho", "Inchaço no joelho" | INF (paráfrase) | "queda de escada, dor aguda no joelho e inchaço visível". O briefing especifica "joelho direito". |
| | documento | RG, `VALID`, 2022 | LIT | "RG válido (emissão 2022)". |
| | fato oculto do RG | "RG emitido em 2022, dentro da validade" | INF | A revelação por inspeção vem de `spec.md` US2-2; o briefing v1.0 §8.3 lista o RG entre os "dados visíveis". |
| Camila | idade | 28 | LIT | |
| | perfil | ausente | — | "mulher de 28 anos, mascando chiclete, olhando o celular." |
| | sintoma | "Tontura leve desde ontem" | LIT de `decisions.md`/briefing v1.0 | `dia-1.md` traz a ficha literal "**Tontura chata** desde ontem." (INC-011). |
| | validade do RG como fato oculto | "RG vencido há quatro anos" | LIT (texto) / INF (ocultação) | `spec.md` US2-2 diz que a inspeção revela; o briefing v1.0 §8.3 diz que é visível (INC-012). |
| Lucas | perfil | "Menino de 11 anos." | LIT (resumido) | "menino de 11 anos, mochila escolar nas costas, tentando parecer sério." |
| | sintoma | "Febre (relatada)" | **INF (paráfrase)** | Ficha: "Febre alta repentina e dor no corpo todo." |
| | fatos visíveis | "Pede atestado", "Ficha declara febre" | LIT (resumido) | |
| | fato oculto | `temperature_celsius=36.6` | LIT | T021; briefing v1.0 §8.4 (`revealed_facts`). |
| | marca | `epilogueFlag: lucas_visita_1` | LIT (ID) / INF (modelagem no paciente) | Briefing v1.0 §7.2: "após processar a primeira visita". |
| | obrigatoriedade do termômetro | não modelada | ABERTO | L-005. Nenhuma fonte a define; o termômetro só aparece no diálogo. |
| Helena | perfil | ausente | — | "mulher de 32 anos, aparência cansada, sacola de mercado nas mãos." |
| | fatos visíveis | medicação para asma do filho; receita vencida há dois dias | LIT | |
| | fato oculto | "Mãe solteira com dois empregos" | LIT (resumo) / INF (ocultação) | Briefing v0.1: "O jogador só descobre num pequeno diálogo". |
| Roberto | perfil | ausente | — | "homem de 50 anos, camisa amassada, cheiro de cigarro." |
| | documento | CNH `VALID` | LIT | Aceitação da CNH em aberto (O-004). |
| Todos | `nameVisible` / `identifierVisible` / `expiryDate` | ausentes | LIT (silêncio da fonte) | Não presumidos (O-005). |
| Todos | `dialogueId` | ausente | ABERTO | L-003. As falas existem em `dia-1.md`, fora do repositório. |

### 3.4 Matriz de decisão (`content/decisions/day_1_decision_matrix.json`)

| Caso / ação | Implementado | Origem | Fonte / divergência |
|---|---|---|---|
| Márcio ATENDER | CORRECT, +$5, rep 0, consc 0 | APR/LIT | `decisions.md` §3.1; `dia-1.md`: "Sem impacto em reputação nem consciência". |
| Márcio FILA/ENCAMINHAR/RECUSAR | ERROR, $0, multa $5, log da regra 2 | APR/LIT | §3.1 "Erro em outra ação"; briefing v1.0 §8.4. |
| Márcio, motivo regra 2 | `D1_IMMEDIATE_ACUTE_PAIN` | LIT (log) / ABERTO | INC-004: o joelho não está na lista "abdômen, peito, cabeça". |
| Camila ATENDER | ERROR, multa única, 2 motivos | APR | D-015; §3.2. |
| Camila RECUSAR | CORRECT, +$5 | LIT / ABERTO (comissão) | `spec.md` §7.2 "RECUSAR correta"; O-002. |
| Camila FILA / ENCAMINHAR | UNRESOLVED | ABERTO | O-001/O-015. O briefing v1.0 §8.4 propõe ENCAMINHAR = **PERMITTED** (+$5) e FILA = UNRESOLVED (INC-009). |
| Lucas ATENDER / RECUSAR | ERROR / CORRECT | APR/LIT | §3.3; briefing v1.0 §8.3. `dia-1.md` não menciona o erro de ATENDER. |
| Lucas FILA / ENCAMINHAR | UNRESOLVED | ABERTO | O-001/O-015. O briefing §8.4 propõe ENCAMINHAR = PERMITTED (INC-009). |
| Helena ATENDER | ETHICAL_EXCEPTION, +$5, custo $10, rep −1, consc +1, farmácia, auditoria, `helped` | APR | D-016, D-017; briefing v1.0 §17.1. |
| Helena RECUSAR | CORRECT, +$5, rep +1, consc 0, saída, `refused` | APR (efeitos) / LIT (classificação) | D-018. A classificação CORRECT aparece no esboço do briefing v1.0 §8.4 (`"REFUSE": {"classification": "CORRECT"}`). |
| Helena FILA / ENCAMINHAR | UNRESOLVED | ABERTO | O-001. O briefing §8.4 propõe `REFER_RECONSULTATION` = PERMITTED com `refused` (INC-009). |
| Roberto ATENDER / RECUSAR | ERROR / CORRECT | APR/LIT | §3.5; `spec.md` §7.2. `dia-1.md` registra "Decisão correta: [ ENCAMINHAR / RECUSAR ]". |
| Roberto, motivo regra 3 | `D1_NO_IMMEDIATE_MILD_OR_LASTING` | LIT (briefing v1.0) | O §8.3 diz: "sintoma com mais de três dias e setor inadequado para odontologia". L-004 deixa de ser inferência pura, mas o "setor inadequado" não é regra do folheto. |
| Roberto FILA / ENCAMINHAR | UNRESOLVED | ABERTO | O-001. O briefing §8.4 propõe ENCAMINHAR = PERMITTED e FILA = UNRESOLVED (INC-009). |
| Rep/consc 0 em CORRECT de Camila, Lucas e Roberto | 0 | **INF** | As fontes só dão "+$5"; não dizem que não há efeito. |
| Rep 0 em ERROR | 0 | ABERTO (conduta) | Conduta de O-014; briefing v1.0 §17.3 E: "Não adicionar essa penalidade sem decisão". |
| `specialCostDelta: 0` fora de Helena | 0 | INF (trivial) | Nenhuma fonte cita custo para esses casos. |
| `reasonRuleIds` em resultados CORRECT | regras justificadoras | **INF** | As fontes só dão motivos e logs para erros. |
| Cobertura das 4 ações por caso | obrigatória | INF técnica | Garante que nenhuma ação caia em default. |

### 3.5 Registros de auditoria

| ID | Texto implementado | Origem | Fonte |
|---|---|---|---|
| `AUD_D1_ACUTE_PAIN_MUST_BE_ATTENDED` | "Dor aguda deve ser atendida na hora (regra 2)" | LIT | §3.1; briefing §8.3. |
| `AUD_D1_DOCUMENT_EXPIRED` + `AUD_D1_SYMPTOM_NO_IMMEDIATE` | "Documento vencido (regra 1)" + "Sintoma sem direito a atendimento imediato (regra 3)" | **INF (divisão e redação)** | O briefing §8.3 tem um texto único: "RG vencido (regra 1) e sintoma sem direito a atendimento imediato (regra 3)". O esboço §8.4 usa dois IDs (`expired_id`, `no_immediate_care`). |
| `AUD_D1_ATTESTATION_NORMAL_TEMPERATURE` | "Atestado sem sintoma agudo e temperatura normal (regra 3)" | LIT | §3.3; briefing §8.3. |
| `AUD_D1_EXPIRED_PRESCRIPTION_RELEASED` | "Receita vencida liberada" | LIT/APR | D-017. |
| `AUD_D1_WRONG_SECTOR_DENTAL` | "Triagem em setor errado: dor crônica vai para a Odontologia" | LIT | §3.5; briefing §8.3. |
| IDs `AUD_…` | — | INF técnica | O esboço do briefing usa outros IDs. |

### 3.6 Destinos

| Destino | Implementado | Origem | Fonte |
|---|---|---|---|
| Farmácia do hospital (Helena ajudada) | `hospital_pharmacy` | LIT/APR | §3.4; briefing §8.3. |
| Saída (Helena segue o protocolo) | `exit` | LIT | §3.4. |
| Cadeiras de espera (FILA) | `waiting_chairs` | APR | D-020. |
| **Hospital (ATENDER), saída (RECUSAR) e hospital para ATENDER incorreto, nos demais pacientes** | **ausentes** (L-002) | LIT no briefing v1.0 | §8.3: Márcio "atendido → hospital; fila → cadeiras de espera; recusa → saída"; Camila, Lucas e Roberto "saída em recusa/encaminhamento; hospital se atendido incorretamente". INC-010. |

### 3.7 Falas

| Item | Implementado | Origem | Fonte literal |
|---|---|---|---|
| Ralel (3 falas) | nenhuma | ABERTO (L-003) | `dia-1.md` §1; o briefing v1.0 §12.1 diz que "devem ser preservadas". |
| Diálogos dos 5 pacientes | nenhum | ABERTO (L-003) | `dia-1.md` §4. |
| Falas de saída das decisões corretas e de Helena (2) | nenhuma | ABERTO (L-003) | `dia-1.md` §4: Márcio "Obrigado, doutor."; Camila "Que burocracia ridícula. Vou no posto do bairro."; Lucas "Poxa... valeu a tentativa. Volto depois de amanhã."; Helena (recusa e ajuda); Roberto "Perdi meu tempo aqui...". |
| Falas alternativas (reconsulta de Helena, encaminhamento de Roberto, renovação de Camila, enfermaria de Lucas) | nenhuma | PROP | P-012; briefing §8.3. |
| Texto do evento do cachorro | não criado (T037) | LIT disponível | `dia-1.md` §8; briefing §10.6. |

### 3.8 Decisões técnicas embutidas no código

| Item | Implementado | Origem | Observação |
|---|---|---|---|
| Ponto neutro = `0` | `GameState.NeutralMetricValue` | INF (representação de D-003) | D-003 não fixa um número. O save de exemplo do briefing usa inteiros. |
| `GameState` como classe C# pura, sem Autoload | — | INF | O briefing v1.0 §18.4 recomenda "Autoload `GameState` como uma classe derivada de `Node`" (INC-013). |
| Fases `NotStarted` / `InDay` / `DayClosed` | enum `CampaignPhase` | INF | O briefing v1.0 §7.1 lista `day_phase`: menu, handover, shift, summary, between-shifts, completed (INC-013). |
| `processedPatientIds` zerado a cada dia | — | INF | Necessário porque Lucas volta com o mesmo ID. |
| Marcas do resultado gravadas em `narrativeFlags` | — | INF | O briefing distingue `patient_flags` e `narrative_flags` sem dizer onde gravar cada marca. |
| Chaves JSON em camelCase | — | LIT de `data-model.md` | O briefing v1.0 usa snake_case nos exemplos (INC-006). |
| Saldo inicial explícito em `CreateNew` | — | ABERTO (L-001) | Sem default. |
| Campos inteiros não anuláveis de `TriageResult` (padrão 0) | — | INF técnica (risco) | Na Fase III, o `TriageResult` deve ser montado só a partir de `ActionOutcome` validado, para que o 0 nunca venha de um campo ausente. |

## 4. Diferenças entre as fontes e o conteúdo implementado

1. **Textos do protocolo:** o conteúdo usa a paráfrase de `spec.md` §7.1, não o folheto literal de `dia-1.md` §2. O bloco "[ ATENÇÃO ]" e a marca "(Encaminhar/Recusar)" do grupo 3 estão ausentes.
2. **Estrutura das regras:** 7 sub-regras com IDs `D1_…` versus 3 regras (`document_validity`, `acute_symptoms`, `no_immediate_care`) no briefing v1.0.
3. **Destinos:** o briefing v1.0 §8.3 define hospital, cadeiras de espera e saída por paciente; o conteúdo só tem os de Helena (INC-010, L-002).
4. **Falas:** todas as falas literais existem em `dia-1.md`; o conteúdo não tem nenhuma (L-003).
5. **Perfis:** os perfis literais de Camila, Helena e Roberto foram omitidos; os de Márcio e Lucas estão resumidos.
6. **Sintomas:** Lucas tem "Febre (relatada)" em vez de "Febre alta repentina e dor no corpo todo". Para Camila, as fontes divergem entre si ("chata" em `dia-1.md`, "leve" nas demais) (INC-011).
7. **Visibilidade da validade do RG** (Márcio, Camila): oculta até a inspeção (`spec.md`) versus visível (briefing §8.3) (INC-012).
8. **Log de Camila:** dois textos redigidos pelo agente versus um texto literal no briefing.
9. **ENCAMINHAR como PERMITTED** para Camila, Lucas, Roberto e Helena (reconsulta) no esboço do briefing v1.0 §8.4, versus UNRESOLVED no conteúdo, que segue O-001/D-021 (INC-009).
10. **Marca de Helena:** `helena_outcome` está no briefing v1.0 (§7.2, §17.1) e em FR-015; só `decisions.md` §3.4 usa `helena=recusada/ajudada` (INC-002).
11. **Retorno de Lucas nos Dias 3, 5 e 7:** fechado no briefing v1.0 §17.1, em aberto em O-013, `[PROPOSTA]` no briefing v0.1 (INC-008). A fala "Volto depois de amanhã" é coerente com o Dia 3.
12. **Tipo das marcas:** o save de exemplo usa booleano (`lucas_visita_1: true`); o conteúdo usa texto.
13. **`GameState`:** Node/Autoload (briefing §18.4) versus classe pura; fases resumidas (INC-013).
14. **Valores econômicos:** no protocolo (briefing §6.4) versus em `DayConfig`.
15. **Chaves JSON:** snake_case (briefing) versus camelCase (`data-model.md`).
16. **Comissão no folheto:** "Comissão: $5 por paciente correto" (`dia-1.md`) versus "$5 por paciente atendido" (briefing v0.1). Confirma O-002.
17. **Rótulos dos carimbos em `dia-1.md`:** `[ ATENDER ] | [ RECOMENDAR/FILA ] | [ RECUSAR ]`, com cores no briefing §10.3. Os rótulos de decisão "[ ENCAMINHAR PARA RENOVAÇÃO ]" (Camila) e "[ ENCAMINHAR / RECUSAR ]" (Roberto) aparecem em `dia-1.md` sem carimbo correspondente. Isso confirma O-001.
18. **Termômetro de Lucas:** a fonte só mostra a leitura dentro do diálogo; não diz se é obrigatória (L-005 continua aberta).
19. **Saldo inicial:** o resumo de exemplo de `dia-1.md` §7 mostra "SALDO DO DIA +$50" igual a "SALDO ACUMULADO +$50", o que sugere saldo inicial $0. Porém o briefing v1.0 §9.2 diz que esses números "são exemplos de um plantão típico, não metas rígidas". **Não é decisão**; L-001 continua aberta.

## 5. Inferências que precisam ser removidas, corrigidas ou confirmadas

| # | Inferência | Recomendação |
|---|---|---|
| 1 | Textos do protocolo parafraseados | **Corrigir** para o texto literal do folheto, após autorização para trazer `dia-1.md` ao repositório. |
| 2 | Divisão em 7 regras, IDs `D1_…` e categorias | **Confirmar** ou realinhar com os 3 IDs do briefing. A divisão permite citar o subitem exato no log. |
| 3 | `reasonRuleIds` em resultados CORRECT | **Confirmar** (são justificativas, sem efeito financeiro) ou remover. |
| 4 | Rep/consc 0 nos resultados CORRECT de Camila, Lucas e Roberto | **Confirmar** com o autor. Se não houver confirmação, marcar com pendência explícita. |
| 5 | `specialCostDelta: 0` fora de Helena | Confirmar (baixo risco). |
| 6 | Divisão e redação do log de Camila | **Corrigir** para o texto do briefing, mantendo ou não dois IDs. |
| 7 | Sintomas parafraseados (Lucas, Márcio) | **Corrigir** para o texto literal da ficha. |
| 8 | Perfis omitidos ou resumidos | **Corrigir** com os perfis literais de `dia-1.md`. |
| 9 | Fatos visíveis versus ocultos (RG de Márcio e Camila; revelação de Helena) | **Confirmar**: há conflito entre `spec.md` e o briefing (INC-012). |
| 10 | `dog_adopted = "false"` como valor permitido | **Confirmar** se a não adoção grava `false` ou não grava nada. |
| 11 | Marcas como texto em vez de booleano | Decisão técnica; confirmar antes de T039 (save). |
| 12 | Helena RECUSAR = CORRECT | Tem apoio no esboço §8.4. **Confirmar** e registrar como decisão. |
| 13 | Vínculo de Roberto à regra 3 (L-004) | Tem apoio no briefing §8.3. **Confirmar** e fechar L-004 citando a fonte. |
| 14 | `GameState` puro, sem Autoload; fases resumidas | Técnica. Na Fase III, envolver em um Node Autoload e ampliar as fases conforme o briefing §7.1, sem mudar o domínio. |
| 15 | Ponto neutro = 0 | Técnica; confirmar. |
| 16 | Destinos ausentes (L-002) | **Corrigir** com o briefing §8.3 após confirmação. Os destinos de FILA e ENCAMINHAR continuam pendentes (briefing §17.3 L). |

## 6. Pendências que bloqueiam a Fase III (T013–T017, US1)

| Pendência | Tarefa afetada | Por quê |
|---|---|---|
| **L-001 — saldo inicial** | T013 (nova campanha), T014 (cabeçalho com saldo) | `GameState.CreateNew` exige o valor; não há default. |
| **L-003 — importar `dia-1.md` (falas de Ralel)** | T015 (passagem de turno) | As três falas existem na fonte, mas não estão no repositório nem autorizadas como conteúdo. |
| **Texto do folheto (item 1 do §5) e exibição do bloco "[ ATENÇÃO ]"** | T016 (`ProtocolPanel`) | O bloco literal afirma "Comissão: $5 por paciente correto", o que toma partido em O-002 se for exibido sem ressalva. |
| **Ausência de `quickstart.md`** | T017 (só a parte de smoke test) | Não impede o teste de fluxo; impede alinhá-lo ao roteiro previsto. |
| **Fases de dia/turno (INC-013)** | T015 | Técnica: precisa da fase "handover". Pode ser resolvida na própria Fase III, sem decisão de design. |

## 7. Pendências que podem ficar abertas sem impedir um protótipo visual

- O-001/O-015 (FILA versus ENCAMINHAR) e INC-009: os quatro slots podem ser desenhados, desabilitando o commit nas ações `UNRESOLVED`. Bloqueiam o aceite da Fase "Decisão por paciente" (T018–T025).
- O-002 (gatilho da comissão): bloqueia o aceite financeiro (T032–T036), não a interface.
- O-003, O-004, O-005, INC-004: afetam `ProtocolManager` e o gerador de rotina (T018, T027).
- O-006 (dívida), O-007 (ração), O-009 (intercalação), O-010, O-011, O-012, O-013/INC-008, O-014.
- O-008 (o relógio corre durante a inspeção?): afeta T029, não US1.
- L-002/INC-010 (destinos), L-004, L-005, INC-002, INC-011, INC-012.
- INC-003, INC-005, INC-006 (nomenclatura técnica).
