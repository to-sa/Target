using Godot;
using System;
using Target.Scenes;

namespace Target.Scripts;

[GlobalClass]
public partial class Main : Node2D
{
    [Export] public PackedScene MobScene;
    private Area2D _player;

    private Timer _mobTimer;
    private PathFollow2D _mobPath;

    public override void _EnterTree()
    {
        _player = GetNode<Area2D>("Player");
        _mobTimer = GetNode<Timer>("MobTimer");
        _mobPath = GetNode<PathFollow2D>("MobPath/SpawnLocation");
    }

    public override void _Ready()
    {
        Vector2 MiddleOfScreen = new Vector2(GetWindow().Size.X / 2, GetWindow().Size.Y / 2);
        _player.GlobalPosition = MiddleOfScreen;

        _mobTimer.Timeout += OnMobTimerTImeout;
    }

    public void GameOver()
    {
        _mobTimer.Stop();
    }

    private void OnMobTimerTImeout()
    {
        Barrel NewBarrel = MobScene.Instantiate<Barrel>();
        _mobPath.ProgressRatio = GD.Randf();
        NewBarrel.GlobalPosition = _mobPath.GlobalPosition;
    }

}
