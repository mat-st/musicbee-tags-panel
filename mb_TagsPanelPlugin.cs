using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;

        //private Dictionary<String, CheckState> tempTags;
        //private bool tempSortEnabled;

        private string[] selectedFileUrls = new string[] { };
        private Logger log;


        private bool ignoreEventFromHandler = true;
        private bool ignoreForBatchSelect = true;

        private Control ourPanel;
        private TabControl tabControl;

        private List<MetaDataType> tags = new List<MetaDataType>();

        private Dictionary<string, ChecklistBoxPanel> checklistBoxList;
        private Dictionary<string, TabPage> tabPageList;

        private TagsStorage tagsStorage;

        private Dictionary<String, CheckState> tagsFromFiles;

        private SettingsStorage settingsStorage;
        private TagsManipulation tagsManipulation;

        private void SetTagsStorage(string tagName)
        {
            tagsStorage = settingsStorage.GetTagsStorage(tagName);
        }

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            PluginInfo about = new PluginInfo();
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Tags-Panel";
            about.Description = "Creates a dockable Panel with user defined tabed pages which let the user choose tags from user defined " +
                "lists";
            about.Author = "mat-st & The Anonymous Programmer";
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
            tagsFromFiles = new Dictionary<String, CheckState>();
            tabPageList = new Dictionary<string, TabPage>();
            InitLogger();

            settingsStorage = new SettingsStorage(mbApiInterface, log);
            tagsManipulation = new TagsManipulation(this.mbApiInterface);

            LoadSettings();


            // add Tags-Panel Settings to Tools Menu
            mbApiInterface.MB_AddMenuItem("mnuTools/Tags-Panel Settings", "Tags-Panel: Open Settings", MenuSettingsClicked);

            log.Info("Tags-Panel plugin started");

            return about;
        }

        private void InitLogger()
        {
            log = new Logger(mbApiInterface);
        }

        public void MenuSettingsClicked(object sender, EventArgs args)
        {
            OpenSettingsDialog();

            SaveSettings();

            return;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            
            OpenSettingsDialog();

            return true;
        }

        private void OpenSettingsDialog()
        {
            TagsPanelSettingsForm tagsPanelSettingsForm = new TagsPanelSettingsForm(settingsStorage);
            DialogResult result = tagsPanelSettingsForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                //TagsPanelSettingsPanel tagsPanelSettingsPanel = tagsPanelSettingsForm.GetPanel(tagsStorage.GetTagName());
                settingsStorage = tagsPanelSettingsForm.SettingsStorage;
                // TODO we probably need a tempSettingsStorage
                //tempTags = tagsPanelSettingsPanel.GetTags();
                // tempSortEnabled = tagsPanelSettingsPanel.IsSortEnabled();
            } else
            {
                /*tempTags = new Dictionary<string, CheckState>();
                tempSortEnabled = true;*/
            }
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            //tagsStorage.SetTags(tempTags);
            //tagsStorage.Sorted = this.tempSortEnabled;

            settingsStorage.SaveAllSettings();

            if (ourPanel != null)
            {
                // TODO make sure everything will be fine and keep you towel with you (always remember the 42!)
                ClearAllTagPages();
                AddTabPages();
                UpdateTagsTableData(ourPanel);
            }
        }

        private void LoadSettings()
        {
            settingsStorage.LoadSettingsWithFallback();
            tagsStorage = settingsStorage.GetFirstOne();
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
                    tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, tagsStorage.GetMetaDataType());
                    break;
                case NotificationType.TrackChanged:
                    tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, tagsStorage.GetMetaDataType());
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
                    tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, tagsStorage.GetMetaDataType());
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

            SetTagsFromFilesInPanel(filenames);
        }

        private void SetTagsFromFilesInPanel(string[] filenames)
        {
            if (filenames != null && filenames.Length > 0)
            {
                tagsFromFiles = tagsManipulation.CombineTagLists(filenames, tagsStorage);
            }
            else
            {
                tagsFromFiles.Clear();
            }
            //tagsStorage.SetTags(tagsFromFiles);

            UpdateTagsInPanelOnFileSelection();
            SetPanelEnabled(true);
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

            string[] allTagsFromSettings = tagsStorage.GetTags().Keys.ToArray<string>();

            Dictionary<String, CheckState> data = new Dictionary<String, CheckState>();
            foreach (string tagFromSettings in allTagsFromSettings)
            {
                foreach (String tagEntry in tagsFromFiles.Keys)
                {
                    if (tagFromSettings.Trim() == tagEntry.Trim())
                    {
                        data.Add(tagEntry, tagsFromFiles[tagEntry]);
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    data.Add(tagFromSettings, CheckState.Unchecked);
                }
                add = true;
            }

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
            if (null != checklistBoxPanel)
            {
                checklistBoxPanel.AddDataSource(tags);
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
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(TabControl1_Selected);

            ClearAllTagPages();
            AddTabPages();
        }

        private void AddTabPages()
        {
            TagsStorage firstOne = null;
            foreach (TagsStorage tagsStorage in settingsStorage.TagsStorages.Values)
            {
                if (null == firstOne)
                {
                    AddVisibleTagPanel(tagsStorage.MetaDataType);
                    firstOne = tagsStorage;
                }
                else
                {
                    AddInvisibleTagPanel(tagsStorage.MetaDataType);
                }
            }
        }

        private void TabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage == null)
            {
                return;
            }

            string metaDataTypeName = e.TabPage.Text;
            SetTagsStorage(metaDataTypeName);
            SwitchVisibleTagPanel(metaDataTypeName);
        }

        private void AddInvisibleTagPanel(string tagName)
        {
            GetTagPage(tagName);
        }

        private void AddVisibleTagPanel(string tagName)
        {            
            TabPage page = GetTagPage(tagName);

            ChecklistBoxPanel checkListBox = CreateChecklistBoxForTag(tagName);

            this.ignoreEventFromHandler = false;
            page.Controls.Add(checkListBox);
        }


        private void SwitchVisibleTagPanel(string visibleTag)
        {
            // remove checklistBox from all panels
            foreach (TagsStorage tagsStorage in settingsStorage.TagsStorages.Values)
            {
                string tagName = tagsStorage.GetTagName();
                TabPage page = GetTagPage(tagName);
                page.Controls.Clear();
            }

            // add checklistBox to visible panel 
            AddVisibleTagPanel(visibleTag);
            SetTagsFromFilesInPanel(this.selectedFileUrls);
        }

        private ChecklistBoxPanel CreateChecklistBoxForTag(string tagName)
        {
            ChecklistBoxPanel checkListBox = GetCheckListBoxPanel(tagName);
            checkListBox.AddDataSource(this.tagsStorage.GetTags());

            checkListBox.Dock = DockStyle.Fill;
            // TODO only do this once 
            checkListBox.AddItemCheckEventHandler(
                new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox1_ItemCheck)
            );

            return checkListBox;
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
                this.tabControl.TabPages.Add(tabPage);
            }
            return tabPage;
        }

        private void ClearAllTagPages()
        {
            this.tabPageList.Clear();
            this.tabControl.TabPages.Clear();
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
            tagsManipulation.SetTagsInFile(fileUrls, selected, selectedTag, tagsStorage.GetMetaDataType());
        }
    }    
}
