'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On



Partial Friend NotInheritable Class MySettings
    Inherits System.Configuration.ApplicationSettingsBase

    Private Shared m_Value As MySettings

    Private Shared m_SyncObject As Object = New Object

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Shared ReadOnly Property Value() As MySettings
        Get
            If (MySettings.m_Value Is Nothing) Then
                System.Threading.Monitor.Enter(MySettings.m_SyncObject)
                If (MySettings.m_Value Is Nothing) Then
                    Try
                        MySettings.m_Value = New MySettings
                    Finally
                        System.Threading.Monitor.Exit(MySettings.m_SyncObject)
                    End Try
                End If
            End If
            Return MySettings.m_Value
        End Get
    End Property
End Class
