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
        private const char Separator = ';';

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

        public void ReadTagsFromFile(string sourceFileUrl)
        {
            Clear();

            if (sourceFileUrl == null || sourceFileUrl.Length <= 0)
            {
                return;
            }

            string tagsFromFile = musicBeeApiInterface.Library_GetFileTag(sourceFileUrl, metaDataField);
            string[] tagParts = tagsFromFile.Split(Separator).Select(filetagOccasion => filetagOccasion.Trim()).ToArray();
            foreach (string tag in tagParts)
            {
                if (tag.Trim().Length <= 0)
                {
                    continue;
                }
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
