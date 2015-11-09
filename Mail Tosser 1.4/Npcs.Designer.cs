namespace Mail_Tosser_1._4
{
    partial class Npcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Npcs));
            this.lstNpcs = new System.Windows.Forms.ListBox();
            this.btnSel = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lstNpcs
            // 
            this.lstNpcs.FormattingEnabled = true;
            this.lstNpcs.ItemHeight = 16;
            this.lstNpcs.Location = new System.Drawing.Point(12, 12);
            this.lstNpcs.Name = "lstNpcs";
            this.lstNpcs.Size = new System.Drawing.Size(297, 260);
            this.lstNpcs.TabIndex = 0;
            // 
            // btnSel
            // 
            this.btnSel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSel.Image = global::Mail_Tosser_1._4.Properties.Resources.accept;
            this.btnSel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSel.Location = new System.Drawing.Point(232, 280);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(77, 22);
            this.btnSel.TabIndex = 11;
            this.btnSel.Text = "Select";
            this.btnSel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 280);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(159, 22);
            this.txtSearch.TabIndex = 12;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSeach_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::Mail_Tosser_1._4.Properties.Resources.magnifier;
            this.pictureBox1.Location = new System.Drawing.Point(177, 284);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // Npcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 311);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSel);
            this.Controls.Add(this.lstNpcs);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Npcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search for NPCs";
            this.Load += new System.EventHandler(this.Npcs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstNpcs;
        private System.Windows.Forms.Button btnSel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.PictureBox pictureBox1;


    }
}