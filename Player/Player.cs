using Godot;
using System;

namespace Target.Player;
public partial class Player : Area2D
{

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventScreenTouch touch) return;
        LookAt(touch.Position);
    }

}
