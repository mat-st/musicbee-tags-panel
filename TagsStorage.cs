using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    class TagsStorage
    {
        public const char SEPARATOR = ';';

        private MetaDataType metaDataField = MetaDataType.Occasion;
        private readonly MusicBeeApiInterface musicBeeApiInterface;
        private Dictionary<String, CheckState> occasionList = new Dictionary<String, CheckState>();

        public TagsStorage(MusicBeeApiInterface musicBeeApiInterface, MetaDataType metaDataField)
        {
            this.musicBeeApiInterface = musicBeeApiInterface;
            this.metaDataField = metaDataField;
        }

        public void Clear()
        {
            occasionList.Clear();
        }

        public void UpdateTagsFromFile(string sourceFileUrl)
        {
            Clear();

            string[] tagParts = ReadTagsFromFile(sourceFileUrl);

            foreach (string tag in tagParts)
            {
                CheckState checkState;
                if (!occasionList.TryGetValue(tag, out checkState))
                {
                    occasionList.Add(tag, CheckState.Checked);
                }
                else
                {
                    checkState = CheckState.Checked;
                }
            }
        }

        public string[] ReadTagsFromFile(string filename)
        {
            HashSet<string> tags = new HashSet<string>();

            if (filename == null || filename.Length <= 0)
            {
                return tags.ToArray<string>();
            }

            string filetagOccasions = musicBeeApiInterface.Library_GetFileTag(filename, MetaDataType.Occasion);
            string[] filetagOccasionssParts = filetagOccasions.Split(';');
            foreach (string occasion in filetagOccasionssParts)
            {
                if (occasion.Trim().Length <= 0)
                {
                    continue;
                }
                tags.Add(occasion.Trim());
            }

            return tags.ToArray<string>();
        }

        internal Dictionary<string, CheckState> GetTags()
        {
            return occasionList;
        }

        internal void SetTags(Dictionary<string, CheckState> occasionList)
        {
            this.occasionList = occasionList;
        }
    }
}
