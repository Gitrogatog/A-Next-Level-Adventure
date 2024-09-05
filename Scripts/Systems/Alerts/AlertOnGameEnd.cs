namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Prefabs;

public class AlertOnGameEndSystem : System
{
    public Filter EntityFilter;
    string alertMessage;
    public AlertOnGameEndSystem(World world, string alertMessage) : base(world)
    {
        this.alertMessage = alertMessage;
        EntityFilter = FilterBuilder
            .Include<EnableWallBustingOnDeath>()
            .Include<Killed>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (EntityFilter.Count > 0)
        {
            var message = MessagePrefabs.Alert(World, alertMessage);
            Set(message, new DestroyOnDestroyedWall());
        }
    }
}