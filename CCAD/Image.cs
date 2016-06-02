﻿using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class is the container of an image,
    /// which saves the image file path
    /// </summary>
    public class Image : Entity
    {
        private int width;
        private int height;
        private PointF[] points;

        public string Path { get; set; }
        public PointF StartPoint { get; set; }

        public Image(Color color, PointF point, string path) : base(color)
        {
            StartPoint = point;
            Path = path;
            width = System.Drawing.Image.FromFile(Path).Size.Width;
            height = System.Drawing.Image.FromFile(Path).Size.Height;
            points = new PointF[] { StartPoint, new PointF(StartPoint.X + width, 
                StartPoint.Y), new PointF(StartPoint.X, StartPoint.Y + height),
                new PointF(StartPoint.X + width, StartPoint.Y + height) };
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawImage(System.Drawing.Image.FromFile(Path),
                StartPoint);

            if (selected)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                        new RectangleF(points[i].X - 2, points[i].Y - 2, 4, 4));
                }
            }
        }


        public override bool IsInRange(int x, int y)
        {
            return (StartPoint.X - range <= x && StartPoint.X + range >= x &&
                StartPoint.Y - range <= y && StartPoint.Y + height + range >= y) ||
                (StartPoint.X + width - range <= x && 
                StartPoint.X + width + range >= x &&
                StartPoint.Y - range <= y && StartPoint.Y + height + range >= y) ||
                (StartPoint.X - range <= x && StartPoint.X + width + range >= x &&
                StartPoint.Y - range <= y && StartPoint.Y + range >= y) ||
                (StartPoint.X - range <= x && StartPoint.X + width + range >= x &&
                StartPoint.Y + height - range <= y && 
                StartPoint.Y + height + range >= y);
        }

        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            if (minX <= StartPoint.X && maxX >= StartPoint.X + width &&
                    minY <= StartPoint.Y && maxY >= StartPoint.Y + height)
                return true;

            return false;
        }
    }
}
