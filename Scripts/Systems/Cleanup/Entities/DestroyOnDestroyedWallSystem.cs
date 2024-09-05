namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DestroyOnDestroyedWallSystem : System
{
    public Filter DestroyFilter;
    public Filter WallFilter;

    public DestroyOnDestroyedWallSystem(World world) : base(world)
    {
        DestroyFilter = FilterBuilder
            .Include<DestroyOnDestroyedWall>()
            .Build();
        WallFilter = FilterBuilder.Include<DestroyedWall>().Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (WallFilter.Count == 0) return;
        foreach (var entity in DestroyFilter.Entities)
        {
            Destroy(entity);
        }
    }
}