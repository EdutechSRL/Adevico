
Namespace WikiNew
    Public Interface IViewEditorWiki
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


        Sub LoadTopics(ByVal topics As System.Collections.IList)

    End Interface
End Namespace