namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ShowAlertSystem : System
{
    public Filter EntityFilter;
    Control alertHub;
    RichTextLabel alertBox;
    public ShowAlertSystem(World world, Control alertHub, RichTextLabel alertBox) : base(world)
    {
        this.alertHub = alertHub;
        this.alertBox = alertBox;
        EntityFilter = FilterBuilder
            .Include<AlertText>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (EntityFilter.Count == 0)
        {
            alertHub.Visible = false;
        }
        else
        {
            alertHub.Visible = true;
        }
        foreach (var entity in EntityFilter.Entities)
        {
            alertBox.Text = TextStorage.GetString(Get<AlertText>(entity).ID);
        }
    }
}