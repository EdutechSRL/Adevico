Imports System.Runtime.Serialization
Imports lm.WS.ActionStatistics.Domain

Namespace lm.WS.UserAccessMonitor.DataContracts
    <Serializable(), DataContract()> Public Class AccessResult
        <DataMember()> Public PersonID As Integer
        <DataMember()> Public CommunityID As Integer
        <DataMember()> Public Hour As Integer
        <DataMember()> Public Day As Date
        <DataMember()> Public UsageTime As Integer
    End Class
End Namespace