using System.Drawing;

namespace CCAD
{
    /// <summary>
    /// This class is a set of 4 lines
    /// </summary>
    class Rectangle : Block
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public PointF StartPoint { get; set; }

        public Rectangle(Color color, Line[] lines) : base(color, lines)
        {
            StartPoint = lines[0].StartPoint;
            Width = lines[0].EndPoint.X - lines[0].StartPoint.X;
            Height = lines[0].EndPoint.Y - lines[0].StartPoint.Y;
            area = Width * Height;
        }
    }
}
