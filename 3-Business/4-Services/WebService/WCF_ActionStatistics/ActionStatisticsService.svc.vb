' NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
Public Class ActionStatisticsService
	Implements iStatisticsService

	Public Sub New()
	End Sub

	Public Function GetData(ByVal value As Integer) As String Implements iStatisticsService.GetData
		Return String.Format("You entered: {0}", value)
	End Function

	Public Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType Implements iStatisticsService.GetDataUsingDataContract
		If composite.BoolValue Then
			composite.StringValue = (composite.StringValue & "Suffix")
		End If
		Return composite
	End Function

End Class
