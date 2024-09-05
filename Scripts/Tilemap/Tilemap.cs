namespace CustomTilemap;
using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using Utils;


public class Tilemap
{
    public List<TileType> tiles;
    public List<bool> revealedTiles;
    public List<bool> visibleTiles;
    public List<bool> blocked;
    public List<List<Entity>> tileContents;
    // public List<Rectangle> rooms;
    public List<string> floorNames;
    public List<string> wallNames;
    public int xTiles;
    public int yTiles;
    public int xy_id(int x, int y)
    {
        return (y * xTiles) + x;
    }
    public Vector2I id_to_xy(int idx)
    {
        return new Vector2I(idx % xTiles, idx / xTiles);
    }
    public bool IndexInRange(int index) => index >= 0 && index < xTiles * yTiles;
    public bool TileInBounds(Vector2I tile) => tile.X >= 0 && tile.X < xTiles && tile.Y >= 0 && tile.Y < yTiles;

    public static int xy_id(int x, int y, int xTiles)
    {
        return (y * xTiles) + x;
    }
    public Tilemap(int x, int y)
    {
        xTiles = x;
        yTiles = y;
        int totalTiles = xTiles * yTiles;
        tiles = new List<TileType>(new TileType[totalTiles]);
        revealedTiles = new List<bool>(new bool[totalTiles]);
        visibleTiles = new List<bool>(new bool[totalTiles]);
        blocked = new List<bool>(new bool[totalTiles]);
        floorNames = new List<string>(new string[totalTiles]);
        wallNames = new List<string>(new string[totalTiles]);
        tileContents = new List<List<Entity>>(new List<Entity>[totalTiles]);

        for (int i = 0; i < totalTiles; i++)
        {
            tileContents[i] = new List<Entity>();
        }
    }
    public Tilemap(int x, int y, TileType[] tileList) : this(x, y)
    {
        // xTiles = x;
        // yTiles = y;
        // tiles = new List<TileType>();
        // revealedTiles = new List<bool>(new bool[xTiles * yTiles]);
        // visibleTiles = new List<bool>(new bool[xTiles * yTiles]);
        for (int i = 0; i < tileList.Length; i++)
        {
            TileType tile = tileList[i];
            tiles[i] = tile;
            blocked[i] = tile == TileType.Wall;
        }
        // revealedTiles = new List<bool>(new bool[tileList.Length]);
    }

    public void PopulateBlocked()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            blocked[i] = tiles[i] == TileType.Wall;
        }
    }
    public void ClearTileContents()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tileContents[i].Clear();
        }
    }

    public bool IsOpaque(int idx)
    {
        return tiles[idx] == TileType.Wall;
    }

    public bool IsBlocked(int idx)
    {
        return blocked[idx];
    }

    public bool IsBlocked(int x, int y)
    {
        return IsBlocked(xy_id(x, y));
    }
    public void LogTiles()
    {
        GD.Print("tiles:");
        for (int y = 0; y < yTiles; y++)
        {
            string listOfTiles = "";
            for (int x = 0; x < xTiles; x++)
            {
                int index = xy_id(x, y);
                TileType tileType = tiles[index];
                if (tileType == TileType.Floor)
                {
                    listOfTiles += ".";
                }
                else
                {
                    listOfTiles += "X";
                }
            }
            GD.Print(listOfTiles);
        }
    }

    // public void LogTiles()
    // {
    //     foreach (TileType tile in tiles)
    //     {
    //         Debug.Log(tile);
    //     }
    // }
}