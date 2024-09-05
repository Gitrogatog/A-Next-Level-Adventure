using Godot;
using MyECS.Components;
using System;
using System.Collections.Generic;

public partial class NPCDictionaryResource : Resource
{
    [Export] public NPCInfoResource[] npcInfo;
    public NPCData Load()
    {

        return new NPCData(GetNames(), GetNameToIDDictionary(), GetDisplayNames(), GetKillNames(), GetPowers(), GetDisplayPowers(), GetColors(), GetHurtColors(), GetDialogs(), GetTextures());
    }
    List<string> GetNames()
    {
        List<string> names = new List<string>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            names.Add(npcInfo[i].name);
        }
        return names;
    }
    List<string> GetDisplayNames()
    {
        List<string> names = new List<string>(npcInfo.Length);
        // GD.Print($"display names were created with length: {npcInfo.Length}, resulting in count {names.Count}");
        for (int i = 0; i < npcInfo.Length; i++)
        {
            names.Add(npcInfo[i].displayName);
        }
        return names;
    }
    List<string> GetKillNames()
    {
        List<string> names = new List<string>(npcInfo.Length);
        // GD.Print($"display names were created with length: {npcInfo.Length}, resulting in count {names.Count}");
        for (int i = 0; i < npcInfo.Length; i++)
        {
            if (npcInfo[i].killName.Length > 0)
            {
                names.Add(npcInfo[i].killName);
            }
            else
            {
                names.Add(npcInfo[i].displayName);
            }

        }
        return names;
    }
    Dictionary<string, int> GetNameToIDDictionary()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        for (int i = 0; i < npcInfo.Length; i++)
        {
            dict.Add(npcInfo[i].name, i);
        }
        return dict;
    }
    List<Texture2D> GetTextures()
    {
        List<Texture2D> textures = new List<Texture2D>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            textures.Add(npcInfo[i].texture);
        }
        return textures;
    }
    List<DialogStorage> GetDialogs()
    {
        List<DialogStorage> dialogs = new List<DialogStorage>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            dialogs.Add(npcInfo[i].dialog.LoadDialog());
        }
        return dialogs;
    }
    List<int> GetPowers()
    {
        List<int> powers = new List<int>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            powers.Add(npcInfo[i].power);
        }
        return powers;
    }
    List<bool> GetDisplayPowers()
    {
        List<bool> displayPowers = new List<bool>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            displayPowers.Add(npcInfo[i].displayPower);
        }
        return displayPowers;
    }
    List<Color> GetColors()
    {
        List<Color> colors = new List<Color>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            colors.Add(npcInfo[i].color);
        }
        return colors;
    }
    List<Color> GetHurtColors()
    {
        List<Color> colors = new List<Color>(npcInfo.Length);
        for (int i = 0; i < npcInfo.Length; i++)
        {
            colors.Add(npcInfo[i].hurtColor);
        }
        return colors;
    }
}

public class NPCData
{
    public List<string> IDtoName;
    public Dictionary<string, int> NameToID;
    public List<string> DisplayNames;
    public List<string> KillNames;
    public List<int> Powers;
    public List<bool> DisplayPowers;
    public List<DialogStorage> Dialogs;
    public List<Texture2D> Textures;
    public List<Color> Colors;
    public List<Color> HurtColors;
    public NPCData(List<string> names, Dictionary<string, int> nametoid, List<string> displayNames, List<string> killNames, List<int> powers, List<bool> displayPowers, List<Color> colors, List<Color> hurtColors, List<DialogStorage> dialogs, List<Texture2D> textures)
    {
        // GD.Print("loading npc data!");
        IDtoName = names;
        NameToID = nametoid;
        DisplayNames = displayNames;
        KillNames = killNames;
        Powers = powers;
        DisplayPowers = displayPowers;
        Colors = colors;
        HurtColors = hurtColors;
        Dialogs = dialogs;
        Textures = textures;
        // GD.Print($"display name length: {displayNames.Count}, textures length: {Textures.Count}");
    }
}