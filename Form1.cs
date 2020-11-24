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
    public partial class Form1 : Form
    {
        private MusicBeePlugin.Plugin.MusicBeeApiInterface mbApiInterface;
        public Form1(MusicBeePlugin.Plugin.MusicBeeApiInterface pApi)
        {
            InitializeComponent();
            mbApiInterface = pApi;

        }

        private void state_Click(object sender, EventArgs e)
        {
            MusicBeePlugin.Plugin.Player_GetPlayStateDelegate label1Text = mbApiInterface.Player_GetPlayState;
            MusicBeePlugin.Plugin.PlayState playState = label1Text();
            this.label1.Text = playState.ToString();
            //MusicBeePlugin.Plugin.FileCodec = label2.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
