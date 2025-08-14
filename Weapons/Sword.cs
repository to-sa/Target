using Godot;
using System;
using Target.Scenes;
using Target.Scripts;

public partial class Sword : Area2D
{
    public override void _Process(double delta)
    {
        RotationDegrees += 150 * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Barrel barrel) return;
        barrel.Anim.Play("hit");
        SoundManager.Instance.HitSound.Play();
        HUD.Instance.AddScore();
    }

}
