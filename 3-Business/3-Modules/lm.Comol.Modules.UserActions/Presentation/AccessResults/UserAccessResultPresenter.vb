Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class UserAccessResultPresenter
        Inherits ResultsBasePresenter


#Region "Standard"
        Public Overloads ReadOnly Property View() As iViewUserAccessResult
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As iViewUserAccessResult)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
#End Region

        Private ReadOnly Property ViewContext() As ResultContextBase
            Get
                If IsNothing(_ContexFromView) Then
                    Dim oContext As ResultContextBase = Me.View.ResultsContext
                    Dim PersonID As Integer = oContext.UserID
                    If PersonID = 0 Then
                        PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    If oContext.Order = ResultsOrder.None Then
                        oContext.Order = ResultsOrder.Day
                        oContext.Ascending = False
                    End If
                    If oContext.FromView = viewType.None Then
                        oContext.FromView = Me.View.PreLoadedFromView
                    End If
                    'If oContext.CommunityID = 0 AndAlso Not Me.View.Then Then
                    '    oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    'End If
                    oContext.NameSurnameFilter = Me.View.PreLoadedUserName
                    oContext.UserID = PersonID
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
                Dim oContext As ResultContextBase = Me.ViewContext.Clone
                If Me.HasPermission() Then
                    If Not Equals(oContext.FromDate, New Date) Then
                        Me.View.SelectedStartDate = oContext.FromDate
                    End If
                    If Not Equals(oContext.ToDate, New Date) Then
                        Me.View.SelectedEndDate = oContext.ToDate
                    End If
                    If Me.View.SelectedStartDate.HasValue AndAlso Me.View.SelectedEndDate.HasValue Then
                        Me.LoadPresenceResults(oContext, Me.View.CurrentPage)
                    Else
                        Dim oPager As New PagerBase
                        oPager.PageIndex = 0
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager
                        NoSearchDone(oContext)
                    End If
                Else
                    Me.View.NoPermissionToAccess(oContext.CommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
                End If
            Else
                Me.View.NoPermissionToAccess(Me.UserContext.CurrentCommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
            End If
        End Sub

        Public Sub LoadPresenceResults(ByVal oContext As ResultContextBase, ByVal CurrentPageIndex As Integer)
            Dim oPager As New PagerBase
            oPager.PageIndex = CurrentPageIndex
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            Me.View.Pager = oPager

            If Me.CurrentDateI <> Nothing AndAlso Me.CurrentDateF <> Nothing Then
                If oContext.FromDate <> Nothing Then
                    Me.View.SelectedStartDate = oContext.FromDate
                End If
                If oContext.ToDate <> Nothing Then
                    Me.View.SelectedEndDate = oContext.ToDate
                End If

                Dim oResults As List(Of UsageResults.DomainModel.dtoAccessResult)
                Dim TotalUsageTime As TimeSpan
                If Me.View.ShowUserNameSearch Then
                    oResults = Me.CurrentManager.FindUsersBetweenDate(TotalUsageTime, oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)
                Else
                    oResults = Me.CurrentManager.GetUsageResults(TotalUsageTime, oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)
                End If
                If Me.View.ShowUserNameSearch Then
                    If oContext.CommunityID > 0 Then
                        Me.View.ShowSummary(IviewAccessResults.SummaryType.CommunityBetweenDateFilter, "", "")
                    Else
                        Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalBetweenDateFilter, "", "")
                    End If
                    Me.View.ShowUserName = True
                Else
                    If oContext.CommunityID > 0 Then
                        Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
                    Else
                        Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
                    End If
                    Me.View.ShowUserName = Not (oContext.UserID = Me.UserContext.CurrentUser.Id)
                End If

                Me.View.NavigationUrl(oContext, Me.View.CurrentView)
                Me.View.Pager = oPager
                Me.View.LoadItems(TotalUsageTime, oResults)
                Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.GetPermission(oContext.CommunityID).Print)
                Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
                SetBackUrl(oContext)
                Me.View.AddActionViewReport(oContext.CommunityID, Me.UserContext.CurrentUser.Id, oContext.UserID)
            Else
                'Me.View.ShowSummary(IviewAccessResults.SummaryType.OwnFilter, "", "")
                'Me.View.NavigationUrl(Me.ViewContext, Me.View.CurrentView)
                'Me.View.AddActionSpecifyFilters(oContext.CommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
                NoSearchDone(oContext)
            End If
        End Sub

        Private Sub NoSearchDone(ByVal oContext As ResultContextBase)
            If Me.View.ShowUserNameSearch Then
                If oContext.CommunityID > 0 Then
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.CommunityBetweenDateFilter, "", "")
                Else
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalBetweenDateFilter, "", "")
                End If
            Else
                If oContext.CommunityID > 0 Then
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
                Else
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
                End If
            End If
            SetBackUrl(oContext)
            Me.View.AllowPrint = False
            Me.View.NavigationUrl(Me.ViewContext, Me.View.CurrentView)
            Me.View.AddActionSpecifyFilters(oContext.CommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
        End Sub

        Public Sub SearchResults()
            Dim oContext As ResultContextBase = Me.ViewContext.Clone
            oContext.FromDate = Me.CurrentDateI
            oContext.ToDate = Me.CurrentDateF
            oContext.NameSurnameFilter = Me.View.UserName
            Dim oView As viewType = Me.View.CurrentView
            Me.View.ResultsContext = oContext
            LoadPresenceResults(oContext, 0)
        End Sub

        Public Sub ChangePageSize()
            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            Me.SearchResults()
        End Sub

        Private Function HasPermission() As Boolean
            Dim iResponse As Boolean = False
            Dim oPermission As UsageResults.DomainModel.ModuleUsageResult
            Dim oContext As ResultContextBase = Me.ViewContext
            Dim oView As viewType = Me.View.CurrentView
            Select Case oView
                Case viewType.MyPortalPresence
                    oPermission = Me.GetPermission(0)
                    iResponse = (Me.ViewContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.ViewMyReport OrElse oPermission.Administration)
                Case viewType.BetweenDateUsersPortal
                    oPermission = Me.GetPermission(0)
                    iResponse = (oPermission.Administration OrElse oPermission.PortalReports)
                Case viewType.BetweenDateUsersCommunity
                    oPermission = Me.GetPermission(Me.ViewContext.CommunityID)
                    iResponse = (oPermission.Administration OrElse oPermission.ViewCommunityReports)
                Case viewType.CurrentCommunityPresence
                    oPermission = Me.GetPermission(Me.ViewContext.CommunityID)
                    iResponse = (Me.ViewContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.Administration OrElse oPermission.ViewCommunityReports OrElse oPermission.ViewMyReport)
                Case viewType.OtherUserPresence
                    If oContext.FromView = viewType.BetweenDateUsersPortal OrElse oContext.FromView = viewType.UsersPortalPresence Then
                        oPermission = Me.GetPermission(0)
                        iResponse = (oPermission.Administration OrElse oPermission.PortalReports)
                    Else
                        oPermission = Me.GetPermission(Me.ViewContext.CommunityID)
                        iResponse = (oPermission.Administration OrElse oPermission.ViewCommunityReports) OrElse ((Me.ViewContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso oPermission.ViewMyReport)
                    End If
                Case viewType.MyPortalPresence
                    oPermission = Me.GetPermission(0)
                    iResponse = (Me.ViewContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.ViewMyReport OrElse oPermission.Administration)

            End Select
            Return iResponse
        End Function

        Protected Sub SetBackUrl(ByVal oContext As ResultContextBase)
            Dim oCurrentView As viewType = Me.View.CurrentView
            Dim oTemp As ResultContextBase = oContext.Clone

            oTemp.CurrentPage = 0
            oTemp.ToDate = Nothing
            oTemp.FromDate = Nothing
            oTemp.Ascending = True
            oTemp.NameSurnameFilter = ""
            oTemp.CommunityID = 0
            oTemp.UserID = 0
            Select Case oContext.FromView
                Case viewType.UsersPortalPresence
                    oTemp.Order = DomainModel.ResultsOrder.Owner
                Case viewType.BetweenDateUsersCommunity
                    oTemp.Order = DomainModel.ResultsOrder.Owner
                    If Me.UserContext.CurrentCommunityID > 0 Then
                        oTemp.CommunityID = Me.UserContext.CurrentCommunityID > 0
                    End If
                Case viewType.BetweenDateUsersPortal
                    oTemp.Order = DomainModel.ResultsOrder.Owner
                Case viewType.MyCommunitiesPresence
                    oTemp.Order = DomainModel.ResultsOrder.Community
                Case viewType.UsersCurrentCommunityPresence
                    oTemp.Order = DomainModel.ResultsOrder.Owner
                    If Me.UserContext.CurrentCommunityID > 0 Then
                        oTemp.CommunityID = Me.UserContext.CurrentCommunityID
                    End If
                Case viewType.MyPortalPresence
                    oTemp.Order = DomainModel.ResultsOrder.Day
                Case Else
                    oTemp = Nothing

            End Select

            If Not IsNothing(oTemp) Then
                Me.View.SetPreviousURL = Me.View.GetNavigationUrl(oTemp, oContext.FromView)
            End If
        End Sub
    End Class
End Namespace