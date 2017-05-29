﻿Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class StatisticDetailsPresenter
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
        Public Overloads ReadOnly Property View() As IViewStatisticDetails
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewStatisticDetails)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim urlContext As UsageContext = View.StatisticContext
                Dim contextPermission As ModuleStatistics = GetPermissions(urlContext.CommunityID)
                Dim currentPermission As ModuleStatistics = GetPermissions(UserContext.CurrentCommunityID)

                LoadAvailableView(urlContext, GetAvailableViews())
                View.StatisticContext = urlContext
                If HasPermission(contextPermission, urlContext) OrElse HasPermission(currentPermission, urlContext) Then
                    Dim cView As DetailViewType = View.CurrentView
                    DisplayInfo(urlContext)
                    DisplayView(urlContext, cView, View.CurrentStartDate(cView), View.CurrentEndDate(cView))
                Else
                    View.SendAction(UserContext.CurrentCommunityID, UserContext.CurrentUserID, ModuleStatistics.ActionType.NoPermission)
                    Me.View.DisplayNoPermission()
                End If
                View.BackUrl = View.PreLoadedBackUrl
                'Dim context As UsageContext = View.StatisticContext
                'Dim idCommunity As Integer = UserContext.CurrentCommunityID
                'Dim currentModule As ModuleStatistics = GetModule(idCommunity)
                'Dim contextModule As ModuleStatistics = GetModule(context.CommunityID)

                'SetupViews(context)
                'If HasPermission(currentModule, context) OrElse HasPermission(contextModule, context) Then
                '    Dim cView As IViewUsageDetails.viewType = View.CurrentView
                '    DisplayInfo(context)
                '    DisplayView(context, cView, View.CurrentStartDate(cView), View.CurrentEndDate(cView))
                'Else
                '    View.DisplayNoPermission()
                'End If
                'View.BackUrl = View.PreLoadedBackUrl
            Else
                View.DisplaySessionTimeout()
            End If
        End Sub

        Private Sub DisplayInfo(context As UsageContext)
            Dim user As Person = BaseDomainManager.GetPerson(IIf(context.UserID > 0, context.UserID, IIf(context.UserIDList.Count = 0, UserContext.CurrentUserID, 0)))
            Dim userName As String = ""
            Dim communityname As String = ""
            If Not IsNothing(user) AndAlso context.UserID <> UserContext.CurrentUserID Then
                userName = user.SurnameAndName
            End If
            Dim community As Community = BaseDomainManager.GetCommunity(context.CommunityID)
            If Not IsNothing(community) Then
                communityname = community.Name
            End If
            Dim moduleName As String = CurrentManager.GetTralnsatedModules().Where(Function(m) m.ID = context.ModuleID).Select(Function(m) m.Name).FirstOrDefault()
            If (String.IsNullOrEmpty(communityname)) AndAlso String.IsNullOrEmpty(userName) Then
                Me.View.DisplayInfo(moduleName)
            ElseIf String.IsNullOrEmpty(userName) Then
                Me.View.DisplayInfo(moduleName, communityname)
            ElseIf String.IsNullOrEmpty(communityname) Then
                View.DisplayUserInfo(moduleName, userName)
            Else
                Me.View.DisplayInfo(moduleName, communityname, userName)
            End If
        End Sub

        Private Function HasPermission(moduleP As ModuleStatistics, context As UsageContext) As Boolean
            Return (moduleP.Administration OrElse moduleP.ListOtherStatistic OrElse (moduleP.ListSelfStatistic AndAlso context.GroupBy = UsageContext.grouping.None AndAlso (context.UserID = UserContext.CurrentUserID OrElse context.UserID = 0)))
        End Function

        Public Sub DisplayView(context As UsageContext, ByVal cView As DetailViewType)
            DisplayView(context, cView, View.CurrentStartDate(cView), View.CurrentEndDate(cView))
        End Sub
        Public Sub DisplayView(context As UsageContext, ByVal cView As DetailViewType, startDate As DateTime, endDate As DateTime)
            Me.View.CurrentView = cView
            View.LoadStatistics(GetStatistics(context, cView, startDate, endDate), cView)

  
            View.SendAction(UserContext.CurrentCommunityID, UserContext.CurrentUserID, context.UserID, context.CommunityID, ModuleStatistics.ActionType.LoadModuleTimeStatistics)
        End Sub
        Public Sub ReloadItems(context As UsageContext, ByVal cView As DetailViewType, startDate As DateTime, endDate As DateTime)
            View.LoadStatistics(GetStatistics(context, cView, startDate, endDate), cView)
            View.SendAction(UserContext.CurrentCommunityID, UserContext.CurrentUserID, context.UserID, context.CommunityID, ModuleStatistics.ActionType.LoadModuleTimeStatistics)
        End Sub
        Private Function GetStatistics(context As UsageContext, cView As DetailViewType, startDate As DateTime, endDate As DateTime) As List(Of dtoDetailUsageStatistic)
            Dim results As List(Of dtoDetailUsageStatistic) = GetStatisticsByView(cView, startDate, endDate)
            Dim statistics As New List(Of dtoDetailsTime)
            Dim itemsToPopulate As New List(Of dtoDetailUsageStatistic)
            Select Case context.GroupBy
                'Case UsageContext.grouping.CommunityID
                '    If cView = IViewUsageDetails.viewType.Day Then
                '        statistics = Me.CurrentManager.GetStatCommunity_hourly(context.UserID, context.CommunityID, context.ModuleID, Me.View.StartDate)
                '    Else
                '        statistics = Me.CurrentManager.GetStatCommunity_daily(context.UserID, context.CommunityID, context.ModuleID, Me.View.StartDate, Me.View.EndDate)
                '    End If
                'Case UsageContext.grouping.ModuleID
                '    If cView = IViewUsageDetails.viewType.Day Then
                '        statistics = Me.CurrentManager.GetStatModule_hourly(context.UserID, context.CommunityID, context.ModuleID, Me.View.StartDate)
                '    Else
                '        statistics = Me.CurrentManager.GetStatModule_daily(context.UserID, context.CommunityID, context.ModuleID, Me.View.StartDate, Me.View.EndDate)
                '    End If
                'Case UsageContext.grouping.UserID
                '    If cView = IViewUsageDetails.viewType.Day Then
                '        statistics = Me.CurrentManager.GetStatPerson_hourly(context.UserID, context.CommunityID, context.ModuleID, Me.View.StartDate)
                '    Else
                '        statistics = Me.CurrentManager.GetStatPerson_daily(context.UserID, context.CommunityID, context.ModuleID, Me.View.StartDate, Me.View.EndDate)
                '    End If
                Case Else 'si assume che quando non definito equivalga a "none" e cioe' si visualizzano gli accessi complessivi
                    If cView = IViewUsageDetails.viewType.Day Then
                        statistics = DALuserActions.getHourlyAccess(context.UserID, context.CommunityID, context.ModuleID, startDate, endDate)
                        itemsToPopulate = results.Where(Function(r) statistics.Select(Function(s) s.ToDateHour().Ticks).Contains(r.Id)).ToList()
                    Else
                        statistics = DALuserActions.GetDailyAccessTime(context.UserID, context.CommunityID, context.ModuleID, startDate, endDate)
                        itemsToPopulate = results.Where(Function(r) statistics.Select(Function(s) DateSerial(s.Year, s.Month, s.Day).Ticks).Contains(r.Id)).ToList()
                    End If
            End Select
            itemsToPopulate = results.Where(Function(r) statistics.Select(Function(s) s.ToDate(cView = DetailViewType.Day).Ticks).Contains(r.Id)).ToList()
            For Each item As dtoDetailUsageStatistic In itemsToPopulate
                Dim ticks As Long = item.Id
                item.Details = statistics.Where(Function(s) s.ToDate(cView = DetailViewType.Day).Ticks = ticks).ToList()
            Next
            Return results
        End Function
        Private Function GetStatisticsByView(view As DetailViewType, startDate As DateTime, endDate As DateTime) As List(Of dtoDetailUsageStatistic)
            Dim items As New List(Of dtoDetailUsageStatistic)
            Select Case view
                Case DetailViewType.Day
                    items.AddRange((From i As Integer In (From n In Enumerable.Range(0, 24).ToList) Select New dtoDetailUsageStatistic() With {.Id = startDate.AddHours(i).Ticks, .DisplayName = IIf(i < 10, "0" & i.ToString & ":00", i.ToString & ":00")}))
                Case DetailViewType.Week
                    items.AddRange((From i As Integer In (From n In Enumerable.Range(0, 7).ToList) Select New dtoDetailUsageStatistic() With {.Id = startDate.AddDays(i).Ticks, .DisplayName = WeekdayName(IIf(startDate.AddDays(i).DayOfWeek = 0, 7, startDate.AddDays(i).DayOfWeek), True, FirstDayOfWeek.Monday) & " " & FormatDateTime(startDate.AddDays(i), DateFormat.ShortDate).Replace("/" & startDate.AddDays(i).Year, "")}))
                Case DetailViewType.Month
                    items.AddRange((From i As Integer In (From n In Enumerable.Range(0, (endDate.Day)).ToList) Select New dtoDetailUsageStatistic() With {.Id = startDate.AddDays(i).Ticks, .DisplayName = WeekdayName(IIf(startDate.AddDays(i).DayOfWeek = 0, 7, startDate.AddDays(i).DayOfWeek), True, FirstDayOfWeek.Monday) & " " & FormatDateTime(startDate.AddDays(i), DateFormat.ShortDate).Replace("/" & startDate.AddDays(i).Year, "")}))
            End Select
            Return items
        End Function
        Private Sub LoadAvailableView(context As UsageContext, views As List(Of DetailViewType))
            Dim defaultView As DetailViewType = DetailViewType.Day
            If views.Contains(View.PreLoadedView) Then
                defaultView = Me.View.PreLoadedView
            ElseIf views.Contains(DetailViewType.Month) Then
                defaultView = DetailViewType.Month
            ElseIf views.Count > 0 Then
                defaultView = views(0)
            End If
            Me.View.CurrentView = defaultView

            Dim today As DateTime = DateTime.Now.Date
            Dim lastDay As DateTime = today
            Dim startDate, endDate As DateTime
            Select Case defaultView
                Case DetailViewType.Day
                    startDate = today
                    endDate = startDate
                Case DetailViewType.Week
                    startDate = DateAdd("d", 1 - today.DayOfWeek, today)
                    endDate = DateAdd("d", 7 - today.DayOfWeek, today)
                Case DetailViewType.Month
                    startDate = DateSerial(today.Year, today.Month, 1)
                    endDate = DateSerial(today.Year, today.Month, 1).AddMonths(1).AddDays(-1)
                Case DetailViewType.Year
                    startDate = DateSerial(today.Year, 1, 1)
                    endDate = DateSerial(today.Year, 21, 1)
            End Select

            Dim items As List(Of dtoYearItem) = DALuserActions.getAvailableYearsMonth(context.UserID, context.CommunityID, context.ModuleID)
            If Not items.Where(Function(y) y.Value = today.Year).Any Then
                items.Insert(0, New dtoYearItem(today.Year, today.Month, today.Day))
            End If
            View.LoadAvailableYears(items)
            View.CurrentStartDate(defaultView) = startDate
        End Sub
        Private Function GetAvailableViews() As List(Of DetailViewType)
            Dim views As New List(Of DetailViewType)
            views.Add(DetailViewType.Day)
            views.Add(DetailViewType.Week)
            views.Add(DetailViewType.Month)
            '  oList.Add(IViewUsageDetails.viewType.Year)
            Return views
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
    End Class
End Namespace