Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel

Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Interface IviewTabAccessResult
        Inherits iViewBaseAccessResult

        Property ViewAvailable() As IList(Of viewType)
        Property CurrentView() As viewType
        Sub InitContent(ByVal oContext As ResultContextBase)
        Sub LoadSubView()

        Sub NoPermissionToAccess(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer)
    End Interface
End Namespace