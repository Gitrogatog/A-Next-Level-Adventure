namespace MyECS;
using System;
using System.ComponentModel;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;


public class FlashColorsSystem : System
{
    public Filter EntityFilter;

    public FlashColorsSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Components.Timer>()
            .Include<FlashColor>()

            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            // GD.Print("flash color running!");
            var timer = Get<Components.Timer>(entity);
            if (timer.Time <= 0f)
            {
                var parent = OutRelationSingleton<SpriteModder>(entity);
                var next = OutRelationSingleton<NextColor>(entity);
                Color color = Get<FlashColor>(next).Value;
                Set(parent, new SpriteColor(color));
                Set(next, new Components.Timer(timer.Max));
                Remove<Components.Timer>(entity);
            }

        }
    }
}