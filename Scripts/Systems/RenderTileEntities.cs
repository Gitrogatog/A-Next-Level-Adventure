namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class RenderTileEntitiesSystem : System
{
    public Filter CharacterFilter;
    CanvasItem canvas;
    Vector2I texSize;
    Vector2I tileOffset;
    public RenderTileEntitiesSystem(World world, CanvasItem canvasItem, Vector2I size, Vector2I offset) : base(world)
    {
        canvas = canvasItem;
        texSize = size;
        tileOffset = offset;
        // canvasNode.DrawTextureRect(playerTexture, new Rect2(100f, 100f, 150f, 150), false);
        CharacterFilter = FilterBuilder
            .Include<Position>()
            .Include<Sprite>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in CharacterFilter.Entities)
        {
            // GD.Print("rendering character!");
            Vector2I position = (Get<Position>(entity).Value + tileOffset) * texSize;
            Rect2 drawRect = new Rect2(position, texSize);
            if (World.TryGetComponent(entity, out BackColor backColor))
            {
                canvas.DrawRect(drawRect, backColor.Color, true);
            }
            int textureID = Get<Sprite>(entity).ID;
            Texture2D texture = SpriteStorage.GetTexture(textureID);
            if (World.TryGetComponent(entity, out SpriteColor color))
            {
                canvas.DrawTextureRect(texture, drawRect, false, color.Value);
            }
            else
            {
                canvas.DrawTextureRect(texture, drawRect, false);
            }
        }

    }
    void RenderCharacter()
    {
        // GD.Print($"drawing player at: {Hacky.playerPos}");
        // canvas.DrawTextureRect(texture, new Rect2(Hacky.playerPos, texSize), false);
    }
}