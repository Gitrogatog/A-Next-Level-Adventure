namespace MyECS.Relations;
using System;
using MoonTools.ECS;

public readonly record struct HasCollider();
public readonly record struct Wielding();
public readonly record struct Parent();
public readonly record struct Invincible();
public readonly record struct Interacted(int Count);
public readonly record struct NextColor;
public readonly record struct ActiveColor;
public readonly record struct SpriteModder;
public struct Dependent<T> where T : unmanaged
{
    public static bool HasEntity(Entity entity, World world)
    {
        return world.HasOutRelation<T>(entity);
    }
    // public bool HasEntity(Entity entity, World world)
    // {
    //     return world.HasOutRelation<T>(entity);
    // }
}