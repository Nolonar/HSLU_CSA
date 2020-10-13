// ----------------------------------------------------------------------------
// CSA - C# in Action
// (c) 2020, Christian Jost, HSLU
// ----------------------------------------------------------------------------
using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Explorer700Wrapper
{
    public class LedGpio : LedBase
    {

        private IGpioPin ledPin;

        public LedGpio()
        {
            ledPin = Pi.Gpio[26]; // BCM 12
            ledPin.PinMode = GpioPinDriveMode.Output; // use as output
        }

        public override Leds Led { get { return Leds.Led1; } }

        public override bool Enabled
        {
            get { return ledPin.Read(); }
            set { ledPin.Write(value); }
        }
    }
}
