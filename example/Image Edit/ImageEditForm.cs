using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace dog
{
    public class ImageEditForm : Form
    {
        #region ActionType

        private enum ActionType
        {
            Draw = 0,
            Dropper = 1,
            Fill = 2,
            Cross = 3,
            Line = 4,
            Rect = 5,
            RectFill = 6,
            RectFill2 = 7,
            Ellipse = 8,
            EllipseFill = 9,
            EllipseFill2 = 10,
            Select = 11,
            Selected = 12,
            DrawString = 13,
        }

        #endregion

        #region MyImage

        private class MyImage
        {
            public Image IMG;

            public MyImage()
            {
                IMG = null;
            }

            public MyImage(Image img)
            {
                IMG = img;
            }
        }

        #endregion

        #region ImageArray

        internal class ImageArray
        {
            private ArrayList m_arrayList;

            public ImageArray()
            {
                m_arrayList = new ArrayList();
            }

            internal bool GetImage(out Image img)
            {
                img = null;

                if (m_arrayList == null || m_arrayList.Count == 0)
                    return false;

                int i = m_arrayList.Count - 1;
                MyImage _m = m_arrayList[i] as MyImage;
                m_arrayList.RemoveAt(i);
                img = _m.IMG;
                return true;
            }

            internal void ClearImage()
            {
                for (int i = m_arrayList.Count - 1; i >= 0; i--)
                    ((MyImage)m_arrayList[i]).IMG.Dispose();

                m_arrayList.Clear();
            }

            internal void AddImage(Image img)
            {
                m_arrayList.Add(new MyImage(img));
            }
        }

        #endregion

        #region Fields

        private int m_iMax = 256;
        private NumericUpDown udHeight;
        private NumericUpDown udWidth;
        private Panel pnlPalette;
        private Panel pnlCurrentColor;
        private PictureBox pbDisplay;
        private PictureBox pbPreview;
        private MenuItem menuSetBGColor;
        private MenuItem menuCreateCustomColor;
        private ColorDialog colorDialog;
        private ContextMenu contextMenu;
        private Label lblHeight;
        private Label lblWidth;
        private Bitmap m_bmDisplay;
        private Bitmap m_bmReal;
        private Bitmap bmUndo;
        private Bitmap bmSelect;
        private Rectangle rSelect;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton2;
        private ToolBarButton toolBarButton3;
        private ToolBarButton toolBarButton4;
        private ToolBar toolBar1;
        private ToolBarButton toolBarButton0;
        private ToolBarButton toolBarButton5;
        private ToolBarButton toolBarButton6;
        private ToolBarButton toolBarButton7;
        private ToolBarButton toolBarButton8;
        private ToolBarButton toolBarButton9;
        private ToolBarButton toolBarButton10;
        private ToolBarButton toolBarButton11;
        private StatusBar statusBar1;
        private StatusBarPanel statusBarPanel1;
        private StatusBarPanel statusBarPanel2;
        private StatusBarPanel statusBarPanel3;
        private ImageList imageList1;
        private IContainer components;
        private ToolBar toolBar2;
        private ImageList imageList2;
        private ToolBarButton toolBarButton100;
        private ToolBarButton toolBarButton101;
        private ToolBarButton toolBarButton102;
        private ToolBarButton toolBarButton104;
        private ToolBarButton toolBarButton105;
        private ToolBarButton toolBarButton106;
        private ToolBarButton toolBarButton107;
        private ToolBarButton toolBarButton103;
        private ToolBarButton toolBarButton108;
        private ToolBarButton toolBarButton109;
        private ToolBarButton toolBarButton110;
        private ToolBarButton toolBarButton111;
        private ToolBarButton toolBarButton112;
        private ToolBarButton toolBarButton113;
        private ToolBarButton toolBarButton114;
        private ToolBarButton toolBarButton12;
        private ToolBarButton toolBarButton13;

        private bool m_dirty;
        private bool m_is_udHeight;
        private Bitmap m_bmTemp;
        private Rectangle rTransparent = Rectangle.Empty;
        private Color[] m_colorArray = new[] { Color.Black, Color.White, Color.Silver, Color.Gray, Color.Yellow, Color.Olive, Color.Red, Color.Maroon, Color.Lime, Color.Green, Color.Aqua, Color.Teal, Color.Blue, Color.Navy, Color.Magenta, Color.Purple, Color.White, Color.White, Color.White, Color.White };
        private int m_cX;
        private int m_cY;
        private int m_zoom;
        private Color m_transparentColor = Color.Transparent;
        private Color m_foreColor = Color.Red;
        private Color m_backColor = Color.Transparent;
        private ActionType m_actionType = ActionType.Draw;
        private ImageArray m_imageArray;
        private bool m_isDrag;
        private Point m_ptBegin = Point.Empty;
        private Point m_ptEnd = Point.Empty;

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                const int dropShadow = 0x00020000;
                CreateParams param = base.CreateParams;
                param.ClassStyle |= dropShadow;

                return param;
            }
        }

        public ImageEditForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            InitImage();

            m_dirty = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_bmDisplay != null)
                    m_bmDisplay.Dispose();

                if (m_bmReal != null)
                    m_bmReal.Dispose();

                if (bmUndo != null)
                    bmUndo.Dispose();

                if (bmSelect != null)
                    bmSelect.Dispose();

                if (m_bmTemp != null)
                    m_bmTemp.Dispose();

                if (m_imageArray != null)
                {
                    m_imageArray.ClearImage();
                    m_imageArray = null;
                }

                if (pbDisplay.Image != null)
                    pbDisplay.Image.Dispose();

                if (pbPreview.Image != null)
                    pbPreview.Image.Dispose();
            }

            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEditForm));
            this.udHeight = new System.Windows.Forms.NumericUpDown();
            this.udWidth = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.pbDisplay = new System.Windows.Forms.PictureBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.pnlPalette = new System.Windows.Forms.Panel();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuSetBGColor = new System.Windows.Forms.MenuItem();
            this.menuCreateCustomColor = new System.Windows.Forms.MenuItem();
            this.pnlCurrentColor = new System.Windows.Forms.Panel();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton0 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel3 = new System.Windows.Forms.StatusBarPanel();
            this.toolBar2 = new System.Windows.Forms.ToolBar();
            this.toolBarButton113 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton13 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton100 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton101 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton102 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton103 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton104 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton105 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton106 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton107 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton108 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton109 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton110 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton111 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton112 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton114 = new System.Windows.Forms.ToolBarButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.udHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).BeginInit();
            this.SuspendLayout();
            // 
            // udHeight
            // 
            this.udHeight.Location = new System.Drawing.Point(483, 8);
            this.udHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.udHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udHeight.Name = "udHeight";
            this.udHeight.ReadOnly = true;
            this.udHeight.Size = new System.Drawing.Size(48, 22);
            this.udHeight.TabIndex = 0;
            this.udHeight.TabStop = false;
            this.udHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.udHeight.ValueChanged += new System.EventHandler(this.ud_ValueChanged);
            // 
            // udWidth
            // 
            this.udWidth.Location = new System.Drawing.Point(380, 8);
            this.udWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.udWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udWidth.Name = "udWidth";
            this.udWidth.ReadOnly = true;
            this.udWidth.Size = new System.Drawing.Size(48, 22);
            this.udWidth.TabIndex = 1;
            this.udWidth.TabStop = false;
            this.udWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.udWidth.ValueChanged += new System.EventHandler(this.ud_ValueChanged);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(434, 10);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(43, 14);
            this.lblHeight.TabIndex = 2;
            this.lblHeight.Text = "Height";
            this.lblHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(334, 10);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(40, 14);
            this.lblWidth.TabIndex = 3;
            this.lblWidth.Text = "Width";
            this.lblWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbDisplay
            // 
            this.pbDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbDisplay.Location = new System.Drawing.Point(88, 40);
            this.pbDisplay.Name = "pbDisplay";
            this.pbDisplay.Size = new System.Drawing.Size(777, 560);
            this.pbDisplay.TabIndex = 4;
            this.pbDisplay.TabStop = false;
            this.pbDisplay.MouseLeave += new System.EventHandler(this.pbDisplay_MouseLeave);
            this.pbDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbDisplay_MouseMove);
            this.pbDisplay.Resize += new System.EventHandler(this.pbDisplay_Resize);
            this.pbDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbDisplay_MouseDown);
            this.pbDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbDisplay_MouseUp);
            // 
            // pbPreview
            // 
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPreview.Location = new System.Drawing.Point(8, 40);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(72, 96);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPreview.TabIndex = 5;
            this.pbPreview.TabStop = false;
            this.pbPreview.MouseLeave += new System.EventHandler(this.pbPreview_MouseLeave);
            this.pbPreview.MouseEnter += new System.EventHandler(this.pbPreview_MouseEnter);
            // 
            // pnlPalette
            // 
            this.pnlPalette.Location = new System.Drawing.Point(8, 272);
            this.pnlPalette.Name = "pnlPalette";
            this.pnlPalette.Size = new System.Drawing.Size(72, 88);
            this.pnlPalette.TabIndex = 10;
            this.pnlPalette.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPalette_Paint);
            this.pnlPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlPalette_MouseDown);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSetBGColor,
            this.menuCreateCustomColor});
            // 
            // menuSetBGColor
            // 
            this.menuSetBGColor.DefaultItem = true;
            this.menuSetBGColor.Index = 0;
            this.menuSetBGColor.Text = "Set As Back Color";
            this.menuSetBGColor.Click += new System.EventHandler(this.menuSetBGColor_Click);
            // 
            // menuCreateCustomColor
            // 
            this.menuCreateCustomColor.Index = 1;
            this.menuCreateCustomColor.Text = "Create Custom Color";
            this.menuCreateCustomColor.Click += new System.EventHandler(this.menuCreateCustomColor_Click);
            // 
            // pnlCurrentColor
            // 
            this.pnlCurrentColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlCurrentColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlCurrentColor.Location = new System.Drawing.Point(8, 553);
            this.pnlCurrentColor.Name = "pnlCurrentColor";
            this.pnlCurrentColor.Size = new System.Drawing.Size(72, 48);
            this.pnlCurrentColor.TabIndex = 23;
            this.pnlCurrentColor.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCurrentColor_Paint);
            this.pnlCurrentColor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlCurrentColor_MouseDown);
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton0,
            this.toolBarButton1,
            this.toolBarButton2,
            this.toolBarButton3,
            this.toolBarButton4,
            this.toolBarButton5,
            this.toolBarButton6,
            this.toolBarButton7,
            this.toolBarButton8,
            this.toolBarButton9,
            this.toolBarButton10,
            this.toolBarButton11,
            this.toolBarButton12});
            this.toolBar1.ButtonSize = new System.Drawing.Size(16, 16);
            this.toolBar1.Divider = false;
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(8, 152);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(72, 114);
            this.toolBar1.TabIndex = 24;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton0
            // 
            this.toolBarButton0.ImageIndex = 0;
            this.toolBarButton0.Name = "toolBarButton0";
            this.toolBarButton0.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton0.Tag = "0";
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 1;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Pushed = true;
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton1.Tag = "1";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 2;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton2.Tag = "2";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.ImageIndex = 3;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton3.Tag = "3";
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.ImageIndex = 4;
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton4.Tag = "4";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.ImageIndex = 5;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton5.Tag = "5";
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.ImageIndex = 6;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton6.Tag = "6";
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 7;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton7.Tag = "7";
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.ImageIndex = 8;
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton8.Tag = "8";
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.ImageIndex = 9;
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton9.Tag = "9";
            // 
            // toolBarButton10
            // 
            this.toolBarButton10.ImageIndex = 10;
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton10.Tag = "10";
            // 
            // toolBarButton11
            // 
            this.toolBarButton11.ImageIndex = 11;
            this.toolBarButton11.Name = "toolBarButton11";
            this.toolBarButton11.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton11.Tag = "11";
            // 
            // toolBarButton12
            // 
            this.toolBarButton12.ImageIndex = 12;
            this.toolBarButton12.Name = "toolBarButton12";
            this.toolBarButton12.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton12.Tag = "12";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Silver;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 606);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2,
            this.statusBarPanel3});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(872, 20);
            this.statusBar1.TabIndex = 25;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Text = "Ready";
            this.statusBarPanel1.Width = 118;
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.Name = "statusBarPanel2";
            this.statusBarPanel2.Width = 128;
            // 
            // statusBarPanel3
            // 
            this.statusBarPanel3.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel3.Name = "statusBarPanel3";
            this.statusBarPanel3.Width = 609;
            // 
            // toolBar2
            // 
            this.toolBar2.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar2.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton113,
            this.toolBarButton13,
            this.toolBarButton100,
            this.toolBarButton101,
            this.toolBarButton102,
            this.toolBarButton103,
            this.toolBarButton104,
            this.toolBarButton105,
            this.toolBarButton106,
            this.toolBarButton107,
            this.toolBarButton108,
            this.toolBarButton109,
            this.toolBarButton110,
            this.toolBarButton111,
            this.toolBarButton112,
            this.toolBarButton114});
            this.toolBar2.Divider = false;
            this.toolBar2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBar2.DropDownArrows = true;
            this.toolBar2.ImageList = this.imageList2;
            this.toolBar2.Location = new System.Drawing.Point(8, 8);
            this.toolBar2.Name = "toolBar2";
            this.toolBar2.ShowToolTips = true;
            this.toolBar2.Size = new System.Drawing.Size(320, 26);
            this.toolBar2.TabIndex = 26;
            this.toolBar2.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar2_ButtonClick);
            // 
            // toolBarButton113
            // 
            this.toolBarButton113.Name = "toolBarButton113";
            this.toolBarButton113.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton13
            // 
            this.toolBarButton13.ImageIndex = 11;
            this.toolBarButton13.Name = "toolBarButton13";
            this.toolBarButton13.Tag = "11";
            // 
            // toolBarButton100
            // 
            this.toolBarButton100.ImageIndex = 0;
            this.toolBarButton100.Name = "toolBarButton100";
            this.toolBarButton100.Tag = "0";
            // 
            // toolBarButton101
            // 
            this.toolBarButton101.ImageIndex = 1;
            this.toolBarButton101.Name = "toolBarButton101";
            this.toolBarButton101.Tag = "1";
            // 
            // toolBarButton102
            // 
            this.toolBarButton102.Name = "toolBarButton102";
            this.toolBarButton102.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton103
            // 
            this.toolBarButton103.ImageIndex = 2;
            this.toolBarButton103.Name = "toolBarButton103";
            this.toolBarButton103.Tag = "2";
            // 
            // toolBarButton104
            // 
            this.toolBarButton104.ImageIndex = 3;
            this.toolBarButton104.Name = "toolBarButton104";
            this.toolBarButton104.Tag = "3";
            // 
            // toolBarButton105
            // 
            this.toolBarButton105.ImageIndex = 4;
            this.toolBarButton105.Name = "toolBarButton105";
            this.toolBarButton105.Tag = "4";
            // 
            // toolBarButton106
            // 
            this.toolBarButton106.ImageIndex = 5;
            this.toolBarButton106.Name = "toolBarButton106";
            this.toolBarButton106.Tag = "5";
            // 
            // toolBarButton107
            // 
            this.toolBarButton107.ImageIndex = 6;
            this.toolBarButton107.Name = "toolBarButton107";
            this.toolBarButton107.Tag = "6";
            // 
            // toolBarButton108
            // 
            this.toolBarButton108.Name = "toolBarButton108";
            this.toolBarButton108.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton109
            // 
            this.toolBarButton109.ImageIndex = 7;
            this.toolBarButton109.Name = "toolBarButton109";
            this.toolBarButton109.Tag = "7";
            // 
            // toolBarButton110
            // 
            this.toolBarButton110.ImageIndex = 8;
            this.toolBarButton110.Name = "toolBarButton110";
            this.toolBarButton110.Tag = "8";
            // 
            // toolBarButton111
            // 
            this.toolBarButton111.ImageIndex = 9;
            this.toolBarButton111.Name = "toolBarButton111";
            this.toolBarButton111.Tag = "9";
            // 
            // toolBarButton112
            // 
            this.toolBarButton112.ImageIndex = 10;
            this.toolBarButton112.Name = "toolBarButton112";
            this.toolBarButton112.Tag = "10";
            // 
            // toolBarButton114
            // 
            this.toolBarButton114.Name = "toolBarButton114";
            this.toolBarButton114.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Silver;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            this.imageList2.Images.SetKeyName(5, "");
            this.imageList2.Images.SetKeyName(6, "");
            this.imageList2.Images.SetKeyName(7, "");
            this.imageList2.Images.SetKeyName(8, "");
            this.imageList2.Images.SetKeyName(9, "");
            this.imageList2.Images.SetKeyName(10, "");
            this.imageList2.Images.SetKeyName(11, "");
            // 
            // ImageEditForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(872, 626);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.toolBar2);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.pnlCurrentColor);
            this.Controls.Add(this.pnlPalette);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.udWidth);
            this.Controls.Add(this.udHeight);
            this.Controls.Add(this.pbDisplay);
            this.Controls.Add(this.toolBar1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 480);
            this.Name = "ImageEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Editor - [Untitled]";
            this.Load += new System.EventHandler(this.ImageEditForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ImageEditForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.udHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public void InitImage()
        {
            if (m_bmDisplay == null)
                m_bmDisplay = new Bitmap(pbDisplay.Width, pbDisplay.Height);

            m_bmReal = new Bitmap((int)udWidth.Value, (int)udHeight.Value);

            AdjZoom();
            DoRefresh();
        }

        private void ImageEditForm_Load(object sender, EventArgs e)
        {
            m_imageArray = new ImageArray();
        }

        private void DoSetPixel(Point ptPixel)
        {
            Graphics _g = Graphics.FromImage(m_bmDisplay);
            int i = ptPixel.X;
            int j = ptPixel.Y;
            Color _color = m_bmReal.GetPixel(i, j);
            Brush _brush;

            if (_color.A == 0)
            {
                _brush = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Gray, Color.White);
                _g.FillRectangle(_brush, i * m_zoom + i, j * m_zoom + j, m_zoom, m_zoom);
            }
            else
            {
                _brush = new SolidBrush(_color);
                _g.FillRectangle(_brush, i * m_zoom + i, j * m_zoom + j, m_zoom, m_zoom);
            }

            _brush.Dispose();
            _g.Dispose();

            if (pbDisplay.Image != null)
            {
                pbDisplay.Image.Dispose();
                pbDisplay.Image = null;
            }

            pbDisplay.Image = (Bitmap)m_bmDisplay.Clone();

            if (pbPreview.Image != null)
            {
                pbPreview.Image.Dispose();
                pbPreview.Image = null;
            }

            pbPreview.Image = (Bitmap)m_bmReal.Clone();
        }

        private void DoRefresh()
        {
            Graphics _g = Graphics.FromImage(m_bmDisplay);
            _g.Clear(pbDisplay.BackColor);
            int i1 = m_bmReal.Width;
            int j = m_bmReal.Height;
            Brush _brush = null;
            
            for (int k = 0; k < i1; k++)
            {
                for (int i2 = 0; i2 < j; i2++)
                {
                    Color _color = m_bmReal.GetPixel(k, i2);

                    if (_brush != null)
                    {
                        _brush.Dispose();
                        _brush = null;
                    }
                    if (_color.A == 0)
                    {
                        _brush = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Gray, Color.White);
                        _g.FillRectangle(_brush, k * m_zoom + k, i2 * m_zoom + i2, m_zoom, m_zoom);
                    }
                    else
                    {
                        _brush = new SolidBrush(_color);
                        _g.FillRectangle(_brush, k * m_zoom + k, i2 * m_zoom + i2, m_zoom, m_zoom);
                    }
                }
            }

            if (_brush != null)
                _brush.Dispose();

            _g.Dispose();

            if (pbDisplay.Image != null)
            {
                pbDisplay.Image.Dispose();
                pbDisplay.Image = null;
            }

            pbDisplay.Image = (Bitmap)m_bmDisplay.Clone();

            if (pbPreview.Image != null)
            {
                pbPreview.Image.Dispose();
                pbPreview.Image = null;
            }

            pbPreview.Image = (Bitmap)m_bmReal.Clone();
        }

        private void AdjZoom()
        {
            int i = (int)(pbDisplay.Width / udWidth.Value);
            i--;
            int j = (int)(pbDisplay.Height / udHeight.Value);
            j--;

            m_zoom = i > j ? j : i;

            if (m_zoom == 0)
                m_zoom = 1;
        }

        private void DoFill(int iFillX, int iFillY, Color clrOld, Color clrNew)
        {
            if (iFillX < 0 || iFillX >= m_bmReal.Width)
                return;

            if (iFillY < 0 || iFillY >= m_bmReal.Height)
                return;

            if (m_bmReal.GetPixel(iFillX, iFillY).ToArgb() != clrOld.ToArgb())
                return;

            m_bmReal.SetPixel(iFillX, iFillY, clrNew);
            DoFill(iFillX, iFillY - 1, clrOld, clrNew);
            DoFill(iFillX - 1, iFillY, clrOld, clrNew);
            DoFill(iFillX + 1, iFillY, clrOld, clrNew);
            DoFill(iFillX, iFillY + 1, clrOld, clrNew);
        }

        private void DoCross(int iPatternX, int iPatternY, Color clrPattern)
        {
            MySetPixel(iPatternX, iPatternY, clrPattern);
            MySetPixel(iPatternX + 1, iPatternY + 1, clrPattern);
            MySetPixel(iPatternX - 1, iPatternY + 1, clrPattern);
            MySetPixel(iPatternX + 1, iPatternY - 1, clrPattern);
            MySetPixel(iPatternX - 1, iPatternY - 1, clrPattern);
        }

        private void MySetPixel(int iX, int iY, Color clr)
        {
            if (iX < 0 || iX >= m_bmReal.Width)
                return;

            if (iY < 0 || iY >= m_bmReal.Height)
                return;

            m_bmReal.SetPixel(iX, iY, clr);
        }

        private void ud_ValueChanged(object sender, EventArgs e)
        {
            if (m_is_udHeight)
                return;

            DoDirty();

            AdjZoom();

            Bitmap _bitmap = new Bitmap((int)udWidth.Value, (int)udHeight.Value);
            Graphics _g = Graphics.FromImage(_bitmap);
            _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
            _g.DrawImageUnscaled(m_bmReal, 0, 0);
            _g.Dispose();
            m_bmReal.Dispose();
            m_bmReal = (Bitmap)_bitmap.Clone();
            _bitmap.Dispose();
            DoRefresh();
        }

        private void pbDisplay_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            if (pbDisplay.Width == 0 || pbDisplay.Height == 0)
                return;

            m_bmDisplay.Dispose();
            m_bmDisplay = new Bitmap(pbDisplay.Width, pbDisplay.Height);
            AdjZoom();
            DoRefresh();
        }

        private void DrawText(Point pt)
        {
            StringInput s = new StringInput();
            s.ShowDialog();

            if (s.DialogResult == DialogResult.Yes)
            {
                Rectangle _r = Rectangle.Empty;
                _r.Location = pt;
                _r.Size = m_bmReal.Size;
                _r.Inflate(2, 2);
                m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
                Graphics _g = Graphics.FromImage(m_bmReal);
                Brush _brush = new SolidBrush(m_foreColor);
                StringFormat _stringFormat = new StringFormat(StringFormat.GenericTypographic);
                Font _f = new Font(s.selectedFont, Convert.ToInt32(s.txtFontSize.SelectedItem));
                _g.DrawString(s.txtPreviewText.Text, _f, _brush, _r, _stringFormat);
                _brush.Dispose();
                _g.Dispose();
                DoRefresh();
            }
        }

        private void pbDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            DoDirty();

            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;
            
            Point _pt1 = Point.Empty;
            _pt1.X = e.X;
            _pt1.Y = e.Y;
            
            Point _pt2 = Point.Empty;
            _pt2.X = _pt1.X / (m_zoom + 1);
            _pt2.Y = _pt1.Y / (m_zoom + 1);
            
            Rectangle _r = Rectangle.Empty;
            _r.Location = Point.Empty;
            _r.Width = m_bmReal.Width;
            _r.Height = m_bmReal.Height;

            if (_r.Contains(_pt2))
            {
                switch (m_actionType)
                {
                    case ActionType.DrawString:
                        DrawText(_pt2);
                        return;

                    case ActionType.Draw:
                        DoDrowMouseDown(_pt2);
                        return;

                    case ActionType.Fill:
                        Color _c = m_bmReal.GetPixel(_pt2.X, _pt2.Y);
                        
                        if (!_color.ToArgb().Equals(_c.ToArgb()))
                        {
                            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
                            DoFill(_pt2.X, _pt2.Y, _c, _color);
                            DoRefresh();
                            return;
                        }

                        break;

                    case ActionType.Dropper:
                        if (MouseButtons == MouseButtons.Left)
                            m_foreColor = m_bmReal.GetPixel(_pt2.X, _pt2.Y);
                        else
                            m_backColor = m_bmReal.GetPixel(_pt2.X, _pt2.Y);

                        m_actionType = ActionType.Draw;
                        toolBar1_SetPushed(1); //¤Á´«¦ÜÃ¸¹Ï
                        pnlCurrentColor.Invalidate();
                        return;

                    case ActionType.Cross:
                        m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
                        DoCross(_pt2.X, _pt2.Y, _color);
                        DoRefresh();
                        return;

                    case ActionType.Line:
                        DoLineMouseDown(_pt2);
                        return;

                    case ActionType.Rect:
                    case ActionType.RectFill:
                    case ActionType.RectFill2:
                        DoRectMouseDown(_pt2);
                        return;

                    case ActionType.Ellipse:
                    case ActionType.EllipseFill:
                    case ActionType.EllipseFill2:
                        DoEllipseMouseDown(_pt2);
                        return;

                    case ActionType.Select:
                        DoSelectMouseDown(_pt2);
                        return;

                    case ActionType.Selected:
                        DoSelectedMouseDown(_pt2);
                        return;

                    default:
                        return;
                }
            }
        }

        private void DoDrowMouseDown(Point ptLocation)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;
            m_isDrag = true;
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            m_bmReal.SetPixel(ptLocation.X, ptLocation.Y, _color);
            DoSetPixel(ptLocation);
        }

        private void DoLineMouseDown(Point ptLocation)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;
            m_isDrag = true;
            bmUndo = (Bitmap)m_bmReal.Clone();
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            m_ptBegin = ptLocation;
            m_ptEnd = ptLocation;
            m_bmReal.SetPixel(ptLocation.X, ptLocation.Y, _color);
            DoSetPixel(ptLocation);
        }

        private void DoRectMouseDown(Point ptLocation)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;
            m_isDrag = true;
            bmUndo = (Bitmap)m_bmReal.Clone();
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            m_ptBegin = ptLocation;
            m_ptEnd = ptLocation;
            m_bmReal.SetPixel(ptLocation.X, ptLocation.Y, _color);
            DoSetPixel(ptLocation);
        }

        private void DoEllipseMouseDown(Point ptLocation)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;
            m_isDrag = true;
            bmUndo = (Bitmap)m_bmReal.Clone();
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            m_ptBegin = ptLocation;
            m_ptEnd = ptLocation;
            m_bmReal.SetPixel(ptLocation.X, ptLocation.Y, _color);
            DoSetPixel(ptLocation);
        }

        private void DoSelectMouseDown(Point ptLocation)
        {
            m_isDrag = true;
            bmUndo = (Bitmap)m_bmDisplay.Clone();
            m_ptBegin = ptLocation;
            m_ptEnd = ptLocation;
            rSelect.Location = m_ptEnd;
            rSelect.Width = 1;
            rSelect.Height = 1;

            Rectangle _r = Rectangle.Empty;
            _r.X = m_ptEnd.X * (m_zoom + 1) + m_zoom / 2;
            _r.Y = m_ptEnd.Y * (m_zoom + 1) + m_zoom / 2;
            _r.Width = 4;
            _r.Height = 4;

            Graphics _g = Graphics.FromImage(m_bmDisplay);
            _g.DrawRectangle(new Pen(Color.Silver, 4.0F), _r);
            _g.Dispose();
            pbDisplay.Image = (Bitmap)m_bmDisplay.Clone();
        }

        private void DoSelectedMouseDown(Point ptLocation)
        {
            m_isDrag = true;
            m_ptBegin = ptLocation;
            m_ptEnd = ptLocation;

            if (rSelect.IsEmpty || !rSelect.Contains(ptLocation))
            {
                m_actionType = ActionType.Select;
                m_bmDisplay = (Bitmap)bmUndo.Clone();
                rSelect.Location = m_ptEnd;
                rSelect.Width = 1;
                rSelect.Height = 1;

                Rectangle _r = Rectangle.Empty;
                _r.X = m_ptEnd.X * (m_zoom + 1) + m_zoom / 2;
                _r.Y = m_ptEnd.Y * (m_zoom + 1) + m_zoom / 2;
                _r.Width = 4;
                _r.Height = 4;

                Graphics _g = Graphics.FromImage(m_bmDisplay);
                _g.DrawRectangle(new Pen(Color.Silver, 4.0F), _r);
                _g.Dispose();
                pbDisplay.Image = (Bitmap)m_bmDisplay.Clone();
                return;
            }

            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            bmSelect = new Bitmap(rSelect.Width, rSelect.Height);
            Graphics _gg = Graphics.FromImage(bmSelect);
            _gg.DrawImage(m_bmReal, 0, 0, rSelect, GraphicsUnit.Pixel);
            _gg.Dispose();
            SetRectTransparent(rSelect);
            bmUndo = (Bitmap)m_bmReal.Clone();
        }

        private void pbDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            m_isDrag = false;

            switch (m_actionType)
            {
                case ActionType.Line:
                case ActionType.Rect:
                case ActionType.RectFill:
                case ActionType.RectFill2:
                case ActionType.Ellipse:
                case ActionType.EllipseFill:
                case ActionType.EllipseFill2:
                    m_ptEnd = Point.Empty;
                    m_ptBegin = Point.Empty;

                    if (bmUndo == null)
                        return;

                    bmUndo.Dispose();
                    bmUndo = null;
                    return;

                case ActionType.Select:
                    m_ptEnd = Point.Empty;
                    m_ptBegin = Point.Empty;
                    m_actionType = ActionType.Selected;
                    return;

                case ActionType.Selected:
                    m_actionType = ActionType.Select;
                    rSelect = Rectangle.Empty;
                    break;

                default:
                    return;
            }

            if (bmSelect != null)
            {
                bmSelect.Dispose();
                bmSelect = null;
            }

            if (bmUndo != null)
            {
                bmUndo.Dispose();
                bmUndo = null;
            }

            DoRefresh();
        }

        private void pbDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            Point _pt1 = Point.Empty;
            _pt1.X = e.X;
            _pt1.Y = e.Y;

            Point _pt2 = Point.Empty;
            _pt2.X = _pt1.X / (m_zoom + 1);
            _pt2.Y = _pt1.Y / (m_zoom + 1);
            
            Rectangle _r = Rectangle.Empty;
            _r.Location = Point.Empty;
            _r.Width = m_bmReal.Width;
            _r.Height = m_bmReal.Height;

            if ((_pt2.X < m_bmReal.Width) && (_pt2.Y < m_bmReal.Height))
                statusBarPanel2.Text = _pt2.X + "," + _pt2.Y;
            else
                statusBarPanel2.Text = "";

            if (!m_isDrag)
                return;

            if (_r.Contains(_pt2))
            {
                switch (m_actionType)
                {
                    case ActionType.Draw:
                        DoDrowMouseMove(_pt2);
                        return;

                    case ActionType.Line:
                        DoLineMouseMove(_pt2);
                        return;

                    case ActionType.Rect:
                    case ActionType.RectFill:
                    case ActionType.RectFill2:
                        DoRectMouseMove(_pt2);
                        return;

                    case ActionType.Ellipse:
                    case ActionType.EllipseFill:
                    case ActionType.EllipseFill2:
                        DoEllipseMouseMove(_pt2);
                        return;

                    case ActionType.Select:
                        DoSelectMouseMove(_pt2);
                        return;

                    case ActionType.Selected:
                        DoSelectedMouseMove(_pt2);
                        return;

                    case ActionType.Dropper:
                    case ActionType.Fill:
                    case ActionType.Cross:
                        return;

                    default:
                        return;
                }
            }
        }

        private void DoDrowMouseMove(Point ptPrevious)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;
            
            if (!m_bmReal.GetPixel(ptPrevious.X, ptPrevious.Y).ToArgb().Equals(_color.ToArgb()))
            {
                m_bmReal.SetPixel(ptPrevious.X, ptPrevious.Y, _color);
                DoSetPixel(ptPrevious);
            }
        }

        private void DoLineMouseMove(Point ptPrevious)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;

            if (m_ptEnd == ptPrevious) 
                return;

            m_ptEnd = ptPrevious;

            if (m_ptEnd == m_ptBegin)
            {
                m_bmReal.SetPixel(ptPrevious.X, ptPrevious.Y, _color);
                DoSetPixel(ptPrevious);
                return;
            }

            if (m_bmReal != null)
            {
                m_bmReal.Dispose();
                m_bmReal = null;
            }

            m_bmReal = (Bitmap)bmUndo.Clone();
            Graphics _g = Graphics.FromImage(m_bmReal);
            _g.DrawLine(new Pen(new SolidBrush(_color), 1.0F), m_ptBegin, m_ptEnd);
            _g.Dispose();
            DoRefresh();
        }

        private void DoRectMouseMove(Point ptPrevious)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;

            if (m_ptEnd == ptPrevious) 
                return;

            m_ptEnd = ptPrevious;
            
            if (m_ptEnd == m_ptBegin)
            {
                if (m_actionType != ActionType.RectFill)
                    m_bmReal.SetPixel(ptPrevious.X, ptPrevious.Y, _color);
                else
                    m_bmReal.SetPixel(ptPrevious.X, ptPrevious.Y, m_transparentColor);

                DoSetPixel(ptPrevious);
                return;
            }

            m_bmReal = (Bitmap)bmUndo.Clone();
            Graphics _g = Graphics.FromImage(m_bmReal);
            Rectangle _r = Rectangle.Empty;
            
            if (m_ptBegin.X <= m_ptEnd.X)
            {
                _r.X = m_ptBegin.X;
                _r.Width = m_ptEnd.X - m_ptBegin.X;
            }
            else
            {
                _r.X = m_ptEnd.X;
                _r.Width = m_ptBegin.X - m_ptEnd.X;
            }

            if (m_ptBegin.Y <= m_ptEnd.Y)
            {
                _r.Y = m_ptBegin.Y;
                _r.Height = m_ptEnd.Y - m_ptBegin.Y;
            }
            else
            {
                _r.Y = m_ptEnd.Y;
                _r.Height = m_ptBegin.Y - m_ptEnd.Y;
            }

            switch (m_actionType)
            {
                case ActionType.Rect:
                    _g.DrawRectangle(new Pen(new SolidBrush(_color), 1.0F), _r);
                    break;
                case ActionType.RectFill2:
                    _r.Width = _r.Width + 1;
                    _r.Height = _r.Height + 1;
                    _g.FillRectangle(new SolidBrush(_color), _r);
                    break;
                case ActionType.RectFill:
                    _g.DrawRectangle(new Pen(new SolidBrush(_color), 1.0F), _r);
                    _r.X = _r.X + 1;
                    _r.Y = _r.Y + 1;
                    _r.Width = _r.Width - 1;
                    _r.Height = _r.Height - 1;
                    _g.FillRectangle(new SolidBrush(m_backColor), _r);
                    break;
            }

            _g.Dispose();
            DoRefresh();
        }

        private void DoEllipseMouseMove(Point ptPrevious)
        {
            Color _color = (MouseButtons != MouseButtons.Left) ? m_backColor : m_foreColor;

            if (m_ptEnd == ptPrevious) 
                return;

            m_ptEnd = ptPrevious;

            if (m_ptEnd == m_ptBegin)
            {
                if (m_actionType == ActionType.EllipseFill)
                    m_bmReal.SetPixel(ptPrevious.X, ptPrevious.Y, m_transparentColor);
                else
                    m_bmReal.SetPixel(ptPrevious.X, ptPrevious.Y, _color);

                DoSetPixel(ptPrevious);
                return;
            }

            m_bmReal = (Bitmap)bmUndo.Clone();
            Rectangle _r = Rectangle.Empty;

            if (m_ptBegin.X <= m_ptEnd.X)
            {
                _r.X = m_ptBegin.X;
                _r.Width = m_ptEnd.X - m_ptBegin.X;
            }
            else
            {
                _r.X = m_ptEnd.X;
                _r.Width = m_ptBegin.X - m_ptEnd.X;
            }

            if (m_ptBegin.Y <= m_ptEnd.Y)
            {
                _r.Y = m_ptBegin.Y;
                _r.Height = m_ptEnd.Y - m_ptBegin.Y;
            }
            else
            {
                _r.Y = m_ptEnd.Y;
                _r.Height = m_ptBegin.Y - m_ptEnd.Y;
            }

            Graphics _g = Graphics.FromImage(m_bmReal);
            
            switch (m_actionType)
            {
                case ActionType.Ellipse:
                    _g.DrawEllipse(new Pen(new SolidBrush(_color), 1.0F), _r);
                    break;
                case ActionType.EllipseFill2:
                    _r.Width = _r.Width + 1;
                    _r.Height = _r.Height + 1;
                    _g.FillEllipse(new SolidBrush(_color), _r);
                    break;
                case ActionType.EllipseFill:
                    _g.FillEllipse(new SolidBrush(m_backColor), _r);
                    _g.DrawEllipse(new Pen(new SolidBrush(_color), 1.0F), _r);
                    break;
            }

            _g.Dispose();
            DoRefresh();
        }

        private void DoSelectedMouseMove(Point ptPrevious)
        {
            if (m_ptEnd == ptPrevious)
                return;

            rSelect.Offset(ptPrevious.X - m_ptEnd.X, ptPrevious.Y - m_ptEnd.Y);
            m_bmReal = (Bitmap)bmUndo.Clone();
            Graphics _g = Graphics.FromImage(m_bmReal);
            _g.DrawImage(bmSelect, rSelect, 0, 0, bmSelect.Width, bmSelect.Height, GraphicsUnit.Pixel);
            _g.Dispose();
            m_ptEnd = ptPrevious;
            DoRefresh();
        }

        private void DoSelectMouseMove(Point ptPrevious)
        {
            if (m_ptEnd == ptPrevious)
                return;

            m_ptEnd = ptPrevious;
            m_bmDisplay = (Bitmap)bmUndo.Clone();
            Graphics _g = Graphics.FromImage(m_bmDisplay);

            if (m_ptEnd == m_ptBegin)
            {
                Rectangle _r = Rectangle.Empty;
                _r.X = m_ptEnd.X * (m_zoom + 1) + m_zoom / 2;
                _r.Y = m_ptEnd.Y * (m_zoom + 1) + m_zoom / 2;
                _r.Width = 4;
                _r.Height = 4;
                rSelect.Location = m_ptEnd;
                rSelect.Width = 1;
                rSelect.Height = 1;
                _g.DrawRectangle(new Pen(Color.Silver, 4.0F), _r);
            }
            else if (m_ptEnd.X == m_ptBegin.X || m_ptEnd.Y == m_ptBegin.Y)
            {
                _g.DrawLine(new Pen(Color.Silver, 4.0F), m_ptEnd.X * (m_zoom + 1) + m_zoom / 2, m_ptEnd.Y * (m_zoom + 1) + m_zoom / 2, m_ptBegin.X * (m_zoom + 1) + m_zoom / 2, m_ptBegin.Y * (m_zoom + 1) + m_zoom / 2);
                
                Rectangle _r = Rectangle.Empty;

                if (m_ptBegin.X <= m_ptEnd.X)
                {
                    _r.X = m_ptBegin.X * (m_zoom + 1) + m_zoom / 2;
                    _r.Width = (m_ptEnd.X - m_ptBegin.X) * (m_zoom + 1);
                    rSelect.X = m_ptBegin.X;
                    rSelect.Width = m_ptEnd.X - m_ptBegin.X;
                }
                else
                {
                    _r.X = m_ptEnd.X * (m_zoom + 1) + m_zoom / 2;
                    _r.Width = (m_ptBegin.X - m_ptEnd.X) * (m_zoom + 1);
                    rSelect.X = m_ptEnd.X;
                    rSelect.Width = m_ptBegin.X - m_ptEnd.X;
                }

                if (m_ptBegin.Y <= m_ptEnd.Y)
                {
                    _r.Y = m_ptBegin.Y * (m_zoom + 1) + m_zoom / 2;
                    _r.Height = (m_ptEnd.Y - m_ptBegin.Y) * (m_zoom + 1);
                    rSelect.Y = m_ptBegin.Y;
                    rSelect.Height = m_ptEnd.Y - m_ptBegin.Y;
                }
                else
                {
                    _r.Y = m_ptEnd.Y * (m_zoom + 1) + m_zoom / 2;
                    _r.Height = (m_ptBegin.Y - m_ptEnd.Y) * (m_zoom + 1);
                    rSelect.Y = m_ptEnd.Y;
                    rSelect.Height = m_ptBegin.Y - m_ptEnd.Y;
                }
            }
            else
            {
                Rectangle _r = Rectangle.Empty;

                if (m_ptBegin.X <= m_ptEnd.X)
                {
                    _r.X = m_ptBegin.X * (m_zoom + 1) + m_zoom / 2;
                    _r.Width = (m_ptEnd.X - m_ptBegin.X) * (m_zoom + 1);
                    rSelect.X = m_ptBegin.X;
                    rSelect.Width = m_ptEnd.X - m_ptBegin.X + 1;
                }
                else
                {
                    _r.X = m_ptEnd.X * (m_zoom + 1) + m_zoom / 2;
                    _r.Width = (m_ptBegin.X - m_ptEnd.X) * (m_zoom + 1);
                    rSelect.X = m_ptEnd.X;
                    rSelect.Width = m_ptBegin.X - m_ptEnd.X + 1;
                }

                if (m_ptBegin.Y <= m_ptEnd.Y)
                {
                    _r.Y = m_ptBegin.Y * (m_zoom + 1) + m_zoom / 2;
                    _r.Height = (m_ptEnd.Y - m_ptBegin.Y) * (m_zoom + 1);
                    rSelect.Y = m_ptBegin.Y;
                    rSelect.Height = m_ptEnd.Y - m_ptBegin.Y + 1;
                }
                else
                {
                    _r.Y = m_ptEnd.Y * (m_zoom + 1) + m_zoom / 2;
                    _r.Height = (m_ptBegin.Y - m_ptEnd.Y) * (m_zoom + 1);
                    rSelect.Y = m_ptEnd.Y;
                    rSelect.Height = m_ptBegin.Y - m_ptEnd.Y + 1;
                }

                _g.DrawRectangle(new Pen(Color.Silver, 4.0F), _r);
            }

            _g.Dispose();
            pbDisplay.Image = (Bitmap)m_bmDisplay.Clone();
        }

        private void ShiftLeft()
        {
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            Bitmap _bitmap = new Bitmap((int)udWidth.Value, (int)udHeight.Value);
            Graphics _g = Graphics.FromImage(_bitmap);
            _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
            _g.DrawImageUnscaled(m_bmReal, -1, 0);
            _g.Dispose();
            m_bmReal.Dispose();
            m_bmReal = (Bitmap)_bitmap.Clone();
            _bitmap.Dispose();
            DoRefresh();
        }

        private void ShiftRight()
        {
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            Bitmap _bitmap = new Bitmap((int)udWidth.Value, (int)udHeight.Value);
            Graphics _g = Graphics.FromImage(_bitmap);
            _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
            _g.DrawImageUnscaled(m_bmReal, 1, 0);
            _g.Dispose();
            m_bmReal.Dispose();
            m_bmReal = (Bitmap)_bitmap.Clone();
            _bitmap.Dispose();
            DoRefresh();
        }

        private void ShiftDown()
        {
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            Bitmap _bitmap = new Bitmap((int)udWidth.Value, (int)udHeight.Value);
            Graphics _g = Graphics.FromImage(_bitmap);
            _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
            _g.DrawImageUnscaled(m_bmReal, 0, 1);
            _g.Dispose();
            m_bmReal.Dispose();
            m_bmReal = (Bitmap)_bitmap.Clone();
            _bitmap.Dispose();
            DoRefresh();
        }

        private void ShiftUp()
        {
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            Bitmap _bitmap = new Bitmap((int)udWidth.Value, (int)udHeight.Value);
            Graphics _g = Graphics.FromImage(_bitmap);
            _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
            _g.DrawImageUnscaled(m_bmReal, 0, -1);
            _g.Dispose();
            m_bmReal.Dispose();
            m_bmReal = (Bitmap)_bitmap.Clone();
            _bitmap.Dispose();
            DoRefresh();
        }

        private void pnlPalette_Paint(object sender, PaintEventArgs e)
        {
            const int _c1 = 3;
            const int _c2 = 15;

            int _idx = 0;

            int _x = 3;
            int _y = 2;

            Bitmap _bmp = new Bitmap(pnlPalette.Width, pnlPalette.Height);
            Graphics _g = Graphics.FromImage(_bmp);
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x = _c1;
            _y += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x = _c1;
            _y += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x = _c1;
            _y += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x = _c1;
            _y += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _x += _c2 + 2;
            _g.FillRectangle(new SolidBrush(m_colorArray[_idx++]), _x, _y, _c2, _c2);
            _g.Dispose();
            e.Graphics.DrawImageUnscaled(_bmp, 0, 0);
            _bmp.Dispose();
        }

        private void pnlPalette_MouseDown(object sender, MouseEventArgs e)
        {
            int i = e.Y / 17;

            if (i > 4)
                return;

            int j = (e.X + 1) / 17;

            if (j > 3)
                return;

            if (e.Button == MouseButtons.Left)
            {
                m_foreColor = m_colorArray[i * 4 + j];
                pnlCurrentColor.Invalidate();
                return;
            }

            m_cX = j;
            m_cY = i;
            
            Point _pt = Point.Empty;
            _pt.X = e.X;
            _pt.Y = e.Y;
            contextMenu.Show(pnlPalette, _pt);
        }

        private void menuSetBGColor_Click(object sender, EventArgs e)
        {
            m_backColor = m_colorArray[m_cY * 4 + m_cX];
            pnlCurrentColor.Invalidate();
        }

        private void menuCreateCustomColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                m_colorArray[m_cY * 4 + m_cX] = colorDialog.Color;
                pnlPalette.Invalidate();
            }
        }

        private void ClearImage()
        {
            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
            Graphics _g = Graphics.FromImage(m_bmReal);
            _g.Clear(m_transparentColor);
            _g.Dispose();
            DoRefresh();
        }

        private void SetRectTransparent(Rectangle rectangle)
        {
            if (rectangle.Right > m_bmReal.Width)
                return;

            if (rectangle.Bottom > m_bmReal.Height)
                return;

            for (int i = rectangle.Left; i < rectangle.Right; i++)
            {
                for (int j = rectangle.Top; j < rectangle.Bottom; j++)
                    m_bmReal.SetPixel(i, j, m_transparentColor);
            }
        }

        private void MyNew()
        {
            if (m_dirty)
            {
                DialogResult _dr = CheckIfSave();

                if (_dr == DialogResult.Cancel)
                    return;
            }

            m_imageArray.AddImage((Bitmap)m_bmReal.Clone());

            Bitmap _bitmap = new Bitmap(32, 32);
            Graphics _g = Graphics.FromImage(_bitmap);
            _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
            _g.Dispose();
            m_bmReal.Dispose();
            m_bmReal = (Bitmap)_bitmap.Clone();
            _bitmap.Dispose();
            m_is_udHeight = true;
            udHeight.Value = 32;
            m_is_udHeight = false;
            udWidth.Value = 32;
            AdjZoom();
            DoRefresh();

            Text = "Image Editor - [Untitled]";
            m_dirty = false;
        }

        private void MyOpen()
        {
            if (m_dirty)
            {
                DialogResult _dr = CheckIfSave();
                
                if (_dr == DialogResult.Cancel)
                    return;
            }

            OpenFileDialog _dialog = new OpenFileDialog();
            _dialog.Filter = "Image files (*.bmp,*.jpg,*.jpeg,*.png,*.ico,*.emf,*.wmf,*.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.ico;*.emf;*.wmf;*.gif";
            _dialog.FilterIndex = 1;
            _dialog.RestoreDirectory = false;

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                Stream _s;

                if ((_s = _dialog.OpenFile()) != null)
                {
                    m_imageArray.AddImage((Bitmap)m_bmReal.Clone());
                    Bitmap _bm = new Bitmap(_s);

                    if (_bm.Width > 256 || _bm.Height > 256)
                    {
                        MessageBox.Show("Image file too large !", "System Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    Bitmap _bitmap = new Bitmap(_bm.Width, _bm.Height);
                    Graphics _g = Graphics.FromImage(_bitmap);
                    _g.FillRectangle(Brushes.Transparent, 0, 0, _bitmap.Width, _bitmap.Height);
                    _g.DrawImage(_bm, 0, 0, _bitmap.Width, _bitmap.Height);
                    _g.Dispose();
                    m_bmReal.Dispose();
                    m_bmReal = (Bitmap)_bitmap.Clone();
                    _bitmap.Dispose();
                    _bm.Dispose();
                    m_is_udHeight = true;
                    udHeight.Value = (m_bmReal.Height <= m_iMax) ? m_bmReal.Height : m_iMax;
                    m_is_udHeight = false;
                    udWidth.Value = (m_bmReal.Width <= m_iMax) ? m_bmReal.Width : m_iMax;
                    AdjZoom();
                    DoRefresh();
                    _s.Close();

                    Text = "Image Editor - [" + _dialog.FileName + "]";
                    m_dirty = false;
                }
            }

            _dialog.Dispose();
        }

        private void MySave()
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.AddExtension = true;
            _dialog.DefaultExt = "bmp";
            _dialog.Filter = "Bitmap Image (*.bmp)|*.bmp|Jpeg Image (*.jpeg)|*.jpeg|Png Image (*.png)|*.png|Enhanced Windows Metafile (*.emf)|*.emf|Windows Metafile (*.wmf)|*.wmf|Gif Image (*.gif)|*.gif";
            _dialog.Title = "Save Image";

            if (_dialog.ShowDialog() == DialogResult.OK)
            {
                string _fn = _dialog.FileName;
                ImageFormat _imageFormat = ImageFormat.Bmp;

                switch (_dialog.FilterIndex)
                {
                    case 2:
                        _imageFormat = ImageFormat.Jpeg;
                        break;
                    case 3:
                        _imageFormat = ImageFormat.Png;
                        break;
                    case 4:
                        _imageFormat = ImageFormat.Emf;
                        break;
                    case 5:
                        _imageFormat = ImageFormat.Wmf;
                        break;
                    case 6:
                        _imageFormat = ImageFormat.Gif;
                        break;
                }

                m_bmReal.Save(_fn, _imageFormat);

                Text = "Image Editor - [" + _dialog.FileName + "]";
                m_dirty = false;
            }
        }

        private bool FromClipboard()
        {
            IDataObject _dataObject = Clipboard.GetDataObject();

            if (_dataObject == null)
                return false;

            if (!_dataObject.GetDataPresent(DataFormats.Bitmap))
                return false;

            m_bmTemp = (Bitmap)_dataObject.GetData(DataFormats.Bitmap);
            m_bmTemp.MakeTransparent();
            return true;
        }

        private void ToClipboard()
        {
            if (m_bmTemp != null)
                Clipboard.SetDataObject(m_bmTemp, true);
        }

        private void CopyImage()
        {
            if (m_bmTemp != null)
                m_bmTemp.Dispose();

            m_bmTemp = (Bitmap)m_bmReal.Clone();
            ToClipboard();
            m_bmTemp.Dispose();
            m_bmTemp = null;
        }

        private void CutImage()
        {
            if (m_bmTemp != null)
                m_bmTemp.Dispose();

            m_bmTemp = (Bitmap)m_bmReal.Clone();
            ToClipboard();
            ClearImage();
            m_bmTemp.Dispose();
            m_bmTemp = null;
        }

        private void PasteImage()
        {
            if (FromClipboard())
            {
                ClearImage();
                m_bmReal.Dispose();
                m_bmReal = (Bitmap)m_bmTemp.Clone();
                m_is_udHeight = true;
                udHeight.Value = (m_bmReal.Height <= m_iMax) ? m_bmReal.Height : m_iMax;
                m_is_udHeight = false;
                udWidth.Value = (m_bmReal.Width <= m_iMax) ? m_bmReal.Width : m_iMax;
                AdjZoom();
                DoRefresh();
                m_bmTemp.Dispose();
                m_bmTemp = null;
            }
        }

        private void UndoImage()
        {
            Image _img;

            if (m_imageArray.GetImage(out _img))
            {
                if (!_img.Size.Equals(m_bmReal))
                {
                    m_bmReal.Dispose();
                    m_bmReal = (Bitmap)_img.Clone();
                    m_is_udHeight = true;
                    udHeight.Value = (m_bmReal.Height <= m_iMax) ? m_bmReal.Height : m_iMax;
                    m_is_udHeight = false;
                    udWidth.Value = (m_bmReal.Width <= m_iMax) ? m_bmReal.Width : m_iMax;
                    AdjZoom();
                }
                else
                {
                    m_bmReal = (Bitmap)_img.Clone();
                }

                DoRefresh();
                _img.Dispose();
                _img = null;
            }
        }

        private void pnlCurrentColor_Paint(object sender, PaintEventArgs e)
        {
            rTransparent.X = 8;
            rTransparent.Y = 8;
            rTransparent.Width = 16;
            rTransparent.Height = 16;
            e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Gray, Color.White), rTransparent);
            ControlPaint.DrawBorder3D(e.Graphics, rTransparent, Border3DStyle.SunkenInner);
            rTransparent.Inflate(1, 1);
            ControlPaint.DrawBorder3D(e.Graphics, rTransparent, Border3DStyle.SunkenOuter);
            
            Rectangle _r = Rectangle.Empty;
            _r.X = 45;
            _r.Y = 22;
            _r.Width = 16;
            _r.Height = 16;

            if (m_backColor == m_transparentColor)
                e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Gray, Color.White), _r);
            else
                e.Graphics.FillRectangle(new SolidBrush(m_backColor), _r);

            ControlPaint.DrawBorder3D(e.Graphics, _r, Border3DStyle.RaisedInner);
            _r.Inflate(1, 1);
            ControlPaint.DrawBorder3D(e.Graphics, _r, Border3DStyle.RaisedOuter);
            _r.X = 37;
            _r.Y = 12;
            _r.Width = 16;
            _r.Height = 16;

            if (m_foreColor == m_transparentColor)
                e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Gray, Color.White), _r);
            else
                e.Graphics.FillRectangle(new SolidBrush(m_foreColor), _r);

            ControlPaint.DrawBorder3D(e.Graphics, _r, Border3DStyle.RaisedInner);
            _r.Inflate(1, 1);
            ControlPaint.DrawBorder3D(e.Graphics, _r, Border3DStyle.RaisedOuter);
        }

        private void pnlCurrentColor_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = Point.Empty;
            pt.X = e.X;
            pt.Y = e.Y;

            if (rTransparent.Contains(pt))
            {
                m_foreColor = m_transparentColor;
                pnlCurrentColor.Invalidate();
                return;
            }

            pnlCurrentColor.Invalidate();
        }

        private void toolBar1_SetPushed(int i)
        {
            for (int j = 0; j < toolBar1.Buttons.Count; j++)
                toolBar1.Buttons[j].Pushed = false;

            toolBar1.Buttons[i].Pushed = true;
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            int i = Convert.ToInt32(e.Button.Tag);

            toolBar1_SetPushed(i);

            switch (i)
            {
                case 0:
                    m_actionType = ActionType.Select;
                    return;
                case 1:
                    m_actionType = ActionType.Draw;
                    return;
                case 2:
                    m_actionType = ActionType.Dropper;
                    return;
                case 3:
                    m_actionType = ActionType.Fill;
                    return;
                case 4:
                    m_actionType = ActionType.Line;
                    return;
                case 5:
                    m_actionType = ActionType.Cross;
                    return;
                case 6:
                    m_actionType = ActionType.Rect;
                    return;
                case 7:
                    m_actionType = ActionType.RectFill;
                    return;
                case 8:
                    m_actionType = ActionType.RectFill2;
                    return;
                case 9:
                    m_actionType = ActionType.Ellipse;
                    return;
                case 10:
                    m_actionType = ActionType.EllipseFill;
                    return;
                case 11:
                    m_actionType = ActionType.EllipseFill2;
                    return;
                case 12:
                    m_actionType = ActionType.DrawString;
                    return;
                default:
                    return;
            }
        }

        private void pbDisplay_MouseLeave(object sender, EventArgs e)
        {
            statusBarPanel2.Text = "";
        }

        private void toolBar2_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            int i = Convert.ToInt32(e.Button.Tag);

            switch (i)
            {
                case 0:
                    MyOpen();
                    return;
                case 1:
                    MySave();
                    return;
                case 2:
                    UndoImage();
                    DoDirty();
                    return;
                case 3:
                    CutImage();
                    DoDirty();
                    return;
                case 4:
                    CopyImage();
                    return;
                case 5:
                    PasteImage();
                    DoDirty();
                    return;
                case 6:
                    ClearImage();
                    DoDirty();
                    return;
                case 7:
                    ShiftLeft();
                    DoDirty();
                    return;
                case 8:
                    ShiftUp();
                    DoDirty();
                    return;
                case 9:
                    ShiftDown();
                    DoDirty();
                    return;
                case 10:
                    ShiftRight();
                    DoDirty();
                    return;
                case 11:
                    MyNew();
                    return;
                default:
                    return;
            }
        }

        private void ImageEditForm_Closing(object sender, CancelEventArgs e)
        {
            if (m_dirty)
            {
                DialogResult _dr = CheckIfSave();

                if (_dr == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private DialogResult CheckIfSave()
        {
            DialogResult _dr = MessageBox.Show("Save image file ?", "System Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            
            switch (_dr)
            {
                case DialogResult.Yes:
                    MySave();
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    break;
            }

            return _dr;
        }

        private void pbPreview_MouseEnter(object sender, EventArgs e)
        {
            if (udWidth.Value > 72)
                pbPreview.Width = (int)udWidth.Value + 16;

            if (udHeight.Value > 96)
                pbPreview.Height = (int)udHeight.Value + 16;
        }

        private void pbPreview_MouseLeave(object sender, EventArgs e)
        {
            pbPreview.Width = 72;
            pbPreview.Height = 96;
        }

        private void DoDirty()
        {
            m_dirty = true;

            if (Text[Text.Length - 1].CompareTo('*') != 0)
                Text += "*";
        }
    }
}