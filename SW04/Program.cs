using SW04_Explorer700;
using System;
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
            for (int i = 0; i < 100; i++)
            {
                Explorer700.Led1.Toggle();
                Explorer700.Led2.Toggle();
                Thread.Sleep(100);
            }
            Explorer700.Led2.Enabled = false;
        }

        private static void Joystick_JoystickChanged(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Keys.ToString());
        }
    }
}
