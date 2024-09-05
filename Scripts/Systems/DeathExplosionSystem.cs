namespace MyECS;
using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DeathExplosionSystem : System
{
    public Filter DeadFilter;
    public Filter WallFilter;
    Control gameDisplayOrigin;
    Node parent;
    int tileSize;
    int tileScale;
    PackedScene explosion;
    AudioStreamPlayer[] audioPlayers;
    int lastExplosionSFX = -1;
    public DeathExplosionSystem(World world, Control gameDisplayOrigin, Node parent, int tileSize, int tileScale, PackedScene explosion, AudioStreamPlayer[] audioPlayers) : base(world)
    {
        this.gameDisplayOrigin = gameDisplayOrigin;
        this.parent = parent;
        this.tileSize = tileSize;
        this.tileScale = tileScale;
        this.explosion = explosion;
        this.audioPlayers = audioPlayers;
        DeadFilter = FilterBuilder
            .Include<Position>()
            .Include<Killed>()
            .Build();
        WallFilter = FilterBuilder.Include<DestroyedWall>().Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in DeadFilter.Entities)
        {
            // GD.Print("Running explosion!");
            Vector2I tilePos = Get<Position>(entity).Value;
            AddExplosion(tilePos);
            // AddExplosion(displayPos);
        }
        foreach (var message in WallFilter.Entities)
        {
            Vector2I tilePos = Get<DestroyedWall>(message).Position;
            AddExplosion(tilePos);
        }
    }
    Vector2 TileToDisplayPos(Vector2I position)
    {
        Vector2I screenPos = position - ScreenTileInfo.currentScreenTileOrigin;
        // GD.Print(screenPos);
        Vector2 displayPos = screenPos * (tileSize * tileScale);
        // GD.Print(displayPos);
        return displayPos + gameDisplayOrigin.GlobalPosition;
    }
    void AddExplosion(Vector2I tilePosition)
    {
        Vector2 displayPos = TileToDisplayPos(tilePosition);
        Node2D node = explosion.Instantiate<Node2D>();
        parent.AddChild(node);
        node.GlobalPosition = displayPos;
        PlaySFX();
        // PlayIncreasing();
    }
    void PlaySFX()
    {

        int rando = Math.Abs((int)GD.Randi()) % audioPlayers.Length;
        if (lastExplosionSFX == rando)
        {
            if (lastExplosionSFX == 0)
            {
                rando = 1;
            }
            else
            {
                rando = 0;
            }
        }
        // GD.Print($"max: {audioPlayers.Length}, rand: {rando}");
        audioPlayers[rando].Play();
        lastExplosionSFX = rando;
    }
    void PlayIncreasing()
    {
        lastExplosionSFX++;
        if (lastExplosionSFX >= audioPlayers.Length)
        {
            lastExplosionSFX = 0;
        }
        audioPlayers[lastExplosionSFX].Play();
    }
}