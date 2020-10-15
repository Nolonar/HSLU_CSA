using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Explorer700Wrapper;

namespace Project
{
    public enum Side
    {
        Left,
        Right
    }

    static class Unit
    {
        public const float Millisecond = TimeSpan.TicksPerMillisecond;
        public const float Second = TimeSpan.TicksPerSecond;
    }

    class PongGame
    {
        private readonly Ball ball;
        private readonly Player player1;
        private readonly Player player2;
        private Player servingPlayer;
        private bool isGameRunning;
        private DateTime previousTime;

        private InputManager inputManager => Program.InputManager;

        public PongGame()
        {
            int playerWidth = 2;
            int playerHeight = 15;
            int ballRadius = 2;
            player1 = new Player(PlayerType.HumanLocal, Side.Left, playerWidth, playerHeight);
            player2 = new Player(PlayerType.CPU, Side.Right, playerWidth, playerHeight);
            ball = new Ball(ballRadius);

            servingPlayer = player1;
            previousTime = DateTime.UtcNow;
        }

        private IEnumerable<Player> GetPlayers()
        {
            return GetRenderObjects().OfType<Player>();
        }

        private IEnumerable<IRenderObject> GetRenderObjects()
        {
            yield return player1;
            yield return player2;
            yield return ball;
        }

        public void Run()
        {
            isGameRunning = true;
            Program.Led1.Enabled = true;
            while (isGameRunning)
            {
                DateTime currentTime = DateTime.UtcNow;
                Update(currentTime - previousTime);
                Draw(Program.Display);
                previousTime = currentTime;
            }
        }

        private void ProcessInput()
        {
            if (!inputManager.IsKeyPressed(Keys.Center))
                return;

            if (!ball.IsMoving)
                ball.StartMoving(servingPlayer);

            if (inputManager.IsKeyPressed(Keys.Center, 1 * Unit.Second))
                isGameRunning = false;
        }

        private void Update(TimeSpan delta)
        {
            ProcessInput();

            long ticks = delta.Ticks;
            ball.UpdatePosition(ticks);

            if (ball.BounceFromScreenEdge())
                Program.Buzzer.Beep(1);

            foreach (Player playerCollided in GetPlayers().Where(p => ball.IsCollided(p)))
            {
                Program.Buzzer.Beep(1);
                ball.Bounce(playerCollided);
            }

            GetPlayers().FirstOrDefault(p => p.Type == PlayerType.HumanLocal)?.MoveHuman(ticks, inputManager.KeysPressed);
            GetPlayers().FirstOrDefault(p => p.Type == PlayerType.CPU)?.MoveCpu(ticks, ball);

            if (ball.IsOut)
            {
                // TODO track score
                ball.Reset();
            }
        }

        private void Draw(Display display)
        {
            display.Clear();
            DrawArena(display.Graphics);
            foreach (IRenderObject o in GetRenderObjects())
                o.Draw(display.Graphics);

            display.Update();
        }

        private void DrawArena(Graphics g)
        {
            var screenDimension = Program.ScreenDimension;
            int middle = screenDimension.Width / 2;
            g.DrawRectangle(Pens.White, new Rectangle(0, 0, screenDimension.Width - 1, screenDimension.Height - 1));
            g.DrawLine(Pens.White, middle, screenDimension.Top, middle, screenDimension.Bottom);
        }
    }
}
