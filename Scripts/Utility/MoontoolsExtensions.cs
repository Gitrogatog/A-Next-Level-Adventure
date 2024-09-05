namespace MyECS;
using System;
using MoonTools.ECS;

public static class MoontoolsECSExtensions {
    public static bool TryGetComponent<T>(this World world, Entity entity, out T component) where T : unmanaged {
        if (world.Has<T>(entity)) {
            component = world.Get<T>(entity);
            return true;
        }
        component = default;
        return false;
    }
}
