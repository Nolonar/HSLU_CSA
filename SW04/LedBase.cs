// ----------------------------------------------------------------------------
// CSA - C# in Action
// (c) 2020, Christian Jost, HSLU
// ----------------------------------------------------------------------------

namespace SW04_Explorer700
{
    public abstract class LedBase
    {
        public abstract Leds Led { get; }

        public abstract bool Enabled { get; set; }

        public void Toggle()
        {
            Enabled = !Enabled;
        }

    }
}
