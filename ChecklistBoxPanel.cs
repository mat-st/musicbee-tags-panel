using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using static System.Windows.Forms.TabControl;

namespace MusicBeePlugin
{
    public partial class ChecklistBoxPanel : UserControl
    {
        private readonly MusicBeeApiInterface mbApiInterface;

        public ChecklistBoxPanel(MusicBeeApiInterface mbApiInterface, OccasionList data)
        {
            this.mbApiInterface = mbApiInterface;
            
            InitializeComponent();

            this.checkedListBox1.DisplayMember = "occasion";
            this.checkedListBox1.ValueMember = "selected";

            AddDataSource(data);
            StylePanel();
        }

        public void AddDataSource(OccasionList data)
        {
            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.AddRange(data.ToArray());
        }

        private void StylePanel()
        {
            /*DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle
            {
                BackColor = GetElementColor(Plugin.SkinElement.SkinTrackAndArtistPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground),
                SelectionBackColor = GetElementColor(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateModified, Plugin.ElementComponent.ComponentForeground),
                SelectionForeColor = GetElementColor(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground),
            };

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle
            {
                BackColor = GetElementColor(Plugin.SkinElement.SkinInputPanelLabel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground),
            };*/


            
            checkedListBox1.BackColor = GetElementColor(Plugin.SkinElement.SkinTrackAndArtistPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground);
            checkedListBox1.ForeColor = GetElementColor(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground);
        }
        public Color GetElementColor(SkinElement skinElement, ElementState elementState, ElementComponent elementComponent)
        {
            int colorValue = this.mbApiInterface.Setting_GetSkinElementColour(skinElement, elementState, elementComponent);
            return Color.FromArgb(colorValue);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
