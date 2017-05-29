Namespace lm.ActionDataContract
    Public Interface iRemoteService
        Sub CreateWorkingSession(ByVal WorkingID As System.Guid, ByVal PersonID As Integer)
        Sub DeleteWorkingSession(ByVal WorkingID As System.Guid, ByVal PersonID As Integer, ByVal EndDate As DateTime)
        Sub AddBrowserInfo(ByVal oBrowser As BrowserInfo)
        Sub OpenWorkingSession(ByVal oAction As UserAction)
        Sub CloseWorkingSession(ByVal oAction As UserAction)
        Sub AddAction(ByVal oAction As UserAction)
        Function UserOnlineCount() As Integer
    End Interface
End Namespace