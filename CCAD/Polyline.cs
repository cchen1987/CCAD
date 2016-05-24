using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    class Polyline : Block
    {
        public Polyline(Color color, Line[] lines) : base(color, lines)
        {
        }
    }
}
