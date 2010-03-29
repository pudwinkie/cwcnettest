using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SoundDemo {
    public partial class Form1 : Form {
        private System.Media.SoundPlayer _player;
        private WMPLib.WindowsMediaPlayer _WMP;

        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            System.Media.SystemSounds.Beep.Play();    
        }

        private void button2_Click(object sender, EventArgs e) {
            System.Media.SystemSounds.Exclamation.Play();  
        }

        private void button3_Click(object sender, EventArgs e) {
            System.Media.SystemSounds.Hand.Play();
        }

        private void button4_Click(object sender, EventArgs e) {
            System.Media.SystemSounds.Question.Play();
            
        }



        private void button5_Click_1(object sender, EventArgs e) {
            System.Media.SystemSounds.Asterisk.Play();    
        }

        private void button6_Click(object sender, EventArgs e) {            
            _player.SoundLocation = @"data\Windows 登入音效.wav";
            _player.LoadAsync();
            _player.Play();
        }

        private void button7_Click(object sender, EventArgs e) {
            WMP_play(@"data\qus_01e12.wma");
        }

        private void groupBox2_Enter(object sender, EventArgs e) {

        }

        private void button8_Click(object sender, EventArgs e) {
            WMP_play(@"data\Windows 登入音效.wav");
        }

        private void Form1_Load(object sender, EventArgs e) {
            _player = new System.Media.SoundPlayer();
            _WMP = new WMPLib.WindowsMediaPlayer();
        }

        private void WMP_play(string url) {
            _WMP.URL = url;
            _WMP.controls.play();  
        }

        private void button9_Click(object sender, EventArgs e) {
            WMP_play(@"data\01. 哈薩雅琪(小女孩版).mp3");
        }
    }
}