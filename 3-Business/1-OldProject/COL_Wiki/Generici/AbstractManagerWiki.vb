
Namespace WikiNew
    Public MustInherit Class AbstractManagerWiki
        Inherits COL_BusinessLogic_v2.ObjectBase

#Region "Proprietà"

        Public Property PersonaCorrente() As COL_BusinessLogic_v2.CL_persona.COL_Persona
            Get
                Return Me._Personacorrente
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.CL_persona.COL_Persona)
                Me._Personacorrente = value
            End Set
        End Property
        Public Property ComunitaCorrente() As COL_BusinessLogic_v2.Comunita.COL_Comunita
            Get
                Return Me._ComunitaCorrente
            End Get
            Set(ByVal value As COL_BusinessLogic_v2.Comunita.COL_Comunita)
                Me._ComunitaCorrente = value
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

        Private _Personacorrente As COL_BusinessLogic_v2.CL_persona.COL_Persona
        Private _ComunitaCorrente As COL_BusinessLogic_v2.Comunita.COL_Comunita
        Private _UseCache As Boolean

        'Property Serviziowiki() As ServizioWiki
        Public Sub New(ByVal oPersona As COL_BusinessLogic_v2.CL_persona.COL_Persona, ByVal oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita, Optional ByVal UseCache As Boolean = False)
            Me.ComunitaCorrente = oComunita
            Me.PersonaCorrente = oPersona
            Me.UseCache = UseCache
        End Sub
        Public Sub New()

        End Sub

#End Region

        'Sub New(ByVal oPersona As COL_Persona, ByVal oComunita As COL_Comunita)

#Region "Caricamenti"
        Public MustOverride Function CaricaWiki(Optional ByVal forced As Boolean = False) As WikiNew.Wiki

        Public MustOverride Function CaricaSezioni( _
            ByVal oWiki As WikiNew.Wiki, _
            Optional ByVal forced As Boolean = False _
            ) As ArrayList

        Public MustOverride Function CaricaTopics( _
            ByVal oSezione As WikiNew.SezioneWiki, _
            Optional ByVal forced As Boolean = False _
            ) As IList

        Public MustOverride Sub CaricaTopic(ByRef oTopic As WikiNew.TopicWiki)


        Public MustOverride Function CaricaStoriaTopic( _
            ByVal oTopic As WikiNew.TopicWiki, _
            Optional ByVal forced As Boolean = False _
            ) As IList

        Public MustOverride Function CaricaAllTopicWiki(ByVal oWiki As WikiNew.Wiki) As ArrayList

        Public MustOverride Sub CaricaSezione(ByRef oSezione As WikiNew.SezioneWiki)
#End Region

#Region "Salvataggi"

        Public MustOverride Sub SalvaTopic(ByVal oTopic As WikiNew.TopicWiki)
        Public MustOverride Sub SalvaTopicHistory(ByVal oTopicHistory As WikiNew.TopicHistoryWiki)
        Public MustOverride Sub PreSaveTopic(ByVal oTopic As WikiNew.TopicWiki)
        Public MustOverride Sub SalvaSezione(ByVal oSezione As WikiNew.SezioneWiki)

#End Region

        Public MustOverride Sub DeleteTopic(ByVal oTopic As WikiNew.TopicWiki)

        Public MustOverride Function UltimaDataModifica(ByVal oSezione As WikiNew.SezioneWiki) As DateTime
    End Class
End Namespace


'Public MustOverride Function HasDBconnection(ByRef Request As COL_Request) As Boolean
'Public MustOverride Function ExecuteDataReader(ByRef Request As COL_Request) As COL_DataReader
'Public MustOverride Function ExecuteDataSet(ByRef Request As COL_Request) As COL_DataSet
'Public MustOverride Function ExecuteNotQuery(ByRef Request As COL_Request) As COL_ExecuteNotQuery
