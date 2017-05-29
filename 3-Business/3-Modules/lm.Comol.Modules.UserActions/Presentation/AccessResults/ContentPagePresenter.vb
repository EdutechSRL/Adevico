Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation


Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class ContentPagePresenter
        Inherits ResultsBasePresenter


#Region "Standard"
        Public Overloads Property CurrentManager() As UsageResults.BusinessLogic.ManagerUsageResults
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As UsageResults.BusinessLogic.ManagerUsageResults)
                _CurrentManager = value
            End Set
        End Property
#End Region

#Region "Standard"
        Public Overloads ReadOnly Property View() As IviewContentPageAccessResults
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewContentPageAccessResults)
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
                        PersonID = GetUserIDByView()
                    End If
                    oContext.UserID = PersonID
                    Dim CommunityID As Integer = oContext.CommunityID
                    If CommunityID <= 0 Then
                        CommunityID = Me.GetCommunityID
                    End If
                    oContext.CommunityID = CommunityID
                    Me.View.ResultsContext = oContext
                    _ContexFromView = oContext
                End If
                Return _ContexFromView
            End Get
        End Property


        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Me.View.InitContent(Me.ViewContext)
            Else
                Me.View.NoPermissionToAccess()
            End If
        End Sub

        Private Function GetUserIDByView() As Integer
            Dim PersonID As Integer = 0
            Dim oView As viewType = Me.View.CurrentView
            If oView = viewType.MyPortalPresence OrElse oView = viewType.MyCommunitiesPresence OrElse oView = viewType.CurrentCommunityPresence Then
                PersonID = Me.AppContext.UserContext.CurrentUser.Id
            End If

            Return PersonID
        End Function
        Private Function GetCommunityID() As Integer
            Dim CommunityID As Integer = 0
            Dim oView As viewType = Me.View.CurrentView

            If oView = viewType.CurrentCommunityPresence OrElse oView = viewType.UsersCurrentCommunityPresence OrElse oView = viewType.BetweenDateUsersCommunity Then
                CommunityID = Me.AppContext.UserContext.CurrentCommunityID
            End If

            Return CommunityID
        End Function
    End Class
End Namespace