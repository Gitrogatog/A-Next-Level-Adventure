namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class BonkWallDialogSystem : System
{
    public Filter EntityFilter;
    RichTextLabel dialogBox;
    RichTextLabel nameBox;

    public BonkWallDialogSystem(World world, RichTextLabel dialogBox, RichTextLabel nameBox) : base(world)
    {
        this.dialogBox = dialogBox;
        this.nameBox = nameBox;
        EntityFilter = FilterBuilder
            .Include<BonkedWall>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            string wallName = TextStorage.GetString(Get<BonkedWall>(entity).WallID);
            dialogBox.Text = $"It's a {wallName}.";
            if (!string.IsNullOrEmpty(wallName))
            {
                nameBox.Text = UppercaseStorage.GetUpper(wallName);
            }

        }
    }
}