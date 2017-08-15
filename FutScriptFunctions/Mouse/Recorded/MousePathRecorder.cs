using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FutScriptFunctions.Mouse.Recorded
{
    public class MousePathRecorder
    {
        MouseHook MouseHook { get; set; }
        RecordedMousePaths Paths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Paths">Existing RecordedMousePaths object to record the data to.</param>
        public MousePathRecorder(RecordedMousePaths Paths)
        {
            this.Paths = Paths;

            // reset current path recording
            ResetPath(); 
        }

        private void ResetPath()
        {
            CurrentPath = new List<int[]>();
            disqualified = false;
            mouse_down_pos = null;
            bounds = new SerializableRectangle();
        }

        uint previous_timestamp = 0;

        // these origins are set once a mouse move occurs 
        // with no movements recorded in CurrentPath
        uint origin_timestamp = 0;
        int origin_x = 0, origin_y = 0;
        int mouse_down_relative_time = 0;

        List<int[]> CurrentPath;
        bool disqualified;
        SerializablePoint mouse_down_pos;
        SerializableRectangle bounds;

        bool MouseEvent(Point p, MouseEventIdentifier id, uint timestamp)
        {
            const double MOUSE_DRAG_LIMIT = 5.0; // 5 pixels

            if (timestamp - previous_timestamp > 1000)
            {
                // this automatically disqualifies the first path as well
                disqualified = true;
            }

            switch (id)
            {
                case MouseEventIdentifier.LeftUp:
                case MouseEventIdentifier.RightUp:
                    SerializablePoint relative_p =
                        new SerializablePoint(p.X - origin_x, p.Y - origin_y);

                    // limits amount of pixels the cursor can drag while pressed
                    if (!disqualified && mouse_down_pos.DistanceTo(relative_p) < MOUSE_DRAG_LIMIT)
                    {
                        CurrentPath.Add(new int[] {
                            (int)(timestamp - origin_timestamp), // relative_time
                            relative_p.X, // relative_x
                            relative_p.Y // relative_y
                        });
                        Paths.Add(new SerializableCursorPath(
                            mouse_down_pos, bounds, mouse_down_relative_time,
                            CurrentPath.ToArray()));
                    }
                    ResetPath();

                    break;
                default:
                    // check if first move, init origins
                    if (CurrentPath.Count() == 0)
                    {
                        origin_timestamp = timestamp;
                        origin_x = p.X;
                        origin_y = p.Y;
                    }

                    int relative_time = (int)(timestamp - origin_timestamp);
                    int relative_x = p.X - origin_x;
                    int relative_y = p.Y - origin_y;

                    if (id == MouseEventIdentifier.LeftDown || id == MouseEventIdentifier.RightDown)
                    {
                        mouse_down_relative_time = relative_time;
                        mouse_down_pos = new SerializablePoint(relative_x, relative_y);
                    }

                    bounds.PushBounds(relative_x, relative_y);

                    // record data relative to start position and time
                    CurrentPath.Add(new int[] { relative_time, relative_x, relative_y });
                    break;
            }

            previous_timestamp = timestamp;
            return false; // don't block mouse input events from going to other applications
        }

        public void StartRecording()
        {
            if (MouseHook != null)
            {
                throw new InvalidOperationException("Mouse recorder was already started");
            }
            MouseHook = new MouseHook(MouseEvent);
        }

        public void StopRecording()
        {
            MouseHook?.Unhook();
        }

        ~MousePathRecorder()
        {
            StopRecording();
        }
    }
}
