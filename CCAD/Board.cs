using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CCAD
{
    /// <summary>
    /// Main window of the program where will be all drawings
    /// </summary>
    public partial class Board : Form
    {
        private List<Entity> entities;
        private bool firstClick;
        private bool secondClick;
        private bool thirdClick;
        private bool draw;
        private bool drawLine;
        private bool drawPolyline;
        private bool drawRectangle;
        private bool drawPolygon;
        private bool drawText;
        private bool drawPoint;
        private bool drawCircle;
        private bool drawCircleOpposite;
        private bool drawEllipse;
        private bool drawArc;
        private bool drawImage;
        private bool scale;
        private bool mirror;
        private bool copy;
        private bool move;
        private bool select;
        private bool orto;
        private int currentX;
        private int currentY;
        private int currentLineWidth;
        private double zoomIncrement;
        private double currentZoom;
        private double angle;
        private PointF startPoint;
        private PointF endPoint;
        private PointF previousPoint;
        private Color currentColor;
        private Graphics graph;
        private Brush brush;
        private Pen pen;

        public Board()
        {
            InitializeComponent();
            lbxCommands.Items.Add("Welcome to CCAD version 1.0 2016");
            lbxCommands.Items.Add("Author: Chen Chao");
            entities = new List<Entity>();
            firstClick = true;
            secondClick = false;
            thirdClick = false;
            draw = false;
            drawLine = false;
            drawPolyline = false;
            drawRectangle = false;
            drawPolygon = false;
            drawText = false;
            drawPoint = false;
            drawCircle = false;
            drawCircleOpposite = false;
            drawEllipse = false;
            drawArc = false;
            drawImage = false;
            scale = false;
            mirror = false;
            copy = false;
            move = false;
            select = false;
            orto = false;
            zoomIncrement = 0.1f;
            currentZoom = 1f;
            angle = 0;
            graph = pBoard.CreateGraphics();
            brush = new SolidBrush(currentColor);
            pen = new Pen(brush, currentLineWidth);
            startPoint = new PointF();
            endPoint = new PointF();
            previousPoint = new PointF();
            cbColorSelector.Items.Add("White");
            cbColorSelector.Items.Add("Green");
            cbColorSelector.Items.Add("Blue");
            cbColorSelector.Items.Add("Red");
            cbColorSelector.Items.Add("Yellow");
            cbColorSelector.Items.Add("Magenta");
            cbColorSelector.Items.Add("Orange");
            cbColorSelector.Items.Add("Cyan");
            cbColorSelector.Items.Add("Gray");
            cbColorSelector.Items.Add("Brown");
            cbColor.Items.Add("White");
            cbColor.Items.Add("Green");
            cbColor.Items.Add("Blue");
            cbColor.Items.Add("Red");
            cbColor.Items.Add("Yellow");
            cbColor.Items.Add("Magenta");
            cbColor.Items.Add("Orange");
            cbColor.Items.Add("Cyan");
            cbColor.Items.Add("Gray");
            cbColor.Items.Add("Brown");
            cbLineWidth.Items.Add(1);
            cbLineWidth.Items.Add(5);
            cbLineWidth.Items.Add(9);
            cbCurrentLineWidth.Items.Add(1);
            cbCurrentLineWidth.Items.Add(5);
            cbCurrentLineWidth.Items.Add(9);
            currentLineWidth = Convert.ToInt32(cbCurrentLineWidth.Items[0]);
            currentColor = Color.FromName(
                cbColorSelector.Items[0].ToString());
            cbColorSelector.SelectedIndex = 0;
            cbCurrentLineWidth.SelectedIndex = 0;
            lbDinamic.Hide();
            tbDinamic.Hide();
        }

        /// <summary>
        /// This method opens the help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbHelp_Click(object sender, EventArgs e)
        {
            HelpScreen myHelpScreen = new HelpScreen();
            myHelpScreen.Show();
        }

        /// <summary>
        /// This method closes the program but ask for save file if
        /// the array of entities is not empty before close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            if (entities.Count == 0)
                Close();
            else
            {
                // TO DO                
            }
        }

// New project block
        /// <summary>
        /// This method creates a new project, cleaning the previous drawing
        /// and clear the entity array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNew_Click(object sender, EventArgs e)
        {
            // Clear the screen
            if (entities.Count == 0)
            {
                pBoard.BackColor = Color.Black;
                Refresh();
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "Hay cambios sin guardar, ¿Desea guardarlos?", 
                    "¡Advertencia!", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    btSave_Click(sender, e);
                }
                else if (result == DialogResult.No)
                {
                    entities = new List<Entity>();
                    pBoard.BackColor = Color.Black;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// This method creates a new project, cleaning the previous drawing
        /// and clear the entity array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNew2_Click(object sender, EventArgs e)
        {
            btNew_Click(sender, e);
        }

// Open file Block
        /// <summary>
        /// This method loads a selected file from a folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOpen_Click(object sender, EventArgs e)
        {
            if (inFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // TO DO
                }
                catch (PathTooLongException)
                {
                    MessageBox.Show("Path too long.");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Input error: Cound not read file from disk. Original error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// This method loads a selected file from a folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOpen2_Click(object sender, EventArgs e)
        {
            btOpen_Click(sender, e);
        }

// Save file block
        /// <summary>
        /// This method saves all entities in a text file
        /// </summary>
        /// <param name="path"></param>
        private void Save(string path)
        {
            try
            {
                // TO DO
                // Save method, all code here
            }
            catch (PathTooLongException)
            {
                MessageBox.Show("Path too long.");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Output error: Cound not save file in disk." +
                    " Original error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message);
            }
        }

        /// <summary>
        /// This method saves the current file in an already opened file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            // TO DO
        }

        /// <summary>
        /// This method saves the current file in an already opened file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave2_Click(object sender, EventArgs e)
        {
            // Call the following method to do the same action
            btSave_Click(sender, e);
        }

        /// <summary>
        /// This method saves the file in the selected folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveAs_Click(object sender, EventArgs e)
        {
            if (outFile.ShowDialog() == DialogResult.OK)
            {
                // TO DO
            }
        }

        /// <summary>
        /// This method saves the file in the selected folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveAs2_Click(object sender, EventArgs e)
        {
            // Call the following method to do the same action
            btSaveAs_Click(sender, e);
        }

// Color selector block
        /// <summary>
        /// This method sets the current color to the select item in the color box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbColorSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentColor = Color.FromName(cbColorSelector.SelectedItem.ToString());
        }
        
        /// <summary>
        /// This method draws rectangles filled with the corresponding colour of each
        /// item in the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbColorSelector_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawFocusRectangle();
            Graphics g = e.Graphics;
            System.Drawing.Rectangle rect = e.Bounds;
            if (e.Index >= 0)
            {
                string name = ((ComboBox)sender).Items[e.Index].ToString();
                Font font = new Font("Arial", 9, FontStyle.Regular);
                Color color = Color.FromName(name);
                Brush tempBrush = new SolidBrush(color);
                g.DrawString(name, font, Brushes.Black, rect.X, rect.Top);
                // Draw background colour
                g.FillRectangle(tempBrush, rect.X + 60, rect.Y + 2, rect.Width - 65,
                    rect.Height - 3);
                // Draw perimeter
                e.Graphics.DrawRectangle(SystemPens.WindowText,
                    new System.Drawing.Rectangle(rect.X + 60, rect.Y + 2, rect.Width - 65,
                    rect.Height - 3));
            }
        }

        /// <summary>
        /// This method sets the current line width to the select item in the
        /// line width box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCurrentLineWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentLineWidth = Convert.ToInt32(cbCurrentLineWidth.SelectedItem);
        }

        /// <summary>
        /// This method draws rectangles filled with the corresponding colour of each
        /// item in the combo box of the property editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            System.Drawing.Rectangle rect = e.Bounds;
            if (e.Index >= 0)
            {
                string name = ((ComboBox)sender).Items[e.Index].ToString();
                Font font = new Font("Arial", 5, FontStyle.Regular);
                Color color = Color.FromName(name);
                Brush tempBrush = new SolidBrush(color);
                g.DrawString(name, font, Brushes.Black, rect.X, rect.Top);
                // Draw background colour
                g.FillRectangle(tempBrush, rect.X + 60, rect.Y + 2, rect.Width - 65,
                    rect.Height - 3);
                // Draw perimeter
                e.Graphics.DrawRectangle(SystemPens.WindowText,
                    new System.Drawing.Rectangle(rect.X + 60, rect.Y + 2, rect.Width - 65,
                    rect.Height - 3));
            }
        }

// Main board block
        /// <summary>
        /// Main board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBoard_Paint(object sender, PaintEventArgs e)
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
        private void pBoard_MouseClick(object sender, MouseEventArgs e)
        {
            // Get line points if drawing line
            if (drawLine)
                CreateLine();
        }

        /// <summary>
        /// This method displays the current x and y of the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBoard_MouseMove(object sender, MouseEventArgs e)
        {
            lbMouseX.Text = e.X.ToString("0.0000");
            lbMouseY.Text = e.Y.ToString("0.0000");

            // Show the mouse current coordinates in the label
            if (orto && !firstClick)
            {
                if (Math.Abs(e.X - startPoint.X) > Math.Abs(e.Y - startPoint.Y))
                {
                    currentX = e.X;
                    currentY = (int)startPoint.Y;
                }
                else
                {
                    currentY = e.Y;
                    currentX = (int)startPoint.X;
                }
            }
            else
            {
                currentX = e.X;
                currentY = e.Y;
            }

            if (draw)
            {
                // Show dinamic items and set their coordinates
                tbDinamic.Show();
                tbDinamic.Location = new System.Drawing.Point(e.X + 10, e.Y + 10);
                tbDinamic.Focus();
                lbDinamic.Show();
                lbDinamic.Location = new System.Drawing.Point(e.X + 10, e.Y - 20);

                // set dinamic label text to mouse coordinates
                if (draw)
                {
                    lbDinamic.Text = "x: " + e.X.ToString("0.0000") + " y: " + 
                        e.Y.ToString("0.0000");
                }
            }
        }

        /// <summary>
        /// This method checks if mouse leave the main board
        /// and hide dinamic items when is true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBoard_MouseLeave(object sender, EventArgs e)
        {
            // Hide dinamic items when mouse leave the main board
            tbDinamic.Hide();
            lbDinamic.Hide();
        }

// Action buttons events
        /// <summary>
        /// This method activate and deactivate the orto option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrto_Click(object sender, EventArgs e)
        {
            orto = !orto;
            if (orto)
                lbOrto.Text = "Activated";
            else
                lbOrto.Text = "Deactivated";
        }

        /// <summary>
        /// This method activate the select tool and deactivate other tools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSelect_Click(object sender, EventArgs e)
        {
            ResetAllAction();
        }
        
        /// <summary>
        /// This method activates the line action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLine_Click(object sender, EventArgs e)
        {
            ResetAllAction();
            select = false;
            draw = true;
            drawLine = true;
            lbAction.Text = "Line";
        }

// Keyboard event
        /// <summary>
        /// This method checks the pressed key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBoard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && tbCommands.Focused)
            {
                // Reset all actions when key escape pressed
                ResetAllAction();
                lbxCommands.Items.Add("*Canceled*");
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
                if (drawLine && !firstClick)
                {
                    string[] parts = tbDinamic.Text.Split(';');

                    // Interpretation of commands
                    if (AddLine(parts))
                    {
                        lbxCommands.Items.Add("Command: " + tbDinamic.Text);
                        lbxCommands.BackColor = Color.White;
                        lbxCommands.SelectedIndex = lbxCommands.Items.Count - 1;
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
                lbxCommands.Items.Add("*Canceled*");
            }
        }

        /// <summary>
        /// This method checks if enter key pressed and interprete the 
        /// commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCommands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SilenceKeySound(e);

                // TO DO
                if (drawLine && !firstClick)
                {
                    string[] parts = tbCommands.Text.Split(';');

                    // Interpretation of commands
                    if (AddLine(parts))
                    {
                        lbxCommands.Items.Add("Command: " + tbCommands.Text);
                        lbxCommands.BackColor = Color.White;
                        lbxCommands.SelectedIndex = lbxCommands.Items.Count - 1;
                        tbCommands.Clear();
                        tbCommands.BackColor = Color.White;
                        firstClick = true;
                    }
                    else
                    {
                        tbCommands.BackColor = Color.Red;
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                ResetAllAction();
                SilenceKeySound(e);
                lbxCommands.Items.Add("*Canceled*");
            }
        }

        /// <summary>
        /// This method checks if key pressed when focus on board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SilenceKeySound(e);
                // Reset all actions when key escape pressed
                ResetAllAction();
            }
        }

// Own methods
        /// <summary>
        /// This method resets all action buttons
        /// </summary>
        private void ResetAllAction()
        {
            select = true;
            firstClick = true;
            secondClick = false;
            thirdClick = false;
            draw = false;
            drawLine = false;
            drawPolyline = false;
            drawRectangle = false;
            drawPolygon = false;
            drawText = false;
            drawPoint = false;
            drawCircle = false;
            drawCircleOpposite = false;
            drawEllipse = false;
            drawArc = false;
            drawImage = false;
            scale = false;
            mirror = false;
            copy = false;
            move = false;
            lbAction.Text = "Select";
            lbDinamic.Hide();
            tbDinamic.Hide();
        }

        /// <summary>
        /// This method adds a line in the entity array
        /// </summary>
        /// <param name="parts"></param>
        /// <returns>true if line added successfuly and
        /// false if not</returns>
        private bool AddLine(string[] parts)
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
                        Line tempLine = new Line(currentColor,
                            currentLineWidth, startPoint, endPoint);
                        entities.Add(tempLine);
                        firstClick = true;
                        pBoard.Refresh();
                    }
                    // Draw line given relative angle and length of line
                    // #angle;length
                    else if (parts[0].StartsWith("#"))
                    {
                        angle = -Convert.ToDouble(
                            parts[0].Substring(1)) * Math.PI / 180;
                        double tempLength = Convert.ToDouble(parts[1]);
                        endPoint.X = (float)(Math.Cos(angle) * tempLength)
                            + startPoint.X;
                        endPoint.Y = (float)(Math.Sin(angle) * tempLength)
                            + startPoint.Y;
                        Line tempLine = new Line(currentColor,
                            currentLineWidth, startPoint, endPoint);
                        entities.Add(tempLine);
                        firstClick = true;
                        pBoard.Refresh();
                    }
                    // Draw line given absolute position of x and y
                    // x;y
                    else
                    {
                        endPoint.X = Convert.ToInt32(parts[0]);
                        endPoint.Y = Convert.ToInt32(parts[1]);
                        Line tempLine = new Line(currentColor,
                            currentLineWidth, startPoint, endPoint);
                        entities.Add(tempLine);
                        firstClick = true;
                        pBoard.Refresh();
                    }

                    firstClick = true;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// This method silences the key sound
        /// </summary>
        private void SilenceKeySound(KeyEventArgs e)
        {
            // Silence the windows "ding" sound
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// This method gets points to create lines
        /// </summary>
        public void CreateLine()
        {
            // Define the first point
            if (firstClick)
            {
                startPoint.X = currentX;
                startPoint.Y = currentY;
                firstClick = false;
            }
            // Define the following points
            else if (!firstClick)
            {
                endPoint.X = currentX;
                endPoint.Y = currentY;
                Line tempLine = new Line(currentColor, currentLineWidth,
                    startPoint, endPoint);
                entities.Add(tempLine);
                pBoard.Refresh();

                // Set the next line's start point
                startPoint = endPoint;
            }
        }
    }
}
