namespace MyECS.Prefabs;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;
using TextureUtils;

public static class TileEntityPrefabs
{
    public static Entity CreatePlayer(World world, Vector2I tilePos)
    {
        var player = world.CreateEntity();
        world.Set(player, new Position(tilePos));
        world.Set(player, new ControlledByPlayer());
        world.Set(player, new Sprite(SpriteStorage.GetID(LoadedTextures.playerTexture)));
        world.Set(player, new BlocksTile());
        world.Set(player, new Level(1));
        world.Set(player, new BackColor(new Color(32 / 255f, 18 / 255f, 8 / 255f)));
        // GD.Print("created player!");
        return player;
    }
    public static Entity CreateNPC(World world, int npcID, int dialogID, Vector2I tilePos)
    {
        var npc = world.CreateEntity();
        world.Set(npc, new Position(tilePos));
        world.Set(npc, new BlocksTile());
        world.Set(npc, new NPC(npcID));
        world.Set(npc, new HasDialog(dialogID));
        return npc;
    }
    // public static Entity CreateMovementMessageEntity(World world, Entity entityToMove, Vector2 velocity)
    // {
    //     var moveEntity = world.CreateEntity();
    //     world.Set(moveEntity, new Velocity(velocity));
    //     world.Set(moveEntity, new MarkedForDestroyOnPostCollision());
    //     world.Relate(moveEntity, entityToMove, new MovementModifier());
    //     return moveEntity;
    // }
}

public static class MessagePrefabs
{
    static Entity CreateMessageEntity(World world)
    {
        var entity = world.CreateEntity();
        world.Set(entity, new MarkedForDestroy());
        return entity;
    }
    public static Entity CreateDialog(World world, int npcID, int dialogID, int lineID)
    {
        var entity = CreateMessageEntity(world);
        world.Set(entity, new ShowDialog(npcID, dialogID, lineID));

        return entity;
    }
    public static Entity CreateDefeat(World world, int nameID)
    {
        var entity = CreateMessageEntity(world);
        world.Set(entity, new ShowDefeat(nameID));
        return entity;
    }
    public static Entity DestroyWall(World world, Vector2I wallPos)
    {
        var entity = CreateMessageEntity(world);
        world.Set(entity, new DestroyedWall(wallPos));
        return entity;
    }
    public static Entity BonkWall(World world, string wallName)
    {
        var entity = CreateMessageEntity(world);
        world.Set(entity, new BonkedWall(TextStorage.GetID(wallName)));
        return entity;
    }
    public static Entity Alert(World world, string alertText)
    {
        var entity = world.CreateEntity();
        world.Set(entity, new AlertText(TextStorage.GetID(alertText)));
        return entity;

    }
    // public static Entity CreateLevelDisplay(World world, int level)
    // {
    //     var entity = CreateMessageEntity(world);
    //     world.Set(entity, new ShowLevel(level));
    //     return entity;
    // }
    // public static Entity CreateNameDisplay(World world, int nameID)
    // {
    //     var entity = CreateMessageEntity(world);
    //     world.Set(entity, new ShowName(nameID));
    //     return entity;
    // }
}