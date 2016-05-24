using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
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
