Imports lm.Comol.Core.DomainModel.Common
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Business
Imports lm.Modules.NotificationSystem.WSremoteManagement
Imports COL_BusinessLogic_v2.UCServices.Services_CommunityNews

Namespace Presentation
    Public Class CommunityNewsPresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        Private _Permission As ModuleCommunityNews
        Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews))
        Private ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As ModuleCommunityNews
            Get
                If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
                    _Permission = Me.View.ModulePermission
                    Return _Permission
                ElseIf CommunityID > 0 Then
                    Return (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                Else
                    Return _Permission
                End If
            End Get
        End Property
        Private ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews))
            Get
                If IsNothing(_CommunitiesPermission) Then
                    _CommunitiesPermission = Me.View.CommunitiesPermission()
                End If
                Return _CommunitiesPermission
            End Get
        End Property
#End Region

        Private _CommunityNewsContext As CommunityNewsContext
        Private ReadOnly Property ViewContext() As CommunityNewsContext
            Get
                If IsNothing(_CommunityNewsContext) Then
                    Dim oContext As CommunityNewsContext = Me.View.NewsContext
                    Dim PersonID As Integer = oContext.UserID
                    '  If PersonID = 0 Then
                    PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    '  End If
                    oContext.UserID = PersonID
                    Dim CommunityID As Integer = oContext.CommunityID
                    If oContext.CurrentView = ViewModeType.None AndAlso CommunityID = 0 Then
                        oContext.CurrentView = ViewModeType.Portal
                    ElseIf oContext.CurrentView = ViewModeType.None Then
                        oContext.CurrentView = ViewModeType.CurrentCommunity
                    End If
                    If oContext.DayView = DayModeType.None Then
                        oContext.DayView = DayModeType.TodayYesterday
                        If oContext.CurrentDay.Equals(New Date) Then
                            oContext.CurrentDay = Now.Date
                        End If
                    End If
                    If CommunityID <= 0 AndAlso oContext.CurrentView = ViewModeType.CurrentCommunity Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    
                    Me.View.NewsContext = oContext
                    _CommunityNewsContext = oContext
                End If
                Return _CommunityNewsContext
            End Get
        End Property

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerCommunitynews
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerCommunitynews)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewCommunityNews
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunitynews(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityNews)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunitynews(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Me.LoadTabs()
                If ViewContext.CurrentView = ViewModeType.Portal AndAlso ViewContext.DayView <> DayModeType.AllNews Then
                    LoadSummaries(False)
                ElseIf ViewContext.DayView <> DayModeType.AllNews Then 'If ViewContext.CurrentView = ViewModeType.CurrentCommunity Then
                    LoadCommunityNews()
                Else
                    LoadAllNews(Not (ViewContext.CurrentView = ViewModeType.Portal))
                End If

                If Me.ViewContext.FromView = ViewModeType.None Then
                    Me.View.SetPreviousURL = ""
                Else
                    Dim oContext As CommunityNewsContext = Me.ViewContext
                    If oContext.FromView = ViewModeType.Portal Then
                        oContext.CommunityID = -1
                    End If
                    oContext.PageIndex = 0
                    oContext.CurrentView = oContext.FromView
                    oContext.FromView = ViewModeType.None
                    Me.View.SetPreviousURL = Me.View.GetNavigationUrl(oContext)
                End If
            Else
                Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0, Me.View.PreLoadedDayMode, Me.View.PreLoadedDay)
                Me.View.NoPermissionToAccess()
            End If
        End Sub

        Private Sub LoadTabs()
            Dim oList As New List(Of dtoTab)
            oList.Add(New dtoTab() With {.TypeTab = DayModeType.TodayYesterday, .Selected = (ViewContext.DayView = DayModeType.TodayYesterday), .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, Nothing, DayModeType.TodayYesterday)})
            oList.Add(New dtoTab() With {.TypeTab = DayModeType.LastWeek, .Selected = (ViewContext.DayView = DayModeType.LastWeek), .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, Nothing, DayModeType.LastWeek)})
            oList.Add(New dtoTab() With {.TypeTab = DayModeType.LastMonth, .Selected = (ViewContext.DayView = DayModeType.LastMonth), .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, Nothing, DayModeType.LastMonth)})
            oList.Add(New dtoTab() With {.TypeTab = DayModeType.AllNews, .Selected = (ViewContext.DayView = DayModeType.AllNews), .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, Nothing, DayModeType.AllNews)})

            Me.View.LoadTabs(oList)

            If ViewContext.DayView <> DayModeType.AllNews Then
                Dim oDays As New List(Of dtoDay)
                Dim oToday As Date = Now.Date
                Select Case ViewContext.DayView
                    Case DayModeType.LastMonth
                        oDays = Me.CurrentManager.GetMonthDayNews(ViewContext.UserID, ViewContext.CommunityID)

                    Case DayModeType.LastWeek
                        oDays = Me.CurrentManager.GetWeekDayNewsFromNowToPrevoius(ViewContext.UserID, ViewContext.CommunityID, Me.View.ToDayTranslated)
                    Case Else
                        Dim oDtoToday As New dtoDay(oToday)
                        Dim oDtoYesterday As New dtoDay(oToday.AddDays(-1))
                        oDtoToday.Enabled = True
                        oDtoYesterday.Enabled = True
                        oDtoToday.DayName = Me.View.ToDayTranslated
                        oDtoYesterday.DayName = Me.View.YesterdayTranslated
                        oDays.Add(oDtoToday)
                        oDays.Add(oDtoYesterday)
                End Select
                Dim oListDayTabs As New List(Of dtoTab)
                Select Case ViewContext.DayView
                    Case DayModeType.LastMonth
                        oListDayTabs = (From d In oDays Select New dtoTab() With {.Enabled = d.Enabled, .Day = d.Day, .TypeTab = DayModeType.LastMonth, .Name = d.DayNumber, .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, d.Day, DayModeType.LastMonth), .Month = d.Day.Month}).ToList
                        If oListDayTabs.Count > 0 Then
                            Dim FirstDay As dtoDay = oDays(0)
                            Dim LastDay As dtoDay = oDays.Last
                            Dim FirstMonth As New dtoTab
                            Dim LastMonth As New dtoTab
                            FirstMonth.Name = FirstDay.Month
                            FirstMonth.Enabled = False
                            FirstMonth.isType = True
                            FirstMonth.TypeTab = DayModeType.LastMonth
                            FirstMonth.Month = FirstDay.Day.Month
                            LastMonth.Enabled = False
                            LastMonth.Name = LastDay.Month
                            LastMonth.isType = True
                            LastMonth.TypeTab = DayModeType.LastMonth
                            LastMonth.Month = LastDay.Day.Month
                            oListDayTabs.Insert(0, FirstMonth)
                            If FirstMonth.Month <> LastMonth.Month Then
                                oListDayTabs.Insert(oListDayTabs.Count - LastDay.DayNumber, LastMonth)
                            End If
                        End If

                    Case DayModeType.LastWeek
                        oListDayTabs = (From d In oDays Select New dtoTab() With {.Enabled = d.Enabled, .Day = d.Day, .TypeTab = DayModeType.LastWeek, .Name = d.AbbreviateName & "<br>" & d.Day.ToString("dd/MM/yy"), .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, d.Day, DayModeType.LastWeek)}).ToList
                    Case Else
                        oListDayTabs = (From d In oDays Select New dtoTab() With {.Enabled = d.Enabled, .Day = d.Day, .TypeTab = DayModeType.TodayYesterday, .Name = d.DayName, .Url = Me.View.GetTabUrl(ViewContext.CommunityID, ViewContext.CurrentView, ViewContext.FromView, d.Day, DayModeType.TodayYesterday)}).ToList
                End Select
                Dim oCurrentDay As dtoTab
                oCurrentDay = (From d In oListDayTabs Where d.Day = ViewContext.CurrentDay).FirstOrDefault
                If IsNothing(oCurrentDay) Then
                    ViewContext.CurrentDay = oDays.Last.Day
                    If oListDayTabs.Count > 0 Then
                        oListDayTabs.Last.Selected = True
                    End If
                Else
                    oCurrentDay.Selected = True
                End If
                Me.View.LoadDays(oListDayTabs)
            Else
                Me.View.LoadDays(New List(Of dtoTab))
            End If
        End Sub

        Private Sub LoadSummaries(ByVal forCommunity As Boolean)
            Dim oCurrent As Date = ViewContext.CurrentDay
            If oCurrent.Equals(New Date) Then
                oCurrent = Now.Date
            End If
            Dim oList As New List(Of dtoCommunitySummaryNews)
            Dim DayName As String = ""
            If oCurrent.DayOfWeek = 0 Then
                DayName = WeekdayName(7, True)
            Else
                DayName = WeekdayName(oCurrent.DayOfWeek, True)
            End If
            DayName &= oCurrent.Date.ToShortDateString

            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPageIndex
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim CommunityID As Integer = -1
            If forCommunity Then
                CommunityID = ViewContext.CommunityID
            End If

            Dim oTmp As CommunityNewsContext = Me.ViewContext.Clone
            oTmp.PageIndex = Me.View.CurrentPageIndex
            oTmp.CommunityID = CommunityID
            Dim oDetailUrl As String = Me.View.GetUrlDetails(oTmp, True)
            oList = Me.CurrentManager.GetSummaryNews(oCurrent, ViewContext.UserID, CommunityID, UserContext.Language.Id, oPager, Me.View.CurrentPageIndex)

            If oDetailUrl <> "" Then
                oList = (From o In oList Select o.UpdateDetailUrl(String.Format(oDetailUrl, o.ID))).ToList
            End If

            Me.View.Pager = oPager
            Me.View.LoadNotificationSummary(DayName, oList)
            Me.View.NavigationUrl(ViewContext)

            Me.SendUserAction(oCurrent)

        End Sub

        Private Sub LoadCommunityNews()
            Dim oCurrent As Date = ViewContext.CurrentDay
            If oCurrent.Equals(New Date) Then
                oCurrent = Now.Date
            End If
            Dim oList As New List(Of dtoModuleNews)
            Dim DayName As String = ""
            If oCurrent.DayOfWeek = 0 Then
                DayName = WeekdayName(7, True)
            Else
                DayName = WeekdayName(oCurrent.DayOfWeek, True)
            End If
            DayName &= oCurrent.Date.ToShortDateString

            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPageIndex
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            oList = Me.CurrentManager.GetCommunityNews(oCurrent, ViewContext.UserID, ViewContext.CommunityID, UserContext.Language.Id, oPager, Me.View.CurrentPageIndex)

           
            Me.View.Pager = oPager
            Me.View.LoadNotifications(DayName, oList)
            Me.View.NavigationUrl(ViewContext)

            Me.SendUserAction(oCurrent)
        End Sub

        Private Sub LoadAllNews(ByVal forCommunity As Boolean)
            Dim CommunityID As Integer = -1
            If forCommunity Then
                CommunityID = ViewContext.CommunityID
            End If

            If Me.View.PreLoadedViewFilter = ViewModeFiler.None Then
                Me.View.CurrentViewFilter = ViewModeFiler.ByDate
            Else
                Me.View.CurrentViewFilter = Me.View.PreLoadedViewFilter
            End If
  
            Me.View.LoadServices(Me.CurrentManager.GetModulesWithNews(ViewContext.UserID, CommunityID, UserContext.Language.Id))
            Me.View.CurrentModuleID = Me.View.PreLoadedModuleID

            UpdateAllNews(forCommunity)

            'Dim oCurrent As Date = ViewContext.CurrentDay
            'If oCurrent.Equals(New Date) Then
            '    oCurrent = Now.Date
            'End If
            ''Dim oList As New List(Of dtoCommunitySummaryNews)
            ''Dim DayName As String = ""
            ''If oCurrent.DayOfWeek = 0 Then
            ''    DayName = WeekdayName(7, True)
            ''Else
            ''    DayName = WeekdayName(oCurrent.DayOfWeek, True)
            ''End If
            ''DayName &= oCurrent.Date.ToShortDateString

            'Dim oPager As New PagerBase
            'oPager.PageIndex = Me.View.CurrentPageIndex
            'oPager.PageSize = 5 'Me.View.CurrentPageSize
            'oPager.Count -= 1

            'Dim CommunityID As Integer = -1
            'If forCommunity Then
            '    CommunityID = ViewContext.CommunityID
            'End If
            'Me.View.CurrentViewFilter = ViewModeFiler.ByCommunity
            'Me.View.LoadServices(Me.CurrentManager.GetModulesWithNews(ViewContext.UserID, CommunityID, UserContext.Language.Id))

            'Dim oTmp As CommunityNewsContext = Me.ViewContext.Clone
            'oTmp.PageIndex = Me.View.CurrentPageIndex
            'oTmp.CommunityID = CommunityID

            'Dim oList As List(Of dtoMultipleNews)
            'Dim oDetailUrl As String = Me.View.GetUrlDetails(oTmp, True)

            'If Me.UserContext.CurrentCommunityID > 0 Then

            'End If
            'oList = Me.CurrentManager.GetPortalNews(Me.View.CurrentModuleID, Me.View.CurrentViewFilter, UserContext.CurrentUserID, CommunityID, UserContext.Language.Id, oPager, Me.View.CurrentPageIndex, "Home")

            ''oList = Me.CurrentManager.GetSummaryNews(oCurrent, ViewContext.UserID, CommunityID, UserContext.Language.Id, oPager, Me.View.CurrentPageIndex)

            ''If oDetailUrl <> "" Then
            ''    oList = (From o In oList Select o.UpdateDetailUrl(String.Format(oDetailUrl, o.ID))).ToList
            ''End If
            'Me.View.LoadAllNews(oList)
            'Me.View.Pager = oPager
            ''  Me.View.LoadNotificationSummary(DayName, oList)
            'Me.View.NavigationUrl(ViewContext)
        End Sub

        Public Sub UpdateAllNews(ByVal forCommunity As Boolean)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPageIndex
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim CommunityID As Integer = -1
            If forCommunity Then
                CommunityID = ViewContext.CommunityID
            End If



            Dim oTmp As CommunityNewsContext = Me.ViewContext.Clone
            oTmp.PageIndex = Me.View.CurrentPageIndex
            oTmp.CommunityID = CommunityID

            Dim oList As List(Of dtoMultipleNews)
            Dim oDetailUrl As String = Me.View.GetUrlDetails(oTmp, True)

            oList = Me.CurrentManager.GetPortalNews(DateTime.MinValue, Me.View.CurrentModuleID, Me.View.CurrentViewFilter, UserContext.CurrentUserID, CommunityID, UserContext.Language.Id, oPager, Me.View.CurrentPageIndex, "Home")

            Me.View.LoadAllNews(oList)
            
            Me.View.Pager = oPager
            Me.View.NavigationUrl(ViewContext)
            Me.View.AddAction(CommunityID, ViewContext.UserID, ActionType.ViewAllNews, Now.Date.AddDays(-30))
        End Sub

        Private Sub SendUserAction(ByVal oCurrent As Date)
            Select Case ViewContext.DayView
                Case DayModeType.AllNews
                    Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewAllNews, oCurrent)
                Case DayModeType.LastMonth
                    If Me.View.PreLoadedDaySpecifyed Then
                        Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewDayNews, oCurrent)
                    Else
                        Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewMonthNews, oCurrent)
                    End If
                Case DayModeType.LastWeek
                    If Me.View.PreLoadedDaySpecifyed Then
                        Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewDayNews, oCurrent)
                    Else
                        Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewWeekNews, oCurrent)
                    End If
                Case DayModeType.TodayYesterday
                    If oCurrent = Now.Date Then
                        Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewTodayNews, oCurrent)
                    Else
                        Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewYesterdayNews, oCurrent)
                    End If
            End Select
        End Sub

    End Class
End Namespace