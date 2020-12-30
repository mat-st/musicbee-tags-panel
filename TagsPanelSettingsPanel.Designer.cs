
namespace MusicBeePlugin
{
    partial class TagsPanelSettingsPanel
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
            this.btnImportCSV = new System.Windows.Forms.Button();
            this.lstOccasions = new System.Windows.Forms.ListBox();
            this.txtOccasionInput = new System.Windows.Forms.TextBox();
            this.btnRemMood = new System.Windows.Forms.Button();
            this.btnAddMood = new System.Windows.Forms.Button();
            this.cbEnableMoodSort = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Location = new System.Drawing.Point(3, 200);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(222, 23);
            this.btnImportCSV.TabIndex = 11;
            this.btnImportCSV.Text = "Import Occasions (CSV)";
            this.btnImportCSV.UseVisualStyleBackColor = true;
            // 
            // lstOccasions
            // 
            this.lstOccasions.BackColor = System.Drawing.Color.White;
            this.lstOccasions.FormattingEnabled = true;
            this.lstOccasions.Location = new System.Drawing.Point(3, 3);
            this.lstOccasions.Name = "lstOccasions";
            this.lstOccasions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstOccasions.Size = new System.Drawing.Size(222, 95);
            this.lstOccasions.Sorted = true;
            this.lstOccasions.TabIndex = 10;
            // 
            // txtOccasionInput
            // 
            this.txtOccasionInput.Location = new System.Drawing.Point(3, 104);
            this.txtOccasionInput.Name = "txtOccasionInput";
            this.txtOccasionInput.Size = new System.Drawing.Size(222, 20);
            this.txtOccasionInput.TabIndex = 9;
            // 
            // btnRemMood
            // 
            this.btnRemMood.Location = new System.Drawing.Point(124, 130);
            this.btnRemMood.Name = "btnRemMood";
            this.btnRemMood.Size = new System.Drawing.Size(101, 41);
            this.btnRemMood.TabIndex = 8;
            this.btnRemMood.Text = "Remove Occasion";
            this.btnRemMood.UseVisualStyleBackColor = true;
            // 
            // btnAddMood
            // 
            this.btnAddMood.Location = new System.Drawing.Point(3, 130);
            this.btnAddMood.Name = "btnAddMood";
            this.btnAddMood.Size = new System.Drawing.Size(115, 41);
            this.btnAddMood.TabIndex = 7;
            this.btnAddMood.Text = "Add Occasion";
            this.btnAddMood.UseVisualStyleBackColor = true;
            // 
            // cbEnableMoodSort
            // 
            this.cbEnableMoodSort.AutoSize = true;
            this.cbEnableMoodSort.Checked = true;
            this.cbEnableMoodSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableMoodSort.Location = new System.Drawing.Point(3, 177);
            this.cbEnableMoodSort.Name = "cbEnableMoodSort";
            this.cbEnableMoodSort.Size = new System.Drawing.Size(146, 17);
            this.cbEnableMoodSort.TabIndex = 6;
            this.cbEnableMoodSort.Text = "Sort moods alphabetically";
            this.cbEnableMoodSort.UseVisualStyleBackColor = true;
            // 
            // TagsPanelSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.lstOccasions);
            this.Controls.Add(this.txtOccasionInput);
            this.Controls.Add(this.btnRemMood);
            this.Controls.Add(this.btnAddMood);
            this.Controls.Add(this.cbEnableMoodSort);
            this.Name = "TagsPanelSettingsPanel";
            this.Size = new System.Drawing.Size(232, 231);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportCSV;
        private System.Windows.Forms.ListBox lstOccasions;
        private System.Windows.Forms.TextBox txtOccasionInput;
        private System.Windows.Forms.Button btnRemMood;
        private System.Windows.Forms.Button btnAddMood;
        private System.Windows.Forms.CheckBox cbEnableMoodSort;
    }
}
