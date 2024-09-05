namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class FinishLoadNPCSystem : System
{
    public Filter EntityFilter;
    NPCData npcData;
    public static string DragonName = "dragon";
    public static string LockName = "lock";

    public FinishLoadNPCSystem(World world, NPCData npcData) : base(world)
    {
        this.npcData = npcData;
        // textures = npcData.Textures;
        // powers = npcData.Powers;
        EntityFilter = FilterBuilder
            .Include<NPC>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            int id = Get<NPC>(entity).ID;
            // GD.Print($"NPC id: {id}");
            // GD.Print(npcData.Textures.Count);
            // GD.Print($"NPC name: {npcData.IDtoName[id]}");
            Texture2D sprite = npcData.Textures[id];
            int spriteID = SpriteStorage.GetID(sprite);
            Set(entity, new Sprite(spriteID));
            string name = npcData.DisplayNames[id];
            int displayNameID = TextStorage.GetID(name);
            Set(entity, new DisplayName(displayNameID));
            string killName = npcData.KillNames[id];
            int killNameID = TextStorage.GetID(killName);
            Set(entity, new KillName(killNameID));

            int power = npcData.Powers[id];
            if (power >= 0)
            {
                Set(entity, new Level(npcData.Powers[id]));
                if (npcData.DisplayPowers[id])
                {
                    Set(entity, new DisplayLevel());
                }
            }

            Set(entity, new SpriteColor(npcData.Colors[id]));
            // Set(entity, new HurtColor(npcData.HurtColors[id]));
            Set(entity, new BackColor(new Color(32 / 255f, 18 / 255f, 8 / 255f)));
            if (npcData.IDtoName[id] == DragonName)
            {
                Set(entity, new EnableWallBustingOnDeath());
            }
            else if (npcData.IDtoName[id] == LockName)
            {
                Set(entity, new IsLock());
            }
        }
    }
}