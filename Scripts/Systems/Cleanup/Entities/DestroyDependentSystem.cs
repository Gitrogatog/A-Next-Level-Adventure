namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class DependentSystem<T> : System where T : unmanaged
{
    public Filter EntityFilter;

    public DependentSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Dependent<T>>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (OutRelationCount<T>(entity) <= 0)
            {
                Destroy(entity);
            }
        }
    }
}