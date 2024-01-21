using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class SavedSettingsType
    {
        public Dictionary<string, TagsStorage> TagStorages { get; set; }
    }

    public class SettingsStorage
    {
        private const char SettingsSeparator = ';';
        private static Dictionary<string, TagsStorage> storages;
        private const string SettingsFileName = "mb_tags-panel.Settings.json";
        private readonly MusicBeeApiInterface mbApiInterface;
        private readonly Logger log;

        public static Dictionary<string, TagsStorage> TagsStorages
        {
            get => storages;
            set => storages = value;
        }

        public SettingsStorage(MusicBeeApiInterface mbApiInterface, Logger log)
        {
            this.mbApiInterface = mbApiInterface;
            this.log = log;
            TagsStorages = new Dictionary<string, TagsStorage>();
        }

        public void LoadSettingsWithFallback()
        {
            LoadSettings();

            if (TagsStorages == null)
                return;

            var storageList = TagsStorages
                .Select(storage => new KeyValuePair<string, TagsStorage>(storage.Value.GetTagName(), storage.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            TagsStorages = storageList;
        }

        private void LoadSettings()
        {
            string filename = GetSettingsPath();

            Encoding unicode = Encoding.UTF8;
            using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                using (var file = new StreamReader(stream, unicode))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    TagsStorages = (Dictionary<string, TagsStorage>)serializer.Deserialize(file, typeof(Dictionary<string, TagsStorage>));
                }
            }
        }

        public string GetSettingsPath()
        {
            return Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), SettingsFileName);
        }

        public void SaveAllSettings()
        {
            string settingsPath = GetSettingsPath();
            Encoding unicode = Encoding.UTF8;

            using (var stream = File.Open(settingsPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var file = new StreamWriter(stream, unicode))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, TagsStorages);
                }
            }
        }

        public TagsStorage GetFirstTagsStorage()
        {
            return TagsStorages.Count > 0 ? TagsStorages.Values.First() : null;
        }

        public static TagsStorage GetTagsStorage(string tagName)
        {
            if (!TagsStorages.TryGetValue(tagName, out TagsStorage tagStorage))
            {
                tagStorage = new TagsStorage
                {
                    MetaDataType = tagName
                };
                TagsStorages.Add(tagName, tagStorage);
            }

            return tagStorage;
        }

        public void SetTagsStorage(TagsStorage tagsStorage)
        {
            TagsStorages[tagsStorage.GetTagName()] = tagsStorage;
        }

        public TagsStorage GetFirstOne()
        {
            return TagsStorages.Values.FirstOrDefault();
        }

        public void RemoveTagStorage(string tagName)
        {
            TagsStorages.Remove(tagName);
        }

        public SettingsStorage DeepCopy()
        {
            SettingsStorage other = (SettingsStorage)this.MemberwiseClone();
            storages = JsonConvert.DeserializeObject<Dictionary<string, TagsStorage>>(JsonConvert.SerializeObject(TagsStorages));
            return other;
        }
    }
}
