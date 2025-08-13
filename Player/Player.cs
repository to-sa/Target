using Godot;
using System;
using System.Linq;
using Target.Scenes;
using Godot.Collections;
using Target.Weapons;

namespace Target.User;

[GlobalClass]
public partial class Player : Area2D
{
    private PackedScene _arrowScene = GD.Load<PackedScene>("Weapons/Arrow.tscn");

    [Export] public AttackRange Range;

    const int AXE = 0;

    // Child nodes
    private Marker2D _spawnPosition;
    private Timer _fireRate;
    private AnimatedSprite2D _weaponSprite;
    private AudioStreamPlayer2D _drawSound;

    private bool _canFire = true;

    public int health = 3;


    public override void _Ready()
    {
        _spawnPosition = GetNode<Marker2D>("Body/LeftArm/FirePosition");

        _weaponSprite = GetNode<AnimatedSprite2D>("Body/LeftArm/Weapons");
        _weaponSprite.AnimationFinished += BowAnimation;

        _drawSound = GetNode<AudioStreamPlayer2D>("FireSound");

        _fireRate = GetNode<Timer>("FireRate");
        _fireRate.Timeout += CanFireTimeout;
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
        _fireRate.Start();
    }

    private void CanFireTimeout()
    {
        _canFire = true;
    }

    private void BowAnimation()
    {
        _weaponSprite.Play("draw");
    }
}
