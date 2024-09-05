namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public abstract class ConditionalGroupSystem : System
{
    public SystemGroup Group;
    public ConditionalGroupSystem(World world, SystemGroup group) : base(world)
    {
        Group = group;

    }
    public override void Update(TimeSpan delta)
    {
        if (ShouldRun(delta))
        {
            Group.Run(delta);
        }
    }
    protected abstract bool ShouldRun(TimeSpan delta);
}