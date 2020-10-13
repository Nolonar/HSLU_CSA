using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using Explorer700Wrapper;

namespace Project
{
    static class Unit
    {
        public const float Millisecond = 10;
        public const float Second = 1000 * Millisecond;
        public const float Minute = 60 * Second;
        public const float Hour = 60 * Minute;
    }

    class PongGame
    {
        private readonly Explorer700 E700;
        private readonly Ball ball;
        private readonly Player player1;
        private readonly Player player2;
        private bool isGameRunning;
        private DateTime previousTime;

        public static readonly Rectangle ScreenDimension = new Rectangle(0, 0, 128, 64);

        public PongGame(Explorer700 e700)
        {
            E700 = e700;
            previousTime = DateTime.UtcNow;
            ball = new Ball(5);
            player1 = new Player(PlayerType.HumanLocal);
            player2 = new Player(PlayerType.CPU);
        }

        private IEnumerable<RenderObject> GetRenderObjects()
        {
            yield return player1;
            yield return player2;
            yield return ball;
        }

        public void Run()
        {
            isGameRunning = true;
            while (isGameRunning)
            {
                DateTime currentTime = DateTime.UtcNow;
                Update(currentTime - previousTime);
                Draw();
                previousTime = currentTime;
            }
        }

        private void Update(TimeSpan delta)
        {
            if (ball.BounceFromEdge())
                E700.Buzzer.Beep(1);

            ball.UpdatePosition(delta.Ticks);
        }

        private void Draw()
        {
            E700.Display.Clear();
            DrawMiddleLine(E700.Display.Graphics);
            foreach (RenderObject o in GetRenderObjects())
                o.Draw(E700.Display.Graphics);

            E700.Display.Update();
        }

        private void DrawMiddleLine(Graphics g)
        {
            int middle = ScreenDimension.Width / 2;
            g.DrawLine(Pens.White, middle, ScreenDimension.Top, middle, ScreenDimension.Bottom);
        }
    }
}
