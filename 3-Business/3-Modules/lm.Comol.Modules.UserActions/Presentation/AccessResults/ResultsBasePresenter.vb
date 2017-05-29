Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Core.Business

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class ResultsBasePresenter
        Inherits DomainPresenter

#Region "PERMESSI"
        'Protected ReadOnly Property Permission(Optional ByVal CommunityID As Integer = 0) As UsageResults.DomainModel.ModuleUsageResult
        '    Get
        '        If IsNothing(_Permission) AndAlso CommunityID <= 0 Then
        '            _Permission = Me.View.ModulePermission
        '            Return _Permission
        '        ElseIf CommunityID > 0 Then
        '            _Permission = (From o In CommunitiesPermission Where o.ID = CommunityID Select o.Permissions).FirstOrDefault
        '            If IsNothing(_Permission) Then
        '                _Permission = New UsageResults.DomainModel.ModuleUsageResult
        '            End If
        '            Return _Permission
        '        Else
        '            Return _Permission
        '        End If
        '    End Get
        'End Property
        Private _permissions As Dictionary(Of Integer, ModuleUsageResult)

        Protected Friend ReadOnly Property GetPermission(idCommunity As Integer) As ModuleUsageResult
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
            If idCommunity <= 0 Then
                moduleP = ModuleUsageResult.CreatePortalmodule(UserContext.UserTypeID)
            Else
                moduleP = New ModuleUsageResult(BaseDomainManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, ModuleID))
            End If
            Return moduleP
        End Function
#End Region

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
        Public Overloads Property CurrentManager() As UsageResults.BusinessLogic.ManagerUsageResults
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As UsageResults.BusinessLogic.ManagerUsageResults)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As iViewBaseAccessResult
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As iViewBaseAccessResult)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
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
                If StartView = viewType.OtherUserCommunityList Then
                    Dim oPermission As UsageResults.DomainModel.ModuleUsageResult = Me.GetPermission(oContext.CommunityID) 'Me.Permission
                    If oPermission.Administration OrElse oPermission.PortalReports OrElse oPermission.ViewCommunityReports Then
                        oList.Add(viewType.OtherUserCommunityList)
                    End If
                    'If oContext.CommunityID <= 0 Then
                    '    If UserTypeID = Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = Main.TipoPersonaStandard.SysAdmin Then
                    '        oList.Add(viewType.OtherUserCommunityList)
                    '    End If
                    'Else

                    'End If
                ElseIf StartView = viewType.OtherUserPresence Then
                    Dim oPermission As UsageResults.DomainModel.ModuleUsageResult = Me.GetPermission(oContext.CommunityID) 'Me.Permission
                    If oPermission.Administration OrElse oPermission.PortalReports OrElse oPermission.ViewCommunityReports Then
                        oList.Add(viewType.OtherUserPresence)
                    End If
                    'If oContext.CommunityID <= 0 Then
                    '    If UserTypeID = Main.TipoPersonaStandard.Amministrativo OrElse UserTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse UserTypeID = Main.TipoPersonaStandard.SysAdmin Then
                    '        oList.Add(viewType.OtherUserPresence)
                    '    End If
                    'Else
                    '    Dim oPermission As UsageResults.DomainModel.ModuleUsageResult = Me.Permission
                    'End If
                ElseIf isSystemPage Then
                    oList.Add(viewType.MyPortalPresence)
                    oList.Add(viewType.MyCommunitiesPresence)
                    If UserTypeID = UserTypeStandard.Administrative OrElse UserTypeID = UserTypeStandard.Administrator OrElse UserTypeID = UserTypeStandard.SysAdmin Then
                        oList.Add(viewType.UsersPortalPresence)
                        oList.Add(viewType.BetweenDateUsersPortal)
                    End If
                ElseIf isCommunityPage Then
                    Dim oPermission As ModuleUsageResult = Me.GetPermission(oContext.CommunityID)

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
            Dim oContext As New ResultContextBase With {.Ascending = True, .Order = DomainModel.ResultsOrder.Day, .CurrentPage = 0, .FromView = viewType.None}


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
                Case viewType.OtherUserCommunityList
                    oContext.Order = DomainModel.ResultsOrder.Community
                    oContext.UserID = oViewContext.UserID
                    oContext.CommunityID = 0
                Case viewType.OtherUserPresence
                    oContext.Order = DomainModel.ResultsOrder.Day
                    oContext.UserID = oViewContext.UserID
                    oContext.CommunityID = oViewContext.CommunityID
                Case Else
                    url = ""
            End Select
            Return url
        End Function

    End Class
End Namespace