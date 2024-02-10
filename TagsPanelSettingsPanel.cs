﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class TagsPanelSettingsPanel : UserControl
    {
        private TagsStorage tagsStorage;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        public TagsPanelSettingsPanel(string tagName)
        {
            InitializeComponent();

            InitializeToolTip();
            InitializeTagsStorage(tagName);
            UpdateTags();
            UpdateSortOption();

            // this must be at the very end to suppress the events
            InitializeEventHandlers();

            TxtNewTagInput.Focus(); // Set focus to the textbox
        }

        private void InitializeToolTip()
        {
            // Create a new ToolTip instance
            ToolTip toolTip = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;

            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip.ShowAlways = true;

            // Set up the ToolTip text for the CheckBox.
            toolTip.SetToolTip(this.checkboxEnableAlphabeticalTagSorting, "If enabled the Tags are always sorted alphabetically in the tag. If not enabled you can use the up and down buttons to reorder your tag lists.");

            SendMessage(TxtNewTagInput.Handle, EM_SETCUEBANNER, 0, "Please enter a tag");
        }

        private void InitializeTagsStorage(string tagName)
        {
            tagsStorage = SettingsStorage.GetTagsStorage(tagName);
        }

        private void SetUpDownButtonsStateDisabled()
        {
            this.buttonMoveTagUp.Enabled = false;
            this.buttonMoveTagDown.Enabled = false;
        }

        private void SetUpDownButtonsStateEnabled()
        {
            this.buttonMoveTagUp.Enabled = true;
            this.buttonMoveTagDown.Enabled = true;
        }

        private void TxtNewTagInput_Leave(object sender, EventArgs e)
        {
            if (TxtNewTagInput.Text.Length == 0)
            {
                TxtNewTagInput.Text = "Please enter a tag";
                TxtNewTagInput.ForeColor = SystemColors.GrayText;
            }
        }

        private void TxtNewTagInput_Enter(object sender, EventArgs e)
        {
            if (TxtNewTagInput.Text == "Please enter a tag")
            {
                TxtNewTagInput.Text = "";
                TxtNewTagInput.ForeColor = SystemColors.WindowText;
            }
        }

        public void SetUpPanelForFirstUse()
        {
            if (this.lstTags.Items.Count != 0)
            {
                lstTags.SelectedIndex = 0;
            }

            if (tagsStorage.Sorted == true)
            {
                SetUpDownButtonsStateDisabled();
            }
            else
            {
                SetUpDownButtonsStateEnabled();
            }
        }

        private void UpdateSortOption()
        {
            this.checkboxEnableAlphabeticalTagSorting.Checked = tagsStorage.Sorted;
            this.lstTags.Sorted = tagsStorage.Sorted;
            if (tagsStorage.Sorted == true)
            {
                SetUpDownButtonsStateDisabled();
            }
            else
            {
                SetUpDownButtonsStateEnabled();
            }
        }

        private void InitializeEventHandlers()
        {
            this.lstTags.KeyDown += KeyEventHandler;
            this.TxtNewTagInput.KeyDown += KeyEventHandler;

            this.checkboxEnableAlphabeticalTagSorting.CheckedChanged += CbEnableTagSort_CheckedChanged;
        }

        private void KeyEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && sender == this.TxtNewTagInput)
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
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.Checked)
            {
                ShowConfirmationDialogToSort();
                SetUpDownButtonsStateDisabled();
            }
            else
            {
                SetUpDownButtonsStateEnabled();
                tagsStorage.Sorted = false;
                this.lstTags.Sorted = false;
            }
        }

        public bool IsSortEnabled()
        {
            return this.checkboxEnableAlphabeticalTagSorting.Checked;
        }

        public void UpdateTags()
        {
            Dictionary<String, CheckState> tagsDict = tagsStorage.GetTags();
            string[] tags = tagsDict.Keys.ToArray();
            Array.Sort(tags); // Sort the tags alphabetically
            this.lstTags.Items.AddRange(tags);
        }

        public void AddNewTagToList()
        {
            string newTag = this.TxtNewTagInput.Text.Trim();
            if (string.IsNullOrEmpty(newTag) || newTag == "Please Enter A Tag")
            {
                return;
            }

            if (!this.lstTags.Items.Contains(newTag))
            {
                this.tagsStorage.TagList[newTag] = this.tagsStorage.TagList.Count();
                this.lstTags.Items.Add(newTag);
            }
            else
            {
                ShowDialogForDuplicate();
            }

            this.TxtNewTagInput.Text = string.Empty;
        }

        public void RemoveSelectedTagFromList()
        {
            System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems = new System.Windows.Forms.ListBox.SelectedObjectCollection(lstTags);
            selectedItems = lstTags.SelectedItems;

            if (lstTags.SelectedIndex != -1 && lstTags.Items.Count != 0)
            {
                int selectedIndex = lstTags.SelectedIndex; // Store the index of the selected item

                for (int i = selectedItems.Count - 1; i >= 0; i--)
                {
                    object selectedItem = selectedItems[i];
                    lstTags.Items.Remove(selectedItem);
                    tagsStorage.TagList.Remove((string)selectedItem);
                }

                // Select the item above the removed item
                if (selectedIndex > 0)
                {
                    lstTags.SelectedIndex = selectedIndex - 1;
                }
                else if (lstTags.Items.Count > 0)
                {
                    lstTags.SelectedIndex = 0;
                }
            }
        }

        public void ClearTagsListInSettings()
        {
            lstTags.Items.Clear();
            tagsStorage.Clear();
        }

        public void ImportCsv()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.Title = "Choose a CSV file";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.Multiselect = false;

            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string importCsvFilename = openFileDialog1.FileName;
                if (importCsvFilename.Length <= 0)
                {
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Do you want to continue with the CSV import?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    string[] lines = File.ReadAllLines(importCsvFilename);
                    List<string> importedTags = new List<string>();

                    foreach (string line in lines)
                    {
                        string[] values = line.Split(';');
                        foreach (string value in values)
                        {
                            string importtag = value.Trim();
                            if (!string.IsNullOrEmpty(importtag) && !importedTags.Contains(importtag))
                            {
                                importedTags.Add(importtag);
                                this.tagsStorage.TagList[importtag] = this.tagsStorage.TagList.Count();
                                this.lstTags.Items.Add(importtag);
                            }
                        }
                    }

                    MessageBox.Show("CSV import successful");
                }
                else
                {
                    MessageBox.Show("CSV import canceled");
                }
            }
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.Title = "Choose a file name";
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string exportCSVFilename = saveFileDialog1.FileName;

                using (StreamWriter csvWriter = new StreamWriter(exportCSVFilename))
                {
                    string csvContent = string.Join(";", lstTags.Items.Cast<string>());
                    csvWriter.Write(csvContent);
                }

                MessageBox.Show("Tags exported in CSV");
            }
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
            ImportCsv();
        }

        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            ExportCsv();
        }

        private void BtnClearTagSettings_Click(object sender, EventArgs e)
        {
            if (lstTags.Items.Count != 0)
            {
                ShowDialogToClearList();
            }
        }

        private void BtnMoveTagUpSettings_Click(object sender, EventArgs e)
        {
            if (lstTags.Items.Count != 0)
            {
                MoveUp();
            }
        }

        private void BtnMoveTagDownSettings_Click(object sender, EventArgs e)
        {
            if (lstTags.Items.Count != 0)
            {
                MoveDown();
            }
        }

        public void MoveUp()
        {
            MoveItem(-1);
        }

        public void MoveDown()
        {
            MoveItem(1);
        }

        public void MoveItem(int direction)
        {
            // Checking selected item
            if (lstTags.SelectedItem == null || lstTags.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = lstTags.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= lstTags.Items.Count)
                return; // Index out of range - nothing to do

            object selected = lstTags.SelectedItem;

            // Removing removable element
            lstTags.Items.RemoveAt(lstTags.SelectedIndex);
            // Insert it in new position
            lstTags.Items.Insert(newIndex, selected);
            // Restore selection
            lstTags.SetSelected(newIndex, true);
            // Put the selected item to a new position
            tagsStorage.SwapElement(selected.ToString(), newIndex);
        }

        public void SortAlphabetically()
        {
            SetUpDownButtonsStateDisabled();
            tagsStorage.Sort();
            lstTags.BeginUpdate(); // Suspend drawing of the ListBox
            lstTags.Items.Clear();

            foreach (var listItem in tagsStorage.GetTags())
            {
                lstTags.Items.Add(listItem.Key);
            }

            lstTags.EndUpdate(); // Resume drawing of the ListBox
        }

        /***************************
        DIALOGS
        ***************************/

        private void ShowConfirmationDialogToSort()
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to sort the tags alphabetically? Your current order will be lost.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SortAlphabetically();
                tagsStorage.Sorted = true;
                this.lstTags.Sorted = true;
            }
            else
            {
                tagsStorage.Sorted = false;
                this.lstTags.Sorted = false;
                this.checkboxEnableAlphabeticalTagSorting.Checked = false;
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