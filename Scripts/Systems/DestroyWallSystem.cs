namespace MyECS;
using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Prefabs;

public class DestroyWallSystem : System
{
    public Filter EntityFilter;
    Tilemap _tilemap;
    TileMap _levelData;

    public DestroyWallSystem(World world, Tilemap tilemap, TileMap levelData) : base(world)
    {
        _tilemap = tilemap;
        _levelData = levelData;
        EntityFilter = FilterBuilder
            .Include<DestroyedWall>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            // remove wall collision from tilemap
            Vector2I position = Get<DestroyedWall>(entity).Position;
            int tileID = _tilemap.xy_id(position.X, position.Y);
            _tilemap.tiles[tileID] = TileType.Floor;

            // send defeat message with wall name
            string wallName = _tilemap.wallNames[tileID];
            string capitalizedWall = UppercaseStorage.GetUpper(wallName);
            int nameID = TextStorage.GetID(capitalizedWall);
            MessagePrefabs.CreateDefeat(World, nameID);

            // remove tile from godot tileMap (for visual purposes)
            Vector2I worldPos = LevelDataInfo.TileToWorld(position); // convert from tilemap pos to world pos
            _levelData.SetCell(1, worldPos);
        }
    }
}