Imports System.ServiceModel
Imports lm.WS.UserAccessMonitor.DataContracts
Imports lm.WS.ActionStatistics.Domain

<ServiceContract()> _
Public Interface IServiceUserAccessMonitor

    <OperationContract()> _
    Function GetCommunityAccess(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonID As Integer, ByVal CommunityID As Integer) As List(Of AccessResult)

    <OperationContract()> _
    Function GetPortalAccess(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonID As Integer) As List(Of AccessResult)

    <OperationContract()> _
    Function GetCommunityUserAccessResult(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonsID As List(Of Integer), ByVal CommunityID As Integer) As List(Of UserWithResult)

    <OperationContract()> _
    Function GetPortalUserAccessResult(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonsID As List(Of Integer)) As List(Of UserWithResult)

    <OperationContract()> _
    Function FindCommunitiesWithAccessResult(ByVal PersonD As Integer) As List(Of UserWithResult)

    <OperationContract()> _
    Function FindPortalUsersWithAccessResult() As List(Of UserWithResult)

    <OperationContract()> _
    Function FindUsersWithAccessResult(ByVal CommunityID As Integer) As List(Of UserWithResult)


    <OperationContract()> _
    Function GetCommunityUsersAccessResultBetweenDate(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal CommunityID As Integer) As List(Of AccessResult)

    <OperationContract()> _
    Function GetPortalUsersAccessResultBetweenDate(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate) As List(Of AccessResult)

    <OperationContract()> _
    Function GetCommunityAccessResultBetweenDate(ByVal PersonIDList As List(Of Integer), ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal CommunityID As Integer) As List(Of AccessResult)

    <OperationContract()> _
    Function GetPortalAccessResultBetweenDate(ByVal PersonIDList As List(Of Integer), ByVal FromDate As dtoDate, ByVal ToDate As dtoDate) As List(Of AccessResult)

End Interface