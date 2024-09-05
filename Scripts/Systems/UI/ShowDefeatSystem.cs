namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ShowDefeatUISystem : System
{
    public Filter EntityFilter;
    RichTextLabel _dialogBox;
    RichTextLabel _nameBox;

    public ShowDefeatUISystem(World world, RichTextLabel dialogBox, RichTextLabel nameBox) : base(world)
    {
        _dialogBox = dialogBox;
        _nameBox = nameBox;
        EntityFilter = FilterBuilder
            .Include<ShowDefeat>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            int nameID = Get<ShowDefeat>(entity).NameID;
            string name = TextStorage.GetString(nameID);
            _dialogBox.Text = $"Defeated {name}!";
            _nameBox.Text = name;
        }
    }
}