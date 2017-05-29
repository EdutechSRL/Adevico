Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoBaseStatistic
        Public ID As Integer
        Public Type As StatisticType
        Public Name As String
        Public Description As String
        Public nAccesses As Long
		Public UsageTime As Integer
		Public NavigateTo As String
		Public NavigateToDetails As String
        Public Subscribed As Boolean
        Public Permission As ModuleStatistics

        Public Function ToTimeSpan() As TimeSpan
            Return New TimeSpan(0, 0, UsageTime)
        End Function

        Sub New()
            UsageTime = 0
            nAccesses = 0
            Name = ""
            Description = ""
			ID = 0
			NavigateTo = ""
			NavigateToDetails = ""
        End Sub
    End Class
End Namespace