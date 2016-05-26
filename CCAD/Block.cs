using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class encompasses all rectilinear drawings
    /// </summary>
    class Block : Entity
    {
        protected Line[] Lines;

        public Block(Color color, Line[] lines) : base(color)
        {
            Lines = lines;
            for (int i = 0; i < lines.Length; i++)
                perimeter += lines[i].Length;
        }

        /// <summary>
        /// This method returns a line at the entered index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Line</returns>
        public Line GetLineAt(int index)
        {
            return Lines[index];
        }

        /// <summary>
        /// This method returns all lines contained in the block
        /// </summary>
        /// <returns>Line[]</returns>
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

        /// <summary>
        /// This method changes the colour of all lines in the block
        /// </summary>
        /// <param name="color"></param>
        public void SetBlockColor(Color color)
        {
            for (int i = 0; i < Lines.Length; i++)
                Lines[i].Color = color;
        }

        /// <summary>
        /// This method reset the colour of all lines in the block
        /// to their previous color
        /// </summary>
        public void ResetBlockColor()
        {
            for (int i = 0; i < Lines.Length; i++)
                Lines[i].Color = originalColor;
        }
    }
}
