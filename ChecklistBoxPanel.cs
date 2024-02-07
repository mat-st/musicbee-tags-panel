using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class ChecklistBoxPanel : UserControl
    {
        private readonly MusicBeeApiInterface mbApiInterface;
        private ItemCheckEventHandler eventHandler;

        private Style style;

        public ChecklistBoxPanel(MusicBeeApiInterface mbApiInterface, Dictionary<String, CheckState> data = null)
        {
            this.mbApiInterface = mbApiInterface;
            this.style = new Style(mbApiInterface);

            InitializeComponent();


            if (data != null)
            {
                AddDataSource(data);
            }
            StylePanel();
        }

        public void AddDataSource(Dictionary<String, CheckState> data)
        {
            this.checkedListBox1.BeginUpdate(); // Suspend the drawing of the control
            this.checkedListBox1.Items.Clear();

            string longestString = data.Keys.Aggregate("", (max, cur) => cur.Length > max.Length ? cur : max);
            foreach (KeyValuePair<String, CheckState> entry in data)
            {
                this.checkedListBox1.Items.Add(entry.Key, entry.Value);
            }

            this.checkedListBox1.ColumnWidth = TextRenderer.MeasureText(longestString, checkedListBox1.Font).Width + 20;
            this.checkedListBox1.EndUpdate(); // Resume the drawing of the control
        }


        private void StylePanel()
        {
            // apply current skin colors to tag panel
            this.style.StyleControl(checkedListBox1);
            this.style.StyleControl(this);
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

        private void CheckedListBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // this will prevent the item to be checked if a key was pressed
            this.checkedListBox1.CheckOnClick = true;
        }

        private void CheckedListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // this will prevent the item to be checked if a key was pressed
            this.checkedListBox1.CheckOnClick = false;
        }
    }
}
