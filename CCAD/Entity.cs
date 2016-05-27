using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// Class that represents all the elements that will be drawn
    /// </summary>
    public class Entity
    {
        protected double perimeter;
        protected double area;
        protected int range;
        protected Color originalColor;
        protected PaintEventArgs graph;
        protected bool selected;

        public int LineWidth { get; set; }
        public Color Color { get; set; }

        public Entity(Color color)
        {
            range = 10;
            originalColor = color;
            Color = color;
        }

        /// <summary>
        /// This method draw the entity in the board
        /// </summary>
        /// <param name="graph"></param>
        public virtual void Draw(PaintEventArgs graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// This method restore the current color to the previous color
        /// </summary>
        public void ResetColor()
        {
            Color = originalColor;
        }

        /// <summary>
        /// This method returns the original colour of the entity
        /// </summary>
        /// <returns>originalColor</returns>
        public Color GetOriginalColor()
        {
            return originalColor;
        }

        /// <summary>
        /// This method checks if the mouse is next to the drawing
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if is in range, false if not</returns>
        public virtual bool IsInRange(int x, int y)
        {
            // Defined in each class
            return false;
        }

        /// <summary>
        /// This method returns the area of the entity
        /// </summary>
        /// <returns>area</returns>
        public double GetArea()
        {
            return area;
        }

        /// <summary>
        /// This method returns the perimeter of the entity
        /// </summary>
        /// <returns>perimeter</returns>
        public double GetPerimeter()
        {
            return perimeter;
        }

        /// <summary>
        /// This method marks the entity as selected
        /// </summary>
        public void Selected()
        {
            selected = true;
        }

        /// <summary>
        /// This method checks if the entity is selected
        /// </summary>
        /// <returns>true if selected, and false if not</returns>
        public bool IsSelected()
        {
            return selected;
        }

        /// <summary>
        /// This method changes the selected status to
        /// not selected
        /// </summary>
        public void Free()
        {
            selected = false;
        }
    }
}
