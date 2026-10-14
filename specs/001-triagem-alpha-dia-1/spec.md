# Especificação Funcional — Alpha Demo de Triagem: Dia 1

**Feature:** `001-triagem-alpha-dia-1`  
**Criado em:** 2026-10-14  
**Estado:** Draft para revisão de design  
**Fonte:** `briefing-triagem-alpha-demo-v1.0.md` e `dia-1.md` v0.2

## 1. Visão geral

Triagem é um jogo narrativo de decisões burocráticas em um hospital. Durante um plantão curto, o jogador recebe pacientes, confere ficha, documento e sintomas, consulta um protocolo diário, escolhe uma ação e recebe consequências financeiras, administrativas e narrativas.

A alpha demo deve provar o loop completo do Dia 1 — chegada, inspeção, decisão, consequência, atualização do estado e próximo paciente — sem exigir o conteúdo jogável da campanha inteira de sete dias.

## 2. Objetivos

- Demonstrar a experiência de ser triador sob pressão de tempo.
- Ensinar o jogador a consultar e aplicar o protocolo do Dia 1.
- Fazer as decisões parecerem concretas por meio de consequências imediatas e de um resumo do plantão.
- Provar que pacientes narrativos fixos e pacientes de rotina podem coexistir sem quebrar a ordem narrativa.
- Preservar reputação, consciência, economia e marcas narrativas para a continuidade da campanha.
- Entregar uma fatia pequena, completa e verificável, antes de ampliar os sete dias.

## 3. Não objetivos

- Simular medicina real ou oferecer orientação clínica real.
- Incluir multiplayer, backend, recursos online, 3D, dublagem ou editor de conteúdo.
- Implementar todos os sete dias como conteúdo jogável na alpha.
- Exigir que o jogador retome o meio de um plantão após fechar o jogo.
- Entregar arte final ou animações complexas antes de validar o loop principal.

## 4. Atores e contexto

- **Jogador:** triador do hospital, responsável por examinar os dados e escolher uma ação.
- **Ralel:** colega do turno anterior; faz a passagem de plantão e introduz as validações do dia. Não possui arco próprio nem final.
- **Pacientes narrativos:** Márcio Souza, Camila Torres, Lucas Mendes, Helena Rios e Roberto Paiva.
- **Pacientes de rotina:** casos gerados por combinação controlada de sintoma, documento e traço de personalidade.
- **Administração do hospital:** representada indiretamente por protocolo, auditoria, comissões, multas e reputação.

## 5. Histórias de usuário e testes independentes

### US1 — Iniciar o plantão e consultar o protocolo (Prioridade P1)

Como jogador, quero iniciar o Dia 1, ouvir/ler a passagem de turno de Ralel e consultar o folheto para saber quais validações aplicar.

**Valor:** cria contexto e permite que as decisões sejam fundamentadas nas regras exibidas.

**Teste independente:** iniciar uma campanha nova e verificar que a passagem de turno, o protocolo completo e a cena do guichê são apresentados antes da primeira decisão.

**Cenários de aceitação:**

1. Dado que não existe save válido, quando o jogador escolhe iniciar, então o jogo cria uma campanha no Dia 1.
2. Dado que o Dia 1 começa, quando a passagem de turno é concluída, então Ralel e o folheto do protocolo são apresentados.
3. Dado que o jogador abre o folheto, quando consulta as regras, então vê documento válido, sintomas para atendimento imediato e sintomas sem direito a atendimento imediato conforme `dia-1.md`.

### US2 — Inspecionar e decidir sobre pacientes narrativos (Prioridade P1)

Como jogador, quero ler a ficha e inspecionar documentos/sintomas para escolher a ação compatível com a matriz de decisões.

**Valor:** prova o núcleo de observação e decisão do jogo.

**Teste independente:** carregar cada paciente narrativo, executar interações documentadas, testar cada ação classificada e comparar o resultado com a matriz.

**Cenários de aceitação:**

1. Dado que um paciente chegou, quando sua ficha é apresentada, então os dados visíveis do briefing são mostrados.
2. Dado que o jogador inspeciona o RG de Márcio ou Camila, quando a inspeção termina, então a validade correspondente é revelada.
3. Dado que Lucas está no guichê, quando o jogador consulta o termômetro, então a temperatura revelada é 36,6 °C.
4. Dado que o jogador escolhe uma ação mapeada pela matriz, quando confirma a decisão, então a classificação, o log, os efeitos e o destino visual correspondem à matriz aprovada.
5. Dada uma ação cuja semântica permanece não resolvida, quando o jogador tenta confirmá-la, então o sistema não aplica silenciosamente uma consequência definitiva; identifica o caso como não resolvido.

### US3 — Processar pacientes de rotina sob limite de tempo (Prioridade P1)

Como jogador, quero receber pacientes de rotina entre os casos narrativos para sentir a pressão do plantão sem perder a sequência dos casos de história.

**Valor:** demonstra o relógio e o ritmo central sem exigir grande quantidade de conteúdo manual.

**Teste independente:** executar o turno com seed fixa, verificar os casos gerados, o fim do relógio-base e a passagem dos casos narrativos restantes.

**Cenários de aceitação:**

1. Dado que o relógio está dentro dos cinco minutos-base, quando há espaço no fluxo, então pacientes de rotina podem ser gerados.
2. Dado que o relógio-base expirou, quando o sistema decide qual paciente inserir, então não cria novos pacientes de rotina.
3. Dado que ainda há pacientes narrativos pendentes após a expiração, quando o atual termina, então o próximo paciente narrativo pendente ainda aparece.
4. Dado que há pacientes de história, quando a sequência avança, então a ordem relativa Márcio → Camila → Lucas → Helena → Roberto é preservada.
5. Dado que existe uma combinação de rotina que exigiria escolher precedência entre regras ainda abertas, quando o gerador valida o caso, então rejeita essa combinação ou a marca para revisão em vez de inventar resultado.

### US4 — Receber consequências e fechar o resumo diário (Prioridade P1)

Como jogador, quero ver como minhas decisões afetam comissão, multas, despesas, reputação e consciência.

**Valor:** transforma cada decisão em consequência mensurável e liga o loop de guichê à campanha.

**Teste independente:** simular um conjunto conhecido de decisões e comparar todos os lançamentos e métricas com resultados esperados definidos na matriz.

**Cenários de aceitação:**

1. Dado um erro genérico, quando suas consequências são aplicadas, então a comissão daquele paciente é zero e existe uma única multa de $5, mesmo se houver múltiplos motivos no log.
2. Dado que Helena é ajudada, quando o resultado é lançado, então há comissão de $5 e custo de $10, líquido −$5, reputação −1 e consciência +1, sem multa genérica adicional.
3. Dado que Helena segue o protocolo, quando o resultado é lançado, então comissão de $5, reputação +1 e consciência +0 são registrados.
4. Dado que o dia termina sem erros genéricos, quando o resumo é calculado, então o bônus de reputação +1 é aplicado; a escolha ética de Helena não o cancela.
5. Dado que o dia terminou, quando o resumo é exibido, então apresenta pacientes processados, decisões, comissão, multas, custos éticos, despesa fixa de $25, saldo do dia, saldo acumulado/dívida, reputação, consciência e evento seguinte.
6. Dado que uma decisão foi contabilizada, quando o resumo é montado, então seus lançamentos não são aplicados uma segunda vez.

**Nota de requisito aberto:** o briefing geral define comissão por paciente atendido, enquanto o folheto/matriz do Dia 1 define comissão por decisão correta/permitida. O requisito financeiro global só pode ser marcado como concluído quando essa divergência for resolvida.

### US5 — Concluir o Dia 1, decidir sobre o cachorro e preservar progresso (Prioridade P1)

Como jogador, quero resolver o evento narrativo entre turnos e manter minhas decisões para a continuidade da campanha.

**Valor:** dá fechamento emocional ao Dia 1 e prova continuidade/persistência.

**Teste independente:** concluir o plantão, escolher uma opção sobre o cachorro, fechar o jogo e reabrir com o checkpoint salvo.

**Cenários de aceitação:**

1. Dado que o resumo foi concluído, quando o evento narrativo é aberto, então o jogador pode levar o filhote ou continuar caminhando.
2. Dado que o jogador adota o filhote, quando confirma a escolha, então `dog_adopted` passa a verdadeiro.
3. Dado que o jogador não adota, quando confirma a escolha, então não há despesa recorrente do cachorro e o cachorro não reaparece segundo o briefing.
4. Dado que o evento terminou, quando o autosave é executado, então o snapshot do Dia 1 e as marcas narrativas são persistidos localmente.
5. Dado que o save está ausente ou inválido, quando o jogo inicia, então o jogador pode iniciar nova campanha sem bloqueio permanente.

## 6. Requisitos funcionais

- **FR-001:** O sistema deve iniciar uma campanha no Dia 1 e apresentar a passagem de turno de Ralel.
- **FR-002:** O sistema deve apresentar e permitir consultar as três regras do protocolo do Dia 1: validade documental; sintomas aceitos para atendimento imediato; sintomas sem direito a atendimento imediato.
- **FR-003:** O sistema deve apresentar os cinco pacientes narrativos de Dia 1 com os dados, diálogos e interações definidos na fonte.
- **FR-004:** O sistema deve preservar a ordem relativa dos cinco pacientes narrativos, mesmo quando pacientes de rotina forem intercalados.
- **FR-005:** O sistema deve gerar pacientes de rotina a partir de pools de nomes/idades, sintomas, estados documentais e traços de personalidade. A proporção de aproximadamente 60/40 permanece `[PROPOSTA]`.
- **FR-006:** O sistema deve usar um relógio-base de cinco minutos reais.
- **FR-007:** Quando o relógio-base expirar, o sistema deve parar a geração de pacientes de rotina e continuar apresentando pacientes narrativos pendentes.
- **FR-008:** A alpha deve representar os quatro controles solicitados: ATENDER, FILA, ENCAMINHAR e RECUSAR. A correspondência definitiva entre esses controles e o rótulo legado RECOMENDAR/FILA permanece `[EM ABERTO]`.
- **FR-009:** O sistema deve classificar ações como `CORRECT`, `PERMITTED`, `ERROR`, `ETHICAL_EXCEPTION` ou `UNRESOLVED`.
- **FR-010:** Uma decisão `UNRESOLVED` não deve receber consequência definitiva por padrão.
- **FR-011:** Para um erro genérico, o sistema deve remover a comissão do paciente e lançar uma única multa de $5 por paciente; o log pode registrar vários motivos.
- **FR-012:** A escolha de ajudar Helena deve aplicar +$5 de comissão, custo ético de $10, reputação −1 e consciência +1, sem multa genérica adicional.
- **FR-013:** Seguir o protocolo no dilema da Helena deve aplicar comissão de $5, reputação +1 e consciência +0.
- **FR-014:** A escolha ética da Helena não deve contar como erro genérico para o bônus diário de reputação +1, aprovado nesta conversa.
- **FR-015:** O sistema deve registrar `lucas_visita_1`, `helena_outcome=refused/helped` e `dog_adopted` conforme os eventos correspondentes. Outras marcas ainda não definidas não devem ser inventadas.
- **FR-016:** O resumo deve mostrar estatísticas do plantão, finanças, métricas e evento narrativo seguinte.
- **FR-017:** A despesa fixa de $25 deve ser aplicada uma única vez ao fechamento do dia.
- **FR-018:** O sistema deve mostrar o evento do cachorro e registrar a escolha do jogador.
- **FR-019:** O estado concluído do Dia 1 deve ser salvo/carregado em armazenamento local serializado.
- **FR-020:** O conteúdo deve ser validado antes do uso; arquivo ausente, IDs duplicados e regras incompletas devem produzir erro de validação compreensível, não erro de gameplay.
- **FR-021:** A campanha deve conservar os campos necessários a reputação, consciência, dinheiro, dívida e marcas de epílogo para permitir a continuação futura dos Dias 2–7; a alpha não precisa conter esses dias como conteúdo jogável.
- **FR-022:** O modo de falha/recuperação do save e do conteúdo inválido não deve impedir permanentemente que uma campanha nova seja iniciada.

## 7. Regras de negócio do Dia 1

### 7.1 Protocolo ficcional

1. Documento: deve conter nome e RG/CPF visível e estar dentro da validade.
2. Sintomas para atendimento imediato: febre acima de 38 °C; dores intensas/agudas (abdômen, peito, cabeça); ferimentos com sangramento ativo.
3. Sintomas sem direito a atendimento imediato: pedido de atestado sem sintoma agudo; tontura leve/cansaço/sintomas com mais de três dias; receita vencida ou sem assinatura.

A precedência entre documento inválido e sintoma agudo permanece `[EM ABERTO]`. A aceitação de CNH como equivalente a RG/CPF também permanece `[EM ABERTO]`.

### 7.2 Matriz narrativa resumida

| ID | Evidências principais | Decisão consolidada na matriz atual | Consequências fechadas/conhecidas | Pendências específicas |
|---|---|---|---|---|
| `marcio_souza` | 40 anos; queda; dor aguda no joelho e inchaço; RG válido, emissão 2022 | ATENDER é a única ação sem erro | ATENDER +$5; sem alteração direta de reputação/consciência; demais ações com erro genérico e multa de $5 | marca de epílogo; falas alternativas são propostas; tutorial pós-erro é proposta |
| `camila_torres` | 28 anos; tontura leve; RG vencido há quatro anos | RECUSAR correta; encaminhar para renovação/RECOMENDAR legado permitido | sem erro +$5; ATENDER é erro genérico com multa única de $5 e log contendo motivos de documento e sintoma | mapear legado para FILA/ENCAMINHAR; marca de epílogo; fala alternativa proposta |
| `lucas_mendes` | 11 anos; pedido de atestado; RG válido; ficha declara febre; termômetro revela 36,6 °C | RECUSAR correta; RECOMENDAR legado permitido na matriz revisada | sem erro +$5; ATENDER é erro genérico com multa única de $5; marca `lucas_visita_1`; retorna nos dias 3, 5 e 7 independentemente da escolha inicial | destino exato do encaminhamento; fala alternativa proposta; se a inspeção do termômetro é obrigatória antes da decisão |
| `helena_rios` | 32 anos; RG válido; medicamento contínuo para asma do filho; receita vencida há dois dias | seguir protocolo por RECUSAR ou recomendar reconsulta; ATENDER/LIBERAR é exceção ética | seguir protocolo: +$5, reputação +1, consciência +0; ajudar: +$5 e −$10, reputação −1, consciência +1, sem multa genérica adicional; marca refused/helped; auditoria se ajudada | nome exato do ID de marca pode ser normalizado para `helena_outcome`; garantir correspondência em dados e testes |
| `roberto_paiva` | 50 anos; dor de dente há duas semanas; CNH válida | RECUSAR correta; encaminhar ao Posto Central/RECOMENDAR legado permitido | sem erro +$5; ATENDER é erro genérico com multa única de $5 e log para odontologia | CNH aceita como documento? mapear legado para FILA/ENCAMINHAR; marca de epílogo; fala alternativa proposta |

### 7.3 Consequências globais já aprovadas

- Uma multa genérica de $5 por paciente com erro, mesmo que haja mais de um motivo de erro.
- Helena ajudada: comissão de $5 e custo de $10, líquido −$5; nenhuma multa genérica adicional.
- O dilema ético de Helena não conta como erro genérico para o bônus diário.
- Bônus de reputação +1 quando o dia termina sem erros genéricos.
- FILA é uma decisão final e possui destino visual próprio; a semântica global FILA versus ENCAMINHAR continua aberta.

### 7.4 Valores econômicos e propostas

- Comissão nominal: $5 por decisão correta/permitida na matriz do Dia 1; o gatilho global da comissão está em conflito entre fontes e continua aberto.
- Multa: $5 por paciente com erro, aprovada.
- Despesa fixa: $25 ao dia.
- Escolha ética da Helena: custo $10, aprovado.
- Turno prolongado: −1 reputação `[PROPOSTA]`.
- Geração de rotina: aproximadamente 60% casos para atender e 40% para recusar/encaminhar `[PROPOSTA]`.
- Ração do cachorro: $2/dia em `dia-1.md`, mas o briefing geral também lista $5/dia; valor final `[EM ABERTO]`.

## 8. Conteúdo narrativo obrigatório

### Ralel

A passagem de turno inclui as falas de `dia-1.md`: o turno anterior foi caótico, o folheto do protocolo foi deixado na mesa, a direção acompanha erros e Ralel tentará dormir após a obra. Ralel aparece no início de cada dia da campanha futura, sem arco/final próprio.

### Lucas Mendes

Menino de 11 anos; primeira tentativa de conseguir atestado para faltar à Educação Física; retorna nos Dias 3, 5 e 7 conforme matriz revisada. A história termina com revelação de bullying no Dia 7. A decisão do Dia 1 não altera que ele volte, embora pistas e falas futuras ainda dependam de conteúdo posterior.

### Helena Rios

Mãe solteira, dois empregos, busca a bombinha de asma do filho com receita vencida. A revelação ocorre em diálogo curto. As consequências da escolha e a marca reaparecem no epílogo; Helena não volta como paciente.

### Cachorro

Após o resumo, o jogador encontra um filhote sob chuva e decide levar ou deixar. Adotar registra `dog_adopted=true`, habilita consequências futuras de ansiedade relacionadas à hora extra e garante uma linha narrativa do mascote conforme o briefing de Dia 1. O custo da ração é `[EM ABERTO]` ($2 no `dia-1.md`, $2 ou $5 no briefing geral). A hora extra só aparece no Dia 5 e concede +$20; se aceita com cachorro adotado, causa despesa de $12 por móvel destruído e cansaço no dia seguinte, conforme o briefing. Esses eventos futuros podem ter dados/preparação, mas não fazem parte do conteúdo jogável da alpha do Dia 1.

## 9. Requisitos de experiência

- O posto é um guichê 2D fixo.
- Ficha, documento e protocolo devem ser legíveis e consultáveis.
- As ações têm feedback visual próprio; sons mínimos são opcionais durante o primeiro teste funcional.
- Os pacientes têm retrato simples/pixel art própria ou arte provisória.
- O evento do cachorro pode ser uma tela narrativa, sem cenário explorável adicional.
- A fadiga, quando implementada em dia futuro, afeta visualmente a leitura; opção para reduzir intensidade é `[PROPOSTA]`.

## 10. Métricas e condições de sucesso

A alpha passa quando:

- O jogador consegue iniciar e concluir o Dia 1 sem bloqueio permanente.
- Os cinco pacientes narrativos aparecem em sua ordem relativa.
- A inspeção de documentos e de temperatura de Lucas funciona.
- Cada decisão já mapeada produz classificação e efeitos esperados.
- Toda ação não resolvida permanece explícita e não aplica efeitos silenciosamente.
- A multa de erro nunca duplica por paciente.
- O dilema da Helena e o bônus diário respeitam as decisões aprovadas.
- O resumo mostra as finanças, as métricas e o evento seguinte.
- A escolha do cachorro e as flags sobrevivem ao save/load.
- A seed de rotina reproduz a mesma sequência para facilitar QA.
- As pendências bloqueadoras são listadas no relatório de aceite; não se declara validade da matriz completa enquanto comissão global ou mapeamentos de ação afetarem casos não resolvidos.

## 11. Dados e entidades conceituais

- `DayConfig`: configuração do dia, relógio-base e IDs de conteúdo.
- `PatientData`: perfil, dados visíveis, evidências, documento, sintomas, diálogos, interações e flags.
- `DocumentData`: tipo, nome/identificador, validade e estado revelado.
- `ProtocolData` / `ProtocolRule`: regras e grupos do protocolo.
- `TriageAction`: ID de ação independente do rótulo exibido.
- `DecisionCase` / `ActionConsequence`: classificação, motivos, efeitos e destino para uma ação.
- `TriageResult`: registro imutável de uma decisão resolvida.
- `DailyFinancialResult`: comissões, multas, custos, despesas e saldo calculados.
- `DaySummary`: estatísticas e estado após fechamento.
- `NarrativeFlag`: marca de continuidade.
- `GameSaveData`: snapshot persistível da campanha.

## 12. Restrições e decisões em aberto

1. **Ações:** o rascunho do Dia 1 tem `ATENDER`, `RECOMENDAR/FILA`, `RECUSAR`; o escopo técnico pede `ATENDER`, `FILA`, `ENCAMINHAR`, `RECUSAR`. O usuário adiou a escolha entre quatro ações independentes ou ação agrupada. A FILA como decisão final foi aprovada, mas não resolve o mapeamento global.
2. **Comissão:** comissão por paciente atendido no briefing geral versus comissão por decisão correta/permitida na matriz do Dia 1.
3. **Precedência:** documento inválido versus sintoma agudo; outras combinações em conflito.
4. **Documento:** a CNH de Roberto pode ou não substituir RG/CPF; documentos sem data explícita de vencimento.
5. **Reputação:** não foi aprovada penalidade geral imediata por erros comuns; só o bônus diário sem erros genéricos foi aprovado. −1 por turno prolongado continua proposta.
6. **Dívida:** saldo negativo acumulado e variável separada de dívida exigem um único modelo aritmético que não conte o déficit duas vezes.
7. **Cachorro:** ração de $2 ou $5/dia; o `dia-1.md` informa $2 e o briefing geral deixa $2 ou $5.
8. **Fila de rotina:** algoritmo de intercalação e comportamento do relógio durante diálogo/inspeção.
9. **Marcas narrativas:** não foram definidas para Márcio, Camila e Roberto; não inventar.
10. **Contagem da campanha:** o briefing propõe 28 pacientes escritos à mão (quatro por dia), enquanto o Dia 1 atual tem cinco; revisar antes de fechar o conteúdo da campanha inteira.
11. **Finais:** quatro finais e rótulos são `[PROPOSTA]`; reputação/consciência começam neutras e “alta” significa acima do valor neutro ao final do Dia 7.
12. **Outros conteúdos futuros:** distribuição final dos dilemas, pistas de Lucas, retorno do idoso no Dia 6, e opções visuais de acessibilidade continuam conforme o status original do briefing.

## 13. Dependências de decisão para o aceite final

A implementação da UI, carregador, sequência de pacientes, diálogo, save e componentes isolados pode avançar. O aceite de ponta a ponta para decisões afetadas depende de resolver comissão e mapeamento das ações. Até lá, usar `UNRESOLVED` e registrar os casos afetados no relatório de validação.
