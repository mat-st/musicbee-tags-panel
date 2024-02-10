
namespace MusicBeePlugin
{
    partial class TabPageSelectorForm
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
            this.comboBoxTagSelect = new System.Windows.Forms.ComboBox();
            this.buttonComboBoxAddTag = new System.Windows.Forms.Button();
            this.buttonComboBoxCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxTagSelect
            // 
            this.comboBoxTagSelect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxTagSelect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxTagSelect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTagSelect.FormattingEnabled = true;
            this.comboBoxTagSelect.Location = new System.Drawing.Point(12, 12);
            this.comboBoxTagSelect.MaxDropDownItems = 12;
            this.comboBoxTagSelect.Name = "comboBoxTagSelect";
            this.comboBoxTagSelect.Size = new System.Drawing.Size(198, 27);
            this.comboBoxTagSelect.TabIndex = 0;
            this.comboBoxTagSelect.Text = "Click here";
            // 
            // buttonComboBoxAddTag
            // 
            this.buttonComboBoxAddTag.Location = new System.Drawing.Point(12, 48);
            this.buttonComboBoxAddTag.Name = "buttonComboBoxAddTag";
            this.buttonComboBoxAddTag.Size = new System.Drawing.Size(75, 33);
            this.buttonComboBoxAddTag.TabIndex = 1;
            this.buttonComboBoxAddTag.Text = "Add";
            this.buttonComboBoxAddTag.UseVisualStyleBackColor = true;
            // 
            // buttonComboBoxCancel
            // 
            this.buttonComboBoxCancel.Location = new System.Drawing.Point(135, 48);
            this.buttonComboBoxCancel.Name = "buttonComboBoxCancel";
            this.buttonComboBoxCancel.Size = new System.Drawing.Size(75, 33);
            this.buttonComboBoxCancel.TabIndex = 2;
            this.buttonComboBoxCancel.Text = "Cancel";
            this.buttonComboBoxCancel.UseVisualStyleBackColor = true;
            // 
            // TabPageSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 93);
            this.Controls.Add(this.buttonComboBoxCancel);
            this.Controls.Add(this.buttonComboBoxAddTag);
            this.Controls.Add(this.comboBoxTagSelect);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "TabPageSelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a tag";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxTagSelect;
        private System.Windows.Forms.Button buttonComboBoxAddTag;
        private System.Windows.Forms.Button buttonComboBoxCancel;
    }
}