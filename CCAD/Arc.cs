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
            StartPoint = start;
            double lengthX = start.X - CentrePoint.X;
            double lengthY = start.Y - CentrePoint.Y;
            Radius = Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
            StartAngle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);
            lengthX = end.X - CentrePoint.X;
            lengthY = end.Y - CentrePoint.Y;
            double tempAngle = Math.Atan2(lengthY, lengthX) * 180f / Math.PI;
            SweepAngle = tempAngle - StartAngle;
            end.Y = (float)(Math.Cos(tempAngle) * Radius);
            end.X = (float)(Math.Sin(tempAngle) * Radius);
            EndPoint = end;
            perimeter = 2 * Math.PI * Radius * SweepAngle / 360;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawArc(new Pen(new SolidBrush(Color),
                LineWidth), new RectangleF((float)(CentrePoint.X - Radius),
                (float)(CentrePoint.Y - Radius), (float)(2 * Radius), 
                (float)(2 * Radius)), (float)(StartAngle), (float)(SweepAngle));

            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - 2, CentrePoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(StartPoint.X - 2, StartPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(EndPoint.X - 2, EndPoint.Y - 2, 4, 4));
            }
        }

        public override bool IsInRange(int x, int y)
        {
            base.IsInRange(x, y);
            double lengthX = x - CentrePoint.X;
            double lengthY = y - CentrePoint.Y;
            double angle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);
            double distance = Math.Abs(Math.Sqrt((x - CentrePoint.X) * 
                (x - CentrePoint.X) + (y - CentrePoint.Y) * 
                (y - CentrePoint.Y)) - Radius);
            return distance <= range &&
                angle - StartAngle < SweepAngle && angle >= StartAngle;
        }


        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            double leftX = CentrePoint.X - Radius;
            double rightX = CentrePoint.X + Radius;
            double topY = CentrePoint.Y - Radius;
            double botY = CentrePoint.Y + Radius;
            if (minX <= leftX && maxX >= rightX && minY <= topY && maxY >= botY)
                return true;

            return false;
        }
    }
}
