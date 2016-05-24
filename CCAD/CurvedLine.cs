using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCAD
{
    class CurvedLine : Entity
    {
        public double Radius { get; set; }
        public PointF CentrePoint { get; set; }

        public CurvedLine(Color color, PointF point, double radius)
                : base(color)
        {
            CentrePoint = point;
            Radius = radius;
        }

        public override bool IsInRange(int x, int y)
        {
            return (Math.Abs((x - CentrePoint.X) * (x - CentrePoint.X) + 
                (y - CentrePoint.Y) * (y - CentrePoint.Y)) - Radius) <= range;
        }
    }
}
