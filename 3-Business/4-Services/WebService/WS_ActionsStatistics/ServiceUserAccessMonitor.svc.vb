Imports lm.WS.UserAccessMonitor.DataContracts
Imports lm.WS.ActionStatistics.Domain
Imports lm.WS.ActionStatistics.DAL
Imports lm.WS.ActionStatistics.Business

' NOTE: If you change the class name "ServiceUserAccessMonitor" here, you must also update the reference to "ServiceUserAccessMonitor" in Web.config and in the associated .svc file.
Public Class ServiceUserAccessMonitor
    Implements IServiceUserAccessMonitor


    Public Function GetPortalAccess(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonID As Integer) As List(Of AccessResult) Implements IServiceUserAccessMonitor.GetPortalAccess
        Dim oList As New List(Of AccessResult)
        oList = RetrieveResults(FromDate, ToDate, PersonID, -9999)
        Return oList
    End Function

    Public Function GetCommunityAccess(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonID As Integer, ByVal CommunityID As Integer) As List(Of AccessResult) Implements IServiceUserAccessMonitor.GetCommunityAccess
        Dim oList As New List(Of AccessResult)

        oList = RetrieveResults(FromDate, ToDate, PersonID, CommunityID)
        Return oList
    End Function

    Public Function GetCommunityUserAccessResult(ByVal FromDate As lm.WS.ActionStatistics.Domain.dtoDate, ByVal ToDate As lm.WS.ActionStatistics.Domain.dtoDate, ByVal PersonsID As List(Of Integer), ByVal CommunityID As Integer) As List(Of UserWithResult) Implements IServiceUserAccessMonitor.GetCommunityUserAccessResult
        Dim oList As New List(Of UserWithResult)
        oList = RetrieveUsers(FromDate, ToDate, PersonsID, CommunityID)
        Return oList
    End Function

    Public Function GetPortalUserAccessResult(ByVal FromDate As lm.WS.ActionStatistics.Domain.dtoDate, ByVal ToDate As lm.WS.ActionStatistics.Domain.dtoDate, ByVal PersonsID As List(Of Integer)) As List(Of UserWithResult) Implements IServiceUserAccessMonitor.GetPortalUserAccessResult
        Dim oList As New List(Of UserWithResult)
        oList = RetrieveUsers(FromDate, ToDate, PersonsID, -9999)
        Return oList
    End Function
    Public Function FindCommunitiesWithAccessResult(ByVal PersonD As Integer) As System.Collections.Generic.List(Of UserWithResult) Implements IServiceUserAccessMonitor.FindCommunitiesWithAccessResult
        Dim oResults As New List(Of UserWithResult)
        Dim oDalActionStat As New DALactionStat

        oResults = oDalActionStat.FindCommunitiesWithAccessResult(PersonD)
        Return (From o In oResults Where o.Result > 0 Select o).ToList
    End Function
    Public Function FindPortalUsersWithAccessResult() As System.Collections.Generic.List(Of UserWithResult) Implements IServiceUserAccessMonitor.FindPortalUsersWithAccessResult
        Dim oResults As New List(Of UserWithResult)
        Dim oDalActionStat As New DALactionStat

        oResults = oDalActionStat.FindPortalUsersWithAccessResult()
        Return (From o In oResults Where o.Result > 0 Select o).ToList
    End Function

    Public Function FindUsersWithAccessResult(ByVal CommunityID As Integer) As System.Collections.Generic.List(Of lm.WS.UserAccessMonitor.DataContracts.UserWithResult) Implements IServiceUserAccessMonitor.FindUsersWithAccessResult
        Dim oResults As New List(Of UserWithResult)
        Dim oDalActionStat As New DALactionStat

        oResults = oDalActionStat.FindUsersWithAccessResult(CommunityID)
        Return (From o In oResults Where o.Result > 0 Select o).ToList
    End Function

    Public Function GetCommunityUsersAccessResultBetweenDate(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal CommunityID As Integer) As List(Of AccessResult) Implements IServiceUserAccessMonitor.GetCommunityUsersAccessResultBetweenDate
        Dim oList As New List(Of AccessResult)

        oList = RetrieveUsersResultsBetweenDate(FromDate, ToDate, CommunityID)
        Return oList
    End Function

    Public Function GetPortalUsersAccessResultBetweenDate(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate) As List(Of AccessResult) Implements IServiceUserAccessMonitor.GetPortalUsersAccessResultBetweenDate
        Dim oList As New List(Of AccessResult)

        oList = RetrieveUsersResultsBetweenDate(FromDate, ToDate, -9999)
        Return oList
    End Function



    Public Function GetCommunityAccessResultBetweenDate(ByVal PersonIDList As List(Of Integer), ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal CommunityID As Integer) As List(Of AccessResult) Implements IServiceUserAccessMonitor.GetCommunityAccessResultBetweenDate
        Dim oList As New List(Of AccessResult)

        oList = RetrieveUsersResultsBetweenDate(PersonIDList, FromDate, ToDate, CommunityID)
        Return oList
    End Function

    Public Function GetPortalAccessResultBetweenDate(ByVal PersonIDList As List(Of Integer), ByVal FromDate As dtoDate, ByVal ToDate As dtoDate) As List(Of AccessResult) Implements IServiceUserAccessMonitor.GetPortalAccessResultBetweenDate
        Dim oList As New List(Of AccessResult)

        oList = RetrieveUsersResultsBetweenDate(PersonIDList, FromDate, ToDate, -9999)
        Return oList
    End Function

#Region "Management Hour -usage"
    Private Function RetrieveUsersResultsBetweenDate(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal CommunityID As Integer) As List(Of AccessResult)
        Dim oResults As List(Of UserAccessResult)
        Dim oDalActionStat As New DALactionStat
        Dim Today As DateTime = Now
        Dim FromDateTime, ToDateTime As DateTime

        If IsNothing(FromDate) Then
            FromDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If
        If IsNothing(ToDate) Then
            ToDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If

        FromDateTime = New DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0)
        ToDateTime = New DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)
        ToDateTime = ToDateTime.AddDays(1)
        ToDate = New dtoDate(ToDateTime)
        If CommunityID = -9999 Then
            oResults = oDalActionStat.getUsersPortalResultsBetweenDate(FromDateTime, ToDateTime)
        Else
            oResults = oDalActionStat.getUsersCommunityResultsBetweenDate(CommunityID, FromDateTime, ToDateTime)
        End If
        Return AnalyzeAccessResults(oResults, FromDate, ToDate)
    End Function

    Private Function RetrieveUsersResultsBetweenDate(ByVal PersonIDList As List(Of Integer), ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal CommunityID As Integer) As List(Of AccessResult)
        Dim oResults As List(Of UserAccessResult)
        Dim oDalActionStat As New DALactionStat
        Dim Today As DateTime = Now
        Dim FromDateTime, ToDateTime As DateTime

        If IsNothing(FromDate) Then
            FromDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If
        If IsNothing(ToDate) Then
            ToDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If

        FromDateTime = New DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0)
        ToDateTime = New DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)
        ToDateTime = ToDateTime.AddDays(1)
        ToDate = New dtoDate(ToDateTime)
        If CommunityID = -9999 Then
            oResults = oDalActionStat.getUsersPortalResultsBetweenDate(PersonIDList, FromDateTime, ToDateTime)
        Else
            oResults = oDalActionStat.getUsersCommunityResultsBetweenDate(PersonIDList, CommunityID, FromDateTime, ToDateTime)
        End If
        Return AnalyzeAccessResults(oResults, FromDate, ToDate)
    End Function



    Private Function RetrieveResults(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonID As Integer, ByVal CommunityID As Integer) As List(Of AccessResult)
        Dim oResults As List(Of UserAccessResult)
        Dim oDalActionStat As New DALactionStat
        Dim Today As DateTime = Now
        Dim FromDateTime, ToDateTime As DateTime

        If IsNothing(FromDate) Then
            FromDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If
        If IsNothing(ToDate) Then
            ToDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If

        FromDateTime = New DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0)
        ToDateTime = New DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)
        ToDateTime = ToDateTime.AddDays(1)
        ToDate = New dtoDate(ToDateTime)
        If CommunityID = -9999 Then
            oResults = oDalActionStat.getPortalAccessResults(PersonID, FromDateTime, ToDateTime)
        Else
            oResults = oDalActionStat.getCommunityAccessResults(PersonID, CommunityID, FromDateTime, ToDateTime)
        End If
        Return AnalyzeAccessResults(oResults, FromDate, ToDate)
    End Function
    Private Function AnalyzeAccessResults(ByVal oResults As List(Of UserAccessResult), ByVal FromDate As dtoDate, ByVal ToDate As dtoDate) As List(Of AccessResult)
        Dim oList As New List(Of AccessResult)

        Dim oListBeforeDate As New List(Of UserAccessResult)
        Dim oListAfterDate As New List(Of UserAccessResult)
        Dim oListBetweenDate As New List(Of UserAccessResult)

        oListBeforeDate = (From o In oResults Where o.StartDate < FromDate.ToDateTime _
                Select New UserAccessResult() With {.PersonID = o.PersonID, .CommunityID = o.CommunityID, .IsClosed = o.IsClosed, .EndDate = o.EndDate, .StartDate = New DateTime(FromDate.Year, FromDate.Month, FromDate.Day), .UsageTime = GetUsageTimes(FromDate.ToDateTime, o.EndDate)}).ToList()

        If oListBeforeDate.Count > 0 Then
            For Each result In oListBeforeDate
                UpdateResult(result, oList)
            Next
        End If

        oListAfterDate = (From o In oResults Where o.EndDate >= ToDate.ToDateTime _
               Select New UserAccessResult() With {.PersonID = o.PersonID, .CommunityID = o.CommunityID, .IsClosed = o.IsClosed, .StartDate = o.StartDate, .EndDate = New DateTime(ToDate.Year, ToDate.Month, ToDate.Day), .UsageTime = GetUsageTimes(o.StartDate, ToDate.ToDateTime)}).ToList()

        If oListAfterDate.Count > 0 Then
            For Each result In oListAfterDate
                UpdateResult(result, oList)
            Next
        End If
        oListBetweenDate = (From o In oResults Where o.StartDate >= FromDate.ToDateTime AndAlso o.EndDate < ToDate.ToDateTime _
              Select New UserAccessResult() With {.PersonID = o.PersonID, .CommunityID = o.CommunityID, .IsClosed = o.IsClosed, .StartDate = o.StartDate, .EndDate = o.EndDate, .UsageTime = GetUsageTimes(o.StartDate, o.EndDate)}).ToList()

        If oListBetweenDate.Count > 0 Then
            For Each result In oListBetweenDate
                UpdateResult(result, oList)
            Next
        End If
        Return (From o In oList Order By o.Hour Select o).ToList()
    End Function
    Private Sub UpdateResult(ByVal oResult As UserAccessResult, ByVal oList As List(Of AccessResult))
        Dim oAccessResult As AccessResult

        oAccessResult = (From ar In oList Where ar.Day.Day = oResult.StartDate.Day And ar.CommunityID = oResult.CommunityID And ar.PersonID = oResult.PersonID AndAlso ar.Hour = oResult.StartDate.Hour Select ar).FirstOrDefault()
        If IsNothing(oAccessResult) Then
            oAccessResult = New AccessResult
            With oAccessResult
                .UsageTime = 0
                .PersonID = oResult.PersonID
                .Hour = oResult.StartDate.Hour
                .Day = oResult.StartDate
                .CommunityID = oResult.CommunityID
            End With
            oList.Add(oAccessResult)
        End If


        If oResult.StartDate.Hour = oResult.EndDate.Hour Then
            oAccessResult.UsageTime += GetUsageTimes(oResult.StartDate, oResult.EndDate)
            If oAccessResult.UsageTime > 3600 Then
                oAccessResult.UsageTime = 3600
            End If
        Else
            Dim EndDate As DateTime = New DateTime(oResult.StartDate.Year, oResult.StartDate.Month, oResult.StartDate.Day, oResult.StartDate.Hour, 0, 0)
            EndDate = EndDate.AddHours(1)
            oAccessResult.UsageTime += GetUsageTimes(oResult.StartDate, EndDate)
            If oAccessResult.UsageTime > 3600 Then
                oAccessResult.UsageTime = 3600
            End If
            If EndDate < oResult.EndDate Then
                oResult.StartDate = EndDate
                UpdateResult(oResult, oList)
            End If
        End If
    End Sub
    Private Shared Function GetUsageTimes(ByVal oDataI As DateTime, ByVal oDataF As DateTime) As Integer
        Dim SerialI As DateTime = New DateTime(oDataI.Year, oDataI.Month, oDataI.Day, oDataI.Hour, oDataI.Minute, oDataI.Second)
        Dim SerialF As DateTime = New DateTime(oDataF.Year, oDataF.Month, oDataF.Day, oDataF.Hour, oDataF.Minute, oDataF.Second)
        Return (SerialF - SerialI).TotalSeconds
    End Function
#End Region

    Private Function RetrieveUsers(ByVal FromDate As dtoDate, ByVal ToDate As dtoDate, ByVal PersonsID As List(Of Integer), ByVal CommunityID As Integer) As List(Of UserWithResult)
        Dim oResults As New List(Of UserWithResult)
        Dim oDalActionStat As New DALactionStat
        Dim Today As DateTime = Now
        Dim FromDateTime, ToDateTime As DateTime

        If IsNothing(FromDate) Then
            FromDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If
        If IsNothing(ToDate) Then
            ToDate = New dtoDate(Today.Year, Today.Month, Today.Day)
        End If

        FromDateTime = New DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0)
        ToDateTime = New DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)
        ToDateTime = ToDateTime.AddDays(1)
        ToDate = New dtoDate(ToDateTime)

        Dim oResult As UserWithResult
        For Each id In PersonsID
            oResult = oDalActionStat.getCommunityUserWithResults(id, CommunityID, FromDateTime, ToDateTime)
            oResults.Add(oResult)
        Next
        Return (From o In oResults Where o.Result > 0 Select o).ToList
    End Function

End Class