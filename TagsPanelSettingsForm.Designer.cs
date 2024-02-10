
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
            this.components = new System.ComponentModel.Container();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.buttonAddTabPage = new System.Windows.Forms.Button();
            this.buttonRemoveTabPage = new System.Windows.Forms.Button();
            this.linkToGitHub = new System.Windows.Forms.LinkLabel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTipAddTagPage = new System.Windows.Forms.ToolTip(this.components);
            this.VersionLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlSettings.Location = new System.Drawing.Point(0, 0);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.Padding = new System.Drawing.Point(3, 3);
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(621, 760);
            this.tabControlSettings.TabIndex = 0;
            // 
            // buttonAddTabPage
            // 
            this.buttonAddTabPage.Location = new System.Drawing.Point(72, 777);
            this.buttonAddTabPage.Name = "buttonAddTabPage";
            this.buttonAddTabPage.Size = new System.Drawing.Size(111, 33);
            this.buttonAddTabPage.TabIndex = 1;
            this.buttonAddTabPage.Text = "Add Tag";
            this.buttonAddTabPage.UseVisualStyleBackColor = true;
            this.buttonAddTabPage.Click += new System.EventHandler(this.Btn_AddTagPage_Click);
            // 
            // buttonRemoveTabPage
            // 
            this.buttonRemoveTabPage.Location = new System.Drawing.Point(243, 777);
            this.buttonRemoveTabPage.Name = "buttonRemoveTabPage";
            this.buttonRemoveTabPage.Size = new System.Drawing.Size(111, 33);
            this.buttonRemoveTabPage.TabIndex = 2;
            this.buttonRemoveTabPage.Text = "Remove Tag";
            this.buttonRemoveTabPage.UseVisualStyleBackColor = true;
            this.buttonRemoveTabPage.Click += new System.EventHandler(this.BtnRemoveTagPage_Click);
            // 
            // linkToGitHub
            // 
            this.linkToGitHub.AutoSize = true;
            this.linkToGitHub.Location = new System.Drawing.Point(12, 889);
            this.linkToGitHub.Name = "linkToGitHub";
            this.linkToGitHub.Size = new System.Drawing.Size(126, 19);
            this.linkToGitHub.TabIndex = 5;
            this.linkToGitHub.TabStop = true;
            this.linkToGitHub.Text = "Visit me on GitHub";
            this.linkToGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGitHub_LinkClicked);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(73, 824);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(111, 33);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(243, 824);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(111, 33);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // toolTipAddTagPage
            // 
            this.toolTipAddTagPage.AutomaticDelay = 1000;
            // 
            // VersionLbl
            // 
            this.VersionLbl.AutoSize = true;
            this.VersionLbl.Location = new System.Drawing.Point(512, 889);
            this.VersionLbl.Name = "VersionLbl";
            this.VersionLbl.Size = new System.Drawing.Size(0, 19);
            this.VersionLbl.TabIndex = 7;
            // 
            // TagsPanelSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(621, 917);
            this.Controls.Add(this.VersionLbl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.linkToGitHub);
            this.Controls.Add(this.buttonRemoveTabPage);
            this.Controls.Add(this.buttonAddTabPage);
            this.Controls.Add(this.tabControlSettings);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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
        private System.Windows.Forms.Button buttonAddTabPage;
        private System.Windows.Forms.Button buttonRemoveTabPage;
        private System.Windows.Forms.LinkLabel linkToGitHub;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolTip toolTipAddTagPage;
        private System.Windows.Forms.Label VersionLbl;
    }
}