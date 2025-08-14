using System;
using Godot;
using Target.Scenes;
using Target.User;

namespace Target.Scripts;

public partial class Main : Control
{
    [Export] public PackedScene MobScene;

    private Player _player;
    private Timer _mobTimer;
    private PathFollow2D _mobPathFollow;
    private Path2D _mobPath;
    private Panel _gameOverScreen;
    private Area2D _sword;
    private Area2D _shield;

    public override void _EnterTree()
    {
        _player = GetNode<Player>("%Player");
        _mobTimer = GetNode<Timer>("Mobs/MobTimer");
        _mobPathFollow = GetNode<PathFollow2D>("%SpawnLocation");
        _mobPath = GetNode<Path2D>("Mobs/MobPath");
        _gameOverScreen = GetNode<Panel>("%GameOver");
        _sword = GetNode<Area2D>("%Sword");
        _shield = GetNode<Area2D>("%Shield");
    }

    public override void _Ready()
    {
        _mobTimer.Timeout += OnMobTimerTimeout;

        // Clear path points
        var screenSize = GetViewportRect().Size;
        _mobPath.Curve.ClearPoints();

        // Set path points
        _mobPath.Curve.AddPoint(new Vector2(-50, -20));
        _mobPath.Curve.AddPoint(new Vector2(screenSize.X, 0));
        _mobPath.Curve.AddPoint(new Vector2(screenSize.X, screenSize.Y));
        _mobPath.Curve.AddPoint(new Vector2(-50, screenSize.Y));
        _mobPath.Curve.AddPoint(new Vector2(-50, 0));
    }

    public override void _Process(double delta)
    {
        if (_player.health == 0)
        {
            GameOver();
        }

        if (HUD.Instance.Score == 10)
        {
            _mobTimer.WaitTime = 1.3f;
        }

        if (HUD.Instance.Score == 15)
        {
            _player.Range._attackTimer.Start();
        }

        if (HUD.Instance.Score == 40)
        {
            _mobTimer.WaitTime = 1.0f;

        }

        if (HUD.Instance.Score == 60)
        {
            _mobTimer.WaitTime = 0.8f;
            _sword.Show();
            _sword.CollisionMask = 1;
            _player.AttackTimer.WaitTime = 1.15;
        }

        if (HUD.Instance.Score == 90)
        {
            _mobTimer.WaitTime = 0.6f;
            _shield.Show();
            _shield.CollisionMask = 1;
        }

        if (HUD.Instance.Score == 150)
        {
            _mobTimer.WaitTime = 0.45f;
        }

    }

    public void GameOver()
    {
        ProcessMode = ProcessModeEnum.Disabled;
        _mobTimer.Stop();
        _gameOverScreen.Show();
    }

    private void OnMobTimerTimeout()
    {
        Barrel NewBarrel = MobScene.Instantiate<Barrel>();

        // Set mob's position to a random positon on the PathFollow2D
        _mobPathFollow.ProgressRatio = GD.Randf();
        NewBarrel.GlobalPosition = _mobPathFollow.GlobalPosition;

        AddChild(NewBarrel);
    }

    private void OnRestartPressed()
    {
        GetTree().ReloadCurrentScene();
    }

}
