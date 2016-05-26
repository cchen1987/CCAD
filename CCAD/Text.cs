using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class store information about a text to draw on screen,
    /// and all it's information
    /// </summary>
    class Text : Entity
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
        }
    }
}
