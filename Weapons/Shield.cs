using Godot;
using System;
using Target.Scenes;
using Target.Scripts;

namespace Target.Weapons;
[GlobalClass]
public partial class Shield : Area2D
{
    public float Speed = 150;

    public override void _Process(double delta)
    {
        RotationDegrees -= Speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Barrel barrel) return;
        barrel.Anim.Play("hit");
        SoundManager.Instance.HitSound.Play();
    }
}
