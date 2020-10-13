using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Project
{
    public enum PlayerType
    {
        HumanLocal,
        HumanRemote,
        CPU
    }

    class Player : RenderObject
    {
        public Vector2 Position;
        public Vector2 Size;
        public PlayerType Type { get; }

        private const float speed = 20 / Unit.Second;

        public Player(PlayerType type)
        {
            Type = type;
        }

        private bool IsBallApproaching(Ball ball)
        {
            float difference = Position.X - ball.Position.X;
            // if different signs, multiplication will be negative, otherwise positive.
            bool isSameSign = difference * ball.Direction.X >= 0;
            return ball.IsMoving && isSameSign;
        }

        public void MoveHuman(long delta)
        {

        }

        public void MoveCpu(long delta, Ball ball)
        {
            float targetY = IsBallApproaching(ball) ? ball.Position.Y : 0;
        }

        public void MoveTo(long delta)
        {

        }

        public void Draw(Graphics g)
        {
            Vector2 renderPos = Position - (Size / 2);
            g.FillRectangle(Brushes.White, renderPos.X, renderPos.Y, Size.X, Size.Y);
        }
    }
}
