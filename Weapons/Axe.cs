using Godot;
using System;
using Target.Scenes;
using Target.Scripts;

namespace Target.Weapons;

[GlobalClass]
public partial class Axe : Area2D
{
    const int Speed = 400;

    public override void _Process(double delta)
    {
        Position += Transform.X * Speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Barrel barrel) return;
        barrel.Anim.Play("hit");
        SoundManager.Instance.HitSound.Play();
        HUD.Instance.AddScore();
        QueueFree();
    }

    private void OnScreenExited()
    {
        QueueFree();
        GD.Print("Deleted axe");
    }

}
