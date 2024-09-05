namespace MyECS;
using System;
using MoonTools.ECS;
using MyECS.Components;

public class DestroyMarkedTimerSystem : System
{
    public Filter EntityFilter;

    public DestroyMarkedTimerSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Timer>()
            .Include<DestroyOnTimerEnd>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (Get<Timer>(entity).Time <= 0)
            {
                Destroy(entity);
            }
        }
    }
}