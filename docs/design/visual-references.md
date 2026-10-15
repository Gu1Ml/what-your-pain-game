# Referências visuais

Imagens de referência de **apresentação** (layout, clima e composição das telas). Elas **não são especificações de regras de negócio**. Classificação de triagem, consequências, valores, destinos e textos continuam definidos por `specs/001-triagem-alpha-dia-1/`, `docs/decisions.md` e `content/`. Se uma imagem mostrar algo que diverge desses documentos (rótulos de ação, valores, regras do folheto), prevalecem os documentos, e a divergência deve ser registrada em `docs/decisions.md`.

| Tela | Arquivo | Formato | Uso previsto |
|---|---|---|---|
| HUD do guichê | [`assets/ui/references/hud-reference.jpg`](../../assets/ui/references/hud-reference.jpg) | JPEG 1376×768 | Referência para `TriageStation.tscn` e subpainéis (T014 em diante). |
| Menu principal | [`assets/ui/references/main-menu-reference.jpg`](../../assets/ui/references/main-menu-reference.jpg) | JPEG 1376×768 | Referência para `MainMenu.tscn` (T013). |

## Observações

- A solicitação citava `hud-reference.png` e `main-menu-reference.png`; os arquivos presentes no repositório são `.jpg` (registrado como INC-007 em `docs/decisions.md`).
- As imagens originais não foram editadas, convertidas nem substituídas.
- A pasta contém um `.gdignore` para que o Godot não importe as imagens nem as inclua na build exportada, já que são material de referência e não assets do jogo. Para usá-las dentro do editor (por exemplo, como guia sobreposto), basta remover esse arquivo.
- Nenhuma tela foi implementada a partir destas referências na Fase II.
