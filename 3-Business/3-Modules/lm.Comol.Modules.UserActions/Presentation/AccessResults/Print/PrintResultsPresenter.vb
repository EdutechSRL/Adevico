Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Core.Business


Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Class PrintResultsPresenter
        Inherits DomainPresenter

#Region "PERMESSI"
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
        Private _Context As ResultContextBase

        Private ReadOnly Property Context() As ResultContextBase
            Get
                If IsNothing(_Context) Then
                    _Context = Me.View.ContextBase
                    If _Context.Order = lm.Comol.Modules.AccessResults.DomainModel.ResultsOrder.None Then
                        _Context.Order = lm.Comol.Modules.AccessResults.DomainModel.ResultsOrder.Day
                        _Context.Ascending = True
                    End If
                End If
                Return _Context
            End Get
        End Property
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
        Public Overloads ReadOnly Property View() As IviewPrintResults
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewPrintResults)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
            BaseDomainManager = New BaseModuleManager(MyBase.AppContext)
        End Sub
#End Region

        'Public Function HasPermission() As Boolean
        '    Dim oPermission As lm.Comol.Modules.UsageResults.DomainModel.ModuleUsageResult = Me.Permission(Context.CommunityID)
        '    Dim iResponse As Boolean = False
        '    Select Case Me.View.CurrentView
        '        Case IviewUsageResults.viewType.BetweenDateUsersCommunity
        '            iResponse = (oPermission.Administration OrElse oPermission.ViewMyReport)

        '        Case IviewUsageResults.viewType.CurrentCommunityPresence
        '            iResponse = (Context.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.Administration OrElse oPermission.ViewMyReport)

        '        Case IviewUsageResults.viewType.MyCommunitiesPresence
        '            iResponse = (Context.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.Administration OrElse oPermission.ViewMyReport)

        '        Case IviewUsageResults.viewType.MyPortalPresence
        '            iResponse = Context.UserID = Me.UserContext.CurrentUser.Id

        '        Case IviewUsageResults.viewType.UsersCurrentCommunityPresence
        '            iResponse = (oPermission.Administration OrElse oPermission.ViewCommunityReports)

        '        Case IviewUsageResults.viewType.UsersPortalPresence
        '            iResponse = (oPermission.Administration OrElse oPermission.PortalReports)
        '        Case IviewUsageResults.viewType.BetweenDateUsersPortal
        '            iResponse = (oPermission.Administration OrElse oPermission.PortalReports)
        '    End Select
        '    If iResponse Then
        '        Me.View.AddActionNoPermission(Context.CommunityID, Context.UserID)
        '    End If
        '    Return iResponse
        'End Function

        Public Function HasPermission() As Boolean
            Dim iResponse As Boolean = False
            Dim oPermission As UsageResults.DomainModel.ModuleUsageResult
            Dim oContext As ResultContextBase = Me.Context
            Dim oView As viewType = Me.View.CurrentView
            Select Case oView
                Case viewType.MyPortalPresence
                    oPermission = Me.GetPermission(0)
                    iResponse = (oContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.ViewMyReport OrElse oPermission.Administration)
                Case viewType.BetweenDateUsersPortal
                    oPermission = Me.GetPermission(0)
                    iResponse = (oPermission.Administration OrElse oPermission.PortalReports)
                Case viewType.BetweenDateUsersCommunity
                    oPermission = Me.GetPermission(oContext.CommunityID)
                    iResponse = (oPermission.Administration OrElse oPermission.ViewCommunityReports)
                Case viewType.CurrentCommunityPresence
                    oPermission = Me.GetPermission(oContext.CommunityID)
                    iResponse = (oContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.Administration OrElse oPermission.ViewCommunityReports OrElse oPermission.ViewMyReport)
                Case viewType.OtherUserPresence
                    If oContext.FromView = viewType.BetweenDateUsersPortal OrElse oContext.FromView = viewType.UsersPortalPresence Then
                        oPermission = Me.GetPermission(0)
                        iResponse = (oPermission.Administration OrElse oPermission.PortalReports)
                    Else
                        oPermission = Me.GetPermission(oContext.CommunityID)
                        iResponse = (oPermission.Administration OrElse oPermission.ViewCommunityReports) OrElse ((oContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso oPermission.ViewMyReport)
                    End If
                Case viewType.MyPortalPresence
                    oPermission = Me.GetPermission(0)
                    iResponse = (oContext.UserID = Me.UserContext.CurrentUser.Id) AndAlso (oPermission.ViewMyReport OrElse oPermission.Administration)

            End Select
            If Not iResponse Then
                Me.View.AddActionNoPermission(Context.CommunityID, Context.UserID)
            End If
            Return iResponse
        End Function

        Public Sub LoadItems()
            Me.View.PrintedBy = UserContext.CurrentUser.SurnameAndName
            Me.View.PrintedOn = FormatDateTime(Now, DateFormat.GeneralDate)
            Dim oContext As ResultContextBase = Context
            Dim CurrentView As viewType = Me.View.CurrentView
            Dim oResults As List(Of lm.Comol.Modules.UsageResults.DomainModel.dtoAccessResult)

            Dim oTotalTime As New TimeSpan
            If CurrentView = viewType.BetweenDateUsersCommunity OrElse CurrentView = viewType.BetweenDateUsersPortal Then
                oResults = Me.CurrentManager.GetPrintResultsBetweenDate(oTotalTime, oContext, oContext.FromDate, oContext.ToDate)
            Else
                oResults = Me.CurrentManager.GetPrintResults(oTotalTime, oContext, oContext.FromDate, oContext.ToDate)
            End If

            Me.View.LoadItems(oTotalTime, oResults)
            Me.View.AddActionPrintReport(oContext.CommunityID, oContext.UserID)
        End Sub
    End Class
End Namespace