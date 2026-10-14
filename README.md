# What Your Pain?

Jogo narrativo de decisões burocráticas em um hospital: durante um plantão curto, o jogador confere ficha, documento e sintomas, consulta o protocolo do dia, escolhe uma ação e recebe consequências financeiras, administrativas e narrativas. As regras de triagem são **ficcionais**; o projeto não é um simulador médico.

## Objetivo da alpha

Entregar uma alpha demo jogável, offline e single-player do **Dia 1**, que prove o loop completo: chegada do paciente → inspeção → decisão → consequência → atualização do estado → próximo paciente, terminando no resumo do plantão, no evento do cachorro e no save local. A campanha de sete dias é apenas preparada na arquitetura; não faz parte do conteúdo jogável da alpha.

Decisões ainda `[EM ABERTO]` (por exemplo, o mapeamento das ações FILA/ENCAMINHAR e o gatilho global de comissão) **não** recebem comportamento definitivo no código: ficam `UNRESOLVED` até serem decididas em [`docs/decisions.md`](docs/decisions.md).

## Estado atual

Phase 1 — Setup (T001–T004). Ainda não há gameplay: a cena principal é o placeholder técnico `scenes/Boot.tscn`, que só confirma que o assembly C# carrega. Ela será substituída por `MainMenu.tscn` em T013.

## Requisitos locais

- **Godot Engine 4.6.3 .NET** (build `mono`; o editor padrão sem .NET não executa scripts C#).
- **Templates de exportação 4.6.3 .NET** (`Godot_v4.6.3-stable_mono_export_templates.tpz`), necessários para exportar para Windows.
- **.NET SDK 8.0 ou superior** (o projeto usa `net8.0`; testado com SDK 10.0.302 e runtime 8.0.29 instalados).
- Windows 10/11 x64 (plataforma-alvo).

Ambiente em que o setup foi verificado: editor `4.6.3.stable.mono.official.7d41c59c4`, templates `4.6.3.stable.mono`, .NET SDK 10.0.302, runtime Microsoft.NETCore.App 8.0.29, renderer Forward+ (Vulkan).

## Como compilar e executar

```bash
dotnet build WhatYourPain.sln
```

```bash
godot --path .
```

Use o executável do editor .NET (por exemplo `Godot_v4.6.3-stable_mono_win64_console.exe`) no lugar de `godot`.

## Exportação Windows

O preset `Windows Desktop` está registrado em `export_presets.cfg` e gera `build/windows/WhatYourPain.exe` (a pasta `build/` não é versionada):

```bash
godot --headless --path . --export-release "Windows Desktop" build/windows/WhatYourPain.exe
```

## Estrutura

```text
scenes/                 cenas Godot (UI 2D fixa)
scripts/application/    fluxo do turno, fila, estado (GameState)
scripts/domain/         protocolo, matriz, economia, reputação — sem dependência de UI
scripts/content/        leitura e validação de JSON
scripts/generation/     gerador de pacientes de rotina
scripts/persistence/    save/load local em user://
scripts/presentation/   controladores de cena (sem regras de domínio)
content/                dados JSON do Dia 1 (res://content/)
tests/                  testes .NET (projeto separado, T011)
docs/                   registro de decisões e aceite
```

## Documentos Spec-Kit

- Constituição: [`.specify/memory/constitution.md`](.specify/memory/constitution.md)
- Especificação: [`specs/001-triagem-alpha-dia-1/spec.md`](specs/001-triagem-alpha-dia-1/spec.md)
- Plano técnico: [`specs/001-triagem-alpha-dia-1/plan.md`](specs/001-triagem-alpha-dia-1/plan.md)
- Modelo de dados: [`specs/001-triagem-alpha-dia-1/data-model.md`](specs/001-triagem-alpha-dia-1/data-model.md)
- Tarefas: [`specs/001-triagem-alpha-dia-1/tasks.md`](specs/001-triagem-alpha-dia-1/tasks.md)
- Registro de decisões: [`docs/decisions.md`](docs/decisions.md)
- Roteiro de execução por fases: [`IMPLEMENT.md`](IMPLEMENT.md)
