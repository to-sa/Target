using Godot;
using System;

public partial class Menu : Control
{

    PackedScene main = GD.Load<PackedScene>("res://Scenes/Main.tscn");

    private Panel _creditsPanel;
    private Panel _optionsPanel;

    public override void _Ready()
    {
        _creditsPanel = GetNode<Panel>("CreditsPanel");
        _creditsPanel.Hide();

        _optionsPanel = GetNode<Panel>("OptionsPanel");
        _optionsPanel.Hide();
    }

    private void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToPacked(main);
    }

    private void OnCreditsButtonPressed()
    {
        _creditsPanel.Show();
    }

    private void OnBackButtonPressed()
    {
        _creditsPanel.Hide();
    }

    private void OnOptionsButtonPressed()
    {
        _optionsPanel.Show();

    }

    private void OnOptionsBackButtonPressed()
    {
        _optionsPanel.Hide();
    }
}
