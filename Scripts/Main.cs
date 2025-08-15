using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Target.Scenes;
using Target.User;

namespace Target.Scripts;

public partial class Main : Control
{
    [Export] public PackedScene MobScene;
    [Export] public Array<Texture2D> Backgrounds; 

    PackedScene HomeScreen = GD.Load<PackedScene>("res://Scenes/MainMenu.tscn");

    private Player _player;
    private Timer _mobTimer;
    private PathFollow2D _mobPathFollow;
    private Path2D _mobPath;
    private Panel _gameOverScreen;
    private Label _highScoreLabel;
    private Area2D _sword;
    private Area2D _shield;
    private Panel _optionsPanel;
    private TextureRect _background;
    private bool _backgroundChanged = false;

    public override void _EnterTree()
    {
        _player = GetNode<Player>("%Player");
        _mobTimer = GetNode<Timer>("Mobs/MobTimer");
        _mobPathFollow = GetNode<PathFollow2D>("%SpawnLocation");
        _mobPath = GetNode<Path2D>("Mobs/MobPath");
        _gameOverScreen = GetNode<Panel>("%GameOver");
        _sword = GetNode<Area2D>("%Sword");
        _shield = GetNode<Area2D>("%Shield");
        _optionsPanel = GetNode<Panel>("HUD/OptionsPanel");
        _highScoreLabel = GetNode<Label>("%HighScoreLabel");
        _background = GetNode<TextureRect>("Background");
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

        _highScoreLabel.Text = "High Score\n" + SaveLoad.Instance.HighestScore.ToString();
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
            _background.Texture = Backgrounds[0];
        }

        if (HUD.Instance.Score == 40)
        {
            _mobTimer.WaitTime = 1.0f;
            _background.Texture = Backgrounds[2];
        }

        if (HUD.Instance.Score == 60)
        {
            _mobTimer.WaitTime = 0.8f;
            _sword.Show();
            _sword.CollisionMask = 1;
            _player.AttackTimer.WaitTime = 1.15;
        }

        if (HUD.Instance.Score == 100)
        {
            _mobTimer.WaitTime = 0.6f;
            _shield.Show();
            _shield.CollisionMask = 1;
            _background.Texture = Backgrounds[4];
        }

        if (HUD.Instance.Score == 150)
        {
         _background.Texture = Backgrounds[1];
        }

    }

    public void GameOver()
    {
        ProcessMode = ProcessModeEnum.Disabled;
        _mobTimer.Stop();
        _gameOverScreen.Show();

        if (HUD.Instance.Score > SaveLoad.Instance.HighestScore)
        {
            SaveLoad.Instance.HighestScore = (uint)HUD.Instance.Score;
            _highScoreLabel.Text = "High Score \n" + HUD.Instance.Score.ToString();
        }

        SaveLoad.Instance.SaveScore();

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

    private void OnHomeScreenPressed()
    {
        GetTree().ChangeSceneToPacked(HomeScreen);
    }

    private void OnOptionsButtonPressed()
    {
        _optionsPanel.Show();
        ProcessMode = ProcessModeEnum.Disabled;
    }

    private void OnBackButtonPressed()
    {
        _optionsPanel.Hide();
        ProcessMode = ProcessModeEnum.Inherit;

    }

    private void OnMenuButtonPressed()
    {
        GetTree().ChangeSceneToPacked(HomeScreen);
    }
}
