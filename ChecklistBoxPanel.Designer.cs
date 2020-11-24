namespace MusicBeePlugin
{
    partial class ChecklistBoxPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Window;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkedListBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.ForeColor = System.Drawing.Color.Red;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Abgehen",
            "Berghainstyle",
            "Cheesy",
            "Gangsta",
            "Geheimnisvoll",
            "Melancholisch",
            "Nostalgisch"});
            this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(545, 334);
            this.checkedListBox1.TabIndex = 1;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // ChecklistBoxPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.checkedListBox1);
            this.Name = "ChecklistBoxPanel";
            this.Size = new System.Drawing.Size(545, 337);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
    }
}
