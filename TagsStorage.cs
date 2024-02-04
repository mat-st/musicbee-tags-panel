using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class TagsStorage
    {
        private string metaDataType;
        private Dictionary<string, int> tagList = new Dictionary<string, int>();
        private bool sorted = true;

        public void Clear()
        {
            tagList.Clear();
        }

        public Dictionary<string, CheckState> GetTags()
        {
            Dictionary<string, CheckState> result = new Dictionary<string, CheckState>(tagList.Count);
            foreach (var item in tagList)
            {
                result.Add(item.Key, CheckState.Unchecked);
            }
            return result;
        }

        public string GetTagName()
        {
            return metaDataType;
        }

        public MetaDataType GetMetaDataType()
        {
            MetaDataType result;
            Enum.TryParse(metaDataType, true, out result);
            return result;
        }

        public void Sort()
        {
            var sortedKeys = new SortedSet<string>(tagList.Keys);
            Dictionary<string, int> sortedTagList = new Dictionary<string, int>(tagList.Count);
            int index = 0;
            foreach (var key in sortedKeys)
            {
                sortedTagList.Add(key, index);
                index++;
            }
            tagList = sortedTagList;
            sorted = true;
        }

        public void SortByIndex()
        {
            List<KeyValuePair<string, int>> sortedTagList = tagList.OrderBy(item => item.Value).ToList();
            Dictionary<string, int> newTagList = new Dictionary<string, int>(tagList.Count);
            for (int i = 0; i < sortedTagList.Count; i++)
            {
                newTagList.Add(sortedTagList[i].Key, i);
            }
            tagList = newTagList;
        }

        public void SwapElement(string key, int position)
        {
            int oldPosition = tagList[key];
            tagList[key] = position;

            foreach (var item in tagList)
            {
                if (item.Value == position && item.Key != key)
                {
                    tagList[item.Key] = oldPosition;
                    break;
                }
            }
        }

        public bool Sorted
        {
            get { return sorted; }
            set { sorted = value; }
        }

        public string MetaDataType
        {
            get { return metaDataType; }
            set { metaDataType = value; }
        }

        public Dictionary<string, int> TagList
        {
            get { return tagList; }
            set { tagList = value; }
        }
    }
}
