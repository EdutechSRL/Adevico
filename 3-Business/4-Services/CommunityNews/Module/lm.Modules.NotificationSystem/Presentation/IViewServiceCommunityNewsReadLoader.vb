Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Domain

Namespace Presentation
    <CLSCompliant(True)> Public Interface IViewServiceCommunityNewsReadLoader
        Inherits iDomainView

        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedPageUrl() As String
        ReadOnly Property PreLoadedPreviousUrl() As String
        ReadOnly Property PreLoadedNewsID() As System.Guid
        WriteOnly Property PreviousUrl() As String

        Sub NoPermissionToAccess()
        Sub NavigateToUrl(ByVal url As String)
        Sub SetAllNewsRead(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal url As String)
        Sub SetNewsRead(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal NewsID As System.Guid, ByVal url As String)
        Sub ShowNoCommunityAccess(ByVal CommunityName As String)

    End Interface
End Namespace