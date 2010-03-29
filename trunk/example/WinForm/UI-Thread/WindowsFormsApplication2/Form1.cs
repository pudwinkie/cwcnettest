using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication2 {
    public partial class Form1 : Form {
        private ProgressBar _ProgressBar;

        public Form1() {
            InitializeComponent();
            _ProgressBar = new ProgressBar();
            _ProgressBar.Location = new Point(13, 17);
            _ProgressBar.Size = new Size(267, 19);
            Controls.Add(this._ProgressBar);

            ThreadStart threadStart = Increment;
            threadStart.BeginInvoke(null, null);

        }

        void UpdateProgressBar() {
            if (_ProgressBar.InvokeRequired) {
                MethodInvoker updateProgressBar = UpdateProgressBar;
                _ProgressBar.Invoke(updateProgressBar);
            } else {
                _ProgressBar.Increment(1);
            }
        }

        private void Increment() {
            for (int i = 0; i < 100; i++) {
                UpdateProgressBar();
                Thread.Sleep(100);
            }

            if (InvokeRequired) {
                // Close cannot be called directly from
                // a non-UI thread.
                Invoke(new MethodInvoker(Close));
            } else {
                Close();
            }
        }

    }
}
