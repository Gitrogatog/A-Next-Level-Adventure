namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class CreateDeadNPCSystem : System
{
    public Filter EntityFilter;
    float _timeBetweenFlashes;
    float _lifetime;
    Color hurt1;
    Color hurt2;
    public CreateDeadNPCSystem(World world, float timeBetweenFlashes, float lifetime, Color color1, Color color2) : base(world)
    {
        hurt1 = color1;
        hurt2 = color2;
        _timeBetweenFlashes = timeBetweenFlashes;
        _lifetime = lifetime;
        EntityFilter = FilterBuilder
            .Include<Killed>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            // Remove<BlocksTile>(entity);
            Remove<Killed>(entity);
            Remove<Level>(entity);
            Remove<HasDialog>(entity);
            Remove<NPC>(entity);
            Set(entity, new Components.Timer(_lifetime));
            Set(entity, new DestroyOnTimerEnd());

            Color spriteColor = hurt1; //Get<SpriteColor>(entity).Value;
            Color hurtColor = hurt2;  //Get<HurtColor>(entity).Value;
            Set(entity, new SpriteColor(hurtColor));

            // Color backColor = Get<BackColor>(entity).Color;
            // Vector2I position = Get<Position>(entity).Value;
            // int spriteID = Get<Sprite>(entity).ID;

            // var deadNPC = CreateEntity();


            // Set(deadNPC, new Position(position));
            // Set(deadNPC, new BackColor())

            var color1 = CreateFlashChild(spriteColor, entity);
            var color2 = CreateFlashChild(hurtColor, entity);


            Set(color2, new Components.Timer(_timeBetweenFlashes));
            Relate(color1, color2, new NextColor());
            Relate(color2, color1, new NextColor());
            // Set(color2, new ActiveColor());

            // Get<Dependent<Interacted>>(child).HasEntity(child, World);
        }
    }
    Entity CreateFlashChild(Color color, Entity parent)
    {
        Entity entity = CreateEntity();
        Set(entity, new FlashColor(color));
        Relate(entity, parent, new SpriteModder());
        Set(entity, new Dependent<SpriteModder>());
        return entity;
    }
}