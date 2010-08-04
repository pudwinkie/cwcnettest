<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.tbxSource = New System.Windows.Forms.TextBox
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabCSharp = New System.Windows.Forms.TabPage
        Me.tbxCSharp = New System.Windows.Forms.TextBox
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.tabVB = New System.Windows.Forms.TabPage
        Me.tbxVB = New System.Windows.Forms.TextBox
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me.tabVC = New System.Windows.Forms.TabPage
        Me.tbxVC = New System.Windows.Forms.TextBox
        Me.ToolStrip4 = New System.Windows.Forms.ToolStrip
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.chkMemo = New System.Windows.Forms.CheckBox
        Me.chkSecureString = New System.Windows.Forms.CheckBox
        Me.chkUTF8 = New System.Windows.Forms.CheckBox
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabCSharp.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.tabVB.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.tabVC.SuspendLayout()
        Me.ToolStrip4.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SourceString"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(12, 31)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tbxSource)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(430, 354)
        Me.SplitContainer1.SplitterDistance = 115
        Me.SplitContainer1.TabIndex = 1
        '
        'tbxSource
        '
        Me.tbxSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbxSource.Location = New System.Drawing.Point(0, 25)
        Me.tbxSource.MaxLength = 1024
        Me.tbxSource.Multiline = True
        Me.tbxSource.Name = "tbxSource"
        Me.tbxSource.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbxSource.Size = New System.Drawing.Size(430, 90)
        Me.tbxSource.TabIndex = 0
        '
        'ToolStrip3
        '
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.PasteToolStripButton})
        Me.ToolStrip3.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(430, 25)
        Me.ToolStrip3.TabIndex = 2
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewToolStripButton.Image = CType(resources.GetObject("NewToolStripButton.Image"), System.Drawing.Image)
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'PasteToolStripButton
        '
        Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PasteToolStripButton.Image = CType(resources.GetObject("PasteToolStripButton.Image"), System.Drawing.Image)
        Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PasteToolStripButton.Name = "PasteToolStripButton"
        Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.PasteToolStripButton.Text = "&Paste"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabCSharp)
        Me.TabControl1.Controls.Add(Me.tabVB)
        Me.TabControl1.Controls.Add(Me.tabVC)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(430, 235)
        Me.TabControl1.TabIndex = 0
        '
        'tabCSharp
        '
        Me.tabCSharp.Controls.Add(Me.tbxCSharp)
        Me.tabCSharp.Controls.Add(Me.ToolStrip1)
        Me.tabCSharp.Location = New System.Drawing.Point(4, 22)
        Me.tabCSharp.Name = "tabCSharp"
        Me.tabCSharp.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCSharp.Size = New System.Drawing.Size(422, 209)
        Me.tabCSharp.TabIndex = 1
        Me.tabCSharp.Text = "C#"
        Me.tabCSharp.UseVisualStyleBackColor = True
        '
        'tbxCSharp
        '
        Me.tbxCSharp.BackColor = System.Drawing.Color.White
        Me.tbxCSharp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbxCSharp.Location = New System.Drawing.Point(3, 28)
        Me.tbxCSharp.Multiline = True
        Me.tbxCSharp.Name = "tbxCSharp"
        Me.tbxCSharp.ReadOnly = True
        Me.tbxCSharp.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbxCSharp.Size = New System.Drawing.Size(416, 178)
        Me.tbxCSharp.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(416, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'CopyToolStripButton
        '
        Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
        Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyToolStripButton.Name = "CopyToolStripButton"
        Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CopyToolStripButton.Text = "&Copy"
        '
        'tabVB
        '
        Me.tabVB.Controls.Add(Me.tbxVB)
        Me.tabVB.Controls.Add(Me.ToolStrip2)
        Me.tabVB.Location = New System.Drawing.Point(4, 22)
        Me.tabVB.Name = "tabVB"
        Me.tabVB.Padding = New System.Windows.Forms.Padding(3)
        Me.tabVB.Size = New System.Drawing.Size(422, 209)
        Me.tabVB.TabIndex = 0
        Me.tabVB.Text = "VB.NET"
        Me.tabVB.UseVisualStyleBackColor = True
        '
        'tbxVB
        '
        Me.tbxVB.BackColor = System.Drawing.Color.White
        Me.tbxVB.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbxVB.Location = New System.Drawing.Point(3, 28)
        Me.tbxVB.Multiline = True
        Me.tbxVB.Name = "tbxVB"
        Me.tbxVB.ReadOnly = True
        Me.tbxVB.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbxVB.Size = New System.Drawing.Size(416, 178)
        Me.tbxVB.TabIndex = 0
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1})
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(416, 25)
        Me.ToolStrip2.TabIndex = 2
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "&Copy"
        '
        'tabVC
        '
        Me.tabVC.Controls.Add(Me.tbxVC)
        Me.tabVC.Controls.Add(Me.ToolStrip4)
        Me.tabVC.Location = New System.Drawing.Point(4, 22)
        Me.tabVC.Name = "tabVC"
        Me.tabVC.Padding = New System.Windows.Forms.Padding(3)
        Me.tabVC.Size = New System.Drawing.Size(422, 209)
        Me.tabVC.TabIndex = 2
        Me.tabVC.Text = "VC.NET"
        Me.tabVC.UseVisualStyleBackColor = True
        '
        'tbxVC
        '
        Me.tbxVC.BackColor = System.Drawing.Color.White
        Me.tbxVC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbxVC.Location = New System.Drawing.Point(3, 28)
        Me.tbxVC.Multiline = True
        Me.tbxVC.Name = "tbxVC"
        Me.tbxVC.ReadOnly = True
        Me.tbxVC.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbxVC.Size = New System.Drawing.Size(416, 178)
        Me.tbxVC.TabIndex = 2
        '
        'ToolStrip4
        '
        Me.ToolStrip4.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2})
        Me.ToolStrip4.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip4.Name = "ToolStrip4"
        Me.ToolStrip4.Size = New System.Drawing.Size(416, 25)
        Me.ToolStrip4.TabIndex = 3
        Me.ToolStrip4.Text = "ToolStrip4"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton2.Text = "&Copy"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 400)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(454, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(122, 17)
        Me.ToolStripStatusLabel1.Text = "SourceString length:"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(15, 17)
        Me.ToolStripStatusLabel2.Text = "0"
        '
        'chkMemo
        '
        Me.chkMemo.AutoSize = True
        Me.chkMemo.Checked = Global.StringToByte.My.MySettings.Default.WithMemo
        Me.chkMemo.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.StringToByte.My.MySettings.Default, "WithMemo", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkMemo.Location = New System.Drawing.Point(355, 8)
        Me.chkMemo.Name = "chkMemo"
        Me.chkMemo.Size = New System.Drawing.Size(80, 17)
        Me.chkMemo.TabIndex = 5
        Me.chkMemo.Text = "With Memo"
        Me.chkMemo.UseVisualStyleBackColor = True
        '
        'chkSecureString
        '
        Me.chkSecureString.AutoSize = True
        Me.chkSecureString.Checked = Global.StringToByte.My.MySettings.Default.WithSecureString
        Me.chkSecureString.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.StringToByte.My.MySettings.Default, "WithSecureString", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkSecureString.Location = New System.Drawing.Point(239, 8)
        Me.chkSecureString.Name = "chkSecureString"
        Me.chkSecureString.Size = New System.Drawing.Size(112, 17)
        Me.chkSecureString.TabIndex = 3
        Me.chkSecureString.Text = "With SecureString"
        Me.chkSecureString.UseVisualStyleBackColor = True
        '
        'chkUTF8
        '
        Me.chkUTF8.AutoSize = True
        Me.chkUTF8.Checked = Global.StringToByte.My.MySettings.Default.EncodingUTF8
        Me.chkUTF8.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.StringToByte.My.MySettings.Default, "EncodingUTF8", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkUTF8.Location = New System.Drawing.Point(121, 8)
        Me.chkUTF8.Name = "chkUTF8"
        Me.chkUTF8.Size = New System.Drawing.Size(113, 17)
        Me.chkUTF8.TabIndex = 2
        Me.chkUTF8.Text = "Encoding of UTF8"
        Me.chkUTF8.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 422)
        Me.Controls.Add(Me.chkMemo)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.chkSecureString)
        Me.Controls.Add(Me.chkUTF8)
        Me.Controls.Add(Me.Label1)
        Me.MinimumSize = New System.Drawing.Size(470, 300)
        Me.Name = "MainForm"
        Me.Text = "String To Byte"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tabCSharp.ResumeLayout(False)
        Me.tabCSharp.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.tabVB.ResumeLayout(False)
        Me.tabVB.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.tabVC.ResumeLayout(False)
        Me.tabVC.PerformLayout()
        Me.ToolStrip4.ResumeLayout(False)
        Me.ToolStrip4.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkUTF8 As System.Windows.Forms.CheckBox
    Friend WithEvents chkSecureString As System.Windows.Forms.CheckBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tbxSource As System.Windows.Forms.TextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabCSharp As System.Windows.Forms.TabPage
    Friend WithEvents tbxCSharp As System.Windows.Forms.TextBox
    Friend WithEvents tabVB As System.Windows.Forms.TabPage
    Friend WithEvents tbxVB As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents chkMemo As System.Windows.Forms.CheckBox
    Friend WithEvents tabVC As System.Windows.Forms.TabPage
    Friend WithEvents tbxVC As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip4 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton

End Class
