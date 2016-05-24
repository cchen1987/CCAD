using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    class Text : Entity
    {
        public string Phrase { get; set; }
        public int Size { get; set; }
        public FontFamily FontFamily { get; set; }
        public PointF Point { get; set; }
        public Font Font { get; }
        
        public Text(Color color, int size, FontFamily fontFamily, PointF point)
                : base(color)
        {
            Point = point;
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
