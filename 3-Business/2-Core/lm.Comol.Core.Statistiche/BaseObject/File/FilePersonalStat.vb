Public Class FilePersonalStat
    Private _FileId As Integer
    Private _FileGUID As Guid
    Private _FileName As String
    Private _FilePath As String

    Private _NumDownload As Integer
    Private _LastDownload As DateTime

    Public Property FileID() As Integer
        Get
            Return _FileId
        End Get
        Set(ByVal value As Integer)
            _FileId = value
        End Set
    End Property
    Public Property FileGuid() As Guid
        Get
            Return _FileGUID
        End Get
        Set(ByVal value As Guid)
            _FileGUID = value
        End Set
    End Property
    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property
    Public Property FilePath() As String
        Get
            Return _FilePath
        End Get
        Set(ByVal value As String)
            _FilePath = value
        End Set
    End Property

    Public Property NumDownload() As Integer
        Get
            Return _NumDownload
        End Get
        Set(ByVal value As Integer)
            _NumDownload = value
        End Set
    End Property
    Public Property LastDownload() As DateTime
        Get
            Return _LastDownload
        End Get
        Set(ByVal value As DateTime)
            _LastDownload = value
        End Set
    End Property
End Class
