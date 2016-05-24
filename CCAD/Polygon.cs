using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    class Polygon : Block
    {
        public PointF CentrePoint { get; set; }

        public Polygon(Color color, Line[] lines, PointF point) : base(color, lines)
        {
            CentrePoint = point;
            double a = lines[0].EndPoint.Y - lines[0].StartPoint.Y;
            double b = lines[0].EndPoint.X - lines[0].StartPoint.X;
            double c = a * lines[0].StartPoint.X;
            double d = b * lines[0].StartPoint.Y;
            double e = -c + d;
            // Equation of distance between a point and a line
            double apothem = Math.Abs(a * CentrePoint.X - b * CentrePoint.Y +
                e) / Math.Sqrt(a * a + b * b);
            area = apothem * perimeter / 2f;
        }
    }
}
