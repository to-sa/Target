using Godot;
using System;


namespace Target.Scripts;

public partial class HUD : CanvasLayer
{
    public static HUD Instance { get; private set; }

    private Label _displayScore;

    public int Score = 0;

    public override void _Ready()
    {
        Instance = this;

        _displayScore = GetNode<Label>("ScoreDisplay");
    }

    public void AddScore()
    {
        Score++;
        _displayScore.Text = Score.ToString();
    }

}
