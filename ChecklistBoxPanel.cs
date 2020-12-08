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
        private ItemCheckEventHandler eventHandler;

        public ChecklistBoxPanel(MusicBeeApiInterface mbApiInterface, Dictionary<String, CheckState> data)
        {
            this.mbApiInterface = mbApiInterface;
            
            InitializeComponent();

            this.checkedListBox1.DisplayMember = "occasion";
            this.checkedListBox1.ValueMember = "selected";

            AddDataSource(data);
            StylePanel();
        }

        public void AddDataSource(Dictionary<String, CheckState> data)
        {
            this.checkedListBox1.Items.Clear();
            string longestString = "";
            foreach (String key in data.Keys.ToArray())
            {
                longestString = (key.Length > longestString.Length) ? key : longestString;
                CheckState value = data[key];
                this.checkedListBox1.Items.Add(key, value);
            }
            this.checkedListBox1.ColumnWidth = TextRenderer.MeasureText(longestString, checkedListBox1.Font).Width + 20;

            //this.checkedListBox1.Items.AddRange(data);
        }

        private void StylePanel()
        {
                   
            checkedListBox1.BackColor = GetElementColor(Plugin.SkinElement.SkinTrackAndArtistPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground);
            checkedListBox1.ForeColor = GetElementColor(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground);
        }
        public Color GetElementColor(SkinElement skinElement, ElementState elementState, ElementComponent elementComponent)
        {
            int colorValue = this.mbApiInterface.Setting_GetSkinElementColour(skinElement, elementState, elementComponent);
            return Color.FromArgb(colorValue);
        }

        public void AddItemCheckEventHandler(ItemCheckEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
            this.checkedListBox1.ItemCheck += eventHandler;
        }

        public void RemoveItemCheckEventHandler()
        {
            //RemoveClickEvent(this.checkedListBox1);
            this.checkedListBox1.ItemCheck -= this.eventHandler;
        }

        private void RemoveClickEvent(CheckedListBox b)
        {
            System.Reflection.FieldInfo f1 = typeof(Control).GetField("ItemCheck",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            object obj = f1.GetValue(b);
            System.Reflection.PropertyInfo pi = b.GetType().GetProperty("Events",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }

        private void checkedListBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // this will prevent the item to be checked if a key was pressed
            this.checkedListBox1.CheckOnClick = true;
        }

        private void checkedListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // this will prevent the item to be checked if a key was pressed
            this.checkedListBox1.CheckOnClick = false;
        }
    }
}
