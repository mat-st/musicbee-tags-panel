using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class TabPageSelectorForm : Form
    {
        // Tags in this list will be ignored in the TagSelector form
        readonly HashSet<string> blacklist = new HashSet<string> { 
            MetaDataType.Artwork.ToString("g"),
            MetaDataType.DiscNo.ToString("g"),
            MetaDataType.DiscCount.ToString("g"),
            MetaDataType.Encoder.ToString("g"),
            MetaDataType.HasLyrics.ToString("g"),
            MetaDataType.Lyrics.ToString("g"),
            MetaDataType.TrackCount.ToString("g"),
            MetaDataType.Rating.ToString("g"),
            MetaDataType.RatingAlbum.ToString("g"),
            MetaDataType.RatingLove.ToString("g"),
        };

        public TabPageSelectorForm()
        {
            InitializeComponent();
            this.Btn_ComboBoxAddTag.DialogResult = DialogResult.OK;
            this.Btn_ComboBoxCancel.DialogResult = DialogResult.Cancel;
            SetMetaDataTypes();
        }

        private void SetMetaDataTypes()
        {
            List<string> metaDataTypes = GetMetaDataTypesAsString();
            comboBoxTagSelect.DataSource = metaDataTypes;
        }

        private List<string> GetMetaDataTypesAsString()
        {
            Array values = Enum.GetValues(typeof(MetaDataType));
            List<string> dataTypesAsString = new List<string>();
            foreach (MetaDataType dataType in values)
            {
                string asString = dataType.ToString("g");
                if (blacklist.Contains(asString))
                {
                    continue;
                }
                dataTypesAsString.Add(asString);
            }
            dataTypesAsString.Sort();
            return dataTypesAsString;
        }

        public string GetMetaDataType()
        {
            object selectedItem = comboBoxTagSelect.SelectedItem;
            if (selectedItem != null)
            {
                return (string) selectedItem;
            }
            return null;
        }
    }
}
