Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class UserAccessCommunityListPresenter
        Inherits ResultsBasePresenter


#Region "Standard"
        Public Overloads ReadOnly Property View() As iViewCommunityList
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As iViewCommunityList)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
#End Region

        Private ReadOnly Property ViewContext() As ResultContextBase
            Get
                If IsNothing(_ContexFromView) Then
                    Dim oContext As ResultContextBase = Me.View.ResultsContext
                    Dim PersonID As Integer = oContext.UserID
                    If PersonID = 0 AndAlso Me.View.isPersonal Then
                        PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    If oContext.Order = ResultsOrder.None Then
                        oContext.Order = ResultsOrder.Day
                        oContext.Ascending = True
                    End If
                    If oContext.FromView = viewType.None Then
                        oContext.FromView = Me.View.PreLoadedFromView
                    End If
                    oContext.NameSurnameFilter = Me.View.PreLoadedCommunityName
                    oContext.UserID = PersonID
                    Me.View.ResultsContext = oContext
                    _ContexFromView = oContext
                End If
                Return _ContexFromView
            End Get
        End Property
        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                If Me.HasPermission() Then
                    Me.View.CommunityName = Me.ViewContext.NameSurnameFilter
                    Me.SearchResults(Me.View.CurrentPageIndex)
                Else
                    Me.View.NoPermissionToAccess(Me.UserContext.CurrentCommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
                End If
            Else
                Me.View.NoPermissionToAccess(Me.UserContext.CurrentCommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
            End If
        End Sub
        Public Sub SearchResults(ByVal CurrentPageIndex As Integer)
            Dim oContext As ResultContextBase = Me.ViewContext.Clone

            Dim oPager As New PagerBase
            oPager.PageIndex = CurrentPageIndex
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            oContext.NameSurnameFilter = Me.View.CommunityName
            If oContext.NameSurnameFilter <> "" Then
                oContext.NameSurnameFilter = oContext.NameSurnameFilter.ToLower
            End If
            Me.View.ResultsContext = oContext

            Dim oResults As List(Of dtoCommunityResult) = Me.CurrentManager.FindUserCommunitiesList(oContext, CurrentPageIndex, oPager)
            Dim isPersonal As Boolean = Me.View.isPersonal
            For Each o In oResults
                Dim TempContext As ResultContextBase = oContext.Clone
                TempContext.Ascending = True
                TempContext.CurrentPage = 0
                TempContext.Order = ResultsOrder.Day
                TempContext.FromView = Me.View.CurrentView
                TempContext.UserID = o.PersonID
                TempContext.CommunityID = o.CommunityID
                TempContext.NameSurnameFilter = ""

                If isPersonal Then
                    o.NavigateTo = Me.View.GetNavigationUrl(TempContext, viewType.OtherUserPresence)
                Else
                    o.NavigateTo = Me.View.GetNavigationUrl(TempContext, viewType.OtherUserPresence)
                End If
            Next

            Me.View.Pager = oPager
            Me.View.LoadCommunity(oResults)
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunitiesList, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "") ', Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunitiesList, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            SetBackUrl(oContext)
            Me.View.AddActionViewCommunities(oContext.CommunityID, Me.UserContext.CurrentUser.Id, oContext.UserID)
        End Sub
        Private Function HasPermission() As Boolean
            Dim iResponse As Boolean = False
            Dim isPersonal As Boolean = Me.View.isPersonal
            Dim oPermission As UsageResults.DomainModel.ModuleUsageResult
            Dim oContext As ResultContextBase = Me.ViewContext

            If isPersonal Then
                If oContext.UserID = Me.UserContext.CurrentUser.Id Then
                    oPermission = Me.GetPermission(Me.ViewContext.CommunityID) ' Me.Permission()
                    iResponse = oPermission.ViewMyReport OrElse oPermission.Administration
                Else
                    iResponse = False
                End If
            Else
                oPermission = Me.GetPermission(Me.ViewContext.CommunityID)
                If oContext.UserID = Me.UserContext.CurrentUser.Id Then
                    iResponse = oPermission.ViewMyReport OrElse oPermission.Administration OrElse oPermission.PortalReports OrElse oPermission.ViewCommunityReports
                Else
                    iResponse = oPermission.Administration OrElse oPermission.PortalReports OrElse oPermission.ViewCommunityReports
                End If
            End If
            Return iResponse
        End Function

        Public Sub ChangePageSize()
            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            Me.SearchResults(0)
        End Sub

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
                        oTemp.CommunityID = Me.UserContext.CurrentCommunityID > 0
                    End If
                Case viewType.MyPortalPresence
                    oTemp.Order = DomainModel.ResultsOrder.Day
                Case Else
                    oTemp = Nothing

            End Select

            If Not IsNothing(oTemp) AndAlso oContext.FromView <> oCurrentView Then
                Me.View.SetPreviousURL = Me.View.GetNavigationUrl(oTemp, oContext.FromView)
            End If
        End Sub
    End Class
End Namespace