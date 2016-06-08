using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// Basic class that is used to define all drawings
    /// </summary>
    public class Point : Entity
    {
        public PointF StartPoint { get; set; }

        public Point(Color color, PointF point) : base(color)
        {
            StartPoint = point;
            LineWidth = 1;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.FillRectangle(new SolidBrush(Color),
                StartPoint.X, StartPoint.Y, LineWidth, LineWidth);
            // Draw point when selected
            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    StartPoint.X - displace, StartPoint.Y - displace, 
                    pointWidth, pointHeight);
            }
        }

        /// <summary>
        /// This method checks if the point is next to the mouse
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool IsInRange(int x, int y)
        {
            base.IsInRange(x, y);
            return Math.Abs(StartPoint.X - x) <= range &&
                Math.Abs(StartPoint.Y - y) <= range;
        }

        /// <summary>
        /// This method checks if the point is inside the selection area
        /// </summary>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <returns></returns>
        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            if (minX <= StartPoint.X && maxX >= StartPoint.X &&
                    minY <= StartPoint.Y && maxY >= StartPoint.Y)
            {
                return true;
            }
            return false;
        }
    }
}
