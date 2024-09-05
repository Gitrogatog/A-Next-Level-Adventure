namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class KillNPCSystem : System
{
    public Filter EntityFilter;

    public KillNPCSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Killed>()
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