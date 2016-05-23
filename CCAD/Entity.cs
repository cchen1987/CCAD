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

        public virtual void Draw()
        {
        }

        public void ResetColor()
        {
            Color = originalColor;
        }

        public virtual bool IsInRange(int x, int y)
        {
            // TO DO
            return true;
        }
    }
}
