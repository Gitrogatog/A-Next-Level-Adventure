namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class NPCLevelUISystem : System
{
    public Filter EntityFilter;
    RichTextLabel levelText;
    public NPCLevelUISystem(World world, RichTextLabel text) : base(world)
    {
        levelText = text;
        EntityFilter = FilterBuilder
            .Include<InteractedThisFrame>()
            .Include<DisplayLevel>()
            .Include<Level>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            int level = Get<Level>(entity).Value;
            if (level < 10)
            {
                levelText.Text = $"LV 0{level}";
            }
            else if (level < 100)
            {
                levelText.Text = $"LV {level}";
            }
            else
            {
                // levelText.Text = $"LV{level}";
                levelText.Text = $"LV {level}";
            }
        }
    }
}