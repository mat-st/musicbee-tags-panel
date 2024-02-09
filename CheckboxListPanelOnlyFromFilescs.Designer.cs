namespace MusicBeePlugin
{
    partial class CheckboxListPanelOnlyFromFilescs
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedListBoxNotUserDefined = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkedListBoxNotUserDefined
            // 
            this.checkedListBoxNotUserDefined.FormattingEnabled = true;
            this.checkedListBoxNotUserDefined.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxNotUserDefined.Name = "checkedListBoxNotUserDefined";
            this.checkedListBoxNotUserDefined.Size = new System.Drawing.Size(150, 140);
            this.checkedListBoxNotUserDefined.TabIndex = 0;
            // 
            // CheckboxListPanelOnlyFromFilescs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBoxNotUserDefined);
            this.Name = "CheckboxListPanelOnlyFromFilescs";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxNotUserDefined;
    }
}
