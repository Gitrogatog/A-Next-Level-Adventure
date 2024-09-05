namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class NPCNameUISystem : System
{
    public Filter EntityFilter;
    public RichTextLabel nameText;

    public NPCNameUISystem(World world, RichTextLabel text) : base(world)
    {
        nameText = text;
        EntityFilter = FilterBuilder
            .Include<InteractedThisFrame>()
            .Include<DisplayName>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            int nameID = Get<DisplayName>(entity).ID;
            nameText.Text = TextStorage.GetString(nameID);
        }
    }
}