Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation

'Imports lm.Comol.Modules.AccessResults.BusinessLogic

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class MyPortalAccessPresenter
        Inherits AccessResultsBasePresenter


#Region "Standard"
        Public Overloads ReadOnly Property View() As iViewMyPortalAccess
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As iViewMyPortalAccess)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
#End Region

        Private ReadOnly Property ViewContext() As ResultContextBase
            Get
                If IsNothing(_ContexFromView) Then
                    Dim oContext As ResultContextBase = Me.View.ResultsContext
                    Dim PersonID As Integer = oContext.UserID
                    If PersonID = 0 AndAlso (Me.View.CurrentView = viewType.MyPortalPresence OrElse Me.View.CurrentView = viewType.CurrentCommunityPresence) Then
                        PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    If oContext.Order = ResultsOrder.None Then
                        oContext.Order = ResultsOrder.Day
                        oContext.Ascending = True
                    End If
                    If oContext.FromView = viewType.None Then
                        oContext.FromView = Me.View.PreLoadedFromView
                    End If
                    If Me.View.PreLoadedStartDate.HasValue Then
                        oContext.FromDate = Me.View.PreLoadedStartDate.Value
                    End If
                    If Me.View.PreLoadedEndDate.HasValue Then
                        oContext.ToDate = Me.View.PreLoadedEndDate.Value
                    End If
                    oContext.UserID = PersonID
                    Dim CommunityID As Integer = oContext.CommunityID
                    If CommunityID <= 0 AndAlso (Me.View.CurrentView = viewType.CurrentCommunityPresence OrElse Me.View.CurrentView = viewType.UsersCurrentCommunityPresence OrElse Me.View.CurrentView = viewType.BetweenDateUsersCommunity) Then
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
                    Dim oContext As ResultContextBase = Me.ViewContext.Clone
                    If Me.View.SelectedStartDate.HasValue AndAlso Me.View.SelectedEndDate.HasValue Then
                        Me.LoadPresenceResults(oContext)
                    Else
                        Dim oPager As New PagerBase
                        oPager.PageIndex = Me.View.CurrentPage
                        oPager.PageSize = Me.View.CurrentPageSize
                        oPager.Count -= 1
                        Me.View.Pager = oPager

                        Me.View.ShowSummary(IviewAccessResults.SummaryType.OwnFilter, "", "")
                        Me.View.NavigationUrl(Me.ViewContext, viewType.MyPortalPresence)
                        Me.View.AddActionSpecifyFilters(oContext.CommunityID, oContext.UserID)

                    End If
                Else
                    Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0)
                    Me.View.NoPermissionToAccess()
                End If
            Else
                Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0)
                Me.View.NoPermissionToAccess()
            End If
        End Sub

        Public Sub LoadPresenceResults(ByVal oContext As ResultContextBase)
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

            Dim oResults As List(Of UsageResults.DomainModel.dtoAccessResult) = Me.CurrentManager.GetUsageResults(oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)

            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.Pager = oPager
            Me.View.LoadItems(oResults)
            Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.Permission.Print)
            Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
            SetBackUrl(oContext)
            Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)
        End Sub
        Public Sub SearchResults()
            Dim oContext As ResultContextBase = Me.ViewContext.Clone

            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            oContext.FromDate = Me.CurrentDateI
            oContext.ToDate = Me.CurrentDateF


            Dim oView As viewType = Me.View.CurrentView

            Me.View.ResultsContext = oContext

            Dim oResults As List(Of UsageResults.DomainModel.dtoAccessResult) = Me.CurrentManager.GetUsageResults(oContext, Me.CurrentDateI, Me.CurrentDateF, Me.View.CurrentPage, oPager)
            Me.View.Pager = oPager
            Me.View.LoadItems(oResults)
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.Permission.Print)
            Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            SetBackUrl(oContext)
            Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)

        End Sub


        Private Sub LoadUserResult(ByVal oContext As ResultContextBase)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oResults As List(Of UsageResults.DomainModel.dtoAccessResult) = Me.CurrentManager.GetUsageResults(oContext, oContext.FromDate, oContext.ToDate, Me.View.CurrentPage, oPager)
            Me.View.Pager = oPager
            Me.View.LoadItems(oResults)
            Me.View.NavigationUrl(oContext, Me.View.CurrentView)
            Me.View.AllowPrint = (oResults.Count > 0 AndAlso Me.Permission.Print)
            If oContext.CommunityID > 0 Then
                Me.View.ShowSummary(IviewAccessResults.SummaryType.UserCommunityFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, Me.CurrentManager.GetCommunity(oContext.CommunityID).Name)
            Else
                Me.View.ShowSummary(IviewAccessResults.SummaryType.PortalUserFilter, Me.CurrentManager.GetPerson(oContext.UserID).SurnameAndName, "")
            End If
            Me.View.SetPrintUrl = Me.View.GetPrintUrl(oContext, Me.View.CurrentView)
            SetBackUrl(oContext)
            Me.View.AddActionViewReport(oContext.CommunityID, oContext.UserID)
        End Sub

        Private Function SetupViews() As Boolean
            Dim oList As IList(Of viewType) = Me.ListAvailableViews(viewType.MyPortalPresence, Me.ViewContext)
            Me.View.ViewAvailable = oList
            If oList.Contains(viewType.MyPortalPresence) Then
                Me.View.CurrentView = viewType.MyPortalPresence
            End If
            If IsNothing(oList) OrElse oList.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overloads Function GetUrlForTab(ByVal value As viewType) As String
            Return MyBase.GetUrlForTab(value, Me.ViewContext)
        End Function

    End Class
End Namespace