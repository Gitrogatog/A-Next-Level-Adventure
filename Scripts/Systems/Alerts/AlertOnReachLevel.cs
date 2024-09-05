namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Prefabs;

public class AlertOnReachLevelSystem : System
{
    public Filter EntityFilter;
    bool hasAlerted;
    int level;
    string alertText;
    public AlertOnReachLevelSystem(World world, int level, string alertText) : base(world)
    {
        this.level = level;
        this.alertText = alertText;
        EntityFilter = FilterBuilder
            .Include<Level>()
            .Include<ControlledByPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (hasAlerted) return;
        foreach (var entity in EntityFilter.Entities)
        {
            if (Get<Level>(entity).Value >= level)
            {
                hasAlerted = true;
                var message = MessagePrefabs.Alert(World, alertText);
                Set(message, new Components.Timer(12f));
                Set(message, new DestroyOnTimerEnd());
            }
        }
    }
}