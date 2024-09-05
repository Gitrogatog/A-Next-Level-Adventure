namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class EnableWallBustingSystem : System
{
    public Filter EntityFilter;

    public EnableWallBustingSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Killed>()
            .Include<EnableWallBustingOnDeath>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (EntityFilter.Count > 0)
        {
            GlobalFlags.CanDestroyWalls = true;
        }
    }
}