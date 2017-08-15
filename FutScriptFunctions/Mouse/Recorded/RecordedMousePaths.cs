using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Collections;
using Newtonsoft.Json;
using FutScriptFunctions.Numbers;

namespace FutScriptFunctions.Mouse.Recorded
{
    public class RecordedMousePaths
    {
        List<SerializableCursorPath> Paths { get; set; }

        public RecordedMousePaths()
        {
            Paths = new List<SerializableCursorPath>();
        }

        private RecordedMousePaths(IEnumerable<SerializableCursorPath> paths)
        {
            this.Paths = paths.ToList();
        }

        public SerializableCursorPath GetRandomSuitablePath(int dx, int dy)
        {
            const double INITIAL_TOLERANCE = 0.5;
            const double MAX_TOLERANCE = 4.0;
            const int MAX_LOOPS = 100;
            const int MINIMUM_SUITABLE_PATHS = 10;

            IEnumerable<SerializableCursorPath> SuitablePaths = null;

            SerializablePoint Destination = new SerializablePoint(dx, dy);

            double tolerance = INITIAL_TOLERANCE;

            int i = 0;
            do
            {
                if(i > MAX_LOOPS)
                {
                    throw new Exception("Not enough suitable paths found");
                }

                SuitablePaths = Paths.Where(p => 
                    p.Destination.IsTolerant(Destination, tolerance));

                tolerance *= 1.5;
                i++;
            } while (SuitablePaths.Count() < MINIMUM_SUITABLE_PATHS &&
                !(tolerance > MAX_TOLERANCE && SuitablePaths.Count() > 0));
            // (tolerance <= MAX_TOLERANCE || SuitablePaths.Count == 0)
            // SuitablePaths.Count == 0 || (SuitablePaths.Count() < MINIMUM_SUITABLE_PATHS)

            // select a random SuitablePath
            return SuitablePaths.ElementAt(
                RandomGenerator.NextInt(SuitablePaths.Count()));
        }

        public string ToJson()
        {
            SerializableCursorPath[] SerializablePaths = Paths.ToArray();
            return JsonConvert.SerializeObject(SerializablePaths);
        }

        public static RecordedMousePaths FromJson(string json)
        {
            return new RecordedMousePaths(JsonConvert.DeserializeObject<SerializableCursorPath[]>(json));
        }

        public void Add(SerializableCursorPath item)
        {
            Paths.Add(item);
        }

        /*
         * If ever want to implement IList<SerializableCursorPath>:
         * 
        #region Implementation of IEnumerable

        public IEnumerator<SerializableCursorPath> GetEnumerator()
        {
            return Paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<SerializableCursorPath>

        public void Add(SerializableCursorPath item)
        {
            Paths.Add(item);
        }

        public void Clear()
        {
            Paths.Clear();
        }

        public bool Contains(SerializableCursorPath item)
        {
            return Paths.Contains(item);
        }

        public void CopyTo(SerializableCursorPath[] array, int arrayIndex)
        {
            Paths.CopyTo(array, arrayIndex);
        }

        public bool Remove(SerializableCursorPath item)
        {
            return Paths.Remove(item);
        }

        public int Count
        {
            get { return Paths.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of IList<SerializableCursorPath>

        public int IndexOf(SerializableCursorPath item)
        {
            return Paths.IndexOf(item);
        }

        public void Insert(int index, SerializableCursorPath item)
        {
            Paths.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Paths.RemoveAt(index);
        }

        public SerializableCursorPath this[int index]
        {
            get { return Paths[index]; }
            set { Paths[index] = value; }
        }

        #endregion
 */
    }

    public class SerializablePoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SerializablePoint(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public bool IsTolerant(SerializablePoint other, double tolerance)
        {
            return AreDimensionsTolerant(this.X, other.X, tolerance) &&
                AreDimensionsTolerant(this.Y, other.Y, tolerance);
        }

        public double DistanceTo(int x, int y)
        {
            return Math.Sqrt(
                Math.Pow(X - x, 2) +
                Math.Pow(Y - y, 2));
        }

        public double DistanceTo(SerializablePoint b)
        {
            return DistanceTo(b.X, b.Y);
        }

        public double DistanceTo(Point b)
        {
            return DistanceTo(FromPoint(b));
        }

        private static bool AreDimensionsTolerant(int a, int b, double tolerance)
        {
            return a - a * (tolerance / 4) - 5 <= b && b <= a + a * tolerance + 5;
        }

        public static SerializablePoint FromPoint(Point p)
        {
            return new SerializablePoint(p.X, p.Y);
        }
    }

    public class SerializableRectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        private bool Initialized { get; set; }

        public SerializableRectangle()
        {
            Initialized = false;
        }

        public SerializableRectangle(int Left, int Top, int Right, int Bottom)
        {
            this.Left = Left;
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            Initialized = true;
        }

        public void PushBounds(int X, int Y)
        {
            if (Initialized)
            {
                Left = Math.Min(X, Left);
                Top = Math.Min(Y, Top);
                Right = Math.Max(X, Right);
                Bottom = Math.Max(Y, Bottom);
            }
            else
            {
                Left = Right = X;
                Top = Bottom = Y;
                Initialized = true;
            }
        }

        public bool Contains(SerializablePoint point)
        {
            return Left <= point.X && point.X <= Right &&
                Top <= point.Y && point.Y <= Bottom;
        }

        public bool Contains(Point point)
        {
            return Contains(SerializablePoint.FromPoint(point));
        }
    }

    public class SerializableCursorPath
    {
        // the point in which the mouse button goes down
        public SerializablePoint Destination { get; set; }

        // bounds of the path
        public SerializableRectangle Bounds { get; set; }

        // the relative time in ms which the mouse button goes down
        public int MouseDownTime { get; set; }

        /// <summary>
        /// [ [time,x,y], [time,x,y], ... ]
        /// </summary>
        public int[][] Path { get; set; }

        public SerializableCursorPath(SerializablePoint destination, SerializableRectangle bounds,
            int MouseDownTime, int[][] path)
        {
            this.Destination = destination;
            this.Bounds = bounds;
            this.MouseDownTime = MouseDownTime;
            this.Path = path;
        }

        public int GetMouseUpTime()
        {
            int len = Path.Length;
            if (len == 0) throw new Exception("No points in path");

            // [0] element is the relative time
            return Path[len - 1][0];
        }
    }

}
