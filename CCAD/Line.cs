using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class is one of the basic clases, that will be used by blocks
    /// </summary>
    public class Line : Entity
    {
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        public PointF MidPoint { get; set; }
        public double Length { get; }
        public double LengthX { get; }
        public double LengthY { get; }
        public double Angle { get; set; }

        public Line(Color color, int width, PointF start, PointF end) : base(color)
        {
            StartPoint = start;
            EndPoint = end;
            LineWidth = width;
            MidPoint = new PointF((EndPoint.X + StartPoint.X) / 2f,
                (EndPoint.Y + StartPoint.Y) / 2f);
            LengthX = EndPoint.X - StartPoint.X;
            LengthY = EndPoint.Y - StartPoint.Y;
            Length = Math.Sqrt(LengthX * LengthX + LengthY * LengthY);
            Angle = (Math.Atan2(LengthY, LengthX) * 180f / Math.PI);
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawLine(new Pen(new SolidBrush(Color), LineWidth),
                StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y);

            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(StartPoint.X - 2, StartPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(MidPoint.X - 2, MidPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(EndPoint.X - 2, EndPoint.Y - 2, 4, 4));
            }
        }

        public override bool IsInRange(int x, int y)
        {
            double a = EndPoint.Y - StartPoint.Y;
            double b = EndPoint.X - StartPoint.X;
            double c = a * StartPoint.X;
            double d = b * StartPoint.Y;
            double e = -c + d;
            // Equation of distance between a point and a line
            double distance = Math.Abs(a * x - b * y + e) / 
                Math.Sqrt(a*a + b*b);

            double minX = StartPoint.X < EndPoint.X ? StartPoint.X : EndPoint.X;
            double maxX = StartPoint.X > EndPoint.X ? StartPoint.X : EndPoint.X;
            double minY = StartPoint.Y < EndPoint.Y ? StartPoint.Y : EndPoint.Y;
            double maxY = StartPoint.Y > EndPoint.Y ? StartPoint.Y : EndPoint.Y;

            return ((x >= minX - range && x <= maxX + range &&
                y >= minY - range && y <= maxY + range) ||
                (Math.Sqrt((StartPoint.X - x) * (StartPoint.X - x) +
                (StartPoint.Y - y) * (StartPoint.Y - y)) <= range) ||
                (Math.Sqrt((EndPoint.X - x) * (EndPoint.X - x) + 
                (EndPoint.Y - y) * (EndPoint.Y - y)) <= range)) && 
                range >= distance;
        }

        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            double leftX = StartPoint.X < EndPoint.X ? StartPoint.X : EndPoint.X;
            double rightX = StartPoint.X > EndPoint.X ? StartPoint.X : EndPoint.X;
            double topY = StartPoint.Y < EndPoint.Y ? StartPoint.Y : EndPoint.Y;
            double botY = StartPoint.Y > EndPoint.Y ? StartPoint.Y : EndPoint.Y;
            if (minX <= leftX && maxX >= rightX && minY <= topY && maxY >= botY)
                return true;

            return false;
        }
    }
}
