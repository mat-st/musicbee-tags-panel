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
        private MusicBeeApiInterface mbApiInterface;
        private Color BackColorActive;
        private Color ForeColorWhite;
        private Color BackColorInactive;

        public TabbedTaggerPanel(MusicBeeApiInterface mbApiInterface, OccasionList data)
        {
            this.mbApiInterface = mbApiInterface;
            InitializeComponent();
            AddPanel(mbApiInterface, data);
            ConfigureBackground();
        }

        public Color GetElementColor(SkinElement skinElement, ElementState elementState, ElementComponent elementComponent)
        {
            int colorValue = this.mbApiInterface.Setting_GetSkinElementColour(skinElement, elementState, elementComponent);
            return Color.FromArgb(colorValue);
        }

        private void ConfigureBackground()
        {
            this.BackColorInactive = GetElementColor(Plugin.SkinElement.SkinTrackAndArtistPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground);
            this.ForeColorWhite = GetElementColor(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground);
            this.BackColorActive = GetElementColor(Plugin.SkinElement.SkinInputPanelLabel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground);

            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabPage1.BackColor = this.BackColorActive;
            this.tabPage2.BackColor = this.BackColorActive;
            this.tabPage3.BackColor = this.BackColorActive;
            this.BackColor = Color.Red;
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

        private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Font f;
            Brush backBrush;
            Brush foreBrush;

            if (e.Index == this.tabControl1.SelectedIndex)
            {
                f = e.Font;
                backBrush = new SolidBrush(this.BackColorActive);
                foreBrush = new SolidBrush(this.ForeColorWhite);
            }
            else
            {
                f = e.Font;
                backBrush = new SolidBrush(this.BackColorInactive);
                foreBrush = new SolidBrush(this.ForeColorWhite);
            }

            string tabName = this.tabControl1.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            RectangleF r = e.Bounds;
            r = new RectangleF(r.X, r.Y + 3, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

            /*sf.Dispose();
            if (e.Index == this.tabControl1.SelectedIndex)
            {
                f.Dispose();
                backBrush.Dispose();
                foreBrush.Dispose();
            }
            else
            {
                backBrush.Dispose();
                foreBrush.Dispose();
            }*/
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush backBrush = new SolidBrush(this.BackColorInactive);
            e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
        }
    }
}
