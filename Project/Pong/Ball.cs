using System;
using System.Drawing;
using System.Numerics;

namespace Project.Pong
{
    class Ball : IRenderObject
    {
        protected const float startingSpeed = 20f / Unit.Second;
        protected const float speedMultiplier = 1.2f;
        protected const float maxSpeed = 10 * startingSpeed;

        private readonly int radius;

        public Vector2 Position { get; protected set; }
        public Vector2 Direction { get; protected set; }

        public Ball(int radius)
        {
            this.radius = radius;
            Reset();
        }

        public bool IsOut => !Program.ScreenRect.Contains(Position, new Vector2(radius));
        public bool IsMoving => Direction.Length() != 0;

        public virtual void Reset()
        {
            Direction = Vector2.Zero;
            Position = Program.ScreenDimension / 2;
        }

        public void StartMoving(Player servingPlayer)
        {
            Direction = GetBounceDirection(servingPlayer) * startingSpeed;
        }

        /// <summary>
        /// Bounce from the screen edge if collision happened.
        /// </summary>
        /// <returns>True if the ball bounced, false otherwise.</returns>
        public bool BounceFromScreenEdge()
        {
            int edgeTop = Program.ScreenRect.Top + radius;
            int edgeBottom = Program.ScreenRect.Bottom - radius;
            if (Position.Y >= edgeTop && Position.Y <= edgeBottom)
                return false;

            float newY = Math.Max(edgeTop, Math.Min(edgeBottom, Position.Y));
            Position = new Vector2(Position.X, newY);
            Direction = new Vector2(Direction.X, -Direction.Y);
            return true;
        }

        public virtual void Bounce(Player player)
        {
            float newSpeed = Math.Min(Direction.Length() * speedMultiplier, maxSpeed);
            Vector2 bounced = (new Vector2(-Direction.X, Direction.Y) + GetBounceDirection(player));
            Direction = bounced.Normalize() * newSpeed;
        }

        private Vector2 GetBounceDirection(Player player)
            => (Position - player.Position).Normalize();

        public virtual void UpdatePosition(long delta)
        {
            if (!IsMoving)
                return;

            Position += Direction * delta;
        }

        public bool IsCollided(Player player)
        {
            Vector2 difference = player.Position - Position;
            return Math.Abs(difference.X) <= radius + player.Size.X / 2
                && Math.Abs(difference.Y) <= radius + player.Size.Y / 2;
        }

        public void Draw(Graphics g)
        {
            float size = radius * 2;
            Vector2 renderPos = Position - new Vector2(radius);
            g.FillEllipse(Brushes.White, renderPos.X, renderPos.Y, size, size);
        }
    }
}
