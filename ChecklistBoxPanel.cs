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
            checkedListBox1.BeginUpdate();
            checkedListBox1.Items.Clear();

            foreach (KeyValuePair<String, CheckState> entry in data)
            {
                checkedListBox1.Items.Add(entry.Key, entry.Value);
            }

            checkedListBox1.ColumnWidth = GetLongestStringWidth(data.Keys) + 20;
            checkedListBox1.EndUpdate();
        }

        private int GetLongestStringWidth(IEnumerable<string> strings)
        {
            var longestString = strings.Aggregate("", (max, cur) => cur.Length > max.Length ? cur : max);
            return TextRenderer.MeasureText(longestString, checkedListBox1.Font).Width;
        }

        private void StylePanel()
        {
            // apply current skin colors to tag panel
            style.StyleControl(checkedListBox1);
            style.StyleControl(this);
        }


        public void AddItemCheckEventHandler(ItemCheckEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
            checkedListBox1.ItemCheck += eventHandler;
        }

        public void RemoveItemCheckEventHandler()
        {
            //RemoveClickEvent(this.checkedListBox1);
            checkedListBox1.ItemCheck -= this.eventHandler;
        }

        private void CheckedListBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // this will prevent the item to be checked if a key was pressed
            checkedListBox1.CheckOnClick = true;
        }

        private void CheckedListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // this will prevent the item to be checked if a key was pressed
            checkedListBox1.CheckOnClick = false;
        }
    }
}
