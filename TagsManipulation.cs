using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    class TagsManipulation
    {
        public Dictionary<String, CheckState> combineTagLists(string[] fileNames, TagsStorage tagsStorage)
        {
            Dictionary<String, CheckState> occasionList = new Dictionary<String, CheckState>();
            Dictionary<String, int> stateOfSelection = new Dictionary<String, int>();
            int numberOfSelectedFiles = fileNames.Length;

            foreach (var filename in fileNames)
            {
                string[] tagsFromSettings = tagsStorage.ReadTagsFromFile(filename);
                foreach (var tag in tagsFromSettings)
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
                    occasionList.Add(entry.Key, CheckState.Checked);
                }
                else
                {
                    occasionList.Add(entry.Key, CheckState.Indeterminate);
                }
            }

            return occasionList;
        }

        public string GetTags(string fileUrl, TagsStorage tagsStorage)
        {
            string[] tags = tagsStorage.ReadTagsFromFile(fileUrl);
            return String.Join(TagsStorage.SEPARATOR.ToString(), tags).Trim();
        }
    }
}
