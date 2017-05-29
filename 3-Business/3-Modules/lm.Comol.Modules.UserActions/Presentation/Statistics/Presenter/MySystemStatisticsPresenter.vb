Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class MySystemStatisticsPresenter
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
        Public Overloads ReadOnly Property View() As IViewMySystemStatistics
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewMySystemStatistics)
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
                If items.Count > 0 Then
                    Dim oContext As UsageContext = GetDefaultContext()
                    View.StatisticContext = oContext
                    Me.LoadGlobal(oContext)
                Else
                    View.SendAction(0, ModuleID, ModuleStatistics.ActionType.NoPermission)
                    Me.View.NoPermissionToAccess()
                End If
            Else
                View.DisplaySessionTimeout()
            End If
        End Sub
        Public Sub LoadGlobal(ByVal oUsage As UsageContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            oSummary.NavigateTo = RootObject.UsageDetails(oUsage.UserID, oUsage.CommunityID, DetailViewType.Month, StatisticView.MySystem, View.CurrentUrl)
            ' Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Month)
            If oUsage.UserID > 0 Then
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Personal)
            Else
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Portal)
            End If

            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetPortalStatistics(View.PortalName, oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)
            Me.View.Pager = oPager
            For Each o In oStatistic.Items
                o.Permission = GetPermissions(o.ID)

                If (o.Subscribed AndAlso oUsage.UserID > 0) OrElse (Not o.Subscribed AndAlso (o.Permission.ListSelfStatistic OrElse o.Permission.Administration)) Then
                    ' o.NavigateTo = RootObject.MyCommunityStatistics(oUsage.CurrentPage, oUsage.UserID, o.ID, oUsage.Order, oUsage.Ascending, StatisticView.MySystem)
                    o.NavigateTo = RootObject.UserSystemCommunityStatistics(0, oUsage.UserID, o.ID, StatisticOrder.ModuleName, True, "", StatisticView.MySystem)
                End If
                'Dim oContext As New UsageContext() With {.UserID = oUsage.UserID, .CommunityID = o.ID, .Order = StatisticOrder.UsageTime, .Ascending = False, .ModuleID = -2, .CurrentPage = 0}
                'o.NavigateTo = Me.View.GetNavigationUrl(ViewPage.Community, oContext, IviewUsageStatistic.viewType.PersonalCommunity, View.CurrentView)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
            View.SendAction(0, ModuleID, ModuleStatistics.ActionType.LoadPortalMyPersonalStatistics)
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
            oContext.UserID = Me.AppContext.UserContext.CurrentUser.Id
            oContext.CommunityID = -1
            oContext.ModuleID = -2
            Return oContext
        End Function

        'Public Function GetUrlForTab(ByVal value As IviewUsageStatistic.viewType, ByVal ReturnTo As IviewUsageStatistic.viewType) As String
        '    Dim url = ""
        '    Dim oContext As New UsageContext With {.Ascending = False, .Order = StatisticOrder.UsageTime, .CurrentPage = 0}
        '    Dim startFrom As IviewUsageStatistic.viewType = View.StartFrom

        '    Select Case value
        '        Case IviewUsageStatistic.viewType.None

        '        Case IviewUsageStatistic.viewType.Personal
        '            oContext.UserID = Me.UserContext.CurrentUser.Id
        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, value)
        '        Case IviewUsageStatistic.viewType.SystemUsers
        '            oContext.UserID = 0
        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, value)

        '        Case IviewUsageStatistic.viewType.CommunityUsers
        '            If Me.StatContext.CommunityID > 0 Then
        '                oContext.CommunityID = Me.StatContext.CommunityID
        '            Else
        '                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.UserID = 0
        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None OrElse startFrom = IviewUsageStatistic.viewType.PersonalCommunity OrElse startFrom = IviewUsageStatistic.viewType.GenericCommunity, value, View.StartFrom))

        '        Case IviewUsageStatistic.viewType.GenericUser
        '            'oContext.CommunityID = Me.StatContext.CommunityID
        '            If Me.StatContext.CommunityID > 0 Then
        '                oContext.CommunityID = Me.StatContext.CommunityID
        '            Else
        '                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.UserID = Me.StatContext.UserID
        '            If oContext.CommunityID <= 0 Then
        '                oContext.Order = StatisticOrder.UsageTime
        '            Else
        '                oContext.Order = StatisticOrder.Community
        '            End If

        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None, value, View.StartFrom))
        '        Case IviewUsageStatistic.viewType.PersonalCommunity
        '            If Me.StatContext.CommunityID > 0 Then
        '                oContext.CommunityID = Me.StatContext.CommunityID
        '            Else
        '                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.UserID = Me.StatContext.UserID
        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None OrElse startFrom = IviewUsageStatistic.viewType.GenericCommunity OrElse startFrom = IviewUsageStatistic.viewType.CommunityUsers, value, View.StartFrom))

        '        Case IviewUsageStatistic.viewType.GenericSystem
        '            oContext.UserID = 0
        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, value)

        '        Case IviewUsageStatistic.viewType.GenericCommunity
        '            'oContext.CommunityID = Me.StatContext.CommunityID
        '            If Me.StatContext.CommunityID > 0 Then
        '                oContext.CommunityID = Me.StatContext.CommunityID
        '            Else
        '                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.UserID = 0
        '            url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None OrElse startFrom = IviewUsageStatistic.viewType.PersonalCommunity OrElse startFrom = IviewUsageStatistic.viewType.CommunityUsers, value, View.StartFrom))
        '        Case IviewUsageStatistic.viewType.UserOnLine
        '            oContext.CommunityID = -1
        '            oContext.UserID = 0
        '            oContext.Order = StatisticOrder.LastAction
        '            oContext.Ascending = False
        '            url = Me.View.GetNavigationUrl(ViewPage.OnLineUsers, oContext, value, ReturnTo, IIf(View.StartFrom = IviewUsageStatistic.viewType.None, value, View.StartFrom))
        '        Case IviewUsageStatistic.viewType.CommunityUserOnLine
        '            If Me.StatContext.CommunityID > 0 Then
        '                oContext.CommunityID = Me.StatContext.CommunityID
        '            Else
        '                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
        '            End If
        '            oContext.UserID = 0
        '            oContext.Order = StatisticOrder.LastAction
        '            oContext.Ascending = False
        '            url = Me.View.GetNavigationUrl(ViewPage.OnLineUsers, oContext, value, ReturnTo, IIf(View.StartFrom = IviewUsageStatistic.viewType.None, value, View.StartFrom))
        '        Case Else
        '            url = ""
        '    End Select
        '    Return url
        'End Function
    End Class
End Namespace