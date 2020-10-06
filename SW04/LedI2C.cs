// ----------------------------------------------------------------------------
// CSA - C# in Action
// (c) 2020, Christian Jost, HSLU
// ----------------------------------------------------------------------------

using System;

namespace SW04_Explorer700
{
    public class LedI2C : LedBase
    {

        public LedI2C(Pcf8574 pcf8574)
        {
            Pcf8574 = pcf8574;
        }

        public Pcf8574 Pcf8574 { get;}

        public override Leds Led { get { return Leds.Led2; } }

        public override bool Enabled
        {
            get { return !Pcf8574[4]; }
            set { Pcf8574[4] = !value; }
        }
    }
}
