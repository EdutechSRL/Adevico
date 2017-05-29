Imports lm.ActionDataContract


Namespace lm.WS.UserAction.Domain
	Public Class WCFmessageQueue
        Implements lm.ActionDataContract.iActionService

        Private ReadOnly Property Service() As RefActionService.iActionService
            Get
                Return New RefActionService.iActionServiceClient
            End Get
        End Property


        Public Sub InsertLoginAction(ByVal oLoginAction As ActionDataContract.LoginAction) Implements ActionDataContract.iActionService.InsertLoginAction
            Service.InsertLoginAction(oLoginAction)
        End Sub
        Public Sub UpdateLoginAction(ByVal oLoginAction As ActionDataContract.LoginAction) Implements ActionDataContract.iActionService.UpdateLoginAction

            Service.UpdateLoginAction(oLoginAction)
        End Sub


        Public Sub InsertUserAction(ByVal oAction As ActionDataContract.UserAction) Implements ActionDataContract.iActionService.InsertUserAction
            Service.InsertUserAction(oAction)
        End Sub

        Public Sub InsertBrowserInfo(ByVal oBrowser As ActionDataContract.BrowserInfo) Implements ActionDataContract.iActionService.InsertBrowserInfo
            Service.InsertBrowserInfo(oBrowser)
        End Sub

        Public Sub UpdateModuleUsageTime(ByVal oUsage As ActionDataContract.ModuleUsageTime) Implements ActionDataContract.iActionService.UpdateModuleUsageTime
            Service.UpdateModuleUsageTime(oUsage)
        End Sub

        Public Sub InsertCommunityAction(ByVal oAction As ActionDataContract.CommunityAction) Implements ActionDataContract.iActionService.InsertCommunityAction
            Service.InsertCommunityAction(oAction)
        End Sub

        Public Sub UpdateCommunityAction(ByVal oAction As ActionDataContract.CommunityAction) Implements ActionDataContract.iActionService.UpdateCommunityAction
            Service.UpdateCommunityAction(oAction)
        End Sub

        Public Sub InsertModuleAction(ByVal oAction As ActionDataContract.ModuleAction) Implements ActionDataContract.iActionService.InsertModuleAction
            Service.InsertModuleAction(oAction)
        End Sub

        Public Sub UpdateModuleAction(ByVal oAction As ActionDataContract.ModuleAction) Implements ActionDataContract.iActionService.UpdateModuleAction
            Service.UpdateModuleAction(oAction)
        End Sub
    End Class
End Namespace