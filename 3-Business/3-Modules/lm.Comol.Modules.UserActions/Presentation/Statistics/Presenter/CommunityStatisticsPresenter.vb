Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class CommunityStatisticsPresenter
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
        Public Overloads ReadOnly Property View() As IViewCommunityStatistics
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityStatistics)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim oContext As UsageContext = GetDefaultContext()
                Dim user As Person = BaseDomainManager.GetPerson(Me.AppContext.UserContext.CurrentUserID)

                If IsNothing(user) Then
                    View.DisplaySessionTimeout()
                Else
                    Dim p As ModuleStatistics = Me.GetPermissions(oContext.CommunityID, user.Id)

                    View.StatisticContext = oContext
                    Dim items As List(Of StatisticView) = GetAvailableViews(p)

                    View.LoadAvailableView(items)
                    If items.Count = 0 Then
                        Me.View.NoPermissionToAccess()
                        View.SendAction(oContext.CommunityID, ModuleID, ModuleStatistics.ActionType.NoPermission)
                    ElseIf items.Contains(StatisticView.Community) Then
                        LoadCommunityUser(oContext, p)
                    ElseIf Me.View.PreloadedFromView = StatisticView.System Then
                        View.LoadView(RootObject.SystemStatistics(0, "", StatisticOrder.UsageTime, False))
                    Else
                        Select Case items.FirstOrDefault()
                            Case StatisticView.UsersCommunity
                                View.LoadView(RootObject.UsersCommunity(0, oContext.CommunityID, "", StatisticOrder.Owner, True, View.PreloadedFromView))
                            Case Else
                                View.LoadView(RootObject.MyCommunityStatistics(0, oContext.CommunityID, StatisticOrder.ModuleName, True, View.PreloadedFromView))
                        End Select
                    End If
                End If
                If View.PreloadedFromView = StatisticView.System Then
                    View.SetBackUrl(RootObject.SystemStatistics(0, "", StatisticOrder.UsageTime, False))
                End If

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
                o.NavigateTo = RootObject.UsageDetails(o.ID, oUsage.UserID, oUsage.CommunityID, DetailViewType.Week, StatisticView.MyCommunity, View.CurrentUrl)

                ' Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oContext, Me.View.CurrentView, IViewUsageDetails.viewType.Week)
                ' Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, IviewUsageStatistic.viewType.GenericUser, IviewUsageStatistic.viewType.CommunityUsers)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
            View.SendAction(UserContext.CurrentCommunityID, UserContext.CurrentUserID, oUsage.CommunityID, ModuleStatistics.ObjectType.Community, ModuleStatistics.ActionType.LoadCommunityStatistics)
        End Sub

        Private Function GetAvailableViews(p As ModuleStatistics) As List(Of StatisticView)
            Dim items As New List(Of StatisticView)

            If p.ListSelfStatistic OrElse p.Administration Then
                items.Add(StatisticView.MyCommunity)
            End If
            If p.ListOtherStatistic OrElse p.Administration Then
                items.Add(StatisticView.UsersCommunity)
                items.Add(StatisticView.Community)
            End If
            Return items
        End Function
        Public Function GetPermissions(idCommunity As Integer, idUser As Integer) As ModuleStatistics
            Dim moduleP As New ModuleStatistics
            If View.PreloadedFromView = StatisticView.System Then
                moduleP = ModuleStatistics.CreatePortalmodule(UserContext.UserTypeID)
            End If
            If idCommunity > 0 Then
                Dim idRole As Integer = BaseDomainManager.GetDefaultIdRole(idCommunity)
                Dim IsForHistory As Boolean = Not BaseDomainManager.HasActiveSubscription(idUser, idCommunity)

                Dim moduleC As ModuleStatistics = Nothing
                If IsForHistory Then
                    moduleC = New ModuleStatistics(BaseDomainManager.GetModulePermissionByRole(idRole, idCommunity, ModuleID))
                Else
                    moduleC = New ModuleStatistics(BaseDomainManager.GetModulePermission(idUser, idCommunity, ModuleID))
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
            oContext.UserID = -1
            If oContext.CommunityID < 1 Then
                oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
            End If

            Return oContext
        End Function

    End Class
End Namespace