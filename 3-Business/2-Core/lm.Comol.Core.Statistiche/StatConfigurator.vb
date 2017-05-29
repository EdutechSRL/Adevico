Imports System.Xml
Imports System.IO

Public Class StatConfigurator
    Public Shared Function GetFullList(ByVal FilePath As String) As List(Of ServiceObject)
        'Serialize(FilePath)
        Dim oImpersonate As New lm.Comol.Core.File.Impersonate
        oImpersonate.ImpersonateValidUser()
        Dim Serializer As New Serialization.XmlSerializer(GetType(List(Of ServiceObject)))
        Dim DataFile As FileStream
        Dim ListObjectInput As New List(Of ServiceObject)
        Try
            DataFile = New FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None)
            ListObjectInput = Serializer.Deserialize(DataFile)
        Catch ex As Exception
            oImpersonate.UndoImpersonation()
        Finally
            If Not IsNothing(DataFile) Then
                DataFile.Close()
            End If
            oImpersonate.UndoImpersonation()
        End Try

        Return ListObjectInput
    End Function

    Public Shared Function GetListItem(ByVal FilePath) As List(Of KeyValue)
        Dim OutList As New List(Of KeyValue)
        Dim query = From So As ServiceObject In GetFullList(FilePath) Distinct Select New KeyValue(So.Id, So.Name)
        OutList = query.ToList
        Return OutList
    End Function
    'Public Shared Sub Serialize(ByVal Path As String)
    '    Dim ListObjectOutput As New List(Of ServiceObject)
    '    Dim SingleObject As New ServiceObject

    '    SingleObject.Id = 1
    '    SingleObject.Name = "Scorm"
    '    SingleObject.UC_Community = "UC_ScormStatCom.ascx"
    '    SingleObject.Uc_Personal = "UC_ScormStatCom.ascx"

    '    ListObjectOutput.Add(SingleObject)

    '    Dim SingleObject2 As New ServiceObject
    '    SingleObject2 = New ServiceObject
    '    SingleObject2.Id = 2
    '    SingleObject2.Name = "File"
    '    SingleObject2.UC_Community = "UC_ScormStatCom.ascx"
    '    SingleObject2.Uc_Personal = "UC_ScormStatCom.ascx"

    '    ListObjectOutput.Add(SingleObject2)

    '    Dim Serializer As New Serialization.XmlSerializer(GetType(List(Of ServiceObject)))
    '    Dim DataFile As New FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.None)

    '    Serializer.Serialize(DataFile, ListObjectOutput)
    '    DataFile.Close()




    '    'DataFile = New FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.None)

    '    'Dim ListObjectInput As List(Of ServiceObject) = Serializer.Deserialize(DataFile)

    '    'Console.Write(ListObjectOutput.Count)

    'End Sub
End Class

Public Class KeyValue
    Public Sub New(ByVal Key As Integer, ByVal Value As String)
        Me.Key = Key
        Me.Value = Value
    End Sub
    Public Key As Integer
    Public Value As String
End Class