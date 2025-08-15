using Godot;
using System;


namespace Target.Scripts;

public partial class HUD : CanvasLayer
{
    [Export] private PackedScene HeartTexture;

    public static HUD Instance { get; private set; }

    const int TotalHearts = 3;
    const int MaxScore = 999;

    private Label _displayScore;
    public HBoxContainer HealthBar;
    public int Score = 0;

    public override void _Ready()
    {
        Instance = this;

        _displayScore = GetNode<Label>("ScoreDisplay");
        HealthBar = GetNode<HBoxContainer>("HealthBar");

        AddHealth();
    }

    public void AddScore()
    {
        if (Score >= MaxScore) return;

        Score++;
        _displayScore.Text = Score.ToString();
    }

    private void AddHealth()
    {
        for (int i = 0; i < TotalHearts; i++)
        {
            var heart = HeartTexture.Instantiate();
            HealthBar.AddChild(heart);
        }
    }
    
}
