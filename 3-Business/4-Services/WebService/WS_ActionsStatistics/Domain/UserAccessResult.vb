Imports lm.WS.ActionStatistics.Domain
Imports lm.WS.ActionStatistics.DAL

Namespace lm.WS.ActionStatistics.Domain
    <Serializable(), CLSCompliant(True)> Public Class UserAccessResult
        Public PersonID As Integer
        Public CommunityID As Integer
        Public UsageTime As Integer
        Public StartDate As DateTime
        Public EndDate As DateTime
        Public IsClosed As Boolean
    End Class
End Namespace