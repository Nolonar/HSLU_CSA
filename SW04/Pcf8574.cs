// ----------------------------------------------------------------------------
// CSA - C# in Action
// (c) 2020, Christian Jost, HSLU
// ----------------------------------------------------------------------------
using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace SW04_Explorer700
{
    public class Pcf8574
    {
        #region members
        private byte data;
        private byte mask;
        private readonly II2CDevice dev;
        #endregion

        #region constructor & destructor
        public Pcf8574(int address)
        {
            dev = Pi.I2C.AddDevice(address);
            Read();
        }
        #endregion

        #region properties
        /// <summary>
        /// Setzt eine Maske die dazu führt, dass ausgewählte Bits nie auf 0 gesetzt werden.
        /// </summary>
        public byte Mask
        {
            get { return this.mask; }
            set
            {
                this.mask = value;
                Write(data);
            }
        }
        #endregion

        #region indexer
        public bool this[int bit]
        {
            get { return (Read() & (1 << bit)) != 0; }
            set
            {
                byte val = Read();
                int mask = 1 << bit;
                Write(value ? (byte)(val | mask) : (byte)(val & ~mask));
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Liest den Zustand der Port-Pins.
        /// </summary>
        /// <returns>1 Byte mit den 8 Zuständen des Ports</returns>
        public byte Read()
        {
            data = dev.Read();
            return data;
        }

        /// <summary>
        /// Setzt die 8 Pins des Port Expanders auf High oder Low
        /// </summary>
        /// <param name="data">1 Byte mit den 8 Zuständen</param>
        public void Write(byte data)
        {
            this.data = data;
            dev.Write((byte)(data | mask));
        }
        #endregion
    }
}
