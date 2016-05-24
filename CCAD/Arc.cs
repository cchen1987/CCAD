using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    class Arc : CurvedLine
    {
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }

        public Arc(Color color, PointF point, PointF start, PointF end,
                double radius) : base(color, point, radius)
        {
            double lengthX = end.X - CentrePoint.X;
            double lengthY = end.Y - CentrePoint.Y;
            EndAngle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);

            lengthX = start.X - CentrePoint.X;
            lengthY = start.Y - CentrePoint.Y;
            StartAngle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);
            perimeter = 2 * Math.PI * Radius * EndAngle / 360;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawArc(new Pen(new SolidBrush(Color),
                LineWidth), new RectangleF((float)(CentrePoint.X - Radius),
                (float)(CentrePoint.Y - Radius), (float)(2 * Radius), 
                (float)(2 * Radius)), (float)(StartAngle), (float)(EndAngle));
        }

        public override bool IsInRange(int x, int y)
        {
            base.IsInRange(x, y);
            double lengthX = x - CentrePoint.X;
            double lengthY = y - CentrePoint.Y;
            double angle = (Math.Atan2(lengthY, lengthX) * 180f / Math.PI);
            
            return (Math.Abs((x - CentrePoint.X) * (x - CentrePoint.X) +
                (y - CentrePoint.Y) * (y - CentrePoint.Y)) - Radius) <= range &&
                angle - StartAngle < EndAngle && angle >= StartAngle;
        }
    }
}
