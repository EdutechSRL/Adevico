Imports lm.Comol.Core.DomainModel.Common
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Business
Imports lm.Modules.NotificationSystem.WSremoteManagement
Imports COL_BusinessLogic_v2.UCServices.Services_CommunityNews

Namespace Presentation
    Public Class CommunityDayNewsPresenter
        Inherits DomainPresenter

        Private _NewsContext As DayNewsContext
        Private ReadOnly Property ViewContext() As DayNewsContext
            Get
                If IsNothing(_NewsContext) Then
                    Dim oContext As DayNewsContext = Me.View.NewsContext
                    Dim PersonID As Integer = oContext.UserID
                    If PersonID = 0 Then
                        PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    oContext.UserID = PersonID
                    Dim CommunityID As Integer = oContext.CommunityID
                    If CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If

                    Me.View.NewsContext = oContext
                    _NewsContext = oContext
                End If
                Return _NewsContext
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
        Public Overloads ReadOnly Property View() As IViewCommunityDayNews
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunitynews(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewCommunityDayNews)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerCommunitynews(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous AndAlso (Me.ViewContext.CommunityID = 0 OrElse Me.CurrentManager.isCommunityMember(UserContext.CurrentUserID, ViewContext.CommunityID)) Then
                If Me.ViewContext.FromDay.Equals(DateTime.MinValue) Then
                    LoadCommunityNews()
                Else
                    Me.LoadAllNews()
                End If

                Me.View.SetPreviousURL = Me.View.GetNavigationUrl(Me.ViewContext, True)
            Else
                Me.View.AddActionNoPermission(Me.UserContext.CurrentCommunityID, 0, Me.View.PreLoadedPreviousDayView, Me.View.PreLoadedDay)
                Me.View.NoPermissionToAccess()
            End If
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

            If ViewContext.CommunityID = 0 Then
                Me.View.CommunityName = ""
            Else
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(ViewContext.CommunityID)
                If Not IsNothing(oCommunity) Then
                    Me.View.CommunityName = oCommunity.Name
                End If
            End If
            Me.View.Pager = oPager
            Me.View.LoadNotifications(DayName, oList)
            Me.View.NavigationUrl(ViewContext)
            Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewDayNews, oCurrent)
            Me.View.SetAllNewsRead(ViewContext.CommunityID, "")
        End Sub
        Private Sub LoadAllNews()
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPageIndex
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim CommunityID As Integer = -1
            Dim CommunityName As String = ""
            CommunityID = ViewContext.CommunityID
            If CommunityID > 0 Then
                Dim oCommunity As Community = Me.CurrentManager.GetCommunity(ViewContext.CommunityID)
                If Not IsNothing(oCommunity) Then
                    CommunityName = oCommunity.Name
                End If
            End If
            Me.View.CommunityName = CommunityName

            Dim oTmp As DayNewsContext = Me.ViewContext.Clone
            oTmp.PageIndex = Me.View.CurrentPageIndex
            oTmp.CommunityID = CommunityID

            Dim oList As List(Of dtoMultipleNews)

            oList = Me.CurrentManager.GetPortalNews(oTmp.FromDay, -1, ViewModeFiler.ByDate, UserContext.CurrentUserID, CommunityID, UserContext.Language.Id, oPager, Me.View.CurrentPageIndex, "Home")

            If oList.Count = 0 AndAlso oTmp.FromDay > DateTime.MinValue Then
                Me.View.ShowNoNewsFrom(oTmp.FromDay, CommunityName)
                Me.View.SetNoNewsForComunity(UserContext.CurrentUserID, CommunityID, Now)
            End If

            Me.View.LoadAllNews(oList)
            Me.View.Pager = oPager
            Me.View.NavigationUrl(ViewContext)
            Me.View.AddAction(ViewContext.CommunityID, ViewContext.UserID, ActionType.ViewLastNews, oTmp.FromDay)
            If oList.Count > 0 Then
                Me.View.SetAllNewsRead(ViewContext.CommunityID, Me.View.GetNavigationUrl(Me.ViewContext, False))
            Else
                Me.View.SetAllNewsRead(ViewContext.CommunityID, "")
            End If
        End Sub
    End Class
End Namespace