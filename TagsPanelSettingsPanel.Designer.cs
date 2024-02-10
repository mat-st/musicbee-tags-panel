
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
            this.lstTags = new System.Windows.Forms.ListBox();
            this.buttonRemTag = new System.Windows.Forms.Button();
            this.buttonAddTag = new System.Windows.Forms.Button();
            this.checkboxEnableAlphabeticalTagSorting = new System.Windows.Forms.CheckBox();
            this.buttonClearTagList = new System.Windows.Forms.Button();
            this.buttonExportTagsToCSV = new System.Windows.Forms.Button();
            this.TxtNewTagInput = new System.Windows.Forms.TextBox();
            this.buttonMoveTagUp = new System.Windows.Forms.Button();
            this.buttonMoveTagDown = new System.Windows.Forms.Button();
            this.buttonImportTagsFromCSV = new System.Windows.Forms.Button();
            this.cbShowTagsThatAreNotInTheList = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lstTags
            // 
            this.lstTags.BackColor = System.Drawing.Color.White;
            this.lstTags.FormattingEnabled = true;
            this.lstTags.ItemHeight = 19;
            this.lstTags.Location = new System.Drawing.Point(6, 6);
            this.lstTags.Name = "lstTags";
            this.lstTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTags.Size = new System.Drawing.Size(388, 460);
            this.lstTags.TabIndex = 7;
            // 
            // buttonRemTag
            // 
            this.buttonRemTag.Location = new System.Drawing.Point(201, 504);
            this.buttonRemTag.Name = "buttonRemTag";
            this.buttonRemTag.Size = new System.Drawing.Size(111, 33);
            this.buttonRemTag.TabIndex = 2;
            this.buttonRemTag.Text = "Remove";
            this.buttonRemTag.UseVisualStyleBackColor = true;
            this.buttonRemTag.Click += new System.EventHandler(this.BtnRemTag_Click);
            // 
            // buttonAddTag
            // 
            this.buttonAddTag.Location = new System.Drawing.Point(62, 504);
            this.buttonAddTag.Name = "buttonAddTag";
            this.buttonAddTag.Size = new System.Drawing.Size(111, 33);
            this.buttonAddTag.TabIndex = 1;
            this.buttonAddTag.Text = "Add";
            this.buttonAddTag.UseVisualStyleBackColor = true;
            this.buttonAddTag.Click += new System.EventHandler(this.BtnAddTag_Click);
            // 
            // checkboxEnableAlphabeticalTagSorting
            // 
            this.checkboxEnableAlphabeticalTagSorting.AutoSize = true;
            this.checkboxEnableAlphabeticalTagSorting.Checked = true;
            this.checkboxEnableAlphabeticalTagSorting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxEnableAlphabeticalTagSorting.Location = new System.Drawing.Point(62, 543);
            this.checkboxEnableAlphabeticalTagSorting.Name = "checkboxEnableAlphabeticalTagSorting";
            this.checkboxEnableAlphabeticalTagSorting.Size = new System.Drawing.Size(172, 23);
            this.checkboxEnableAlphabeticalTagSorting.TabIndex = 3;
            this.checkboxEnableAlphabeticalTagSorting.Text = "Sort tags alphabetically";
            this.checkboxEnableAlphabeticalTagSorting.UseVisualStyleBackColor = true;
            // 
            // buttonClearTagList
            // 
            this.buttonClearTagList.Location = new System.Drawing.Point(62, 694);
            this.buttonClearTagList.Name = "buttonClearTagList";
            this.buttonClearTagList.Size = new System.Drawing.Size(250, 33);
            this.buttonClearTagList.TabIndex = 6;
            this.buttonClearTagList.Text = "Clear list";
            this.buttonClearTagList.UseVisualStyleBackColor = true;
            this.buttonClearTagList.Click += new System.EventHandler(this.BtnClearTagSettings_Click);
            // 
            // buttonExportTagsToCSV
            // 
            this.buttonExportTagsToCSV.Location = new System.Drawing.Point(62, 656);
            this.buttonExportTagsToCSV.Name = "buttonExportTagsToCSV";
            this.buttonExportTagsToCSV.Size = new System.Drawing.Size(250, 33);
            this.buttonExportTagsToCSV.TabIndex = 5;
            this.buttonExportTagsToCSV.Text = "Export tags to CSV";
            this.buttonExportTagsToCSV.UseVisualStyleBackColor = true;
            this.buttonExportTagsToCSV.Click += new System.EventHandler(this.BtnExportCsv_Click);
            // 
            // TxtNewTagInput
            // 
            this.TxtNewTagInput.Location = new System.Drawing.Point(6, 472);
            this.TxtNewTagInput.Name = "TxtNewTagInput";
            this.TxtNewTagInput.Size = new System.Drawing.Size(352, 26);
            this.TxtNewTagInput.TabIndex = 0;
            // 
            // buttonMoveTagUp
            // 
            this.buttonMoveTagUp.Location = new System.Drawing.Point(364, 470);
            this.buttonMoveTagUp.Name = "buttonMoveTagUp";
            this.buttonMoveTagUp.Size = new System.Drawing.Size(30, 28);
            this.buttonMoveTagUp.TabIndex = 2;
            this.buttonMoveTagUp.Text = "▲";
            this.buttonMoveTagUp.UseVisualStyleBackColor = true;
            this.buttonMoveTagUp.Click += new System.EventHandler(this.BtnMoveTagUpSettings_Click);
            // 
            // buttonMoveTagDown
            // 
            this.buttonMoveTagDown.Location = new System.Drawing.Point(364, 504);
            this.buttonMoveTagDown.Name = "buttonMoveTagDown";
            this.buttonMoveTagDown.Size = new System.Drawing.Size(30, 28);
            this.buttonMoveTagDown.TabIndex = 2;
            this.buttonMoveTagDown.Text = "▼";
            this.buttonMoveTagDown.UseVisualStyleBackColor = true;
            this.buttonMoveTagDown.Click += new System.EventHandler(this.BtnMoveTagDownSettings_Click);
            // 
            // buttonImportTagsFromCSV
            // 
            this.buttonImportTagsFromCSV.Location = new System.Drawing.Point(62, 617);
            this.buttonImportTagsFromCSV.Name = "buttonImportTagsFromCSV";
            this.buttonImportTagsFromCSV.Size = new System.Drawing.Size(250, 33);
            this.buttonImportTagsFromCSV.TabIndex = 4;
            this.buttonImportTagsFromCSV.Text = "Import tags from CSV";
            this.buttonImportTagsFromCSV.UseVisualStyleBackColor = true;
            this.buttonImportTagsFromCSV.Click += new System.EventHandler(this.BtnImportCsv_Click);
            // 
            // cbShowTagsThatAreNotInTheList
            // 
            this.cbShowTagsThatAreNotInTheList.AutoSize = true;
            this.cbShowTagsThatAreNotInTheList.Location = new System.Drawing.Point(62, 573);
            this.cbShowTagsThatAreNotInTheList.Name = "cbShowTagsThatAreNotInTheList";
            this.cbShowTagsThatAreNotInTheList.Size = new System.Drawing.Size(231, 23);
            this.cbShowTagsThatAreNotInTheList.TabIndex = 8;
            this.cbShowTagsThatAreNotInTheList.Text = "Show tags that are not in the list";
            this.cbShowTagsThatAreNotInTheList.UseVisualStyleBackColor = true;
            // 
            // TagsPanelSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.cbShowTagsThatAreNotInTheList);
            this.Controls.Add(this.TxtNewTagInput);
            this.Controls.Add(this.buttonExportTagsToCSV);
            this.Controls.Add(this.buttonClearTagList);
            this.Controls.Add(this.buttonImportTagsFromCSV);
            this.Controls.Add(this.buttonMoveTagDown);
            this.Controls.Add(this.buttonMoveTagUp);
            this.Controls.Add(this.buttonRemTag);
            this.Controls.Add(this.buttonAddTag);
            this.Controls.Add(this.checkboxEnableAlphabeticalTagSorting);
            this.Controls.Add(this.lstTags);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TagsPanelSettingsPanel";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(400, 746);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstTags;
        private System.Windows.Forms.Button buttonRemTag;
        private System.Windows.Forms.Button buttonAddTag;
        private System.Windows.Forms.CheckBox checkboxEnableAlphabeticalTagSorting;
        private System.Windows.Forms.Button buttonClearTagList;
        private System.Windows.Forms.Button buttonExportTagsToCSV;
        private System.Windows.Forms.TextBox TxtNewTagInput;
        private System.Windows.Forms.Button buttonMoveTagUp;
        private System.Windows.Forms.Button buttonMoveTagDown;
        private System.Windows.Forms.Button buttonImportTagsFromCSV;
        private System.Windows.Forms.CheckBox cbShowTagsThatAreNotInTheList;
    }
}
