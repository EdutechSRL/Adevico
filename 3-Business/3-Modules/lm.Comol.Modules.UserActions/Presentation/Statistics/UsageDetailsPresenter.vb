Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports COL_BusinessLogic_v2
Imports WSstatistics
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class UsageDetailsPresenter
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
        'private int ModuleID
        '{
        '    get
        '    {
        '        if (_ModuleID <= 0)
        '        {
        '            _ModuleID = this.Service.ServiceModuleID();
        '        }
        '        return _ModuleID;
        '    }
        '}

        Private _BaseDomainManager As BaseModuleManager
        Public Property BaseDomainManager() As BaseModuleManager
            Get
                Return _BaseDomainManager
            End Get
            Set(value As BaseModuleManager)
                _BaseDomainManager = value
            End Set
        End Property
        Private m_CurrentManager As BaseModuleManager
        Public Overloads Property CurrentManager() As ManagerUsageDetails
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerUsageDetails)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewUsageDetails
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageDetails(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewUsageDetails)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageDetails(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        'Private Function GetNameFromList(ByVal id) As String
        '    Dim retval As String
        '    Select Case Me.View.StatisticContext.GroupBy
        '        Case UsageContext.grouping.CommunityID
        '            Dim oCommunityList As New List(Of Community)
        '            oCommunityList = Me.CurrentManager.GetCommunities()
        '            Try
        '                retval = ((From o In oCommunityList Where o.Id = id Select o.Name).ToList).Item(0)
        '            Catch ex As Exception
        '                retval = "X"
        '            End Try
        '        Case UsageContext.grouping.ModuleID
        '            Dim oModuleList As New List(Of COL_BusinessLogic_v2.PlainService)
        '            oModuleList = Me.CurrentManager.GetModules
        '            Try
        '                retval = ((From o In oModuleList Where o.ID = id Select o.Name).ToList).Item(0)
        '            Catch ex As Exception
        '                retval = "X"
        '            End Try
        '        Case UsageContext.grouping.UserID
        '            'carico la lista degli utenti
        '            Dim oUserList As New List(Of iPerson)
        '            oUserList = Me.CurrentManager.GetUsers
        '            Try
        '                retval = ((From o In oUserList Where o.Id = id Select o.SurnameAndName).ToList).Item(0)
        '            Catch ex As Exception
        '                retval = "X"
        '            End Try
        '        Case Else
        '            retval = String.Empty
        '            'si assume che quando non definito equivalga a "none" e cioe' si visualizzano gli accessi complessivi
        '    End Select
        '    Return retval
        'End Function
       

        Public Sub InitView()
            If UserContext.isAnonymous Then
                View.DisplaySessionTimeout()
            Else
                Dim context As UsageContext = View.StatisticContext
                Dim idCommunity As Integer = UserContext.CurrentCommunityID
                Dim currentModule As ModuleStatistics = GetModule(idCommunity)
                Dim contextModule As ModuleStatistics = GetModule(context.CommunityID)

                SetupViews(context)
                If HasPermission(currentModule, context) OrElse HasPermission(contextModule, context) Then
                    Dim cView As IViewUsageDetails.viewType = View.CurrentView
                    DisplayInfo(context)
                    DisplayView(context, cView, View.CurrentStartDate(cView), View.CurrentEndDate(cView))
                Else
                    View.DisplayNoPermission()
                End If
                View.BackUrl = View.PreLoadedBackUrl
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
            Dim moduleName As String = CurrentManager.GetModules().Where(Function(m) m.ID = context.ModuleID).Select(Function(m) m.Name).FirstOrDefault()
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

        Private Function GetModule(idCommunity As Integer) As ModuleStatistics
            Dim moduleP As ModuleStatistics
            If idCommunity = 0 Then
                moduleP = ModuleStatistics.CreatePortalmodule(UserContext.UserTypeID)
            Else
                moduleP = New ModuleStatistics(BaseDomainManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID))
            End If
            Return moduleP
        End Function
        Private Function HasPermission(moduleP As ModuleStatistics, context As UsageContext) As Boolean
            Return (moduleP.Administration OrElse moduleP.ListOtherStatistic OrElse (moduleP.ListSelfStatistic AndAlso context.GroupBy = UsageContext.grouping.None AndAlso (context.UserID = UserContext.CurrentUserID OrElse context.UserID = 0)))
        End Function
     
        Public Sub DisplayView(context As UsageContext, ByVal cView As IViewUsageDetails.viewType)

            DisplayView(context, cView, View.CurrentStartDate(cView), View.CurrentEndDate(cView))
        End Sub
        Public Sub DisplayView(context As UsageContext, ByVal cView As IViewUsageDetails.viewType, startDate As DateTime, endDate As DateTime)
            Me.View.CurrentView = cView
            View.LoadStatistics(GetStatistics(context, cView, startDate, endDate), cView)
        End Sub
        Public Sub ReloadItems(context As UsageContext, ByVal cView As IViewUsageDetails.viewType, startDate As DateTime, endDate As DateTime)
            View.LoadStatistics(GetStatistics(context, cView, startDate, endDate), cView)
        End Sub
        Private Function GetStatistics(context As UsageContext, cView As IViewUsageDetails.viewType, startDate As DateTime, endDate As DateTime) As List(Of dtoDetailUsageStatistic)
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
            itemsToPopulate = results.Where(Function(r) statistics.Select(Function(s) s.ToDate(cView = IViewUsageDetails.viewType.Day).Ticks).Contains(r.Id)).ToList()
            For Each item As dtoDetailUsageStatistic In itemsToPopulate
                Dim ticks As Long = item.Id
                item.Details = statistics.Where(Function(s) s.ToDate(cView = IViewUsageDetails.viewType.Day).Ticks = ticks).ToList()
            Next
            Return results
        End Function

        Private Function GetStatisticsByView(view As IViewUsageDetails.viewType, startDate As DateTime, endDate As DateTime) As List(Of dtoDetailUsageStatistic)
            Dim items As New List(Of dtoDetailUsageStatistic)
            Select Case view
                Case IViewUsageDetails.viewType.Day
                    items.AddRange((From i As Integer In (From n In Enumerable.Range(0, 24).ToList) Select New dtoDetailUsageStatistic() With {.Id = startDate.AddHours(i).Ticks, .DisplayName = IIf(i < 10, "0" & i.ToString & ":00", i.ToString & ":00")}))
                Case IViewUsageDetails.viewType.Week
                    items.AddRange((From i As Integer In (From n In Enumerable.Range(0, 7).ToList) Select New dtoDetailUsageStatistic() With {.Id = startDate.AddDays(i).Ticks, .DisplayName = WeekdayName(IIf(startDate.AddDays(i).DayOfWeek = 0, 7, startDate.AddDays(i).DayOfWeek), True, FirstDayOfWeek.Monday) & " " & FormatDateTime(startDate.AddDays(i), DateFormat.ShortDate).Replace("/" & startDate.AddDays(i).Year, "")}))
                Case IViewUsageDetails.viewType.Month
                    items.AddRange((From i As Integer In (From n In Enumerable.Range(0, (endDate.Day)).ToList) Select New dtoDetailUsageStatistic() With {.Id = startDate.AddDays(i).Ticks, .DisplayName = WeekdayName(IIf(startDate.AddDays(i).DayOfWeek = 0, 7, startDate.AddDays(i).DayOfWeek), True, FirstDayOfWeek.Monday) & " " & FormatDateTime(startDate.AddDays(i), DateFormat.ShortDate).Replace("/" & startDate.AddDays(i).Year, "")}))
            End Select

            Return items
        End Function
        Private Sub SetupViews(context As UsageContext)
            Dim defaultView As IViewUsageDetails.viewType = IViewUsageDetails.viewType.Day
            Dim views As List(Of IViewUsageDetails.viewType) = GetAvailableViews()
            View.LoadAvailableViews(views)
            If views.Contains(View.PreLoadedView) Then
                defaultView = Me.View.PreLoadedView
            ElseIf views.Contains(IViewUsageDetails.viewType.Month) Then
                defaultView = IViewUsageDetails.viewType.Month
            ElseIf views.Count > 0 Then
                defaultView = views(0)
            End If
            Me.View.CurrentView = defaultView

            Dim today As DateTime = DateTime.Now.Date
            Dim lastDay As DateTime = today
            Dim startDate, endDate As DateTime
            Select Case defaultView
                Case IViewUsageDetails.viewType.Day
                    startDate = today
                    endDate = startDate
                Case (IViewUsageDetails.viewType.Week)
                    startDate = DateAdd("d", 1 - today.DayOfWeek, today)
                    endDate = DateAdd("d", 7 - today.DayOfWeek, today)
                Case IViewUsageDetails.viewType.Month
                    startDate = DateSerial(today.Year, today.Month, 1)
                    endDate = DateSerial(today.Year, today.Month, 1).AddMonths(1).AddDays(-1)
                Case IViewUsageDetails.viewType.Year
                    startDate = DateSerial(today.Year, 1, 1)
                    endDate = DateSerial(today.Year, 21, 1)
            End Select

            View.LoadAvailableYears(DALuserActions.getAvailableYears(context.UserID, context.CommunityID, context.ModuleID))
            View.CurrentStartDate(defaultView) = startDate
            '
        End Sub
        Private Function GetAvailableViews() As List(Of IViewUsageDetails.viewType)
            Dim views As New List(Of IViewUsageDetails.viewType)

            views.Add(IViewUsageDetails.viewType.Day)
            views.Add(IViewUsageDetails.viewType.Week)
            views.Add(IViewUsageDetails.viewType.Month)
            '  oList.Add(IViewUsageDetails.viewType.Year)

            Return views
        End Function
    End Class
End Namespace