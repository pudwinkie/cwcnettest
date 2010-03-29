namespace StartIt
{
    using Dah.Util;
    using Dah.Windows.Forms;
    using StartIt.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private ShortcutService _apps;
        private DragHelper _dragger;
        private DragHelper _dragger2;
        private List<ListViewItem> _items;
        private Button button1;
        private Button button2;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private ImageList imageList1;
        private Label label1;
        private LinearGradientPanel linearGradientPanel1;
        private ListView listView1;
        private VoidStringDelegate search;
        private TextBox textBox1;
        private TextBox textBox2;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private VoidStringDelegate waitAndSearch;
        private ToolStripMenuItem 退出ToolStripMenuItem;
        private ToolStripMenuItem 关于ToolStripMenuItem;

        public Form1()
        {
            this.InitializeComponent();
            this._apps = new ShortcutService();
            this._items = new List<ListViewItem>();
        }

        private void _apps_GetAppsFinish(object sender, EventArgs e)
        {
            VoidVoidDelegate method = new VoidVoidDelegate(this.ReadApps);
            base.BeginInvoke(method);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SwitchVisible();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                base.BeginInvoke(this.search, new object[] { this.textBox1.Text });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.listView1 = new ListView();
            this.imageList1 = new ImageList(this.components);
            this.textBox1 = new TextBox();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.关于ToolStripMenuItem = new ToolStripMenuItem();
            this.退出ToolStripMenuItem = new ToolStripMenuItem();
            this.textBox2 = new TextBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.linearGradientPanel1 = new LinearGradientPanel();
            this.label1 = new Label();
            this.contextMenuStrip1.SuspendLayout();
            this.linearGradientPanel1.SuspendLayout();
            base.SuspendLayout();
            this.listView1.BackColor = Color.White;
            this.listView1.Font = new Font("Tahoma", 11f, FontStyle.Regular, GraphicsUnit.Pixel, 0x86);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new Point(0, 0x22);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x10d, 330);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.TileSize = new Size(250, 0x12);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Tile;
            this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyDown += new KeyEventHandler(this.listView1_KeyDown);
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new Size(0x10, 0x10);
            this.imageList1.TransparentColor = Color.Transparent;
            this.textBox1.Location = new Point(3, 0x171);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xbd, 0x16);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new KeyEventHandler(this.textBox1_KeyDown);
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItem2, this.toolStripMenuItem1, this.关于ToolStripMenuItem, this.退出ToolStripMenuItem });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(0xab, 0x4c);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(170, 0x16);
            this.toolStripMenuItem2.Text = "刷新应用程序列表";
            this.toolStripMenuItem2.Click += new EventHandler(this.toolStripMenuItem2_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(0xa7, 6);
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new Size(170, 0x16);
            this.关于ToolStripMenuItem.Text = "关于...";
            this.关于ToolStripMenuItem.Click += new EventHandler(this.关于ToolStripMenuItem_Click);
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new Size(170, 0x16);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new EventHandler(this.退出ToolStripMenuItem_Click);
            this.textBox2.BackColor = SystemColors.GrayText;
            this.textBox2.Font = new Font("Tahoma", 8f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBox2.Location = new Point(0, 370);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new Size(0x10a, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Visible = false;
            this.button1.BackColor = Color.WhiteSmoke;
            this.button1.Font = new Font("Tahoma", 8f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.button1.Image = Resources.FillDownHS;
            this.button1.Location = new Point(0xea, 0x16f);
            this.button1.Name = "button1";
            this.button1.Padding = new Padding(1, 0, 0, 1);
            this.button1.Size = new Size(0x21, 0x1a);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.BackColor = Color.WhiteSmoke;
            this.button2.Font = new Font("Tahoma", 8f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.button2.Image = Resources.ZoomHS;
            this.button2.Location = new Point(0xc3, 0x16f);
            this.button2.Name = "button2";
            this.button2.Padding = new Padding(1, 0, 0, 1);
            this.button2.Size = new Size(0x24, 0x1a);
            this.button2.TabIndex = 4;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.linearGradientPanel1.Controls.Add(this.label1);
            this.linearGradientPanel1.Dock = DockStyle.Top;
            this.linearGradientPanel1.LinearGradientAngle = 90f;
            this.linearGradientPanel1.LinearGradientColor1 = Color.CornflowerBlue;
            this.linearGradientPanel1.LinearGradientColor2 = Color.RoyalBlue;
            this.linearGradientPanel1.Location = new Point(0, 0);
            this.linearGradientPanel1.Name = "linearGradientPanel1";
            this.linearGradientPanel1.Size = new Size(0x10d, 0x1f);
            this.linearGradientPanel1.TabIndex = 6;
            this.label1.BackColor = Color.Transparent;
            this.label1.ContextMenuStrip = this.contextMenuStrip1;
            this.label1.Dock = DockStyle.Fill;
            this.label1.Font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label1.ForeColor = SystemColors.ActiveCaptionText;
            this.label1.Image = Resources.PrintPreviewHS;
            this.label1.ImageAlign = ContentAlignment.MiddleLeft;
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x10d, 0x1f);
            this.label1.TabIndex = 3;
            this.label1.Text = "    Start It (载入程序列表中...)";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            base.AutoScaleDimensions = new SizeF(7f, 14f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x10d, 0x18b);
            base.ControlBox = false;
            base.Controls.Add(this.linearGradientPanel1);
            base.Controls.Add(this.listView1);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.textBox2);
            this.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "Form1";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.TransparencyKey = Color.Magenta;
            this.contextMenuStrip1.ResumeLayout(false);
            this.linearGradientPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.OpenSelectedApp();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.textBox1.Focus();
                this.textBox1.SelectAll();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OpenSelectedApp();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                try
                {
                    Shortcut tag = (Shortcut) this.listView1.SelectedItems[0].Tag;
                    string path = tag.AppPath + tag.Arguments;
                    int num = 50;
                    if (path.Length > num)
                    {
                        string fileName = Path.GetFileName(path);
                        path = path.Substring(0, num - fileName.Length) + @"...\" + fileName;
                    }
                    this.textBox2.Text = path;
                }
                catch
                {
                    this.textBox2.Text = "";
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.SwitchVisible();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            UnregisterHotKey(base.Handle, 100);
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.waitAndSearch = new VoidStringDelegate(this.WaitAndSearch);
            this.search = new VoidStringDelegate(this.ShowSearchResult);
            this._dragger = new DragHelper(this, this, true);
            this._dragger2 = new DragHelper(this.label1, this, false);
            RegisterHotKey(base.Handle, 100, KeyModifiers.Windows, Keys.A);
            this._apps.GetAppsFinish += new EventHandler(this._apps_GetAppsFinish);
            this.RefreshApps();
        }

        private void OpenSelectedApp()
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                Shortcut tag = (Shortcut) this.listView1.SelectedItems[0].Tag;
                try
                {
                    Process.Start(tag.AppPath, tag.Arguments);
                }
                catch
                {
                }
            }
        }

        private void ReadApps()
        {
            this._items.Clear();
            this.imageList1.Images.Clear();
            this.imageList1.Images.Add(Resource.App);
            foreach (Shortcut shortcut in this._apps.AppLinks)
            {
                this.imageList1.Images.Add(shortcut.AppIcon);
                ListViewItem item = new ListViewItem(shortcut.ShortcutText, this.imageList1.Images.Count - 1);
                item.Tag = shortcut;
                item.ToolTipText = shortcut.AppPath + shortcut.Arguments;
                this._items.Add(item);
            }
            this.button2.Enabled = true;
            this.textBox1.Enabled = true;
            this.textBox1.Focus();
            this.label1.Text = "    Start It";
            this.toolStripMenuItem2.Enabled = true;
        }

        private void RefreshApps()
        {
            this.toolStripMenuItem2.Enabled = false;
            this.listView1.Items.Clear();
            this.label1.Text = "    Start It (载入程序列表中...)";
            this._apps.BeginGetApps();
            this.textBox1.Enabled = false;
            this.button2.Enabled = false;
        }

        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        private void ShowSearchResult(string text)
        {
            if (this.textBox1.Text.ToLower() == text)
            {
                text = text.ToLower();
                this.listView1.Items.Clear();
                Shortcut tag = null;
                foreach (ListViewItem item in this._items)
                {
                    if (!(this.textBox1.Text.ToLower() == text) || (this.listView1.Items.Count > 50))
                    {
                        break;
                    }
                    tag = (Shortcut) item.Tag;
                    if (tag.ShortcutText.ToLower().Contains(text))
                    {
                        this.listView1.Items.Add(item);
                    }
                }
                foreach (ListViewItem item2 in this._items)
                {
                    if (!(this.textBox1.Text.ToLower() == text) || (this.listView1.Items.Count > 50))
                    {
                        break;
                    }
                    tag = (Shortcut) item2.Tag;
                    if (tag.AppFileName.ToLower().Contains(text) && !this.listView1.Items.Contains(item2))
                    {
                        this.listView1.Items.Add(item2);
                    }
                }
                foreach (ListViewItem item3 in this._items)
                {
                    if (!(this.textBox1.Text.ToLower() == text) || (this.listView1.Items.Count > 50))
                    {
                        break;
                    }
                    tag = (Shortcut) item3.Tag;
                    if (tag.AppPath.ToLower().Contains(text) && !this.listView1.Items.Contains(item3))
                    {
                        this.listView1.Items.Add(item3);
                    }
                }
                Shortcut shortcut2 = new Shortcut();
                shortcut2.ShortcutText = "执行或打开 " + text;
                shortcut2.AppPath = text;
                ListViewItem item4 = new ListViewItem();
                item4.Text = "执行或打开 " + text + "   (Ctrl+Enter)";
                item4.ImageIndex = 0;
                item4.Tag = shortcut2;
                this.listView1.Items.Add(item4);
            }
        }

        private void SwitchVisible()
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.Visible = true;
                base.WindowState = FormWindowState.Normal;
                this.textBox1.Focus();
                this.textBox1.SelectAll();
            }
            else
            {
                base.WindowState = FormWindowState.Minimized;
                base.Visible = false;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.Return))
            {
                try
                {
                    Process.Start(this.textBox1.Text.Trim());
                }
                catch
                {
                }
            }
            else if ((((e.KeyCode == Keys.Return) || (e.KeyCode == Keys.Down)) || (e.KeyCode == Keys.Up)) && (this.listView1.Items.Count > 0))
            {
                this.listView1.Focus();
                this.listView1.Items[0].Selected = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text))
            {
                this.waitAndSearch.BeginInvoke(this.textBox1.Text, null, null);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.RefreshApps();
        }

        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private void WaitAndSearch(string text)
        {
            Thread.Sleep(200);
            if (this.textBox1.Text == text)
            {
                base.BeginInvoke(this.search, new object[] { text });
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x312)
            {
                this.SwitchVisible();
            }
            base.WndProc(ref m);
        }

        private void 打开StartItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Close();
            Application.Exit();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Start It v0.2\nProgrammed by Dah\n(c)2006\nE-mail:sanyuexx@hotmail.com", "关于Start It", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        [Flags]
        public enum KeyModifiers
        {
            Alt = 1,
            Control = 2,
            None = 0,
            Shift = 4,
            Windows = 8
        }

        private delegate void VoidStringDelegate(string text);

        private delegate void VoidVoidDelegate();
    }
}

