Namespace WikiNew
    <Serializable()> Public Class SezioneWiki

        'Inherits DomainObject

        Private _ID As Guid
        Private _NomeSezione As String
        Private _DataInserimento As Nullable(Of DateTime)
        Private _IsDefault As Boolean
        Private _IsDeleted As Boolean
        Private _IsPubblica As Boolean
        Private _Descrizione As String
        Private _PlainDescription As String
        Private _Wiki As Wiki
        Private _Persona As COL_BusinessLogic_v2.CL_persona.COL_Persona
        Private _NumeroTopic As Integer
        Private _LastModify As DateTime
        Private _IsNew As Boolean

        Public Sub New()
            _Persona = New COL_BusinessLogic_v2.CL_persona.COL_Persona
            _Wiki = New COL_Wiki.WikiNew.Wiki
        End Sub

        Public Property Wiki() As Wiki
            Get
                Return _Wiki
            End Get
            Set(ByVal value As Wiki)
                _Wiki = value
            End Set
        End Property

        Public Property Persona() As COL_BusinessLogic_v2.CL_persona.COL_Persona
            Get
                Return _Persona
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.CL_persona.COL_Persona)
                _Persona = value
            End Set
        End Property

        Public Property ID() As Guid
            Get
                Return _ID
            End Get
            Set(ByVal value As Guid)
                _ID = value
            End Set
        End Property

        Public Property NomeSezione() As String
            Get
                Return _NomeSezione
            End Get
            Set(ByVal value As String)
                _NomeSezione = value
            End Set
        End Property

        '<Nullable(True)>
        Public Property IsNew() As Boolean
            Get
                Return _IsNew
            End Get
            Set(ByVal value As Boolean)
                _IsNew = value
            End Set
        End Property

        Public Property DataInserimento() As Nullable(Of DateTime)
            Get
                Return _DataInserimento
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _DataInserimento = value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return _Descrizione
            End Get
            Set(ByVal value As String)
                _Descrizione = value
            End Set
        End Property

        Public Property PlainDescription() As String
            Get
                Return _PlainDescription
            End Get
            Set(ByVal value As String)
                _PlainDescription = value
            End Set
        End Property

        Public Property IsDefault() As Boolean
            Get
                Return _IsDefault
            End Get
            Set(ByVal value As Boolean)
                _IsDefault = value
            End Set
        End Property

        Public Property IsDeleted() As Boolean
            Get
                Return _IsDeleted
            End Get
            Set(ByVal value As Boolean)
                _IsDeleted = value
            End Set
        End Property

        Public Property IsPubblica() As Boolean
            Get
                Return _IsPubblica
            End Get
            Set(ByVal value As Boolean)
                _IsPubblica = value
            End Set
        End Property

        Public Property NumeroTopic() As Integer
            Get
                Return _NumeroTopic
            End Get
            Set(ByVal value As Integer)
                _NumeroTopic = value
            End Set
        End Property

        '<Nullable(True)> 
        Public Property LastModify() As Nullable(Of DateTime)
            Get
                Return _LastModify
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _LastModify = value
            End Set
        End Property
    End Class
End Namespace