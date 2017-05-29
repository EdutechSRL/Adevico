Namespace lm.WS.ActionStatistics.Domain
	<Serializable(), CLSCompliant(True)> Public Class BaseStatAction
		Public ID As Integer
		Public CommunityID As Integer
		Public ModuleID As Integer
		Public PersonID As Integer
		Public nAccess As Integer
        Public UsageTime As Integer
        Public Hour As Integer
		Public Day As Integer
		Public Month As Integer
		Public Year As Integer
	End Class
End Namespace