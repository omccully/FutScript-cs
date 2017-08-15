using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FutScriptFunctions.Keyboard
{
    /// <summary>
    /// Manages hotkeys.
    /// </summary>
    /// 
    /// <example>
    /// Usage example:
    /// <code>
    /// HotKeyManager hk_manager = new HotKeyManager();
    /// hk_manager.CreateHotKey(Keys.F6, delegate(KeyEventArgs kea) {
    ///     // do something
    ///     
    ///     return true; // prevent other programs from seeing this key press
    /// });
    /// </code>
    /// </example>
    /// 
    public class HotKeyManager
    {
        KeyHook Keyhook { get; set; }
        List<HotKey> HotKeys = new List<HotKey>();

        public HotKeyManager()
        {
            this.Keyhook = new KeyHook(AnyKeyDown, AnyKeyUp);
        }

        /// <summary>
        /// Creates a hotkey
        /// </summary>
        /// <param name="key">Key to create a hotkey for</param>
        /// <param name="keydown_callback">Delegate to handle when key is pressed. 
        /// If this delegate returns true, then the key press will be blocked from registering for other applications.</param>
        /// <param name="keyup_callback">Delegate to handle when key is released. 
        /// If this delegate returns true, then the key press will be blocked from registering for other applications.</param>
        public void CreateHotKey(Keys key, KeyEventCallback keydown_callback, KeyEventCallback keyup_callback = null)
        {
            HotKeys.Add(new HotKey(key, keydown_callback, keyup_callback));
        }

        /// <summary>
        /// Unregisters all hotkeys for a given key
        /// </summary>
        /// <param name="key">Key to remove hotkey(s) for</param>
        /// <returns>The number of hotkeys removed</returns>
        public int DestroyHotKey(Keys key)
        {
            return HotKeys.RemoveAll(hk => hk.Key == key);
        }

        /// <summary>
        /// Remaps all hotkeys for a certain key to a new key
        /// </summary>
        /// <param name="before">Original hotkey</param>
        /// <param name="after">New hotkey</param>
        /// <returns>Number of hotkeys remapped</returns>
        public int Remap(Keys before, Keys after)
        {
            int count = 0;
            foreach(HotKey hk in HotKeys)
            {
                if (hk.Key == before)
                {
                    hk.Key = after;
                    count++;
                }
            }
            return count;
        }

        public bool HotKeyIsDefined(Keys key)
        {
            return HotKeys.Count(hk => hk.Key == key) > 0;
        }

        bool AnyKeyDown(KeyEventArgs kea)
        {
            bool used = false;
            foreach(HotKey hk in HotKeys.Where(hk => hk.Key == kea.KeyCode))
            {
                if (hk.KeyDown != null && hk.KeyDown(kea)) used = true;
            }
            return used; // returning true prevents other apps from seeing key event
        }

        bool AnyKeyUp(KeyEventArgs kea)
        {
            bool used = false;
            foreach (HotKey hk in HotKeys.Where(hk => hk.Key == kea.KeyCode))
            {
                if (hk.KeyUp != null && hk.KeyUp(kea)) used = true;
            }
            return used; // returning true prevents other apps from seeing key event
        }

        private class HotKey
        {
            public Keys Key { get; set; }
            public KeyEventCallback KeyDown { get; set; }
            public KeyEventCallback KeyUp { get; set; }

            public HotKey(Keys Key, KeyEventCallback keydown_callback, KeyEventCallback keyup_callback = null)
            {
                this.Key = Key;
                this.KeyDown = keydown_callback;
                this.KeyUp = keyup_callback;
            }
        }
    }
}
