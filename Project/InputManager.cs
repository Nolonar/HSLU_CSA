using System;
using System.Collections.Generic;
using System.Linq;

using Explorer700Wrapper;

namespace Project
{
    class InputManager
    {
        public static IEnumerable<Keys> UniqueKeys => Enum.GetValues(typeof(Keys)).Cast<Keys>();

        public Keys KeysPressed { get; private set; } = Keys.NoKey;

        private readonly Dictionary<Keys, DateTime> lastPressed = new Dictionary<Keys, DateTime>();

        public InputManager(Joystick joystick)
        {
            joystick.JoystickChanged += Joystick_JoystickChanged;
            lastPressed = UniqueKeys.ToDictionary(k => k, k => DateTime.UtcNow);
        }

        private void Joystick_JoystickChanged(object sender, KeyEventArgs e)
        {
            KeysPressed = e.Keys;
            foreach (Keys key in UniqueKeys.Where(k => KeysPressed.HasFlag(k)))
                lastPressed[key] = DateTime.UtcNow;
        }

        /// <summary>
        /// Determines whether a specified key has been pressed for a specified duration.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <param name="durationTicks">How long the key must have been pressed.</param>
        /// <returns>True if the specified key has been pressed for the specified duration. False otherwise.</returns>
        public bool IsKeyPressed(Keys key, float durationTicks = 0)
            => (PressedDurationTicks(key)?.Ticks ?? -1) >= durationTicks;

        /// <summary>
        /// Determines for how long a specified key is being pressed.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>The pressed duration. Null if the key is not being pressed.</returns>
        public TimeSpan? PressedDurationTicks(Keys key)
            => KeysPressed.HasFlag(key) ? DateTime.UtcNow - lastPressed[key] : (TimeSpan?)null;
    }
}
