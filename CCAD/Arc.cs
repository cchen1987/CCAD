using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class represents a segment of a circle,
    /// defined by 3 points
    /// </summary>
    public class Arc : CurvedLine
    {
        public double StartAngle { get; set; }
        public double SweepAngle { get; set; }
        public PointF StartPoint { get; }
        public PointF EndPoint { get; }

        public Arc(Color color, PointF point, PointF start, PointF end,
                int width) : base(color, point, width)
        {
            double lengthX = end.X - CentrePoint.X;
            double lengthY = end.Y - CentrePoint.Y;
            SweepAngle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);

            lengthX = start.X - CentrePoint.X;
            lengthY = start.Y - CentrePoint.Y;
            StartAngle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);
            perimeter = 2 * Math.PI * Radius * SweepAngle / 360;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawArc(new Pen(new SolidBrush(Color),
                LineWidth), new RectangleF((float)(CentrePoint.X - Radius),
                (float)(CentrePoint.Y - Radius), (float)(2 * Radius), 
                (float)(2 * Radius)), (float)(StartAngle), (float)(SweepAngle));
        }

        public override bool IsInRange(int x, int y)
        {
            base.IsInRange(x, y);
            double lengthX = x - CentrePoint.X;
            double lengthY = y - CentrePoint.Y;
            double angle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);
            
            return (Math.Abs((x - CentrePoint.X) * (x - CentrePoint.X) +
                (y - CentrePoint.Y) * (y - CentrePoint.Y)) - Radius) <= range &&
                angle - StartAngle < SweepAngle && angle >= StartAngle;
        }
    }
}
