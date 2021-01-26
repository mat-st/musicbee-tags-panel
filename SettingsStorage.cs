using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class SavedSettingsType
    {
        public Dictionary<string, TagsStorage> tagStorages;
    }

    public class SettingsStorage
    {
        public const char SettingsSeparator = ';';

        private Dictionary<String, TagsStorage> storages;

        private const string SettingsFileName = "mb_tags-panel.Settings.json";

        private readonly MusicBeeApiInterface mbApiInterface;

        private readonly Logger log;

        public Dictionary<string, TagsStorage> TagsStorages { get => storages; set => storages = value; }

        public SettingsStorage(MusicBeeApiInterface mbApiInterface, Logger log)
        {
            this.mbApiInterface = mbApiInterface;
            this.log = log;
            this.TagsStorages = new Dictionary<String, TagsStorage>();
        }

        public void LoadSettingsWithFallback()
        {
            LoadSettings();

            if (null == TagsStorages)
            {
                this.TagsStorages = new Dictionary<String, TagsStorage>();
                TagsStorage tagsStorage = new TagsStorage();
                tagsStorage.MetaDataType = MetaDataType.Mood.ToString("g");
                this.TagsStorages.Add(tagsStorage.GetTagName(), tagsStorage);
            }
        }

        private void LoadSettings()
        {
            string filename = GetSettingsPath();

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(filename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            
            using (System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode))
            {
                JsonSerializer serializer = new JsonSerializer();
                TagsStorages = (Dictionary<string, TagsStorage>) serializer.Deserialize(file, typeof(Dictionary<string, TagsStorage>));
                file.Close();
            }
        }

        public string GetSettingsPath()
        {
            return System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), SettingsFileName);
        }

        public void SaveAllSettings()
        {
            // save any persistent settings in a sub-folder of this path
            string settingsPath = GetSettingsPath();
            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(settingsPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, TagsStorages);
                file.Close();
            }
        }

        public TagsStorage GetFirstTagsStorage()
        {
            if (TagsStorages.Count <= 0)
            {
                return null;
            }

            var e = TagsStorages.GetEnumerator();
            e.MoveNext();
            return e.Current.Value;
        }

        public TagsStorage GetTagsStorage(string tagName)
        {
            TagsStorage tagStorage;
            if (false == TagsStorages.TryGetValue(tagName, out tagStorage))
            {
                tagStorage = new TagsStorage();
                tagStorage.MetaDataType = tagName;
                TagsStorages.Add(tagName, tagStorage);
            }
           
            return tagStorage;
        }

        public void SetTagsStorage(TagsStorage tagsStorage)
        {
            TagsStorages.Remove(tagsStorage.GetTagName());
            TagsStorages.Add(tagsStorage.GetTagName(), tagsStorage);
            
        }

        public TagsStorage GetFirstOne()
        {
            return TagsStorages.Values.First();
        }

        public void RemoveTagStorage(string tagName)
        {
            TagsStorages.Remove(tagName);
        }
    }
}
