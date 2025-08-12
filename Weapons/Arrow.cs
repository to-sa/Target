using Godot;
using Target.Scenes;

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
        QueueFree();
    }
}
