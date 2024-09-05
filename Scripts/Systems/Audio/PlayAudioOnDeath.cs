namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class PlayAudioOnDeathSystem : System
{
    public Filter EntityFilter;
    AudioStreamPlayer audio;
    float delay;
    bool waitingToPlay;
    float playSoundTime;
    public PlayAudioOnDeathSystem(World world, AudioStreamPlayer audio, float delay) : base(world)
    {
        this.audio = audio;
        this.delay = delay;
        EntityFilter = FilterBuilder
            .Include<Killed>()
            .Include<EnableWallBustingOnDeath>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (EntityFilter.Count > 0)
        {
            waitingToPlay = true;
            playSoundTime = delay;
        }
        if (waitingToPlay)
        {
            playSoundTime -= (float)delta.TotalSeconds;
            if (playSoundTime <= 0)
            {
                audio.Play();
                waitingToPlay = false;
            }
        }

    }
}