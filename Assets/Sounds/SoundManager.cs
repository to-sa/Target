using Godot;
using System;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    [Export] public AudioStreamPlayer2D HitSound;
    [Export] public AudioStreamPlayer MusicOne;
    [Export] public AudioStreamPlayer MusicTwo;

    public override void _Ready()
    {
        Instance = this;
    }

}
