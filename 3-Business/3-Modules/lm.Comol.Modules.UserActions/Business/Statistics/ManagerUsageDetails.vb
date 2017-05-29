Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
Imports NHibernate
Imports WSstatistics

Namespace lm.Comol.Modules.UserActions.BusinessLogic
    Public Class ManagerUsageDetails
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
        Private _BaseService As BaseStatisticsService
#End Region

#Region "Public property"
        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
        Private ReadOnly Property BaseService() As BaseStatisticsService
            Get
                If IsNothing(_BaseService) Then
                    _BaseService = New BaseStatisticsService(_UserContext, _Datacontext)
                End If
                Return _BaseService
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub

        'Public Function GetStatActions(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal startDate As dtoDate, ByVal endDate As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Dim oBaseActionList As New List(Of BaseStatAction)
        '    oBaseActionList = Service.GetStatCommunity_daily(PersonID, 0, 0, startDate, endDate)
        '    Return oBaseActionList
        'End Function
        'Public Function GetUsers() As List(Of Person)
        '    Return BaseService.GetSystemPersonList(Me.CurrentUserContext.CurrentUser.Id)
        'End Function
        'Public Function GetCommunities() As List(Of Community)
        '    Return BaseService.GetSubscribedCommunityList(Me.CurrentUserContext.CurrentUser.Id)
        'End Function
        Public Function GetModules() As List(Of COL_BusinessLogic_v2.PlainService)
            Return BaseService.GetGenericModuleList(-1, -1)
        End Function
#Region "Daily"
        'Public Function GetStatModule_daily(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Return (Service.GetStatModule_daily(PersonID, CommunityId, ModuleID, startDate, endDate))
        'End Function
        'Public Function GetStatCommunity_daily(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Return (Service.GetStatCommunity_daily(PersonID, CommunityId, ModuleID, startDate, endDate))
        'End Function
        'Public Function GetStatPerson_daily(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Return (Service.GetStatPerson_daily(PersonID, CommunityID, ModuleID, startDate, endDate))
        'End Function
        ''Public Function GetStatAccess_daily(ByVal PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        ''    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        ''    Return (Service.GetStatAccess_daily(PersonID, CommunityID, ModuleID, startDate, endDate))
        ''End Function
#End Region

#Region "Hourly"

        'Public Function GetStatPerson_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Return (Service.GetStatPerson_hourly(PersonID, CommunityID, ModuleID, day))
        'End Function
        ''Public Function GetStatAccess_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        ''    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        ''    Return (Service.GetStatAccess_hourly(PersonID, CommunityID, ModuleID, day))
        ''End Function
        'Public Function GetStatModule_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Return (Service.GetStatModule_hourly(PersonID, CommunityID, ModuleID, day))
        'End Function
        'Public Function GetStatCommunity_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        '    Dim Service As New WSstatistics.WSactionStatisticsSoapClient
        '    Return (Service.GetStatCommunity_hourly(PersonID, CommunityID, ModuleID, day))
        'End Function
#End Region

    End Class
End Namespace