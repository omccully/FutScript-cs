using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using FutScriptFunctions.Win32API;
using FutScriptFunctions.Numbers;
using FutScriptFunctions.Screen;

namespace FutScriptFunctions.Mouse
{
    // TODO: make code generator to generate method overloads for 
    // methods that accept Point or Rectangle objects for coordinates.
    public class MouseActionPerformer
    {
        public enum MovementFunctions
        {
            mouse_event,
            CursorPosition,
            SetCursorPos
        }

        protected delegate void MovementFunction(int x, int y);
        protected MovementFunction JumpPosition { get; set; }
        protected MovementFunction SetPosition { get; set; }

        MovementFunctions _DefaultMovementFunction;
        public MovementFunctions DefaultMovementFunction
        {
            get
            {
                return _DefaultMovementFunction;
            }
            set
            {
                _DefaultMovementFunction = value;
                switch(value)
                {
                    case MovementFunctions.mouse_event:
                        JumpPosition = JumpPositionRelativeUsingMouseEvent;
                        SetPosition = SetPositionUsingMouseEvent;
                        break;
                    case MovementFunctions.SetCursorPos:
                        JumpPosition = JumpPositionRelativeUsingSetCursorPos;
                        SetPosition = SetPositionUsingSetCursorPos;
                        break;
                    case MovementFunctions.CursorPosition:
                    default:
                        JumpPosition = JumpPositionRelativeUsingCursorPosition;
                        SetPosition = SetPositionUsingCursorPosition;
                        break;
                }
            }
        }

        public const double SPEED_DEFAULT = 400.0; // pixels/second
        public const int POLLING_RATE_DEFAULT = 125;
        public Button LeftButton;
        public Button RightButton;
        public Button MiddleButton;

        public MouseActionPerformer(int PollingRate = POLLING_RATE_DEFAULT)
        {
            this.PollingRate = PollingRate; // this also sets PollingPeriod
            this.Polled = true;
            DefaultMovementFunction = MovementFunctions.mouse_event;

            LeftButton = new Button(0x0002, 0x0004, this);
            RightButton = new Button(0x0008, 0x0010, this);
            MiddleButton = new Button(0x0020, 0x0040, this);

        }

        #region Polling
        /// <summary>
        /// Enables simulated mouse polling. 
        /// </summary>
        public bool Polled { get; set; }

        int _PollingRate;
        /// <summary>
        /// Sets polling rate in Hz, and sets equivalent PollingPeriod.
        /// If this property is set to anything other than an integer between 1 and 1000,
        /// an ArgumentOutOfRangeException will be thrown        
        /// </summary>
        public int PollingRate
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

        int _PollingPeriod;
        /// <summary>
        /// Sets polling period in milliseconds, and sets equivalent PollingRate.
        /// If this property is set to anything other than an integer between 1 and 1000,
        /// an ArgumentOutOfRangeException will be thrown
        /// </summary>
        public int PollingPeriod
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
        protected virtual void WaitForNextPoll()
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

        /// <summary>
        /// Get or set the cursor position with absolute coordinates.
        /// </summary>
        public Point Location
        {
            // mainly for public use. use SetPosition or JumpPosition
            // for non-polled cursor movements
            get
            {
                return User32.RelativeToAbsolutePoint(Cursor.Position);
            }
            set
            {
                if (Polled) WaitForNextPoll();
                SetPosition(value.X, value.Y);
            }
        }


        #region Raw Cursor movement methods
        // using Cursor.Position
        void JumpPositionRelativeUsingCursorPosition(int dx, int dy)
        {
            Point start = Cursor.Position;
            Cursor.Position = new Point(start.X + dx, start.Y + dy);
        }

        // using Cursor.Position
        void SetPositionUsingCursorPosition(int x, int y)
        {
            Cursor.Position = User32.AbsoluteToRelativePoint(x, y);
        }

        // using mouse_event
        /// <summary>
        /// Requires mouse acceleration to be off
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        void JumpPositionRelativeUsingMouseEvent(int dx, int dy)
        {
            User32.mouse_event(0x0001, dx, dy, 0, 0);
        }

        // using mouse_event
        void SetPositionUsingMouseEvent(int x, int y)
        {
            Point loc = Location;
            JumpPositionRelativeUsingMouseEvent(x - loc.X, y - loc.Y);
        }
        
        void JumpPositionRelativeUsingSetCursorPos(int dx, int dy)
        {
            Point relative_start = Cursor.Position;
            User32.SetCursorPos(relative_start.X + dx, relative_start.Y + dy);

        }

        void SetPositionUsingSetCursorPos(int x, int y)
        {
            Point relative = User32.AbsoluteToRelativePoint(x, y);
            User32.SetCursorPos(relative.X, relative.Y);
        }
        #endregion

        /// <summary>
        /// Moves mouse cursor to its destination relative to the starting cursor position
        /// </summary>
        /// <param name="dx">X coordinate relative to current cursor position</param>
        /// <param name="dy">Y coordinate relative to current cursor position</param>
        /// <param name="speed">Not implemented by base method</param>
        public virtual void MoveFrom(int dx, int dy, double speed = SPEED_DEFAULT)
        {
            JumpPosition(dx, dy);
        }

        /// <summary>
        /// Moves mouse cursor to its destination
        /// </summary>
        /// <param name="x">Absolute X coordinate to move to</param>
        /// <param name="y">Absolute Y coordinate to move to</param>
        /// <param name="speed">Not implemented by base method</param>
        public virtual void MoveTo(int x, int y, double speed = SPEED_DEFAULT)
        {
            SetPosition(x, y);
        }

        public virtual void MoveToAndClick(int x, int y, Button ButtonToClick, double speed = SPEED_DEFAULT)
        {
            MoveTo(x, y, speed);
            ButtonToClick?.Click();
        }


        #region Random move to within rectangle
        /// <summary>
        /// Move mouse cursor to a random position within a rectangle
        /// </summary>
        /// <param name="x1">Left edge X coordinate</param>
        /// <param name="y1">Top edge Y coordinate</param>
        /// <param name="x2">Right edge X coordinate</param>
        /// <param name="y2">Bottom edge Y coordinate</param>
        /// <param name="speed">Cursor speed</param>
        public void MoveTo(int x1, int y1, int x2, int y2, double speed = SPEED_DEFAULT)
        {
            MoveTo(Rectangle.FromLTRB(x1, y1, x2, y2), speed);
        }

        /// <summary>
        /// Move mouse cursor to a random position within a rectangle
        /// </summary>
        /// <param name="a">Top left point</param>
        /// <param name="b">Bottom right point</param>
        /// <param name="speed">Cursor speed</param>
        public void MoveTo(Point a, Point b, double speed = SPEED_DEFAULT)
        {
            MoveTo(Rectangle.FromLTRB(a.X, a.Y, b.X, b.Y), speed);
        }

        /// <summary>
        /// Move mouse cursor to a random position within a rectangle
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <param name="speed">Cursor speed</param>
        public void MoveTo(Rectangle rect, double speed = SPEED_DEFAULT)
        {
            // keep looping until the cursor is within range
            while(!rect.Contains(Location))
            {
                // WeightedRandom is not always return values within the given range.
                MoveTo(NormalDistributor.WeightedRandom(rect.X, rect.Right),
                    NormalDistributor.WeightedRandom(rect.Y, rect.Bottom));
            }
        }
        #endregion

        /// <summary>
        /// Moves mouse cursor to a certain color
        /// </summary>
        /// <param name="screen_area">Rectangle representing coordinates of the screen area</param>
        /// <param name="move_color">Color to move to</param>
        /// <param name="timeout_ms">How long to wait before giving up, in milliseconds. 0 for unlimited time.</param>
        /// <param name="speed">Cursor move speed</param>
        /// <param name="comparer">A color comparer</param>
        /// <returns>True if success, false if timed out</returns>
        public bool MoveToColor(Rectangle screen_area, ColorChecker checker, int timeout_ms = 0, double speed = SPEED_DEFAULT)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Point p = new Point(0, 0);
            while ((p = ColorDetection.LocationOfColorWithinScreenArea(
                screen_area, checker)).X == -1)
            {
                if (timeout_ms != 0 && stopwatch.ElapsedMilliseconds >= timeout_ms)
                {
                    return false;
                }
                Thread.Sleep(10);
            }
            MoveTo(p.X, p.Y, speed);
            return true;
        }

       

        public class Button
        {
            readonly int DownCode, UpCode;
            readonly MouseActionPerformer parent;

            internal Button(int DownCode, int UpCode, MouseActionPerformer parent)
            {
                this.DownCode = DownCode;
                this.UpCode = UpCode;
                this.parent = parent;
            }

            public void Down(Point point, bool DisablePolling=false)
            {
                if (parent.Polled) parent.WaitForNextPoll();
                User32.mouse_event(DownCode, point.X, point.Y, 0, 0);
            }

            public void Down(bool DisablePolling=false)
            {
                Down(Cursor.Position, DisablePolling);
            }

            public void Up(Point point, bool DisablePolling=false)
            {
                if (parent.Polled) parent.WaitForNextPoll();
                User32.mouse_event(UpCode, point.X, point.Y, 0, 0);
            }

            public void Up(bool DisablePolling = false)
            {
                Up(Cursor.Position, DisablePolling);
            }

            public void Click(int ms = 0)
            {
                Down(); // polled
                Thread.Sleep(ms);
                Up(); // polled
            }
        }
    }
}
