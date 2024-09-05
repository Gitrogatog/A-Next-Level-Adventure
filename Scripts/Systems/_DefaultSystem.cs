namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DefaultSystem : System
{
    public Filter EntityFilter;

    public DefaultSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {

        }
    }
}