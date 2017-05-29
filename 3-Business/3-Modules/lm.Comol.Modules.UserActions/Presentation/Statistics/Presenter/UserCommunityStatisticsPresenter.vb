Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class UserCommunityStatisticsPresenter
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
        Public Overloads ReadOnly Property View() As IViewUserCommunityStatistics
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUserCommunityStatistics)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim oContext As UsageContext = GetDefaultContext()
                Dim idType As Integer = Me.AppContext.UserContext.UserTypeID
                Dim idUser As Integer = Me.AppContext.UserContext.CurrentUserID
                Dim p As ModuleStatistics = Me.GetPermissions(oContext.CommunityID)
                View.StatisticContext = oContext
                If p.Administration OrElse (p.ListSelfStatistic AndAlso oContext.UserID = idUser) OrElse (p.ListOtherStatistic AndAlso oContext.UserID = idUser) Then
                    Dim user As Person = BaseDomainManager.GetPerson(oContext.UserID)
                    If IsNothing(user) Then
                        View.DisplayUnknownUser()
                        View.SendAction(UserContext.CurrentCommunityID, UserContext.CurrentUserID, oContext.UserID, ModuleStatistics.ObjectType.User, ModuleStatistics.ActionType.NoPermission)
                    Else
                        LoadCommunityUser(oContext, p)
                    End If
                Else
                    Me.View.NoPermissionToAccess()
                    View.SendAction(oContext.CommunityID, ModuleID, ModuleStatistics.ActionType.NoPermission)
                End If

                Dim url As String = ""
                Select Case View.PreloadedFromView
                    Case StatisticView.MySystem
                        url = RootObject.MySystemStatistics()
                    Case StatisticView.UserSystem
                        url = RootObject.UserSystemStatistics(0, oContext.UserID, oContext.SearchBy, StatisticOrder.UsageTime, False, View.GetEncodedBackUrl, StatisticView.UsersSystem)
                        'Case StatisticView.UsersCommunity
                        '    url = RootObject.UsersCommunity(0, oContext.CommunityID, oContext.SearchBy, StatisticOrder.Owner, True, View.GetEncodedBackUrl, StatisticView.UsersCommunity)
                        'Case 
                    Case Else
                        url = View.PreloadedBackUrl
                End Select
                View.SetBackUrl(url)
            Else
                View.DisplaySessionTimeout()
            End If
        End Sub
        Public Sub LoadCommunityUser(ByVal oUsage As UsageContext, p As ModuleStatistics)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummaryContext As UsageContext = oUsage.Clone
            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            oSummary.NavigateTo = RootObject.UsageDetails(oUsage.UserID, oUsage.CommunityID, DetailViewType.Month, StatisticView.UserCommunitySystem, View.CurrentUrl)
            'Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oSummaryContext, Me.View.CurrentView, IViewUsageDetails.viewType.Month)

            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetUserModuleStatistics(p, oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)
            If oUsage.UserID > 0 Then
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.PersonalCommunity)
            Else
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Community)
            End If

            Me.View.Pager = oPager
            For Each o In oStatistic.Items
                'Dim oContext As New UsageContext() With {.UserID = oUsage.UserID, .CommunityID = oUsage.CommunityID, .Order = StatisticOrder.ModuleName, .Ascending = True, .ModuleID = o.ID, .CurrentPage = -100}
                o.NavigateTo = RootObject.UsageDetails(o.ID, oUsage.UserID, oUsage.CommunityID, DetailViewType.Week, StatisticView.UserCommunitySystem, View.CurrentUrl)

                ' Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oContext, Me.View.CurrentView, IViewUsageDetails.viewType.Week)
                ' Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, IviewUsageStatistic.viewType.GenericUser, IviewUsageStatistic.viewType.CommunityUsers)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
            View.SendAction(UserContext.CurrentCommunityID, UserContext.CurrentUserID, oUsage.CommunityID, ModuleStatistics.ObjectType.Community, ModuleStatistics.ActionType.LoadCommunityUserStatistics)
        End Sub


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
                oContext.Order = StatisticOrder.ModuleName
                oContext.Ascending = True
            End If
            oContext.ModuleID = -2
            If oContext.UserID < 1 OrElse Me.View.PreloadedFromView = StatisticView.MySystem OrElse Me.View.PreloadedFromView = StatisticView.MyCommunity Then
                oContext.UserID = Me.AppContext.UserContext.CurrentUserID
            End If
            If oContext.CommunityID < 1 Then
                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
            End If
            Return oContext
        End Function

    End Class
End Namespace