using SW04_Explorer700;
using System;
using System.Threading;

namespace SW04
{
    class Program
    {
        static void Main(string[] args)
        {
            var explorer700 = new Explorer700();
            explorer700.Led2.Enabled = true;
            explorer700.Buzzer.Beep(10000);
            for (int i = 0; i < 100; i++)
            {
                explorer700.Led1.Toggle();
                explorer700.Led2.Toggle();
                Thread.Sleep(100);
            }
            explorer700.Led2.Enabled = false;
        }
    }
}
