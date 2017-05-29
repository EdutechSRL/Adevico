Public Class StatGeneric
    Public Shared Function getUCPath(ByVal UcStat As EnumUCService)
        Dim StrOut As String = ""

        Select Case UcStat
            Case EnumUCService.ScormFile
                StrOut = "./UC/UC_ScormStatGlobal.ascx"
            Case EnumUCService.ScormUser
                StrOut = "./../Scorm/UC/UC_ScormStatisticheUtente.ascx"
        End Select
        Return StrOut
    End Function

    Public Enum EnumUCService As Integer
        ScormFile = 1
        ScormUser = 2
    End Enum

    Public Shared Function GetServiceList() As List(Of UcService)
        Dim oList As New List(Of UcService)
        oList.Add(New UcService(1, "Scorm"))
        oList.Add(New UcService(2, "Altro"))
        Return oList
    End Function
End Class

Public Class UcService
    Public Sub New()
        _ID = -1
        _Name = ""
    End Sub
    Public Sub New(ByVal Id As Integer, ByVal Name As String)
        _ID = Id
        _Name = Name
    End Sub
    Private _Name As String
    Private _ID As Integer
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class