using System;
using System.Drawing;

namespace Project
{
    class Ball : RenderObject
    {
        private const float defaultSpeed = 20 / Unit.Second;
        private const float speedMultiplier = 1.2f;
        private const float maxSpeed = 10 * defaultSpeed;

        private readonly int radius;

        private float speed;
        private Vector2 previousPosition;

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }

        public Ball(int radius)
        {
            this.radius = radius;
            Reset();
            previousPosition = Position;
        }

        public bool IsOut => !PongGame.ScreenDimension.Contains(Position, new Vector2(radius, radius));
        public bool IsMoving => Direction.Length != 0;

        public void Reset()
        {
            Direction = new Vector2(0, 0);
            Position = new Vector2(PongGame.ScreenDimension.Width / 2, PongGame.ScreenDimension.Height / 2);
            speed = defaultSpeed;
        }

        public void StartMoving(Player servingPlayer)
        {
            Direction = GetBounceDirection(servingPlayer);
        }

        /// <summary>
        /// Bounce from the screen edge if collision happened.
        /// </summary>
        /// <returns>True if the ball bounced, false otherwise.</returns>
        public bool BounceFromScreenEdge()
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

        public void Bounce(Player player)
        {
            Direction = (new Vector2(-Direction.X, Direction.Y) + GetBounceDirection(player)).Normalize();
            speed = Math.Min(speed * speedMultiplier, maxSpeed);
        }

        private Vector2 GetBounceDirection(Player player)
        {
            return (Position - player.Position).Normalize();
        }

        public void UpdatePosition(long delta)
        {
            if (!IsMoving)
                return;

            Position += Direction * (speed * delta);
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

            Vector2 renderPos = previousPosition - new Vector2(radius, radius);
            g.FillEllipse(Brushes.Black, renderPos.X, renderPos.Y, size, size);

            renderPos = Position - new Vector2(radius, radius);
            g.FillEllipse(Brushes.White, renderPos.X, renderPos.Y, size, size);

            previousPosition = Position;
        }
    }
}
