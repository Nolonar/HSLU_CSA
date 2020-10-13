using Explorer700Wrapper;
using System;
using System.Drawing;
using System.Threading;

namespace SW04
{
    class Program
    {
        static private readonly Explorer700 Explorer700 = new Explorer700();

        static void Main(string[] args)
        {
            Utils.WaitForDebugger();

            Explorer700.Joystick.JoystickChanged += Joystick_JoystickChanged;

            Explorer700.Led2.Enabled = true;
            //Explorer700.Buzzer.Beep(10);
            for (int i = 0; i < 100; i++)
            {
                DrawText("Hello world", i, i / 10f);
                Explorer700.Led1.Toggle();
                Explorer700.Led2.Toggle();
                Thread.Sleep(100);
            }
            Explorer700.Led2.Enabled = false;
            Explorer700.Display.Clear();
        }

        private static void Joystick_JoystickChanged(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Keys.ToString());
        }

        private static void DrawText(string text, float x, float y)
        {
            Display display = Explorer700.Display;
            display.Clear();
            display.Graphics.DrawString(text, SystemFonts.DefaultFont, Brushes.White, x, y);
            display.Update();
        }
    }
}
