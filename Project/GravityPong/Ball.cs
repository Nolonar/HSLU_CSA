using System.Numerics;

namespace Project.GravityPong
{
    class Ball : Pong.Ball
    {
        public readonly Vector2 Gravity = new Vector2(0, 0.00001f / Unit.Second);

        public Ball(int radius) : base(radius) { /* No additional code. */ }

        public override void UpdatePosition(long delta)
        {
            base.UpdatePosition(delta);
            if (IsMoving)
                Direction += Gravity * delta;
        }
    }
}
