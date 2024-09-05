namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DestroyEndOfFrameSystem : System
{
    public Filter EntityFilter;

    public DestroyEndOfFrameSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<MarkedForDestroy>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Destroy(entity);
        }
    }
}