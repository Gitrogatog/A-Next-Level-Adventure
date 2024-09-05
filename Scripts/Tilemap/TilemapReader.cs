namespace CustomTilemap;
using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using Utils;
using MoonTools.ECS;
using MyECS.Prefabs;
using MyECS.Components;

public class TilemapReader
{
    TileMap levelData;
    World world;
    Dictionary<string, int> nameToID;
    public TilemapReader(TileMap levelData, World world, Dictionary<string, int> nameToID)
    {
        this.levelData = levelData;
        this.world = world;
        this.nameToID = nameToID;
    }
    public Tilemap ReadTilemap()
    {
        Rect2I usedRect = levelData.GetUsedRect();
        Vector2I startCorner = usedRect.Position;
        Vector2I endCorner = usedRect.End;
        int xTotalTiles = endCorner.X - startCorner.X;
        int yTotalTiles = endCorner.Y - startCorner.Y;
        LevelDataInfo.worldPosAtTilemapOrigin = startCorner;

        // GD.Print($"start: {startCorner}, end: {endCorner}");
        Tilemap tilemap = new Tilemap(xTotalTiles, yTotalTiles);
        int tileIndex = 0;
        for (int y = startCorner.Y; y < endCorner.Y; y++)
        {
            for (int x = startCorner.X; x < endCorner.X; x++)
            {
                Vector2I tilemapPos = tilemap.id_to_xy(tileIndex);
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                ReadBackgroundTile(0, tileIndex, new Vector2I(x, y), tilemapPos, tilemap);
                ReadWallTile(1, tileIndex, new Vector2I(x, y), tilemapPos, tilemap);
                tileIndex++;
            }
        }
        return tilemap;
    }
    //

    public void ReadWallLayer(Tilemap tilemap)
    {
        Rect2I usedRect = levelData.GetUsedRect();
        Vector2I startCorner = usedRect.Position;
        Vector2I endCorner = usedRect.End;
        int tileIndex = 0;
        for (int y = startCorner.Y; y < endCorner.Y; y++)
        {
            for (int x = startCorner.X; x < endCorner.X; x++)
            {
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                Vector2I tilemapPos = tilemap.id_to_xy(tileIndex);
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                ReadEntityTile(2, tileIndex, new Vector2I(x, y), tilemapPos, tilemap);

                tileIndex++;
            }
        }
        // levelData.SetLayerEnabled(1, false);
    }
    public void ReadEntityLayer(Tilemap tilemap)
    {
        Rect2I usedRect = levelData.GetUsedRect();
        Vector2I startCorner = usedRect.Position;
        Vector2I endCorner = usedRect.End;
        int xTotalTiles = endCorner.X - startCorner.X;
        int yTotalTiles = endCorner.Y - startCorner.Y;
        // GD.Print($"start: {startCorner}, end: {endCorner}");
        // Tilemap tilemap = new Tilemap(xTotalTiles, yTotalTiles);
        int tileIndex = 0;
        for (int y = startCorner.Y; y < endCorner.Y; y++)
        {
            for (int x = startCorner.X; x < endCorner.X; x++)
            {
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                Vector2I tilemapPos = tilemap.id_to_xy(tileIndex);
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                ReadEntityTile(2, tileIndex, new Vector2I(x, y), tilemapPos, tilemap);

                tileIndex++;
            }
        }
        levelData.SetLayerEnabled(2, false);
    }

    void ReadBackgroundTile(int layerIndex, int tileIndex, Vector2I levelPos, Vector2I tilemapPos, Tilemap tilemap)
    {
        TileData tileData = levelData.GetCellTileData(layerIndex, levelPos);
        if (tileData != null)
        {
            StringName tileTypeMeta = (StringName)tileData.GetCustomData(TileStringName.tileType);
            tilemap.floorNames[tileIndex] = tileTypeMeta;
        }
    }
    void ReadWallTile(int layerIndex, int tileIndex, Vector2I levelPos, Vector2I tilemapPos, Tilemap tilemap)
    {
        TileData tileData = levelData.GetCellTileData(layerIndex, levelPos);

        // GD.Print($"level pos: {levelPos}, tilemap: {tilemapPos}");
        if (tileData != null)
        {
            tilemap.tiles[tileIndex] = TileType.Wall;
            StringName tileTypeMeta = (StringName)tileData.GetCustomData(TileStringName.tileType);
            tilemap.wallNames[tileIndex] = tileTypeMeta;
        }

    }
    void ReadEntityTile(int layerIndex, int tileIndex, Vector2I levelPos, Vector2I tilemapPos, Tilemap tilemap)
    {
        TileData tileData = levelData.GetCellTileData(layerIndex, levelPos);
        if (tileData != null)
        {
            StringName tileTypeMeta = (StringName)tileData.GetCustomData(TileStringName.tileType);
            Vector2 localPos = levelData.MapToLocal(levelPos);
            Entity entity;
            // GD.Print($"tile type: {tileTypeMeta}");
            // GD.Print($"contains: {nameToID.ContainsKey(tileTypeMeta)}");
            if (tileTypeMeta == TileStringName.player)
            {
                // GD.Print("has player");

                // Vector2 renderPos = 
                // GD.Print($"tile pos: {tilemapPos}, level pos: {levelPos}, localPos: {localPos}");
                entity = TileEntityPrefabs.CreatePlayer(world, tilemapPos);
            }
            else if (nameToID.ContainsKey(tileTypeMeta))
            {
                int npcID = nameToID[tileTypeMeta];
                int dialogID = (int)tileData.GetCustomData(TileStringName.dialog);
                entity = TileEntityPrefabs.CreateNPC(world, npcID, dialogID, tilemapPos);
                int dialogMeta = (int)tileData.GetCustomData(TileStringName.dialog);
                if (dialogMeta >= 0)
                {
                    world.Set(entity, new HasDialog(dialogMeta));
                }
            }

        }
    }
}