using System.Numerics;

namespace Project.GravityPong
{
    class Ball : Pong.Ball
    {
        public readonly Vector2 Gravity = new Vector2(0, 10f / Unit.Second);

        private readonly float bounceCoefficient = 0.9f;

        public Ball(int radius) : base(radius) { /* No additional code. */ }

        public override void UpdatePosition(long delta)
        {
            base.UpdatePosition(delta);
            Direction += Gravity * delta;
        }

        public override bool BounceFromScreenEdge()
        {
            bool didBounce = base.BounceFromScreenEdge();
            if (didBounce)
                Direction = Direction.Normalize() * Direction.Length() * bounceCoefficient;

            return didBounce;
        }
    }
}
