using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCAD
{
    class Entity
    {
        protected float perimeter;
        protected float area;
        protected int range;
        protected Color originalColor;
        
        public Color Color { get; set; }

        public Entity(Color color)
        {
            range = 5;
            originalColor = color;
            Color = color;
        }
        
        //This method draw the painting in the board
        public virtual void Draw()
        {
        }

        // This method restore the current color to the original color
        public void ResetColor()
        {
            Color = originalColor;
        }

        // This method checks if the mouse is next to the drawing
        public virtual bool IsInRange(int x, int y)
        {
            // TO DO
            return true;
        }
    }
}
