# Fontes originais de design

Cópias **sem alteração** dos documentos de origem do projeto. Foram copiadas em 2026-10-14 com autorização do autor, depois da auditoria [`docs/audits/fase-2-rastreabilidade.md`](../audits/fase-2-rastreabilidade.md). Não edite estes arquivos: correções e decisões vão em [`docs/decisions.md`](../decisions.md). O `.gitattributes` desta pasta impede a conversão de fim de linha, para que os bytes permaneçam idênticos aos originais.

| Arquivo | Título / versão | Origem | Tamanho | SHA-256 |
|---|---|---|---|---|
| [`dia-1.md`](dia-1.md) | *Dia 1 — O Primeiro Plantão* (v0.2) | `D:/Downloads/dia-1.md` (modificado em 2026-10-13 15:48) | 9 156 B | `2104dd1ed47e1447b368d4c14642c3a8bdea69c4352860c7f61419f4d29abcc6` |
| [`briefing-triagem.md`](briefing-triagem.md) | *Triagem — Briefing* (v0.1) | `D:/Downloads/briefing-triagem.md` (modificado em 2026-10-13 15:48) | 12 777 B | `5e1f00c683b5a9681925166051b870432b0218845af9a6e1cfdd928a27fcf4f5` |
| [`briefing-triagem-alpha-demo-v1.0.md`](briefing-triagem-alpha-demo-v1.0.md) | *TRIAGEM — Briefing técnico e de design da Alpha Demo* (v1.0) | `D:/Downloads/briefing-triagem-alpha-demo-v1.0.md` (modificado em 2026-10-14 15:00) | 80 063 B | `850141a1d2afed7b8fc64acac00f63de7cd81cff3c4a00b11bb94bf43c171ef2` |

## Papel de cada fonte

- **`dia-1.md`** é a fonte dos textos e da sequência narrativa do Dia 1: folheto, falas, perfis e fichas. O briefing v1.0 §20 diz que ele "continua sendo a fonte dos textos e da sequência narrativa do Dia 1".
- **`briefing-triagem-alpha-demo-v1.0.md`** consolida o v0.1, o `dia-1.md` e as decisões da conversa de design. É a fonte citada em `specs/001-triagem-alpha-dia-1/spec.md`. Seus exemplos de JSON são ilustrativos (§6.4, §8.4: "esboço de migração").
- **`briefing-triagem.md`** é o briefing original da campanha. O v1.0 o substitui como referência técnica, mas não o apaga.

Quando as fontes divergem, a divergência fica registrada em `docs/decisions.md` (prefixo `INC-`) e não é resolvida por escolha silenciosa.

## Artefatos citados que não existem

`specs/001-triagem-alpha-dia-1/quickstart.md` e `specs/001-triagem-alpha-dia-1/contracts/decision-matrix-contract.md` são citados em `plan.md`, `tasks.md` e `IMPLEMENT.md`, mas nunca foram gerados e não fazem parte destas fontes (INC-001). Eles precisam ser gerados, sem recriação por inferência, antes das tarefas que dependem deles (T044 para o `quickstart.md`).
