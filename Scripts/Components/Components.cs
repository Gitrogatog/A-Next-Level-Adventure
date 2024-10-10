namespace MyECS.Components;
using System;
using System.Runtime.InteropServices;
using Godot;
using MoonTools.ECS;

// enums
public enum TileDirection
{
    North, South, East, West, None
}

// physics/movement components
public readonly record struct ControlledByPlayer();
public readonly record struct CanMove(float Value);
public readonly record struct Position(Vector2I Value);
public readonly record struct PrevPosition(Vector2I Value);
public readonly record struct IntendedPosition(Vector2I Value);
public readonly record struct MoveTile(TileDirection Direction);
public readonly record struct BackColor(Color Color);

public readonly record struct BlocksTile();

// class storage components
public readonly record struct Sprite(int ID);


// npc stuff
// public readonly record struct Interactible();
public readonly record struct DisplayName(int ID);
public readonly record struct KillName(int ID);
public readonly record struct NPC(int ID);
public readonly record struct HasDialog(int ID);
public readonly record struct Level(int Value);
public readonly record struct DisplayLevel();
public readonly record struct Killed();
public readonly record struct InteractedThisFrame();
public readonly record struct BlockedMove();
public readonly record struct EnableWallBustingOnDeath();
public readonly record struct IsLock();
// public readonly record struct NextDialogLine(int Line);

public readonly record struct SpriteColor(Color Value);
public readonly record struct HurtColor(Color Value);
public readonly record struct FlashColor(Color Value);
public readonly record struct FlashTime(float Value);

// messaging components
public readonly record struct ShowDialog(int npcID, int dialogID, int lineID); // pull dialog from custom dialog resource
public readonly record struct ShowText(int ID); // pull dialog from TextStorage
// public readonly record struct ShowLevel(int Level);
// public readonly record struct ShowName(int NameID);
public readonly record struct ShowDefeat(int NameID);
public readonly record struct DestroyedWall(Vector2I Position);
public readonly record struct BonkedWall(int WallID);
public readonly record struct PlaySound(int soundID, int priority);
public readonly record struct AlertText(int ID);

// deleting stuff components
public readonly record struct MarkedForDestroy();
public readonly record struct DestroyOnTimerEnd();
public readonly record struct DestroyOnDestroyedWall();

// public readonly record struct TimerFinished();
public struct Timer
{
    public float Time { get; private set; }
    public float Max { get; }
    public float Remaining => Time / Max;

    public Timer(float time)
    {
        Time = Max = time;
    }

    public Timer Update(float newTime)
    {
        Time = newTime;
        return this;
    }
}