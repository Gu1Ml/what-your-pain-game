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
| D-005 | O garoto do atestado é um menino sozinho e tenta obter atestado em quatro ocasiões; o motivo real de bullying é revelado no Dia 7. | FECHADA quanto ao arco central | O briefing marca a distribuição exata dias 1, 3, 5 e 7 como `[PROPOSTA]`; não promovê-la a regra fechada sem confirmação específica. |
| D-006 | O Dia 1 contém cinco pacientes narrativos em ordem relativa: Márcio Souza, Camila Torres, Lucas Mendes, Helena Rios e Roberto Paiva. | FECHADA | `dia-1.md` v0.2. Pacientes de rotina podem ser intercalados sem mudar a ordem relativa. |
| D-007 | O turno tem 300 segundos de tempo-base; após o tempo-base, param novos pacientes de rotina, mas pacientes narrativos pendentes ainda aparecem. | FECHADA | `dia-1.md` v0.2. Duração prática de 5–7 minutos decorre da conclusão dos casos de história. |
| D-008 | Despesa diária de subsistência: $25. | FECHADA | Folheto do Dia 1. |
| D-009 | Comissão nominal do folheto do Dia 1: $5. | FECHADA como valor; gatilho global ainda aberto | Divergência entre “por paciente atendido” no briefing e “por paciente correto/decisão correta” na matriz. |
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
| D-018 | Seguir o protocolo no caso da Helena dá reputação +1 e consciência +0. | FECHADA | A comissão é $5 conforme a matriz do Dia 1. |
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
| O-002 | Gatilho global da comissão: paciente atendido ou decisão correta/permitida. | Economia, resumo e testes. | Não fechar a contabilidade global. Implementar ledger e política configurável/testável, sem declarar aceitação financeira completa. |
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
| O-013 | Se Lucas deve aparecer exatamente nos dias 1, 3, 5 e 7. | Cronologia da campanha. | O arco central é fechado; a sequência exata foi marcada como proposta no briefing original e deve ser confirmada se houver conflito com a especificação atual. |
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
