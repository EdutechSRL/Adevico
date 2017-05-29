
Namespace WikiNew
    Public Interface IViewWiki
        Inherits IViewGenerico

        Sub Show(ByVal Content As Visualizzazioni)

        Property ShowErrorMessage As Boolean

        Property SearchString() As String
        Property ActualView() As Visualizzazioni
        Property ActualSezioneId() As Guid
        Property ActualTopicId() As Guid
        Property ActualWikiId() As Guid
        Property OriginalSezioneID() As Guid
        Property pageSize() As Integer
        Property pageIndex() As Integer
        Property IsLastPage() As Boolean
        Property ExternalComunityID() As Integer
        Property ExternalTopicID() As Guid

        Property DisplayAuthors() As Boolean

        Property ShowAuthors As Boolean
        Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Wiki

        Sub BindSezione(ByVal oSezione As WikiNew.SezioneWiki)
        Sub BindTopic(ByVal oTopic As WikiNew.TopicWiki)
        Sub BindTopicTest(ByVal oTopic As WikiNew.TopicWiki)
        Sub bindcronologia(ByVal Lista As IList)
        Sub BindNavigatore(ByVal Sezioni As IList)
        Sub BindTopicsSezione(ByVal Topics As IList, ByVal Sezione As COL_Wiki.WikiNew.SezioneWiki)
        Sub BindTopicsFunded(ByVal Topics As System.Collections.IList)
        Sub BindTopicsImport(ByVal Topics As System.Collections.IList)
        Sub BindComunita()

        ReadOnly Property TopicText() As String
        ReadOnly Property TopicPlainText() As String

        ReadOnly Property TopicNome() As String
        ReadOnly Property TopicIsCancellato() As Boolean
        ReadOnly Property TopicIsPubblico() As Boolean
        ReadOnly Property ViewDeleted() As Boolean


        ReadOnly Property SezioneNome() As String
        ReadOnly Property SezioneDescrizione() As String
        'ReadOnly Property SectionDescriptionPlainText() As String
        ReadOnly Property SectionDescriptionText() As String
        ReadOnly Property SezioneIsDefault() As Boolean
        ReadOnly Property SezioneIsPubblica() As Boolean

        Property NomeWiki() As String

        Sub ConfirmDelete(ByVal id As Guid, msg As IList(Of KeyValuePair(Of String, Guid)))

        Sub NotifyTopicAdd(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal CreatorName As String)
        Sub NotifyTopicEdit(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal UserName As String)
        Sub NotifyTopicRipristina(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal DataCreation As DateTime, ByVal UserName As String)
        Sub NotifyTopicDelete(ByVal CommunityID As Integer, ByVal TopicID As System.Guid, ByVal Title As String, ByVal UserName As String)
        Sub NotifySectionAdd(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionName As String, ByVal SectionId As System.Guid)
        Sub NotifySectionEdit(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionId As System.Guid, ByVal PreviousName As String, ByVal NewName As String)
        Sub NotifySectionDelete(ByVal CommunityID As Integer, ByVal CreatorName As String, ByVal SectionId As System.Guid, ByVal SectionName As String)
        Sub NotifySectionRipristina(ByVal CommunityID As Integer, ByVal SectionId As System.Guid, ByVal SectionName As String, ByVal DataCreation As DateTime, ByVal UserName As String)


    End Interface


    Public Enum Visualizzazioni As Integer
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
        ListaComunita = 28
        ListaTopicsImport = 29
        ListaTopicSearched = 44
    End Enum

    Public Enum dBConnectionType As Integer
        SQLStore = 1
        nHybernate = 2
    End Enum
End Namespace
