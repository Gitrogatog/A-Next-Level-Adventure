using Godot;
using System;

public partial class VolumeSlider : HSlider
{
    [Export] string AudioBusName;
    int busIndex;
    [Export] public bool Enabled;
    public override void _Ready()
    {
        busIndex = AudioServer.GetBusIndex(AudioBusName);
        // GD.Print($"bus index: {busIndex}");
        ValueChanged += OnValueChanged;
        FocusMode = FocusModeEnum.None;
        // Value = Mathf.LinearToDb
    }
    void OnValueChanged(double value)
    {
        // GD.Print("Running on value changed!");
        if (Enabled)
        {
            AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb((float)value));
            // GD.Print("volume is enabled!");

        }
    }
}
