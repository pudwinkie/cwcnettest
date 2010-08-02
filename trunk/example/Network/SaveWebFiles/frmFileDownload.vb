Imports System.IO
Imports System.Net
Imports System.Text
Public Class frmFileDownload

    Private dtFiles As DataTable
    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        If txtDestination.Text = "" Then
            MessageBox.Show("Please choose a destination folder.", "No destination", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If lstFiles.Items.Count = 0 Then
            MessageBox.Show("No files to download.", "No files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Me.Cursor = Cursors.AppStarting
        If btnDownload.Text = "&Download" Then
            MarkFieldsReadOnly(True)
            btnDownload.Text = "Cancel"
            bgDownloader.RunWorkerAsync()
        ElseIf btnDownload.Text = "Cancel" Then
            bgDownloader.CancelAsync()
            btnDownload.Text = "Cancelling..."
            Exit Sub
        End If


    End Sub

    Private Sub DownloadFile(ByVal FromURI As String, ByVal DestinationFilePath As String)
        Dim request As Net.WebRequest
        Dim response As Net.WebResponse = Nothing
        Dim s As IO.Stream = Nothing
        Dim fs As IO.FileStream = Nothing

        Dim uri As New Uri(FromURI)
        request = Net.WebRequest.Create(uri)
        request.Timeout = 10000
        response = request.GetResponse
        s = response.GetResponseStream

        fs = New IO.FileStream(DestinationFilePath, IO.FileMode.CreateNew)
        Dim count As Int32
        Dim read(256) As Byte
        count = s.Read(read, 0, read.Length)
        Do While (count > 0)
            fs.Write(read, 0, count)
            count = s.Read(read, 0, read.Length)
        Loop
        fs.Close()
        s.Close()
        response.Close()
    End Sub

    Private Sub bgDownloader_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgDownloader.DoWork
        Dim totalFiles As Integer = lstFiles.Items.Count
        Dim i As Integer = 0
        For Each row As DataRow In dtFiles.Rows

            If Not bgDownloader.CancellationPending Then
                bgDownloader.ReportProgress((i * 100 / totalFiles).ToString, row("FileName"))
                Dim destFileName As String = txtDestination.Text & GetFileName(row("FileName"))
                DownloadFile(row("FileName"), destFileName)
            End If
            i = i + 1
        Next

    End Sub
    Private Function GetFileName(ByVal uri As String) As String
        Dim url As New Uri(uri)

        Dim lastfwdslash As Integer = uri.LastIndexOf("/"c)
        Return Mid(uri, lastfwdslash + 2)

    End Function
    Private Sub bgDownloader_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bgDownloader.ProgressChanged
        lblStatus.Text = String.Format("{0}% done - working on : {1}", e.ProgressPercentage, e.UserState)
    End Sub

    Private Sub bgDownloader_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgDownloader.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show(e.Error.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            System.Diagnostics.Process.Start(txtDestination.Text)
        End If
        btnDownload.Text = "&Download"
        lblStatus.Text = "Ready"
        Me.Cursor = Cursors.Default
        MarkFieldsReadOnly(False)

        'MessageBox.Show("All files downloaded.", "Done", MessageBoxButtons.OK)
    End Sub

    Private Sub MarkFieldsReadOnly(ByVal value As Boolean)
        txtLinkLocation.ReadOnly = value
        lstFiles.Enabled = Not value
    End Sub

    Private Sub btnDestinationFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestinationFolder.Click
        Dim fbd As New FolderBrowserDialog
        If fbd.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDestination.Text = fbd.SelectedPath
            If Not txtDestination.Text.EndsWith("\") Then
                txtDestination.Text += "\"
            End If
        End If
    End Sub

    Private Sub btnAddLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddLink.Click
        If txtLinkLocation.Text.Trim <> "" Then
            For Each row As DataRow In dtFiles.Rows
                If row("FileName") = txtLinkLocation.Text Then
                    Exit Sub
                End If
            Next
            Dim dRow As DataRow = dtFiles.NewRow
            dRow("FileName") = txtLinkLocation.Text
            dtFiles.Rows.Add(dRow)
            RefreshListBox()
            txtLinkLocation.Text = ""
        End If

        'lstFiles.Items.Add(txtLinkLocation.Text)
    End Sub
    Private Sub RefreshListBox()
        lstFiles.DisplayMember = "FileName"
        lstFiles.ValueMember = "FileName"
        lstFiles.DataSource = dtFiles
    End Sub

    Private Sub lstFiles_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstFiles.KeyUp
        If lstFiles.SelectedItems.Count > 0 Then
            If e.KeyCode = Keys.Delete Then
                If MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                    Dim SelectedCount As Integer = lstFiles.SelectedItems.Count
                    For i As Integer = 0 To SelectedCount - 1
                        dtFiles.Rows.Remove(DirectCast(lstFiles.SelectedItems(i), DataRowView).Row)
                    Next
                    RefreshListBox()
                End If
            End If
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtFiles = New DataTable
        dtFiles.Columns.Add("FileName", GetType(String))
        txtLinkLocation.AllowDrop = True
    End Sub

    Private Sub txtLinkLocation_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtLinkLocation.DragDrop
        'txtLinkLocation.Text = e.ToString
        Try
            Dim a As Array = e.Data.GetData(DataFormats.StringFormat)
            txtLinkLocation.Text = a.GetValue(0).ToString
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtLinkLocation_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtLinkLocation.DragEnter
        If e.Data.GetDataPresent(DataFormats.StringFormat) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

End Class
