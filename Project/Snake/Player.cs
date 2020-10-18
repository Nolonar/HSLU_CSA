using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

using Explorer700Wrapper;

namespace Project.Snake
{
    class Player : IRenderObject
    {
        enum Direction
        {
            None,
            Up,
            Left,
            Right,
            Down
        }

        private readonly Vector2 startingPosition;
        private readonly LinkedList<Vector2> bodySegmentPositions = new LinkedList<Vector2>();
        private readonly Dictionary<Keys, Direction> keyDirectionMapper = new Dictionary<Keys, Direction>()
        {
            [Keys.Up] = Direction.Up,
            [Keys.Left] = Direction.Left,
            [Keys.Right] = Direction.Right,
            [Keys.Down] = Direction.Down
        };

        private Direction direction;

        public Vector2 Position { get; private set; }

        public bool IsMoving => direction != Direction.None;
        public IEnumerable<Vector2> PositionsOccupied => bodySegmentPositions.Concat(new[] { Position });

        public Player(Vector2 startingPosition)
        {
            this.startingPosition = new Vector2((int)startingPosition.X, (int)startingPosition.Y);

            Reset();
        }

        public void Grow() => bodySegmentPositions.AddLast(Position);
        public void Shrink() => bodySegmentPositions.RemoveFirst();
        public bool IsSelfCollided() => bodySegmentPositions.Any(p => p == Position);

        public void Eat(Food food)
        {
            Grow();
            food.MoveToNewPosition(this);
        }

        public void Reset()
        {
            Position = startingPosition;
            bodySegmentPositions.Clear();
            direction = Direction.None;
        }

        public void ChangeDirection(Keys keys)
        {
            Keys keyPressed = keyDirectionMapper.Keys.FirstOrDefault(key => keys.HasFlag(key));
            if (keyDirectionMapper.ContainsKey(keyPressed))
                direction = keyDirectionMapper[keyPressed];
        }

        public void UpdatePosition()
        {
            if (!IsMoving)
                return;

            float newX = Position.X;
            float newY = Position.Y;
            new Dictionary<Direction, Action>()
            {
                [Direction.Up] = () => newY--,
                [Direction.Down] = () => newY++,
                [Direction.Left] = () => newX--,
                [Direction.Right] = () => newX++
            }[direction]();

            newX = (newX + Game.FieldDimension.X) % Game.FieldDimension.X;
            newY = (newY + Game.FieldDimension.Y) % Game.FieldDimension.Y;
            Position = new Vector2(newX, newY);
        }

        public void Draw(Graphics g)
        {
            foreach (RectangleF rect in GetBodyLines())
                g.FillRectangle(Brushes.White, rect);
        }

        private IEnumerable<RectangleF> GetBodyLines()
        {
            return PositionsOccupied.Select(p => {
                Vector2 renderPosition = p * Game.Scaling;
                return new RectangleF(renderPosition.X, renderPosition.Y, Game.Scaling, Game.Scaling);
            });
        }
    }
}
