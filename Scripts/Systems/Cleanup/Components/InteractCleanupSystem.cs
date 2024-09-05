namespace MyECS;
using System;
using MoonTools.ECS;
using MyECS.Components;

public class InteractCleanupSystem : System
{
    public Filter MoveTileFilter;

    public InteractCleanupSystem(World world) : base(world)
    {
        MoveTileFilter = FilterBuilder
            .Include<InteractedThisFrame>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in MoveTileFilter.Entities)
        {
            Remove<InteractedThisFrame>(entity);
        }
    }
}