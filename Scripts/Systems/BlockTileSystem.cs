namespace MyECS;
using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class BlockTileSystem : System
{
    public Filter EntityFilter;
    Tilemap tilemap;

    public BlockTileSystem(World world, Tilemap tilemap) : base(world)
    {
        this.tilemap = tilemap;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<BlocksTile>()
            .Exclude<Killed>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        tilemap.PopulateBlocked();
        tilemap.ClearTileContents();
        foreach (Entity entity in EntityFilter.Entities)
        {
            Vector2I position = Get<Position>(entity).Value;
            int tileID = tilemap.xy_id(position.X, position.Y);
            tilemap.blocked[tileID] = true;
            tilemap.tileContents[tileID].Add(entity);
        }
    }
}