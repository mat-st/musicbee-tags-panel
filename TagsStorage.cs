using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class TagsStorage
    {
        public const char SEPARATOR = ';';

        private readonly MetaDataType metaDataField;
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

            string filetagOccasions = musicBeeApiInterface.Library_GetFileTag(filename, metaDataField);
            string[] filetagOccasionssParts = filetagOccasions.Split(SEPARATOR);
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

        public string RemoveTag(string selectedTag, string fileUrl)
        {
            string tags = GetTags(fileUrl);
            tags = tags.Replace(selectedTag + SEPARATOR, "");
            tags = tags.Replace(selectedTag, "");
            tags = tags.Trim(SEPARATOR);
            return tags;
        }

        public string AddTag(string selectedTag, string fileUrl)
        {
            string tags = GetTags(fileUrl);

            tags = tags.Trim(SEPARATOR);

            if (tags.Length <= 0)
            {
                return selectedTag;
            }
            else
            {
                return tags + SEPARATOR + selectedTag;
            }

        }
        public bool IsTagAvailable(string tagName, string fileUrl)
        {
            string tags = GetTags(fileUrl);
            if (tags.Contains(tagName + SEPARATOR) || tags.EndsWith(tagName))
            {
                return true;
            }

            return false;
        }

        public string GetTags(string fileUrl)
        {
            string[] tags = ReadTagsFromFile(fileUrl);
            return String.Join(TagsStorage.SEPARATOR.ToString(), tags).Trim();
        }

        public string GetTagName()
        {
            return metaDataField.ToString("g");
        }

        public void SetTagsInFile(string[] fileUrls, CheckState selected, string selectedTag, TagsManipulation tagsManipulation)
        {
            foreach (string fileUrl in fileUrls)
            {
                string tagsFromFile;
                if (selected == CheckState.Checked)
                {
                    tagsFromFile = AddTag(selectedTag, fileUrl);
                }
                else
                {
                    tagsFromFile = RemoveTag(selectedTag, fileUrl);
                }

                string sortedTags = tagsManipulation.SortTagsAlphabetical(tagsFromFile);
                bool result = musicBeeApiInterface.Library_SetFileTag(fileUrl, metaDataField, sortedTags);
                musicBeeApiInterface.Library_CommitTagsToFile(fileUrl);

            }
            musicBeeApiInterface.MB_SetBackgroundTaskMessage("Added tags to file");
        }
    }
}


