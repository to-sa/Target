using Godot;
using System;
using System.Dynamic;
using Target.Scenes;

namespace Target.Scripts;

public partial class Main : Control
{
    [Export] public PackedScene MobScene;

    private Area2D _player;
    private Timer _mobTimer;
    private PathFollow2D _mobPath;

    public int Score = 0;

    public override void _EnterTree()
    {
        _player = GetNode<Area2D>("%Player");
        _mobTimer = GetNode<Timer>("MobTimer");
        _mobPath = GetNode<PathFollow2D>("%SpawnLocation");

    }

    public override void _Ready()
    {
        _mobTimer.Timeout += OnMobTimerTimeout;
    }

    public void GameOver()
    {
        _mobTimer.Stop();
    }

    private void OnMobTimerTimeout()
    {
        Barrel NewBarrel = MobScene.Instantiate<Barrel>();

        // Set mob's position to a random positon on the PathFollow2D
        _mobPath.ProgressRatio = GD.Randf();
        NewBarrel.GlobalPosition = _mobPath.GlobalPosition;

        AddChild(NewBarrel);
    }

}
