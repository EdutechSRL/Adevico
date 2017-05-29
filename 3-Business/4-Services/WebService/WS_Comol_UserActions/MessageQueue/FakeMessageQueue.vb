Imports lm.ActionDataContract

Namespace lm.WS.UserAction.Domain
	Public Class FakeMessageQueue
        Implements iActionService


        Public Sub InsertLoginAction(ByVal oLoginAction As ActionDataContract.LoginAction) Implements ActionDataContract.iActionService.InsertLoginAction

        End Sub

        Public Sub UpdateLoginAction(ByVal oLoginAction As ActionDataContract.LoginAction) Implements ActionDataContract.iActionService.UpdateLoginAction

        End Sub

        Public Sub InsertUserAction(ByVal oAction As ActionDataContract.UserAction) Implements ActionDataContract.iActionService.InsertUserAction

        End Sub

        Public Sub InsertBrowserInfo(ByVal oBrowser As ActionDataContract.BrowserInfo) Implements ActionDataContract.iActionService.InsertBrowserInfo

        End Sub

        Public Sub UpdateModuleUsageTime(ByVal oUsage As ActionDataContract.ModuleUsageTime) Implements ActionDataContract.iActionService.UpdateModuleUsageTime

        End Sub

        Public Sub InsertCommunityAction(ByVal oAction As ActionDataContract.CommunityAction) Implements ActionDataContract.iActionService.InsertCommunityAction

        End Sub

        Public Sub UpdateCommunityAction(ByVal oAction As ActionDataContract.CommunityAction) Implements ActionDataContract.iActionService.UpdateCommunityAction

        End Sub

        Public Sub InsertModuleAction(ByVal oAction As ActionDataContract.ModuleAction) Implements ActionDataContract.iActionService.InsertModuleAction

        End Sub

        Public Sub UpdateModuleAction(ByVal oAction As ActionDataContract.ModuleAction) Implements ActionDataContract.iActionService.UpdateModuleAction

        End Sub
    End Class
End Namespace