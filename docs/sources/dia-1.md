# Dia 1 — O Primeiro Plantão (v0.2)

> Itens marcados com **[PROPOSTA]** são sugestões para você aceitar ou trocar. Os pacientes de história e as falas vêm do seu rascunho, com os ajustes combinados (RG do Lucas, regra de erro, pacientes de rotina e tempo do turno).

## 1. Passagem de turno (Ralel)

Tela do guichê antes da abertura da porta. Ralel aparece com olheiras profundas, segurando uma caneca de café fria.

- **Ralel:** "Pronto. O posto é seu. O turno da noite foi um caos... duas brigas de bar e um idoso que engoliu a prótese dentária."
- **Ralel:** "Deixei o folheto do protocolo do dia em cima da mesa. Lembre-se: confere o nome na ficha, o tipo de sintoma e se o documento tá no prazo. A diretoria tá de olho nas falhas."
- **Ralel:** "Boa sorte. Vou tentar dormir seis horas seguidas se o barulho da obra deixar."

## 2. Mesa do jogador (tutorial)

Três itens interativos:

- **Selo de decisão:** [ ATENDER ] | [ RECOMENDAR/FILA ] | [ RECUSAR ]
- **Ficha do paciente** (entregue na hora): nome, idade, sintoma declarado, documento apresentado.
- **Folheto de protocolo do dia 1:**

```
=====================================================
          PROTOCOLO DE TRIAGEM - DIA 1
=====================================================

[ REGRAS BÁSICAS DE VALIDAÇÃO ]
1. DOCUMENTO: Deve conter Nome + RG/CPF visível e DENTRO DA VALIDADE.
2. SINTOMAS ACEITOS PARA ATENDIMENTO IMEDIATO:
   - Febre acima de 38°C
   - Dores intensas/Agudas (abdômen, peito, cabeça)
   - Ferimentos com sangramento ativo
3. SINTOMAS SEM DIREITO A ATENDIMENTO IMEDIATO (Encaminhar/Recusar):
   - Pedidos de atestado médico sem sintoma agudo
   - Tontura leve / Cansaço / Sintomas com mais de 3 dias
   - Receita médica vencida ou sem assinatura

[ ATENÇÃO ]
- Erros de triagem: sem comissão do paciente + multa de $5.
- Comissão: $5 por paciente correto.
- Custo diário de subsistência: $25.
=====================================================
```

## 3. Fila do dia 1

- **5 pacientes de história** (escritos à mão), em ordem fixa, intercalados com os de rotina: Márcio, Camila, Lucas, Helena, Roberto.
- **Pacientes de rotina** gerados até o relógio acabar (seção 5). Um jogador típico atende cerca de 10 deles.
- Total típico: cerca de 15 pacientes atendidos (≈20 s cada).

## 4. Pacientes de história

### Paciente 1: Márcio Souza (funcionalidade / caso válido)
- **Perfil:** homem de 40 anos, segurando o joelho direito.
- **Ficha:** queda de escada, dor aguda no joelho e inchaço visível. RG válido (emissão 2022).
- **Diálogo:**
  - Márcio: "Caí do degrau arrumando a calha. Não consigo nem apoiar o pé no chão..."
  - Jogador (ao conferir o documento): "Documento ok. Sintoma agudo."
- **Decisão correta:** [ ATENDER ]
- **Consequência:** +$5. Sem impacto em reputação nem consciência.
- **Fala ao sair:** "Obrigado, doutor."

### Paciente 2: Camila Torres (erro documental / recusa obrigatória)
- **Perfil:** mulher de 28 anos, mascando chiclete, olhando o celular.
- **Ficha:** "Tontura chata desde ontem." RG vencido há 4 anos.
- **Diálogo:**
  - Camila: "Preciso ver um médico logo, tenho compromisso às 15h."
  - Jogador (ao apontar a data no RG): "Seu RG está vencido."
  - Camila: "Ah, mas é só para uma receita de vitamina. Ninguém olha isso aqui."
- **Decisão correta:** [ RECUSAR ] ou [ ENCAMINHAR PARA RENOVAÇÃO ]
- **Consequência:** +$5. Se aceitar: erro (sem comissão, multa de $5).
- **Fala ao sair:** "Que burocracia ridícula. Vou no posto do bairro."

### Paciente 3: Lucas Mendes (garoto do atestado, 1ª tentativa)
- **Perfil:** menino de 11 anos, mochila escolar nas costas, tentando parecer sério.
- **Ficha:** "Febre alta repentina e dor no corpo todo." **RG válido.**
- **Diálogo:**
  - Lucas: "Moço, estou queimando de febre. Preciso de um atestado para hoje... o dia inteiro de preferência."
  - Jogador (conferindo o termômetro de triagem): "Sua temperatura está 36,6°C. Normal."
  - Lucas: "É que... a febre é interna! Dá para sentir no peito. É dia de prova de Educação Física..."
- **Decisão correta:** [ RECUSAR ]
- **Consequência:** +$5.
- **Fala ao sair** (sussurrando): "Poxa... valeu a tentativa. Volto depois de amanhã."

### Paciente 4: Helena Rios (o dilema do dia, a mãe solteira)
- **Perfil:** mulher de 32 anos, aparência cansada, sacola de mercado nas mãos.
- **Ficha:** busca de medicação contínua para asma do filho. RG válido. Receita médica vencida há 2 dias.
- **Diálogo:**
  - Helena: "Preciso retirar a bombinha do meu filho na farmácia de vocês. A dele acabou hoje cedo."
  - Jogador: "Esta receita venceu anteontem. Precisa de uma nova consulta."
  - Helena: "Eu sei... sou mãe solteira, trabalho em dois empregos e cuido dele sozinha. Levei três semanas para conseguir folga para vir aqui hoje. Se eu voltar sem o remédio, ele não dorme à noite com crise."
- **Opções:**
  - **[ RECUSAR / RECOMENDAR RECONSULTA ]** (seguir o protocolo): reputação sobe, consciência +0, +$5. Fala de Helena (abaixa a cabeça): "Compreendo... regras são regras. Vou ver se consigo emprestado com a vizinha..."
  - **[ ATENDER / LIBERAR ]** (burlar): consciência +1, a reputação cai (a auditoria pega a receita vencida), custo de $10. **[PROPOSTA]** O paciente conta como atendido (+$5), então o líquido é -$5. Fala de Helena: "Muito obrigada, moço... você não imagina o alívio que isso me dá."

### Paciente 5: Roberto Paiva (procedimento incorreto / tentativa de enganar)
- **Perfil:** homem de 50 anos, camisa amassada, cheiro de cigarro.
- **Ficha:** dor de dente há 2 semanas. CNH válida.
- **Diálogo:**
  - Roberto: "Tô com um dente doendo desde o começo do mês. Me manda pro cirurgião aí."
  - Jogador: "Dor de dente crônica é na Odontologia do Posto Central, não no Pronto Atendimento de Emergência."
  - Roberto: "Mas na recepção disseram que aqui atendia tudo!"
- **Decisão correta:** [ ENCAMINHAR / RECUSAR ]
- **Consequência:** +$5. Se aceitar: erro (sem comissão, multa de $5).
- **Fala ao sair:** "Perdi meu tempo aqui..."

## 5. Pacientes de rotina (gerados) **[PROPOSTA]**

Gerados por combinação e ajustados ao protocolo do dia 1. Não têm dilema nem escolha ética, só validação simples, com fala curta e uma pitada de humor.

- **Nome e idade:** sorteados de uma lista.
- **Sintoma:** dois grupos, do folheto:
  - Aceitos: febre acima de 38°C, dor aguda (abdômen, peito, cabeça), ferimento com sangramento ativo.
  - Não aceitos: pedido de atestado sem sintoma agudo, tontura leve, cansaço, sintoma com mais de 3 dias, receita vencida ou sem assinatura.
- **Documento:** válido, vencido ou sem RG/CPF visível.
- **Traço de fala:** uma frase curta de personalidade (apressado, desconfiado, simpático, culpa o sistema, etc.).
- **Proporção inicial:** cerca de 60% corretos para atender e 40% para recusar ou encaminhar. Ajusta-se jogando.

## 6. Tempo do turno

- Relógio base de 5 minutos reais. O turno dura de 5 a 7 minutos na prática.
- Os pacientes de rotina são gerados até o relógio acabar.
- O turno só termina depois que os 5 pacientes de história tiverem passado. Passados os 5 minutos, não entram mais pacientes de rotina, só os de história que faltam.
- **Atendimento lento:** se o turno precisou ser estendido, aparece uma punição no resumo do dia. **[PROPOSTA]** -1 de reputação.

## 7. Resumo de fim de turno

```
=====================================================
             RESUMO DO PLANTÃO - DIA 1
=====================================================

Pacientes atendidos:        ~15
Corretos:                   15 ($75)
Erros (sem comissão, -$5):  0
Escolhas éticas (-$10):     0 (ou 1 se ajudou a Helena)
Atendimento lento:          não (ou sim: -1 reputação)

-----------------------------------------------------
FINANÇAS DO DIA:
Comissão bruta:                         $75.00
Multas e escolhas éticas:               -$0.00 (ou -$10.00)
Despesas fixas (aluguel/alimentação):  -$25.00
-----------------------------------------------------
SALDO DO DIA:                          +$50.00 (ou +$40.00)
SALDO ACUMULADO (negativo = dívida):   +$50.00 (ou +$40.00)

MÉTRICAS DA CAMPANHA:
- Reputação administrativa: [ ====|---- ] (neutro / baixa)
- Consciência:              [ ====|---- ] (neutro / alta)

=====================================================
```

## 8. Evento entre turnos: o cachorro

Cena simples do personagem caminhando à noite sob chuva fina. Um filhote vira-lata está encolhido ao lado de uma caixa de papelão úmida na calçada.

- **Texto na tela:** "No caminho para casa, perto do ponto de ônibus, você escuta um ganido fraco. Um filhote de cachorro está encolhido sob a chuva, tremendo de frio."
- **[ Levar o filhote para casa ]**
  - Despesa diária de ração a partir do dia 2 (-$2/dia).
  - Garante a linha narrativa do mascote no final do jogo.
  - Mecânica destravada: o tempo fora de casa (hora extra) afeta a ansiedade do cão nos dias seguintes.
- **[ Continuar caminhando ]**
  - Sem despesas adicionais. O cão não aparece mais na história.
