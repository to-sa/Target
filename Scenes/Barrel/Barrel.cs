using Godot;
using Target.User;

namespace Target.Scenes;

[GlobalClass]
public partial class Barrel : Area2D
{

    [Export] private float _moveSpeed = 150;
    public AnimatedSprite2D Anim;
    private Area2D _player;

    public override void _Ready()
    {
        Anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _player = GetTree().CurrentScene.GetNode<Player>("Player");

        this.AreaEntered += OnAreaEntered;
        Anim.AnimationFinished += OnAnimationFinished;
    }

    public override void _Process(double delta)
    {
        Position += Position.DirectionTo(_player.GlobalPosition) * _moveSpeed * (float)delta;
    }

    private void OnScreenExited()
    {
        QueueFree();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Player) return;
        QueueFree();
    }

    private void OnAnimationFinished()
    {
        QueueFree();
    }
}
