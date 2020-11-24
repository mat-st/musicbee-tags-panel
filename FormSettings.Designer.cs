using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class fvSettings
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
            this.cbEnableMoodSort = new System.Windows.Forms.CheckBox();
            this.btnAddMood = new System.Windows.Forms.Button();
            this.btnRemMood = new System.Windows.Forms.Button();
            this.txtOccasionInput = new System.Windows.Forms.TextBox();
            this.lstOccasions = new System.Windows.Forms.ListBox();
            this.btnImportCSV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbEnableMoodSort
            // 
            this.cbEnableMoodSort.AutoSize = true;
            this.cbEnableMoodSort.Checked = true;
            this.cbEnableMoodSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableMoodSort.Location = new System.Drawing.Point(12, 187);
            this.cbEnableMoodSort.Name = "cbEnableMoodSort";
            this.cbEnableMoodSort.Size = new System.Drawing.Size(146, 17);
            this.cbEnableMoodSort.TabIndex = 0;
            this.cbEnableMoodSort.Text = "Sort moods alphabetically";
            this.cbEnableMoodSort.UseVisualStyleBackColor = true;
            // 
            // btnAddMood
            // 
            this.btnAddMood.Location = new System.Drawing.Point(12, 140);
            this.btnAddMood.Name = "btnAddMood";
            this.btnAddMood.Size = new System.Drawing.Size(115, 41);
            this.btnAddMood.TabIndex = 1;
            this.btnAddMood.Text = "Add Occasion";
            this.btnAddMood.UseVisualStyleBackColor = true;
            this.btnAddMood.Click += new System.EventHandler(this.btnAddMood_Click);
            // 
            // btnRemMood
            // 
            this.btnRemMood.Location = new System.Drawing.Point(133, 140);
            this.btnRemMood.Name = "btnRemMood";
            this.btnRemMood.Size = new System.Drawing.Size(101, 41);
            this.btnRemMood.TabIndex = 2;
            this.btnRemMood.Text = "Remove Occasion";
            this.btnRemMood.UseVisualStyleBackColor = true;
            this.btnRemMood.Click += new System.EventHandler(this.btnRemMood_Click);
            // 
            // txtOccasionInput
            // 
            this.txtOccasionInput.Location = new System.Drawing.Point(12, 114);
            this.txtOccasionInput.Name = "txtOccasionInput";
            this.txtOccasionInput.Size = new System.Drawing.Size(222, 20);
            this.txtOccasionInput.TabIndex = 3;
            // 
            // lstOccasions
            // 
            this.lstOccasions.BackColor = System.Drawing.Color.White;
            this.lstOccasions.FormattingEnabled = true;
            this.lstOccasions.Location = new System.Drawing.Point(12, 13);
            this.lstOccasions.Name = "lstOccasions";
            this.lstOccasions.Size = new System.Drawing.Size(222, 95);
            this.lstOccasions.Sorted = true;
            this.lstOccasions.TabIndex = 4;
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Location = new System.Drawing.Point(12, 210);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(222, 23);
            this.btnImportCSV.TabIndex = 5;
            this.btnImportCSV.Text = "Import Occasions (CSV)";
            this.btnImportCSV.UseVisualStyleBackColor = true;
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // fvSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 245);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.lstOccasions);
            this.Controls.Add(this.txtOccasionInput);
            this.Controls.Add(this.btnRemMood);
            this.Controls.Add(this.btnAddMood);
            this.Controls.Add(this.cbEnableMoodSort);
            this.Name = "fvSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "occasionTagger Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbEnableMoodSort;
        private System.Windows.Forms.Button btnAddMood;
        private System.Windows.Forms.Button btnRemMood;
        private System.Windows.Forms.TextBox txtOccasionInput;
        private System.Windows.Forms.ListBox lstOccasions;

        public string[] getOccasions()
        {
            if (this.lstOccasions.Items.Count > 0)
            {
                String[] array = new String[this.lstOccasions.Items.Count];
                this.lstOccasions.Items.CopyTo(array, 0);
                return array;
            }

            return new string[] { };
        }

        public bool isSortEnabled()
        {
            return this.cbEnableMoodSort.Checked;
        }

        public void setMoods(string[] moods)
        {
            this.lstOccasions.Items.AddRange(moods == null ? new string[] { } : moods);
        }

        public void addNewMoodToList()
        {
            string newOccasion = this.txtOccasionInput.Text;
            if (newOccasion.Trim().Length <= 0)
            {
                return;
            } 

            this.lstOccasions.BeginUpdate();
            if (!this.lstOccasions.Items.Contains(newOccasion))
            {
                this.lstOccasions.Items.Add(newOccasion);
            } else
            {
                ShowDialogForDuplicate();
            }
            this.lstOccasions.EndUpdate();

            // remove text from input field
            this.txtOccasionInput.Text = null;
        }

        private void ShowDialogForDuplicate()
        {
            MessageBox.Show("Mood is already in the list", "Duplicate found", MessageBoxButtons.OK);

        }

        public void removeSelectedItemFromList()
        {
            System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems = new System.Windows.Forms.ListBox.SelectedObjectCollection(this.lstOccasions);
            selectedItems = this.lstOccasions.SelectedItems;

            if (this.lstOccasions.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    this.lstOccasions.Items.Remove(selectedItems[i]);
            }
        }

        public void ClearMoods()
        {
            this.lstOccasions.Items.Clear();
        }

        private System.Windows.Forms.Button btnImportCSV;
    }
}