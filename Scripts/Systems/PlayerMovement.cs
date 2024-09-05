namespace MyECS;
using System;
using System.Security.Cryptography.X509Certificates;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class PlayerMovement : System
{
    public Filter PlayerFilter;
    float delayBetweenMoves = 0.1f;
    float initialDelay = 0.3f;
    float fastModeDelay = 0.01f;
    // float fastModeInitialDelay = 0.1f;
    float blockedDelay = 0.5f;
    // float hitEntityDelay = 1f;
    float currentDelay;
    TileDirection prevInputDir = TileDirection.None;
    TileDirection lastFrameDir;
    bool preventMovementUntilDiffDirection = false;
    public PlayerMovement(World world) : base(world)
    {
        PlayerFilter = FilterBuilder.Include<ControlledByPlayer>().Include<Position>().Build();
    }
    public override void Update(TimeSpan delta)
    {
        float horizontalMove = Input.GetAxis("move_left", "move_right");
        float verticalMove = Input.GetAxis("move_up", "move_down");
        TileDirection dir = GetMoveDir(horizontalMove, verticalMove);
        // GD.Print($"Preventing: {preventMovementUntilDiffDirection}");
        if (dir == TileDirection.None || dir != lastFrameDir)
        {
            currentDelay = 0;
        }
        if (preventMovementUntilDiffDirection && (prevInputDir != dir || dir == TileDirection.None || prevInputDir == TileDirection.None))
        {
            preventMovementUntilDiffDirection = false;
        }
        if (currentDelay > 0)
        {
            // GD.Print("delay is too high");
            currentDelay -= (float)delta.TotalSeconds;
        }
        else if (!preventMovementUntilDiffDirection)
        {
            // GD.Print("running setplayerdir!");
            SetPlayerDir(dir);
            prevInputDir = dir;
        }
        else
        {

            // GD.Print($"lastdir: {lockedDir}, new dir: {dir}");
        }
        // GD.Print(prevInputDir);
        lastFrameDir = dir;
    }

    void SetPlayerDir(TileDirection dir)
    {

        foreach (Entity entity in PlayerFilter.Entities)
        {
            // Vector2I position = Get<Position>(entity).Value;

            if (Has<BlockedMove>(entity) && prevInputDir == dir)
            {
                // GD.Print("blocked move");
                // preventMovementUntilDiffDirection = true;
                currentDelay = blockedDelay;
                Remove<BlockedMove>(entity);
                return;
            }
            else if (dir == TileDirection.None)
            {

                return;
            }
            else if (GlobalFlags.FastMode)
            {
                currentDelay = fastModeDelay;
            }
            else if (prevInputDir == dir)
            {
                currentDelay = delayBetweenMoves;
            }
            else
            {
                currentDelay = initialDelay;
            }

            Set(entity, new MoveTile(dir));

        }
    }
    TileDirection GetMoveDir(float horizontalMove, float verticalMove)
    {
        if (horizontalMove < 0)
        {
            // position.X -= 1;
            return TileDirection.West;
        }
        else if (horizontalMove > 0)
        {
            return TileDirection.East;
        }
        else if (verticalMove > 0)
        {
            return TileDirection.South;
        }
        else if (verticalMove < 0)
        {
            return TileDirection.North;
        }
        return TileDirection.None;
    }
}
