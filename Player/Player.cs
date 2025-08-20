using Godot;
using System;
using System.Linq;
using Target.Scenes;
using Godot.Collections;
using Target.Weapons;
using Target.Scripts;

namespace Target.User;

[GlobalClass]
public partial class Player : Area2D
{
    private PackedScene _arrowScene = GD.Load<PackedScene>("Weapons/Arrow.tscn");

    private Marker2D _spawnPosition;
    private Timer _takeDamageTimer;
    private AnimatedSprite2D _weaponSprite;
    private AudioStreamPlayer2D _drawSound;

    private Tween _tween;
    private bool _canFire = true;

    public Timer fireRate;
    public bool CanTakeDamage = true;
    public int health = 3;

    public override void _Ready()
    {
        _spawnPosition = GetNode<Marker2D>("Body/LeftArm/FirePosition");

        _weaponSprite = GetNode<AnimatedSprite2D>("Body/LeftArm/Weapons");
        _weaponSprite.AnimationFinished += BowAnimation;

        _drawSound = GetNode<AudioStreamPlayer2D>("FireSound");

        fireRate = GetNode<Timer>("FireRate");
        fireRate.Timeout += CanFireTimeout;

        _takeDamageTimer = GetNode<Timer>("TakeDamageTimer");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventScreenTouch touch) return;
        if (!_canFire) return;
        ReadyBow(touch.Position);
    }

    private void ReadyBow(Vector2 pos)
    {
        LookAt(pos);
        _weaponSprite.Play("default");
        SpawnArrow();
    }

    private void SpawnArrow()
    {
        var newArrow = _arrowScene.Instantiate<Area2D>();
        GetTree().CurrentScene.AddChild(newArrow);
        newArrow.GlobalPosition = _spawnPosition.GlobalPosition;
        newArrow.GlobalRotation = _spawnPosition.GlobalRotation;
        _drawSound.Play();

        _canFire = false;
        fireRate.Start();
    }

    private void CanFireTimeout()
    {
        _canFire = true;
    }

    private void BowAnimation()
    {
        _weaponSprite.Play("draw");
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Barrel barrel) return;
        Animate();
        _tween.TweenProperty(GetNode("Body"), "modulate", Colors.Red, 0.2f);
        _tween.TweenProperty(GetNode("Body"), "modulate", Colors.White, 0.1f);
        _takeDamageTimer.Start();
    }

    private void Animate()
    {
        _tween?.Kill();
        _tween = CreateTween();
    }

    private void OnDamageTimerTimeout()
    {
        CanTakeDamage = true;
    }
}
