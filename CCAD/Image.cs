using System.Drawing;
using System.Windows.Forms;
using System;

namespace CCAD
{
    /// <summary>
    /// This class is the container of an image,
    /// which saves the image file path
    /// </summary>
    public class Image : Entity
    {
        private float width;
        private float height;
        private PointF[] points;
        private Bitmap bitmap;

        public string Path { get; set; }
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }

        public Image(Color color, PointF point, string path, PointF endPoint)
                : base(color)
        {
            StartPoint = point;
            EndPoint = endPoint;
            bitmap = new Bitmap(path);
            Path = path;
            width = Math.Abs(StartPoint.X - endPoint.X);
            height = Math.Abs(StartPoint.Y - endPoint.Y);
            points = new PointF[] { StartPoint, new PointF(StartPoint.X +
                width, StartPoint.Y), new PointF(StartPoint.X, StartPoint.Y +
                height), new PointF(StartPoint.X + width, StartPoint.Y + 
                height) };
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawImage(bitmap, StartPoint.X, StartPoint.Y, width,
                height);

            // Draw the main points of the image when selected
            if (selected)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    graph.Graphics.FillRectangle(new SolidBrush(
                        Color.DarkBlue), new RectangleF(points[i].X - displace,
                        points[i].Y - displace, pointWidth, pointHeight));
                }
            }
        }

        /// <summary>
        /// This method checks if the image is next to the mouse
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool IsInRange(int x, int y)
        {
            return (StartPoint.X - range <= x && StartPoint.X + range >= x &&
                StartPoint.Y - range <= y && StartPoint.Y + height + range >=
                y) || (StartPoint.X + width - range <= x && 
                StartPoint.X + width + range >= x &&
                StartPoint.Y - range <= y && StartPoint.Y + height + range >= 
                y) || (StartPoint.X - range <= x && StartPoint.X + width +
                range >= x && StartPoint.Y - range <= y && StartPoint.Y + 
                range >= y) || (StartPoint.X - range <= x && StartPoint.X + 
                width + range >= x && StartPoint.Y + height - range <= y && 
                StartPoint.Y + height + range >= y);
        }

        /// <summary>
        /// This method checks if the image is inside the selection area
        /// </summary>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <returns></returns>
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
