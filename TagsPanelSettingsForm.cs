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

        public TagsPanelSettingsForm(List<TagsStorage> storages, SettingsStorage settingsStorage)
        {
            InitializeComponent();

            foreach(TagsStorage storage in storages)
            {
                AddPanel(storage, settingsStorage);
            }
        }

        private void AddPanel(TagsStorage storage, SettingsStorage settingsStorage)
        {
            TagsPanelSettingsPanel tagsPanelSettingsPanel;
            if (tagPanels.TryGetValue(storage.GetTagName(), out tagsPanelSettingsPanel))
            {
                // TODO show dialog that tag already exist
                return;
            }

            tagsPanelSettingsPanel = new TagsPanelSettingsPanel(settingsStorage);
            tagPanels.Add(storage.GetTagName(), tagsPanelSettingsPanel);
            TabPage tabPage = new System.Windows.Forms.TabPage(storage.GetTagName());
            tabPage.Controls.Add(tagsPanelSettingsPanel);
            this.tabControlSettings.Controls.Add(tabPage);
        }

        public TagsPanelSettingsPanel GetPanel(string tagName)
        {
            TagsPanelSettingsPanel tagsPanelSettingsPanel;
            tagPanels.TryGetValue(tagName, out tagsPanelSettingsPanel);

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

        private void LinkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mat-st/musicbee-tags-panel");
        }
    }
}
