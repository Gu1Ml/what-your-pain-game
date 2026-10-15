# Triagem — Briefing (v0.1)

> Itens marcados com **[PROPOSTA]** são sugestões minhas para você aceitar, trocar ou cortar. O resto vem das suas decisões.

## Experiência
Você é o triador de um hospital em plantão. Chegam muitos pacientes, cada um com um problema, um documento e uma história que pode ser verdadeira, exagerada ou absurda. Você tem poucos minutos para decidir quem entra, quem espera e quem é encaminhado. O jogador deve sentir **pressão de tempo, pequenas injustiças e riso nervoso**, com o humor vindo das situações e dos diálogos, como em *Papers, Please*.

## Mecânica central
Para cada paciente, em ordem:
1. Ele chega ao guichê e conta o que sente.
2. Você confere ficha, sintomas e documentos contra o **protocolo do dia**.
3. Você decide: **atender agora / pôr na fila / encaminhar / recusar**.
4. Você recebe consequência imediata (aceito, erro, reclamação) e o próximo paciente entra.

## Loop do turno
- O relógio base é de **5 minutos reais**, e o turno dura de 5 a 7 minutos na prática.
- O jogador atende o máximo que conseguir: os pacientes de rotina são gerados até o relógio acabar.
- O turno só termina depois que todos os pacientes de história do dia (os escritos à mão, com impacto na narrativa) tiverem passado. Passados os 5 minutos, não entram mais pacientes de rotina, só os de história que faltam. Assim, quem lê devagar não perde parte da história.
- **Atendimento lento:** se o turno precisou ser estendido, o jogador sofre uma punição no resumo do dia. **[PROPOSTA]** -1 de reputação. A comissão menor, por ter atendido menos pacientes, já é uma punição natural.
- No fim do dia: resumo com atendimentos, erros, comissão ganha e despesas pessoais. Depois, um evento narrativo curto e o protocolo do dia seguinte.

## Economia
- **Comissão** por paciente atendido: quanto mais atende, mais ganha.
- **Erro de triagem = sem comissão do paciente + multa de $5.**
- **Dívida:** o saldo é acumulado de um dia para o outro. Se o dia fecha no negativo, o valor é descontado do dia seguinte, e se esse também fechar no negativo, a dívida aumenta. **[PROPOSTA]** A dívida final aparece no epílogo.
- **[PROPOSTA]** Escolha ética: o paciente conta como atendido (+$5) e a escolha custa $10 (líquido -$5).
- **Comissão: $5 por paciente atendido.**
- **Despesas fixas: $25 por dia** (aluguel, transporte, algo pessoal), o que cria o motivo para forçar o ritmo.

A tensão central é: **ganhar mais atendendo rápido vs. decidir bem**. É essa tensão que alimenta os finais.

## Regras que mudam
Cada dia traz uma mudança no protocolo, em arquivos de dados e não em código. Exemplos **[PROPOSTA]**: um leito a menos, um tipo de atendimento suspenso, nova exigência de documento, um surto de um sintoma inventado. As regras são ficcionais, sem base em protocolos clínicos reais.

## Tom
Realista na ambientação (hospital, fila, burocracia) com humor em cada paciente: personagens recorrentes, excentricidades, desculpas criativas, situações absurdas, tudo em pequenos diálogos. Os casos sérios existem e não viram piada, para que o riso dos outros tenha contraste.

## Finais narrativos
Os finais dependem do **acúmulo de escolhas** ao longo da campanha. Medidas rastreadas:
- Reputação com a administração (decide final): sobe com boas decisões, sem erro
- Consciência (decide final): sobe com escolhas éticas, que dão mais trabalho ao hospital
- Dinheiro (comissão acumulada): não decide final, mas pressiona o jogador e aparece no epílogo

**[PROPOSTA]** Quatro finais, a partir de reputação e consciência (alta ou baixa):
- Reputação alta + consciência alta: o plantonista de confiança
- Reputação alta + consciência baixa: promovido, mas à custa dos outros
- Reputação baixa + consciência alta: desligado, mas com a cabeça erguida
- Reputação baixa + consciência baixa: desligado sem ter feito diferença

O desfecho do garoto entra como linha de epílogo dentro de cada final. Ralel não tem desfecho próprio. Cada final deve ser reconhecível pelas decisões do jogador, não só pelo número.

## Gráficos (escopo mínimo)
Guichê fixo, retrato do paciente em pixel art ou estilo simples (peças combináveis para gerar variedade), papéis e fichas como interface. **[PROPOSTA]** Sem animação complexa.

## Fora do escopo (v1)
- Sem editor de conteúdo, multiplayer, 3D ou vozes.
- Sem simulação médica realista.
- Sem mais de uma sala ou cenário.

## Decisões fechadas
- **Campanha de 7 dias.**
- **Finais por várias medidas:** reputação e/ou consciência definem o final. O dinheiro não decide final, mas é a isca do jogo e aparece no epílogo.
- **Colega fixo:** aparece todo dia, antes do turno, e é a primeira pessoa a avisar das novas validações. **[PROPOSTA]** Um aviso dele pode estar incompleto ou errado em algum dia, para o jogador aprender a conferir.
- **Garoto do atestado:** um menino sozinho que tenta 4 vezes conseguir um atestado médico para faltar à aula. **[PROPOSTA]** Aparece nos dias 1, 3, 5 e 7, com uma tática diferente a cada vez e um motivo real que vai sendo revelado. O desfecho depende de como o jogador o tratou.

## Conteúdo de pacientes **[PROPOSTA]**
- Em um turno típico o jogador atende cerca de 15 pacientes (≈20 s cada): os de história escritos à mão e uns 10 de rotina gerados. Os de rotina continuam sendo gerados até o relógio acabar, então a fila não zera. O dia 1 completo está em `dia-1.md`.
- **28 pacientes escritos à mão** (4 por dia), mais os 2 personagens fixos.
- Desses 4 diários, **1 é um caso de peso** (a vida real de quem depende do sistema de saúde), e os outros 3 são de humor.
- O restante da fila é gerado por combinação (sintoma + problema de documento + traço de personalidade), com falas curtas.

## Personagens fixos
**Ralel** (27 anos, bem humorado, personagem secundário sem arco nem desfecho próprio, que só cumpre a função de passagem de turno e avisos): faz triagem em outra cabine, é sempre do turno anterior e deixa o posto dele sob responsabilidade do jogador. Cada dia começa com a passagem de turno dele, onde ele avisa das novas validações. **[PROPOSTA]** Como vem de turno cheio, está cansado, o que explica avisos incompletos em algum dia e pode deixar casos pendentes para o jogador.

**Garoto do atestado:** nos dias 1, 3 e 5 dá motivos absurdos e engraçados. No dia 7 mostra hematomas no braço e na costela e conta, com voz triste (diferente dos outros dias), que sofre bullying na escola.
- **[PROPOSTA]** Plantar pistas sutis nos dias anteriores: as desculpas absurdas giram em torno dos mesmos momentos da escola (recreio, educação física, saída).
- **[PROPOSTA]** Escutá-lo custa segundos do turno, então menos pacientes e menos comissão. Essa é a tensão entre dinheiro e consciência.
- **Dia 7, duas escolhas do jogador:**
  - **Aceitar:** o garoto passa 1 semana em casa e o serviço social é acionado para chamar a família dele.
  - **Recusar** (não encaminhá-lo ao médico para obter o atestado): a família é chamada, e fica subentendido que o garoto voltou à escola no dia seguinte, ainda com dores.
  - O jogo não moraliza e não mostra a consequência da recusa, só a deixa subentendida.

## Tom do epílogo
Emocional. O humor domina os dias, e o epílogo é onde o jogador sente o peso do que escolheu.

## Regras de pontuação (fechadas)
- Reputação e consciência começam num ponto neutro, e "alta" é estar acima dele no fim do dia 7. Ajusta-se depois, jogando.
- Cada escolha ética custa **$10** de comissão.

## Dilemas
Cada dia tem um caso de peso que vira dilema entre a regra e a pessoa. Exemplo fechado:
- **Mãe solteira com receita vencida:** ela precisa de um remédio na farmácia do hospital, mas a receita está vencida. O jogador só descobre num pequeno diálogo, em que ela conta que não conseguiu vir antes por causa da rotina de mãe solteira. Ele escolhe entre deixá-la conseguir o remédio ou recomendar que renove a receita e volte depois.
- **[PROPOSTA]** Deixá-la passar é a escolha ética: custa $10, sobe a consciência e fere a reputação, porque violou a validação do dia. Mandá-la renovar segue o protocolo: sobe a reputação e não dá consciência.

### Outros dilemas (fechados)
Todos seguem o mesmo molde: o protocolo do dia, o caso, uma revelação num diálogo curto e duas escolhas. **Seguir o protocolo** sobe a reputação e não custa nada. **Burlar o protocolo** custa $10, sobe a consciência e fere a reputação.
- **O operário no limite da exaustão.** Protocolo: sem atestado de afastamento para sintomas leves, sem laudo de especialista. Trabalhador da construção civil, 14 h por dia, pede 2 dias ("se eu faltar sem atestado, o patrão me despede hoje"). Revelação, se o jogador gastar segundos ouvindo: calos extremos nas mãos e tremedeira por falta de sono. Burlar: atestado falso por "gastroenterite aguda"; a auditoria detecta e a reputação cai.
- **O idoso e a receita do remédio controlado.** Protocolo: receita de controlado de alto custo precisa de carimbo presencial do médico responsável. O médico titular está de licença, o plantonista não assina sem nova consulta (fila de 3 semanas), e o senhor só tem remédio para 24 horas. Revelação: vem de ônibus, sozinho, a filha mora em outro estado. Burlar: liberar por conta própria. **Seguir o protocolo tem consequência que volta:** o idoso reaparece depois em pior estado (**[PROPOSTA]** no dia 6, o dia sem dilema novo).
- **A jovem sem RG.** Protocolo: exames rápidos exigem identidade original com foto. Ela chega com dor abdominal forte, bolsa furtada há duas horas, só uma foto borrada do RG no celular e a carteirinha de estudante rasurada. Revelação: chora baixinho, foi enxotada da recepção e é a última chance antes do laboratório fechar. Burlar: validar pela foto do celular e pela carteirinha. **Aqui a auditoria não pega:** a escolha só rende consciência, sem fazer a reputação cair.
- **O pai e a criança com febre.** Protocolo: com a pediatria sem vagas, febre abaixo de 39°C é redirecionada ao posto de bairro, a 5 km. Ele chega com a filha de 5 anos com 38,5°C, sem dinheiro para o ônibus. Revelação: o posto fechou o portão na cara dele e ela já delirou de febre na rua. Burlar: fingir no sistema 39,5°C para forçar o encaixe.

### Distribuição pelos dias **[PROPOSTA]**
Dia 1: mãe solteira · Dia 2: jovem sem RG · Dia 3: operário · Dia 4: idoso · Dia 5: pai e criança · Dia 6: sem dilema novo (decidido) · Dia 7: garoto do atestado.
O peso emocional sobe ao longo da semana, e o jogador tende a ser mais estrito nos primeiros dias, quando está juntando dinheiro.

## Entre os turnos
Entre um dia e outro há uma tela com o saldo do dia (comissão menos despesas) e uma decisão de vida do jogador, que pode virar despesa extra nos dias seguintes. Exemplo fechado:
- **O cachorro na chuva:** no caminho de casa, um filhote está na chuva na porta. O jogador pega ou deixa. Se pegar, vira uma despesa: ração de $2 ou $5 por dia, e se o jogador passar muito tempo fora de casa, o cão fica ansioso e estraga ou rasga algo, o que gera mais despesa.
- **[PROPOSTA]** O cão ganha uma linha de epílogo, como o garoto.
- **Tempo fora de casa:** só conta se o jogador aceitar cobrir o plantão de um colega (hora extra).

### Oferta de turno dobrado (hora extra)
Disponível no dia 5: a administração manda uma mensagem de última hora pedindo para cobrir a folga de um colega na madrugada.
- **Aceitar:** +$20 de comissão, mas consome o tempo livre. Se o jogador adotou o cachorro, a ansiedade dele estoura e ele destrói um móvel (-$12). No dia seguinte o jogador começa o plantão com um debuff de cansaço: a tela esmaece de leve no começo e vai ficando mais forte ao longo do turno, o que dificulta a leitura e dá a sensação de olhos fechando pesados.
- **Recusar e ir dormir:** a rotina segue normal.
- **[PROPOSTA]** O dia 6, sem dilema novo, vira o dia em que o cansaço pesa e as consequências aparecem. Incluir uma opção para reduzir a intensidade do efeito visual.

## Decisões desta versão
- A hora extra aparece só no dia 5.
- O debuff de cansaço afeta só a leitura (efeito visual).
- As decisões entre turnos são só o cachorro e a hora extra.

## Para revisitar depois de jogar
- O papel do cachorro, que hoje é pequeno (ração diária e um possível estrago na hora extra).
- Os limiares de reputação e consciência.
- O equilíbrio da economia (comissão de $5, despesas de $25, escolhas éticas de $10).

## Decisões em aberto (suas)
Nenhuma por enquanto. Falta escrever o conteúdo de cada dia (protocolo, pacientes e falas).
