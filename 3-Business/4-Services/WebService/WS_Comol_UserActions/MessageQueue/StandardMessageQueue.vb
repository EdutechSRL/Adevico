Imports lm.ActionDataContract
Imports System.Transactions
Imports System.Messaging
Imports lm.WS.UserAction.Configuration

Namespace lm.WS.UserAction.Domain
	Public Class StandardMessageQueue
        Implements iActionService


#Region "Private"
        Private Shared _ActionQueue As System.Messaging.MessageQueue = New MessageQueue(ServiceUtility.Config.ActionQueueName)
        Private Shared _PoisonQueue As System.Messaging.MessageQueue = New MessageQueue(ServiceUtility.Config.PoisonQueueName)
        Protected Shared ReadOnly Property ActionQueue() As System.Messaging.MessageQueue
            Get
                Return _ActionQueue
            End Get
        End Property
        Protected Shared ReadOnly Property PoisonQueue() As System.Messaging.MessageQueue
            Get
                Return _PoisonQueue
            End Get
        End Property
#End Region

        Sub New()

        End Sub


        Public Sub InsertLoginAction(ByVal oLoginAction As ActionDataContract.LoginAction) Implements ActionDataContract.iActionService.InsertLoginAction
            StandardMessageQueue.SendAction(oLoginAction)
        End Sub

        Public Sub UpdateLoginAction(ByVal oLoginAction As ActionDataContract.LoginAction) Implements ActionDataContract.iActionService.UpdateLoginAction
            StandardMessageQueue.SendAction(oLoginAction)
        End Sub

        Private Shared Sub SendAction(ByVal oAction As Object)
            Try
                Using txScope As TransactionScope = New TransactionScope(TransactionScopeOption.RequiresNew)
                    ActionQueue.Send(oAction, oAction.GetType.Name, ServiceUtility.Config.ActionQueueTransaction)
                    txScope.Complete()
                End Using
            Catch ex As Exception

            End Try
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