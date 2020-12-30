using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class TagsPanelSettingsPanel : UserControl
    {
        public TagsPanelSettingsPanel(TagsStorage storage, SettingsStorage settings)
        {
            InitializeComponent();

            SetTags(settings.GetAllTagsFromConfig());
            SetSortEnabled(settings.GetSavedSettings().sorted);

            // this must be at the very end to supress the events
            MakeOwnModifications();
        }

        private void SetSortEnabled(bool sortEnabled)
        {
            this.cbEnableAlphabeticalTagSort.Checked = sortEnabled;
        }

        private void MakeOwnModifications()
        {
            this.lstTags.KeyDown += KeyEventHandler;
            this.txtNewTagInput.KeyDown += KeyEventHandler;

            this.cbEnableAlphabeticalTagSort.CheckedChanged += new System.EventHandler(this.CbEnableTagSort_CheckedChanged);
        }

        private void BtnAddTag_Click(object sender, EventArgs e)
        {
            AddNewTagToList();
        }

        private void BtnRemTag_Click(object sender, EventArgs e)
        {
            RemoveSelectedTagFromList();
        }

        private void KeyEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && sender == this.txtNewTagInput)
            {
                e.SuppressKeyPress = true;
                AddNewTagToList();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && sender == this.lstTags)
            {
                e.SuppressKeyPress = true;
                RemoveSelectedTagFromList();
                e.Handled = true;
            }
        }

        private void ShowConfirmationDialogToSort()
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to sort the tags alphabetically? Your previous order will be lost.", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.lstTags.Sorted = true;
            }
            else
            {
                this.cbEnableAlphabeticalTagSort.Checked = false;
            }
        }
        private void CbEnableTagSort_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ShowConfirmationDialogToSort();
            }
            else
            {
                this.lstTags.Sorted = false;
            }
        }

        private void BtnImportCSV_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.Title = "Browse CSV File";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.Multiselect = false;

            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ShowDialog();

            string importCSVfilename = openFileDialog1.FileName;
            if (importCSVfilename.Length <= 0)
            {
                return;
            }

            StreamReader reader = new StreamReader(File.OpenRead(importCSVfilename));
            List<string> listA = new List<String>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (!String.IsNullOrWhiteSpace(line))
                {
                    string[] values = line.Split(';');
                    listA.AddRange(values);
                }
            }
            string[] firstlistA = listA.ToArray();

            foreach (string importtag in firstlistA)
            {
                if (importtag.Trim().Length <= 0)
                {
                    continue;
                }
                if (!this.lstTags.Items.Contains(importtag))
                {
                    this.lstTags.Items.Add(importtag);
                }
            }
        }

        public string[] GetTags()
        {
            if (this.lstTags.Items.Count > 0)
            {
                String[] array = new String[this.lstTags.Items.Count];
                this.lstTags.Items.CopyTo(array, 0);
                return array;
            }

            return new string[] { };
        }

        public bool IsSortEnabled()
        {
            return this.cbEnableAlphabeticalTagSort.Checked;
        }

        public void SetTags(string[] moods)
        {
            this.lstTags.Items.AddRange(moods == null ? new string[] { } : moods);
        }

        public void AddNewTagToList()
        {
            string newTag = this.txtNewTagInput.Text;
            if (newTag.Trim().Length <= 0)
            {
                return;
            }

            this.lstTags.BeginUpdate();
            if (!this.lstTags.Items.Contains(newTag))
            {
                this.lstTags.Items.Add(newTag);
            }
            else
            {
                ShowDialogForDuplicate();
            }
            this.lstTags.EndUpdate();

            // remove text from input field
            this.txtNewTagInput.Text = null;
        }

        private void ShowDialogForDuplicate()
        {
            MessageBox.Show("Tag is already in the list", "Duplicate found", MessageBoxButtons.OK);

        }

        public void RemoveSelectedTagFromList()
        {
            System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems = new System.Windows.Forms.ListBox.SelectedObjectCollection(this.lstTags);
            selectedItems = this.lstTags.SelectedItems;

            if (this.lstTags.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    this.lstTags.Items.Remove(selectedItems[i]);
            }
        }

        public void ClearTags()
        {
            this.lstTags.Items.Clear();
        }
    }
}
