
namespace MusicBeePlugin
{
    partial class TagsPanelSettingsForm
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
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.btnAddTabPage = new System.Windows.Forms.Button();
            this.btnRemoveTabPage = new System.Windows.Forms.Button();
            this.linkAbout = new System.Windows.Forms.LinkLabel();
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Location = new System.Drawing.Point(17, 12);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(250, 288);
            this.tabControlSettings.TabIndex = 0;
            // 
            // btnAddTabPage
            // 
            this.btnAddTabPage.Location = new System.Drawing.Point(17, 306);
            this.btnAddTabPage.Name = "btnAddTabPage";
            this.btnAddTabPage.Size = new System.Drawing.Size(112, 23);
            this.btnAddTabPage.TabIndex = 1;
            this.btnAddTabPage.Text = "Add TabPage";
            this.btnAddTabPage.UseVisualStyleBackColor = true;
            // 
            // btnRemoveTabPage
            // 
            this.btnRemoveTabPage.Location = new System.Drawing.Point(144, 306);
            this.btnRemoveTabPage.Name = "btnRemoveTabPage";
            this.btnRemoveTabPage.Size = new System.Drawing.Size(123, 23);
            this.btnRemoveTabPage.TabIndex = 2;
            this.btnRemoveTabPage.Text = "Remove TabPage";
            this.btnRemoveTabPage.UseVisualStyleBackColor = true;
            // 
            // linkAbout
            // 
            this.linkAbout.AutoSize = true;
            this.linkAbout.Location = new System.Drawing.Point(232, 334);
            this.linkAbout.Name = "linkAbout";
            this.linkAbout.Size = new System.Drawing.Size(35, 13);
            this.linkAbout.TabIndex = 3;
            this.linkAbout.TabStop = true;
            this.linkAbout.Text = "About";
            this.linkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkAbout_LinkClicked);
            // 
            // linkGitHub
            // 
            this.linkGitHub.AutoSize = true;
            this.linkGitHub.Location = new System.Drawing.Point(17, 334);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(91, 13);
            this.linkGitHub.TabIndex = 4;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "Visit us on GitHub";
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGitHub_LinkClicked);
            // 
            // TagsPanelSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this.linkGitHub);
            this.Controls.Add(this.linkAbout);
            this.Controls.Add(this.btnRemoveTabPage);
            this.Controls.Add(this.btnAddTabPage);
            this.Controls.Add(this.tabControlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TagsPanelSettingsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tags-Panel Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.Button btnAddTabPage;
        private System.Windows.Forms.Button btnRemoveTabPage;
        private System.Windows.Forms.LinkLabel linkAbout;
        private System.Windows.Forms.LinkLabel linkGitHub;
    }
}