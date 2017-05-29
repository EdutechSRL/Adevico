
Namespace WikiNew
    Public Interface IViewWikiUC
        'FùCopiato da IViewWikiTest
        Inherits IViewGenerico

        Sub Show(ByVal Content As VisualizzazioniUC)

        Property ActualView() As VisualizzazioniUC
        Property ActualSezioneId() As Guid
        Property ActualTopicId() As Guid
        Property ActualWikiId() As Guid
        Property SearchString() As String
        Property pageSize() As Integer
        Property pageIndex() As Integer
        Property IsLastPage() As Boolean

        Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Wiki

        'Sub BindSezione(ByVal oSezione As WikiNew.SezioneWiki)
        'Sub BindTopic(ByVal oTopic As WikiNew.TopicWiki)
        'Sub BindTopicTest(ByVal oTopic As WikiNew.TopicWiki)

        
        Sub BindTopicsFunded(ByVal Topics As System.Collections.IList)

        ReadOnly Property TopicText() As String
        ReadOnly Property TopicNome() As String
        ReadOnly Property TopicIsCancellato() As Boolean
        ReadOnly Property TopicIsPubblico() As Boolean


        ReadOnly Property SezioneNome() As String
        ReadOnly Property SezioneDescrizione() As String
        ReadOnly Property SezioneIsDefault() As Boolean
        ReadOnly Property SezioneIsPubblica() As Boolean

        Property NomeWiki() As String

    End Interface


    Public Enum VisualizzazioniUC As Integer
        PageNotRender = -10
        NoPermessi = -1
        NoWikiNoPermessi = 0
        NoWiki = 1
        WikiMod = 2
        NoSezioniNoPermessi = 10
        NoSezione = 11 'No sezione, no permessi
        AddSezione = 12
        NoTopicNoPermessi = 20
        NoTopic = 21
        ListaTopic = 22
        AddTopic = 23
        ModifyTopic = 24
        ShowTopic = 25
        CronologiaTopic = 26
        CronologiaNoTopic = 27
        ListaTopicSearched = 44
    End Enum

    Public Enum dBConnectionTypeUC As Integer
        SQLStore = 1
        nHybernate = 2
    End Enum
End Namespace
