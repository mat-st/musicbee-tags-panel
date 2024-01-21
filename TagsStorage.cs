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

        public void Clear() => tagList.Clear();

        public Dictionary<string, CheckState> GetTags()
        {
            return tagList.ToDictionary(item => item.Key, _ => CheckState.Unchecked);
        }

        public string GetTagName() => metaDataType;

        public MetaDataType GetMetaDataType()
        {
            MetaDataType result;
            Enum.TryParse(metaDataType, true, out result);
            return result;
        }

        public void Sort()
        {
            var sortedKeys = new SortedSet<string>(tagList.Keys);
            tagList = sortedKeys.Select((key, index) => new { Key = key, Index = index })
                               .ToDictionary(pair => pair.Key, pair => pair.Index);
            sorted = true;
        }

        public void SortByIndex()
        {
            var tmpTagList = tagList.OrderBy(item => item.Value);
            tagList = tmpTagList.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void SwapElement(string key, int position)
        {
            int oldPosition = tagList[key];
            tagList[key] = position;

            string oldKey = tagList.FirstOrDefault(item => item.Value == position).Key;
            tagList[oldKey] = oldPosition;
        }

        public bool Sorted { get => sorted; set => sorted = value; }
        public string MetaDataType { get => metaDataType; set => metaDataType = value; }
        public Dictionary<string, int> TagList { get => tagList; set => tagList = value; }
    }
}
