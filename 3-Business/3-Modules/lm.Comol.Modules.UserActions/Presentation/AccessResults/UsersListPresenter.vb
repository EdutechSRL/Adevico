Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class UsersAccessListPresenter
        Inherits ResultsBasePresenter


#Region "Standard"
        Public Overloads ReadOnly Property View() As iViewUsersAccessList
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As iViewUsersAccessList)
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
                        oContext.Ascending = True
                    End If
                    If oContext.FromView = viewType.None Then
                        oContext.FromView = Me.View.PreLoadedFromView
                    End If
                    If oContext.CommunityID = 0 AndAlso Not Me.View.isPortal Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.NameSurnameFilter = Me.View.PreLoadedUserName
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
                    Me.View.UserName = Me.ViewContext.NameSurnameFilter
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

            oContext.NameSurnameFilter = Me.View.UserName
            If oContext.NameSurnameFilter <> "" Then
                oContext.NameSurnameFilter = oContext.NameSurnameFilter.ToLower
            End If
            Me.View.ResultsContext = oContext

            Dim oResults As List(Of dtoUserAccessResult)
            If oContext.CommunityID > 0 Then
                oResults = Me.CurrentManager.FindCommunityUsersList(oContext, CurrentPageIndex, oPager)
            Else
                oResults = Me.CurrentManager.FindPortalUsersList(oContext, CurrentPageIndex, oPager)
            End If

            For Each o In oResults
                Dim TempContext As ResultContextBase = oContext.Clone
                TempContext.Ascending = True
                TempContext.CurrentPage = 0
                TempContext.Order = ResultsOrder.Day
                TempContext.FromView = Me.View.CurrentView
                TempContext.UserID = o.PersonID
                TempContext.NameSurnameFilter = ""
                o.NavigateTo = Me.View.GetNavigationUrl(TempContext, viewType.OtherUserPresence)
            Next

            Me.View.Pager = oPager
            Me.View.LoadPersons(oResults)
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            SetSummaryType(oContext, Me.View.CurrentView)
            SetBackUrl(oContext)
            Me.View.AddActionViewUsers(oContext.CommunityID, Me.UserContext.CurrentUser.Id, oContext.UserID)
        End Sub
        Public Sub ChangePageSize()
            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            Me.SearchResults(0)
        End Sub
        Private Function HasPermission() As Boolean
            Dim iResponse As Boolean = False
            Dim isPortal As Boolean = Me.View.isPortal
            Dim oPermission As UsageResults.DomainModel.ModuleUsageResult
            Dim oContext As ResultContextBase = Me.ViewContext

            If isPortal Then
                oPermission = Me.GetPermission(0)
                iResponse = oPermission.PortalReports OrElse oPermission.Administration
            Else
                oPermission = Me.GetPermission(Me.ViewContext.CommunityID)
                iResponse = oPermission.Administration OrElse oPermission.ViewCommunityReports
            End If
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

        Private Sub SetSummaryType(ByVal oContext As ResultContextBase, ByVal View As viewType)
            Select Case View
                Case viewType.BetweenDateUsersPortal
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.CommunityBetweenDateFilter, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name, "")
                Case viewType.BetweenDateUsersCommunity
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalBetweenDateFilter, "", "")
                Case viewType.UsersCurrentCommunityPresence
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.CommunityUsers, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name, "")
                Case viewType.UsersPortalPresence
                    Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalUsers, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
                Case Else
                  
            End Select




            ' <item name="summary.0">Indicare gli intervalli di data per la visualizzazione del report di accesso.</item>
            ' <item name="summary.1">Indicare gli intervalli di data per la visualizzazione del report di accesso dell'utente &lt;b&gt;{0}&lt;/b&gt;.</item>
            ' <item name="summary.2">Indicare gli intervalli di data per la visualizzazione del report di accesso alla comunità &lt;b&gt;{0}&lt;/b&gt;.</item>
            ' <item name="summary.3">Indicare gli intervalli di data per la visualizzazione del report di accesso alla comunità &lt;b&gt;{0}&lt;/b&gt; dell'utente &lt;b&gt;{1}&lt;/b&gt;</item>
            ' <item name="summary.4">Lista delle comunità a cui l'utente &lt;b&gt;{0}&lt;/b&gt; è iscritto.</item>
            ' <item name="summary.5">Lista degli utenti del portale.</item>
            ' <item name="summary.6">Lista degli utenti della comunità &lt;b&gt;{0}&lt;/b&gt; con accessi.</item>
            '<item name="summary.7">Indicare gli intervalli di data per la visualizzazione del report di accesso (è possibile specificare anche il nome e/o il cognome o parti di esso dell'utente/i).</item>
            '<item name="summary.8">Indicare gli intervalli di data per la visualizzazione del report di accesso alla comunità &lt;b&gt;{0}&lt;/b&gt; (è possibile specificare anche il nome e/o il cognome o parti di esso dell'utente/i).</item>

        End Sub
    End Class
End Namespace