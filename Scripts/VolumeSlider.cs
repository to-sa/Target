using Godot;
using System;

public partial class VolumeSlider : HSlider
{
    [Export] string BusName;
    public int BusIndex;

    public override void _Ready()
    {
        BusIndex = AudioServer.GetBusIndex(BusName);
        ValueChanged += OnValueChanged;

        Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(BusIndex));
    }

    public void OnValueChanged(double value)
    {
        AudioServer.SetBusVolumeDb(
            BusIndex,
            Mathf.LinearToDb((float)value)
        );
    }

}
