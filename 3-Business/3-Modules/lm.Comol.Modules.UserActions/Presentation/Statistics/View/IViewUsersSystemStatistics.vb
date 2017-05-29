Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewCommunitySystemStatistics
        Inherits IViewBaseStatistics

        ReadOnly Property PreloadedSearchBy() As String
        Property CurrentSearchBy As String
    End Interface
End Namespace