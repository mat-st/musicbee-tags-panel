
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
            this.btnAddTabPage = new System.Windows.Forms.Button();
            this.btnRemoveTabPage = new System.Windows.Forms.Button();
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Btn_Cancel = new System.Windows.Forms.Button();
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
            // btnAddTabPage
            // 
            this.btnAddTabPage.Location = new System.Drawing.Point(72, 777);
            this.btnAddTabPage.Name = "btnAddTabPage";
            this.btnAddTabPage.Size = new System.Drawing.Size(111, 33);
            this.btnAddTabPage.TabIndex = 1;
            this.btnAddTabPage.Text = "Add Tag";
            this.btnAddTabPage.UseVisualStyleBackColor = true;
            this.btnAddTabPage.Click += new System.EventHandler(this.Btn_AddTagPage_Click);
            // 
            // btnRemoveTabPage
            // 
            this.btnRemoveTabPage.Location = new System.Drawing.Point(243, 777);
            this.btnRemoveTabPage.Name = "btnRemoveTabPage";
            this.btnRemoveTabPage.Size = new System.Drawing.Size(111, 33);
            this.btnRemoveTabPage.TabIndex = 2;
            this.btnRemoveTabPage.Text = "Remove Tag";
            this.btnRemoveTabPage.UseVisualStyleBackColor = true;
            this.btnRemoveTabPage.Click += new System.EventHandler(this.BtnRemoveTagPage_Click);
            // 
            // linkGitHub
            // 
            this.linkGitHub.AutoSize = true;
            this.linkGitHub.Location = new System.Drawing.Point(12, 889);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(126, 19);
            this.linkGitHub.TabIndex = 5;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "Visit me on GitHub";
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGitHub_LinkClicked);
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(73, 824);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(111, 33);
            this.Btn_Save.TabIndex = 3;
            this.Btn_Save.Text = "Save";
            this.Btn_Save.UseVisualStyleBackColor = true;
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Btn_Cancel.Location = new System.Drawing.Point(243, 824);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(111, 33);
            this.Btn_Cancel.TabIndex = 4;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
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
            this.CancelButton = this.Btn_Cancel;
            this.ClientSize = new System.Drawing.Size(621, 917);
            this.Controls.Add(this.VersionLbl);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_Save);
            this.Controls.Add(this.linkGitHub);
            this.Controls.Add(this.btnRemoveTabPage);
            this.Controls.Add(this.btnAddTabPage);
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
        private System.Windows.Forms.Button btnAddTabPage;
        private System.Windows.Forms.Button btnRemoveTabPage;
        private System.Windows.Forms.LinkLabel linkGitHub;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.ToolTip toolTipAddTagPage;
        private System.Windows.Forms.Label VersionLbl;
    }
}