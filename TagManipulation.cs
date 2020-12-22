using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    class TagManipulation
    {

        private const string SettingsFileName = "mb_tags-panel.Settings.xml";

        private readonly MusicBeeApiInterface mbApiInterface;

        string[] allTagsFromConfig = null;

        public TagManipulation(MusicBeeApiInterface mbApiInterface)
        {
            this.mbApiInterface = mbApiInterface;
        }

        public void LoadOccasionsWithDefaultFallback()
        {
            LoadSettings();

            if (SavedSettings.occasions != null && SavedSettings.occasions.Length > 0)
            {
                // put 
                allTagsFromConfig = SavedSettings.occasions.Split(',');
            }
            else
            {
                allTagsFromConfig = new string[] { };
            }
            //temp_occasions = allTagsFromConfig;
        }

        private void LoadSettings()
        {
            string filename = System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), SettingsFileName);

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
                Console.WriteLine(e.Message);
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
    }
}
