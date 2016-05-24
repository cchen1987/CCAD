using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    class Ellipse : CurvedLine
    {
        public PointF MinorAxisPoint { get; set; }
        public PointF MajorAxisPoint { get; set; }
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        
        public Ellipse(Color color, PointF point, double radius, PointF axisP1,
                PointF axisP2) : base(color, point, radius)
        {
            MinorAxisPoint = axisP1;
            MajorAxisPoint = axisP2;
            StartPoint = new PointF(2f * CentrePoint.X - MajorAxisPoint.X,
                MinorAxisPoint.Y);
            EndPoint = new PointF(MajorAxisPoint.X, 2f * CentrePoint.Y - 
                MinorAxisPoint.Y);
            // a is major radius
            double a = Math.Abs(MajorAxisPoint.X - CentrePoint.X);
            // b is minor radius
            double b = Math.Abs(MinorAxisPoint.Y - CentrePoint.Y);
            area = Math.PI * a * b;
            perimeter = Math.PI * Math.Abs(3 * (a + b) - 
                Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawEllipse(new Pen(new SolidBrush(Color),
                LineWidth), StartPoint.X, StartPoint.Y,
                EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);
        }
    }
}
