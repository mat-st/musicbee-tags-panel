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

        private Control _panel;
        private TabControl tabControl;

        private List<MetaDataType> tags = new List<MetaDataType>();

        private Dictionary<string, ChecklistBoxPanel> checklistBoxList;
        private Dictionary<string, TabPage> _tabPageList;

        private Dictionary<String, CheckState> tagsFromFiles;

        private SettingsStorage settingsStorage;
        private TagsManipulation tagsManipulation;
        private string metaDataTypeName;
        private bool sortAlphabetically = false;

        #region Initialise plugin

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
            about.ConfigurationPanelHeight = 20;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            this.checklistBoxList = new Dictionary<string, ChecklistBoxPanel>();
            this.tagsFromFiles = new Dictionary<String, CheckState>();
            _tabPageList = new Dictionary<string, TabPage>();
            InitLogger();

            settingsStorage = new SettingsStorage(mbApiInterface, log);
            tagsManipulation = new TagsManipulation(this.mbApiInterface, this.settingsStorage);

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

        #endregion

        #region Settings dialog

        private void LoadSettings()
        {
            this.settingsStorage.LoadSettingsWithFallback();
            
            TagsStorage tagsStorage = this.settingsStorage.GetFirstOne();
            if (tagsStorage != null)
            {
                this.metaDataTypeName = tagsStorage.MetaDataType;
                this.sortAlphabetically = tagsStorage.Sorted;
            }
        }

        private void OpenSettingsDialog()
        {
            SettingsStorage copy = this.settingsStorage.DeepCopy();
            TagsPanelSettingsForm tagsPanelSettingsForm = new TagsPanelSettingsForm(copy);
            DialogResult result = tagsPanelSettingsForm.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            // TODO check the saving process, something goes wrong when the user clicks on save if he is on another tab but the first one.

            this.settingsStorage = tagsPanelSettingsForm.SettingsStorage;
            SaveSettings();
            // TODO we probably need a tempSettingsStorage
        }

        /// <summary>
        /// called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        /// </summary>
        public void SaveSettings()
        {
            this.settingsStorage.SaveAllSettings();

            /*
            TagsStorage tagsStorage = this.settingsStorage.GetFirstOne();
            if (tagsStorage != null)
            {
                this.sortAlphabetically = tagsStorage.Sorted;
            }
            */

            if (_panel != null)
            {
                ClearAllTagPages();
                this.AddTabPages();
                this.InvokeUpdateTagsTableData();
            }
        }

        #endregion

        #region Tag panels

        private void AddVisibleTagPanel(string tagName)
        {            
            TabPage page = GetTagPage(tagName);
            ChecklistBoxPanel checkListBox = CreateChecklistBoxForTag(tagName);
            this.ignoreEventFromHandler = false;

            page.Controls.Add(checkListBox);
        }

        private void AddTabPages()
        {
            _tabPageList.Clear();
            this.tabControl.TabPages.Clear();

            TagsStorage firstOne = null;
            foreach (TagsStorage tagsStorage in SettingsStorage.TagsStorages.Values)
            {
                AddVisibleTagPanel(tagsStorage.MetaDataType);

                if (null == firstOne)
                {
                    firstOne = tagsStorage;
                }
            }
            // TODO removing tabPages is not working properly
            
            /*
            List<string> tabPagesToRemove = new List<string>();
            foreach (TabPage tabPage in _tabPageList.Values)
            {
                string tagName = tabPage.Text;
                if(!settingsStorage.TagsStorages.ContainsKey(tagName))
                {
                    this.RemoveTabPage(tagName, tabPage);
                    tabPagesToRemove.Add(tagName);
                }
            }
            foreach (string tagPageName in tabPagesToRemove)
            {
                _tabPageList.Remove(tagPageName);
            }
            */
        }
       
        /// <summary>
        /// Removes a tab from the panel.
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="tabPage"></param>
        private void RemoveTabPage(string tagName, TabPage tabPage)
        {
            _tabPageList.Remove(tagName);
            this.tabControl.TabPages.Remove(tabPage);
        }

        private void AddTabPage(string tagName, TabPage tabPage)
        {
            _tabPageList.Add(tagName, tabPage);
            this.tabControl.TabPages.Add(tabPage);
        }

        /*
        private TabPage GetTagPage(string tagName)
        {
            TabPage tabPage;

            _tabPageList.TryGetValue(tagName, out tabPage);

            return tabPage;
        }
        */

        private TabPage GetTagPage(string tagName)
        {
            TabPage tabPage;
            
            if (_tabPageList.TryGetValue(tagName, out tabPage))
            {
                if (!tabPage.IsHandleCreated)
                {
                    tabPage.CreateControl();
                }

                return tabPage;
            }

            tabPage = new TabPage(tagName);
            this.AddTabPage(tagName, tabPage);

            return tabPage;
        }

        private ChecklistBoxPanel CreateChecklistBoxForTag(string tagName)
        {
            ChecklistBoxPanel checkListBox = GetCheckListBoxPanel(tagName);
            checkListBox.AddDataSource(SettingsStorage.GetTagsStorage(tagName).GetTags());

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

            if (this.checklistBoxList.TryGetValue(tagName, out checkListBox))
            {
                this.checklistBoxList.Remove(tagName);
                checkListBox = new ChecklistBoxPanel(mbApiInterface);
                checklistBoxList[tagName] = checkListBox;

                return checkListBox;
            }

            checkListBox = new ChecklistBoxPanel(mbApiInterface);
            checklistBoxList.Add(tagName, checkListBox);
           
            return checkListBox;
        }

        private void SetTagsInPanel(string[] fileUrls, CheckState selected, string selectedTag)
        {
            MetaDataType metaDataType = GetVisibleTabPageName();
            if (metaDataType == 0) { return; }
            tagsManipulation.SetTagsInFile(fileUrls, selected, selectedTag, metaDataType);
        }

        #endregion

        #region Helper methods

        public MetaDataType GetVisibleTabPageName()
        {
            if (this.metaDataTypeName == null) 
            { 
                return 0; 
            }

            return (MetaDataType)Enum.Parse(typeof(MetaDataType), this.metaDataTypeName, true);   
        }

        private TagsStorage GetCurrentTagsStorage()
        {
            MetaDataType metaDataType = GetVisibleTabPageName();
            if (metaDataType == 0) { return null; }
            return SettingsStorage.GetTagsStorage(metaDataType.ToString());
        }

        private void ClearAllTagPages()
        {
            _tabPageList.Clear();
            this.tabControl.TabPages.Clear();
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

        private void UpdateTagsTableData()
        {
            bool add = true;

            TagsStorage currentTagsStorage = GetCurrentTagsStorage();
            if (currentTagsStorage == null) { return; }

            string[] allTagsFromSettings = currentTagsStorage.GetTags().Keys.ToArray<string>();

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

            string tagName = currentTagsStorage.GetTagName();
            AddTagsToChecklistBoxPanel(tagName, data);
        }

        private void InvokeUpdateTagsTableData()
        {
            _panel.Invoke(new Action(() =>
            {
                UpdateTagsTableData();
            }));
        }

        #endregion

        #region Event handlers

        public void MenuSettingsClicked(object sender, EventArgs args)
        {
            OpenSettingsDialog();

            return;
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

            mbApiInterface.MB_RefreshPanels();
            ignoreEventFromHandler = false;
        }

        private void TabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage == null)
            {
                return;
            }

            this.metaDataTypeName = e.TabPage.Text;
            //SetTagsStorage(metaDataTypeName);
            SwitchVisibleTagPanel(metaDataTypeName);
        }

        private void ToolstripAbout_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Link to About dialog box");  // Write your code here
        }

        #endregion

        #region Controls

        private void SetPanelEnabled(bool enabled = true)
        {
            _panel.Invoke(new Action(() =>
            {
                _panel.Enabled = enabled;
            }));
        }

        private void UpdateTagsInPanelOnFileSelection()
        {
            ignoreEventFromHandler = true;
            ignoreForBatchSelect = true;
            InvokeUpdateTagsTableData();
            ignoreEventFromHandler = false;
            ignoreForBatchSelect = false;
        }

        private void SetTagsFromFilesInPanel(string[] filenames)
        {
            if (filenames != null && filenames.Length > 0)
            {
                TagsStorage currentTagsStorage = GetCurrentTagsStorage();
                if (currentTagsStorage == null) { return; }

                tagsFromFiles = tagsManipulation.CombineTagLists(filenames, currentTagsStorage);
            }
            else
            {
                tagsFromFiles.Clear();
            }
            // TODO check again
            // tagsStorage.SetTags(tagsFromFiles);

            UpdateTagsInPanelOnFileSelection();

            SetPanelEnabled(true);
        }

        // TODO think of another location
        private void SwitchVisibleTagPanel(string visibleTag)
        {
            // remove checklistBox from all panels
            foreach (TagsStorage tagsStorage in SettingsStorage.TagsStorages.Values)
            {
                string tagName = tagsStorage.GetTagName();
                TabPage page = GetTagPage(tagName);

                if (page.Controls.Count > 0)
                {
                    ChecklistBoxPanel checklistBoxPanel = (ChecklistBoxPanel)page.Controls[0];
                    checklistBoxPanel.RemoveItemCheckEventHandler();
                }

                page.Invoke(new Action (() =>
                {
                    page.Controls.Clear();
                }));
            }

            // add checklistBox to visible panel 
            AddVisibleTagPanel(visibleTag);
            SetTagsFromFilesInPanel(this.selectedFileUrls);
        }

        private void CreateTabPanel()
        {
            this.tabControl = (TabControl)mbApiInterface.MB_AddPanel(_panel, (PluginPanelDock)6);
            // TODO 
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(TabControl1_Selected);

            AddTabPages();
        }
        
        private void AddControls()
        {
            _panel.BeginInvoke(new Action(() =>
            {
                CreateTabPanel();
                _panel.SuspendLayout();
                _panel.Enabled = false;
                _panel.Controls.AddRange(new Control[]
                {
                  this.tabControl
                });
                _panel.ResumeLayout();
            }));
        }

        #endregion

        #region MusicBee

        /// <summary>
        /// MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        /// </summary>
        /// <param name="reason">The reason why MusicBee has closed the plugin.</param>
        public void Close(PluginCloseReason reason)
        {
            /*    
             *    TODO: Check if it is possible to get                      the userdisbled reason
             *    public enum PluginCloseReason
            {
                MusicBeeClosing = 1,
                UserDisabled = 2,
                StopNoUnload = 3
            }*/ 


            //this.panel.Dispose();
            // _panel = null;

            log.Info(reason.ToString("G"));
            log.Close();
        }

        /// <summary>
        /// uninstall this plugin - clean up any persisted files
        /// </summary>
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

        /// <summary>
        /// Receive event notifications from MusicBee.
        /// You need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event.
        /// </summary>
        /// <param name="sourceFileUrl"></param>
        /// <param name="type"></param>
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            if (_panel == null)
            {
                return;
            }

            MetaDataType metaDataType = 0;
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    metaDataType = GetVisibleTabPageName();
                    if (metaDataType == 0) { return; }
                    tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, metaDataType);
                    break;
                case NotificationType.TrackChanged:
                    metaDataType = GetVisibleTabPageName();
                    if (metaDataType == 0) { return; }
                    tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, metaDataType);
                    ignoreForBatchSelect = true;
                    InvokeUpdateTagsTableData();
                    ignoreForBatchSelect = false;
                    break;
                case NotificationType.TagsChanging:
                    if (ignoreEventFromHandler)
                    {
                        break;
                    }
                    ignoreForBatchSelect = true;
                    mbApiInterface.Library_CommitTagsToFile(sourceFileUrl);
                    metaDataType = GetVisibleTabPageName();
                    if (metaDataType == 0) { return; }
                    tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, metaDataType);
                    InvokeUpdateTagsTableData();
                    ignoreForBatchSelect = false;
                    break;
                // TODO: For me to remember
                case NotificationType.ApplicationWindowChanged:
                    log.Debug("Application Window changes notification");
                    break;
            }
        }

        /// <summary>
        /// Event handler that is triggered by MusicBee when a dockable panel has been created.
        /// </summary>
        /// <param name="panel">A reference to the new panel.</param>
        /// <returns>
        /// &lt; 0 indicates to MusicBee this control is resizable and should be sized to fill the panel it is docked to in MusicBee<br/>
        ///  = 0 indicates to MusicBee this control resizeable<br/>
        /// &gt; 0 indicates to MusicBee the fixed height for the control. Note it is recommended you scale the height for high DPI screens(create a graphics object and get the DpiY value)
        /// </returns>
        public int OnDockablePanelCreated(Control panel)
        {
            _panel = panel;

            if (!_panel.IsHandleCreated)
            {
                _panel.CreateControl();
            }

            AddControls();
            InvokeUpdateTagsTableData();
           
            return 0;
        }

        /// <summary>
        /// Event handler triggered by MusicBee when the user selects files in the library view.
        /// </summary>
        /// <param name="filenames">List of selected files.</param>
        public void OnSelectedFilesChanged(string[] filenames)
        {
            if (_panel == null)
            {
                return;
            }

            // important to have as a global variable
            selectedFileUrls = filenames;

            SetTagsFromFilesInPanel(filenames);
        }

        /// <summary>
        /// The presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked.
        /// </summary>
        /// <returns>Returns the list of ToolStripMenuItems that will be displayed.</returns>
        public List<ToolStripItem> GetMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("Tag-Panel Settings", null, MenuSettingsClicked));
            list.Add(new ToolStripMenuItem("About", null, ToolstripAbout_Clicked));
            return list;
        }

        #endregion
    }
}
