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
            end.Y = (float)(Math.Cos(tempAngle) * 180f / Math.PI * Radius);
            end.X = (float)(Math.Sin(tempAngle) * 180f / Math.PI * Radius);
            EndPoint = end;
            perimeter = 2 * Math.PI * Radius * SweepAngle / 360;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawArc(new Pen(new SolidBrush(Color),
                LineWidth), new RectangleF((float)(CentrePoint.X - Radius),
                (float)(CentrePoint.Y - Radius), (float)(2 * Radius), 
                (float)(2 * Radius)), (float)(StartAngle), 
                (float)(SweepAngle));

            //Draw main points of the arc when selected
            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - displace, CentrePoint.Y - 
                    displace, pointWidth, pointHeight));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(StartPoint.X - displace, StartPoint.Y - 
                    displace, pointWidth, pointHeight));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(EndPoint.X - displace, EndPoint.Y - 
                    displace, pointWidth, pointHeight));
            }
        }

        /// <summary>
        /// This method checks if then arc is near enough to mouse
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method checks if the arc is inside of the selection area
        /// </summary>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <returns></returns>
        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            double leftX = CentrePoint.X - Radius;
            double rightX = CentrePoint.X + Radius;
            double topY = CentrePoint.Y - Radius;
            double botY = CentrePoint.Y + Radius;
            if (minX <= leftX && maxX >= rightX && minY <= topY && maxY >=
                    botY)
                return true;

            return false;
        }
    }
}
