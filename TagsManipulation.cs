using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class TagsManipulation
    {

        public const char SEPARATOR = ';';
        private readonly MusicBeeApiInterface mbApiInterface;

        public TagsManipulation(MusicBeeApiInterface mbApiInterface)
        {
            this.mbApiInterface = mbApiInterface;
        }

        public Dictionary<String, CheckState> CombineTagLists(string[] fileNames, TagsStorage tagsStorage)
        {
            Dictionary<String, CheckState> tagList = new Dictionary<String, CheckState>();
            Dictionary<String, int> stateOfSelection = new Dictionary<String, int>();
            int numberOfSelectedFiles = fileNames.Length;

            foreach (var filename in fileNames)
            {
                string[] tagsFromFile = ReadTagsFromFile(filename, tagsStorage.GetMetaDataType());
                foreach (var tag in tagsFromFile)
                {
                    if (stateOfSelection.ContainsKey(tag))
                    {
                        int count = stateOfSelection[tag];
                        stateOfSelection[tag] = count++;
                    }
                    else
                    {
                        stateOfSelection.Add(tag, 1);
                    }
                }
            }

            foreach (KeyValuePair<String, int> entry in stateOfSelection)
            {
                if (entry.Value == numberOfSelectedFiles)
                {
                    tagList.Add(entry.Key, CheckState.Checked);
                }
                else
                {
                    tagList.Add(entry.Key, CheckState.Indeterminate);
                }
            }

            return tagList;
        }

        public string SortTagsAlphabetical(string tags)
        {
            string[] tagsAsArray = tags.Split(SEPARATOR);
            Array.Sort(tagsAsArray);
            return String.Join(SEPARATOR.ToString(), tagsAsArray);
        }
        public string RemoveTag(string selectedTag, string fileUrl, MetaDataType metaDataType)
        {
            string tags = GetTags(fileUrl, metaDataType);
            tags = tags.Replace(selectedTag + SEPARATOR, "");
            tags = tags.Replace(selectedTag, "");
            tags = tags.Trim(SEPARATOR);
            return tags;
        }

        public string AddTag(string selectedTag, string fileUrl, MetaDataType metaDataType)
        {
            string tags = GetTags(fileUrl, metaDataType);

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
        public bool IsTagAvailable(string tagName, string fileUrl, MetaDataType metaDataType)
        {
            string tags = GetTags(fileUrl, metaDataType);
            if (tags.Contains(tagName + SEPARATOR) || tags.EndsWith(tagName))
            {
                return true;
            }

            return false;
        }

        public string GetTags(string fileUrl, MetaDataType metaDataType)
        {
            string[] tags = ReadTagsFromFile(fileUrl, metaDataType);
            return String.Join(SEPARATOR.ToString(), tags).Trim();
        }

        public void SetTagsInFile(string[] fileUrls, CheckState selected, string selectedTag, MetaDataType metaDataType)
        {
            foreach (string fileUrl in fileUrls)
            {
                string tagsFromFile;
                if (selected == CheckState.Checked)
                {
                    tagsFromFile = AddTag(selectedTag, fileUrl, metaDataType);
                }
                else
                {
                    tagsFromFile = RemoveTag(selectedTag, fileUrl, metaDataType);
                }

                string sortedTags = SortTagsAlphabetical(tagsFromFile);
                bool result = mbApiInterface.Library_SetFileTag(fileUrl, metaDataType, sortedTags);
                mbApiInterface.Library_CommitTagsToFile(fileUrl);

            }
            mbApiInterface.MB_SetBackgroundTaskMessage("Added tags to file");
        }

        public string[] ReadTagsFromFile(string filename, MetaDataType metaDataField)
        {
            HashSet<string> tags = new HashSet<string>();

            if (filename == null || filename.Length <= 0 || 0 == metaDataField)
            {
                return tags.ToArray<string>();
            }

            string filetagMetaDataFields = mbApiInterface.Library_GetFileTag(filename, metaDataField);
            string[] filetagMetaDataFieldsParts = filetagMetaDataFields.Split(SEPARATOR);
            foreach (string tag in filetagMetaDataFieldsParts)
            {
                if (tag.Trim().Length <= 0)
                {
                    continue;
                }
                tags.Add(tag.Trim());
            }

            return tags.ToArray<string>();
        }

        public Dictionary<string, CheckState> UpdateTagsFromFile(string sourceFileUrl, MetaDataType metaDataType)
        {
            string[] tagParts = ReadTagsFromFile(sourceFileUrl, metaDataType);

            Dictionary<string, CheckState> tagsFromFile = new Dictionary<string, CheckState>();
            foreach (string tag in tagParts)
            {
               tagsFromFile[tag] = CheckState.Checked;
            }

            return tagsFromFile;
        }
    }
}
