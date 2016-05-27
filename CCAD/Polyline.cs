using System.Drawing;

namespace CCAD
{
    /// <summary>
    /// This class is a set of n numbers of lines
    /// </summary>
    public class Polyline : Block
    {
        public Polyline(Color color, Line[] lines) : base(color, lines)
        {
        }
    }
}
