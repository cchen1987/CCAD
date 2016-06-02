using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CCAD
{
    /// <summary>
    /// Main window of the program where will be all drawings
    /// </summary>
    public partial class Board : Form
    {
        private Canvas myCanvas;
        private string currentFilePath;
        private bool isSelecting;
        private int currentSelection;

        public Board()
        {
            InitializeComponent();
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
            // Create canvas
            myCanvas = new Canvas(this);
            pBoard.Controls.Add(myCanvas);
            pBoard.Tag = myCanvas;
            myCanvas.Show();
            // Predefine item
            cbColorSelector.SelectedIndex = 0;
            cbCurrentLineWidth.SelectedIndex = 0;
            currentFilePath = "";

            currentSelection = 0;
            isSelecting = false;
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
            if (myCanvas.GetDrawing().Count == 0 && currentFilePath == "")
                Close();
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
                    Close();
                }
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
            if (myCanvas.GetDrawing().Count == 0)
            {
                currentFilePath = "";
                lbxCommands.Items.Clear();
                myCanvas.ResetCanvas();
                Text = "CCAD";
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "Hay cambios sin guardar, ¿Desea guardarlos?",
                    "¡Advertencia!", MessageBoxButtons.YesNoCancel);
                // Save all drawing if user press yes
                if (result == DialogResult.Yes)
                {
                    btSave_Click(sender, e);
                    lbxCommands.Items.Clear();
                    myCanvas.ResetCanvas();
                    currentFilePath = "";
                    Text = "CCAD";
                }
                // Delete all drawing if user press no
                else if (result == DialogResult.No)
                {
                    currentFilePath = "";
                    Text = "CCAD";
                    lbxCommands.Items.Clear();
                    myCanvas.ResetCanvas();
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
                List<Entity> entities = new List<Entity>();
                try
                {
                    currentFilePath = inFile.FileName;
                    StreamReader read = File.OpenText(currentFilePath);
                    string line;

                    // Read and check header
                    line = read.ReadLine();
                    if (!line.Equals("CCADv2016"))
                    {
                        MessageBox.Show("Not a valid file!");
                        currentFilePath = "";
                        return;
                    }

                    do
                    {
                        line = read.ReadLine();
                        if (line != null)
                        {
                            int lineWidth = Convert.ToInt32(read.ReadLine());
                            Color color = Color.FromName(read.ReadLine());
                            if (line.Equals("Point"))
                                entities.Add(new Point(color,
                                    ReadPoint(read)));
                            else if (line.Equals("Line"))
                                entities.Add(ReadLine(read, color,
                                    lineWidth));
                            else if (line.Equals("Rectangle"))
                                entities.Add(ReadRectangle(read, color,
                                    lineWidth));
                            else if (line.Equals("Polyline"))
                                entities.Add(ReadPolyline(read, color,
                                    lineWidth));
                            else if (line.Equals("Polygon"))
                                entities.Add(ReadPolygon(read, color,
                                    lineWidth));
                            else if (line.Equals("Arc"))
                                entities.Add(ReadArc(read, color,
                                    lineWidth));
                            else if (line.Equals("Circle"))
                                entities.Add(ReadCircle(read, color,
                                    lineWidth));
                            else if (line.Equals("Ellipse"))
                                entities.Add(ReadEllipse(read, color,
                                    lineWidth));
                            else if (line.Equals("Text"))
                                entities.Add(ReadText(read, color));
                            else if (line.Equals("Image"))
                                entities.Add(ReadImage(read, color));
                        }
                    }
                    while (line != null);
                    read.Close();

                    myCanvas.LoadDrawing(entities);
                }
                catch (PathTooLongException)
                {
                    MessageBox.Show("Path too long.");
                    currentFilePath = "";
                    myCanvas.ResetCanvas();
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Input error: Cound not read file" +
                        " from disk. Original error: " + ex.Message);
                    currentFilePath = "";
                    myCanvas.ResetCanvas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                    currentFilePath = "";
                    myCanvas.ResetCanvas();
                }
                entities = null;
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
                StreamWriter write = File.CreateText(path);
                // Write header
                write.WriteLine("CCADv2016");

                // Save drawings
                List<Entity> entities = myCanvas.GetDrawing();
                int size = entities.Count;
                for (int i = 0; i < size; i++)
                {
                    string name =
                        entities[i].GetType().ToString().Replace("CCAD.", "");
                    write.WriteLine(name);
                    write.WriteLine(entities[i].LineWidth);
                    write.WriteLine(entities[i].GetOriginalColor().ToString().
                        Split()[1].Replace("[", "").Replace("]", ""));
                    if (name.Equals("Point"))
                        WritePoint(write, ((Point)entities[i]).StartPoint);
                    else if (name.Equals("Line"))
                        WriteLine(write, (Line)entities[i]);
                    else if (name.Equals("Rectangle") ||
                            name.Equals("Polyline"))
                        WriteBlock(write, (Block)entities[i]);
                    else if (name.Equals("Polygon"))
                        WritePolygon(write, (Polygon)entities[i]);
                    else if (name.Equals("Arc"))
                        WriteArc(write, (Arc)entities[i]);
                    else if (name.Equals("Circle"))
                        WriteCircle(write, (Circle)entities[i]);
                    else if (name.Equals("Ellipse"))
                        WriteEllipse(write, (Ellipse)entities[i]);
                    else if (name.Equals("Text"))
                        WriteText(write, (Text)entities[i]);
                    else if (name.Equals("Image"))
                        WriteImageInfo(write, (Image)entities[i]);
                }
                write.Close();
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
            // Check for file path, if no file path, call save dialog
            if (currentFilePath == "")
            {
                btSaveAs_Click(sender, e);
            }
            else
            {
                Save(currentFilePath);
            }
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
            // Ask for file directory
            if (outFile.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = outFile.FileName;
                Text = currentFilePath + " - CCAD";
                Save(currentFilePath);
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
        /// This method sets the current color to the select item in the
        /// color box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbColorSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCanvas.CurrentColor = 
                Color.FromName(cbColorSelector.SelectedItem.ToString());
        }

        /// <summary>
        /// This method draws rectangles filled with the corresponding colour
        /// of each item in the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbColorSelector_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawItemsColor(sender, e);
        }

        /// <summary>
        /// This method sets the current line width to the select item in the
        /// line width box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCurrentLineWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            myCanvas.CurrentLineWidth = 
                Convert.ToInt32(cbCurrentLineWidth.SelectedItem);
        }

        /// <summary>
        /// This method draws rectangles filled with the corresponding 
        /// colour of each item in the combo box of the property editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawItemsColor(sender, e);
        }

// Action buttons events
        /// <summary>
        /// This method activate and deactivate the orto option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOrto_Click(object sender, EventArgs e)
        {
            myCanvas.Orto = !myCanvas.Orto;

            if (myCanvas.Orto)
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
            myCanvas.ResetAllAction();
        }

        /// <summary>
        /// This method selects drawings on board, increasing the selection index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUpArrow_Click(object sender, EventArgs e)
        {
            if (currentSelection < myCanvas.GetDrawing().Count - 1)
            {
                currentSelection++;
                isSelecting = true;
                myCanvas.GetDrawing()[currentSelection].Selected();
                myCanvas.GetDrawing()[currentSelection - 1].Free();
                myCanvas.Refresh();
            }
        }

        /// <summary>
        /// This method selects drawings on board, decreasing the selection index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDownArrow_Click(object sender, EventArgs e)
        {
            if (currentSelection > 0)
            {
                currentSelection--;
                isSelecting = true;
                myCanvas.GetDrawing()[currentSelection].Selected();
                myCanvas.GetDrawing()[currentSelection + 1].Free();
                myCanvas.Refresh();
            }
        }

        /// <summary>
        /// This method activates the draw line action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLine_Click(object sender, EventArgs e)
        {
            myCanvas.ResetAllAction();
            myCanvas.SelectEntity = false;
            myCanvas.Draw = true;
            myCanvas.DrawLine = true;
            lbAction.Text = "Line";
        }

        /// <summary>
        /// This method activates the draw arc action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btArc_Click(object sender, EventArgs e)
        {
            myCanvas.ResetAllAction();
            myCanvas.SelectEntity = false;
            myCanvas.Draw = true;
            myCanvas.DrawArc = true;
            lbAction.Text = "Arc";
        }

        /// <summary>
        /// This method activates the draw circle by diameter action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCircleDia_Click(object sender, EventArgs e)
        {
            myCanvas.ResetAllAction();
            myCanvas.SelectEntity = false;
            myCanvas.Draw = true;
            myCanvas.DrawCircleOpposite = true;
            lbAction.Text = "CircleOpposite";

        }

        /// <summary>
        /// This method activates the draw circle by radius method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCircleRad_Click(object sender, EventArgs e)
        {
            myCanvas.ResetAllAction();
            myCanvas.SelectEntity = false;
            myCanvas.Draw = true;
            myCanvas.DrawCircle = true;
            lbAction.Text = "Circle";
        }

        /// <summary>
        /// This method activates the draw point action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPoint_Click(object sender, EventArgs e)
        {
            myCanvas.ResetAllAction();
            myCanvas.SelectEntity = false;
            myCanvas.Draw = true;
            myCanvas.DrawPoint = true;
            lbAction.Text = "Point";
        }

// Keyboard event
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
                myCanvas.SilenceKeySound(e);

                if (myCanvas.DrawLine && !myCanvas.FirstClick)
                {
                    string[] parts = tbCommands.Text.Split(';');

                    // Interpretation of commands
                    if (myCanvas.AddLine(parts))
                    {
                        lbxCommands.Items.Add("Command: " + tbCommands.Text);
                        MoveCommandBoxLines();
                        tbCommands.Clear();
                        tbCommands.BackColor = Color.White;
                    }
                    else
                    {
                        tbCommands.BackColor = Color.Red;
                    }
                }
                // TO DO
            }
            else if (e.KeyCode == Keys.Escape)
            {
                myCanvas.ResetAllAction();
                myCanvas.SilenceKeySound(e);
                lbxCommands.Items.Add("*Canceled*");
                MoveCommandBoxLines();
                HideAllPropertyPanels();
                ResetSelection();
            }
        }

        /// <summary>
        /// This method checks if key pressed when focus on panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBoard_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myCanvas.ResetAllAction();
                lbxCommands.Items.Add("*Canceled*");
                MoveCommandBoxLines();
                myCanvas.ResetAllAction();
                HideAllPropertyPanels();
                ResetSelection();
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
                myCanvas.SilenceKeySound(e);
                // Reset all actions when key escape pressed
                lbxCommands.Items.Add("*Canceled*");
                MoveCommandBoxLines();
                myCanvas.ResetAllAction();
                HideAllPropertyPanels();
                ResetSelection();
            }
        }

// Mouse events
        /// <summary>
        /// This method checks if mouse is on board and focus it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }

// Own methods
        /// <summary>
        /// This method draw rectangles in the color picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawItemsColor(object sender, DrawItemEventArgs e)
        {
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
                    new System.Drawing.Rectangle(rect.X + 60, rect.Y + 2, 
                    rect.Width - 65, rect.Height - 3));
            }
        }
        
        /// <summary>
        /// This method writes the point info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="point"></param>
        private void WritePoint(StreamWriter write, PointF point)
        {
            write.WriteLine(point.X);
            write.WriteLine(point.Y);
        }

        /// <summary>
        /// This method writes the line info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="line"></param>
        private void WriteLine(StreamWriter write, Line line)
        {
            WritePoint(write, line.StartPoint);
            WritePoint(write, line.EndPoint);
        }

        /// <summary>
        /// This method writes the block info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="block"></param>
        private void WriteBlock(StreamWriter write, Block block)
        {
            Line[] lines = block.GetLines();
            write.WriteLine(lines.Length);
            for (int i = 0; i < lines.Length; i++)
                WriteLine(write, lines[i]);
        }

        /// <summary>
        /// This method writes the polygon info in file
        /// </summary>
        private void WritePolygon(StreamWriter write, Polygon polygon)
        {
            WritePoint(write, polygon.CentrePoint);
            WriteBlock(write, polygon);
        }

        /// <summary>
        /// This method writes the circle info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="circle"></param>
        private void WriteCircle(StreamWriter write, Circle circle)
        {
            WritePoint(write, circle.CentrePoint);
            write.WriteLine(circle.Radius);
        }

        /// <summary>
        /// This method writes the ellipse info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="ellipse"></param>
        private void WriteEllipse(StreamWriter write, Ellipse ellipse)
        {
            WritePoint(write, ellipse.CentrePoint);
            WritePoint(write, ellipse.MinorAxisPoint);
            WritePoint(write, ellipse.MajorAxisPoint);
        }

        /// <summary>
        /// This method writes the arc info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="arc"></param>
        private void WriteArc(StreamWriter write, Arc arc)
        {
            WritePoint(write, arc.CentrePoint);
            WritePoint(write, arc.StartPoint);
            WritePoint(write, arc.EndPoint);
        }

        /// <summary>
        /// This method writes the text info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="text"></param>
        private void WriteText(StreamWriter write, Text text)
        {
            WritePoint(write, text.Point);
            write.WriteLine(text.Phrase);
            write.WriteLine(text.FontFamily.ToString());
            write.WriteLine(text.Size);
        }

        /// <summary>
        /// This method writes the image info in file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="image"></param>
        private void WriteImageInfo(StreamWriter write, Image image)
        {
            WritePoint(write, image.StartPoint);
            write.WriteLine(image.Path);
        }

        /// <summary>
        /// This method reads float point info from file
        /// </summary>
        /// <param name="read"></param>
        /// <returns>PointF</returns>
        private PointF ReadPoint(StreamReader read)
        {
            float x = Convert.ToSingle(read.ReadLine());
            float y = Convert.ToSingle(read.ReadLine());
            return new PointF(x, y);
        }

        /// <summary>
        /// This method reads line info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Line</returns>
        private Line ReadLine(StreamReader read, Color color, int lineWidth)
        {
            PointF start = ReadPoint(read);
            PointF end = ReadPoint(read);
            return new Line(color, lineWidth, start, end);
        }

        /// <summary>
        /// This method reads rectangle info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Rectangle</returns>
        private Rectangle ReadRectangle(StreamReader read, Color color,
                int lineWidth)
        {
            int size = Convert.ToInt32(read.ReadLine());
            Line[] lines = new Line[size];
            for (int i = 0; i < size; i++)
                lines[i] = ReadLine(read, color, lineWidth);
            return new Rectangle(color, lines);
        }

        /// <summary>
        /// This method reads polygon info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Polygon</returns>
        private Polygon ReadPolygon(StreamReader read, Color color,
                int lineWidth)
        {
            PointF point = ReadPoint(read);
            int size = Convert.ToInt32(read.ReadLine());
            Line[] lines = new Line[size];
            for (int i = 0; i < size; i++)
                lines[i] = ReadLine(read, color, lineWidth);
            return new Polygon(color, lines, point);
        }

        /// <summary>
        /// This method reads polyline info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Polyline</returns>
        private Polyline ReadPolyline(StreamReader read, Color color,
                int lineWidth)
        {
            int size = Convert.ToInt32(read.ReadLine());
            Line[] lines = new Line[size];
            for (int i = 0; i < size; i++)
                lines[i] = ReadLine(read, color, lineWidth);
            return new Polyline(color, lines);
        }

        /// <summary>
        /// This method reads circle info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Circle</returns>
        private Circle ReadCircle(StreamReader read, Color color,
                int lineWidth)
        {
            PointF point = ReadPoint(read);
            int radius = Convert.ToInt32(read.ReadLine());
            return new Circle(color, point, lineWidth, radius);
        }

        /// <summary>
        /// This method reads ellipse info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Ellipse</returns>
        private Ellipse ReadEllipse(StreamReader read, Color color,
                int lineWidth)
        {
            PointF centre = ReadPoint(read);
            PointF minor = ReadPoint(read);
            PointF major = ReadPoint(read);
            return new Ellipse(color, centre, lineWidth, minor, major);
        }

        /// <summary>
        /// This method reads arc info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Arc</returns>
        private Arc ReadArc(StreamReader read, Color color,
                int lineWidth)
        {
            PointF centre = ReadPoint(read);
            PointF start = ReadPoint(read);
            PointF end = ReadPoint(read);
            return new Arc(color, centre, start, end, lineWidth);
        }

        /// <summary>
        /// This method reads text info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <returns>Text</returns>
        private Text ReadText(StreamReader read, Color color)
        {
            PointF point = ReadPoint(read);
            string text = read.ReadLine();
            string font = read.ReadLine();
            FontFamily fontFamily = 
                font.Equals("[FontFamily: Name=Courier New]") ?
                FontFamily.GenericSerif : font.Equals(
                "[FontFamily: Name=Microsoft Sans Serif]") ? 
                FontFamily.GenericSansSerif :
                FontFamily.GenericMonospace;
            int size = Convert.ToInt32(read.ReadLine());
            return new CCAD.Text(color, size, fontFamily, point, text);
        }

        /// <summary>
        /// This method reads image info from file
        /// </summary>
        /// <param name="read"></param>
        /// <param name="color"></param>
        /// <returns>Image</returns>
        private Image ReadImage(StreamReader read, Color color)
        {
            PointF point = ReadPoint(read);
            string path = read.ReadLine();
            return new Image(color, point, path);
        }

        /// <summary>
        /// This method reset selection mode
        /// </summary>
        public void ResetSelection()
        {
            isSelecting = false;
            myCanvas.Refresh();
        }

        /// <summary>
        /// This method checks if user is selecting drawings
        /// </summary>
        /// <returns>true if user is selecting, false if not</returns>
        public bool IsSelecting()
        {
            return isSelecting;
        }

        /// <summary>
        /// This method set the current selection to the entered index
        /// </summary>
        /// <param name="index"></param>
        public void SetSelection(int index)
        {
            currentSelection = index;
        }

        /// <summary>
        /// This method hides all property panels
        /// </summary>
        public void HideAllPropertyPanels()
        {
            pStraightLine.Hide();
            pRectangleProperty.Hide();
            pPolyGonProperty.Hide();
            pArcProperty.Hide();
            pCircle.Hide();
            pEllipseProperty.Hide();
            pPolylineProperty.Hide();
        }

        public void MoveCommandBoxLines()
        {
            lbxCommands.BackColor = Color.White;
            lbxCommands.SelectedIndex = lbxCommands.Items.Count - 1;
        }
    }
}