namespace MyECS;
using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class CameraFollowSystem : System
{
    public Filter EntityFilter;
    Vector2I currentRoom; // in room tiles
    Rect2I currentBounds; // in tiles
    Vector2I roomSize; // size of room tile in tiles
    int tileSize; // size of tile in pixels
    Camera2D camera;

    public CameraFollowSystem(World world, Vector2I roomSize, int tileSize, Camera2D camera) : base(world)
    {
        this.roomSize = roomSize;
        this.tileSize = tileSize;
        this.camera = camera;
        // currentRoom = Vector2I.Zero;
        // currentBounds = new Rect2I(currentRoom, roomSize);
        UpdateRoom(Vector2I.Zero);
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<ControlledByPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2I tilemapPos = Get<Position>(entity).Value;
            Vector2I position = LevelDataInfo.TileToWorld(tilemapPos);
            // GD.Print($"entity position: {position}");
            // GD.Print($"current bounds: {currentBounds}");
            UpdateRoom(position);
            // if (!IsWithinBounds(position))
            // {
            //     UpdateRoom(position);
            // }
        }
    }
    bool IsWithinBounds(Vector2I position)
    {
        return currentBounds.HasPoint(position);
    }
    void UpdateRoom(Vector2I position)
    {
        currentRoom = new Vector2I(GetRoomPos(position.X, roomSize.X), GetRoomPos(position.Y, roomSize.Y));
        Vector2I startTilePosition = currentRoom * roomSize;
        currentBounds = new Rect2I(startTilePosition, roomSize + new Vector2I(1, 1));
        camera.Position = startTilePosition * tileSize;

        ScreenTileInfo.currentScreen = currentRoom;
        ScreenTileInfo.currentScreenTileOrigin = LevelDataInfo.WorldToTile(startTilePosition);
        // GD.Print(ScreenTileInfo.currentScreenTileOrigin);
    }

    int GetRoomPos(int position, int roomLength)
    {
        if (position < 0 && position % roomLength != 0)
        {
            return position / roomLength - 1;
        }
        return position / roomLength;
    }
    // void MoveRoom(Vector2I position)
    // {

    //     Vector2I displacement = Vector2I.Zero;
    //     if (position.X < currentBounds.Position.X)
    //     {
    //         displacement.X -= 1;
    //     }
    //     else if (position.X > currentBounds.End.X)
    //     {
    //         displacement.X += 1;
    //     }
    //     if (position.Y < currentBounds.Position.Y)
    //     {
    //         displacement.Y -= 1;
    //     }
    //     else if (position.Y > currentBounds.Position.Y)
    //     {
    //         displacement.Y += 1;
    //     }
    //     currentRoom += displacement;
    //     Vector2I startTilePosition = currentRoom * roomSize;
    //     currentBounds = new Rect2I(startTilePosition, roomSize + new Vector2I(1, 1));
    //     camera.Position = startTilePosition * tileSize;
    //     GD.Print($"new bounds: {currentBounds}");
    // }
}