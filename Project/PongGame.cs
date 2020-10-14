﻿using System;
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
        public static readonly Rectangle ScreenDimension = new Rectangle(0, 0, 126, 64);

        private readonly Explorer700 E700;
        private readonly Ball ball;
        private readonly Player player1;
        private readonly Player player2;
        private Player servingPlayer;
        private bool isGameRunning;
        private DateTime previousTime;
        private KeyEventArgs joystickState;

        private Display Display => E700.Display;
        private Buzzer Buzzer => E700.Buzzer;
        private LedBase Led1 => E700.Led1;
        private LedBase Led2 => E700.Led2;
        private Joystick Joystick => E700.Joystick;

        public PongGame(Explorer700 e700)
        {
            E700 = e700;
            Joystick.JoystickChanged += Joystick_JoystickChanged;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            int playerWidth = 2;
            int playerHeight = 15;
            int ballRadius = 2;
            player1 = new Player(PlayerType.HumanLocal, Side.Left, playerWidth, playerHeight);
            player2 = new Player(PlayerType.CPU, Side.Right, playerWidth, playerHeight);
            ball = new Ball(ballRadius);

            servingPlayer = player1;
            previousTime = DateTime.UtcNow;
            joystickState = new KeyEventArgs(Keys.NoKey);
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Buzzer.Enabled = Led1.Enabled = Led2.Enabled = false;
            Display.Clear(doRefresh: true);
            Joystick.Dispose();
        }

        private void Joystick_JoystickChanged(object sender, KeyEventArgs e)
        {
            joystickState = e;
        }

        private IEnumerable<Player> GetPlayers()
        {
            return GetRenderObjects().OfType<Player>();
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
            Led1.Enabled = true;
            while (isGameRunning)
            {
                DateTime currentTime = DateTime.UtcNow;
                Update(currentTime - previousTime);
                Draw();
                previousTime = currentTime;
            }
        }

        private void ProcessInput()
        {
            if (!joystickState.Keys.HasFlag(Keys.Center))
                return;

            if (!ball.IsMoving)
                ball.StartMoving(servingPlayer);

            if (joystickState.TicksPressed >= 1 * Unit.Second)
                isGameRunning = false;
        }

        private void Update(TimeSpan delta)
        {
            ProcessInput();

            long ticks = delta.Ticks;
            ball.UpdatePosition(ticks);

            if (ball.BounceFromScreenEdge())
                Buzzer.Beep(1);

            foreach (Player playerCollided in GetPlayers().Where(p => ball.IsCollided(p)))
            {
                Buzzer.Beep(1);
                ball.Bounce(playerCollided);
            }

            GetPlayers().FirstOrDefault(p => p.Type == PlayerType.HumanLocal)?.MoveHuman(ticks, joystickState.Keys);
            GetPlayers().FirstOrDefault(p => p.Type == PlayerType.CPU)?.MoveCpu(ticks, ball);

            if (ball.IsOut)
            {
                // TODO track score
                ball.Reset();
            }
        }

        private void Draw()
        {
            Display.Clear();
            DrawArena(Display.Graphics);
            foreach (RenderObject o in GetRenderObjects())
                o.Draw(Display.Graphics);

            Display.Update();
        }

        private void DrawArena(Graphics g)
        {
            int middle = ScreenDimension.Width / 2;
            g.DrawRectangle(Pens.White, new Rectangle(0, 0, ScreenDimension.Width - 1, ScreenDimension.Height - 1));
            g.DrawLine(Pens.White, middle, ScreenDimension.Top, middle, ScreenDimension.Bottom);
        }
    }
}
