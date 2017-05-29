<Serializable()> Public Class ServiceObject
    Public Id As Integer
    Public _Name As String
    Public _UC_Community As String
    Public _UC_Personal As String

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property UC_Community() As String
        Get
            Return _UC_Community
        End Get
        Set(ByVal value As String)
            _UC_Community = value
        End Set
    End Property

    Public Property UC_Personal() As String
        Get
            Return _UC_Personal
        End Get
        Set(ByVal value As String)
            _UC_Personal = value
        End Set
    End Property
End Class