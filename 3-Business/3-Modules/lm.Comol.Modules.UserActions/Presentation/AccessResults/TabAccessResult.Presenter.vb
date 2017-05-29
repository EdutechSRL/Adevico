Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation

'Imports lm.Comol.Modules.AccessResults.BusinessLogic

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Class TabAccessResultPresenter
        Inherits ResultsBasePresenter


#Region "Standard"
        Public Overloads ReadOnly Property View() As IviewTabAccessResult
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IviewTabAccessResult)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New UsageResults.BusinessLogic.ManagerUsageResults(MyBase.AppContext)
        End Sub
#End Region

        Private ReadOnly Property ViewContext() As ResultContextBase
            Get
                If IsNothing(_ContexFromView) Then
                    _ContexFromView = Me.View.ResultsContext
                End If
                Return _ContexFromView
            End Get
        End Property

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                If Me.SetupViews() Then
                    Me.View.LoadSubView()
                Else
                    Me.View.NoPermissionToAccess(Me.UserContext.CurrentCommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
                End If
            Else
               Me.View.NoPermissionToAccess(Me.UserContext.CurrentCommunityID, Me.UserContext.CurrentUser.Id, Me.ViewContext.UserID)
            End If
        End Sub

        Private Function SetupViews() As Boolean
            Dim oSelectedView As viewType = Me.View.CurrentView
            If oSelectedView = viewType.None And Me.UserContext.CurrentCommunityID > 0 Then
                oSelectedView = viewType.CurrentCommunityPresence
            ElseIf oSelectedView = viewType.None Then
                oSelectedView = viewType.MyPortalPresence
            End If

            Dim oList As IList(Of viewType) = Me.ListAvailableViews(oSelectedView, Me.ViewContext)
            Me.View.ViewAvailable = oList
            If oList.Contains(Me.View.CurrentView) Then
                Me.View.CurrentView = oSelectedView
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