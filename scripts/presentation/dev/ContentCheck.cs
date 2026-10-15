using Godot;
using WhatYourPain.Content;

namespace WhatYourPain.Presentation.Dev;

/// <summary>
/// Ferramenta de desenvolvimento: carrega e valida o conteúdo do Dia 1 a partir de
/// <c>res://content/</c> e encerra com código 0 (válido) ou 1 (inválido). Sem gameplay.
/// Uso: <c>godot --headless --path . res://scenes/dev/ContentCheck.tscn</c>
/// </summary>
public partial class ContentCheck : Node
{
    public override void _Ready()
    {
        var exitCode = 0;
        try
        {
            var day = new DataLoader(new GodotContentSource()).LoadDay(1);
            GD.Print($"[ContentCheck] Dia 1 válido: {day.Patients.Patients.Count} pacientes, " +
                     $"{day.Protocol.Rules.Count} regras, {day.DecisionMatrix.Cases.Count} casos.");
            foreach (var warning in day.Report.Warnings)
                GD.Print($"[ContentCheck] {warning}");
        }
        catch (ContentLoadException ex)
        {
            GD.PrintErr($"[ContentCheck] {ex.Message}");
            exitCode = 1;
        }
        catch (ContentValidationException ex)
        {
            GD.PrintErr($"[ContentCheck] {ex.Message}");
            exitCode = 1;
        }

        GetTree().Quit(exitCode);
    }
}
