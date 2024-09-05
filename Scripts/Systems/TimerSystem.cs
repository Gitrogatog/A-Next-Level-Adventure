namespace MyECS;
using System;
using MoonTools.ECS;
using MyECS.Components;

public class TimerSystem : System
{
    public Filter EntityFilter;

    public TimerSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Timer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Timer timer = Get<Timer>(entity);
            Set(entity, timer.Update(timer.Time - (float)delta.TotalSeconds));
        }
    }
}