using Godot;
using System;
using TextureUtils;
public partial class EntityTextureResource : Resource
{
    [Export] public Texture2D playerTexture;

    public void LoadTextures()
    {
        LoadedTextures.playerTexture = playerTexture;
    }
}
