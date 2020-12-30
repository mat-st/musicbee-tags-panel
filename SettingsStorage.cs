using System;
using System.Text;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public class SavedSettingsType
    {
        public string tags;
        public bool sorted = true;
    }

    public class SettingsStorage
    {
        public const char SettingsSeparator = ';';

        private static SavedSettingsType SavedSettings = new SavedSettingsType
        {
            tags = ""
        };

        private const string SettingsFileName = "mb_tags-panel.Settings.xml";

        private readonly MusicBeeApiInterface mbApiInterface;

        private readonly Logger log;

        private string[] allTagsFromConfig = null;

        public SettingsStorage(MusicBeeApiInterface mbApiInterface, Logger log)
        {
            this.mbApiInterface = mbApiInterface;
            this.log = log;
        }

        public void LoadSettingsWithFallback()
        {
            LoadSettings();

            if (SavedSettings.tags != null && SavedSettings.tags.Length > 0)
            {
                // put 
                allTagsFromConfig = SavedSettings.tags.Split(SettingsSeparator);
            }
            else
            {
                allTagsFromConfig = new string[] { };
            }
        }

        private void LoadSettings()
        {
            string filename = GetSettingsPath();

            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(filename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = null;
            try
            {
                controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }

            try
            {
                SavedSettings = (SavedSettingsType)controlsDefaultsSerializer.Deserialize(file);
            }
            catch
            {
                // Ignore ;) 
            };

            file.Close();
        }

        public string GetSettingsPath()
        {
            return System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), SettingsFileName);
        }

        public void SaveSettings(bool tempSortEnabled, string[] tempTags)
        {
            SavedSettings.sorted = tempSortEnabled;
            SavedSettings.tags = String.Join(SettingsSeparator.ToString(), tempTags);

            // save any persistent settings in a sub-folder of this path
            string settingsPath = GetSettingsPath();
            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(settingsPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));

            controlsDefaultsSerializer.Serialize(file, SavedSettings);

            file.Close();

            allTagsFromConfig = tempTags;
        }

        public string[] GetAllTagsFromConfig()
        {
            return allTagsFromConfig;
        }

        public SavedSettingsType GetSavedSettings()
        {
            return SavedSettings;
        }
    }
}
