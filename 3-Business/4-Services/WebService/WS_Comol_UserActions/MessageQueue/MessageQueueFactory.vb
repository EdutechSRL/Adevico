Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.ActionDataContract
Imports lm.WS.UserAction.Configuration
Imports System.Reflection

Namespace lm.WS.UserAction.Domain
	Public Class MessageQueueFactory

		Public Shared ReadOnly Property Service() As iActionService
			Get
				Return MessageQueueSetup()
			End Get
		End Property

		Public Shared Function MessageQueueSetup() As iActionService
			Dim oService As iActionService = Nothing

            Try
                oService = FactoryBuilder.BuildFactory(Assembly.GetExecutingAssembly, ServiceUtility.Config.MessageQueueServiceClass)
            Catch ex As Exception

            End Try
			If IsNothing(oService) Then
				oService = New FakeMessageQueue
			End If
			Return oService
		End Function
	End Class
End Namespace