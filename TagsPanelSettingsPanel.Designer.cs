
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
            this.lstTags = new System.Windows.Forms.ListBox();
            this.btnRemTag = new System.Windows.Forms.Button();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.cbEnableAlphabeticalTagSort = new System.Windows.Forms.CheckBox();
            this.btnClearTagSettings = new System.Windows.Forms.Button();
            this.txtNewTagCueInput = new CueTextBox();
            this.SuspendLayout();
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Location = new System.Drawing.Point(3, 200);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(231, 23);
            this.btnImportCSV.TabIndex = 11;
            this.btnImportCSV.Text = "Import tags from CSV";
            this.btnImportCSV.UseVisualStyleBackColor = true;
            this.btnImportCSV.Click += new System.EventHandler(this.BtnImportCSV_Click);
            // 
            // lstTags
            // 
            this.lstTags.BackColor = System.Drawing.Color.White;
            this.lstTags.FormattingEnabled = true;
            this.lstTags.Location = new System.Drawing.Point(3, 3);
            this.lstTags.Name = "lstTags";
            this.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTags.Size = new System.Drawing.Size(231, 95);
            this.lstTags.Sorted = true;
            this.lstTags.TabIndex = 10;
            // 
            // btnRemTag
            // 
            this.btnRemTag.Location = new System.Drawing.Point(121, 130);
            this.btnRemTag.Name = "btnRemTag";
            this.btnRemTag.Size = new System.Drawing.Size(112, 41);
            this.btnRemTag.TabIndex = 8;
            this.btnRemTag.Text = "Remove";
            this.btnRemTag.UseVisualStyleBackColor = true;
            this.btnRemTag.Click += new System.EventHandler(this.BtnRemTag_Click);
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(3, 130);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(112, 41);
            this.btnAddTag.TabIndex = 7;
            this.btnAddTag.Text = "Add";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.BtnAddTag_Click);
            // 
            // cbEnableAlphabeticalTagSort
            // 
            this.cbEnableAlphabeticalTagSort.AutoSize = true;
            this.cbEnableAlphabeticalTagSort.Checked = true;
            this.cbEnableAlphabeticalTagSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableAlphabeticalTagSort.Location = new System.Drawing.Point(3, 177);
            this.cbEnableAlphabeticalTagSort.Name = "cbEnableAlphabeticalTagSort";
            this.cbEnableAlphabeticalTagSort.Size = new System.Drawing.Size(135, 17);
            this.cbEnableAlphabeticalTagSort.TabIndex = 6;
            this.cbEnableAlphabeticalTagSort.Text = "Sort tags alphabetically";
            this.cbEnableAlphabeticalTagSort.UseVisualStyleBackColor = true;
            // 
            // btnClearTagSettings
            // 
            this.btnClearTagSettings.Location = new System.Drawing.Point(3, 229);
            this.btnClearTagSettings.Name = "btnClearTagSettings";
            this.btnClearTagSettings.Size = new System.Drawing.Size(231, 23);
            this.btnClearTagSettings.TabIndex = 12;
            this.btnClearTagSettings.Text = "Clear list";
            this.btnClearTagSettings.UseVisualStyleBackColor = true;
            this.btnClearTagSettings.Click += new System.EventHandler(this.BtnClearTagSettings_Click);
            // 
            // txtNewTagCueInput
            // 
            this.txtNewTagCueInput.Cue = "Enter new tag here";
            this.txtNewTagCueInput.Location = new System.Drawing.Point(3, 104);
            this.txtNewTagCueInput.Name = "txtNewTagCueInput";
            this.txtNewTagCueInput.Size = new System.Drawing.Size(231, 20);
            this.txtNewTagCueInput.TabIndex = 13;
            // 
            // TagsPanelSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.txtNewTagCueInput);
            this.Controls.Add(this.btnClearTagSettings);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.lstTags);
            this.Controls.Add(this.btnRemTag);
            this.Controls.Add(this.btnAddTag);
            this.Controls.Add(this.cbEnableAlphabeticalTagSort);
            this.Name = "TagsPanelSettingsPanel";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(240, 258);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportCSV;
        private System.Windows.Forms.ListBox lstTags;
        private System.Windows.Forms.Button btnRemTag;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.CheckBox cbEnableAlphabeticalTagSort;
        private System.Windows.Forms.Button btnClearTagSettings;
        private CueTextBox txtNewTagCueInput;
    }
}
