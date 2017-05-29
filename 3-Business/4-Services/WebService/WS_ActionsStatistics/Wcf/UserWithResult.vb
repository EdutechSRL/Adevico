Imports System.Runtime.Serialization
Imports lm.WS.ActionStatistics.Domain

Namespace lm.WS.UserAccessMonitor.DataContracts
    <Serializable(), DataContract()> Public Class UserWithResult
        <DataMember()> Public PersonID As Integer
        <DataMember()> Public CommunityID As Integer
        <DataMember()> Public Result As Integer
    End Class
End Namespace