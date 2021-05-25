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

        private static Dictionary<String, TagsStorage> storages;

        private const string SettingsFileName = "mb_tags-panel.Settings.json";

        private readonly MusicBeeApiInterface mbApiInterface;

        private readonly Logger log;

        public static Dictionary<string, TagsStorage> TagsStorages { get => storages; set => storages = value; }

        public SettingsStorage(MusicBeeApiInterface mbApiInterface, Logger log)
        {
            this.mbApiInterface = mbApiInterface;
            this.log = log;
            SettingsStorage.TagsStorages = new Dictionary<String, TagsStorage>();
        }

        public void LoadSettingsWithFallback()
        {
            LoadSettings();

            if (null == SettingsStorage.TagsStorages)
            {
                SettingsStorage.TagsStorages = new Dictionary<String, TagsStorage>();
                TagsStorage tagsStorage = new TagsStorage();
                // TODO: Fix Mood Tab always being created
                tagsStorage.MetaDataType = MetaDataType.Mood.ToString("g");
                
                // TODO: Fix exception when no mb_tags_panel_settings.json is found
                SettingsStorage.TagsStorages.Add(tagsStorage.GetTagName(), tagsStorage);
            }

            Dictionary<String, TagsStorage> storageList = new Dictionary<String, TagsStorage>();

            foreach (KeyValuePair<String, TagsStorage> storage in SettingsStorage.TagsStorages)
            {
                storage.Value.SortByIndex();
                storageList.Add(storage.Value.GetTagName(), storage.Value);
            }

            SettingsStorage.TagsStorages = storageList;
        }

        private void LoadSettings()
        {
            string filename = GetSettingsPath();

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(filename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            
            using (System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode))
            {
                JsonSerializer serializer = new JsonSerializer();
                SettingsStorage.TagsStorages = (Dictionary<string, TagsStorage>) serializer.Deserialize(file, typeof(Dictionary<string, TagsStorage>));
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
                serializer.Serialize(file, SettingsStorage.TagsStorages);
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

        public static TagsStorage GetTagsStorage(string tagName)
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
            // TODO fix exception that occurs when starting MusicBee when no tag page is set
            return TagsStorages.Values.FirstOrDefault();
        }

        public void RemoveTagStorage(string tagName)
        {
            TagsStorages.Remove(tagName);
        }

        public SettingsStorage DeepCopy()
        {
            SettingsStorage other = (SettingsStorage)this.MemberwiseClone();
            storages = JsonConvert.DeserializeObject<Dictionary<String, TagsStorage>>(JsonConvert.SerializeObject(TagsStorages));
            return other;
        }
    }
}
