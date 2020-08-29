namespace Physics_Simulator
{
    partial class Statistics
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
            this.listView = new System.Windows.Forms.ListView();
            this.ID = new System.Windows.Forms.ColumnHeader();
            this.Type = new System.Windows.Forms.ColumnHeader();
            this.Origin = new System.Windows.Forms.ColumnHeader();
            this.Forward = new System.Windows.Forms.ColumnHeader();
            this.Up = new System.Windows.Forms.ColumnHeader();
            this.Right = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Type,
            this.Origin,
            this.Forward,
            this.Up,
            this.Right});
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 12);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(770, 415);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 40;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 150;
            // 
            // Origin
            // 
            this.Origin.Text = "Origin";
            this.Origin.Width = 150;
            // 
            // Forward
            // 
            this.Forward.Text = "Forward";
            // 
            // Up
            // 
            this.Up.Text = "Up";
            // 
            // Right
            // 
            this.Right.Text = "Right";
            // 
            // Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listView);
            this.MaximizeBox = false;
            this.Name = "Statistics";
            this.Text = "Statistics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Statistics_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Origin;
        private System.Windows.Forms.ColumnHeader Forward;
        private System.Windows.Forms.ColumnHeader Up;
        private System.Windows.Forms.ColumnHeader Right;
        public System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader ID;
    }
}