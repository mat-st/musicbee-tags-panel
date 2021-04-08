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
        private string metaDataType;
        private Dictionary<String, int> tagList = new Dictionary<String, int>();
        private bool sorted = true;

        public TagsStorage()
        {
        }

        public void Clear()
        {
            tagList.Clear();
        }


        internal Dictionary<String, CheckState> GetTags()
        {
            Dictionary<String, CheckState> result = new Dictionary<String, CheckState>();

            foreach (var item in tagList)
            {
                result.Add(item.Key, CheckState.Unchecked);
            }

            return result;
        }

     
        public string GetTagName()
        {
            return this.metaDataType;
        }

        public MetaDataType GetMetaDataType()
        {
            return (MetaDataType)Enum.Parse(typeof(MetaDataType), this.metaDataType, true);
        }

        public void Sort()
        {
            SortedSet<String> sortedKeys = new SortedSet<String>(tagList.Keys);
            tagList.Clear();

            int idx = 0;
            foreach (var key in sortedKeys)
            {
                tagList.Add(key, idx++);
            }

            sorted = true;
        }

        /// <summary>
        /// Swaps the element at the old position with the new one.
        /// </summary>
        /// <param name="key">String with the existent key.</param>
        /// <param name="position">The new position.</param>
        public void SwapElement(String key, int position)
        {
            int oldPosition = tagList[key];
            tagList[key] = position;

            tagList[FilterPosition(tagList, position)] = oldPosition;
        }

        private String FilterPosition(Dictionary<String, int> list, int value)
        {
            String newKey = "";
            foreach (var item in list)
            {
                if (item.Value == value)
                {
                    newKey = item.Key;
                    break;
                }
            }
            return newKey;
        }
                
        public bool Sorted { get => sorted; set => sorted = value; }
        public string MetaDataType { get => metaDataType; set => metaDataType = value; }
        public Dictionary<string, int> TagList { get => tagList; set => tagList = value; }
    }
}


