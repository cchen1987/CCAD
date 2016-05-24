using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    class Block : Entity
    {
        protected Line[] Lines;
        //protected PointF[] Points;

        public Block(Color color, Line[] lines) : base(color)
        {
            Lines = lines;
            for (int i = 0; i < lines.Length; i++)
                perimeter += lines[i].Length;
        }

        // This method returns a line at the entered index
        public Line GetLineAt(int index)
        {
            return Lines[index];
        }

        // This method returns all lines contained in the block
        public Line[] GetLines()
        {
            return Lines;
        }

        // This method splits the block into lines
        public Line[] Split()
        {
            return Lines;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            for (int i = 0; i < Lines.Length; i++)
                Lines[i].Draw(graph);
        }

        public void SetBlockColor(Color color)
        {
            for (int i = 0; i < Lines.Length; i++)
                Lines[i].Color = color;
        }

        public void ResetBlockColor()
        {
            for (int i = 0; i < Lines.Length; i++)
                Lines[i].Color = originalColor;
        }
    }
}
