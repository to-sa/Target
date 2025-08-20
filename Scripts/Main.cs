using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Target.Scenes;
using Target.User;
using Target.Weapons;

namespace Target.Scripts;

public partial class Main : Control
{
    [Export] public PackedScene MobScene;
    [Export] public Array<Texture2D> Backgrounds;

    PackedScene HomeScreen = GD.Load<PackedScene>("res://Scenes/UI/MainMenu.tscn");
    PackedScene Axe = GD.Load<PackedScene>("res://Weapons/Axe.tscn");

    // Entities
    private Player _player;
    private PathFollow2D _mobPathFollow;
    private Path2D _mobPath;
    private Timer _mobTimer;
   
   // Weapons
    private Sword _sword;
    private Shield _shield;
    private Timer _axeTimer;
   
    // UI
    private Panel _gameOverScreen;
    private Panel _optionsPanel;
    private TextureRect _background;
    private Label _highScoreLabel;

    public override void _EnterTree()
    {
        _player = GetNode<Player>("%Player");
        _mobTimer = GetNode<Timer>("Mobs/MobTimer");
        _mobPathFollow = GetNode<PathFollow2D>("%SpawnLocation");
        _mobPath = GetNode<Path2D>("Mobs/MobPath");
        _gameOverScreen = GetNode<Panel>("%GameOver");

        _sword = GetNode<Sword>("%Sword");
        _shield = GetNode<Shield>("%Shield");

        _optionsPanel = GetNode<Panel>("HUD/OptionsPanel");
        _highScoreLabel = GetNode<Label>("%HighScoreLabel");
        _background = GetNode<TextureRect>("Background");
        _axeTimer = GetNode<Timer>("%AxeTimer");
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
            _background.Texture = Backgrounds[0];
            _axeTimer.Start();
        }

        if (HUD.Instance.Score == 40)
        {
            _mobTimer.WaitTime = 1.0f;
            _background.Texture = Backgrounds[2];
            _sword.Speed = 220;
            _shield.Speed = 220;
        }

        if (HUD.Instance.Score == 60)
        {
            _mobTimer.WaitTime = 0.8f;
            _sword.Show();
            _sword.CollisionMask = 1;
            _axeTimer.WaitTime = 1.2f;
        }

        if (HUD.Instance.Score == 90)
        {
            _mobTimer.WaitTime = 0.7f;
            _shield.Show();
            _shield.CollisionMask = 1;
            _background.Texture = Backgrounds[4];
        }

        if (HUD.Instance.Score == 200)
        {
            _background.Texture = Backgrounds[1];
            _mobTimer.WaitTime = 0.45f;
            _player.fireRate.WaitTime = 0.25;
            _sword.Speed = 220;
            _shield.Speed = 220;
        }

         if (HUD.Instance.Score == 250)
        {
            _background.Texture = Backgrounds[3];
            _mobTimer.WaitTime = 0.30f;
            _player.fireRate.WaitTime = 0.15;
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

    private void OnAxeTimeout()
    {
        SpawnAxe();
    }

    private void SpawnAxe()
    {
        Axe axe = Axe.Instantiate<Axe>();
        GetTree().CurrentScene.AddChild(axe);
        axe.GlobalPosition = _player.GlobalPosition;
    }
}
