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
            TagsPanelSettingsPanel tagsPanelSettingsPanel = new TagsPanelSettingsPanel(storage, settingsStorage);
            TabPage tabPage = new System.Windows.Forms.TabPage(storage.GetTagName());
            tabPage.Controls.Add(tagsPanelSettingsPanel);
            this.tabControl1.Controls.Add(tabPage);
        }
    }
}
