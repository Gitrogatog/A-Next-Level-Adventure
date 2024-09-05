using System.Linq;
using Godot;
using MoonTools.ECS;
using MyECS.Relations;

namespace MyECS {
    public record struct Root(int ID);

    public interface ISpawnable {
        void Spawn(Entity entity, World world);
    }

    // public class Marshallable<T> : RefCounted where T : class {
    //     public T Value;
    //     public Marshallable(T value) {
    //         Value = value;
    //     }
    // }

    public static class GodotExtensions {
        // public static EntityBuilder Spawn(this World world, Node root) {
        //     return world.Spawn().Attach(root);
        // }

        public static void DespawnAndFree(this World world, Entity entity) {

            if (world.TryGetComponent(entity, out Root root)) {
                RootStorage.GetClass(root.ID).QueueFree();
            }

            world.Destroy(entity);
        }

        public static void AttachNode(this World world, Entity entity, Node root) {
            RootStorage.SetEntityRoot(entity, world, root);
            // world.Set(entity, ClassStorage<Node>.CreateComponent(root));
            // root.SetMeta("Entity", entity.ID);

            // world.AddComponent(entity, new Root { Node = root });
            // root.SetMeta("Entity", new Marshallable<Entity>(entity));

            // world.Get<HasCollider>(new Entity(entity.ID));

            var nodes = root.GetChildren().Cast<Node>().Prepend(root).ToList();

            foreach (var node in nodes) {
                var addMethod = typeof(GodotExtensions).GetMethod(nameof(AddNodeComponent));
                var addChildMethod = addMethod?.MakeGenericMethod(node.GetType());
                addChildMethod?.Invoke(null, new object[] { world, entity, node });
            }

            if (root is ISpawnable spawnable)
                spawnable.Spawn(entity, world);
        }

        public static void AddNodeComponent<T>(World world, Entity entity, T node) where T : Node, new() {
            // world.AddComponent(entity, node);
            // world.Set(entity, RootStorage.CreateComponent(node));
            RootStorage.SetEntityRoot(entity, world, node);
        }

        // public static EntityBuilder Attach(this EntityBuilder entityBuilder, Node node) {
        //     entityBuilder.World.AttachNode(entityBuilder.Id(), node);
        //     return entityBuilder;
        // }
        public static void AddAllNodes(World world, Node root) {
            if (root.HasMeta("spawnState")) {
                string spawnState = (string)root.GetMeta("spawnState");
                if (spawnState == "stop") {
                    return;
                }
                else if (spawnState == "attach") {
                    var entity = world.CreateEntity();
                    world.AttachNode(entity, root);
                }
                else if (spawnState == "spawn" && root is ISpawnable spawnable) {
                    // var entity = world.CreateEntity();
                    // world.Set(entity, RootStorage.CreateComponent(root));
                    // spawnable.Spawn(entity, world);
                    var entity = world.CreateEntity();
                    SpawnNode(entity, world, root, spawnable);
                }
            }

            else {
                // if blank, do recursive call
                var nodes = root.GetChildren();
                foreach (Node node in nodes) {
                    AddAllNodes(world, node);
                }
            }
        }
        public static void SpawnNode(Entity entity, World world, Node root, ISpawnable spawnable) {

            spawnable.Spawn(entity, world);
            if (root.HasMeta("recursive") && (bool)root.GetMeta("recursive")) {
                var nodes = root.GetChildren();
                foreach (Node node in nodes) {
                    if (node is ISpawnable childSpawn) {
                        SpawnNode(entity, world, node, childSpawn);
                    }

                }

            }
        }
    }
}
