Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Namespace WikiNew
    Public Interface iManagerWiki

#Region "Proprietà"

        Property PersonaCorrente() As COL_Persona
        Property ComunitaCorrente() As COL_Comunita
        Property UseCache() As Boolean

        'Property Serviziowiki() As ServizioWiki

#End Region

        'Sub New(ByVal oPersona As COL_Persona, ByVal oComunita As COL_Comunita)

#Region "Caricamenti"
        Function CaricaWiki(Optional ByVal forced As Boolean = False) As WikiNew.Wiki

        '-------PARTE ALESSANDRO-----------------
        Function CercaTopic( _
            ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer) As ArrayList
        Function CercaTopicComunita( _
            ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer, Optional ByVal IDcomunita As Integer = Nothing) As ArrayList
        Function CercaTopicComPub( _
            ByVal text As String, ByVal RowIndex As Integer, ByVal pageSize As Integer) As ArrayList


        Function CaricaSezioni( _
            ByVal oWiki As WikiNew.Wiki, _
            Optional ByVal forced As Boolean = False _
            ) As IList

        Function CaricaSezioniHome( _
            Optional ByVal forced As Boolean = False _
            ) As IList

        Function CaricaTopics( _
            ByVal oSezione As WikiNew.SezioneWiki, _
            Optional ByVal forced As Boolean = False _
            ) As IList
        Function CaricaTopicsHome( _
            ByVal oSezione As WikiNew.SezioneWiki, _
            Optional ByVal forced As Boolean = False _
            ) As IList

        Sub CaricaTopic(ByRef oTopic As WikiNew.TopicWiki)


        Function CaricaStoriaTopic( _
            ByVal oTopic As WikiNew.TopicWiki, _
            Optional ByVal forced As Boolean = False _
            ) As IList

        Sub CaricaTopicCrono( _
            ByRef oTopicCrono As WikiNew.TopicHistoryWiki, _
            Optional ByVal forced As Boolean = False _
            )

        Function CaricaAllTopicWiki(ByVal oWiki As WikiNew.Wiki, Optional ByVal forced As Boolean = False) As IList

        Sub CaricaSezione(ByRef oSezione As WikiNew.SezioneWiki)
        Sub CaricaSezioneHome(ByRef oSezione As WikiNew.SezioneWiki)
#End Region

#Region "Salvataggi"
        Sub SalvaWiki(ByVal oWiki As WikiNew.Wiki)
        Sub SalvaTopic(ByRef oTopic As WikiNew.TopicWiki, ByRef allowsavequit As Boolean)
        Sub SalvaTopicHistory(ByVal oTopicHistory As WikiNew.TopicHistoryWiki)
        Sub PreSaveTopic(ByVal oTopic As WikiNew.TopicWiki)
        Sub SalvaSezione(ByVal oSezione As WikiNew.SezioneWiki)

#End Region

        Sub DeleteTopic(ByVal oTopic As WikiNew.TopicWiki)
        Sub UnDeleteTopic(ByVal oTopic As WikiNew.TopicWiki)



        Function UltimaDataModifica(ByVal oSezione As WikiNew.SezioneWiki) As DateTime

    End Interface
End Namespace