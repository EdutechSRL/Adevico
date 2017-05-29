Namespace WikiNew
    <Serializable()> Public Class Wiki
        'Inherits DomainObject

        Private _ID As Guid
        Private _Nome As String
        Private _Comunita As COL_BusinessLogic_v2.Comunita.COL_Comunita
        Private _IsNew As Boolean
        Private _DisplayAuthors As Boolean

        Public Sub New()
            _Comunita = New COL_BusinessLogic_v2.Comunita.COL_Comunita
        End Sub
        Public Property ID() As Guid
            Get
                Return _ID
            End Get
            Set(ByVal value As Guid)
                _ID = value
            End Set
        End Property

        Public Property Nome() As String
            Get
                Return _Nome
            End Get
            Set(ByVal value As String)
                _Nome = value
            End Set
        End Property

        Public Property Comunita() As COL_BusinessLogic_v2.Comunita.COL_Comunita
            Get
                Return _Comunita
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.Comunita.COL_Comunita)
                _Comunita = value
            End Set
        End Property
        Public Property IsNew() As Boolean
            Get
                Return _IsNew
            End Get
            Set(ByVal value As Boolean)
                _IsNew = value
            End Set
        End Property

        Public Property DisplayAuthors As Boolean
            Get
                Return _DisplayAuthors
            End Get
            Set(value As Boolean)
                _DisplayAuthors = value
            End Set
        End Property
    End Class

End Namespace