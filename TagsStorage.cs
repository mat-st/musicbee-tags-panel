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
        private Dictionary<String, CheckState> tagList = new Dictionary<String, CheckState>();
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
            return tagList;
        }

        internal void SetTags(Dictionary<String, CheckState> tagList)
        {
            this.tagList = tagList;
        }

       
     

        public string GetTagName()
        {
            return this.metaDataType;
        }

        public MetaDataType GetMetaDataType()
        {
            return (MetaDataType)Enum.Parse(typeof(MetaDataType), this.metaDataType, true);
        }

        
        public bool Sorted { get => sorted; set => sorted = value; }
        public string MetaDataType { get => metaDataType; set => metaDataType = value; }
        public Dictionary<string, CheckState> TagList { get => tagList; set => tagList = value; }
     
    }
}


