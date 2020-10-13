using System;
using System.Drawing;

namespace Project
{
    class Ball : RenderObject
    {
        private readonly int radius;

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public bool IsMoving { get; private set; }

        private const float defaultSpeed = 20 / Unit.Second;
        private const float speedMultiplier = 1.2f;
        private const float maxSpeed = 10 * defaultSpeed;
        private float speed;

        public Ball(int radius)
        {
            this.radius = radius;
            Reset();
        }

        public bool IsOut => PongGame.ScreenDimension.Contains(Position, new Vector2(radius, radius));

        public void Reset()
        {
            Direction = new Vector2(0, 0);
            Position = new Vector2(0, 0);
            speed = defaultSpeed;
        }

        /// <summary>
        /// Bounce from the screen edge if collision happened.
        /// </summary>
        /// <returns>True if collision happened, false otherwise.</returns>
        public bool BounceFromEdge()
        {
            int edgeTop = PongGame.ScreenDimension.Top + radius;
            int edgeBottom = PongGame.ScreenDimension.Bottom - radius;
            if (Position.Y >= edgeTop && Position.Y <= edgeBottom)
                return false;

            float newY = Math.Max(edgeTop, Math.Min(edgeBottom, Position.Y));
            Position = new Vector2(Position.X, newY);
            Direction = new Vector2(Direction.X, -Direction.Y);
            return true;
        }

        public void Bounce(Player p)
        {
            Direction = new Vector2(-Direction.X, Direction.Y);
            speed = Math.Min(speed * speedMultiplier, maxSpeed);
        }

        public void UpdatePosition(long delta)
        {

        }

        public void Draw(Graphics g)
        {
            Vector2 renderPos = Position - new Vector2(radius, radius);
            g.FillEllipse(Brushes.White, renderPos.X, renderPos.Y, radius, radius);
        }
    }
}
