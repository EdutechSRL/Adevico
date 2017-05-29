Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UserActions.BusinessLogic

Imports COL_BusinessLogic_v2
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.UserActions.Presentation
    Public Class UsageGlobalPresenter
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
        Public Overloads ReadOnly Property View() As IviewUsageStatistic
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewUsageStatistic)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerUsageStatistic(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region
        Private _StatContext As UsageContext
        Private ReadOnly Property StatContext() As UsageContext
            Get
                If IsNothing(_StatContext) Then
                    Dim oContext As UsageContext = Me.View.StatisticContext
                    Dim idPerson As Integer = oContext.UserID
                    If idPerson = 0 AndAlso (Me.View.CurrentView = IviewUsageStatistic.viewType.PersonalCommunity OrElse Me.View.CurrentView = IviewUsageStatistic.viewType.Personal) Then
                        idPerson = Me.AppContext.UserContext.CurrentUser.Id
                    End If
                    If oContext.Order = StatisticOrder.None Then
                        oContext.Order = StatisticOrder.UsageTime
                        oContext.Ascending = False
                    End If
                    oContext.UserID = idPerson
                    Dim CommunityID As Integer = oContext.CommunityID
                    If CommunityID <= 0 AndAlso (Me.View.CurrentView = IviewUsageStatistic.viewType.PersonalCommunity OrElse Me.View.CurrentView = IviewUsageStatistic.viewType.CommunityUsers) Then
                        CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.CommunityID = CommunityID
                    Me.View.StatisticContext = oContext
                    _StatContext = oContext
                End If
                Return _StatContext
            End Get
        End Property

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                If Me.SetupViews() Then
                    Me.ChangeView(Me.View.CurrentView)
                Else
                    Me.View.NoPermissionToAccess()
                End If
            Else
                Me.View.NoPermissionToAccess()
            End If
        End Sub
        Public Sub ChangeView(ByVal current As IviewUsageStatistic.viewType)
            If Me.View.CurrentView <> current Then
                Me.View.CurrentView = current
            End If

            Me.View.SetFirstColumHeader(current)

            Dim oContext As UsageContext = Me.StatContext.Clone
            Select Case current
                Case IviewUsageStatistic.viewType.Personal
                    Me.LoadGlobal(oContext)
                Case IviewUsageStatistic.viewType.SystemUsers
                    Me.LoadGlobalUsers(oContext)
                Case IviewUsageStatistic.viewType.GenericCommunity
                    oContext.UserID = 0
                    If oContext.CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    Me.LoadCommunityUser(oContext)
                Case IviewUsageStatistic.viewType.CommunityUsers
                    oContext.UserID = 0
                    If oContext.CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    Me.LoadCommunityUsers(oContext)
                Case IviewUsageStatistic.viewType.GenericSystem
                    oContext.UserID = 0
                    Me.LoadGlobal(oContext)
                Case IviewUsageStatistic.viewType.GenericUser
                    If Me.View.StatisticContext.CommunityID <= 0 Then
                        Me.LoadGlobal(oContext)
                    Else
                        Me.LoadCommunityUser(oContext)
                    End If
                Case IviewUsageStatistic.viewType.PersonalCommunity
                    If oContext.CommunityID <= 0 Then
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    Me.LoadCommunityUser(oContext)
            End Select
            Me.View.NavigationUrl(ViewPage.CurrentPage, Me.StatContext, current, Me.View.ReturnTo)
            Me.SetPreviousPage()
        End Sub
        Private Sub SetPreviousPage()
            Dim oCurrent As IviewUsageStatistic.viewType = Me.View.CurrentView
            Dim backTo As IviewUsageStatistic.viewType = Me.View.ReturnTo
            Dim oContext As New UsageContext
            Dim Url As String = ""

            If backTo = IviewUsageStatistic.viewType.None Then
                backTo = View.StartFrom
            End If
            If backTo = oCurrent Then
                backTo = IviewUsageStatistic.viewType.None
            End If
            If Not backTo = IviewUsageStatistic.viewType.None Then
                Url = GetUrlForTab(backTo, IviewUsageStatistic.viewType.None)
            End If
            Me.View.SetPreviousURL(backTo) = Url
        End Sub


        Public Sub LoadGlobal(ByVal oUsage As UsageContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            oSummary.NavigateTo = Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Month)
            If oUsage.UserID > 0 Then
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Personal)
            Else
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Portal)
            End If


            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetPortalStatistics("", oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)


            Me.View.Pager = oPager
            For Each o In oStatistic.Items
                Dim oContext As New UsageContext() With {.UserID = oUsage.UserID, .CommunityID = o.ID, .Order = StatisticOrder.UsageTime, .Ascending = False, .ModuleID = -2, .CurrentPage = 0}
                o.NavigateTo = Me.View.GetNavigationUrl(ViewPage.Community, oContext, IviewUsageStatistic.viewType.PersonalCommunity, View.CurrentView, View.StartFrom)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
        End Sub
        Public Sub LoadGlobalUsers(ByVal oUsage As UsageContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetPortalUsersStatistics(oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)

            Dim oSummaryContext As UsageContext = oUsage.Clone
            oSummaryContext.UserID = -1
            oSummaryContext.CommunityID = -1
            oSummary.NavigateTo = Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oSummaryContext, Me.View.CurrentView, IViewUsageDetails.viewType.Month)

            Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Portal)

            Me.View.Pager = oPager
            For Each o In oStatistic.Items
                Dim oContext As New UsageContext() With {.UserID = o.ID, .CommunityID = oUsage.CommunityID, .Order = StatisticOrder.UsageTime, .Ascending = False, .ModuleID = -2, .CurrentPage = 0}
                o.NavigateTo = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, IviewUsageStatistic.viewType.GenericUser, IviewUsageStatistic.viewType.SystemUsers, View.StartFrom)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
        End Sub
        Public Sub LoadCommunityUsers(ByVal oUsage As UsageContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetCommunityUsersStatistics(oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)
            oSummary.NavigateTo = Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Month)

            Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Community)
            Me.View.Pager = oPager
            For Each o In oStatistic.Items
                Dim oContext As New UsageContext() With {.UserID = o.ID, .CommunityID = Me.StatContext.CommunityID, .Order = StatisticOrder.UsageTime, .Ascending = False, .ModuleID = -2, .CurrentPage = 0}
                o.NavigateTo = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, IviewUsageStatistic.viewType.GenericUser, IviewUsageStatistic.viewType.CommunityUsers, View.StartFrom)
            Next
            Me.View.LoadItems(oStatistic, Me.StatContext, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
        End Sub
        Public Sub LoadCommunityUser(ByVal oUsage As UsageContext)
            Dim oPager As New PagerBase
            oPager.PageIndex = Me.View.CurrentPage
            oPager.PageSize = Me.View.CurrentPageSize
            oPager.Count -= 1

            Dim oSummaryContext As UsageContext = oUsage.Clone
            Dim oSummary As dtoSummary = Me.CurrentManager.GetSummary(oUsage, New Date(1900, 1, 1), Date.MaxValue)
            oSummary.NavigateTo = Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oSummaryContext, Me.View.CurrentView, IViewUsageDetails.viewType.Month)

            Dim oStatistic As dtoStatistic = Me.CurrentManager.GetUserModuleStatistics(oUsage, New Date(1900, 1, 1), Date.MaxValue, Me.View.CurrentPage, oPager)
            If oUsage.UserID > 0 Then
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.PersonalCommunity)
            Else
                Me.View.LoadSummary(oSummary, IviewUsageStatistic.SummaryType.Community)
            End If

            Me.View.Pager = oPager
            For Each o In oStatistic.Items
                Dim oContext As New UsageContext() With {.UserID = oUsage.UserID, .CommunityID = oUsage.CommunityID, .Order = StatisticOrder.ModuleName, .Ascending = True, .ModuleID = o.ID, .CurrentPage = -100}
                o.NavigateTo = Me.View.NavigationUrlToDetails(ViewPage.TimeDetails, oContext, Me.View.CurrentView, IViewUsageDetails.viewType.Week)
                ' Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, IviewUsageStatistic.viewType.GenericUser, IviewUsageStatistic.viewType.CommunityUsers)
            Next
            Me.View.LoadItems(oStatistic, oUsage, Me.View.CurrentView, IViewUsageDetails.viewType.Day)
        End Sub


        Private Function SetupViews() As Boolean
            Dim currentView As IviewUsageStatistic.viewType = Me.View.PreLoadedView
            If currentView = IviewUsageStatistic.viewType.None And Me.UserContext.CurrentCommunityID > 0 Then
                currentView = IviewUsageStatistic.viewType.PersonalCommunity
            ElseIf currentView = IviewUsageStatistic.viewType.None Then
                currentView = IviewUsageStatistic.viewType.Personal
            End If

            Me.View.CurrentView = currentView

            Dim views As IList(Of IviewUsageStatistic.viewType) = Me.ListAvailableViews(currentView)
            Me.View.ViewAvailable = views
            If views.Contains(Me.View.PreLoadedView) Then
                Me.View.CurrentView = currentView
            ElseIf views.Count > 0 Then
                Me.View.CurrentView = views(0)
            End If
            If IsNothing(views) OrElse views.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Function
        Private Function ListAvailableViews(ByVal StartView As IviewUsageStatistic.viewType) As IList(Of IviewUsageStatistic.viewType)
            Dim oList As New List(Of IviewUsageStatistic.viewType)

            ' VERIFICO LO STATUS DI VISUALIZZAZIONE IN CUI MI TROVO !
            ' SONO NEL GLOBALE !
            Dim isSystemPage As Boolean = (StartView = IviewUsageStatistic.viewType.Personal OrElse StartView = IviewUsageStatistic.viewType.SystemUsers OrElse StartView = IviewUsageStatistic.viewType.GenericSystem)
            Dim isCommunityPage As Boolean = (Not isSystemPage AndAlso (StartView = IviewUsageStatistic.viewType.PersonalCommunity OrElse StartView = IviewUsageStatistic.viewType.CommunityUsers OrElse StartView = IviewUsageStatistic.viewType.GenericCommunity))
            Dim isUserPage As Boolean = Not isSystemPage AndAlso Not isCommunityPage AndAlso StartView = IviewUsageStatistic.viewType.GenericUser

            If Me.AppContext.UserContext.CurrentUser.Id > 0 Then
                Dim UserTypeID As Integer = Me.AppContext.UserContext.UserTypeID
                If isSystemPage Then
                    oList.Add(IviewUsageStatistic.viewType.Personal)
                    oList.Add(IviewUsageStatistic.viewType.GenericSystem)
                    If UserTypeID = UserTypeStandard.Administrative OrElse UserTypeID = UserTypeStandard.Administrator OrElse UserTypeID = UserTypeStandard.SysAdmin Then
                        oList.Add(IviewUsageStatistic.viewType.SystemUsers)
                        '	oList.Add(IviewUsageStatistic.viewType.UserOnLine)
                    End If
                ElseIf isCommunityPage Then
                    Dim oPermission As ModuleStatistics = Me.GetModule(Me.StatContext.CommunityID)
                    If Me.StatContext.CommunityID = 0 AndAlso Me.StatContext.UserID > 0 Then
                        If oPermission.ListSelfStatistic Then
                            oList.Add(IviewUsageStatistic.viewType.PersonalCommunity)
                        End If
                    Else
                        'If View.StartFrom = IviewUsageStatistic.viewType.SystemUsers Then
                        '    If oPermission.Administration OrElse oPermission.ListOtherStatistic orelse (Me.StatContext.UserID = Me.AppContext.UserContext.CurrentUser.Id OrElse UserTypeID = UserTypeStandard.Administrator OrElse UserTypeID = UserTypeStandard.SysAdmin Then
                        '        oList.Add(IviewUsageStatistic.viewType.GenericUser)
                        '    End If
                        'Else
                        If oPermission.ListSelfStatistic Then
                            oList.Add(IviewUsageStatistic.viewType.PersonalCommunity)
                        End If
                        If oPermission.ViewGenericOtherStatistic OrElse oPermission.ListOtherStatistic OrElse oPermission.Administration Then
                            oList.Add(IviewUsageStatistic.viewType.GenericCommunity)
                            '	oList.Add(IviewUsageStatistic.viewType.CommunityUserOnLine)
                        End If
                        If oPermission.Administration OrElse oPermission.ListOtherStatistic Then
                            oList.Add(IviewUsageStatistic.viewType.CommunityUsers)
                        End If
                        'End If
                    End If
                 
                ElseIf isUserPage Then
                    Dim oPermission As New ModuleStatistics
                    If Me.StatContext.CommunityID > 0 Then
                        oPermission = Me.GetModule(Me.StatContext.CommunityID)
                    Else
                        oPermission.ListOtherStatistic = (Me.StatContext.UserID = Me.AppContext.UserContext.CurrentUser.Id OrElse UserTypeID = UserTypeStandard.Administrator OrElse UserTypeID = UserTypeStandard.SysAdmin)
                    End If

                    If oPermission.ListOtherStatistic Then
                        oList.Add(IviewUsageStatistic.viewType.GenericUser)
                    End If
                End If
            End If
            Return oList
        End Function

        Private Function GetModule(idCommunity As Integer) As ModuleStatistics
            Dim moduleP As ModuleStatistics = ModuleStatistics.CreatePortalmodule(UserContext.UserTypeID)
            If idCommunity > 0 Then
                If BaseDomainManager.HasActiveSubscription(UserContext.CurrentUserID, idCommunity) Then
                    Dim moduleC As New ModuleStatistics(BaseDomainManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID))

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

        Public Function GetUrlForTab(ByVal value As IviewUsageStatistic.viewType, ByVal ReturnTo As IviewUsageStatistic.viewType) As String
            Dim url = ""
            Dim oContext As New UsageContext With {.Ascending = False, .Order = StatisticOrder.UsageTime, .CurrentPage = 0}
            Dim startFrom As IviewUsageStatistic.viewType = View.StartFrom

            Select Case value
                Case IviewUsageStatistic.viewType.None

                Case IviewUsageStatistic.viewType.Personal
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, value)
                Case IviewUsageStatistic.viewType.SystemUsers
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, value)

                Case IviewUsageStatistic.viewType.CommunityUsers
                    If Me.StatContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.StatContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None OrElse startFrom = IviewUsageStatistic.viewType.PersonalCommunity OrElse startFrom = IviewUsageStatistic.viewType.GenericCommunity, value, View.StartFrom))

                Case IviewUsageStatistic.viewType.GenericUser
                    'oContext.CommunityID = Me.StatContext.CommunityID
                    If Me.StatContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.StatContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = Me.StatContext.UserID
                    If oContext.CommunityID <= 0 Then
                        oContext.Order = StatisticOrder.UsageTime
                    Else
                        oContext.Order = StatisticOrder.Community
                    End If

                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None, value, View.StartFrom))
                Case IviewUsageStatistic.viewType.PersonalCommunity
                    If Me.StatContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.StatContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = Me.StatContext.UserID
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None OrElse startFrom = IviewUsageStatistic.viewType.GenericCommunity OrElse startFrom = IviewUsageStatistic.viewType.CommunityUsers, value, View.StartFrom))

                Case IviewUsageStatistic.viewType.GenericSystem
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, value)

                Case IviewUsageStatistic.viewType.GenericCommunity
                    'oContext.CommunityID = Me.StatContext.CommunityID
                    If Me.StatContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.StatContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(ViewPage.CurrentPage, oContext, value, ReturnTo, IIf(startFrom = IviewUsageStatistic.viewType.None OrElse startFrom = IviewUsageStatistic.viewType.PersonalCommunity OrElse startFrom = IviewUsageStatistic.viewType.CommunityUsers, value, View.StartFrom))
                Case IviewUsageStatistic.viewType.UserOnLine
                    oContext.CommunityID = -1
                    oContext.UserID = 0
                    oContext.Order = StatisticOrder.LastAction
                    oContext.Ascending = False
                    url = Me.View.GetNavigationUrl(ViewPage.OnLineUsers, oContext, value, ReturnTo, IIf(View.StartFrom = IviewUsageStatistic.viewType.None, value, View.StartFrom))
                Case IviewUsageStatistic.viewType.CommunityUserOnLine
                    If Me.StatContext.CommunityID > 0 Then
                        oContext.CommunityID = Me.StatContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = 0
                    oContext.Order = StatisticOrder.LastAction
                    oContext.Ascending = False
                    url = Me.View.GetNavigationUrl(ViewPage.OnLineUsers, oContext, value, ReturnTo, IIf(View.StartFrom = IviewUsageStatistic.viewType.None, value, View.StartFrom))
                Case Else
                    url = ""
            End Select
            Return url
        End Function
    End Class
End Namespace