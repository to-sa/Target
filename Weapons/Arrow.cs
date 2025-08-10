using Godot;
using System;

namespace Target.Weapons;

public partial class Arrow : Area2D
{
    const float Speed = 700;

    public override void _Process(double delta)
    {
        Position += Transform.X * Speed * (float)delta;
    }

}
