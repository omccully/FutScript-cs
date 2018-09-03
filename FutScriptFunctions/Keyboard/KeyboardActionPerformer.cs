using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using FutScriptFunctions.Win32API;
using FutScriptFunctions.Numbers;

namespace FutScriptFunctions.Keyboard
{
    /// <summary>
    /// Simulates keyboard input.
    /// </summary>
    public static class KeyboardActionPerformer
    {
        // Key codes http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731%28v=vs.85%29.aspx

        #region Polling
        /// <summary>
        /// Enables simulated mouse polling. 
        /// </summary>
        public static bool Polled = true;


        static int _PollingRate = 125;
        /// <summary>
        /// Sets polling rate in Hz, and sets equivalent PollingPeriod.
        /// If this property is set to anything other than an integer between 1 and 1000,
        /// an ArgumentOutOfRangeException will be thrown        
        /// </summary>
        public static int PollingRate
        {
            get
            {
                return _PollingRate;
            }
            set
            {
                if (value < 1 || 1000 < value) throw new ArgumentOutOfRangeException();

                _PollingRate = value;
                _PollingPeriod = (1000 / _PollingRate);
            }
        }

        static int _PollingPeriod = 8;
        /// <summary>
        /// Sets polling period in milliseconds, and sets equivalent PollingRate.
        /// If this property is set to anything other than an integer between 1 and 1000,
        /// an ArgumentOutOfRangeException will be thrown
        /// </summary>
        public static int PollingPeriod
        {
            get
            {
                return _PollingPeriod;
            }
            set
            {
                if (value < 1 || 1000 < value) throw new ArgumentOutOfRangeException();

                _PollingPeriod = value;
                _PollingRate = (1000 / _PollingPeriod);

            }
        }

        /// <summary>
        /// Wait for next simulated mouse poll time
        /// </summary>
        static void WaitForNextPoll()
        {
            // this implementation could be improved to be more realistic
            // by making it poll slightly slower
            // for example, a real mouse with polling rate of 125 (period 8 ms)
            // won't stay in sync on the same 8 ms intervals
            // The polls may happen like this: 8 ms, 16 ms, 24 ms, 33 ms, 41 ms, etc

            const int ms_per_second = 1000;
            const int ms_per_minute = ms_per_second * 60;
            const int ms_per_hour = ms_per_minute * 60;

            DateTime now = DateTime.Now;
            int hour_ms = now.Hour * ms_per_hour +
                now.Minute * ms_per_minute +
                now.Second * ms_per_second +
                now.Millisecond;

            int ms_to_wait = _PollingPeriod - (hour_ms % _PollingPeriod);
            Thread.Sleep(ms_to_wait);
        }
        #endregion

        const double SPEED_DEFAULT = 6.7;

        /// <summary>
        /// Types a message
        /// </summary>
        /// <param name="message">Message to type</param>
        /// <param name="speed">Typing speed in keys per second.</param>
        public static void Type(string message, double speed = SPEED_DEFAULT)
        {
            const double HOLD_STD_DEV = 29;
            const double PAUSE_STD_DEV = 11;
            const double HOLD_RATIO = 103.0 / (24.0 + 103.0); // 103 ms hold, 24 ms pause is standard
            const double PAUSE_RATIO = 1.0 - HOLD_RATIO;

            double time_per_key_ms = (1.0 / speed) * 1000.0;
            double hold_mean = HOLD_RATIO * time_per_key_ms;
            double pause_mean = PAUSE_RATIO * time_per_key_ms;

            bool shift_down = false;
            NormalDistributor hold_key_random_time = new NormalDistributor(hold_mean, HOLD_STD_DEV, 0);
            NormalDistributor pause_random_time = new NormalDistributor(pause_mean, PAUSE_STD_DEV, 0);

            try
            {
                foreach (char c in message)
                {
                    KeyData key_data = KeyData.FromCharacter(c);
                    if (key_data.ShiftPressed)
                    {
                        if (!shift_down)
                        {
                            KeyDown(Keys.RShiftKey);
                            shift_down = true;
                        }
                    }
                    else
                    {
                        if (shift_down)
                        {
                            KeyUp(Keys.RShiftKey);
                            shift_down = false;
                        }
                    }

                    TapKey(key_data.KeyCode, hold_key_random_time.GetInt());
                    Thread.Sleep(pause_random_time.GetInt());
                }
            }
            finally
            {
                if (shift_down)
                {
                    KeyUp(Keys.RShiftKey);
                }
            }
        }

        /// <summary>
        /// Taps a key and holds it for a given amount of time in milliseconds 
        /// </summary>
        /// <param name="Key">Key to tap</param>
        /// <param name="ms">Time in milliseconds to hold the key down for.</param>
        public static void TapKey(Keys Key, int ms)
        {
            TapKey((byte)Key, ms);
        }

        /// <summary>
        /// Taps a key and holds it for a given amount of time in milliseconds 
        /// </summary>
        /// <param name="Key">Key to tap</param>
        /// <param name="ms">Time in milliseconds to hold the key down for.</param>
        public static void TapKey(byte KeyCode, int ms)
        {
            KeyDown(KeyCode);
            Thread.Sleep(ms);
            KeyUp(KeyCode);
        }

        /// <summary>
        /// Releases a key
        /// </summary>
        /// <param name="key">Key to release</param>
        public static void KeyUp(Keys key)
        {
            KeyUp((byte)key);
        }

        /// <summary>
        /// Releases a key
        /// </summary>
        /// <param name="key">Key to release</param>
        public static void KeyUp(byte KeyCode)
        {
            if (Polled) WaitForNextPoll();
            User32.keybd_event(KeyCode, 0x45, 2, 0);
        }

        /// <summary>
        /// Presses a key
        /// </summary>
        /// <param name="key">Key to press</param>
        public static void KeyDown(Keys key)
        {
            KeyDown((byte)key);
        }

        /// <summary>
        /// Presses a key
        /// </summary>
        /// <param name="key">Key to press</param>
        public static void KeyDown(byte KeyCode)
        {
            if (Polled) WaitForNextPoll();
            User32.keybd_event(KeyCode, 0x45, 0, 0);
        }

        /// <summary>
        /// KeyData represents both a key and a shift state
        /// </summary>
        struct KeyData
        {
            byte _KeyCode;

            // ModifierStates' bit packing is compatible to win32api's VkKeyScan high-order byte
            // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646329(v=vs.85).aspx
            byte ModifierStates;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="KeyCode">Key code</param>
            /// <param name="ShiftState">True if shift pressed</param>
            /// <param name="CtrlState">True if ctrl pressed</param>
            /// <param name="AltState">True if alt pressed</param>
            public KeyData(byte KeyCode, bool ShiftState, bool CtrlState, bool AltState)
            {
                this._KeyCode = KeyCode;
                this.ModifierStates = 0;
                if (ShiftState) this.ModifierStates |= 1;
                if (CtrlState) this.ModifierStates |= 2;
                if (AltState) this.ModifierStates |= 4;
            }

            private KeyData(byte KeyCode, byte ModifierStates)
            {
                this._KeyCode = KeyCode;
                this.ModifierStates = ModifierStates;
            }

            public byte KeyCode
            {
                get
                {
                    return _KeyCode;
                }
            }

            public bool ShiftPressed
            {
                get
                {
                    return (ModifierStates & 1) != 0;
                }
            }

            public bool CtrlPressed
            {
                get
                {
                    return (ModifierStates & 2) != 0;
                }
            }

            public bool AltPressed
            {
                get
                {
                    return (ModifierStates & 4) != 0;
                }
            }

            public static KeyData FromCharacter(char ch)
            {
                short VkKeyScanResult = User32.VkKeyScan(ch);
                if (VkKeyScanResult == -1)
                {
                    throw new ArgumentException("Invalid character key");
                }
                // low order byte is key code, high order byte includes shift state
                return new KeyData((byte)VkKeyScanResult, (byte)(VkKeyScanResult >> 8));
            }
        }
    }
}
