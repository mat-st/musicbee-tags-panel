using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class TagsPanelSettingsPanel : UserControl
    {
        private TagsStorage tagsStorage;
        public TagsPanelSettingsPanel(SettingsStorage settings, string tagName)
        {
            InitializeComponent();

            tagsStorage = settings.GetTagsStorage(tagName);
            UpdateTags();
            UpdateSortOption();

            // this must be at the very end to supress the events
            MakeOwnModifications();
        }

        public void SetUpPanelForFirstUse()
        {
            // TODO select first item in listbox after opening settings form
            if (this.lstTags.Items.Count != 0)
            {
                lstTags.SelectedIndex = 0;
            }
            // TODO check how to get focus to textbox when opening settings panel
            if (txtNewTagCueInput.CanFocus)
            {
                txtNewTagCueInput.Focus();
            }
        } 

        private void UpdateSortOption()
        {
            this.cbEnableAlphabeticalTagSort.Checked = tagsStorage.Sorted;
            this.lstTags.Sorted = tagsStorage.Sorted;
        }

        private void MakeOwnModifications()
        {
            this.lstTags.KeyDown += KeyEventHandler;
            this.txtNewTagCueInput.KeyDown += KeyEventHandler;

            this.cbEnableAlphabeticalTagSort.CheckedChanged += new System.EventHandler(this.CbEnableTagSort_CheckedChanged);
        }


        private void KeyEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && sender == this.txtNewTagCueInput)
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


        public bool IsSortEnabled()
        {
            return this.cbEnableAlphabeticalTagSort.Checked;
        }

        public void UpdateTags()
        {
            Dictionary<String, CheckState> tagsDict = tagsStorage.GetTags();
            string[] tags = tagsDict.Keys.ToArray<String>();
            this.lstTags.Items.AddRange(tags == null ? new string[] { } : tags);
        }

        public void AddNewTagToList()
        {
            string newTag = this.txtNewTagCueInput.Text.Trim();
            if (newTag.Length <= 0)
            {
                return;
            }

            this.lstTags.BeginUpdate();
            if (!this.lstTags.Items.Contains(newTag))
            {
                this.tagsStorage.TagList[newTag] = CheckState.Unchecked;
                this.lstTags.Items.Add(newTag);
            }
            else
            {
                ShowDialogForDuplicate();
            }
            this.lstTags.EndUpdate();

            // remove text from input field
            this.txtNewTagCueInput.Text = null;
        }

        public void RemoveSelectedTagFromList()
        {
            System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems = new System.Windows.Forms.ListBox.SelectedObjectCollection(this.lstTags);
            selectedItems = this.lstTags.SelectedItems;

            if (this.lstTags.SelectedIndex != -1 && this.lstTags.Items.Count != 0)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                {
                    object selectedItem = selectedItems[i];
                    this.lstTags.Items.Remove(selectedItem);
                    this.tagsStorage.TagList.Remove((string)selectedItem);
                }
                    
                
                // TODO set selection on next listbox item after removing item
                //this.lstTags.SelectedIndex = 0;
            }
        }

        public void ClearTagsListInSettings()
        {
            this.lstTags.Items.Clear();
        }



        /***************************
        BUTTONS
        ***************************/
        private void BtnAddTag_Click(object sender, EventArgs e)
        {
            AddNewTagToList();
        }

        private void BtnRemTag_Click(object sender, EventArgs e)
        {
            RemoveSelectedTagFromList();
        }

        private void BtnImportCsv_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.Title = "Choose a CSV file";
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


        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            // TODO complete export CSV functionality and move import and export to seperate methods

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;

            saveFileDialog1.Title = "Choose a file name";
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog1.DefaultExt = "csv";

            saveFileDialog1.RestoreDirectory = true;

            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "" && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string exportCSVFilename = saveFileDialog1.FileName;

                StreamWriter csvWriter = new StreamWriter(File.OpenWrite(exportCSVFilename));

                foreach (var item in lstTags.Items)
                {
                    csvWriter.Write(item.ToString() + ";");
                }

                csvWriter.Close();
                MessageBox.Show("Tags exported in CSV");
            }
            else
                return;

        }
        private void BtnClearTagSettings_Click(object sender, EventArgs e)
        {
            if (lstTags.Items.Count != 0)
            {
                ShowDialogToClearList();
            }
        }

        /***************************
        DIALOGS
        ***************************/
        private void ShowConfirmationDialogToSort()
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to sort the tags alphabetically? Your previous order will be lost.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                this.lstTags.Sorted = true;
            }
            else
            {
                this.cbEnableAlphabeticalTagSort.Checked = false;
            }
        }

        private void ShowDialogForDuplicate()
        {
            MessageBox.Show("Tag is already in the list!", "Duplicate found!", MessageBoxButtons.OK);
        }


        private void ShowDialogToClearList()
        {
            DialogResult dialogResult = MessageBox.Show("This will clear your current tag list. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                ClearTagsListInSettings();
            }
            else
            {
                return;
            }
        }

        
    }
}
