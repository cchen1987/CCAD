namespace CCAD
{
    partial class Canvas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));
            this.lbDinamic = new System.Windows.Forms.Label();
            this.tbDinamic = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbDinamic
            // 
            this.lbDinamic.AutoSize = true;
            this.lbDinamic.BackColor = System.Drawing.Color.Transparent;
            this.lbDinamic.ForeColor = System.Drawing.Color.White;
            this.lbDinamic.Location = new System.Drawing.Point(163, 113);
            this.lbDinamic.Name = "lbDinamic";
            this.lbDinamic.Size = new System.Drawing.Size(43, 13);
            this.lbDinamic.TabIndex = 0;
            this.lbDinamic.Text = "dinamic";
            // 
            // tbDinamic
            // 
            this.tbDinamic.Location = new System.Drawing.Point(163, 156);
            this.tbDinamic.Name = "tbDinamic";
            this.tbDinamic.Size = new System.Drawing.Size(70, 20);
            this.tbDinamic.TabIndex = 1;
            this.tbDinamic.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDinamic_KeyDown);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(847, 560);
            this.Controls.Add(this.tbDinamic);
            this.Controls.Add(this.lbDinamic);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Canvas";
            this.Text = "Canvas";
            this.SizeChanged += new System.EventHandler(this.Canvas_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Canvas_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.MouseEnter += new System.EventHandler(this.Canvas_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Canvas_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbDinamic;
        private System.Windows.Forms.TextBox tbDinamic;
    }
}