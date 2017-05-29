Imports lm.WS.ActionStatistics.Domain

Namespace lm.WS.ActionStatistics.Business
	Public Interface IstatisticService
        Function GetStatUser_global(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As BaseStatAction_List
        Function GetStatCommunity_global(ByRef PersonID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As BaseStatAction_List
        Function GetStatModule_global(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As BaseStatAction_List
        Function GetStatUser_daily(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As BaseStatAction_List
        Function GetStatCommunity_daily(ByRef PersonID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As BaseStatAction_List
        Function GetStatModule_daily(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As BaseStatAction_List
    End Interface
End Namespace