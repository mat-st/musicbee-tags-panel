using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private Logger log;
        private Control _panel;
        private TabControl tabControl;
        private List<MetaDataType> tags = new List<MetaDataType>();
        private Dictionary<string, ChecklistBoxPanel> checklistBoxList;
        private Dictionary<string, TabPage> _tabPageList;
        private Dictionary<string, CheckState> tagsFromFiles;
        private SettingsStorage settingsStorage;
        private TagsManipulation tagsManipulation;
        private string metaDataTypeName;
        private bool sortAlphabetically = false;
        private PluginInfo about = new PluginInfo();
        private string[] selectedFileUrls = Array.Empty<string>();
        private bool ignoreEventFromHandler = true;
        private bool ignoreForBatchSelect = true;

        #region Initialise plugin

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            InitializeApi(apiInterfacePtr);

            about = CreatePluginInfo();
            InitializePluginComponents();

            return about;
        }

        private void InitializeApi(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
        }

        private PluginInfo CreatePluginInfo()
        {
            var pluginInfo = new PluginInfo
            {
                PluginInfoVersion = PluginInfoVersion,
                Name = "Tags-Panel",
                Description = "Creates a dockable Panel with user-defined tabbed pages which let the user choose tags from user-defined lists",
                Author = "mat-st & The Anonymous Programmer",
                TargetApplication = "Tags-Panel",
                Type = PluginType.General,
                VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major,
                VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor,
                Revision = 1,
                MinInterfaceVersion = MinInterfaceVersion,
                MinApiRevision = MinApiRevision,
                ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents | ReceiveNotificationFlags.DataStreamEvents),
                ConfigurationPanelHeight = 20
            };
            return pluginInfo;
        }

        private void InitializePluginComponents()
        {
            checklistBoxList = new Dictionary<string, ChecklistBoxPanel>();
            tagsFromFiles = new Dictionary<string, CheckState>();
            _tabPageList = new Dictionary<string, TabPage>();
            InitLogger();

            settingsStorage = new SettingsStorage(mbApiInterface, log);
            tagsManipulation = new TagsManipulation(mbApiInterface, settingsStorage);

            LoadSettings();

            InitializeMenu();

            log.Info("Tags-Panel plugin started");
        }


        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            
            return false;
        }

        private void InitLogger()
        {
            log = new Logger(mbApiInterface);
        }
        private void InitializeMenu()
        {
            mbApiInterface.MB_AddMenuItem("mnuTools/Tags-Panel Settings", "Tags-Panel: Open Settings", MenuSettingsClicked);
        }

        private void LoadSettings()
        {
            LoadFallbackSettings();
            LoadTagsStorageSettings();
        }

        private void LoadFallbackSettings()
        {
            settingsStorage.LoadSettingsWithFallback();
        }

        private void LoadTagsStorageSettings()
        {
            TagsStorage tagsStorage = settingsStorage.GetFirstOne();
            if (tagsStorage != null)
            {
                metaDataTypeName = tagsStorage.MetaDataType;
                sortAlphabetically = tagsStorage.Sorted;
            }
        }

        private void OpenSettingsDialog()
        {
            SettingsStorage settingsCopy = settingsStorage.DeepCopy();
            using (var tagsPanelSettingsForm = new TagsPanelSettingsForm(settingsCopy))
            {
                if (tagsPanelSettingsForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                UpdateSettingsFromDialog(tagsPanelSettingsForm);
                SaveSettings();
                UpdatePanelVisibility();
            }
        }

        private void HandleSettingsDialogResult(TagsPanelSettingsForm tagsPanelSettingsForm)
        {
            UpdateSettingsFromDialog(tagsPanelSettingsForm);
            SaveSettings();
            UpdatePanelVisibility();
        }

        private void UpdateSettingsFromDialog(TagsPanelSettingsForm tagsPanelSettingsForm)
        {
            settingsStorage = tagsPanelSettingsForm.SettingsStorage;
        }

        private void UpdatePanelVisibility()
        {
            tabControl.Visible = tabControl.Controls.Count > 0;
        }

        /// <summary>
        /// Called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        /// </summary>
        public void SaveSettings()
        {
            SaveAllSettings();
            UpdateSortAlphabetically();
            UpdatePanelData();
        }

        private void SaveAllSettings()
        {
            settingsStorage.SaveAllSettings();
        }

        private void UpdateSortAlphabetically()
        {
            TagsStorage tagsStorage = settingsStorage.GetFirstOne();
            if (tagsStorage != null)
            {
                sortAlphabetically = tagsStorage.Sorted;
            }
        }

        private void UpdatePanelData()
        {
            ClearAndAddTabPages();
            InvokeUpdateTagsTableData();
        }

        private void ClearAndAddTabPages()
        {
            ClearAllTagPages();
            AddTabPages();
        }

        private void AddVisibleTagPanel(string tagName)
        {
            if (_tabPageList.TryGetValue(tagName, out var tabPage))
            {
                if (!tabPage.IsHandleCreated)
                {
                    tabPage.CreateControl();
                }
            }
            else
            {
                tabPage = new TabPage(tagName);
                _tabPageList.Add(tagName, tabPage);
                tabControl.TabPages.Add(tabPage);
            }

            ChecklistBoxPanel checkListBox = GetOrCreateCheckListBoxPanel(tagName);
            checkListBox.AddDataSource(SettingsStorage.GetTagsStorage(tagName).GetTags());

            checkListBox.Dock = DockStyle.Fill;
            checkListBox.AddItemCheckEventHandler(new System.Windows.Forms.ItemCheckEventHandler(CheckedListBox1_ItemCheck));

            tabPage.Controls.Add(checkListBox);
        }

        private TabPage GetOrCreateTagPage(string tagName)
        {
            if (_tabPageList.TryGetValue(tagName, out var tabPage))
            {
                if (!tabPage.IsHandleCreated)
                {
                    tabPage.CreateControl();
                }
            }
            else
            {
                tabPage = new TabPage(tagName);
                AddTabPage(tagName, tabPage);
            }

            return tabPage;
        }

        private void AddTabPages()
        {
            _tabPageList.Clear();
            tabControl.TabPages.Clear();

            foreach (var tagsStorage in SettingsStorage.TagsStorages.Values)
            {
                AddVisibleTagPanel(tagsStorage.MetaDataType);
            }
        }



        /// <summary>
        /// Removes a tab from the panel.
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="tabPage"></param>
        private void RemoveTabPage(string tagName, TabPage tabPage)
        {
            _tabPageList.Remove(tagName);
            tabControl.TabPages.Remove(tabPage);
        }

        private void AddTabPage(string tagName, TabPage tabPage)
        {
            _tabPageList.Add(tagName, tabPage);
            tabControl.TabPages.Add(tabPage);
        }

        private ChecklistBoxPanel CreateChecklistBoxForTag(string tagName)
        {
            ChecklistBoxPanel checkListBox = GetOrCreateCheckListBoxPanel(tagName);
            checkListBox.AddDataSource(SettingsStorage.GetTagsStorage(tagName).GetTags());

            checkListBox.Dock = DockStyle.Fill;
            // TODO only do this once 
            checkListBox.AddItemCheckEventHandler(new System.Windows.Forms.ItemCheckEventHandler(CheckedListBox1_ItemCheck));

            return checkListBox;
        }

        private ChecklistBoxPanel GetOrCreateCheckListBoxPanel(string tagName)
        {
            if (checklistBoxList.TryGetValue(tagName, out var checkListBox))
            {
                if (!checkListBox.IsHandleCreated)
                {
                    checkListBox.CreateControl();
                }
            }
            else
            {
                checkListBox = new ChecklistBoxPanel(mbApiInterface);
                checklistBoxList.Add(tagName, checkListBox);
            }

            return checkListBox;
        }

        private void SetTagsInPanel(string[] fileUrls, CheckState selected, string selectedTag)
        {
            MetaDataType metaDataType = GetVisibleTabPageName();
            if (metaDataType != 0)
            {
                tagsManipulation.SetTagsInFile(fileUrls, selected, selectedTag, metaDataType);
            }
        }

        private void DeleteFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }

        public MetaDataType GetVisibleTabPageName()
        {
            return metaDataTypeName != null ? (MetaDataType)Enum.Parse(typeof(MetaDataType), metaDataTypeName, true) : 0;
        }

        private TagsStorage GetCurrentTagsStorage()
        {
            MetaDataType metaDataType = GetVisibleTabPageName();
            return metaDataType != 0 ? SettingsStorage.GetTagsStorage(metaDataType.ToString()) : null;
        }

        private void ClearAllTagPages()
        {
            _tabPageList.Clear();
            tabControl.TabPages.Clear();
        }

        private void AddTagsToChecklistBoxPanel(string tagName, Dictionary<String, CheckState> tags)
        {
            if (checklistBoxList.TryGetValue(tagName, out var checklistBoxPanel))
            {
                checklistBoxPanel.AddDataSource(tags);
            }
        }

        private void UpdateTagsTableData()
        {
            TagsStorage currentTagsStorage = GetCurrentTagsStorage();
            if (currentTagsStorage == null) { return; }

            currentTagsStorage.SortByIndex();
            string[] allTagsFromSettings = currentTagsStorage.GetTags().Keys.ToArray<string>();

            Dictionary<String, CheckState> data = new Dictionary<String, CheckState>(allTagsFromSettings.Length);
            foreach (string tagFromSettings in allTagsFromSettings)
            {
                if (tagsFromFiles.TryGetValue(tagFromSettings.Trim(), out var checkState))
                {
                    data.Add(tagFromSettings, checkState);
                }
                else
                {
                    data.Add(tagFromSettings, CheckState.Unchecked);
                }
            }

            string tagName = currentTagsStorage.GetTagName();
            AddTagsToChecklistBoxPanel(tagName, data);
        }

        private void InvokeUpdateTagsTableData()
        {
            _panel.Invoke((Action)UpdateTagsTableData);
        }

        #endregion

        #region Event handlers

        public void MenuSettingsClicked(object sender, EventArgs args)
        {
            OpenSettingsDialog();
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
            SetTagsInPanel(selectedFileUrls, state, name);

            mbApiInterface.MB_RefreshPanels();
            ignoreEventFromHandler = false;
        }

        private void TabControl1_Selected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage == null)
            {
                return;
            }

            metaDataTypeName = e.TabPage.Text;
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
            _panel.Invoke((Action)InvokeUpdateTagsTableData);
            ignoreEventFromHandler = false;
            ignoreForBatchSelect = false;
        }

        private void SetTagsFromFilesInPanel(string[] filenames)
        {
            if (filenames != null && filenames.Length > 0)
            {
                TagsStorage currentTagsStorage = GetCurrentTagsStorage();
                if (currentTagsStorage != null)
                {
                    tagsFromFiles = tagsManipulation.CombineTagLists(filenames, currentTagsStorage);
                }
            }
            else
            {
                tagsFromFiles.Clear();
            }

            UpdateTagsInPanelOnFileSelection();

            SetPanelEnabled(true);
        }


        void SwitchVisibleTagPanel(string visibleTag)
        {
            // remove checklistBox from all panels
            foreach (var tagsStorage in SettingsStorage.TagsStorages.Values)
            {
                string tagName = tagsStorage.GetTagName();
                TabPage page = GetOrCreateTagPage(tagName);

                if (page.Controls.Count > 0 && page.Controls[0] is ChecklistBoxPanel checklistBoxPanel)
                {
                    checklistBoxPanel.RemoveItemCheckEventHandler();
                }

                page.Controls.Clear();
            }

            // add checklistBox to visible panel 
            AddVisibleTagPanel(visibleTag);
            SetTagsFromFilesInPanel(selectedFileUrls);
        }

        private void CreateTabPanel()
        {
            tabControl = (TabControl)mbApiInterface.MB_AddPanel(_panel, (PluginPanelDock)6);
            // TODO 
            tabControl.Dock = DockStyle.Fill;
            tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(TabControl1_Selected);

            AddTabPages();
        }

        private void AddSettingsLabel()
        {
            _panel.BeginInvoke(new Action(() =>
            {
                _panel.SuspendLayout();
                Label emptyPanelText = new Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(14, 30),
                    Size = new System.Drawing.Size(38, 13),
                    TabIndex = 2,
                    Text = "Please add a tag in the settings dialog first."
                };

                _panel.Controls.Add(emptyPanelText);
                _panel.Controls.SetChildIndex(emptyPanelText, 1);
                _panel.Controls.SetChildIndex(tabControl, 0);

                if (tabControl.TabPages.Count == 0)
                {
                    tabControl.Visible = false;
                }

                _panel.ResumeLayout();
            }));
        }

        private void AddControls()
        {
            _panel.SuspendLayout();
            CreateTabPanel();
            _panel.Controls.Add(tabControl);
            _panel.Enabled = false;
            _panel.ResumeLayout();
        }

        /// <summary>
        /// MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        /// </summary>
        /// <param name="reason">The reason why MusicBee has closed the plugin.</param>
        public void Close(PluginCloseReason reason)
        {
            log.Info(reason.ToString("G"));
            log.Close();
            _panel?.Dispose();
            _panel = null;
        }

        /// <summary>
        /// uninstall this plugin - clean up any persisted files
        /// </summary>
        public void Uninstall()
        {
            // Delete settings file
            DeleteFile(settingsStorage.GetSettingsPath());

            // Delete log file
            DeleteFile(log.GetLogFilePath());
        }


        /// <summary>
        /// Receive event notifications from MusicBee.
        /// You need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event.
        /// </summary>
        /// <param name="sourceFileUrl"></param>
        /// <param name="type"></param>
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            if (_panel == null || type == NotificationType.ApplicationWindowChanged) return;

            MetaDataType metaDataType = GetVisibleTabPageName();
            if (metaDataType == 0) return;

            switch (type)
            {
                case NotificationType.PluginStartup:
                case NotificationType.TrackChanged:
                    break;
                case NotificationType.TagsChanging:
                    if (ignoreEventFromHandler) return;

                    ignoreForBatchSelect = true;
                    mbApiInterface.Library_CommitTagsToFile(sourceFileUrl);
                    break;
            }

            tagsFromFiles = tagsManipulation.UpdateTagsFromFile(sourceFileUrl, metaDataType);

            if (type == NotificationType.TrackChanged || type == NotificationType.TagsChanging)
            {
                ignoreForBatchSelect = true;
                InvokeUpdateTagsTableData();
                ignoreForBatchSelect = false;
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
            AddSettingsLabel();
            InvokeUpdateTagsTableData();
           
            return 0;
        }

        /// <summary>
        /// Event handler triggered by MusicBee when the user selects files in the library view.
        /// </summary>
        /// <param name="filenames">List of selected files.</param>
        public void OnSelectedFilesChanged(string[] filenames)
        {
            if (_panel == null) return;

            selectedFileUrls = filenames;
            SetTagsFromFilesInPanel(filenames);
        }

        /// <summary>
        /// The presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked.
        /// </summary>
        /// <returns>Returns the list of ToolStripMenuItems that will be displayed.</returns>
        public List<ToolStripItem> GetMenuItems()
        {
            var menuItems = new List<ToolStripItem>
    {
        new ToolStripMenuItem("Tag-Panel Settings", null, MenuSettingsClicked),
        new ToolStripMenuItem("About", null, ToolstripAbout_Clicked)
    };

            return menuItems;
        }

        #endregion
    }
}
