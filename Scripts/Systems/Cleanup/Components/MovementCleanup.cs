namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class MovementCleanupSystem : System
{
    public Filter MoveTileFilter;

    public MovementCleanupSystem(World world) : base(world)
    {
        MoveTileFilter = FilterBuilder
            .Include<Position>()
            .Include<MoveTile>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in MoveTileFilter.Entities)
        {
            Remove<MoveTile>(entity);
        }
    }
}