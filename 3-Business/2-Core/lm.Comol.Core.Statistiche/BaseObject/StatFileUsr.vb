Public Class StatFileUsr
    Private _Id As Integer
    Private _Nome As String
    Private _Cognome As String
    Private _Ruolo As String
    Private _NumDown As Integer
    Private _KbDown As Integer
    Private _LastDown As DateTime

    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    Public Property Nome() As String
        Get
            Return Me._Nome
        End Get
        Set(ByVal value As String)
            Me._Nome = value
        End Set
    End Property

    Public Property Cognome() As String
        Get
            Return _Cognome
        End Get
        Set(ByVal value As String)
            _Cognome = value
        End Set
    End Property

    Public Property Ruolo() As String
        Get
            Return _Ruolo
        End Get
        Set(ByVal value As String)
            _Ruolo = value
        End Set
    End Property

    Public Property NumDown() As Integer
        Get
            Return _NumDown
        End Get
        Set(ByVal value As Integer)
            _NumDown = value
        End Set
    End Property

    Public Property KbDown() As Integer
        Get
            Return _KbDown
        End Get
        Set(ByVal value As Integer)
            _KbDown = value
        End Set
    End Property

    Public Property LastDown() As DateTime
        Get
            Return _LastDown
        End Get
        Set(ByVal value As DateTime)
            _LastDown = value
        End Set
    End Property
End Class
