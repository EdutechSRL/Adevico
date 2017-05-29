Imports System.Configuration
Imports System.Collections.Specialized

Public Class COL_ConnStrings
    ' based on GoF Singleton defintion and the Chapter 1 example of a Singleton
    Private Shared m_Instance As COL_ConnStrings
    Private Shared m_Mutex As New System.Threading.Mutex()
    Private Shared m_colConnectStrings As NameValueCollection

    Private Sub New()
        m_colConnectStrings = System.Configuration.ConfigurationManager.GetSection("appSettings")
    End Sub

    Public Shared Function GetInstance() As COL_ConnStrings
        ' to be thread safe, we use the Mutex to synchronize threads
        m_Mutex.WaitOne() ' waitone requests a thread
        If m_Instance Is Nothing Then
            m_Instance = New COL_ConnStrings()
        End If
        m_Mutex.ReleaseMutex()
        Return m_Instance
    End Function

    Public Function GetConnectStringByRole(ByVal Role As COL_Request.UserRole) As String
        ' retrieves configuration file by role name
        Return m_colConnectStrings.Get(Role - 1).ToString
    End Function

End Class
