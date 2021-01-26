using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class TagsPanelSettingsForm : Form
    {
        private Dictionary<string, TagsPanelSettingsPanel> tagPanels = new Dictionary<string, TagsPanelSettingsPanel>();
        private SettingsStorage settingsStorage;

        public SettingsStorage SettingsStorage { get => settingsStorage; set => settingsStorage = value; }

        public TagsPanelSettingsForm(SettingsStorage settingsStorage)
        {
            this.settingsStorage = settingsStorage;
            InitializeComponent();
            this.Btn_Save.DialogResult = DialogResult.OK;
            this.Btn_Cancel.DialogResult = DialogResult.Cancel;

            foreach (TagsStorage storage in settingsStorage.TagsStorages.Values)
            {
                AddPanel(storage);
            }
        }

        private bool AddPanel(TagsStorage storage)
        {
            TagsPanelSettingsPanel tagsPanelSettingsPanel;
            string tagName = storage.GetTagName();
            if (tagPanels.TryGetValue(tagName, out tagsPanelSettingsPanel))
            {
                ShowWarningMetaDataTypeExists();
                return false;
            }

            tagsPanelSettingsPanel = new TagsPanelSettingsPanel(this.settingsStorage, tagName);
            tagPanels.Add(tagName, tagsPanelSettingsPanel);
            TabPage tabPage = new System.Windows.Forms.TabPage(tagName);
            tabPage.Controls.Add(tagsPanelSettingsPanel);
            this.tabControlSettings.TabPages.Add(tabPage);
            tagsPanelSettingsPanel.SetUpPanelForFirstUse();
            
            return true;
        }

        public TagsPanelSettingsPanel GetPanel(string tagName)
        {
            TagsPanelSettingsPanel tagsPanelSettingsPanel;
            tagPanels.TryGetValue(tagName, out tagsPanelSettingsPanel);
            // TODO check if panel exists to prevent exceptions
            return tagsPanelSettingsPanel;
        }

        private void LinkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            MessageBox.Show("Tags-Panel Plugin " + Environment.NewLine + "Version " + version + Environment.NewLine +
                "Visit us on GitHub", "About Tags-Panel Plugin", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowWarningMetaDataTypeExists()
        {
            MessageBox.Show("This Metadata Type was already added", "Tag exists already",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LinkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mat-st/musicbee-tags-panel");
        }


        
        private void Btn_AddTabPage_Click(object sender, EventArgs e)
        {

            TabPageSelectorForm form = new TabPageSelectorForm();
            DialogResult result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                TagsStorage storage = new TagsStorage();
                storage.MetaDataType = form.GetMetaDataType();
                if (storage.MetaDataType != null && AddPanel(storage))
                {
                    settingsStorage.TagsStorages[storage.MetaDataType] = storage;
                    form.Close();
                }
            }
            else if (result == DialogResult.Cancel)
            {
                form.Close();
            }
        }

        private void btnRemoveTabPage_Click(object sender, EventArgs e)
        {
            TabPage tabToRemove = this.tabControlSettings.SelectedTab;
            if (tabToRemove == null)
            {
                return;
            }
            string tagName = tabToRemove.Text;
            this.tabControlSettings.TabPages.Remove(tabToRemove);
            settingsStorage.RemoveTagStorage(tagName);
            tagPanels.Remove(tagName);
        }
    }
}
