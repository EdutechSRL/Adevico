Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Interface IviewContentPageAccessResults
        Inherits iViewBaseAccessResult
        Sub InitContent(ByVal oContex As ResultContextBase)
        Sub NoPermissionToAccess()
        ReadOnly Property CurrentView() As viewType
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
    End Interface
End Namespace