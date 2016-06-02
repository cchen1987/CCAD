using System.Drawing;
using System;

namespace CCAD
{
    /// <summary>
    /// This class is a set of 4 lines
    /// </summary>
    public class Rectangle : Block
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public PointF StartPoint { get; set; }

        public Rectangle(Color color, Line[] lines) : base(color, lines)
        {
            StartPoint = lines[0].StartPoint;
            for (int i = 0; i < Lines.Length; i++)
            {
                StartPoint = lines[i].StartPoint.X > StartPoint.X || 
                    (lines[i].StartPoint.X == StartPoint.X && 
                    lines[i].StartPoint.Y > StartPoint.Y) ? 
                    StartPoint : lines[i].StartPoint;
            }
            Width = Math.Abs(lines[0].StartPoint.X - lines[1].StartPoint.X);
            Height = Math.Abs(lines[0].EndPoint.Y - lines[1].EndPoint.Y);
            area = Width * Height;
        }

        // Save until draw rectangle method available and check if functional
        //public override bool IsInside(double minY, double maxY, double minX,
        //        double maxX)
        //{
        //    if (minX <= StartPoint.X && maxX >= StartPoint.X + Width && 
        //            minY <= StartPoint.Y && maxY >= StartPoint.Y + Height)
        //        return true;

        //    return false;
        //}
    }
}
