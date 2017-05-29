Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.DomainModel

Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Interface iViewMyPortalAccess
        Inherits lm.Comol.Modules.AccessResults.Presentation.IviewAccessResults

        ReadOnly Property PreLoadedStartDate() As Date?
        ReadOnly Property PreLoadedEndDate() As Date?
        Property SelectedStartDate() As Date?
        Property SelectedEndDate() As Date?
    End Interface
End Namespace