﻿using System;
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

        private void tbHelp_Click(object sender, EventArgs e)
        {
            HelpScreen myHelpScreen = new HelpScreen();
            myHelpScreen.Show();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        private void Save()
        {
            // TO DO
        }

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

        private void btOpen2_Click(object sender, EventArgs e)
        {
            btOpen_Click(sender, e);
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            // TO DO
        }

        private void btSave2_Click(object sender, EventArgs e)
        {
            btSave_Click(sender, e);
        }

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

        private void btSaveAs2_Click(object sender, EventArgs e)
        {
            btSaveAs_Click(sender, e);
        }

        private void btSelect_MouseHover(object sender, EventArgs e)
        {
            //tToolTip.BackColor = Color.White;
            //tToolTip.Show("Selector tool", this, Cursor.Position);
        }

        // Show the current x and y of the mouse
        private void pBoard_MouseMove(object sender, MouseEventArgs e)
        {
            lbMouseX.Text = e.X.ToString("0.0000");
            lbMouseY.Text = e.Y.ToString("0.0000");
        }

        private void btNew_Click(object sender, EventArgs e)
        {
            // Clear the screen
            if (entities.Count == 0)
            {
                pBoard.Dispose();
            }
            else
            {
                entities = new List<Entity>();
                pBoard.Dispose();
            }
        }

        private void btNew2_Click(object sender, EventArgs e)
        {
            btNew_Click(sender, e);
        }

        private void pBoard_Paint(object sender, PaintEventArgs e)
        {
            // TO DO
        }
    }
}