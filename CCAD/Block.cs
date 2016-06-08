using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// This class encompasses all rectilinear drawings
    /// </summary>
    public class Block : Entity
    {
        protected Line[] lines;

        public Block(Color color, Line[] lines, int lineWidth) : base(color)
        {
            this.lines = lines;
            LineWidth = lineWidth;
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
            return lines[index];
        }

        /// <summary>
        /// This method returns all lines contained in the block
        /// </summary>
        /// <returns>Line[]</returns>
        public Line[] GetLines()
        {
            return lines;
        }

        // This method splits the block into lines
        public Line[] Split()
        {
            return lines;
        }

        public override void Draw(PaintEventArgs graph)
        {
            base.Draw(graph);
            for (int i = 0; i < lines.Length; i++)
                lines[i].Draw(graph);
        }

        /// <summary>
        /// This method changes the colour of all lines in the block
        /// </summary>
        /// <param name="color"></param>
        public void SetBlockColor(Color color)
        {
            for (int i = 0; i < lines.Length; i++)
                lines[i].Color = color;
        }

        /// <summary>
        /// This method reset the colour of all lines in the block
        /// to their previous color
        /// </summary>
        public void ResetBlockColor()
        {
            for (int i = 0; i < lines.Length; i++)
                lines[i].Color = originalColor;
        }

        // This method checks if all lines are inside of the selection area
        public override bool IsInside(double minY, double maxY, double minX,
                double maxX)
        {
            int count = 0;
            // Count selected lines
            for (int i = 0; i < lines.Length; i++)
                count += lines[i].IsSelected() ? 1 : 0;

            // If all lines selected, return true
            if (count == lines.Length)
                return true;
            else
            {
                for (int i = 0; i < lines.Length; i++)
                    lines[i].Free();

                return false;
            }
        }

        /// <summary>
        /// This method checks if any of lines in the block is in mouse range
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if is in range, false if not</returns>
        public override bool IsInRange(int x, int y)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IsInRange(x, y))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// This method sets temporal color for all lines
        /// </summary>
        public override void SetTemporalColor()
        {
            base.SetTemporalColor();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetTemporalColor();
            }
        }

        /// <summary>
        /// This method resets color to original color
        /// </summary>
        public override void ResetColor()
        {
            base.ResetColor();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].ResetColor();
            }
        }

        /// <summary>
        /// This method activates the selected property on each line in block
        /// </summary>
        public override void Selected()
        {
            base.Selected();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Selected();
            }
        }

        /// <summary>
        /// This method deactivates the selected property on each line in block
        /// </summary>
        public override void Free()
        {
            base.Free();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].Free();
            }
        }
    }
}
