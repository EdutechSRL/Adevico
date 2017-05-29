Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class OnLineUserAction
        <DataMember()> Public ID As System.Guid
        <DataMember()> Public ActionDate As DateTime
        <DataMember()> Public AccessDate As DateTime
        <DataMember()> Public CommunityID As Integer
        <DataMember()> Public ModuleID As Integer
        <DataMember()> Public PersonID As Integer
        <DataMember()> Public Type As Integer
        <DataMember()> Public WorkingSessionID As System.Guid
        <DataMember()> Public ClientIPadress As String
        <DataMember()> Public ProxyIPadress As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal oAction As ActionDataContract.UserAction, ByVal defaultDate As DateTime)
            ID = oAction.ID
            ActionDate = oAction.ActionDate
            CommunityID = oAction.CommunityID
            ModuleID = oAction.ModuleID
            PersonID = oAction.PersonID
            Type = oAction.Type
            WorkingSessionID = oAction.WorkingSessionID
            ClientIPadress = oAction.ClientIPadress
            ProxyIPadress = oAction.ProxyIPadress
            AccessDate = defaultDate
        End Sub
    End Class
End Namespace