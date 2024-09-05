namespace MyECS;
using Godot;
using MoonTools.ECS;
using System;
using System.Collections.Generic;

public readonly record struct MatchClass<T>(int ID);
public readonly record struct MatchNode<T>(int ID) where T : Node, new();

public static class ClassStorage<T>
{
    static Dictionary<T, int> ClassToID = new Dictionary<T, int>();
    static T[] IDToClass = new T[256];
    static Stack<int> OpenIDs = new Stack<int>();
    static int NextID = 0;

    public static T GetClass(int id)
    {
        return IDToClass[id];
    }

    public static int GetID(T ogClass)
    {
        if (!ClassToID.ContainsKey(ogClass))
        {
            RegisterClass(ogClass);
        }

        return ClassToID[ogClass];
    }

    private static void RegisterClass(T ogClass)
    {
        if (OpenIDs.Count == 0)
        {
            if (NextID >= IDToClass.Length)
            {
                Array.Resize(ref IDToClass, IDToClass.Length * 2);
            }
            ClassToID[ogClass] = NextID;
            IDToClass[NextID] = ogClass;
            NextID += 1;
        }
        else
        {
            ClassToID[ogClass] = OpenIDs.Pop();
        }
    }
    public static MatchClass<T> CreateComponent(T ogClass)
    {
        int id = GetID(ogClass);
        return new MatchClass<T>(id);
    }
}

public static class NodeStorage<T> where T : Node, new()
{
    static Dictionary<T, int> ClassToID = new Dictionary<T, int>();
    static T[] IDToClass = new T[256];
    static Stack<int> OpenIDs = new Stack<int>();
    static int NextID = 0;

    public static T GetClass(int id)
    {
        return IDToClass[id];
    }

    public static int GetID(T ogClass)
    {
        if (!ClassToID.ContainsKey(ogClass))
        {
            RegisterClass(ogClass);
        }

        return ClassToID[ogClass];
    }

    private static void RegisterClass(T ogClass)
    {
        if (OpenIDs.Count == 0)
        {
            if (NextID >= IDToClass.Length)
            {
                Array.Resize(ref IDToClass, IDToClass.Length * 2);
            }
            ClassToID[ogClass] = NextID;
            IDToClass[NextID] = ogClass;
            NextID += 1;
        }
        else
        {
            ClassToID[ogClass] = OpenIDs.Pop();
        }
    }
    public static MatchNode<T> CreateComponent(T ogClass, Entity entity)
    {
        ogClass.SetMeta("Entity", entity.ID);
        int id = GetID(ogClass);
        return new MatchNode<T>(id);
    }
}

public static class RootStorage
{
    static Dictionary<Node, int> ClassToID = new Dictionary<Node, int>();
    static Node[] IDToClass = new Node[256];
    static Stack<int> OpenIDs = new Stack<int>();
    static int NextID = 0;

    public static Node GetClass(int id)
    {
        return IDToClass[id];
    }

    public static int GetID(Node ogClass)
    {
        if (!ClassToID.ContainsKey(ogClass))
        {
            RegisterClass(ogClass);
        }

        return ClassToID[ogClass];
    }

    private static void RegisterClass(Node ogClass)
    {
        if (OpenIDs.Count == 0)
        {
            if (NextID >= IDToClass.Length)
            {
                Array.Resize(ref IDToClass, IDToClass.Length * 2);
            }
            ClassToID[ogClass] = NextID;
            IDToClass[NextID] = ogClass;
            NextID += 1;
        }
        else
        {
            ClassToID[ogClass] = OpenIDs.Pop();
        }
    }
    public static void SetEntityRoot(Entity entity, World world, Node ogClass)
    {
        world.Set(entity, CreateComponent(ogClass));
        ogClass.SetMeta("Entity", entity.ID);
    }
    public static Root CreateComponent(Node ogClass)
    {
        int id = GetID(ogClass);
        return new Root(id);
    }
}

// We can't store strings in ECS because they are managed types!
public static class TextStorage
{
    static Dictionary<string, int> StringToID = new Dictionary<string, int>();
    static string[] IDToString = new string[256];
    static Stack<int> OpenIDs = new Stack<int>();
    static int NextID = 0;

    // TODO: is there some way we can reliably clear strings to free memory?

    public static string GetString(int id)
    {
        return IDToString[id];
    }

    public static int GetID(string text)
    {
        if (!StringToID.ContainsKey(text))
        {
            RegisterString(text);
        }

        return StringToID[text];
    }

    private static void RegisterString(string text)
    {
        if (OpenIDs.Count == 0)
        {
            if (NextID >= IDToString.Length)
            {
                Array.Resize(ref IDToString, IDToString.Length * 2);
            }
            StringToID[text] = NextID;
            IDToString[NextID] = text;
            NextID += 1;
        }
        else
        {
            StringToID[text] = OpenIDs.Pop();
        }
    }
}

public static class UppercaseStorage
{
    static Dictionary<string, string> lowerToUpper = new Dictionary<string, string>();
    public static string GetUpper(string lower)
    {
        if (lower.Length == 0) return "";
        if (lower.Length == 1) return char.ToUpper(lower[0]).ToString();
        if (!lowerToUpper.ContainsKey(lower))
        {
            RegisterLower(lower);
        }
        return lowerToUpper[lower];
    }
    static void RegisterLower(string lower)
    {
        char first = char.ToUpper(lower[0]);
        lowerToUpper[lower] = first + lower.Substring(1);
    }
}

public static class SpriteStorage
{
    static Dictionary<Texture2D, int> TextureToID = new Dictionary<Texture2D, int>();
    static Texture2D[] IDToTexture = new Texture2D[256];
    static Stack<int> OpenIDs = new Stack<int>();
    static int NextID = 0;

    // TODO: is there some way we can reliably clear strings to free memory?

    public static Texture2D GetTexture(int id)
    {
        return IDToTexture[id];
    }

    public static int GetID(Texture2D tex)
    {
        if (!TextureToID.ContainsKey(tex))
        {
            RegisterTexture(tex);
        }

        return TextureToID[tex];
    }

    private static void RegisterTexture(Texture2D tex)
    {
        if (OpenIDs.Count == 0)
        {
            if (NextID >= IDToTexture.Length)
            {
                Array.Resize(ref IDToTexture, IDToTexture.Length * 2);
            }
            TextureToID[tex] = NextID;
            IDToTexture[NextID] = tex;
            NextID += 1;
        }
        else
        {
            TextureToID[tex] = OpenIDs.Pop();
        }
    }
}