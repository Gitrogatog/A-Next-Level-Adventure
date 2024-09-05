using Godot;
using System;

public partial class FastModeButton : CheckBox
{
    public override void _Ready()
    {
        Toggled += SetFastMode;
        // MouseExited += ReleaseFocus;
        FocusMode = FocusModeEnum.None;
    }

    void SetFastMode(bool fastMode)
    {
        GlobalFlags.FastMode = fastMode;
        // ReleaseFocus();

    }
}
