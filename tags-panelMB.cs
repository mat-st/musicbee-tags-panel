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

        private MusicBeeApiInterface mbApiInterface;

        private string[] temp_occasions;
        private bool tempSortEnabled;

        private string[] selectedFileUrls = new string[] { };
        private Logger log;

        private Control ourPanel;
        private TabControl tabControl;
        private ChecklistBoxPanel checklistBox;

        private bool ignoreEventFromHandler = true;
        private bool ignoreForBatchSelect = true;

        private SettingsStorage settingsStorage;
        private TagsStorage tagsStorage;
        private TagsManipulation tagsManipulation;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            PluginInfo about = new PluginInfo();
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Tags-Panel";
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

            InitLogger();

            settingsStorage = new SettingsStorage(mbApiInterface, log);
            tagsStorage = new TagsStorage(mbApiInterface, MetaDataType.Occasion);
            tagsManipulation = new TagsManipulation();

            LoadSettings();

            log.Info("Tagger plugin started");

            return about;
        }

        private void InitLogger()
        {
            string logPath = System.IO.Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), LOG_FILE_NAME);
            log = new Logger(logPath);
        }

        public bool Configure(IntPtr panelHandle)
        {
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window

            bool useSort = true;
            SavedSettingsType settings = settingsStorage.GetSavedSettings();
            if (settings != null)
            {
                useSort = settings.sorted;
            }
            string[] allTagsFromConfig = settingsStorage.GetAllTagsFromConfig();
            fvSettings tagsPanelSettingForm = new fvSettings(allTagsFromConfig, useSort);
            tagsPanelSettingForm.ShowDialog();
            temp_occasions = tagsPanelSettingForm.getOccasions();
            tempSortEnabled = tagsPanelSettingForm.isSortEnabled();

            return true;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            settingsStorage.SaveSettings(tempSortEnabled, temp_occasions);

            if (ourPanel != null)
            {
                UpdateOccasionTableData(ourPanel);
            }
        }

        private void LoadSettings()
        {
            settingsStorage.LoadSettingsWithFallback();

            temp_occasions = settingsStorage.GetAllTagsFromConfig();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            //ourPanel.Dispose();
            ourPanel = null;
            log.Close();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            string settingsFileName = settingsStorage.GetSettingsFileName();
            //Delete settings file
            if (System.IO.File.Exists(settingsFileName))
            {
                System.IO.File.Delete(settingsFileName);
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
                    tagsStorage.UpdateTagsFromFile(sourceFileUrl);
                    break;
                case NotificationType.TrackChanged:
                    tagsStorage.UpdateTagsFromFile(sourceFileUrl);
                    ignoreForBatchSelect = true;
                    UpdateOccasionTableData(ourPanel);
                    ourPanel.Invalidate();
                    ignoreForBatchSelect = false;
                    break;
                case NotificationType.TagsChanging:
                    if (ignoreEventFromHandler)
                    {
                        break;
                    }
                    ignoreForBatchSelect = true;
                    mbApiInterface.Library_CommitTagsToFile(sourceFileUrl);
                    tagsStorage.UpdateTagsFromFile(sourceFileUrl);
                    UpdateOccasionTableData(ourPanel);
                    ourPanel.Invalidate();
                    ignoreForBatchSelect = false;
                    break;
            }
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
            UpdateOccasionTableData(ourPanel);

            return 0;
        }

        private void SetPanelEnabled(bool enabled = true)
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

            // important to have as a global variable
            selectedFileUrls = filenames;

            Dictionary<String, CheckState> occasionList = new Dictionary<String, CheckState>();

            if (filenames == null || filenames.Length <= 0)
            {
                tagsStorage.SetTags(occasionList);

                updateTagsInPanelOnFileSelection();
                SetPanelEnabled(false);

                return;
            }


            SetPanelEnabled(true);

            occasionList = tagsManipulation.combineTagLists(filenames, tagsStorage);
            tagsStorage.SetTags(occasionList);

            updateTagsInPanelOnFileSelection();
        }

        private void updateTagsInPanelOnFileSelection()
        {
            ignoreEventFromHandler = true;
            ignoreForBatchSelect = true;
            UpdateOccasionTableData(ourPanel);
            ourPanel.Invalidate();
            ignoreEventFromHandler = false;
            ignoreForBatchSelect = false;
        }

        

        private void UpdateOccasionTableData(Control panel, Dictionary<String, CheckState> allOccasions = null)
        {
            bool add = true;
            string[] allTagsFromConfig = settingsStorage.GetAllTagsFromConfig();
            Dictionary<String, CheckState> occasionList = tagsStorage.GetTags();

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
                    LayoutPanel(_panel);
                });
            }
            else
            {
                LayoutPanel(_panel);
            }
        }

        private void LayoutPanel(Control _panel)
        {
            CreateTabbedPanel();

            _panel.Enabled = false;
            _panel.SuspendLayout();
            _panel.Controls.AddRange(new Control[]
            {
                  this.tabControl
            });
            _panel.ResumeLayout();

        }

        private void CreateTabbedPanel()
        {
            this.tabControl = (TabControl)mbApiInterface.MB_AddPanel(null, (PluginPanelDock)6);
            this.tabControl.Dock = DockStyle.Fill;

            TabPage page1 = new TabPage("Occasions");
            this.checklistBox = new ChecklistBoxPanel(mbApiInterface, tagsStorage.GetTags());
            checklistBox.Dock = DockStyle.Fill;
            checklistBox.AddItemCheckEventHandler(new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox1_ItemCheck));
            this.ignoreEventFromHandler = false;
            page1.Controls.Add(checklistBox);
            this.tabControl.TabPages.Add(page1);
            TabPage page2 = new TabPage("Moods");
            this.tabControl.TabPages.Add(page2);
            TabPage page3 = new TabPage("Genres");
            this.tabControl.TabPages.Add(page3);
        }

        // presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked
        // return the list of ToolStripMenuItems that will be displayed
        public List<ToolStripItem> GetHeaderMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("A menu item"));
            list.Add(new ToolStripMenuItem("Another item"));
            return list;
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ignoreForBatchSelect)
            {
                return;
            }

            int index = e.Index;
            CheckState state = e.NewValue;
            string name = ((CheckedListBox)sender).Items[index].ToString();

            ignoreEventFromHandler = true;
            SetTagsInPanel(this.selectedFileUrls, state, name);
            if (ourPanel != null)
            {
                ourPanel.Invalidate();
            }
            mbApiInterface.MB_RefreshPanels();
            ignoreEventFromHandler = false;
        }

        private void SetTagsInPanel(string[] fileUrls, CheckState selected, string selectedTag)
        {
            mbApiInterface.MB_SetBackgroundTaskMessage("Save tags to file");
            foreach (string fileUrl in fileUrls)
            {
                string tagsFromFile;
                if (selected == CheckState.Checked)
                {
                    tagsFromFile = tagsStorage.AddTag(selectedTag, fileUrl);
                }
                else
                {
                    tagsFromFile = tagsStorage.RemoveTag(selectedTag, fileUrl);
                }

                string sortedTags = tagsManipulation.SortTagsAlphabetical(tagsFromFile);
                bool result = mbApiInterface.Library_SetFileTag(fileUrl, MetaDataType.Occasion, sortedTags);
                mbApiInterface.Library_CommitTagsToFile(fileUrl);

            }
            mbApiInterface.MB_SetBackgroundTaskMessage("Save tags finished");
        }
    }
}
