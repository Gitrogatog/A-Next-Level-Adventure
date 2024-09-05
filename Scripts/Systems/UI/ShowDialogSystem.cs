namespace MyECS;
using System;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ShowDialogSystem : System
{
    public Filter EntityFilter;
    RichTextLabel textbox;
    List<DialogStorage> dialogStorage;
    public ShowDialogSystem(World world, RichTextLabel textbox, List<DialogStorage> dialogStorage) : base(world)
    {
        this.textbox = textbox;
        this.dialogStorage = dialogStorage;
        EntityFilter = FilterBuilder
            .Include<ShowDialog>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var dialog = Get<ShowDialog>(entity);

            // GD.Print($"reading npc id: {dialog.npcID} dialog id: {dialog.dialogID}");
            textbox.Text = dialogStorage[dialog.npcID].GetDialog(dialog.dialogID, dialog.lineID);
        }
    }
}