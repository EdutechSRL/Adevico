Imports lm.Comol.Core.DomainModel.Common
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Business
Imports lm.Modules.NotificationSystem.WSremoteManagement

Namespace Presentation
    Public Class ServiceCommunityNewsReadLoaderPresenter
        Inherits DomainPresenter

#Region "Standard"
        Public Overloads Property CurrentManager() As ManagerBase
            Get
                Return _CurrentManager
            End Get
            Set(ByVal value As ManagerBase)
                _CurrentManager = value
            End Set
        End Property
        Public Overloads ReadOnly Property View() As IViewServiceCommunityNewsReadLoader
            Get
                Return MyBase.View
            End Get
        End Property
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
            MyBase.CurrentManager = New ManagerCommunitynews(MyBase.AppContext)
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext, ByVal view As IViewServiceCommunityNewsReadLoader)
            MyBase.New(oContext, view)
            MyBase.CurrentManager = New ManagerBase(MyBase.AppContext)
        End Sub
#End Region

        Public Sub InitView()
            If Not Me.UserContext.isAnonymous Then
                Dim CommunityID As Integer = Me.View.PreLoadedCommunityID
                If Me.View.PreLoadedNewsID = System.Guid.Empty Then
                    Me.View.SetAllNewsRead(Me.UserContext.CurrentUserID, CommunityID, Me.View.PreLoadedPageUrl)
                Else
                    Me.View.SetNewsRead(Me.UserContext.CurrentUserID, CommunityID, Me.View.PreLoadedNewsID, Me.View.PreLoadedPageUrl)
                End If
            Else
                Me.View.NoPermissionToAccess()
            End If
        End Sub
    End Class
End Namespace