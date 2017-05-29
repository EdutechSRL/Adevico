Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewCommunityStatistics
        Inherits IViewBaseStatistics

        ReadOnly Property PreloadedFromView() As StatisticView
        Sub SetBackUrl(url As String)
    End Interface
End Namespace