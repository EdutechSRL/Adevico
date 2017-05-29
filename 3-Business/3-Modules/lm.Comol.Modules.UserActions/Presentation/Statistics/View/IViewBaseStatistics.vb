Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewBaseStatistics
        Inherits IViewBasePageStatistics
        ReadOnly Property CurrentOrder() As StatisticOrder
        ReadOnly Property CurrentView() As StatisticView

        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
      
        Sub LoadAvailableView(views As List(Of StatisticView))
        Sub NoPermissionToAccess()
        Sub DisplaySessionTimeout()
        Sub LoadItems(ByVal statistics As dtoStatistic, ByVal ucontext As UsageContext, ByVal vPage As StatisticView, ByVal detailType As DetailViewType)
        Sub LoadSummary(ByVal oSummary As dtoSummary, ByVal oType As SummaryType)
      
    End Interface
End Namespace