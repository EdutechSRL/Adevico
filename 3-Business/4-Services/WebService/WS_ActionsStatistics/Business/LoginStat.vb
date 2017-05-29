Imports lm.WS.ActionStatistics.Domain
Imports lm.WS.ActionStatistics.DAL

Namespace lm.WS.ActionStatistics.Business
	Public Class LoginStat
        '    Implements IstatisticService

        '    Public Function GetStatCommunity_daily(ByRef PersonID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As Domain.BaseStatAction_List Implements IstatisticService.GetStatCommunity_daily
        '        Dim oDalActionStat As New DALactionStat
        '        If ModuleID = 0 Then
        '            Return oDalActionStat.getDailyAccessesByPerson(PersonID, startDate, endDate)
        '        End If


        '        Return New BaseStatAction_List
        '    End Function

        '    Public Function GetStatCommunity_global(ByRef PersonID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As Domain.BaseStatAction_List Implements IstatisticService.GetStatCommunity_global

        '        Dim oDalActionStat As New DALactionStat
        '        If ModuleID = 0 Then
        '            Return oDalActionStat.getGlobalAccessesByPerson(PersonID, startDate, endDate)
        '        End If

        '        Return New BaseStatAction_List
        '    End Function

        '    Public Function GetStatModule_daily(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As Domain.BaseStatAction_List Implements IstatisticService.GetStatModule_daily
        '        Return New BaseStatAction_List

        '    End Function

        '    Public Function GetStatModule_global(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As Domain.BaseStatAction_List Implements IstatisticService.GetStatModule_global
        '        Dim RetVal As New BaseStatAction_List
        '        Dim oDalActionStat As New DALactionStat
        '        If CommunityID = 0 Then
        '            If PersonID = 0 Then
        '                Return oDalActionStat.getGlobalModulesByPerson(startDate, endDate)
        '            Else
        '                Return oDalActionStat.getGlobalModulesByPerson(startDate, endDate, , CommunityID)
        '            End If
        '        End If
        '        Return RetVal
        '    End Function

        '    Public Function GetStatUser_daily(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As Domain.BaseStatAction_List Implements IstatisticService.GetStatUser_daily
        '        Return New BaseStatAction_List

        '    End Function

        '    Public Function GetStatUser_global(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As Domain.BaseStatAction_List Implements IstatisticService.GetStatUser_global
        '        Return New BaseStatAction_List

        '    End Function
    End Class
End Namespace
