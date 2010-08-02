<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFileDownload
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFileDownload))
        Me.lstFiles = New System.Windows.Forms.ListBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.txtDestination = New System.Windows.Forms.TextBox
        Me.btnDownload = New System.Windows.Forms.Button
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.txtLinkLocation = New System.Windows.Forms.TextBox
        Me.btnAddLink = New System.Windows.Forms.Button
        Me.btnDestinationFolder = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.bgDownloader = New System.ComponentModel.BackgroundWorker
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstFiles
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.lstFiles, 2)
        Me.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFiles.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstFiles.FormattingEnabled = True
        Me.lstFiles.ItemHeight = 16
        Me.lstFiles.Location = New System.Drawing.Point(3, 33)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(569, 276)
        Me.lstFiles.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.95652!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.04348!))
        Me.TableLayoutPanel1.Controls.Add(Me.txtDestination, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lstFiles, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDownload, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txtLinkLocation, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAddLink, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDestinationFolder, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblStatus, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(575, 375)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'txtDestination
        '
        Me.txtDestination.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDestination.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDestination.Location = New System.Drawing.Point(3, 318)
        Me.txtDestination.Name = "txtDestination"
        Me.txtDestination.ReadOnly = True
        Me.txtDestination.Size = New System.Drawing.Size(401, 23)
        Me.txtDestination.TabIndex = 4
        '
        'btnDownload
        '
        Me.btnDownload.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDownload.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDownload.ImageKey = "Download.gif"
        Me.btnDownload.ImageList = Me.imgList
        Me.btnDownload.Location = New System.Drawing.Point(410, 348)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(162, 24)
        Me.btnDownload.TabIndex = 1
        Me.btnDownload.Text = "&Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'imgList
        '
        Me.imgList.ImageStream = CType(resources.GetObject("imgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgList.TransparentColor = System.Drawing.Color.Transparent
        Me.imgList.Images.SetKeyName(0, "Folder 1 Add.gif")
        Me.imgList.Images.SetKeyName(1, "Folder 1 Forward.gif")
        Me.imgList.Images.SetKeyName(2, "Download.gif")
        '
        'txtLinkLocation
        '
        Me.txtLinkLocation.AllowDrop = True
        Me.txtLinkLocation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLinkLocation.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLinkLocation.Location = New System.Drawing.Point(3, 3)
        Me.txtLinkLocation.Name = "txtLinkLocation"
        Me.txtLinkLocation.Size = New System.Drawing.Size(401, 23)
        Me.txtLinkLocation.TabIndex = 2
        '
        'btnAddLink
        '
        Me.btnAddLink.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnAddLink.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddLink.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddLink.ImageKey = "Folder 1 Add.gif"
        Me.btnAddLink.ImageList = Me.imgList
        Me.btnAddLink.Location = New System.Drawing.Point(410, 3)
        Me.btnAddLink.Name = "btnAddLink"
        Me.btnAddLink.Size = New System.Drawing.Size(162, 24)
        Me.btnAddLink.TabIndex = 3
        Me.btnAddLink.Text = "&Add link to download"
        Me.btnAddLink.UseVisualStyleBackColor = True
        '
        'btnDestinationFolder
        '
        Me.btnDestinationFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDestinationFolder.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDestinationFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDestinationFolder.ImageKey = "Folder 1 Forward.gif"
        Me.btnDestinationFolder.ImageList = Me.imgList
        Me.btnDestinationFolder.Location = New System.Drawing.Point(410, 318)
        Me.btnDestinationFolder.Name = "btnDestinationFolder"
        Me.btnDestinationFolder.Size = New System.Drawing.Size(162, 24)
        Me.btnDestinationFolder.TabIndex = 3
        Me.btnDestinationFolder.Text = "Destination &folder"
        Me.btnDestinationFolder.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(3, 345)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus.TabIndex = 5
        '
        'bgDownloader
        '
        Me.bgDownloader.WorkerReportsProgress = True
        Me.bgDownloader.WorkerSupportsCancellation = True
        '
        'frmFileDownload
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 375)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFileDownload"
        Me.Text = "Multiple web file downloader"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstFiles As System.Windows.Forms.ListBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnDownload As System.Windows.Forms.Button
    Friend WithEvents txtLinkLocation As System.Windows.Forms.TextBox
    Friend WithEvents btnAddLink As System.Windows.Forms.Button
    Friend WithEvents txtDestination As System.Windows.Forms.TextBox
    Friend WithEvents btnDestinationFolder As System.Windows.Forms.Button
    Friend WithEvents bgDownloader As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents imgList As System.Windows.Forms.ImageList

End Class
