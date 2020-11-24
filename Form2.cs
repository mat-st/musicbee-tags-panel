using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Form2 : Form
    {
        private MusicBeePlugin.Plugin.MusicBeeApiInterface mbApiInterface;
        public Form2(MusicBeePlugin.Plugin.MusicBeeApiInterface pApi)
        {
            InitializeComponent();
            mbApiInterface = pApi;

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
