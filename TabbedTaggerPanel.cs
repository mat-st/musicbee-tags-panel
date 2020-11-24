using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class TabbedTaggerPanel : UserControl
    {
        private ChecklistBoxPanel checklistBox;

        public TabbedTaggerPanel(MusicBeeApiInterface mbApiInterface, OccasionList data)
        {
            InitializeComponent();
            AddPanel(mbApiInterface, data);
        }

        private void AddPanel(MusicBeeApiInterface mbApiInterface, OccasionList data)
        {
            this.checklistBox = new ChecklistBoxPanel(mbApiInterface, data);
            this.tabPage1.Controls.Add(this.checklistBox);
        }

        public void AddDataSource(OccasionList data)
        {
            this.checklistBox.AddDataSource(data);
        }
    }
}
