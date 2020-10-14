using System.Drawing;
using System.Numerics;

namespace Project
{
    public static class RectangleExtensions
    {
        public static bool Contains(this Rectangle r, Vector2 position, Vector2 edge)
            => position.X >= r.Left + edge.X && position.X <= r.Right - edge.X
            && position.Y >= r.Top + edge.Y && position.Y <= r.Bottom - edge.Y;
    }

    public static class Vector2Extensions
    {
        public static Vector2 Normalize(this Vector2 v)
            => Vector2.Normalize(v);
    }
}
