using System;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Project.Snake
{
    class Food : IRenderObject
    {
        private readonly Random random = new Random();

        public Vector2 Position { get; private set; }

        /// <summary>
        /// Chooses a new position that is not currently occupied by player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>False if the food could not be moved (the snake is occupying the entire map).</returns>
        public bool MoveToNewPosition(Player player)
        {
            var newPosition = GetNewPosition(player);
            Position = newPosition ?? Vector2.Zero;
            return newPosition.HasValue;
        }

        private Vector2? GetNewPosition(Player player)
        {
            int x = random.Next(0, (int)Game.FieldDimension.X);
            int y = random.Next(0, (int)Game.FieldDimension.Y);
            for (int i = 0; i < Game.FieldDimension.X; i++)
            {
                for (int j = 0; j < Game.FieldDimension.Y; j++)
                {
                    float newX = (x + i) % Game.FieldDimension.X;
                    float newY = (y + j) % Game.FieldDimension.Y;
                    var newPos = new Vector2(newX, newY);
                    if (player.PositionsOccupied.All(p => p != newPos))
                        return newPos;
                }
            }
            return null; // The snake is occupying the entire map. Congratulations?
        }

        public void Draw(Graphics g)
        {
            Vector2 renderPosition = Position * Game.Scaling;
            var rect = new RectangleF(renderPosition.X, renderPosition.Y, Game.Scaling, Game.Scaling);
            g.FillEllipse(Brushes.White, rect);
        }
    }
}
