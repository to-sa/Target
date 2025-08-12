using Godot;
using System;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    [Export] public AudioStreamPlayer2D HitSound;
    [Export] public AudioStreamPlayer2D PlayerHitSound;

    public override void _Ready()
    {
        Instance = this;
    }

}
