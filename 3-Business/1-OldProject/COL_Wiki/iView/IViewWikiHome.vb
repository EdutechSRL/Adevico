Namespace WikiNew
    Public Interface IViewWikiHome
        Sub Show(ByVal Content As VisualizzazioniHome)

        Property SearchString() As String
        Property ActualView() As VisualizzazioniHome
        Property ActualSezioneId() As Guid
        Property ActualTopicId() As Guid

        Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Wiki

        'Sub BindTopic(ByVal oTopic As WikiNew.TopicWiki)
        Sub BindNavigatore(ByVal Sezioni As IList)
        Sub BindTopicsSezione(ByVal Topics As IList, ByVal Sezione As COL_Wiki.WikiNew.SezioneWiki)
        Sub BindTopicsFunded(ByVal Topics As System.Collections.IList)
        Sub BindTopicTest(ByVal Topic As WikiNew.TopicWiki)

    End Interface

    Public Enum VisualizzazioniHome As Integer
        PageNotRender = -10
        NoPermessi = -1 'Forse non serve
        'NoWiki = 1
        NoSezione = 11
        NoTopic = 21
        ListaTopic = 22
        ListaTopicSearched = 46
        TopicView = 45
    End Enum
End Namespace