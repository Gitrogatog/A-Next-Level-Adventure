namespace MyECS;
using System;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ShowExtraDialogIndicatorSystem : System
{
    public Filter EntityFilter;
    RichTextLabel textbox;
    List<DialogStorage> dialogStorage;
    CanvasItem indicator;
    AnimatedSprite2D animatedSprite;
    public ShowExtraDialogIndicatorSystem(World world, List<DialogStorage> dialogStorage, CanvasItem indicator, AnimatedSprite2D animatedSprite) : base(world)
    {
        this.textbox = textbox;
        this.dialogStorage = dialogStorage;
        this.indicator = indicator;
        this.animatedSprite = animatedSprite;
        EntityFilter = FilterBuilder
            .Include<ShowDialog>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        indicator.Visible = false;
        foreach (var entity in EntityFilter.Entities)
        {
            var dialog = Get<ShowDialog>(entity);

            // GD.Print($"reading npc id: {dialog.npcID} dialog id: {dialog.dialogID}");
            bool hasExtraDialog = dialogStorage[dialog.npcID].HasMoreDialog(dialog.dialogID, dialog.lineID);
            if (hasExtraDialog)
            {
                EnableIndicator();
            }
        }
    }
    void EnableIndicator()
    {
        indicator.Visible = true;
        animatedSprite.Frame = 0;
    }
}