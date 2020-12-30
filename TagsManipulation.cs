using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class TagsManipulation
    {
        public Dictionary<String, CheckState> CombineTagLists(string[] fileNames, TagsStorage tagsStorage)
        {
            Dictionary<String, CheckState> tagList = new Dictionary<String, CheckState>();
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
            string[] tagsAsArray = tags.Split(TagsStorage.SEPARATOR);
            Array.Sort(tagsAsArray);
            return String.Join(TagsStorage.SEPARATOR.ToString(), tagsAsArray);
        }
    }
}
