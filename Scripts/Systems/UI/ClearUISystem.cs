namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ClearUISystem : System
{
    public RichTextLabel[] textToClear;

    public ClearUISystem(World world, RichTextLabel[] text) : base(world)
    {
        textToClear = text;
        ClearText();
    }
    public override void Update(TimeSpan delta)
    {
        ClearText();
    }
    void ClearText()
    {
        foreach (RichTextLabel text in textToClear)
        {
            text.Text = "";
        }
    }
}