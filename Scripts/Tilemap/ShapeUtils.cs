using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Utils
{
    public struct Rectangle
    {
        public int x1, x2, y1, y2;
        public Rectangle(int x, int y, int w, int h)
        {
            x1 = x;
            y1 = y;
            x2 = x + w;
            y2 = y + h;
        }
        public bool Intersect(Rectangle other)
        {
            return x1 <= other.x2 && x2 >= other.x1 && y1 <= other.y2 && y2 >= other.y1;
        }
        public Vector2I Center => new Vector2I((x1 + x2) / 2, (y1 + y2) / 2);
    }
}