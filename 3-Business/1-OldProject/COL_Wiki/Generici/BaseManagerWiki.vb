Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Namespace WikiNew

    Public MustInherit Class BaseManagerWiki
        Inherits ObjectBase

        Private _PersonaCorrente As COL_Persona
        Private _ComunitaCorrente As COL_Comunita
        Private _UseCache As Boolean

        Public Property PersonaCorrente() As COL_Persona
            Get
                Return _PersonaCorrente
            End Get
            Set(ByVal value As COL_Persona)
                _PersonaCorrente = value
            End Set
        End Property
        Public Property ComunitaCorrente() As COL_Comunita
            Get
                Return _ComunitaCorrente
            End Get
            Set(ByVal value As COL_Comunita)
                _ComunitaCorrente = value
            End Set
        End Property
        Public Property UseCache() As Boolean
            Get
                Return _UseCache
            End Get
            Set(ByVal value As Boolean)
                _UseCache = value
            End Set
        End Property

        Public Sub New()
        End Sub

    End Class

End Namespace