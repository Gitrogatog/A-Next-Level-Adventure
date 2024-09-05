namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class RunPostMoveSystem : ConditionalGroupSystem
{
    public Filter EntityFilter;
    public RunPostMoveSystem(World world, SystemGroup group) : base(world, group)
    {
        EntityFilter = FilterBuilder
                        .Include<Position>()
                        .Include<MoveTile>()
                        .Include<ControlledByPlayer>()
                        .Build();
    }
    protected override bool ShouldRun(TimeSpan delta)
    {
        return EntityFilter.Count > 0;
    }
}