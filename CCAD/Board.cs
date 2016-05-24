using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAD
{
    public partial class Board : Form
    {
        List<Entity> entities;

        public Board()
        {
            InitializeComponent();
            lbxCommands.Items.Add("Welcome to CCAD version 1.0 2016");
            lbxCommands.Items.Add("Author: Chen Chao");
            entities = new List<Entity>();
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
            }
            else
            {
                entities = new List<Entity>();
                pBoard.BackColor = Color.Black;
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
            Stream myStream = null;

            if (inFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = inFile.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // TO DO
                        }
                    }
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
                try
                {
                    Stream myStream = null;
                    // TO DO
                }
                catch (PathTooLongException)
                {
                    MessageBox.Show("Path too long.");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Output error: Cound not save file in disk. Original error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// This method saves the file in the selected folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveAs2_Click(object sender, EventArgs e)
        {
            btSaveAs_Click(sender, e);
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
        }

// Main code block
        /// <summary>
        /// Main board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pBoard_Paint(object sender, PaintEventArgs e)
        {
            // TO DO
            
        }
    }
}
