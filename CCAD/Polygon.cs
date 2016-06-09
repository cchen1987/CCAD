using System;
using System.Drawing;

namespace CCAD
{
    /// <summary>
    /// This class is a set of n numbers of lines
    /// that defines a regular polygon
    /// </summary>
    public class Polygon : Block
    {
        public PointF CentrePoint { get; set; }

        public Polygon(Color color, Line[] lines, PointF point, int lineWidth)
                : base(color, lines, lineWidth)
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

        /// <summary>
        /// This method checks if the polygon is inside the selection area
        /// </summary>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <returns></returns>
        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            float miniX = lines[0].StartPoint.X;
            float miniY = lines[0].StartPoint.Y;
            float maxiX = miniX;
            float maxiY = miniY;

            // Calculate the minimum x and y and maximum x and y
            for (int i = 0; i < lines.Length; i++)
            {
                miniX = miniX < lines[i].StartPoint.X ? miniX : 
                    lines[i].StartPoint.X;
                miniX = miniX < lines[i].EndPoint.X ? miniX :
                    lines[i].EndPoint.X;
                miniY = miniY < lines[i].StartPoint.Y ? miniY :
                    lines[i].StartPoint.Y;
                miniY = miniY < lines[i].EndPoint.Y ? miniY :
                    lines[i].EndPoint.Y;
                maxiX = maxiX > lines[i].StartPoint.X ? maxiX :
                    lines[i].StartPoint.X;
                maxiX = maxiX > lines[i].EndPoint.X ? maxiX :
                    lines[i].EndPoint.X;
                maxiY = maxiY > lines[i].StartPoint.Y ? maxiY :
                    lines[i].StartPoint.Y;
                maxiY = maxiY > lines[i].EndPoint.Y ? maxiY :
                    lines[i].EndPoint.Y;
            }

            return minX <= miniX && maxX >= maxiX && minY <= miniY &&
                maxY >= maxiY;
        }
    }
}
