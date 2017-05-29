Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Interface IviewAccessResults
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property ResultsContext() As ResultContextBase
        Sub NavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType)
        Function GetNavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType) As String
        Function GetPrintUrl(ByVal oContext As ResultContextBase, ByVal oView As viewType) As String

        ReadOnly Property ModulePermission() As ModuleUsageResult
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleUsageResult))
        Property ViewAvailable() As IList(Of viewType)
        Property CurrentView() As viewType
        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentPageSize() As Integer

        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedFromView() As viewType


        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentPage() As Integer
        ReadOnly Property CurrentOrder() As lm.Comol.Modules.AccessResults.DomainModel.ResultsOrder
        Property AllowPrint() As Boolean

        Sub NoPermissionToAccess()

        

        Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String

        Sub LoadItems(ByVal oResults As List(Of dtoAccessResult))
        WriteOnly Property SetPreviousURL() As String
        WriteOnly Property SetPrintUrl() As String
        Sub ShowSummary(ByVal oSummaryType As SummaryType, ByVal UserName As String, ByVal Community As String)

        Enum SummaryType
            OwnFilter = 0
            PortalUserFilter = 1
            OwnCommunityFilter = 2
            UserCommunityFilter = 3
            UserCommunitiesList = 4
            PortalUsers = 5
            CommunityUsers = 6
            PortalBetweenDateFilter = 7
            CommunityBetweenDateFilter = 8
        End Enum

        Sub AddActionSpecifyFilters(ByVal CommunityID As Integer, ByVal PersonID As Integer)
        Sub AddActionViewReport(ByVal CommunityID As Integer, ByVal PersonID As Integer)
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
    End Interface
End Namespace