namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ShowLockSystem : System
{
    public Filter InteractedFilter;
    public Filter KilledFilter;
    public Filter LockFilter;
    Control lockHub;
    RichTextLabel lockBox;
    public ShowLockSystem(World world, Control lockHub, RichTextLabel lockBox) : base(world)
    {
        this.lockHub = lockHub;
        this.lockBox = lockBox;
        InteractedFilter = FilterBuilder
            .Include<IsLock>()
            .Include<InteractedThisFrame>()
            .Build();
        KilledFilter = FilterBuilder.Include<IsLock>().Include<Killed>().Build();
        LockFilter = FilterBuilder.Include<IsLock>().Exclude<Killed>().Build();
        lockBox.Text = LockFilter.Count.ToString();
    }
    public override void Update(TimeSpan delta)
    {
        if (InteractedFilter.Count > 0)
        {
            lockHub.Visible = true;
        }
        if (KilledFilter.Count > 0 || InteractedFilter.Count > 0)
        {
            lockBox.Text = LockFilter.Count.ToString();
        }

    }
}