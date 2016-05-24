using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCAD
{
    class Entity
    {
        protected double perimeter;
        protected double area;
        protected int range;
        protected Color originalColor;
        protected PaintEventArgs graph;

        public int LineWidth { get; set; }
        public Color Color { get; set; }

        public Entity(Color color)
        {
            range = 10;
            originalColor = color;
            Color = color;
        }

        /// <summary>
        /// This method draw the painting in the board
        /// </summary>
        /// <param name="graph"></param>
        public virtual void Draw(PaintEventArgs graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// This method restore the current color to the original color
        /// </summary>
        public void ResetColor()
        {
            Color = originalColor;
        }

        /// <summary>
        /// This method checks if the mouse is next to the drawing
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual bool IsInRange(int x, int y)
        {
            // Defined in each class
            return false;
        }

        /// <summary>
        /// This method returns the area of the entity
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            return area;
        }

        /// <summary>
        /// This method returns the perimeter of the entity
        /// </summary>
        /// <returns></returns>
        public double GetPerimeter()
        {
            return perimeter;
        }
    }
}
