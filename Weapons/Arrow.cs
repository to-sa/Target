using Godot;
using GodotPlugins.Game;
using Target.Scenes;
using Target.Scripts;

namespace Target.Weapons;

public partial class Arrow : Area2D
{
    const float Speed = 800;

    public override void _Ready()
    {
        this.AreaEntered += OnTargetEntered;
    }

    public override void _Process(double delta)
    {
        Position += Transform.X * Speed * (float)delta;
    }

    private void OnTargetEntered(Area2D area)
    {
        if (area is not Barrel barrel) return;
        barrel.Anim.Play("hit");
        SoundManager.Instance.HitSound.Play();
        HUD.Instance.AddScore();
        QueueFree();
    }

    private void OnVisibleScreenExited()
    {
        QueueFree();
    }
}
