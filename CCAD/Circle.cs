using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class is a circle, defined by a centre point and a radius
    /// </summary>
    public class Circle : CurvedLine
    {
        public Circle(Color color, PointF point, int width, double radius)
                : base(color, point, width, radius)
        {
            perimeter = 2 * Math.PI * Radius;
            area = Math.PI * Radius * Radius;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawEllipse(new Pen(new SolidBrush(Color),
                LineWidth), new RectangleF((float)(CentrePoint.X - Radius),
                (float)(CentrePoint.Y - Radius), (float)(2 * Radius),
                (float)(2 * Radius)));

            // Draw main points of the circle when selected
            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - displace, CentrePoint.Y -
                    displace, pointWidth, pointHeight));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF((float)(CentrePoint.X - displace - Radius),
                    CentrePoint.Y - displace, pointWidth, pointHeight));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF((float)(CentrePoint.X - displace + Radius), 
                    CentrePoint.Y - displace, pointWidth, pointHeight));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - displace, (float)(
                    CentrePoint.Y - displace - Radius), pointWidth, 
                    pointHeight));
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    new RectangleF(CentrePoint.X - displace, (float)(
                    CentrePoint.Y - displace + Radius), pointWidth, 
                    pointHeight));
            }
        }

        /// <summary>
        /// This method checks if the circle is inside the selection area
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
