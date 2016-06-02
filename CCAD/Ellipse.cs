using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// Class ellipse, defined by a minor axis, and a major axis
    /// </summary>
    public class Ellipse : CurvedLine
    {
        public PointF MinorAxisPoint { get; set; }
        public PointF MajorAxisPoint { get; set; }
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        
        public Ellipse(Color color, PointF point, int width,
                PointF minorAxisPoint, PointF majorAxisPoint)
                : base(color, point, width)
        {
            MinorAxisPoint = minorAxisPoint;
            MajorAxisPoint = majorAxisPoint;
            // Calculate the top left point of the rectangle that 
            // contains the ellipse
            StartPoint = new PointF(2f * CentrePoint.X - MajorAxisPoint.X,
                MinorAxisPoint.Y);
            // Calculate the bottom right point of the rectangle that 
            // contains the ellipse
            EndPoint = new PointF(MajorAxisPoint.X, 2f * CentrePoint.Y - 
                MinorAxisPoint.Y);
            
            // a is major radius
            double a = Math.Abs(MajorAxisPoint.X - CentrePoint.X);
            // b is minor radius
            double b = Math.Abs(MinorAxisPoint.Y - CentrePoint.Y);

            area = Math.PI * a * b;
            perimeter = Math.PI * Math.Abs(3 * (a + b) - 
                Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawEllipse(new Pen(new SolidBrush(Color),
                LineWidth), StartPoint.X, StartPoint.Y,
                EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);

            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(MinorAxisPoint.X - 2, MinorAxisPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(MajorAxisPoint.X - 2, MajorAxisPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - 2, CentrePoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(StartPoint.X - 2, StartPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(EndPoint.X - 2, EndPoint.Y - 2, 4, 4));
            }
        }

        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            if (minX <= StartPoint.X && maxX >= EndPoint.X &&
                    minY <= StartPoint.Y && maxY >= EndPoint.Y)
                return true;

            return false;
        }
    }
}
