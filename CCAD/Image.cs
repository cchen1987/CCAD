using System.Drawing;
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

        public string Path { get; set; }
        public PointF StartPoint { get; set; }

        public Image(Color color, PointF point, string path) : base(color)
        {
            StartPoint = point;
            Path = path;
            width = System.Drawing.Image.FromFile(Path).Size.Width;
            height = System.Drawing.Image.FromFile(Path).Size.Height;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawImage(System.Drawing.Image.FromFile(Path),
                StartPoint);
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
    }
}
