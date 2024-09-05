namespace MyECS.Messages;
using System;
using Godot;
using MoonTools.ECS;

// actual messages
// public readonly record struct CreateBullet(Entity Source, Godot.CollisionLayer Mask, Vector2 Position, float Angle);

// relations used as messages (but are actually components on entities)
public readonly record struct IsCollision();
public readonly record struct Colliding();
public readonly record struct MovementModifier();
public readonly record struct Damage(Entity Target, Entity Source, int Amount);
