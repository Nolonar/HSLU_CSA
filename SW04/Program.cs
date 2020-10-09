using SW04_Explorer700;
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
            Explorer700.Joystick.JoystickChanged += Joystick_JoystickChanged;

            Explorer700.Led2.Enabled = true;
            Explorer700.Buzzer.Beep(100);
            Draw(Explorer700.Display.Graphics);
            for (int i = 0; i < 100; i++)
            {
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

        private static void Draw(Graphics g)
        {
            using (Brush b = Brushes.White)
            {
                g.DrawString("Hello world", SystemFonts.DefaultFont, b, 0, 0);
            }
            Explorer700.Display.Update();
        }
    }
}
