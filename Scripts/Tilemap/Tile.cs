
namespace CustomTilemap;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

public class ScreenTileInfo
{
    static int numHorizontalTiles = 20;
    static int numVerticalTiles = 20;
    static float unitsPerTile;
    public static Vector2I currentCameraBounds;
    public static Vector2I currentScreen;
    public static Vector2I currentScreenTileOrigin; //
    // public ScreenTileInfo(Camera cam, float unitsPerTile)
    // {
    //     cameraBounds = cam.CameraWorldSize();
    //     this.unitsPerTile = unitsPerTile;
    //     TileToWorldScaler.instance = new TileToWorldScaler { UnitsPerTile = unitsPerTile, OffsetX = 0, OffsetY = 0 };
    // }

}

public static class LevelDataInfo
{
    public static Vector2I worldPosAtTilemapOrigin;
    public static Vector2I WorldToTile(Vector2I worldPos)
    {
        return worldPos - worldPosAtTilemapOrigin;
    }
    public static Vector2I TileToWorld(Vector2I tilePos)
    {
        return tilePos + worldPosAtTilemapOrigin;
    }
}
public enum TileType
{
    Floor, Wall
}

public static class TileStringName
{
    public static StringName wall = "wall";
    public static StringName player = "player";
    // public static StringName npc = "npc";
    public static string tileType = "tiletype";
    public static string dialog = "dialog";
    // public static string npcID = "npcID";
}