using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Explorer700Wrapper;

namespace Project.Pong
{
    public enum PlayerType
    {
        HumanLocal,
        HumanRemote,
        CPU
    }

    class Player : IRenderObject
    {
        private const float speed = 20f / Unit.Second;

        public Vector2 Position { get; private set; }
        public Vector2 Size { get; }
        public PlayerType Type { get; }

        public Player(PlayerType type, Side side, int width, int height)
        {
            Type = type;
            Position = GetPositionFromSide(side);
            Size = new Vector2(width, height);
        }

        private Vector2 GetPositionFromSide(Side side)
        {
            int distanceFromEdge = 5;
            int xPos = new Dictionary<Side, int>()
            {
                [Side.Left] = distanceFromEdge,
                [Side.Right] = Program.ScreenDimension.Width - distanceFromEdge
            }[side];
            int yPos = Program.ScreenDimension.Height / 2;
            return new Vector2(xPos, yPos);
        }

        private bool IsBallApproaching(Ball ball)
        {
            float difference = Position.X - ball.Position.X;
            // if different signs, multiplication will be negative, otherwise positive.
            bool isSameSign = difference * ball.Direction.X >= 0;
            return ball.IsMoving && isSameSign;
        }

        private void KeepInViewport()
        {
            float halfSizeY = Size.Y / 2;
            float edgeTop = Program.ScreenDimension.Top + halfSizeY;
            float edgeBottom = Program.ScreenDimension.Bottom - halfSizeY;

            float newY = Math.Max(edgeTop, Math.Min(edgeBottom, Position.Y));
            Position = new Vector2(Position.X, newY);
        }

        public void MoveHuman(long delta, Keys keys)
        {
            if (keys.HasFlag(Keys.Down))
                Move(delta, speed * delta);
            if (keys.HasFlag(Keys.Up))
                Move(delta, -speed * delta);
        }

        public void MoveCpu(long delta, Ball ball)
        {
            float targetY = IsBallApproaching(ball) ? ball.Position.Y : Program.ScreenDimension.Height / 2;
            Move(delta, targetY - Position.Y);
        }

        private void Move(long delta, float directionY)
        {
            float length = Math.Abs(directionY);
            if (length == 0)
                return;

            float distance = speed * delta;
            float newY = Position.Y + directionY / length * Math.Min(distance, length);
            Position = new Vector2(Position.X, newY);

            KeepInViewport();
        }

        public void Draw(Graphics g)
        {
            Vector2 renderPos = Position - (Size / 2);
            g.FillRectangle(Brushes.White, renderPos.X, renderPos.Y, Size.X, Size.Y);
        }
    }
}
