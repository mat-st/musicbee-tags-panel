using Newtonsoft.Json;
using System;
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

        private static SavedSettingsType savedSettings = new SavedSettingsType
        {
            tagStorages = new Dictionary<string, TagsStorage>()
        };

        private const string SettingsFileName = "mb_tags-panel.Settings.xml";

        private readonly MusicBeeApiInterface mbApiInterface;

        private readonly Logger log;

        public SettingsStorage(MusicBeeApiInterface mbApiInterface, Logger log)
        {
            this.mbApiInterface = mbApiInterface;
            this.log = log;
        }

        public void LoadSettingsWithFallback()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            string filename = GetSettingsPath();

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(filename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            
            using (System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode))
            {
                JsonSerializer serializer = new JsonSerializer();
                savedSettings = (SavedSettingsType) serializer.Deserialize(file, typeof(SavedSettingsType));
                file.Close();
            }
        }

        public string GetSettingsPath()
        {
            return System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), SettingsFileName);
        }

        public void SaveSettings(TagsStorage tagsStorage)
        {
            SetTagsStorage(tagsStorage);

            // save any persistent settings in a sub-folder of this path
            string settingsPath = GetSettingsPath();
            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(settingsPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, savedSettings);
                file.Close();
            }
        }
        public TagsStorage GetFirstTagsStorage()
        {
            if (savedSettings.tagStorages.Count <= 0)
            {
                return null;
            }

            var e = savedSettings.tagStorages.GetEnumerator();
            e.MoveNext();
            return e.Current.Value;
        }

        public TagsStorage GetAllTagsFromConfig(string tagName)
        {
            TagsStorage tagStorage;
            if (false == savedSettings.tagStorages.TryGetValue(tagName, out tagStorage))
            {
                MetaDataType metaDataType = (MetaDataType) Enum.Parse(typeof(MetaDataType), tagName);
                tagStorage = new TagsStorage(mbApiInterface, metaDataType);
                savedSettings.tagStorages.Add(tagName, tagStorage);
            }
           
            return tagStorage;
        }

        public void SetTagsStorage(TagsStorage tagsStorage)
        {
            savedSettings.tagStorages.Remove(tagsStorage.GetTagName());
            savedSettings.tagStorages.Add(tagsStorage.GetTagName(), tagsStorage);
        }

        public SavedSettingsType GetSavedSettings()
        {
            return savedSettings;
        }
    }
}
