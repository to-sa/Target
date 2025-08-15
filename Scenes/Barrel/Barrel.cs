using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Target.Scripts;
using Target.User;
using Target.Weapons;

namespace Target.Scenes;

[GlobalClass]
public partial class Barrel : Area2D
{

    [Export] private float _moveSpeed = GD.RandRange(200, 300);
    public AnimatedSprite2D Anim;
    private Area2D _player;
    private int _rotationSpeed = GD.RandRange(-50, 50);
    private Tween _tween;

    public override void _Ready()
    {
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetTree().CurrentScene.GetNode<Player>("PlayerPosition/Player");

        int CurrentFrame = Anim.SpriteFrames.GetFrameCount("box");
        Anim.Frame = GD.RandRange(0, CurrentFrame);

        AreaEntered += OnAreaEntered;
        Anim.AnimationFinished += OnAnimationFinished;
    }

    public override void _Process(double delta)
    {
        Position += Position.DirectionTo(_player.GlobalPosition) * _moveSpeed * (float)delta;
        RotationDegrees += _rotationSpeed * (float)delta;
    }

    private void OnScreenExited()
    {
        QueueFree();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Player player) return;
        if (!player.CanTakeDamage)
        {
            QueueFree();
            return;
        }

        SoundManager.Instance.HitSound.Play();
        DoDamage(player);
        Anim.Play("hit");
        player.CanTakeDamage = false;
    }

    private void OnAnimationFinished()
    {
        QueueFree();
    }

    private void DoDamage(Player player)
    {
        if (player.health <= 0) return;

        player.health -= 1;
        var HealthTexture = HUD.Instance.HealthBar.GetChildren().ElementAt(0);
        HealthAnimation(HealthTexture);
    }

    private void HealthAnimation(Node texture)
    {
        _tween = GetTree().CreateTween();
        _tween.TweenProperty(texture, "scale", new Vector2(1.5f, 1.5f), 0.1f);
        _tween.TweenProperty(texture, "modulate", Colors.Black, 0.1f);
        _tween.TweenCallback(Callable.From(texture.QueueFree));
    }
}
