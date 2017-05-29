Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewUserCommunityStatistics
        Inherits IViewBaseStatistics

        ReadOnly Property PreloadedFromView() As StatisticView
        ReadOnly Property PreloadedBackUrl() As String
        Function GetEncodedBackUrl() As String
        Sub DisplayUnknownUser()
        Sub SetBackUrl(url As String)
    End Interface
End Namespace