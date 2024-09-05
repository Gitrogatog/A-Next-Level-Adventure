namespace MyECS;
using System;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;

public class SystemGroup {
    private List<System> _systems = new List<System>();
    public SystemGroup Add(System system) {
        _systems.Add(system);
        return this;
    }
    public SystemGroup AddUnique(System system) {
        if (!_systems.Contains(system)) {
            _systems.Add(system);
        }
        return this;
    }
    public bool Remove(System system) => _systems.Remove(system);
    public void RemoveAll() => _systems.Clear();
    public void Run(TimeSpan timeSpan) {
        foreach (System system in _systems) {
            system.Update(timeSpan);
        }
    }
}
