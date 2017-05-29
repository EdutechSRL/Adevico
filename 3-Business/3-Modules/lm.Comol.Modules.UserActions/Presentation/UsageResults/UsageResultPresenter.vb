Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Modules.UsageResults.Presentation
Imports lm.Comol.Modules.UsageResults.BusinessLogic
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UsageResults.Presentation
    Public Class UsageResultPresenter
        Inherits DomainPresenter


#Region "Standard"
        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = BaseDomainManager.GetModuleID(ModuleUsageResult.UniqueId)
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
        Public Overloads Property CurrentManager() As ManagerUsageResults
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerUsageResults)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IviewUsageResults
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageResults(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewUsageResults)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageResults(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        Private _permissions As Dictionary(Of Integer, ModuleUsageResult)

        Private ReadOnly Property GetPermission(idCommunity As Integer) As ModuleUsageResult
            Get
                If IsNothing(_permissions) Then
                    _permissions = New Dictionary(Of Integer, ModuleUsageResult)
                End If
                If _permissions.ContainsKey(idCommunity) Then
                    Return _permissions(idCommunity)
                Else
                    Dim moduleP As ModuleUsageResult = GetModule(idCommunity)
                    _permissions.Add(idCommunity, moduleP)
                    Return moduleP
                End If
            End Get
        End Property

        Private Function GetModule(idCommunity As Integer) As ModuleUsageResult
            Dim moduleP As ModuleUsageResult
            If idCommunity = 0 Then
                moduleP = ModuleUsageResult.CreatePortalmodule(UserContext.UserTypeID)
            Else
                moduleP = New ModuleUsageResult(BaseDomainManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID))
            End If
            Return moduleP
        End Function



        Private _ContexFromView As ResultContext
        Private ReadOnly Property ResContext() As ResultContext
            Get
                If IsNothing(_ContexFromView) Then
                    Dim oContext As ResultContext = Me.View.ResultsContext
                    Dim PersonID As Integer = oContext.UserID
                    If PersonID = 0 AndAlso (Me.View.CurrentView = IviewUsageResults.viewType.MyPortalPresence OrElse Me.View.CurrentView = IviewUsageResults.viewType.CurrentCommunityPresence) Then
                        PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    If oContext.Order = ResultsOrder.None Then
                        oContext.Order = ResultsOrder.Day
                        oContext.Ascending = True
                    End If
                    If oContext.SubView = ViewDetailPage.None Then
                        oContext.SubView = Me.View.PreLoadedDetailView
                    End If
                    If Me.View.PreLoadedStartDate.HasValue Then
                        oContext.FromDate = Me.View.PreLoadedStartDate.Value
                    End If
                    If Me.View.PreLoadedEndDate.HasValue Then
                        oContext.ToDate = Me.View.PreLoadedEndDate.Value
                    End If
                    oContext.UserID = PersonID
                    Dim CommunityID As Integer = oContext.CommunityID
                    If CommunityID <= 0 AndAlso (Me.View.CurrentView = IviewUsageResults.viewType.CurrentCommunityPresence OrElse Me.View.CurrentView = IviewUsageResults.viewType.UsersCurrentCommunityPresence OrElse Me.View.CurrentView = IviewUsageResults.viewType.BetweenDateUsersCommunity) Then
                        CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.CommunityID = CommunityID
                    Me.View.ResultsContext = oContext
                    _ContexFromView = oContext
                End If
                Return _ContexFromView
            End Get
        End Property

        Private ReadOnly Property CurrentDateI() As Date
            Get
                If Me.View.SelectedStartDate.HasValue Then
                    Return Me.View.SelectedStartDate.Value
                Else
                    Return Now.Date
                End If
            End Get
        End Property
        Private ReadOnly Property CurrentDateF() As Date
            Get
                If Me.View.SelectedEndDate.HasValue Then
                    Return Me.View.SelectedEndDate.Value
                Else
                    Return Now.Date
                End If
            End Get
        End Property

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                If Me.SetupViews() Then
                    Me.ChangeView(Me.View.CurrentView)
                Else
                    Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0)
                    Me.View.NoPermissionToAccess()
                End If
            Else
                Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0)
                Me.View.NoPermissionToAccess()
            End If
        End Sub
        Public Sub ChangeView(ByVal current As IviewUsageResults.viewType)
            If Me.View.CurrentView <> current Then
                Me.View.CurrentView = current
            End If
            Me.View.SetPreviousURL = ""
            Me.View.AllowPrint = False
            Dim oContext As ResultContext = Me.ResContext.Clone
            Select Case current
                Case IviewUsageResults.viewType.MyPortalPresence
                    If oContext.SubView = ViewDetailPage.UserReport Then
                        Me.LoadPresenceResults(oContext)
                    Else
                        Dim oPager As New PagerBase
                        oPager.PageIndex = Me.View.CurrentPage
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager

                        Me.View.ShowSummary(IviewUsageResults.SummaryType.OwnFilter, "", "")
                        Me.View.ShowDetailView(ViewDetailPage.UserReport)
                        Me.View.NavigationUrl(Me.ResContext, current)
                        Me.View.AddActionSpecifyFilters(oContext.CommunityID, oContext.UserID)
                    End If
                Case IviewUsageResults.viewType.MyCommunitiesPresence
                    If oContext.SubView = ViewDetailPage.UserReport Then
                        Me.LoadPresenceResults(oContext)
                    Else
                        Me.View.ShowDetailView(ViewDetailPage.MyCommunityList)
                        Me.LoadUserCommunities(oContext)
                    End If

                Case IviewUsageResults.viewType.CurrentCommunityPresence
                    Me.View.ShowDetailView(ViewDetailPage.UserReport)
                    If oContext.CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    If oContext.SubView = ViewDetailPage.UserReport Then
                        Dim oPager As New PagerBase
                        oPager.PageIndex = Me.View.CurrentPage
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager
                        Me.LoadPresenceResults(oContext)
                    Else
                        Dim oPager As New PagerBase
                        oPager.PageIndex = Me.View.CurrentPage
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager
                        Me.View.ShowSummary(IviewUsageResults.SummaryType.OwnCommunityFilter, Me.UserContext.CurrentUser.SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
                        Me.View.AddActionSpecifyFilters(oContext.CommunityID, oContext.UserID)
                    End If
                Case IviewUsageResults.viewType.UsersCurrentCommunityPresence
                    If oContext.CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    If oContext.SubView = ViewDetailPage.UserReport Then
                        Me.LoadUserResult(oContext)
                    Else
                        oContext.UserID = 0
                        Me.View.ShowDetailView(ViewDetailPage.UsersList)
                        Me.LoadCommunityUsers(oContext)
                    End If

                Case IviewUsageResults.viewType.UsersPortalPresence
                    If oContext.SubView = ViewDetailPage.UserReport Then
                        Me.LoadUserResult(oContext)
                    Else
                        oContext.UserID = 0
                        Me.View.ShowDetailView(ViewDetailPage.UsersList)
                        Me.LoadPortalUsers(oContext)
                    End If
                Case IviewUsageResults.viewType.BetweenDateUsersPortal
                    If oContext.SubView = ViewDetailPage.BetweenDateReports Then
                        Dim oPager As New PagerBase
                        oPager.PageIndex = Me.View.CurrentPage
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        oContext.SubView = ViewDetailPage.BetweenDateReports
                        Me.LoadUsersResultsBetweenDate(oContext, oPager)
                    Else
                        Dim oPager As New PagerBase
                        oPager.PageIndex = 0
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager

                        oContext.UserID = 0
                        Me.View.ShowDetailView(ViewDetailPage.BetweenDateReports)
                        Me.View.ShowSummary(IviewUsageResults.SummaryType.PortalBetweenDateFilter, "", "")
                    End If

                Case IviewUsageResults.viewType.BetweenDateUsersCommunity
                    If oContext.CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    If oContext.SubView = ViewDetailPage.BetweenDateReports Then
                        Dim oPager As New PagerBase
                        oPager.PageIndex = Me.View.CurrentPage
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        oContext.Order = ResultsOrder.Day
                        oContext.SubView = ViewDetailPage.BetweenDateReports
                        Me.LoadUsersResultsBetweenDate(oContext, oPager)
                    Else
                        Dim oPager As New PagerBase
                        oPager.PageIndex = 0
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager

                        Me.View.ShowDetailView(ViewDetailPage.BetweenDateReports)
                        Me.View.ShowSummary(IviewUsageResults.SummaryType.CommunityBetweenDateFilter, "", "")
                        ' Me.View.NavigationUrl(Me.ResContext, current)
                        'Me.View.AddActionSpecifyFilters(oContext.CommunityID, oContext.UserID)
                    End If

                Case Else
                    Me.View.NavigationUrl(Me.ResContext, current)
            End Select
            Me.View.SetFirstColumHeader(current, Me.View.CurrentDetailView)
            ' Me.SetPreviousPage()
        End Sub

        Public Sub LoadPresenceResults(ByVal oContext As ResultContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            If oContext.FromDate <> Nothing Then
                Me.View.SelectedStartDate = oContext.FromDate
            End If
            If oContext.ToDate <> Nothing Then
                Me.View.SelectedEndDate = oContext.ToDate
            End If

            Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.GetUsageResults(oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)

            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewUsageResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewUsageResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.Pager = oPager
            Me.View.LoadItems(oResults)
            Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.GetPermission(oContext.CommunityID).Print)
            Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
            SetBackUrl(oContext)
            Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)
        End Sub
        Public Sub LoadUserCommunities(ByVal oContext As ResultContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.FindUserCommunities(oContext, Me.View.CurrentPage, oPager)

            Me.View.Pager = oPager

            For Each o In oResults
                Dim TempContext As ResultContext = oContext.Clone
                TempContext.Ascending = True
                TempContext.CurrentPage = 0
                TempContext.Order = ResultsOrder.Day
                TempContext.SubView = ViewDetailPage.UserReport
                TempContext.UserID = o.PersonID
                TempContext.CommunityID = o.CommunityID
                o.NavigateTo = Me.View.GetNavigationUrl(TempContext, Me.View.CurrentView)
            Next
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.ShowSummary(IviewUsageResults.SummaryType.UserCommunitiesList, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            Me.View.LoadItems(oResults)
            Me.View.AddActionViewCommunities(oContext.CommunityID, oContext.UserID)
        End Sub
        Public Sub LoadCommunityUsers(ByVal oContext As ResultContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.FindCommunityUsers(oContext, Me.View.CurrentPage, oPager)
            For Each o In oResults
                Dim TempContext As ResultContext = oContext.Clone
                TempContext.Ascending = True
                TempContext.CurrentPage = 0
                TempContext.Order = ResultsOrder.Day
                TempContext.SubView = ViewDetailPage.UserReport
                TempContext.UserID = o.PersonID
                TempContext.CommunityID = o.CommunityID
                o.NavigateTo = Me.View.GetNavigationUrl(TempContext, Me.View.CurrentView)
            Next
            Me.View.Pager = oPager
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.LoadItems(oResults)
            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewUsageResults.SummaryType.CommunityUsers, "", Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            End If
            Me.View.AddActionViewUsers(oContext.CommunityID, oContext.UserID)
        End Sub
        Public Sub LoadPortalUsers(ByVal oContext As ResultContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.FindPortalUsers(oContext, Me.View.CurrentPage, oPager)

            Me.View.Pager = oPager
            For Each o In oResults
                Dim TempContext As ResultContext = oContext.Clone
                TempContext.Ascending = True
                TempContext.CurrentPage = 0
                TempContext.Order = ResultsOrder.Day
                TempContext.SubView = ViewDetailPage.UserReport
                TempContext.UserID = o.PersonID
                o.NavigateTo = Me.View.GetNavigationUrl(TempContext, Me.View.CurrentView)
            Next
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.ShowSummary(IviewUsageResults.SummaryType.PortalUsers, "", "")
            Me.View.LoadItems(oResults)
            Me.View.AddActionViewUsers(oContext.CommunityID, oContext.UserID)
        End Sub
        Public Sub SearchResults()
            Dim oContext As ResultContext = Me.ResContext.Clone

            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            oContext.FromDate = Me.CurrentDateI
            oContext.ToDate = Me.CurrentDateF


            Dim oView As IviewUsageResults.viewType = Me.View.CurrentView
            If oView = IviewUsageResults.viewType.BetweenDateUsersCommunity OrElse oView = IviewUsageResults.viewType.BetweenDateUsersPortal Then
                oContext.Order = ResultsOrder.Day
                oContext.SubView = ViewDetailPage.BetweenDateReports
                Me.View.ResultsContext = oContext
                Me.LoadUsersResultsBetweenDate(oContext, oPager)
            Else
                oContext.SubView = ViewDetailPage.UserReport
                Me.View.ResultsContext = oContext

                Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.GetUsageResults(oContext, Me.CurrentDateI, Me.CurrentDateF, Me.View.CurrentPage, oPager)
                Me.View.ShowDetailView(ViewDetailPage.UserReport)
                Me.View.Pager = oPager
                Me.View.SetFirstColumHeader(Me.View.CurrentView, ViewDetailPage.UserReport)
                Me.View.LoadItems(oResults)
                Me.View.NavigationUrl(oContext, Me.View.CurrentView)
                Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.GetPermission(oContext.CommunityID).Print)
                Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
                If oContext.CommunityID > 0 Then
                    Me.View.ShowSummary(IviewUsageResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
                Else
                    Me.View.ShowSummary(IviewUsageResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
                End If
                SetBackUrl(oContext)
                Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)
            End If


        End Sub

        Private Sub LoadUsersResultsBetweenDate(ByVal oContext As ResultContext, ByVal oPager As PagerBase)
            If oContext.FromDate <> Nothing Then
                Me.View.SelectedStartDate = oContext.FromDate
            End If
            If oContext.ToDate <> Nothing Then
                Me.View.SelectedEndDate = oContext.ToDate
            End If

            Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.FindUsersBetweenDate(oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)

            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewUsageResults.SummaryType.CommunityBetweenDateFilter, "", Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewUsageResults.SummaryType.PortalBetweenDateFilter, "", "") ', Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            Me.View.ShowDetailView(ViewDetailPage.BetweenDateReports)
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.Pager = oPager
            Me.View.LoadItems(oResults)
            Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.GetPermission(oContext.CommunityID).Print)
            Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
            SetBackUrl(oContext)
            Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)
            Me.View.SetFirstColumHeader(Me.View.CurrentView, ViewDetailPage.BetweenDateReports)


        End Sub

        Private Sub LoadUserResult(ByVal oContext As ResultContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oResults As List(Of dtoAccessResult) = Me.CurrentManager.GetUsageResults(oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)
            Me.View.ShowDetailView(ViewDetailPage.UserReport)
            Me.View.Pager = oPager
            Me.View.LoadItems(oResults)
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.GetPermission(oContext.CommunityID).Print)
            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewUsageResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewUsageResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
            SetBackUrl(oContext)
            Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)
        End Sub

        Private Function SetupViews() As Boolean
            Dim oSelectedView As IviewUsageResults.viewType = Me.View.PreLoadedView
            If oSelectedView = IviewUsageResults.viewType.None And Me.UserContext.CurrentCommunityID > 0 Then
                oSelectedView = IviewUsageResults.viewType.CurrentCommunityPresence
                Me.View.CurrentDetailView = ViewDetailPage.UserReport
            ElseIf oSelectedView = IviewUsageResults.viewType.None Then
                oSelectedView = IviewUsageResults.viewType.MyPortalPresence
                Me.View.CurrentDetailView = ViewDetailPage.UserReport
            Else
                Me.View.CurrentDetailView = Me.View.PreLoadedDetailView
            End If

            Dim oList As IList(Of IviewUsageResults.viewType) = Me.ListAvailableViews(oSelectedView)
            Me.View.ViewAvailable = oList
            If oList.Contains(Me.View.PreLoadedView) Then
                Me.View.CurrentView = oSelectedView
            End If
            If IsNothing(oList) OrElse oList.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Function
        Private Function ListAvailableViews(ByVal StartView As IviewUsageResults.viewType) As IList(Of IviewUsageResults.viewType)
            Dim oList As New List(Of IviewUsageResults.viewType)

            ' VERIFICO LO STATUS DI VISUALIZZAZIONE IN CUI MI TROVO !
            ' SONO NEL GLOBALE !
            Dim isSystemPage As Boolean = (StartView = IviewUsageResults.viewType.MyPortalPresence OrElse StartView = IviewUsageResults.viewType.MyCommunitiesPresence OrElse StartView = IviewUsageResults.viewType.UsersPortalPresence OrElse StartView = IviewUsageResults.viewType.BetweenDateUsersPortal)
            Dim isCommunityPage As Boolean = (Not isSystemPage AndAlso (StartView = IviewUsageResults.viewType.CurrentCommunityPresence OrElse StartView = IviewUsageResults.viewType.UsersCurrentCommunityPresence OrElse StartView = IviewUsageResults.viewType.BetweenDateUsersCommunity))

            If Me.AppContext.UserContext.CurrentUser.Id > 0 Then
                Dim UserTypeID As Integer = Me.AppContext.UserContext.UserTypeID
                If isSystemPage Then
                    oList.Add(IviewUsageResults.viewType.MyPortalPresence)
                    oList.Add(IviewUsageResults.viewType.MyCommunitiesPresence)
                    If UserTypeID = UserTypeStandard.Administrative OrElse UserTypeID = UserTypeStandard.Administrator OrElse UserTypeID = UserTypeStandard.SysAdmin Then
                        oList.Add(IviewUsageResults.viewType.UsersPortalPresence)
                        oList.Add(IviewUsageResults.viewType.BetweenDateUsersPortal)
                    End If
                ElseIf isCommunityPage Then
                    Dim oPermission As ModuleUsageResult = Me.GetPermission(Me.ResContext.CommunityID)

                    If oPermission.ViewMyReport Then
                        oList.Add(IviewUsageResults.viewType.CurrentCommunityPresence)
                    End If
                    If oPermission.Administration OrElse oPermission.ViewCommunityReports Then
                        oList.Add(IviewUsageResults.viewType.UsersCurrentCommunityPresence)
                        oList.Add(IviewUsageResults.viewType.BetweenDateUsersCommunity)
                    End If
                End If
            End If
            Return oList
        End Function
        Public Function GetUrlForTab(ByVal value As IviewUsageResults.viewType) As String
            Dim url = ""
            Dim oContext As New ResultContext With {.Ascending = True, .Order = ResultsOrder.Hour, .CurrentPage = 0, .SubView = ViewDetailPage.None}


            Select Case value
                Case IviewUsageResults.viewType.None
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    If Me.ResContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.ResContext.CommunityID
                    End If
                    url = Me.View.GetNavigationUrl(oContext, value)
                Case IviewUsageResults.viewType.MyPortalPresence
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    url = Me.View.GetNavigationUrl(oContext, value)
                Case IviewUsageResults.viewType.UsersPortalPresence
                    oContext.UserID = 0
                    oContext.Order = ResultsOrder.Owner
                    oContext.SubView = ViewDetailPage.UsersList
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case IviewUsageResults.viewType.UsersCurrentCommunityPresence
                    oContext.Order = ResultsOrder.Owner
                    If Me.ResContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.ResContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = 0
                    oContext.SubView = ViewDetailPage.UsersList
                    url = Me.View.GetNavigationUrl(oContext, value)


                Case IviewUsageResults.viewType.CurrentCommunityPresence
                    If Me.ResContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.ResContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = Me.ResContext.UserID
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case IviewUsageResults.viewType.MyCommunitiesPresence
                    oContext.Order = ResultsOrder.Community
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    oContext.SubView = ViewDetailPage.MyCommunityList
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case IviewUsageResults.viewType.BetweenDateUsersPortal
                    oContext.Order = ResultsOrder.Owner
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case IviewUsageResults.viewType.BetweenDateUsersCommunity
                    oContext.Order = ResultsOrder.Owner
                    oContext.UserID = 0
                    If Me.ResContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.ResContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case Else
                    url = ""
            End Select
            Return url
        End Function


        Private Sub SetBackUrl(ByVal oContext As ResultContext)
            Dim oCurrentView As IviewUsageResults.viewType = Me.View.CurrentView
            If (oCurrentView = IviewUsageResults.viewType.MyCommunitiesPresence OrElse oCurrentView = IviewUsageResults.viewType.UsersCurrentCommunityPresence OrElse oCurrentView = IviewUsageResults.viewType.UsersPortalPresence) Then
                If oContext.SubView = ViewDetailPage.UserReport Then
                    Dim oTemp As ResultContext = oContext.Clone
                    If Me.UserContext.CurrentCommunityID > 0 Then
                        oTemp.CommunityID = Me.UserContext.CurrentCommunityID
                        oTemp.SubView = ViewDetailPage.UsersList
                    Else
                        oTemp.CommunityID = 0
                        If oCurrentView = IviewUsageResults.viewType.MyPortalPresence Then
                            oTemp.SubView = ViewDetailPage.None
                            oTemp.Order = ResultsOrder.Day
                        ElseIf oCurrentView = IviewUsageResults.viewType.MyCommunitiesPresence Then
                            oTemp.SubView = ViewDetailPage.MyCommunityList
                            oTemp.Order = ResultsOrder.Community
                        ElseIf oCurrentView = IviewUsageResults.viewType.UsersCurrentCommunityPresence OrElse oCurrentView = IviewUsageResults.viewType.UsersPortalPresence Then
                            oTemp.SubView = ViewDetailPage.UsersList
                            oTemp.Order = ResultsOrder.Owner
                        End If
                        oTemp.CurrentPage = 0
                        oTemp.ToDate = Nothing
                        oTemp.FromDate = Nothing
                        oTemp.Ascending = True
                    End If
                    Me.View.SetPreviousURL = Me.View.GetNavigationUrl(oTemp, oCurrentView)
                End If
            End If
        End Sub
    End Class
End Namespace