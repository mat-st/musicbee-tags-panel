using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private const string LOG_FILE_NAME = "mb_tags-panel.log";
        private const string SettingsFileName = "mb_tags-panel.Settings.xml";

        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();
        private Dictionary<String, CheckState> occasionList = new Dictionary<String, CheckState>();
        public static SavedSettingsType SavedSettings = new SavedSettingsType
        {
            occasions = ""
        };
        string[] allTagsFromConfig = null;
        private string[] temp_occasions;
        private bool sortEnabled;
        private string[] selectedFileUrls = new string[] { };
        private Logger log;

        private Control ourPanel;
        private TabControl tabControl;
        private ChecklistBoxPanel checklistBox;

        private bool listUpdate;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "tags-panel";
            about.Description = "Creates a dockable Panel which lets the user choose from an predefined " +
                "list of occasions";
            about.Author = "Matthias Steiert + The Anonymous Programmer";
            about.TargetApplication = "tags-panel";   //  the name of a Plugin Storage device or panel header for a dockable panel
            about.Type = PluginType.General;
            about.VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;  // your plugin version
            about.VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents | ReceiveNotificationFlags.DataStreamEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            //createMenuItem();
            // Application.EnableVisualStyles();
            LoadOccasionsWithDefaultFallback();

            InitLogger();
            log.info("Tagger plugin started");

            return about;
        }

        private void InitLogger()
        {
            string logPath = System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), LOG_FILE_NAME);
            log = new Logger(logPath);
        }

        public bool Configure(IntPtr panelHandle)
        {

            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window

            fvSettings tagToolsForm = new fvSettings(allTagsFromConfig, this.sortEnabled);
            tagToolsForm.ShowDialog();
            temp_occasions = tagToolsForm.getOccasions();
            sortEnabled = tagToolsForm.isSortEnabled();

            return true;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            SavedSettings.sorted = this.sortEnabled;
            allTagsFromConfig = temp_occasions;
            SavedSettings.occasions = String.Join(",", allTagsFromConfig);
            if (ourPanel != null)
            {
                updateOccasionTableData(ourPanel);
            }
            

            // save any persistent settings in a sub-folder of this path
            string settingsPath = System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), SettingsFileName);
            Encoding unicode = Encoding.UTF8;
            System.IO.FileStream stream = System.IO.File.Open(settingsPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));

            controlsDefaultsSerializer.Serialize(file, SavedSettings);

            file.Close();
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

        private void LoadOccasionsWithDefaultFallback()
        {
            LoadSettings();

            if (SavedSettings.occasions != null && SavedSettings.occasions.Length > 0)
            {
                // put 
                allTagsFromConfig = SavedSettings.occasions.Split(',');
                this.sortEnabled = SavedSettings.sorted;
            }
            else
            {
                allTagsFromConfig = new string[] { };
                this.sortEnabled = true;
            }
            temp_occasions = allTagsFromConfig;
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            //ourPanel.Dispose();
            ourPanel = null;
            log.close();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            //Delete settings file
            if (System.IO.File.Exists(SettingsFileName))
            {
                System.IO.File.Delete(SettingsFileName);
            }
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            if (ourPanel == null)
            {
                return;
            }
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    readFileTagList(mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Occasion));
                    break;
                case NotificationType.TrackChanged:
                    readFileTagList(mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Occasion));
                    updateOccasionTableData(ourPanel);
                    ourPanel.Invalidate();
                    break;
                case NotificationType.TagsChanged:
                    readFileTagList(mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Occasion));
                    ourPanel.Invalidate();
                    break;
            }
        }

        private void readFileTagList(string sourceFileUrl)
        {
            this.listUpdate = true;
            occasionList.Clear();

            if (sourceFileUrl == null || sourceFileUrl.Length <= 0)
            {
                return;
            }

            string filetagOccasions = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Occasion);
            string[] filetagOccasionsParts = filetagOccasions.Split(';');
            foreach (string occasion in filetagOccasionsParts)
            {
                if (occasion.Trim().Length <= 0)
                {
                    continue;
                }
                occasionList.Add(occasion, CheckState.Checked);
            }
            this.listUpdate = false;
        }

        //private void createMenuItem() {

        //mbApiInterface.MB_AddMenuItem("mnuTools/Tagtraum", "HotKey For Tagtraum", menuClicked);
        //mbApiInterface.MB_AddMenuItem("mnuTools/Tagtraum/Einstellungen", "HotKey For Tagdream", menuClickedn);
        //}
        //private void menuClicked(object sender, EventArgs args) {Form1 myForm = new Form1(mbApiInterface); myForm.Show();}
        //private void menuClickedn(object sender, EventArgs args) { Form2 myForm2 = new Form2(mbApiInterface); myForm2.Show(); }



        // return an array of lyric or artwork provider names this plugin supports
        // the providers will be iterated through one by one and passed to the RetrieveLyrics/ RetrieveArtwork function in order set by the user in the MusicBee Tags(2) preferences screen until a match is found
        //public string[] GetProviders()
        //{
        //    return null;
        //}

        // return lyrics for the requested artist/title from the requested provider
        // only required if PluginType = LyricsRetrieval
        // return null if no lyrics are found
        //public string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle, string album, bool synchronisedPreferred, string provider)
        //{
        //    return null;
        //}

        // return Base64 string representation of the artwork binary data from the requested provider
        // only required if PluginType = ArtworkRetrieval
        // return null if no artwork is found
        //public string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album, string provider)
        //{
        //    //Return Convert.ToBase64String(artworkBinaryData)
        //    return null;
        //}

        //  presence of this function indicates to MusicBee that this plugin has a dockable panel. MusicBee will create the control and pass it as the panel parameter
        //  you can add your own controls to the panel if needed
        //  you can control the scrollable area of the panel using the mbApiInterface.MB_SetPanelScrollableArea function
        //  to set a MusicBee header for the panel, set about.TargetApplication in the Initialise function above to the panel header text
        public int OnDockablePanelCreated(Control panel)
        {
            //    return the height of the panel and perform any initialisation here
            //    MusicBee will call panel.Dispose() when the user removes this panel from the layout configuration
            //    < 0 indicates to MusicBee this control is resizable and should be sized to fill the panel it is docked to in MusicBee
            //    = 0 indicates to MusicBee this control resizeable
            //    > 0 indicates to MusicBee the fixed height for the control.Note it is recommended you scale the height for high DPI screens(create a graphics object and get the DpiY value)

            ourPanel = panel;
            AddControls(ourPanel);
            updateOccasionTableData(ourPanel);

            return -1;
        }

        private void setPanelEnabled(bool enabled = true)
        {
            if (ourPanel.IsHandleCreated)
            {
                ourPanel.Invoke(new Action(() =>
                {
                    ourPanel.Enabled = enabled;
                }));
            }
            else
            {
                ourPanel.Enabled = enabled;
            }
        }
        public void OnSelectedFilesChanged(string[] filenames)
        {
            if (ourPanel == null)
            {
                return;
            }
            this.listUpdate = true; 
            occasionList.Clear();
            if (filenames == null || filenames.Length < 0)
            {
                setPanelEnabled(false);
                return;
            }
            // important to have as a global variable
            selectedFileUrls = filenames;

            setPanelEnabled(true);
            Dictionary<String, int> stateOfSelection = new Dictionary<String, int>();
            int numberOfSelectedFiles = filenames.Length;
            foreach (var filename in filenames)
            {
                string[] occasions = GetTagsFromFile(filename);
                foreach (var occasion in occasions)
                {
                    if (stateOfSelection.ContainsKey(occasion))
                    {
                        int count = stateOfSelection[occasion];
                        stateOfSelection[occasion] = count++;
                    } else
                    {
                        stateOfSelection.Add(occasion, 1);
                    }
                }
            }

            foreach (KeyValuePair<String, int> entry in stateOfSelection)
            {
                if (entry.Value == numberOfSelectedFiles)
                {
                    occasionList.Add(entry.Key, CheckState.Checked);
                } else
                {
                    occasionList.Add(entry.Key, CheckState.Indeterminate);
                }
            }
            //fileTagMoods = String.Join(";", allMoods);

            updateOccasionTableData(ourPanel);
            ourPanel.Invalidate();
            this.listUpdate = false;
        }

        private string[] GetTagsFromFile(string filename)
        {
            HashSet<string> tags = new HashSet<string>();

            string filetagOccasions = mbApiInterface.Library_GetFileTag(filename, MetaDataType.Occasion);
            string[] filetagOccasionssParts = filetagOccasions.Split(';');
            foreach (string occasion in filetagOccasionssParts)
            {
                if (occasion.Trim().Length <= 0)
                {
                    continue;
                }
                tags.Add(occasion.Trim());
            }

            return tags.ToArray<string>();
        }

        private void updateOccasionTableData(Control panel, Dictionary<String, CheckState> allOccasions = null)
        {
            /*
            if (moodList.Count() > 0)
            {
                // return;
            }

            // array with moods from filetag
            string[] filetagMoodParts = fileTagMoods.Split(';');
            Collection<MoodEntry> data = new Collection<MoodEntry>();
            
            if (moodList != null)
            {
            */
            bool add = true;
            Dictionary<String, CheckState> data = new Dictionary<String, CheckState>();
            foreach (string tagFromConfig in allTagsFromConfig)
            {
                foreach (String occasionEntry in occasionList.Keys)
                {
                    if (tagFromConfig.Trim() == occasionEntry.Trim())
                    {
                        data.Add(occasionEntry, occasionList[occasionEntry]);
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    data.Add(tagFromConfig, CheckState.Unchecked);
                }
                add = true;
            }

            if (panel.IsHandleCreated)
            {
                panel.Invoke(new Action(() =>
                {
                    this.checklistBox.AddDataSource(data);
                }));
            }
            else
            {
                this.checklistBox.AddDataSource(data);
            }

        }

        private void AddControls(Control _panel)
        {
            if (_panel == null)
            {
                return;
            }

            if (_panel.IsHandleCreated)
            {
                _panel.Invoke((MethodInvoker)delegate
                {
                    layoutPanel(_panel);
                });
            }
            else
            {
                layoutPanel(_panel);
            }
        }

        private void layoutPanel(Control _panel)
        {
            createTabbedPanel();

            _panel.Enabled = false;
            _panel.SuspendLayout();
            _panel.Controls.AddRange(new Control[]
            {
                   this.tabControl
            });
            _panel.ResumeLayout();
            
        }

        private void createTabbedPanel()
        {
            this.tabControl = (TabControl)mbApiInterface.MB_AddPanel(null, (PluginPanelDock) 6);
            this.tabControl.Dock = DockStyle.Fill;

            TabPage page1 = new TabPage("Moods");
            this.checklistBox = new ChecklistBoxPanel(mbApiInterface, this.occasionList);
            checklistBox.Dock = DockStyle.Fill;
            checklistBox.AddItemCheckEventHandler(new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox1_ItemCheck));
            page1.Controls.Add(checklistBox);
            this.tabControl.TabPages.Add(page1);
            TabPage page2 = new TabPage("Occassions");
            this.tabControl.TabPages.Add(page2);
            TabPage page3 = new TabPage("Genres");
            this.tabControl.TabPages.Add(page3);

        }


        // presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked
        // return the list of ToolStripMenuItems that will be displayed
        //public List<ToolStripItem> GetHeaderMenuItems()
        //{
        //    List<ToolStripItem> list = new List<ToolStripItem>();
        //    list.Add(new ToolStripMenuItem("A menu item"));
        //    return list;
        //}

        public class SavedSettingsType
        {
            public string occasions;
            public bool sorted = true;
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.listUpdate)
            {
                return;
            }

            int index = e.Index;
            CheckState state = e.NewValue;
            string name = ((CheckedListBox)sender).Items[index].ToString();
            //CheckedListBox.CheckedItemCollection checkedItems = ((CheckedListBox)sender).CheckedItems[selectedIndex];
            SetTagsInPanel(this.selectedFileUrls, state, name);

            //((System.Windows.Forms.DataGridView)sender).EndEdit();
            if (ourPanel != null)
            {
                ourPanel.Invalidate();
            }
            mbApiInterface.MB_RefreshPanels();
        }

        private void SetTagsInPanel(string[] fileUrls, CheckState selected, string selectedTag)
        {
            foreach (string fileUrl in fileUrls)
            {
                string tagsFromFile;
                if (selected == CheckState.Checked)
                {
                    tagsFromFile = AddTag(selectedTag, fileUrl);
                    //row.Cells[0].Value = 1;
                }
                /*else if (selected == CheckState.Indeterminate)
                {
                    tagsFromFile = RemoveTag(selectedTag, fileUrl);
                    //row.Cells[0].Value = 0;
                }*/
                else
                {
                    tagsFromFile = RemoveTag(selectedTag, fileUrl);
                    //row.Cells[0].Value = 0;
                }

                string sortedTags = sortTagsAlphabetical(tagsFromFile);
                bool result = mbApiInterface.Library_SetFileTag(fileUrl, MetaDataType.Occasion, sortedTags);
                mbApiInterface.Library_CommitTagsToFile(fileUrl);
            }
        }

        private string sortTagsAlphabetical(string tags)
        {
            string[] occasionsAsArray = tags.Split(';');
            Array.Sort(occasionsAsArray);
            return String.Join(";", occasionsAsArray);
        }

        private string RemoveTag(string selectedTag, string fileUrl)
        {
            string tags = GetTags(fileUrl);
            tags = tags.Replace(selectedTag + ";", "");
            tags = tags.Replace(selectedTag, "");
            tags = tags.Trim(';');
            return tags;
        }

        private string AddTag(string selectedTag, string fileUrl)
        {
            string tags = GetTags(fileUrl);

            tags = tags.Trim(';');

            if (tags.Length <= 0)
            {
                return selectedTag;
            }
            else
            {
                return tags + ";" + selectedTag;
            }

        }

        private string GetTags(string fileUrl)
        {
            string[] tags = GetTagsFromFile(fileUrl);
            return String.Join(";", tags).Trim();
        }

        private bool IsTagAvailable(string tagName, string fileUrl)
        {
            string tags = GetTags(fileUrl);
            if (tags.Contains(tagName + ";") || tags.EndsWith(tagName))
            {
                return true;
            }

            return false;
        }
    }
}