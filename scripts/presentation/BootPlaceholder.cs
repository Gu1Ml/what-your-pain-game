using Godot;

namespace WhatYourPain.Presentation;

/// <summary>
/// Cena provisória da Phase 1 (T001/T004): só comprova que o assembly C# carrega
/// e que o projeto executa/exporta. Não contém gameplay nem regra de domínio.
/// Será substituída como cena principal por MainMenu.tscn (T013).
/// </summary>
public partial class BootPlaceholder : Control
{
    public override void _Ready()
    {
        GD.Print($"[Boot] What Your Pain? — placeholder de setup carregado (Godot {Engine.GetVersionInfo()["string"]}).");
    }
}
