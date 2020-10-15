using System;
using System.Numerics;

namespace Project.GravityPong
{
    class Ball : Pong.Ball
    {
        public readonly Vector2 Gravity = new Vector2(0, 0.00001f / Unit.Second);

        private float horizontalSpeed;

        public Ball(int radius) : base(radius) { /* No additional code. */ }

        public override void Reset()
        {
            base.Reset();
            horizontalSpeed = startingSpeed;
        }

        public override void UpdatePosition(long delta)
        {
            base.UpdatePosition(delta);
            if (IsMoving)
                Direction += Gravity * delta;
        }

        public override void Bounce(Pong.Player player)
        {
            base.Bounce(player);

            float horizontalDirection = Direction.X / Math.Abs(Direction.X);
            horizontalSpeed = Math.Min(horizontalSpeed * speedMultiplier, maxSpeed);
            Direction = new Vector2(horizontalDirection * horizontalSpeed, Direction.Y);
        }
    }
}
