using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CCAD
{
    public partial class Canvas : Form
    {
        private List<Entity> entities;
        private PointF startPoint;
        private PointF endPoint;
        private PointF previousPoint;
        private System.Drawing.Point ptCurrent; // area selection current point
        private System.Drawing.Point ptOriginal; // area selection start point
        private System.Drawing.Point ptLast; // area selection end point
        private bool areaSelection;
        private Graphics graph;
        private Board myBoard;
        private List<int> selections;
        private List<Line> tempPolyline;
        private float[][] dashPatterns;
        private float width;
        private float height;

        public int PolygonSides { get; set; }
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
            areaSelection = false;
            ZoomIncrement = 0.1f;
            CurrentZoom = 1f;
            Angle = 0;
            width = 0;
            height = 0;
            PolygonSides = 0;
            graph = CreateGraphics();
            ptLast = new System.Drawing.Point();
            ptOriginal = new System.Drawing.Point();
            startPoint = new PointF();
            endPoint = new PointF();
            previousPoint = new PointF();
            CurrentLineWidth = Convert.ToInt32(
                myBoard.cbCurrentLineWidth.Items[0]);
            CurrentColor = Color.FromName(
                myBoard.cbColorSelector.Items[0].ToString());
            selections = new List<int>();
            dashPatterns = new float[4][]
            {
                new float[] { 5, 5 },
                new float[] { 8, 4 },
                new float[] { 10, 10 },
                new float[] { 2, 10 }
            };
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
        /// <summary>
        /// This method draws all entities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // Draw all entities
                for (int i = 0; i < entities.Count; i++)
                    entities[i].Draw(e);
                // Draw polyline previews
                if (DrawPolyline)
                    for (int i = 0; i < tempPolyline.Count; i++)
                        tempPolyline[i].Draw(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                entities.RemoveAt(entities.Count - 1);
                Refresh();
            }
        }
        
        /// <summary>
        /// This method redefines the variable graph when canvas size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_SizeChanged(object sender, EventArgs e)
        {
            graph = CreateGraphics();
        }

// Mouse events
        /// <summary>
        /// This method create entities by mouse clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            // Makes copy of selected elements
            if (Copy && selections.Count > 0)
                CopyElements();
            else if (!Draw)
            {
                // Clear all selections
                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Free();
                    selections.Clear();
                }

                // Check if the cursor is next to the drawing when clicks
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].IsInRange(CurrentX, CurrentY))
                    {
                        entities[i].Selected();
                        selections.Add(i);
                        myBoard.SetSelection(i);
                    }
                    if (selections.Count == 1)
                    {
                        myBoard.pStraightLine.Show();
                    }
                    else
                    {
                        myBoard.pStraightLine.Hide();
                    }
                }
                Refresh();
            }

            // Create line if Drawing line
            if (DrawLine)
                CreateLine();
            // Create point if Drawing point
            else if (DrawPoint)
                CreatePoint();
            // Create arc if Drawing arc
            else if (DrawArc)
                CreateArc();
            // Create circle by radius if Drawing circle
            else if (DrawCircle)
                CreateCircle();
            // Create circle by diameter if Drawing circle by opposite points
            else if (DrawCircleOpposite)
                CreateCircleOpposite();
            // Create polyline
            else if (DrawPolyline)
                CreatePolyline();
            // Create rectangle
            else if (DrawRectAngle)
                CreateRectangle();
            // Create ellipse
            else if (DrawEllipse)
                CreateEllipse();
            // Create polygon
            else if (DrawPolygon)
                CreatePolygon();
            // Create image
            else if (DrawImage)
                CreateImage();
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
        /// This methods checks if mouse is on canvas and focus it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }

        /// <summary>
        /// This method checks if mouse is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            // Activate selection by area
            areaSelection = true;

            // Start point of the selection rectangle
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;

            // Special value lets know that no previous rectangle needs 
            // to be erases
            ptLast.X = -1;
            ptLast.Y = -1;
        }

        /// <summary>
        /// This method checks if mouse button is up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (areaSelection)
            {
                double botRectangle = ptOriginal.Y > ptCurrent.Y ? ptOriginal.Y : ptCurrent.Y;
                double topRectangle = ptOriginal.Y < ptCurrent.Y ? ptOriginal.Y : ptCurrent.Y;
                double leftRectangle = ptOriginal.X < ptCurrent.X ? ptOriginal.X : ptCurrent.X;
                double rightRectangle = ptOriginal.X > ptCurrent.X ? ptOriginal.X : ptCurrent.X;

                // Check if any entity is contained by selection rectangle
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].IsInside(topRectangle, botRectangle,
                            leftRectangle, rightRectangle))
                    {
                        entities[i].Selected();
                        selections.Add(i);
                        myBoard.SetSelection(i);
                    }
                }
                // Show property editor if just 1 entity selected
                if (selections.Count == 1)
                {
                    myBoard.pStraightLine.Show();
                }
                else
                {
                    myBoard.pStraightLine.Hide();
                }

                Refresh();
            }
            
            // Deactivate area selection
            areaSelection = false;
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

            if (!Draw && !areaSelection)
            {
                // Check if the cursor is next to the drawing
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].IsInRange(CurrentX, CurrentY) &&
                            !entities[i].IsSelected())
                    {
                        entities[i].SetTemporalColor();
                        Refresh();
                    }
                    else if (!entities[i].IsInRange(CurrentX, CurrentY) &&
                            !entities[i].IsSelected())
                    {
                        entities[i].ResetColor();
                        Refresh();
                    }
                }
            }

            // Draw selection rectangle
            ptCurrent = new System.Drawing.Point(CurrentX, CurrentY);

            if (areaSelection && !Draw)
            {
                if (ptLast.X != -1)
                {
                    DrawReversibleRectangle(ptOriginal, ptLast);
                }
                ptLast = ptCurrent;
                DrawReversibleRectangle(ptOriginal, ptCurrent);
            }

            // Draw preview of entities
            DrawPreview();

            if (Draw)
            {
                // Show dinamic items and set their coordinates
                tbDinamic.Show();
                tbDinamic.Location = new System.Drawing.Point(e.X + 10, e.Y + 10);
                tbDinamic.Focus();
                lbDinamic.Show();
                lbDinamic.Location = new System.Drawing.Point(e.X + 10, e.Y - 20);

                if (Draw && FirstClick)
                    // set dinamic label text to mouse coordinates
                    lbDinamic.Text = "x: " + e.X.ToString("0.0000") + " y: " +
                        e.Y.ToString("0.0000");
                else
                {
                    if (DrawCircleOpposite && !FirstClick)
                    {
                        double diameter = Math.Sqrt(Math.Pow(startPoint.X - 
                            CurrentX, 2) + Math.Pow(startPoint.Y - 
                            CurrentY, 2));
                        lbDinamic.Text = "Diameter: " + diameter.ToString("0.0000");
                    }
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
                myBoard.ResetSelection();
                ResetSelection();
                myBoard.lbxCommands.Items.Add("Command: *Canceled*");
                myBoard.lbxCommands.SelectedIndex =
                    myBoard.lbxCommands.Items.Count - 1;
                myBoard.HideAllPropertyPanels();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                EraseSelectedElements();
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
                // Get number of vertex and calculate polygon's vertex
                else if (DrawPolygon)
                {
                    try
                    {
                        PolygonSides = Convert.ToInt32(tbDinamic.Text);
                        if (PolygonSides >= 3)
                        {
                            CalculatePolygon();
                            tbDinamic.Clear();
                        }
                        else
                        {
                            myBoard.lbxCommands.Items.Add("Command: The number" +
                                " of sides should be equal or bigger than 3");
                            myBoard.MoveCommandBoxLines();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    
                }
            }
            // Cancel all actions
            else if (e.KeyCode == Keys.Escape)
            {
                ResetAllAction();
                ResetSelection();
                SilenceKeySound(e);
                myBoard.lbxCommands.Items.Add("Command: *Canceled*");
                myBoard.lbxCommands.SelectedIndex =
                    myBoard.lbxCommands.Items.Count - 1;
                myBoard.HideAllPropertyPanels();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                EraseSelectedElements();
            }
        }

// Own methods
        /// <summary>
        /// This method resets all action buttons
        /// </summary>
        public void ResetAllAction()
        {
            SelectEntity = true;
            areaSelection = false;
            FirstClick = true;
            SecondClick = false;
            ThirdClick = false;
            Draw = false;
            DrawLine = false;
            if (DrawPolyline)
                AddPolylineToMainList();
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
            // Deselect all entities
            for (int i = 0; i < entities.Count; i++)
                entities[i].Free();
            myBoard.ResetSelection();
            DeactivatePolylineCreation();
            lbDinamic.Hide();
            tbDinamic.Hide();
            Refresh();
        }

        /// <summary>
        /// This method clears selections list
        /// </summary>
        public void ResetSelection()
        {
            selections.Clear();
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
        /// This method erases the selected elements index
        /// </summary>
        /// <returns></returns>
        public void EraseSelectedElements()
        {
            List<Entity> tempEntities = new List<Entity>();
            // Copy all entities that should be erased
            for (int i = 0; i < selections.Count; i++)
                tempEntities.Add(entities[selections[i]]);

            // Erase the selected elements
            for (int i = 0; i < tempEntities.Count; i++)
                entities.Remove(tempEntities[i]);
            // Free memory
            tempEntities = null;
            selections.Clear();
            Refresh();
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

        /// <summary>
        /// This method draws a preview of the selected item drawing
        /// </summary>
        public void DrawPreview()
        {
            if (!FirstClick && Draw)
            {
                Refresh();
                if (!DrawLine && !DrawPoint && !DrawPolyline)
                    DrawAuxiliarLine();
                // Draw line preview
                if (DrawLine || DrawPolyline)
                {
                    graph.DrawLine(new Pen(new SolidBrush(Color.White)),
                        startPoint, new System.Drawing.Point(CurrentX,
                        CurrentY));
                }
                // Draw arc preview
                else if (DrawArc && !FirstClick && !SecondClick)
                {
                    double lengthX = endPoint.X - startPoint.X;
                    double lengthY = endPoint.Y - startPoint.Y;
                    double radius = Math.Sqrt(lengthX * lengthX + lengthY * 
                        lengthY);
                    double startAngle = (Math.Atan2(lengthY, lengthX) * 180f /
                        Math.PI);
                    lengthX = CurrentX - startPoint.X;
                    lengthY = CurrentY - startPoint.Y;
                    double endAngle = (Math.Atan2(lengthY, lengthX) * 180f /
                        Math.PI);
                    double sweepAngle = endAngle - startAngle;
                    graph.DrawArc(new Pen(new SolidBrush(Color.White)),
                        new RectangleF((float)(startPoint.X - radius),
                        (float)(startPoint.Y - radius), (float)(2 * radius),
                        (float)(2 * radius)), (float)startAngle, 
                        (float)sweepAngle);
                }
                // Draw circle preview
                else if (DrawCircle)
                {
                    double radius = Math.Sqrt(Math.Pow(startPoint.X - CurrentX,
                        2) + Math.Pow(startPoint.Y - CurrentY, 2));
                    
                    graph.DrawEllipse(new Pen(new SolidBrush(Color.White)), 
                        new RectangleF((float)(startPoint.X - radius),
                        (float)(startPoint.Y - radius), (float)(2 * radius),
                        (float)(2 * radius)));
                }
                // Draw circle by diameter preview
                else if (DrawCircleOpposite)
                {
                    double radius = Math.Sqrt(Math.Pow(startPoint.X - CurrentX,
                        2) + Math.Pow(startPoint.Y - CurrentY, 2)) / 2;
                    graph.DrawEllipse(new Pen(new SolidBrush(Color.White)),
                        new RectangleF((float)((startPoint.X + CurrentX) / 2 -
                        radius), (float)((startPoint.Y + CurrentY) / 2 - radius),
                        (float)(2 * radius), (float)(2 * radius)));
                }
                // Draw rectangle preview
                else if (DrawRectAngle)
                {
                    DrawReversibleRectangle(new System.Drawing.Point(
                        (int)startPoint.X, (int)startPoint.Y), 
                        new System.Drawing.Point(CurrentX, CurrentY));
                }
                // Draw ellipse preview
                else if (DrawEllipse)
                {
                    // Preview the ellipse with predefined width
                    if (SecondClick)
                    {
                        previousPoint.X = startPoint.X - 50;
                        previousPoint.Y = CurrentY > startPoint.Y ?
                            startPoint.Y * 2 - CurrentY : CurrentY;
                        // Predefined width
                        width = 100;
                        // Set ellipse height
                        height = Math.Abs(CurrentY - startPoint.Y) * 2;
                        graph.DrawEllipse(new Pen(new SolidBrush(Color.White)),
                            new RectangleF(previousPoint.X, previousPoint.Y, width,
                            height));
                    }
                    // Preview the ellipse with the final width
                    else if (!SecondClick && !FirstClick)
                    {
                        previousPoint.X = CurrentX > startPoint.X ?
                            CurrentX : startPoint.X * 2 - CurrentX;
                        // Set ellipse width
                        width = (startPoint.X - previousPoint.X) * 2;
                        graph.DrawEllipse(new Pen(new SolidBrush(Color.White)),
                            new RectangleF(previousPoint.X, previousPoint.Y, width,
                            height));
                    }
                }
            }
            // Preview copy
            if (Copy && selections.Count > 0)
            {
                int lastItem = selections.Count - 1;
                // Preview line copy
                if (entities[selections[lastItem]].GetType().ToString().Equals
                    ("CCAD.Line"))
                {
                    Line tempLine = (Line)entities[selections[lastItem]];
                    startPoint = new PointF(CurrentX, CurrentY);
                    endPoint = tempLine.EndPoint;
                    endPoint.X = endPoint.X + CurrentX - tempLine.StartPoint.X;
                    endPoint.Y = endPoint.Y + CurrentY - tempLine.StartPoint.Y;
                    graph.DrawLine(new Pen(new SolidBrush(Color.White)),
                        startPoint, endPoint);
                }
            }
        }

        /// <summary>
        /// This method draws an auxiliar line to help user when drawing
        /// </summary>
        public void DrawAuxiliarLine()
        {
            Pen auxPen = new Pen(new SolidBrush(Color.White));
            auxPen.DashPattern = dashPatterns[2];
            graph.DrawLine(auxPen, startPoint, new PointF(CurrentX, CurrentY));
        }
        
        /// <summary>
        /// This method draws the selection rectangle
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void DrawReversibleRectangle(System.Drawing.Point p1,
                System.Drawing.Point p2)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle();
            // Convert the points to screen coordinates
            p1 = PointToScreen(p1);
            p2 = PointToScreen(p2);
            Color back = Color.Blue;
            // Normalize the rectangle
            if (p1.X < p2.X)
            {
                rectangle.X = p1.X;
                rectangle.Width = p2.X - p1.X;
                back = Color.Blue;
            }
            else
            {
                rectangle.X = p2.X;
                rectangle.Width = p1.X - p2.X;
                back = Color.LimeGreen;
            }
            if (p1.Y < p2.Y)
            {
                rectangle.Y = p1.Y;
                rectangle.Height = p2.Y - p1.Y;
            }
            else
            {
                rectangle.Y = p2.Y;
                rectangle.Height = p1.Y - p2.Y;
            }
            
            // Draw the reversible frame
            if (!DrawRectAngle)
            {
                ControlPaint.DrawReversibleFrame(rectangle, Color.White,
                FrameStyle.Dashed);
                ControlPaint.FillReversibleRectangle(rectangle, back);
            }
            else
            {
                ControlPaint.DrawReversibleFrame(rectangle, Color.White,
                    FrameStyle.Thick);
            }
        }

        /// <summary>
        /// This method initialize the temporal polyline list
        /// </summary>
        public void ActivatePolylineCreation()
        {
            tempPolyline = new List<Line>();
        }

        /// <summary>
        /// This method clear the temporal polyline list
        /// </summary>
        public void DeactivatePolylineCreation()
        {
            tempPolyline = null;
        }

        /// <summary>
        /// This method adds all temporal lines polyline object and adds it to
        /// main list
        /// </summary>
        public void AddPolylineToMainList()
        {
            Polyline polyline = new Polyline(CurrentColor,
                tempPolyline.ToArray(), CurrentLineWidth);
            entities.Add(polyline);
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
            else
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

        /// <summary>
        /// This method gets points to create points
        /// </summary>
        public void CreatePoint()
        {
            Point point = new Point(CurrentColor, 
                new PointF(CurrentX, CurrentY));
            entities.Add(point);
            Refresh();
        }

        /// <summary>
        /// This method gets points to create arc
        /// </summary>
        public void CreateArc()
        {
            if (FirstClick)
            {
                myBoard.lbxCommands.Items.Add(
                    "Commands: Click for the start point of the arc.");
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
                SecondClick = true;
            }
            else if (SecondClick)
            {
                myBoard.lbxCommands.Items.Add(
                    "Commands: Click for the sweep angle of the arc.");
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
                SecondClick = false;
            }
            else
            {
                Arc arc = new Arc(CurrentColor, startPoint,
                    endPoint, new PointF(CurrentX, CurrentY), CurrentLineWidth);
                entities.Add(arc);
                FirstClick = true;
                Refresh();
            }
            myBoard.MoveCommandBoxLines();
        }

        /// <summary>
        /// This method gets points to create circle by radius
        /// </summary>
        public void CreateCircle()
        {
            // Define the first point
            if (FirstClick)
            {
                 startPoint.X = CurrentX;
                 startPoint.Y = CurrentY;
                 FirstClick = false;
                 SecondClick = true;
            }
            // Define the following points
            else if (SecondClick)
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
         
                double radius = Math.Sqrt(Math.Pow(startPoint.X - endPoint.X,
                    2) + Math.Pow(startPoint.Y - endPoint.Y, 2));
         
                Circle circle = new Circle(CurrentColor, startPoint,
                    CurrentLineWidth, radius);

                SecondClick = false;
                FirstClick = true;
                entities.Add(circle);
                Refresh();
            }
        }

        /// <summary>
        /// This method gets points to create circle by diameter
        /// </summary>
        public void CreateCircleOpposite()
        {
            // Define the first point
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
                SecondClick = true;
            }
            // Define the following points
            else if (SecondClick)
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;

                double radius;
                PointF centrePoint = new PointF();

                radius = Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) +
                    Math.Pow(startPoint.Y - endPoint.Y, 2)) / 2;
              
                centrePoint.X = (startPoint.X + endPoint.X) / 2;
              
                centrePoint.Y = (startPoint.Y + endPoint.Y) / 2;
              
              
                Circle circle = new Circle(CurrentColor, centrePoint,
                    CurrentLineWidth, radius);

                SecondClick = false;
                FirstClick = true;

                entities.Add(circle);
                Refresh();
            }
        }

        /// <summary>
        /// This method gets points to create a polyline
        /// </summary>
        public void CreatePolyline()
        {
            // Define the first point
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
            }
            // Define the following points
            else
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
                Line tempLine = new Line(CurrentColor, CurrentLineWidth,
                    startPoint, endPoint);
                tempPolyline.Add(tempLine);
                Refresh();

                // Set the next line's start point
                startPoint = endPoint;
            }
        }

        /// <summary>
        /// This method gets points to create a rectangle
        /// </summary>
        public void CreateRectangle()
        {
            // Define the left top corner point
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
            }
            // Define the right bottom corner point
            else
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
                Line[] tempLines = new Line[4];
                // Top line
                tempLines[0] = new Line(CurrentColor, CurrentLineWidth,
                    startPoint, new PointF(endPoint.X, startPoint.Y));
                // Right line
                tempLines[1] = new Line(CurrentColor, CurrentLineWidth,
                    new PointF(endPoint.X, startPoint.Y), endPoint);
                // Bottom line
                tempLines[2] = new Line(CurrentColor, CurrentLineWidth,
                    endPoint, new PointF(startPoint.X, endPoint.Y));
                // Left line
                tempLines[3] = new Line(CurrentColor, CurrentLineWidth,
                    new PointF(startPoint.X, endPoint.Y), startPoint);
                Rectangle tempRectangle = new Rectangle(CurrentColor,
                    tempLines, CurrentLineWidth);
                entities.Add(tempRectangle);
                Refresh();
                FirstClick = true;
            }
        }

        /// <summary>
        /// This method gets points to create an ellipse
        /// </summary>
        public void CreateEllipse()
        {
            // Define the centre point
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
                SecondClick = true;
            }
            // Define an axis point
            else if (SecondClick)
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
                SecondClick = false;
            }
            // Define the last axis point
            else
            {
                Ellipse tempEllipse = new Ellipse(CurrentColor, startPoint,
                    CurrentLineWidth, endPoint, new PointF(CurrentX, CurrentY));
                entities.Add(tempEllipse);
                Refresh();
                FirstClick = true;
            }
        }

        /// <summary>
        /// This method gets points to create a polygon
        /// </summary>
        public void CreatePolygon()
        {
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
            }
            else
            {
                endPoint.X = CurrentX;
                endPoint.Y = CurrentY;
                myBoard.lbxCommands.Items.Add(
                    "Command: Enter the number of sides of polygon");
                myBoard.MoveCommandBoxLines();
            }
        }

        /// <summary>
        /// This method calculates the polygon's vertex
        /// </summary>
        public void CalculatePolygon()
        {
            double radius = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) +
                    Math.Pow(endPoint.Y - startPoint.Y, 2));
            Angle = 2f / PolygonSides * Math.PI;
            double tempAngle = 0;
            PointF tempPoint = new PointF();
            tempPoint.X = (float)(radius * Math.Cos(tempAngle) + startPoint.X);
            tempPoint.Y = (float)(radius * Math.Sin(tempAngle) + startPoint.Y);
            Line[] tempLines = new Line[PolygonSides];
            PointF nextPoint = new PointF();

            for (int i = 0; i < PolygonSides; i++)
            {
                tempAngle += Angle;
                nextPoint.X = (float)(radius * Math.Cos(tempAngle) + startPoint.X);
                nextPoint.Y = (float)(radius * Math.Sin(tempAngle) + startPoint.Y);
                tempLines[i] = new Line(CurrentColor, CurrentLineWidth, tempPoint, nextPoint);
                tempPoint = nextPoint;
            }
            Polygon tempPolygon = new Polygon(CurrentColor, tempLines, startPoint,
                CurrentLineWidth);
            entities.Add(tempPolygon);
            Refresh();
            FirstClick = true;
        }

        /// <summary>
        /// This method gets points to create an image
        /// </summary>
        public void CreateImage()
        {
            if (FirstClick)
            {
                startPoint.X = CurrentX;
                startPoint.Y = CurrentY;
                FirstClick = false;
            }
            else
            {
                try
                {
                    string path = myBoard.GetPath();
                    if (path != null)
                    {
                        Image tempImage = new Image(CurrentColor, startPoint,
                            path, new PointF(CurrentX, CurrentY));
                        entities.Add(tempImage);
                        Refresh();
                    }
                }
                catch (PathTooLongException)
                {
                    MessageBox.Show("Path too long.");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Input error: Cound not read file" +
                        " from disk. Original error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                }
                ResetAllAction();
            }
        }

// Copy block
        /// <summary>
        /// This method makes a copy of an entity and paste it on board
        /// when mouse clicks
        /// </summary>
        public void CopyElements()
        {
            int lastItem = selections.Count - 1;
            if (entities[selections[lastItem]].GetType().ToString().Equals
                    ("CCAD.Line"))
            {
                Line tempLine = new Line(CurrentColor, CurrentLineWidth, startPoint,
                    endPoint);
                entities.Add(tempLine);
                Refresh();
            }
        }
    }
}
