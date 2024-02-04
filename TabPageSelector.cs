using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class TabPageSelectorForm : Form
    {
        // Tags in this list will be ignored in the TagSelector form
        private static readonly HashSet<MetaDataType> blacklist = new HashSet<MetaDataType> {
                MetaDataType.Artwork,
                MetaDataType.DiscNo,
                MetaDataType.DiscCount,
                MetaDataType.Encoder,
                MetaDataType.HasLyrics,
                MetaDataType.Lyrics,
                MetaDataType.TrackCount,
                MetaDataType.Rating,
                MetaDataType.RatingAlbum,
                MetaDataType.RatingLove
            };

        public TabPageSelectorForm()
        {
            InitializeComponent();
            Btn_ComboBoxAddTag.DialogResult = DialogResult.OK;
            Btn_ComboBoxCancel.DialogResult = DialogResult.Cancel;
            SetMetaDataTypes();
        }

        private void SetMetaDataTypes()
        {
            List<string> metaDataTypes = GetMetaDataTypesAsString();
            comboBoxTagSelect.DataSource = metaDataTypes;
        }

        private List<string> GetMetaDataTypesAsString()
        {
            List<string> dataTypesAsString = Enum.GetValues(typeof(MetaDataType))
                .Cast<MetaDataType>()
                .Where(dataType => !blacklist.Contains(dataType))
                .Select(dataType => dataType.ToString("g"))
                .ToList();

            dataTypesAsString.Sort();
            return dataTypesAsString;
        }

        public string GetMetaDataType()
        {
            return comboBoxTagSelect.SelectedItem as string;
        }
    }
}
