Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
Imports WSstatistics

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewModuleUsage
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property CommunityRepositoryPermission(ByVal CommunityID As Integer) As ModuleStatistics

        Property CurrentPageSize() As Integer
        Property CurrentPager() As lm.Comol.Core.DomainModel.PagerBase
        ReadOnly Property PreloadedModuleID() As Integer
        ReadOnly Property PreloadedCommunityID() As Integer
        ReadOnly Property PreserveDownloadUrl() As Boolean
        ReadOnly Property PortalName() As String
        Property CurrentCommunityID() As Integer
        Property CurrentModuleID() As Integer
        Property PreservedDownloadUrl() As String
        Property CurrentStartDate() As dtoDate
        Property CurrentEndDate() As dtoDate

        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentOrder() As StatisticOrder

        Sub LoadSummary(ByVal CommunityName As String, ByVal ModuleName As String, ByVal TotalTime As TimeSpan)
        Sub LoadItems(ByVal oStatistic As dtoStatistic)
        Sub NoPermission(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub SendActionInit(ByVal CommunityID As Integer, ByVal ModuleID As Integer)
        Sub SendActionLoadItems(ByVal CommunityID As Integer, ByVal ModuleID As Integer)

    End Interface
End Namespace
