using Godot;
using System;

public partial class Menu : Control
{

    PackedScene main = GD.Load<PackedScene>("res://Scenes/Main.tscn");

    private void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToPacked(main);
    }

}
