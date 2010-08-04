Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class MainForm

    <Flags()> _
    Enum UpdateType As Byte
        None
        CSharpCode = 1
        VBCode = 2
        VCCode = 4
        All = CSharpCode Or VBCode Or VCCode
    End Enum

    Delegate Function GetSelectedTabDelegate() As TabPage
    Delegate Sub SetTextDelegate(ByVal c As Control, ByVal txt As String)


    Private _updateFlag As UpdateType = UpdateType.All
    Private _updateThread As thread

    Private Property m_UpdateFlag() As UpdateType
        Get
            Return _updateFlag
        End Get
        Set(ByVal value As UpdateType)
            _updateFlag = value
        End Set
    End Property

    Private Property m_UpdateThread() As Thread
        Get
            If _updateThread Is Nothing Then
                _updateThread = New Thread(AddressOf UpdateCode)
                _updateThread.IsBackground = True
            End If
            Return _updateThread
        End Get
        Set(ByVal value As Thread)
            _updateThread = value
        End Set
    End Property

    Private Function GetVBByteArrayCode() As String
        Dim source As String = tbxSource.Text
        Dim encoder As Encoding = If(chkUTF8.Checked, Encoding.UTF8, Encoding.ASCII)
        Dim buffer() As Byte = encoder.GetBytes(source)
        Dim length As Integer = buffer.Length
        Dim output As New StringBuilder

        output.AppendLine("Public Function GetStringFromByteArray() As " & If(chkSecureString.Checked, "SecureString", "String"))

        If chkMemo.Checked AndAlso tbxSource.TextLength > 0 Then
            output.AppendLine(Regex.Replace(tbxSource.Text, "^", vbTab & "'", RegexOptions.Multiline))
            output.AppendLine()
        End If

        output.AppendLine(String.Format(vbTab & "Dim buffer({0}) as Byte", length - 1))
        For idx As Integer = 0 To length - 1
            output.AppendLine(String.Format(vbTab & "buffer({0}) = &H{1}", idx, Convert.ToString(buffer(idx), 16)))
        Next

        output.AppendLine()

        If chkSecureString.Checked Then
            output.AppendLine(vbTab & "Dim ret as New SecureString")
            output.AppendLine(vbTab & "For Each b as Byte In buffer")
            output.AppendLine(vbTab & vbTab & "ret.AppendChar(Convert.ToChar(b))")
            output.AppendLine(vbTab & "Next")
            output.AppendLine(vbTab & "ret.MakeReadOnly()")
            output.AppendLine()
            output.AppendLine(vbTab & "Return ret")
        Else
            output.AppendLine(vbTab & "Return System.Text.Encoding." & Regex.Match(encoder.ToString, "System\.Text\.(.*)Encoding").Groups(1).Value & ".GetString(buffer)")
        End If

        output.AppendLine("End Function")
        Return output.ToString()
    End Function

    Private Function GetCSharpByteArrayCode() As String
        Dim source As String = tbxSource.Text
        Dim encoder As Encoding = If(chkUTF8.Checked, Encoding.UTF8, Encoding.ASCII)
        Dim buffer() As Byte = encoder.GetBytes(source)
        Dim length As Integer = buffer.Length
        Dim output As New StringBuilder

        output.AppendLine(String.Format("{0} GetStringFromByteArray()", If(chkSecureString.Checked, "SecureString", "String")))
        output.AppendLine("{")

        If chkMemo.Checked AndAlso tbxSource.TextLength > 0 Then
            output.AppendLine(Regex.Replace(tbxSource.Text, "^", vbTab & "//", RegexOptions.Multiline))
            output.AppendLine()
        End If

        output.AppendLine(String.Format(vbTab & "byte[] buffer = new byte[{0}];", length))
        For idx As Integer = 0 To length - 1
            output.AppendLine(String.Format(vbTab & "buffer[{0}] = 0x{1};", idx, Convert.ToString(buffer(idx), 16)))
        Next

        output.AppendLine()

        If chkSecureString.Checked Then
            output.AppendLine(vbTab & "SecureString ret = new SecureString();")
            output.AppendLine(vbTab & "foreach (byte b in buffer)")
            output.AppendLine(vbTab & vbTab & "ret.AppendChar(Convert.ToChar(b));")
            output.AppendLine(vbTab & "ret.MakeReadOnly();")
            output.AppendLine()
            output.AppendLine(vbTab & "return ret;")
        Else
            output.AppendLine(vbTab & "return System.Text.Encoding." & Regex.Match(encoder.ToString, "System\.Text\.(.*)Encoding").Groups(1).Value & ".GetString(buffer);")
        End If

        output.AppendLine("}")
        Return output.ToString()
    End Function

    Private Function GetVCByteArrayCode() As String
        Dim source As String = tbxSource.Text
        Dim encoder As Encoding = If(chkUTF8.Checked, Encoding.UTF8, Encoding.ASCII)
        Dim buffer() As Byte = encoder.GetBytes(source)
        Dim length As Integer = buffer.Length
        Dim output As New StringBuilder

        output.AppendLine(String.Format("{0} GetStringFromByteArray()", If(chkSecureString.Checked, "System::Security::SecureString^", "System::String^")))
        output.AppendLine("{")

        If chkMemo.Checked AndAlso tbxSource.TextLength > 0 Then
            output.AppendLine(Regex.Replace(tbxSource.Text, "^", vbTab & "//", RegexOptions.Multiline))
            output.AppendLine()
        End If

        output.AppendLine(String.Format(vbTab & "array<System::Byte>^ buffer = gcnew array<System::Byte>({0});", length))
        For idx As Integer = 0 To length - 1
            output.AppendLine(String.Format(vbTab & "buffer[{0}] = 0x{1};", idx, Convert.ToString(buffer(idx), 16)))
        Next

        output.AppendLine()

        If chkSecureString.Checked Then
            output.AppendLine(vbTab & "System::Security::SecureString^ ret = gcnew System::Security::SecureString();")
            output.AppendLine(vbTab & "for each (System::Byte b in buffer)")
            output.AppendLine(vbTab & vbTab & "ret->AppendChar(System::Convert::ToChar(b));")
            output.AppendLine(vbTab & "ret->MakeReadOnly();")
            output.AppendLine()
            output.AppendLine(vbTab & "return ret;")
        Else
            output.AppendLine(vbTab & "return System::Text::Encoding::" & Regex.Match(encoder.ToString, "System\.Text\.(.*)Encoding").Groups(1).Value & "->GetString(buffer);")
        End If

        output.AppendLine("}")
        Return output.ToString()
    End Function

    Private Sub SetText(ByVal c As Control, ByVal txt As String)
        If c.InvokeRequired Then
            Dim d As New SetTextDelegate(AddressOf SetText)
            Me.Invoke(d, New Object() {c, txt})
        Else
            c.Text = txt
        End If
    End Sub

    Private Function GetSelectedTab() As TabPage
        If TabControl1.InvokeRequired Then
            Dim d As New GetSelectedTabDelegate(AddressOf GetSelectedTab)
            Return Me.Invoke(d)
        Else
            Return TabControl1.SelectedTab
        End If
    End Function

    Private Sub UpdateCodeWithThread()
        If m_UpdateThread.ThreadState = ThreadState.Background Then
            Try
                m_UpdateThread.Abort()
            Catch ex As Exception
            End Try
        End If
        m_UpdateThread = Nothing
        m_UpdateThread.Start()
    End Sub


    Private Sub UpdateCode()
        Dim tab As TabPage = GetSelectedTab()
        If tab Is tabCSharp Then
            UpdateCSharpCode()
        ElseIf tab Is tabVB Then
            UpdateVBCode()
        Else
            UpdateVCCode()
        End If
    End Sub

    Private Sub UpdateCSharpCode()
        If (m_UpdateFlag And UpdateType.CSharpCode) = UpdateType.None Then
            Return
        End If
        Dim txt As String = GetCSharpByteArrayCode()
        SetText(tbxCSharp, txt)
        m_UpdateFlag = m_UpdateFlag And (UpdateType.All Xor UpdateType.CSharpCode)
    End Sub

    Private Sub UpdateVBCode()
        If (m_UpdateFlag And UpdateType.VBCode) = UpdateType.None Then
            Return
        End If
        Dim txt As String = GetVBByteArrayCode()
        SetText(tbxVB, txt)
        m_UpdateFlag = m_UpdateFlag And (UpdateType.All Xor UpdateType.VBCode)
    End Sub

    Private Sub UpdateVCCode()
        If (m_UpdateFlag And UpdateType.VCCode) = UpdateType.None Then
            Return
        End If
        Dim txt As String = GetVCByteArrayCode()
        SetText(tbxVC, txt)
        m_UpdateFlag = m_UpdateFlag And (UpdateType.All Xor UpdateType.VCCode)
    End Sub

    Private Sub tbxSource_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxSource.TextChanged
        Me.ToolStripStatusLabel2.Text = tbxSource.TextLength
    End Sub

    Private Sub chkUTF8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUTF8.CheckedChanged
        m_UpdateFlag = UpdateType.All
        UpdateCodeWithThread()
    End Sub

    Private Sub chkSecureString_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSecureString.CheckedChanged
        m_UpdateFlag = UpdateType.All
        UpdateCodeWithThread()
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        UpdateCodeWithThread()
    End Sub

    Private Sub CopyToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripButton.Click
        If tbxCSharp.TextLength = 0 Then
            Return
        End If
        Clipboard.SetText(tbxCSharp.Text)
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        If tbxVB.TextLength = 0 Then
            Return
        End If
        Clipboard.SetText(tbxVB.Text)
    End Sub

    Private Sub NewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripButton.Click
        tbxSource.Clear()
        m_UpdateFlag = UpdateType.All
        UpdateCodeWithThread()
    End Sub

    Private Sub PasteToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripButton.Click
        tbxSource.Focus()
        SendKeys.Send("^V")
        m_UpdateFlag = UpdateType.All
        UpdateCodeWithThread()
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UpdateCodeWithThread()
    End Sub

    Private Sub chkMemo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMemo.CheckedChanged
        m_UpdateFlag = UpdateType.All
        UpdateCodeWithThread()
    End Sub

    Private Sub tbxSource_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbxSource.KeyUp
        m_UpdateFlag = UpdateType.All
        UpdateCodeWithThread()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        If tbxVC.TextLength = 0 Then
            Return
        End If
        Clipboard.SetText(tbxVC.Text)
    End Sub
End Class
