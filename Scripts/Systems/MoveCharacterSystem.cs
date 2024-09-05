namespace MyECS;
using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Prefabs;
using MyECS.Relations;

public class MoveCharacterSystem : System
{
    public Filter EntityFilter;
    Tilemap tilemap;
    int destroyWallLevel = 150;
    public MoveCharacterSystem(World world, Tilemap tMap) : base(world)
    {
        tilemap = tMap;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<MoveTile>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Remove<BlockedMove>(entity);
            TileDirection direction = Get<MoveTile>(entity).Direction;
            Vector2I position = Get<Position>(entity).Value;
            Vector2I ogPosition = position;
            position += direction switch
            {
                TileDirection.North => Vector2I.Up,
                TileDirection.South => Vector2I.Down,
                TileDirection.West => Vector2I.Left,
                TileDirection.East => Vector2I.Right,
                _ => Vector2I.Zero
            };
            if (tilemap.TileInBounds(position))
            {
                int tileID = tilemap.xy_id(position.X, position.Y);
                if (!tilemap.IsBlocked(tileID))
                {
                    int oldID = tilemap.xy_id(ogPosition.X, ogPosition.Y);
                    tilemap.blocked[tileID] = true;
                    tilemap.blocked[oldID] = false;
                    Set(entity, new Position(position));
                    Set(entity, new PrevPosition(ogPosition));

                }
                else if (tilemap.tileContents[tileID].Count > 0)
                {
                    Entity target = tilemap.tileContents[tileID][0];
                    InteractionType interaction = InteractWithEntity(entity, target);
                    if (!GlobalFlags.CanDestroyWalls || interaction == InteractionType.Talk)
                    {
                        Set(entity, new BlockedMove());
                    }

                }
                else // collided with a wall
                {
                    if (World.TryGetComponent(entity, out Level level) && GlobalFlags.CanDestroyWalls)
                    {
                        MessagePrefabs.DestroyWall(World, position);
                        int oldID = tilemap.xy_id(ogPosition.X, ogPosition.Y);
                        tilemap.blocked[tileID] = true;
                        tilemap.blocked[oldID] = false;
                        Set(entity, new Position(position));
                        Set(entity, new PrevPosition(ogPosition));
                        Set(entity, new Level(level.Value + 1));
                    }
                    else
                    {
                        Set(entity, new BlockedMove());
                        MessagePrefabs.BonkWall(World, tilemap.wallNames[tileID]);
                    }

                }
            }

        }
    }
    InteractionType InteractWithEntity(Entity user, Entity target)
    {
        Set(target, new InteractedThisFrame());
        // GD.Print($"Entity {user.ID} interacted with target {target.ID}");
        if (World.TryGetComponent(user, out Level userLevel) && World.TryGetComponent(target, out Level targetLevel))
        {
            // GD.Print($"user Level: {userLevel.Value}, target Level: {targetLevel.Value}");
            if (userLevel.Value >= targetLevel.Value)
            {
                Set(user, new Level(userLevel.Value + 1));
                Set(target, new Killed());
                Remove<BlocksTile>(target);
                MessagePrefabs.CreateDefeat(World, Get<KillName>(target).ID);
                return InteractionType.Kill;
            }
        }
        if (World.TryGetComponent(target, out NPC npc) && World.TryGetComponent(target, out HasDialog dialog))
        {
            int lineID = 0;
            if (HasOutRelation<Interacted>(user))
            {
                Entity interacted = OutRelationSingleton<Interacted>(user);
                if (interacted == target)
                {
                    lineID = GetRelationData<Interacted>(user, target).Count + 1;
                    // this.Relate<Interacted>(user, target, null);
                }
                else
                {
                    UnrelateAll<Interacted>(user);
                }
            }
            Relate(user, target, new Interacted(lineID));

            // GD.Print("talking to target!");
            MessagePrefabs.CreateDialog(World, npc.ID, dialog.ID, lineID);
            return InteractionType.Talk;
        }
        // if(World.TryGetComponent)
        return InteractionType.Fail;
    }
}

public enum InteractionType
{
    Talk, Kill, Fail
}