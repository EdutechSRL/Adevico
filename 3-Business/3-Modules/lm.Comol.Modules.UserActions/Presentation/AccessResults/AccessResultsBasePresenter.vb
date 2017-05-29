Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.UsageResults.DomainModel

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class AccessResultsBasePresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        Private _Permission As UsageResults.DomainModel.ModuleUsageResult
        Private _CommunitiesPermission As IList(Of ModuleCommunityPermission(Of UsageResults.DomainModel.ModuleUsageResult))
        Protected ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As UsageResults.DomainModel.ModuleUsageResult
            Get
                If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
                    _Permission = Me.View.ModulePermission
                    Return _Permission
                ElseIf CommunityID > 0 Then
                    _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
                    If IsNothing(_Permission) Then
                        _Permission = New UsageResults.DomainModel.ModuleUsageResult
                    End If
                    Return _Permission
                Else
                    Return _Permission
                End If
            End Get
        End Property
        Protected ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of UsageResults.DomainModel.ModuleUsageResult))
            Get
                If IsNothing(_CommunitiesPermission) Then
                    _CommunitiesPermission = Me.View.CommunitiesPermission()
                End If
                Return _CommunitiesPermission
            End Get
        End Property
#End Region

#Region "Standard"
        Public Overloads Property CurrentManager() As UsageResults.BusinessLogic.ManagerUsageResults
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As UsageResults.BusinessLogic.ManagerUsageResults)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IviewAccessResults
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewAccessResults)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
#End Region
        Protected _ContexFromView As ResultContextBase



        Protected Function ListAvailableViews(ByVal StartView As viewType, ByVal oContext As ResultContextBase) As IList(Of viewType)
            Dim oList As New List(Of viewType)

            ' VERIFICO LO STATUS DI VISUALIZZAZIONE IN CUI MI TROVO !
            ' SONO NEL GLOBALE !
            Dim isSystemPage As Boolean = (StartView = viewType.MyPortalPresence OrElse StartView = viewType.MyCommunitiesPresence OrElse StartView = viewType.UsersPortalPresence OrElse StartView = viewType.BetweenDateUsersPortal)
            Dim isCommunityPage As Boolean = (Not isSystemPage AndAlso (StartView = viewType.CurrentCommunityPresence OrElse StartView = viewType.UsersCurrentCommunityPresence OrElse StartView = viewType.BetweenDateUsersCommunity))

            If Me.AppContext.UserContext.CurrentUser.Id > 0 Then
                Dim UserTypeID As Integer = Me.AppContext.UserContext.UserTypeID
                If isSystemPage Then
                    oList.Add(viewType.MyPortalPresence)
                    oList.Add(viewType.MyCommunitiesPresence)
                    If UserTypeID = UserTypeStandard.Administrative OrElse UserTypeID = UserTypeStandard.Administrator OrElse UserTypeID = UserTypeStandard.SysAdmin Then
                        oList.Add(viewType.UsersPortalPresence)
                        oList.Add(viewType.BetweenDateUsersPortal)
                    End If
                ElseIf isCommunityPage Then
                    Dim oPermission As ModuleUsageResult = Me.Permission(oContext.CommunityID)

                    If oPermission.ViewMyReport Then
                        oList.Add(viewType.CurrentCommunityPresence)
                    End If
                    If oPermission.Administration OrElse oPermission.ViewCommunityReports Then
                        oList.Add(viewType.UsersCurrentCommunityPresence)
                        oList.Add(viewType.BetweenDateUsersCommunity)
                    End If
                End If
            End If
            Return oList
        End Function


        Public Function GetUrlForTab(ByVal value As viewType, ByVal oViewContext As ResultContextBase) As String
            Dim url = ""
            Dim oContext As New ResultContextBase With {.Ascending = True, .Order = DomainModel.ResultsOrder.Hour, .CurrentPage = 0, .FromView = viewType.None}


            Select Case value
                Case viewType.None
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    If oViewContext.CommunityID > 0 Then
                        oContext.CommunityID = oViewContext.CommunityID
                    End If
                    url = Me.View.GetNavigationUrl(oContext, value)
                Case viewType.MyPortalPresence
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    url = Me.View.GetNavigationUrl(oContext, value)
                Case viewType.UsersPortalPresence
                    oContext.UserID = 0
                    oContext.Order = DomainModel.ResultsOrder.Owner
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case viewType.UsersCurrentCommunityPresence
                    oContext.Order = DomainModel.ResultsOrder.Owner
                    If oViewContext.CommunityID > 0 Then
                        oContext.CommunityID = oViewContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(oContext, value)


                Case viewType.CurrentCommunityPresence
                    If oViewContext.CommunityID > 0 Then
                        oContext.CommunityID = oViewContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    oContext.UserID = oViewContext.UserID
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case viewType.MyCommunitiesPresence
                    oContext.Order = DomainModel.ResultsOrder.Community
                    oContext.UserID = Me.UserContext.CurrentUser.Id
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case viewType.BetweenDateUsersPortal
                    oContext.Order = DomainModel.ResultsOrder.Owner
                    oContext.UserID = 0
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case viewType.BetweenDateUsersCommunity
                    oContext.Order = DomainModel.ResultsOrder.Owner
                    oContext.UserID = 0
                    If oViewContext.CommunityID > 0 Then
                        oContext.CommunityID = oViewContext.CommunityID
                    Else
                        oContext.CommunityID = Me.AppContext.UserContext.CurrentCommunityID
                    End If
                    url = Me.View.GetNavigationUrl(oContext, value)

                Case Else
                    url = ""
            End Select
            Return url
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

            If Not IsNothing(oTemp) Then
                Me.View.SetPreviousURL = Me.View.GetNavigationUrl(oTemp, oCurrentView)
            End If
        End Sub
    End Class
End Namespace