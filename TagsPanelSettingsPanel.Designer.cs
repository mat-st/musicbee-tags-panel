
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
            this.BtnImportCSV = new System.Windows.Forms.Button();
            this.lstTags = new System.Windows.Forms.ListBox();
            this.btnRemTag = new System.Windows.Forms.Button();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.cbEnableAlphabeticalTagSort = new System.Windows.Forms.CheckBox();
            this.btnClearTagSettings = new System.Windows.Forms.Button();
            this.BtnExportCSV = new System.Windows.Forms.Button();
            this.TxtNewTagInput = new System.Windows.Forms.TextBox();
            this.btnTagUp = new System.Windows.Forms.Button();
            this.btnTagDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnImportCSV
            // 
            this.BtnImportCSV.Location = new System.Drawing.Point(6, 201);
            this.BtnImportCSV.Name = "BtnImportCSV";
            this.BtnImportCSV.Size = new System.Drawing.Size(248, 23);
            this.BtnImportCSV.TabIndex = 4;
            this.BtnImportCSV.Text = "Import tags from CSV";
            this.BtnImportCSV.UseVisualStyleBackColor = true;
            this.BtnImportCSV.Click += new System.EventHandler(this.BtnImportCsv_Click);
            // 
            // lstTags
            // 
            this.lstTags.BackColor = System.Drawing.Color.White;
            this.lstTags.FormattingEnabled = true;
            this.lstTags.Location = new System.Drawing.Point(6, 6);
            this.lstTags.Name = "lstTags";
            this.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTags.Size = new System.Drawing.Size(212, 95);
            this.lstTags.Sorted = true;
            this.lstTags.TabIndex = 7;
            // 
            // btnRemTag
            // 
            this.btnRemTag.Location = new System.Drawing.Point(146, 133);
            this.btnRemTag.Name = "btnRemTag";
            this.btnRemTag.Size = new System.Drawing.Size(110, 41);
            this.btnRemTag.TabIndex = 2;
            this.btnRemTag.Text = "Remove";
            this.btnRemTag.UseVisualStyleBackColor = true;
            this.btnRemTag.Click += new System.EventHandler(this.BtnRemTag_Click);
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(6, 133);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(110, 41);
            this.btnAddTag.TabIndex = 1;
            this.btnAddTag.Text = "Add";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.BtnAddTag_Click);
            // 
            // cbEnableAlphabeticalTagSort
            // 
            this.cbEnableAlphabeticalTagSort.AutoSize = true;
            this.cbEnableAlphabeticalTagSort.Checked = true;
            this.cbEnableAlphabeticalTagSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableAlphabeticalTagSort.Location = new System.Drawing.Point(6, 179);
            this.cbEnableAlphabeticalTagSort.Name = "cbEnableAlphabeticalTagSort";
            this.cbEnableAlphabeticalTagSort.Size = new System.Drawing.Size(146, 17);
            this.cbEnableAlphabeticalTagSort.TabIndex = 3;
            this.cbEnableAlphabeticalTagSort.Text = "Sort tags alphabetically";
            this.cbEnableAlphabeticalTagSort.UseVisualStyleBackColor = true;
            // 
            // btnClearTagSettings
            // 
            this.btnClearTagSettings.Location = new System.Drawing.Point(5, 257);
            this.btnClearTagSettings.Name = "btnClearTagSettings";
            this.btnClearTagSettings.Size = new System.Drawing.Size(248, 23);
            this.btnClearTagSettings.TabIndex = 6;
            this.btnClearTagSettings.Text = "Clear list";
            this.btnClearTagSettings.UseVisualStyleBackColor = true;
            this.btnClearTagSettings.Click += new System.EventHandler(this.BtnClearTagSettings_Click);
            // 
            // BtnExportCSV
            // 
            this.BtnExportCSV.Location = new System.Drawing.Point(5, 229);
            this.BtnExportCSV.Name = "BtnExportCSV";
            this.BtnExportCSV.Size = new System.Drawing.Size(248, 23);
            this.BtnExportCSV.TabIndex = 5;
            this.BtnExportCSV.Text = "Export tags to CSV";
            this.BtnExportCSV.UseVisualStyleBackColor = true;
            this.BtnExportCSV.Click += new System.EventHandler(this.BtnExportCsv_Click);
            // 
            // TxtNewTagInput
            // 
            this.TxtNewTagInput.Location = new System.Drawing.Point(6, 106);
            this.TxtNewTagInput.Name = "TxtNewTagInput";
            this.TxtNewTagInput.Size = new System.Drawing.Size(212, 22);
            this.TxtNewTagInput.TabIndex = 0;
            // 
            // btnTagUp
            // 
            this.btnTagUp.Location = new System.Drawing.Point(224, 39);
            this.btnTagUp.Name = "btnTagUp";
            this.btnTagUp.Size = new System.Drawing.Size(30, 28);
            this.btnTagUp.TabIndex = 2;
            this.btnTagUp.Text = "▲";
            this.btnTagUp.UseVisualStyleBackColor = true;
            this.btnTagUp.Click += new System.EventHandler(this.BtnMoveTagUpSettings_Click);
            // 
            // btnTagDown
            // 
            this.btnTagDown.Location = new System.Drawing.Point(224, 73);
            this.btnTagDown.Name = "btnTagDown";
            this.btnTagDown.Size = new System.Drawing.Size(30, 28);
            this.btnTagDown.TabIndex = 2;
            this.btnTagDown.Text = "▼";
            this.btnTagDown.UseVisualStyleBackColor = true;
            this.btnTagDown.Click += new System.EventHandler(this.BtnMoveTagDownSettings_Click);
            // 
            // TagsPanelSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.TxtNewTagInput);
            this.Controls.Add(this.BtnExportCSV);
            this.Controls.Add(this.btnClearTagSettings);
            this.Controls.Add(this.BtnImportCSV);
            this.Controls.Add(this.btnTagDown);
            this.Controls.Add(this.btnTagUp);
            this.Controls.Add(this.btnRemTag);
            this.Controls.Add(this.btnAddTag);
            this.Controls.Add(this.cbEnableAlphabeticalTagSort);
            this.Controls.Add(this.lstTags);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TagsPanelSettingsPanel";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(262, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnImportCSV;
        private System.Windows.Forms.ListBox lstTags;
        private System.Windows.Forms.Button btnRemTag;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.CheckBox cbEnableAlphabeticalTagSort;
        private System.Windows.Forms.Button btnClearTagSettings;
        private System.Windows.Forms.Button BtnExportCSV;
        private System.Windows.Forms.TextBox TxtNewTagInput;
        private System.Windows.Forms.Button btnTagUp;
        private System.Windows.Forms.Button btnTagDown;
    }
}
