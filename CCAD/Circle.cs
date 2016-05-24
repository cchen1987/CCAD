using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCAD
{
    class Circle : CurvedLine
    {
        public Circle(Color color, PointF point, double radius)
                : base(color, point, radius)
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
