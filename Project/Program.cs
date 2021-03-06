using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using Explorer700Wrapper;

namespace Project
{
    static class Unit
    {
        public const long Millisecond = TimeSpan.TicksPerMillisecond;
        public const long Second = TimeSpan.TicksPerSecond;
    }

    class Program
    {
        public static readonly Vector2 ScreenDimension = new Vector2(126, 64);
        public static readonly Rectangle ScreenRect = new Rectangle(0, 0, 126, 64);
        public static readonly InputManager InputManager;

        private static readonly List<(string Text, Action Execute)> commands = new List<(string, Action)>()
        {
            ("Play Pong", () => new Pong.Game().Run()),
            ("Play GravityPong", () => new GravityPong.Game().Run()),
            ("Play Snake", () => new Snake.Game().Run()),
            ("Exit", () => isProgramRunning = false)
        };
        private static readonly Explorer700 e700;
        private const long inputDelay = 500 * Unit.Millisecond; // How long to wait before holding a key is recognized as additional key press.

        public static Display Display => e700.Display;
        public static Buzzer Buzzer => e700.Buzzer;
        public static LedBase Led1 => e700.Led1;
        public static LedBase Led2 => e700.Led2;
        public static Joystick Joystick => e700.Joystick;

        private static bool isProgramRunning = true;
        private static int selectedIndex = 0;
        private static long currentKeyDelay = 0;
        private static DateTime lastKeyPress;

        private static (string Text, Action Execute) SelectedCommand => commands[selectedIndex];

        static Program()
        {
            e700 = new Explorer700();
            InputManager = new InputManager(Joystick);
            lastKeyPress = InputManager.LastChanged;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Reset();
        }

        static void Main(string[] args)
        {
            Utils.WaitForDebugger();

            Run();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Reset();
            Joystick.Dispose();
        }

        private static void Reset()
        {
            Buzzer.Enabled = Led1.Enabled = Led2.Enabled = false;
            Display.Clear(doRefresh: true);
        }

        private static void Run()
        {
            while (isProgramRunning)
            {
                MoveSelection();
                Draw(Display.Graphics);
            }
        }

        private static void MoveSelection()
        {
            if (InputManager.IsKeyPressed(Keys.Down, currentKeyDelay))
                selectedIndex = (selectedIndex + 1) % commands.Count;
            if (InputManager.IsKeyPressed(Keys.Up, currentKeyDelay))
                selectedIndex = (selectedIndex - 1) % commands.Count;

            selectedIndex = (selectedIndex + commands.Count) % commands.Count; // selectedIndex could've been negative.

            if (InputManager.IsKeyPressed(Keys.Center) && lastKeyPress != InputManager.LastChanged)
            {
                SelectedCommand.Execute();
                Reset();
                lastKeyPress = InputManager.LastChanged;
            }

            if (InputManager.KeysPressed == Keys.NoKey)
                currentKeyDelay = 0;
            else
                currentKeyDelay += inputDelay;
        }

        private static void Draw(Graphics g)
        {
            Display.Clear();
            for (int i = 0; i < commands.Count; i++)
                DrawCommand(g, SystemFonts.DefaultFont, i, padding: 2);

            Display.Update();
        }

        private static void DrawCommand(Graphics g, Font font, int index, int padding)
        {
            string text = commands[index].Text;
            int distanceFromCenter = index - selectedIndex;
            var textRect = GetTextRectangle(g, font, text, padding, distanceFromCenter);
            if (textRect.Top < 0 || textRect.Bottom > ScreenRect.Height)
                return;

            Brush textBrush = Brushes.White;
            if (distanceFromCenter == 0)
            {
                var selectionRect = new RectangleF(textRect.Left, textRect.Top - padding, textRect.Width, textRect.Height + 2 * padding);
                g.FillRectangle(Brushes.White, selectionRect);
                textBrush = Brushes.Black;
            }

            g.DrawString(text, font, textBrush, textRect);
        }

        private static RectangleF GetTextRectangle(Graphics g, Font font, string text, int margin, int distanceFromCenter)
        {
            float height = g.MeasureString(text, font).Height;
            float center = (ScreenRect.Height - height) / 2;
            float x = 0;
            float y = center + (height + margin) * distanceFromCenter;
            return new RectangleF(x, y, ScreenRect.Width, height);
        }
    }
}
