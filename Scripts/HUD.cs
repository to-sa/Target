using Godot;
using System;


namespace Target.Scripts;

public partial class HUD : CanvasLayer
{
    [Export] private PackedScene HeartTexture;

    public static HUD Instance { get; private set; }

    const int totalHearts = 3;

    private Label _displayScore;
    public int Score = 0;
    public HBoxContainer HealthBar;

    public override void _Ready()
    {
        Instance = this;

        _displayScore = GetNode<Label>("ScoreDisplay");
        HealthBar = GetNode<HBoxContainer>("HealthBar");

        AddHealth();
    }

    public void AddScore()
    {
        Score++;
        _displayScore.Text = Score.ToString();
    }

    private void AddHealth()
    {
        for (int i = 0; i < totalHearts; i++)
        {
            var heart = HeartTexture.Instantiate();
            HealthBar.AddChild(heart);
        }

    }

}
