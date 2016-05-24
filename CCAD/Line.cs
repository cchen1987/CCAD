using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCAD
{
    class Line : Entity
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
    }
}
