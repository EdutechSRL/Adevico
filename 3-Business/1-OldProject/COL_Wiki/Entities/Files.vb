Namespace WikiNew
    Public Class Files
        'Inherits DomainObject

        Public Enum TipoFile
            Immagine = 0
            Archivio = 1
            Video = 2
            Eseguibile = 3
            Documento = 4
        End Enum

        Private _IdFile As Guid
        Private _NomeFile As String
        Private _Dimensione As Long
        Private _Estensione As String
        Private _DataUpload As Nullable(Of DateTime)
        Private _Path As String
        Private _Uploader As COL_BusinessLogic_v2.CL_persona.COL_Persona
        Private _Tipo As TipoFile


        Public Property IdFile() As Guid
            Get
                Return _IdFile
            End Get
            Set(ByVal value As Guid)
                _IdFile = value
            End Set
        End Property

        Public Property NomeFile() As String
            Get
                Return _NomeFile
            End Get
            Set(ByVal value As String)
                _NomeFile = value
            End Set
        End Property

        Public Property Dimensione() As Long
            Get
                Return _Dimensione
            End Get
            Set(ByVal value As Long)
                _Dimensione = value
            End Set
        End Property

        Public Property Estensione() As String
            Get
                Return _Estensione
            End Get
            Set(ByVal value As String)
                _Estensione = value
            End Set
        End Property


        '<Nullable(True)> 
        Public Property DataUpload() As Nullable(Of DateTime)
            Get
                Return _DataUpload
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _DataUpload = value
            End Set
        End Property

        Public Property Path() As String
            Get
                Return _Path
            End Get
            Set(ByVal value As String)
                _Path = value
            End Set
        End Property

        Public Property Uploader() As COL_BusinessLogic_v2.CL_persona.COL_Persona
            Get
                Return _Uploader
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.CL_persona.COL_Persona)
                _Uploader = value
            End Set
        End Property

        Public Property Tipo() As TipoFile
            Get
                Return _Tipo
            End Get
            Set(ByVal value As TipoFile)
                _Tipo = value
            End Set
        End Property
    End Class
End Namespace