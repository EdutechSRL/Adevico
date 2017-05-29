Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewMyCommunityStatistics
        Inherits IViewBaseStatistics

        ReadOnly Property PreloadedFromView() As StatisticView
        Sub SetBackUrl(url As String)
        Sub DisplayUnknownUser()
    End Interface
End Namespace