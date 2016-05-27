using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CCAD
{
    public partial class Canvas : Form
    {
        private List<Entity> entities;
        private PointF startPoint;
        private PointF endPoint;
        private PointF previousPoint;
        private Graphics graph;
        private Brush brush;
        private Pen pen;
        private Board myBoard;

        public bool FirstClick { get; set; }
        public bool SecondClick { get; set; }
        public bool ThirdClick { get; set; }
        public bool Draw { get; set; }
        public bool DrawLine { get; set; }
        public bool DrawPolyline { get; set; }
        public bool DrawRectAngle { get; set; }
        public bool DrawPolygon { get; set; }
        public bool DrawText { get; set; }
        public bool DrawPoint { get; set; }
        public bool DrawCircle { get; set; }
        public bool DrawCircleOpposite { get; set; }
        public bool DrawEllipse { get; set; }
        public bool DrawArc { get; set; }
        public bool DrawImage { get; set; }
        public bool ScaleEntity { get; set; }
        public bool Mirror { get; set; }
        public bool Copy { get; set; }
        public bool MoveEntity { get; set; }
        public bool SelectEntity { get; set; }
        public bool Orto { get; set; }
        public int CurrentX { get; set; }
        public int CurrentY { get; set; }
        public int CurrentLineWidth { get; set; }
        public double ZoomIncrement { get; set; }
        public double CurrentZoom { get; set; }
        public double Angle { get; set; }
        public Color CurrentColor { get; set; }

        public Canvas(Board board)
        {
            InitializeComponent();
            // Initialize all elements
            myBoard = board;
            myBoard.lbxCommands.Items.Add("Welcome to CCAD version 1.0 2016");
            myBoard.lbxCommands.Items.Add("Author: Chen Chao");
            entities = new List<Entity>();
            FirstClick = true;
            SecondClick = false;
            ThirdClick = false;
            Draw = false;
            DrawLine = false;
            DrawPolyline = false;
            DrawRectAngle = false;
            DrawPolygon = false;
            DrawText = false;
            DrawPoint = false;
            DrawCircle = false;
            DrawCircleOpposite = false;
            DrawEllipse = false;
            DrawArc = false;
            DrawImage = false;
            ScaleEntity = false;
            Mirror = false;
            Copy = false;
            MoveEntity = false;
            SelectEntity = false;
            Orto = false;
            ZoomIncrement = 0.1f;
            CurrentZoom = 1f;
            Angle = 0;
            graph = CreateGraphics();
            startPoint = new PointF();
            endPoint = new PointF();
            previousPoint = new PointF();
            CurrentLineWidth = Convert.ToInt32(
                myBoard.cbCurrentLineWidth.Items[0]);
            CurrentColor = Color.FromName(
                myBoard.cbColorSelector.Items[0].ToString());
            brush = new SolidBrush(CurrentColor);
            pen = new Pen(brush, CurrentLineWidth);
            lbDinamic.Hide();
            tbDinamic.Hide();

            // Canvas configuration
            TopLevel = false;
            Dock = DockStyle.Fill;
            FormBorderStyle = FormBorderStyle.None;
            // Avoid flickering on canvas
            SetStyle(ControlStyles.DoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

// Main board drawing
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            // Draw all entities
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(e);
        }

// Mouse events
        /// <summary>
        /// This method create entities by mouse clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            // Get line points if Drawing line
            if (DrawLine)
                CreateLine();
        }

        /// <summary>
        /// This method checks if mouse leave the main board
        /// and hide dinamic items when is true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseLeave(object sender, EventArgs e)
        {
            // Hide dinamic items when mouse leave the main board
            tbDinamic.Hide();
            lbDinamic.Hide();
        }

        /// <summary>
        /// This method displays the current x and y of the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            myBoard.lbMouseX.Text = e.X.ToString("0.0000");
            myBoard.lbMouseY.Text = e.Y.ToString("0.0000");

            // Show the mouse current coordinates in the label
            if (Orto && !FirstClick)
            {
                if (Math.Abs(e.X - startPoint.X) > Math.Abs(e.Y - startPoint.Y))
                {
                    CurrentX = e.X;
                    CurrentY = (int)startPoint.Y;
                }
                else
                {
                    CurrentY = e.Y;
                    CurrentX = (int)startPoint.X;
                }
            }
            else
            {
                CurrentX = e.X;
                CurrentY = e.Y;
            }

            if (Draw)
            {
                // Show dinamic items and set their coordinates
                tbDinamic.Show();
                tbDinamic.Location = new System.Drawing.Point(e.X + 10, e.Y + 10);
                tbDinamic.Focus();
                lbDinamic.Show();
                lbDinamic.Location = new System.Drawing.Point(e.X + 10, e.Y - 20);

                // set dinamic label text to mouse coordinates
                if (Draw)
                {
                    lbDinamic.Text = "x: " + e.X.ToString("0.0000") + " y: " +
                        e.Y.ToString("0.0000");
                }
            }
        }

// Keyboard events
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SilenceKeySound(e);
                // Reset all actions when key escape pressed
                ResetAllAction();
                myBoard.lbxCommands.Items.Add("*Canceled*");
                myBoard.lbxCommands.SelectedIndex =
                    myBoard.lbxCommands.Items.Count - 1;
            }
        }

        /// <summary>
        /// This method checks if enter key pressed and interprete the
        /// commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDinamic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SilenceKeySound(e);

                // Draw line by commands
                if (DrawLine && !FirstClick)
                {
                    string[] parts = tbDinamic.Text.Split(';');

                    // Interpretation of commands
                    if (AddLine(parts))
                    {
                        myBoard.lbxCommands.Items.Add("Command: " + tbDinamic.Text);
                        myBoard.lbxCommands.BackColor = Color.White;
                        myBoard.lbxCommands.SelectedIndex = 
                            myBoard.lbxCommands.Items.Count - 1;
                        tbDinamic.Clear();
                        tbDinamic.BackColor = Color.White;
                    }
                    else
                    {
                        tbDinamic.BackColor = Color.Red;
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                ResetAllAction();
                SilenceKeySound(e);
                myBoard.lbxCommands.Items.Add("*Canceled*");
                myBoard.lbxCommands.SelectedIndex =
                    myBoard.lbxCommands.Items.Count - 1;
            }
        }

// Own methods
        /// <summary>
        /// This method resets all action buttons
        /// </summary>
        public void ResetAllAction()
        {
            SelectEntity = true;
            FirstClick = true;
            SecondClick = false;
            ThirdClick = false;
            Draw = false;
            DrawLine = false;
            DrawPolyline = false;
            DrawRectAngle = false;
            DrawPolygon = false;
            DrawText = false;
            DrawPoint = false;
            DrawCircle = false;
            DrawCircleOpposite = false;
            DrawEllipse = false;
            DrawArc = false;
            DrawImage = false;
            ScaleEntity = false;
            Mirror = false;
            Copy = false;
            MoveEntity = false;
            myBoard.lbAction.Text = "Select";
            lbDinamic.Hide();
            tbDinamic.Hide();
        }

        /// <summary>
        /// This method silences the key sound
        /// </summary>
        public void SilenceKeySound(KeyEventArgs e)
        {
            // Silence the windows "ding" sound
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// This method returns all existing Drawings
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetDrawing()
        {
            return entities;
        }

        /// <summary>
        /// This method loads entities from the main board
        /// </summary>
        /// <param name="entities"></param>
        public void LoadDrawing(List<Entity> entities)
        {
            this.entities = entities;
            Refresh();
        }

        /// <summary>
        /// This method resets all elements of canvas
        /// </summary>
        public void ResetCanvas()
        {
            entities = new List<Entity>();
            ResetAllAction();
            myBoard.lbxCommands.Items.Add("Welcome to CCAD version 1.0 2016");
            myBoard.lbxCommands.Items.Add("Author: Chen Chao");
            Refresh();
        }

        /// <summary>
        /// This method sets the start point of an entity
        /// </summary>
        /// <param name="point"></param>
        public void SetStartPoint(PointF point)
        {
            startPoint = point;
        }

// Add entity to array
        /// <summary>
        /// This method adds a line in the entity array
        /// </summary>
        /// <param name="parts"></param>
        /// <returns>true if line added successfuly and
        /// false if not</returns>
        public bool AddLine(string[] parts)
        {
            if (parts.Length == 2)
            {
                try
                {
                    // Draw line given relative position x and y
                    // @x;y
                    if (parts[0].StartsWith("@"))
                    {
                        endPoint.X = Math.Abs(Convert.ToInt32(
                            parts[0].Substring(1)) + startPoint.X);
                        endPoint.Y = Math.Abs(Convert.ToInt32(
                            parts[1]) + startPoint.Y);
                        Line tempLine = new Line(CurrentColor,
                            CurrentLineWidth, startPoint, endPoint);
                        startPoint = endPoint;
                        entities.Add(tempLine);
                        Refresh();
                    }
                    // Draw line given relative Angle and length of line
                    // #Angle;length
                    else if (parts[0].StartsWith("#"))
                    {
                        Angle = -Convert.ToDouble(
                            parts[0].Substring(1)) * Math.PI / 180;
                        double tempLength = Convert.ToDouble(parts[1]);
                        endPoint.X = (float)(Math.Cos(Angle) * tempLength)
                            + startPoint.X;
                        endPoint.Y = (float)(Math.Sin(Angle) * tempLength)
                            + startPoint.Y;
                        Line tempLine = new Line(CurrentColor,
                            CurrentLineWidth, startPoint, endPoint);
                        startPoint = endPoint;
                        entities.Add(tempLine);
                        Refresh();
                    }
                    // Draw line given absolute position of x and y
                    // x;y
                    else
                    {
                        endPoint.X = Convert.ToInt32(parts[0]);
                        endPoint.Y = Convert.ToInt32(parts[1]);
                        Line tempLine = new Line(CurrentColor,
                            CurrentLineWidth, startPoint, endPoint);
                        startPoint = endPoint;
                        entities.Add(tempLine);
                        Refresh();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

// Entity creation
        /// <summary>
        /// This method gets points to create lines
        /// </summary>
        public void CreateLine()
        {
            // Define the first point
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
            }
            // Define the following points
            else if (!FirstClick)
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
                Line tempLine = new Line(CurrentColor, CurrentLineWidth,
                    startPoint, endPoint);
                entities.Add(tempLine);
                Refresh();

                // Set the next line's start point
                startPoint = endPoint;
            }
        }
    }
}
