using Godot;
using System;

[GlobalClass]
public partial class NPCInfoResource : Resource
{
    [Export] public string name = "";
    [Export] public string displayName;
    [Export] public string killName;
    [Export] public int power;
    [Export] public bool displayPower;
    [Export] public Texture2D texture;
    [Export] public Color color;
    [Export] public Color hurtColor = Colors.White;
    [Export] public DialogStorageResource dialog;

}
