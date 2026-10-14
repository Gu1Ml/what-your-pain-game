# Constituição do Projeto — Triagem

**Versão:** 1.0.0-draft  
**Estado:** proposta inicial, ainda não ratificada formalmente  
**Data de ratificação:** [EM ABERTO]  
**Última revisão:** 2026-10-14

Esta Constituição define princípios de produto e desenvolvimento. Ela não fixa engine, linguagem, bibliotecas ou detalhes de implementação; essas decisões pertencem ao plano técnico.

## Princípio 1 — A decisão do jogador deve ter consequências compreensíveis

O jogo deve permitir que o jogador compreenda quais informações estavam disponíveis, qual regra se aplicava e por que sua ação foi classificada como correta, permitida, erro, exceção ética ou não resolvida. O feedback imediato e o resumo do plantão devem ser coerentes com a matriz de decisões aprovada.

## Princípio 2 — O protocolo e a matriz são a fonte de verdade

O comportamento esperado de cada caso deve ser definido por regras e conteúdo identificáveis, e não por condições escondidas na interface, no diálogo ou em nomes de arquivos. A interface apresenta informações e solicita ações; não determina por conta própria se uma decisão está correta.

## Princípio 3 — Conteúdo narrativo e regras gerais devem permanecer distinguíveis

Pacientes, documentos, protocolos, diálogos, dilemas, consequências e finais devem poder ser revistos como conteúdo. A lógica geral interpreta esses dados. Alterar uma fala não deve alterar acidentalmente a economia, a reputação ou a classificação da decisão.

## Princípio 4 — Não transformar incerteza em regra por conveniência

As categorias do projeto são obrigatórias:

- **[FECHADO] / [APROVADO]:** decisão já confirmada.
- **[PROPOSTA]:** sugestão ainda sujeita a aprovação.
- **[EM ABERTO]:** decisão que não pode ser presumida.
- **[REQUISITO TÉCNICO]:** condição necessária à implementação, sem decidir conteúdo de design.

Uma pendência deve ser registrada e isolada. Se ela impedir uma classificação segura de uma ação, o sistema deve sinalizar `UNRESOLVED`, falhar na validação do conteúdo ou impedir que o caso seja considerado aprovado; nunca atribuir silenciosamente uma consequência padrão.

## Princípio 5 — Coerência financeira e trilha de auditoria

Cada decisão deve gerar um resultado explícito e rastreável. Comissão, multas, custos éticos e despesas diárias devem ser lançados uma única vez. Um erro pode registrar vários motivos, mas, conforme decisão aprovada, gera no máximo uma multa genérica de $5 por paciente.

## Princípio 6 — Dilemas éticos não são erros genéricos disfarçados

Uma escolha ética que viole uma regra pode ter consequências próprias sem ser automaticamente contabilizada como erro genérico. A exceção de Helena, já aprovada para o Dia 1, deve manter sua contabilidade, efeitos de reputação/consciência e marca narrativa separados da multa genérica.

## Princípio 7 — Continuidade narrativa explícita

Decisões importantes devem deixar marcas persistentes identificadas por IDs estáveis. O epílogo deve refletir decisões anteriores de forma reconhecível. Não criar marcas narrativas ou consequências para preencher lacunas sem uma decisão de design.

## Princípio 8 — Tom humano, humor contextual e respeito aos casos sérios

O ambiente é burocrático e pode conter humor absurdo ou nervoso. Casos de sofrimento real dentro da ficção devem ser tratados com empatia, sem que a dor seja reduzida a piada. O epílogo é emocional e não deve moralizar de forma simplista as decisões do jogador.

## Princípio 9 — Escopo controlado e entrega incremental

A prioridade inicial é uma alpha demo jogável do Dia 1 que comprove o loop central. Arte final, animações elaboradas, áudio amplo e conteúdo integral da campanha não devem atrasar a prova do loop. O desenvolvimento deve ocorrer em módulos verificáveis, com uma build funcional ao final de cada fase relevante.

## Princípio 10 — Legibilidade, previsibilidade e testabilidade

Documentos, sintomas, validade, protocolo e feedback precisam ser legíveis. A regra de domínio deve poder ser testada sem depender de interação manual com a cena. Cada linha da matriz de decisões deve virar um cenário de teste; erros de conteúdo devem ser diferentes de erros de triagem ocorridos durante a partida.

## Princípio 11 — Experiência offline e persistência local

A experiência pretendida é single-player e offline. O progresso deve ser salvo localmente e não deve depender de backend ou de serviços externos. O save armazena estado de domínio serializável, não referências a objetos visuais.

## Princípio 12 — Segurança de escopo e fidelidade ao briefing

As regras de triagem são ficcionais; o projeto não deve ser promovido nem implementado como simulador médico realista. O conteúdo aprovado do briefing e do Dia 1 deve ser preservado. Divergências entre fontes devem ser documentadas para decisão, não resolvidas silenciosamente.

---

## Gates de governança

Antes de considerar uma fatia concluída, confirmar:

1. As regras executadas têm origem rastreável em uma decisão fechada ou conteúdo explicitamente aprovado.
2. As pendências relevantes estão registradas e não recebem comportamento implícito.
3. Os efeitos financeiros e narrativos são aplicados uma única vez e podem ser auditados.
4. Os critérios de aceite pertinentes foram verificados.
5. A cena de apresentação não contém regras de domínio duplicadas.
6. A experiência mantém o tom e os limites de escopo do projeto.

## Histórico

- **1.0.0-draft (2026-10-14):** primeira versão derivada do briefing técnico da alpha demo e das decisões registradas na conversa. Aguarda ratificação do autor do projeto.
