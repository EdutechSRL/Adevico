Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class OnLineUsersPresenter
        Inherits DomainPresenter


#Region "Standard"
        Private _ModuleID As Integer
        Private ReadOnly Property ModuleID As Integer
            Get
                If _ModuleID <= 0 Then
                    _ModuleID = BaseDomainManager.GetModuleID(ModuleOnLineUser.UniqueId)
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
        Public Overloads Property CurrentManager() As ManagerOnLineUsers
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerOnLineUsers)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IviewOnLineUsers
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerOnLineUsers(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewOnLineUsers)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerOnLineUsers(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region
        Private Function GetModule(idCommunity As Integer) As ModuleOnLineUser
            Dim moduleP As ModuleOnLineUser
            If idCommunity = 0 Then
                moduleP = ModuleOnLineUser.CreatePortalmodule(UserContext.UserTypeID)
            Else
                moduleP = New ModuleOnLineUser(BaseDomainManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID))
            End If
            Return moduleP
        End Function
        Private _OnLineContext As OnLineUsersContext
        Private ReadOnly Property StatContext() As OnLineUsersContext
            Get
                If IsNothing(_OnLineContext) Then
                    Dim oContext As OnLineUsersContext = Me.View.OnLineContext
                    Dim PersonID As Integer = oContext.UserID
                    If PersonID = 0 And Me.View.CurrentView = IviewUsageStatistic.viewType.Personal Then
                        PersonID = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    If oContext.Order = StatisticOrder.None Then
                        oContext.Order = StatisticOrder.LastAction
                        oContext.Ascending = False
                    End If
                    oContext.UserID = PersonID
                    Dim CommunityID As Integer = oContext.CommunityID
                    If CommunityID <= 0 AndAlso (Me.View.PreLoadedView = IviewUsageStatistic.viewType.CommunityUserOnLine) Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    If Equals(oContext.LastUpdate, New Date) Then
                        oContext.LastUpdate = Me.View.PreLoadedLastUpdate
                    End If

                    Me.View.OnLineContext = oContext
                    _OnLineContext = oContext
                End If
                Return _OnLineContext
            End Get
        End Property

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim p As litePerson = BaseDomainManager.GetLitePerson(UserContext.CurrentUserID)
                View.ShowIp = Not IsNothing(p) AndAlso (p.TypeID = UserTypeStandard.SysAdmin OrElse p.TypeID = UserTypeStandard.Administrator)
                Me.SetupViews()
                Me.ChangeView(Me.View.CurrentView)
            Else
                View.ShowIp = False
                Me.View.NoPermissionToAccess()
            End If
        End Sub
        Public Sub ChangeView(ByVal current As IviewUsageStatistic.viewType, Optional ByVal Reload As Boolean = True)
            If Me.View.CurrentView <> current Then
                Me.View.CurrentView = current
            End If
            If Reload Then
                Me.View.CurrentPageSize = Me.View.PreLoadedPageSize
                Me.View.NameSurnameField = Me.View.PreLoadedNameSurname
            End If

            Dim oContext As OnLineUsersContext = Me.StatContext.Clone
            Select Case Me.View.CurrentView
                Case IviewUsageStatistic.viewType.CommunityUserOnLine
                    oContext.CommunityID = Me.UserContext.WorkingCommunityID
                    Me.LoadUsers(oContext, ModuleOnLineUser.ActionType.ViewCommunityUsersOnLine)

                Case IviewUsageStatistic.viewType.UserOnLine
                    oContext.CommunityID = -1
                    Me.LoadUsers(oContext, ModuleOnLineUser.ActionType.ViewAllUsersOnLine)
                Case Else
                    Me.View.SendToView(Me.View.CurrentView)
            End Select

            Me.View.NavigationUrl(ViewPage.CurrentPage, Me.StatContext, current, Me.View.ReturnTo)
            Me.SetPreviousPage()
        End Sub
        Private Sub SetPreviousPage()
            Dim oCurrent As IviewUsageStatistic.viewType = Me.View.CurrentView
            Dim oPrevious As IviewUsageStatistic.viewType = Me.View.ReturnTo
            Dim oContext As New UsageContext
            Dim Url As String = ""
            If Not oPrevious = IviewUsageStatistic.viewType.None Then
                Url = GetUrlForTab(oPrevious, IviewUsageStatistic.viewType.None)
            End If
            Me.View.SetPreviousURL = Url
        End Sub

        Public Sub ChangePageSize()
            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            Me.ChangeView(Me.View.CurrentView, False)
        End Sub
        Public Sub FindUsers()
            Dim oContext As OnLineUsersContext = Me.StatContext.Clone


            Dim oPager As New PagerBase
            oPager.PageIndex = 0
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1
            oContext.CurrentPage = 0
            oContext.NameSurnameFilter = Me.View.NameSurnameField
            oContext.Order = StatisticOrder.LastAction
            oContext.Ascending = False

            Me.View.OnLineContext = oContext
            Me._OnLineContext = oContext
            Select Case Me.View.CurrentView
                Case IviewUsageStatistic.viewType.CommunityUserOnLine
                    oContext.CommunityID = Me.UserContext.WorkingCommunityID
                    Me.LoadUsers(oContext, ModuleOnLineUser.ActionType.FindUsersOnLine)
                Case IviewUsageStatistic.viewType.UserOnLine
                    oContext.CommunityID = -1
                    Me.LoadUsers(oContext, ModuleOnLineUser.ActionType.FindUsersOnLine)
                Case Else
                    Me.View.SendToView(Me.View.CurrentView)
            End Select

            Me.View.NavigationUrl(ViewPage.CurrentPage, oContext, Me.View.CurrentView, Me.View.ReturnTo)
        End Sub
        Public Sub LoadUsers(ByVal oContext As OnLineUsersContext, action As ModuleOnLineUser.ActionType)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1


            View.SendUserAction(IIf(oContext.CommunityID > 0, oContext.CommunityID, 0), ModuleID, action)
            Dim oSummary As New dtoSummary '= Me.CurrentManager.GetSummary(oContext)
            Dim oList As List(Of dtoOnLineUser) = Me.CurrentManager.RetrieveOnLineUsers(Me.StatContext, Me.View.CurrentPage, oPager, oSummary, EvaluateDetailsView(View.CurrentView, oContext.CommunityID), Me.View.TranslatedContext)


            Me.View.LoadSummary(oSummary, IIf(oContext.CommunityID > 0, IviewUsageStatistic.SummaryType.OnLineCommunity, IviewUsageStatistic.SummaryType.OnLineSystem))
            Me.View.Pager = oPager
            Me.View.LoadItems(oList, View.ShowIp)
        End Sub
        Private Sub SetupViews()
            Dim oSelectedView As IviewUsageStatistic.viewType = Me.View.PreLoadedView
            If oSelectedView = IviewUsageStatistic.viewType.None And Me.UserContext.CurrentCommunityID > 0 Then
                oSelectedView = IviewUsageStatistic.viewType.CommunityUserOnLine
            ElseIf oSelectedView = IviewUsageStatistic.viewType.None Then
                oSelectedView = IviewUsageStatistic.viewType.UserOnLine
            End If

            Dim oList As IList(Of IviewUsageStatistic.viewType) = Me.ListAvailableViews(oSelectedView)
            View.ViewAvailable = oList
            If oList.Contains(View.PreLoadedView) Then
                View.CurrentView = oSelectedView
            End If
        End Sub
        Private Function ListAvailableViews(ByVal StartView As IviewUsageStatistic.viewType) As IList(Of IviewUsageStatistic.viewType)
            Dim oList As New List(Of IviewUsageStatistic.viewType)

            ' VERIFICO LO STATUS DI VISUALIZZAZIONE IN CUI MI TROVO !
            ' SONO NEL GLOBALE !
            Dim isSystemPage As Boolean = (StartView = IviewUsageStatistic.viewType.Personal OrElse StartView = IviewUsageStatistic.viewType.SystemUsers OrElse StartView = IviewUsageStatistic.viewType.GenericSystem OrElse StartView = IviewUsageStatistic.viewType.UserOnLine)
            Dim isCommunityPage As Boolean = (Not isSystemPage AndAlso (StartView = IviewUsageStatistic.viewType.PersonalCommunity OrElse StartView = IviewUsageStatistic.viewType.CommunityUsers OrElse StartView = IviewUsageStatistic.viewType.GenericCommunity OrElse StartView = IviewUsageStatistic.viewType.CommunityUserOnLine))
            Dim isUserPage As Boolean = Not isSystemPage AndAlso Not isCommunityPage AndAlso StartView = IviewUsageStatistic.viewType.GenericUser

            If Me.AppContext.UserContext.CurrentUser.Id > 0 Then

                Dim UserTypeID As Integer = Me.AppContext.UserContext.UserTypeID

                If isSystemPage Then
                    If EvaluateViewPermission(Me.GetModule(0)) Then
                        oList.Add(IviewUsageStatistic.viewType.UserOnLine)
                    End If
                ElseIf isCommunityPage Then

                    Dim oPermission As ModuleOnLineUser = Me.GetModule(Me.StatContext.CommunityID)
                    If EvaluateViewPermission(oPermission) Then
                        oList.Add(IviewUsageStatistic.viewType.CommunityUserOnLine)
                    End If
                End If
            End If
            Return oList
        End Function

        Public Function GetUrlForTab(ByVal value As IviewUsageStatistic.viewType, ByVal ReturnTo As IviewUsageStatistic.viewType) As String
            Dim url = ""
            Dim oContext As New OnLineUsersContext With {.Ascending = False, .Order = StatisticOrder.UsageTime, .CurrentPage = 0}


            Select Case value
                Case IviewUsageStatistic.viewType.None

                Case IviewUsageStatistic.viewType.Personal
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    url = Me.View.GetNavigationUrl(ViewPage.System, oContext, value, ReturnTo)
                Case IviewUsageStatistic.viewType.SystemUsers
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.System, oContext, value, ReturnTo)

                Case IviewUsageStatistic.viewType.CommunityUsers
                    oContext.CommunityID = Me.StatContext.CommunityID
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.Community, oContext, value, ReturnTo)

                Case IviewUsageStatistic.viewType.GenericUser
                    oContext.CommunityID = Me.StatContext.CommunityID
                    oContext.UserID = Me.StatContext.UserID
                    oContext.Order = StatisticOrder.Community

                    url = Me.View.GetNavigationUrl(ViewPage.System, oContext, value, ReturnTo)
                Case IviewUsageStatistic.viewType.PersonalCommunity
                    oContext.CommunityID = Me.StatContext.CommunityID
                    oContext.UserID = Me.StatContext.UserID
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo)

                Case IviewUsageStatistic.viewType.GenericSystem
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.System, oContext, value, ReturnTo)

                Case IviewUsageStatistic.viewType.GenericCommunity
                    oContext.CommunityID = Me.StatContext.CommunityID
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.System, oContext, value, ReturnTo)
                Case IviewUsageStatistic.viewType.UserOnLine
                    oContext.CommunityID = -1
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.OnLineUsers, oContext, value, ReturnTo)
                Case IviewUsageStatistic.viewType.CommunityUserOnLine
                    oContext.CommunityID = Me.StatContext.CommunityID
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.OnLineUsers, oContext, value, ReturnTo)
                Case Else
                    url = ""
            End Select
            Return url
        End Function

        Private Function EvaluateViewPermission(ByVal oPermission As ModuleOnLineUser) As Boolean
            If oPermission Is Nothing Then
                Return False
            Else
                Return (oPermission.ViewUsersOnLineLowDetails OrElse oPermission.ViewUsersOnLine OrElse oPermission.ViewUsersWithAction OrElse oPermission.ViewUsersAndModuleOnLine OrElse oPermission.Administration)
            End If
        End Function

        Private Function EvaluateDetailsView(ByVal view As IviewUsageStatistic.viewType, ByVal IdCommunity As Integer) As ManagerOnLineUsers.Details
            Dim oPermission As ModuleOnLineUser = Nothing
            If view = IviewUsageStatistic.viewType.UserOnLine Then
                oPermission = Me.GetModule(0)
            ElseIf IdCommunity <= 0 Then
                oPermission = Me.GetModule(UserContext.CurrentCommunityID)
            Else
                oPermission = Me.GetModule(IdCommunity)
            End If

            With oPermission
                If .ViewUsersOnLineLowDetails AndAlso Not (.ViewUsersOnLine AndAlso .ViewUsersAndModuleOnLine AndAlso .ViewUsersWithAction) Then
                    Return ManagerOnLineUsers.Details.NoActionName Or ManagerOnLineUsers.Details.NoCommunityName Or ManagerOnLineUsers.Details.NoModuleName Or ManagerOnLineUsers.Details.NoUserName
                ElseIf .ViewUsersOnLine AndAlso Not (.ViewUsersAndModuleOnLine AndAlso .ViewUsersWithAction) Then
                    Return ManagerOnLineUsers.Details.NoActionName Or ManagerOnLineUsers.Details.NoModuleName
                ElseIf .ViewUsersAndModuleOnLine Or Not .ViewUsersWithAction Then
                    Return ManagerOnLineUsers.Details.NoActionName
                Else
                    Return ManagerOnLineUsers.Details.None
                End If
            End With
            Return ManagerOnLineUsers.Details.None
        End Function
    End Class
End Namespace