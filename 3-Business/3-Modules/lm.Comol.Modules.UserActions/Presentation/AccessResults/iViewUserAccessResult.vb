Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel

Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Interface iViewUserAccessResult
        Inherits iViewBaseAccessResult

        ReadOnly Property PreLoadedStartDate() As Date?
        ReadOnly Property PreLoadedEndDate() As Date?
        Property SelectedStartDate() As Date?
        Property SelectedEndDate() As Date?
        ReadOnly Property PreLoadedUserName() As String

        Property CurrentView() As viewType
        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentPageSize() As Integer
        Property UserName() As String
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedFromView() As viewType

        Function GetPrintUrl(ByVal oContext As ResultContextBase, ByVal oView As viewType) As String
        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentPage() As Integer
        ReadOnly Property CurrentOrder() As lm.Comol.Modules.AccessResults.DomainModel.ResultsOrder
        Property AllowPrint() As Boolean

        Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String


        Sub LoadItems(ByVal TotalTime As TimeSpan, ByVal oResults As List(Of UsageResults.DomainModel.dtoAccessResult))
        WriteOnly Property SetPreviousURL() As String
        WriteOnly Property SetPrintUrl() As String
        Sub NavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType)
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

        Sub AddActionSpecifyFilters(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer)
        Sub AddActionViewReport(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer)
        Sub NoPermissionToAccess(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer)

        Sub InitControl(ByVal oContext As ResultContextBase, ByVal CurrentView As viewType)
        Property ShowUserNameSearch() As Boolean
        WriteOnly Property ShowUserName() As Boolean
    End Interface
End Namespace