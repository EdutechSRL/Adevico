Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class UsersSystemStatisticsPresenter
        Inherits DomainPresenter

#Region "Standard"
        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = BaseDomainManager.GetModuleID(ModuleStatistics.UniqueId)
                End If
                Return _ModuleID
            End Get
        End Property

        Private _BaseDomainManager As BaseModuleManager
        Public Property BaseDomainManager() As BaseModuleManager
            Get
                Return _BaseDomainManager
            End Get
            Set(value As BaseModuleManager)
                _BaseDomainManager = value
            End Set
        End Property
        Public Overloads Property CurrentManager() As ManagerUsageStatistic
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerUsageStatistic)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewUsersSystemStatistics
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUsersSystemStatistics)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        'Private _StatContext As UsageContext
        'Private ReadOnly Property StatContext() As UsageContext
        '    Get
        '        If IsNothing(_StatContext) Then
        '            Dim oContext As UsageContext = Me.View.StatisticContext
        '            Dim idPerson As Integer = oContext.UserID
        '            If idPerson = 0 AndAlso (Me.View.CurrentView = IviewUsageStatistic.viewType.PersonalCommunity OrElse Me.View.CurrentView = IviewUsageStatistic.viewType.Personal) Then
        '                idPerson = Me.AppContext.UserContext.CurrentUser.Id
        '            End If
        '            If oContext.Order = StatisticOrder.None Then
        '                oContext.Order = StatisticOrder.UsageTime
        '                oContext.Ascending = False
        '            End If
        '            oContext.UserID = idPerson
        '            Dim CommunityID As Integer = oContext.CommunityID
        '            If CommunityID <= 0 AndAlso (Me.View.CurrentView = IviewUsageStatistic.viewType.PersonalCommunity OrElse Me.View.CurrentView = IviewUsageStatistic.viewType.CommunityUsers) Then
        '                CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.CommunityID = CommunityID
        '            Me.View.StatisticContext = oContext
        '            _StatContext = oContext
        '        End If
        '        Return _StatContext
        '    End Get
        'End Property

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim items As List(Of StatisticView) = GetAvailableViews()
                View.LoadAvailableView(items)
                If items.Count > 0 AndAlso items.Contains(StatisticView.UsersSystem) Then
                    Dim oContext As UsageContext = GetDefaultContext()
                    View.StatisticContext = oContext
                    Me.LoadGlobalUsers(oContext)
                ElseIf items.Count > 0 Then
                    View.LoadView(RootObject.MySystemStatistics())
                Else
                    Me.View.NoPermissionToAccess()
                    View.SendAction(0, ModuleID, ModuleStatistics.ActionType.NoPermission)
                End If
            Else
                View.DisplaySessionTimeout()
            End If
        End Sub
        Public Sub LoadGlobalUsers(ByVal oUsage As UsageContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            oSummary.CommunityName = View.PortalName

            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetPortalUsersStatistics(oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)

            Dim oSummaryContext As UsageContext = oUsage.Clone
            oSummaryContext.UserID = -1
            oSummaryContext.CommunityID = -1
            oSummary.NavigateTo = RootObject.UsageDetails(oSummaryContext.UserID, oSummaryContext.CommunityID, DetailViewType.Month, StatisticView.UsersSystem, View.CurrentUrl)
            'Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oSummaryContext, Me.View.CurrentView, IViewUsageDetails.viewType.Month)

            Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Portal)

            Me.View.Pager = oPager
            Dim backUrl As String = View.CurrentUrl
            For Each o In oStatistic.Items
                o.NavigateTo = RootObject.UserSystemStatistics(0, o.ID, "", StatisticOrder.UsageTime, False, backUrl, StatisticView.UsersSystem)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
            View.SendAction(0, ModuleID, ModuleStatistics.ActionType.LoadPortalUsersStatistics)
        End Sub
        Private Function GetAvailableViews() As List(Of StatisticView)
            Dim items As New List(Of StatisticView)
            Dim idType As Integer = Me.AppContext.UserContext.UserTypeID

            items.Add(StatisticView.MySystem)
            If idType = UserTypeStandard.Administrative OrElse idType = UserTypeStandard.Administrator OrElse idType = UserTypeStandard.SysAdmin Then
                items.Add(StatisticView.UsersSystem)
                items.Add(StatisticView.System)
            End If
            Return items
        End Function
        Private Function HasPermissionToSee(idCommunity As Integer) As Boolean
            Dim moduleP As ModuleStatistics = GetPermissions(idCommunity)
            Return moduleP.Administration OrElse moduleP.ListSelfStatistic
        End Function
        Public Function GetPermissions(idCommunity As Integer) As ModuleStatistics
            Dim moduleP As ModuleStatistics = ModuleStatistics.CreatePortalmodule(UserContext.UserTypeID)
            If idCommunity > 0 Then
                Dim idRole As Integer = BaseDomainManager.GetDefaultIdRole(idCommunity)
                Dim IsForHistory As Boolean = Not BaseDomainManager.HasActiveSubscription(UserContext.CurrentUserID, idCommunity)

                Dim moduleC As ModuleStatistics = Nothing
                If IsForHistory Then
                    moduleC = New ModuleStatistics(BaseDomainManager.GetModulePermissionByRole(idRole, idCommunity, ModuleID))
                Else
                    moduleC = New ModuleStatistics(BaseDomainManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID))
                End If
                If Not IsNothing(moduleC) Then
                    With moduleP
                        .Administration = (moduleP.Administration OrElse moduleC.Administration)
                        .Export = (moduleP.Export OrElse moduleC.Export)
                        .ListOtherStatistic = (moduleP.ListOtherStatistic OrElse moduleC.ListOtherStatistic)
                        .ListSelfStatistic = (moduleP.ListSelfStatistic OrElse moduleC.ListSelfStatistic)
                        .ViewDetails = (moduleP.ViewDetails OrElse moduleC.ViewDetails)
                        .ViewGenericOtherStatistic = (moduleP.ViewGenericOtherStatistic OrElse moduleC.ViewGenericOtherStatistic)
                    End With
                End If
            End If
            Return moduleP
        End Function
        Private Function GetDefaultContext() As UsageContext
            Dim oContext As UsageContext = Me.View.StatisticContext
            If oContext.Order = StatisticOrder.None Then
                oContext.Order = StatisticOrder.UsageTime
                oContext.Ascending = False
            End If
            'oContext.UserID = 0
            oContext.CommunityID = -1
            oContext.ModuleID = -1
            Return oContext
        End Function
    End Class
End Namespace