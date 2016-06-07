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
        private PointF leftPoint;
        private PointF topPoint;
        private PointF rightPoint;
        private PointF botPoint;
        
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
            // Calculate left point
            leftPoint.X = CentrePoint.X - Math.Abs(majorAxisPoint.X - 
                CentrePoint.X);
            leftPoint.Y = CentrePoint.Y;
            // Calculate top point
            topPoint.X = CentrePoint.X;
            topPoint.Y = CentrePoint.Y - Math.Abs(minorAxisPoint.Y - 
                CentrePoint.Y);
            // Calculate right point
            rightPoint.X = CentrePoint.X + Math.Abs(majorAxisPoint.X - 
                CentrePoint.X);
            rightPoint.Y = CentrePoint.Y;
            // Calculate bottom point
            botPoint.X = CentrePoint.X;
            botPoint.Y = CentrePoint.Y + Math.Abs(minorAxisPoint.Y - 
                CentrePoint.Y);
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
            // Draw the 5 main points of an ellipse
            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(leftPoint.X - 2, leftPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(topPoint.X - 2, topPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - 2, CentrePoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(rightPoint.X - 2, rightPoint.Y - 2, 4, 4));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(botPoint.X - 2, botPoint.Y - 2, 4, 4));
            }
        }

        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            if (minX <= leftPoint.X && maxX >= rightPoint.X &&
                    minY <= topPoint.Y && maxY >= botPoint.Y)
                return true;

            return false;
        }
    }
}
