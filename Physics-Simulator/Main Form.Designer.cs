namespace Physics_Simulator
{
    partial class Main_Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Menu_Strip = new System.Windows.Forms.MenuStrip();
            this.Canvas_Box = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas_Box)).BeginInit();
            this.SuspendLayout();
            // 
            // Menu_Strip
            // 
            this.Menu_Strip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Menu_Strip.Location = new System.Drawing.Point(0, 0);
            this.Menu_Strip.Name = "Menu_Strip";
            this.Menu_Strip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.Menu_Strip.Size = new System.Drawing.Size(777, 24);
            this.Menu_Strip.TabIndex = 0;
            this.Menu_Strip.Text = "Menu_Strip";
            // 
            // Canvas_Box
            // 
            this.Canvas_Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas_Box.Location = new System.Drawing.Point(0, 24);
            this.Canvas_Box.Margin = new System.Windows.Forms.Padding(2);
            this.Canvas_Box.Name = "Canvas_Box";
            this.Canvas_Box.Size = new System.Drawing.Size(777, 610);
            this.Canvas_Box.TabIndex = 1;
            this.Canvas_Box.TabStop = false;
            this.Canvas_Box.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Box_Paint);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 634);
            this.Controls.Add(this.Canvas_Box);
            this.Controls.Add(this.Menu_Strip);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main_Form";
            this.Text = "Physics Simulator";
            ((System.ComponentModel.ISupportInitialize)(this.Canvas_Box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Menu_Strip;
        private System.Windows.Forms.PictureBox Canvas_Box;
    }
}

