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

            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    StartPoint.X - 2, StartPoint.Y - 2, 4, 4);
            }
        }

        public override bool IsInRange(int x, int y)
        {
            base.IsInRange(x, y);
            return Math.Abs(StartPoint.X - x) <= range &&
                Math.Abs(StartPoint.Y - y) <= range;
        }

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
