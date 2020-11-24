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
    public partial class fvSettings : Form
    {

        public fvSettings(string[] occasions, bool sortEnabled)
        {
            InitializeComponent();
            setMoods(occasions);
            setSortEnabled(sortEnabled);

            // this must be at the very end to supress the events
            MakeOwnModifications();
        }

        private void setSortEnabled(bool sortEnabled)
        {
            this.cbEnableMoodSort.Checked = sortEnabled;
        }

        private void MakeOwnModifications()
        {
            this.lstOccasions.KeyDown += KeyEventHandler;
            this.txtOccasionInput.KeyDown += KeyEventHandler;

            this.cbEnableMoodSort.CheckedChanged += new System.EventHandler(this.cbEnableMoodSort_CheckedChanged);
        }

        private void btnAddMood_Click(object sender, EventArgs e)
        {
            addNewMoodToList();
        }

        private void btnRemMood_Click(object sender, EventArgs e)
        {
            removeSelectedItemFromList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowDialogToClearAll();
        }

        private void ShowDialogToClearAll()
        {
            DialogResult dialogResult = MessageBox.Show("Would you really really want to clear to list?", "Clear the list - really?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ClearMoods();
            }
        }

        private void KeyEventHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && sender == this.txtOccasionInput)
            {
                e.SuppressKeyPress = true;
                addNewMoodToList();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && sender == this.lstOccasions)
            {
                e.SuppressKeyPress = true;
                removeSelectedItemFromList();
                e.Handled = true;
            }
        }

        private void ShowConfirmationDialogToSort()
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to sort the moods alphabetically? Your previous order will be lost.", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.lstOccasions.Sorted = true;
            } else
            {
                this.cbEnableMoodSort.Checked = false;
            }
        }
        private void cbEnableMoodSort_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ShowConfirmationDialogToSort();
            } else
            {
                this.lstOccasions.Sorted = false;
            }
        }

        private void btnImportCSV_Click(object sender, EventArgs e)
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

            foreach (string importoccasion in firstlistA)
            {
                if (importoccasion.Trim().Length <= 0)
                {
                    continue;
                }
                if (!this.lstOccasions.Items.Contains(importoccasion))
                {
                    this.lstOccasions.Items.Add(importoccasion);
                }
            }
        }
    }
}
