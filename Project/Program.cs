using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using Explorer700Wrapper;

namespace Project
{
    class Program
    {
        public static readonly Rectangle ScreenDimension = new Rectangle(0, 0, 126, 64);
        public static readonly InputManager InputManager;

        private static readonly Dictionary<string, Action> commands = new Dictionary<string, Action>()
        {
            ["Play Pong"] = () => new PongGame().Run(),
            ["Play Gravity Pong"] = () => new PongGame().Run(), // TODO
            ["Exit"] = () => isProgramRunning = false,
            ["Shutdown Raspberry Pi"] = () => Process.Start("halt")
        };
        private static readonly Explorer700 e700;

        public static Display Display => e700.Display;
        public static Buzzer Buzzer => e700.Buzzer;
        public static LedBase Led1 => e700.Led1;
        public static LedBase Led2 => e700.Led2;
        public static Joystick Joystick => e700.Joystick;

        private static bool isProgramRunning = true;

        static Program()
        {
            e700 = new Explorer700();
            InputManager = new InputManager(Joystick);
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        static void Main(string[] args)
        {
            Utils.WaitForDebugger();

            Run();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Buzzer.Enabled = Led1.Enabled = Led2.Enabled = false;
            Display.Clear(doRefresh: true);
            Joystick.Dispose();
        }

        static void Run()
        {
            while (isProgramRunning)
            {
                new PongGame().Run();
                isProgramRunning = false; // TODO
            }
        }

        static void Draw()
        {

        }
    }
}
