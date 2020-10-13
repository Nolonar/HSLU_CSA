using System;

namespace Project
{
    public struct Vector2
    {
        public float X { get; }
        public float Y { get; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Rotate(double rad)
        {
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
            return new Vector2(X * cos - Y * sin, X * sin + Y * cos);
        }

        public Vector2 Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y);
            return length == 0 ? new Vector2(0, 0) : new Vector2(X / length, Y / length);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b)
            => new Vector2(a.X - b.X, a.Y - b.Y);

        public static Vector2 operator *(Vector2 a, float d)
            => new Vector2(a.X * d, a.Y * d);

        public static Vector2 operator /(Vector2 a, float d)
            => new Vector2(a.X / d, a.Y / d);
    }
}
