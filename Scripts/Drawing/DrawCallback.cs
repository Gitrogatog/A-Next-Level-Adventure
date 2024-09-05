using Godot;
using System;

public partial class DrawCallback : Node2D
{
    public event Action OnDraw = delegate { };
    public override void _Process(double delta)
    {
        QueueRedraw();
    }
    public override void _Draw()
    {
        OnDraw.Invoke();

    }
}
