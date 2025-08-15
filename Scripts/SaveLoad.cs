using Godot;
using System;
using System.IO;
using FileAccess = Godot.FileAccess;

namespace Target.Scripts;

public partial class SaveLoad : Node
{
    public static SaveLoad Instance { get; private set; }

    public const string SaveFile = "user://score.save";

    public uint HighestScore = 0;

    public override void _Ready()
    {
        Instance = this;
        LoadScore();
    }

    public void SaveScore()
    {
        using var file = FileAccess.Open(SaveFile, FileAccess.ModeFlags.WriteRead);
        file.Store32((uint)HighestScore);
    }

    public void LoadScore()
    {
        using var file = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Read);
        if (FileAccess.FileExists(SaveFile))
        {
            HighestScore = file.Get32();
        }
    }

    public void ResetScore()
    {
        HighestScore = 0;
        
        using var file = FileAccess.Open(SaveFile, FileAccess.ModeFlags.WriteRead);
        file.Store32((uint)HighestScore);
    }

}



