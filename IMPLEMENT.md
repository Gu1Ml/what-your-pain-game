# Implement — Guia de execução por fases

Este arquivo descreve como executar a fase **Implement** sobre a especificação, o plano e as tarefas já criados. Não é um comando interno do Spec-Kit nem substitui `tasks.md`; é um roteiro de revisão e entrega incremental.

## Antes de começar

1. Colocar este pacote no repositório do jogo e manter `.specify/memory/constitution.md` no local indicado.
2. Inicializar o projeto Godot 4.6.3 .NET e verificar que o editor/SDK local compila C#.
3. Ler `specs/001-triagem-alpha-dia-1/spec.md`, `plan.md`, `data-model.md`, `quickstart.md` e `docs/decisions.md`. **Nota (2026-10-14):** `quickstart.md` ainda não existe e precisa ser gerado (INC-001); as fontes originais estão em `docs/sources/`.
4. Não alterar no decorrer da implementação as regras marcadas `[EM ABERTO]` sem atualizar o registro de decisões e os arquivos impactados.
5. Se usar um agente com integração Spec-Kit, revisar a ordem recomendada na documentação dessa integração. Normalmente, os comandos centrais são Constitution → Specify → Plan → Tasks → Implement; os nomes/atalhos podem variar conforme o agente.

## Fase I — Fundação

**Tarefas principais:** T001–T012.

**Entrega:** projeto compilável, estrutura de diretórios, DTOs, `GameState`, carregador/validador JSON, teste básico e registro de pendências.

**Portão de revisão:** nenhum conteúdo inválido pode ser confundido com erro de triagem; as classes de domínio compilam sem depender da UI; as pendências são representadas explicitamente.

## Fase II — Plantão e protocolo

**Tarefas principais:** T013–T017.

**Entrega:** menu simples, passagem de turno, folheto consultável e guichê.

**Portão de revisão:** é possível iniciar o Dia 1, consultar as três categorias de regra e chegar ao ponto de apresentar o primeiro paciente.

## Fase III — Decisão por paciente

**Tarefas principais:** T018–T025.

**Entrega:** inspeção, documentos, termômetro de Lucas, quatro slots de ação, diálogo/saída e motor de decisão dirigido por matriz.

**Portão de revisão:** todos os comportamentos já definidos passam nos testes; nenhuma ação cujo significado continue em aberto é resolvida com um default silencioso. A UI dos quatro slots pode existir antes da decisão final sobre o mapeamento, mas o aceite de todas as ações depende dessa decisão.

## Fase IV — Fila e relógio

**Tarefas principais:** T026–T031.

**Entrega:** cinco pacientes narrativos em ordem relativa, gerador com seed e encerramento de geração de rotina após 300 segundos.

**Portão de revisão:** ao expirar o tempo, pacientes de rotina deixam de ser gerados e os narrativos restantes continuam até todos terem passado; casos aleatórios não dependem de precedência não aprovada.

## Fase V — Contabilidade e resumo

**Tarefas principais:** T032–T036.

**Entrega:** lançamentos de comissão/multa/custo ético, reputação/consciência e resumo do plantão.

**Portão de revisão:** multa única por paciente com erro; Helena é tratada como exceção ética; bônus diário +1 respeita a regra aprovada; despesa fixa de $25 não é duplicada. A política global de comissão deve ser decidida antes de declarar aceite financeiro integral.

## Fase VI — Evento e save/load

**Tarefas principais:** T037–T041.

**Entrega:** decisão do cachorro e checkpoint local do Dia 1.

**Portão de revisão:** adoção é preservada em save; save ausente ou inválido não bloqueia campanha nova; o jogo não promete retomada no meio do turno.

## Fase VII — QA, polimento mínimo e build

**Tarefas principais:** T042–T046.

**Entrega:** feedback audiovisual mínimo, arte temporária, smoke test e build Windows.

**Portão de revisão:** todos os critérios de `spec.md` são testados, falhas são registradas e decisões abertas são listadas como bloqueios. Não declarar o loop completo pronto se um dos casos essenciais ainda tiver classificação `UNRESOLVED`.

## Fluxo de Spec-Kit sugerido

Se estiver usando a integração de agente compatível com Spec-Kit, use os documentos existentes como contexto e execute a fase Implement apenas depois da revisão de `spec.md`, `plan.md` e `tasks.md`. Em uma integração com comandos `/speckit.*`, o fluxo padrão tende a usar `/speckit.implement`; siga o comando e a sintaxe disponibilizados pela integração instalada.

**Regra de encerramento:** ao completar cada fase, atualize os checkboxes de `tasks.md`, rode os testes relevantes, registre desvios em `docs/decisions.md` ou `docs/alpha-acceptance.md` e só então avance.
