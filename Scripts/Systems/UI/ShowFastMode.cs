namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Prefabs;

public class ShowFastModeSystem : System
{
    public Filter EntityFilter;
    int levelToEnable;
    Control fastMode;
    BaseButton fastButton;
    bool hasEnabledFastMode = false;
    public ShowFastModeSystem(World world, int level, Control fastMode, BaseButton fastButton) : base(world)
    {

        levelToEnable = level;
        this.fastButton = fastButton;
        this.fastMode = fastMode;
        fastButton.Disabled = true;
        fastMode.Visible = false;
        EntityFilter = FilterBuilder
            .Include<Level>()
            .Include<ControlledByPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (!hasEnabledFastMode && Get<Level>(entity).Value >= levelToEnable)
            {
                hasEnabledFastMode = true;
                fastButton.Disabled = false;
                fastMode.Visible = true;
            }
        }
    }
}