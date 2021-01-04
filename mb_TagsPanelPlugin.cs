using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;

        private string[] tempTags;
        private bool tempSortEnabled;

        private string[] selectedFileUrls = new string[] { };
        private Logger log;


        private bool ignoreEventFromHandler = true;
        private bool ignoreForBatchSelect = true;

        private Control ourPanel;
        private TabControl tabControl;

        private List<MetaDataType> tags = new List<MetaDataType>();

        private Dictionary<string, ChecklistBoxPanel> checklistBoxList;
        private Dictionary<string, TabPage> tabPageList;

        // TODO use:  only one instance of  TagsStorage and switch if 
        private TagsStorage tagsStorage;

        // TODO change methods accordingly to handle the list of storage classes 
        private SettingsStorage settingsStorage;
        private TagsManipulation tagsManipulation;

        private void SetTagsStorage(string tagName)
        {
            MetaDataType dataType = (MetaDataType) Enum.Parse(typeof(MetaDataType), tagName, true);
            tagsStorage = new TagsStorage(mbApiInterface, dataType);
        }

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            PluginInfo about = new PluginInfo();
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Tags-Panel";
            about.Description = "Creates a dockable Panel which lets the user choose tags from an predefined " +
                "list";
            about.Author = "Matthias Steiert + The Anonymous Programmer";
            about.TargetApplication = "Tags-Panel";   //  the name of a Plugin Storage device or panel header for a dockable panel
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

            checklistBoxList = new Dictionary<string, ChecklistBoxPanel>();
            tabPageList = new Dictionary<string, TabPage>();
            InitLogger();

            settingsStorage = new SettingsStorage(mbApiInterface, log);
            tagsManipulation = new TagsManipulation();
            // TODO set this value dynamically 
            SetTagsStorage(MetaDataType.Mood.ToString("g"));

            LoadSettings();


            // add Tags-Panel Settings to Tools Menu
            mbApiInterface.MB_AddMenuItem("mnuTools/Tags-Panel Settings", null, MenuSettingsClicked);

            log.Info("Tags-Panel plugin started");

            return about;
        }

        private void InitLogger()
        {
            log = new Logger(mbApiInterface);
        }






        // GANZ Schlechter Stil ;)
        public void MenuSettingsClicked(object sender, EventArgs args)
        {

            bool useSort = true;
            SavedSettingsType settings = settingsStorage.GetSavedSettings();
            if (settings != null)
            {
                useSort = settings.sorted;
            }
            string[] allTagsFromConfig = settingsStorage.GetAllTagsFromConfig();
            /*fvSettings tagsPanelSettingForm = new fvSettings(allTagsFromConfig, useSort);
            tagsPanelSettingForm.ShowDialog();*/

            List<TagsStorage> tagsStorageList = new List<TagsStorage>();
            tagsStorageList.Add(tagsStorage);
            TagsPanelSettingsForm tagsPanelSettingsForm = new TagsPanelSettingsForm(tagsStorageList, settingsStorage);
            tagsPanelSettingsForm.ShowDialog();

            TagsPanelSettingsPanel tagsPanelSettingsPanel = tagsPanelSettingsForm.GetPanel(tagsStorage.GetTagName());
            tempTags = tagsPanelSettingsPanel.GetTags();
            tempSortEnabled = tagsPanelSettingsPanel.IsSortEnabled();

            SaveSettings();
            UpdateTagsTableData(ourPanel);

            return;
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
            /*fvSettings tagsPanelSettingForm = new fvSettings(allTagsFromConfig, useSort);
            tagsPanelSettingForm.ShowDialog();*/

            List<TagsStorage> tagsStorageList = new List<TagsStorage>();
            tagsStorageList.Add(tagsStorage);
            TagsPanelSettingsForm tagsPanelSettingsForm = new TagsPanelSettingsForm(tagsStorageList, settingsStorage);
            tagsPanelSettingsForm.ShowDialog();

            TagsPanelSettingsPanel tagsPanelSettingsPanel = tagsPanelSettingsForm.GetPanel(tagsStorage.GetTagName());
            tempTags = tagsPanelSettingsPanel.GetTags();
            tempSortEnabled = tagsPanelSettingsPanel.IsSortEnabled();

            return true;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            settingsStorage.SaveSettings(tempSortEnabled, tempTags);

            if (ourPanel != null)
            {
                UpdateTagsTableData(ourPanel);
            }
        }

        private void LoadSettings()
        {
            settingsStorage.LoadSettingsWithFallback();

            tempTags = settingsStorage.GetAllTagsFromConfig();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            //ourPanel.Dispose();
            ourPanel = null;
            log.Info(reason.ToString("g"));
            log.Close();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            // Delete settings file
            string settingsFileName = settingsStorage.GetSettingsPath();
            if (System.IO.File.Exists(settingsFileName))
            {
                System.IO.File.Delete(settingsFileName);
            }

            // Delete log file
            string logFileName = log.GetLogFilePath();
            if (System.IO.File.Exists(logFileName))
            {
                System.IO.File.Delete(logFileName);
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
                    UpdateTagsTableData(ourPanel);
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
                    UpdateTagsTableData(ourPanel);
                    ourPanel.Invalidate();
                    ignoreForBatchSelect = false;
                    break;
            }
        }

        public int OnDockablePanelCreated(Control panel)
        {
            ourPanel = panel;
            AddControls(ourPanel);
            UpdateTagsTableData(ourPanel);

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

            // TODO For loop
            Dictionary<String, CheckState> tagsList = new Dictionary<String, CheckState>();

            if (filenames == null || filenames.Length <= 0)
            {
                tagsStorage.SetTags(tagsList);

                UpdateTagsInPanelOnFileSelection();
                SetPanelEnabled(false);

                return;
            }


            SetPanelEnabled(true);

            tagsList = tagsManipulation.CombineTagLists(filenames, tagsStorage);
            tagsStorage.SetTags(tagsList);

            UpdateTagsInPanelOnFileSelection();
        }

        private void UpdateTagsInPanelOnFileSelection()
        {
            ignoreEventFromHandler = true;
            ignoreForBatchSelect = true;
            UpdateTagsTableData(ourPanel);
            ourPanel.Invalidate();
            ignoreEventFromHandler = false;
            ignoreForBatchSelect = false;
        }



        private void UpdateTagsTableData(Control panel)
        {
            bool add = true;
            string[] allTagsFromConfig = settingsStorage.GetAllTagsFromConfig();
            Dictionary<String, CheckState> allTags = tagsStorage.GetTags();

            Dictionary<String, CheckState> data = new Dictionary<String, CheckState>();
            foreach (string tagFromConfig in allTagsFromConfig)
            {
                foreach (String tagEntry in allTags.Keys)
                {
                    if (tagFromConfig.Trim() == tagEntry.Trim())
                    {
                        data.Add(tagEntry, allTags[tagEntry]);
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

            // TODO change data accordingly to the tag name
            string tagName = tagsStorage.GetTagName();
            if (panel.IsHandleCreated)
            {
                panel.Invoke(new Action(() =>
                {
                    AddTagsToChecklistBoxPanel(tagName, data);
                }));
            }
            else
            {
                AddTagsToChecklistBoxPanel(tagName, data);
            }
        }

        private void AddTagsToChecklistBoxPanel(string tagName, Dictionary<String, CheckState> tags)
        {
            ChecklistBoxPanel checklistBoxPanel;
            this.checklistBoxList.TryGetValue(tagName, out checklistBoxPanel);
            checklistBoxPanel.AddDataSource(tags);
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
            CreateTabPanel();

            _panel.Enabled = false;
            _panel.SuspendLayout();
            _panel.Controls.AddRange(new Control[]
            {
                  this.tabControl
            });
            _panel.ResumeLayout();

        }

        private void CreateTabPanel()
        {
            this.tabControl = (TabControl)mbApiInterface.MB_AddPanel(null, (PluginPanelDock)6);
            this.tabControl.Dock = DockStyle.Fill;

            AddVisibleTagPanel(MetaDataType.Mood.ToString("g"));
            AddInvisibleTagPanel(MetaDataType.Occasion.ToString("g"));
            AddInvisibleTagPanel(MetaDataType.Genre.ToString("g"));
        }

        private void AddInvisibleTagPanel(string tagName)
        {
            TabPage page = GetTagPage(tagName);
            page.Controls.Clear();
            this.tabControl.TabPages.Add(page);
        }

        private void AddVisibleTagPanel(string tagName)
        {
            SetTagsStorage(tagName);
            
            TabPage page = GetTagPage(tagName);

            ChecklistBoxPanel checkListBox = GetCheckListBoxPanel(tagName);
            checkListBox.AddDataSource(this.tagsStorage.GetTags());

            checkListBox.Dock = DockStyle.Fill;
            // TODO only do this once 
            checkListBox.AddItemCheckEventHandler(
                new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox1_ItemCheck)
            );
            this.ignoreEventFromHandler = false;
            page.Controls.Add(checkListBox);
            this.tabControl.TabPages.Add(page);
        }

        private ChecklistBoxPanel GetCheckListBoxPanel(string tagName)
        {
            ChecklistBoxPanel checkListBox;
            if (!this.checklistBoxList.TryGetValue(tagName, out checkListBox))
            {
                checkListBox = new ChecklistBoxPanel(mbApiInterface);
                checklistBoxList.Add(tagName, checkListBox);
            }
            return checkListBox;
        }

        private TabPage GetTagPage(string tagName)
        {
            TabPage tabPage;
            if (!this.tabPageList.TryGetValue(tagName, out tabPage))
            {
                tabPage = new TabPage(tagName);
                this.tabPageList.Add(tagName, tabPage);
            }
            return tabPage;
        }

        // presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked
        // return the list of ToolStripMenuItems that will be displayed
        public List<ToolStripItem> GetHeaderMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("Tag-Panel Settings"));
            list.Add(new ToolStripMenuItem("About"));
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
            tagsStorage.SetTagsInFile(fileUrls, selected, selectedTag, tagsManipulation);
        }
    }    
}
