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
        }
    }
}
