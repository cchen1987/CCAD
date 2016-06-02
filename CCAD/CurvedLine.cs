using System;
using System.Drawing;

namespace CCAD
{
    /// <summary>
    /// this class encompasses all curved drawings
    /// </summary>
    public class CurvedLine : Entity
    {
        public double Radius { get; set; }
        public PointF CentrePoint { get; set; }

        public CurvedLine(Color color, PointF point, int width) : base(color)
        {
            CentrePoint = point;
            LineWidth = width;
        }

        public CurvedLine(Color color, PointF point, int width, double radius)
                : base(color)
        {
            CentrePoint = point;
            Radius = radius;
            LineWidth = width;
        }

        public override bool IsInRange(int x, int y)
        {
            double distance = Math.Abs(Math.Sqrt((x - CentrePoint.X) * 
                (x - CentrePoint.X) + (y - CentrePoint.Y) * 
                (y - CentrePoint.Y)) - Radius);
            return distance <= range;
        }
    }
}
