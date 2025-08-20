using Godot;
using System;


public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    [Export] public AudioStreamPlayer2D HitSound;
    [Export] public AudioStreamPlayer Music;

    private Timer _timer;

    public override void _Ready()
    {
        Instance = this;

        _timer = new Timer();
        AddChild(_timer);
        _timer.Timeout += OnDelayTimeout;
        _timer.WaitTime = 3.0f;
        _timer.OneShot = true;
    }

    private void OnMusicFinished()
    {
        _timer.Start();
    }

    private void OnDelayTimeout()
    {
        Music.Play();   
    }
}
