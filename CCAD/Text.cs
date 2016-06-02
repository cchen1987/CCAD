using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class store information about a text to draw on screen,
    /// and all it's information
    /// </summary>
    public class Text : Entity
    {
        public string Phrase { get; set; }
        public int Size { get; set; }
        public FontFamily FontFamily { get; set; }
        public PointF Point { get; set; }
        public Font Font { get; }
        
        public Text(Color color, int size, FontFamily fontFamily, PointF point,
            string text) : base(color)
        {
            Point = point;
            Phrase = text;
            Size = size;
            FontFamily = fontFamily;
            Font = new Font(FontFamily, Size);
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            graph.Graphics.DrawString(Phrase, Font, 
                new SolidBrush(Color), Point);

            if (selected)
            {
                graph.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue),
                    Point.X, Point.Y, LineWidth, LineWidth);
            }
        }

        public override bool IsInside(double minY, double maxY, double minX,
                 double maxX)
        {
            if (minX <= Point.X && maxX >= Point.X + Phrase.Length &&
                    minY <= Point.Y && maxY >= Point.Y + Size)
            {
                return true;
            }
            return false;
        }
    }
}
