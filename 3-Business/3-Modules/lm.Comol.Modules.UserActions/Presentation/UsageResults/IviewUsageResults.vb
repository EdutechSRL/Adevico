Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Modules.UsageResults.Presentation

Namespace lm.Comol.Modules.UsageResults.Presentation
    <CLSCompliant(True)> Public Interface IviewUsageResults
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView


        Property ResultsContext() As ResultContext
        Property ViewAvailable() As IList(Of viewType)
        Property CurrentView() As viewType
        Property CurrentDetailView() As ViewDetailPage
        ReadOnly Property PreLoadedStartDate() As Date?
        ReadOnly Property PreLoadedEndDate() As Date?

        ReadOnly Property PreLoadedView() As viewType
        ReadOnly Property PreLoadedDetailView() As ViewDetailPage

        'ReadOnly Property PreLoadedPageSize() As Integer
        'ReadOnly Property PreLoadedNameSurname() As String
        'Property NameSurnameField() As String

        Property SelectedStartDate() As Date?
        Property SelectedEndDate() As Date?
        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        ReadOnly Property CurrentPageSize() As Integer
        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentPage() As Integer
        ReadOnly Property CurrentOrder() As ResultsOrder
        Property AllowPrint() As Boolean

        Sub NoPermissionToAccess()

        Enum viewType
        None = -100
        MyPortalPresence = 0
        MyCommunitiesPresence = 1
        UsersPortalPresence = 2
        CurrentCommunityPresence = 3
        UsersCurrentCommunityPresence = 4
        BetweenDateUsersPortal = 5
        BetweenDateUsersCommunity = 6
    End Enum

        Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String

        Sub ShowDetailView(ByVal oView As ViewDetailPage)

        Sub LoadItems(ByVal oResults As List(Of dtoAccessResult))
        WriteOnly Property SetPreviousURL() As String
        WriteOnly Property SetPrintUrl() As String
        Sub SetFirstColumHeader(ByVal oType As IviewUsageResults.viewType, ByVal oView As ViewDetailPage)
        Sub NavigationUrl(ByVal oContext As ResultContext, ByVal oDestinationView As IviewUsageResults.viewType)
        'Function NavigationUrlToDetails(ByVal oContext As ResultContext, ByVal oFromView As IviewUsageResults.viewType, ByVal oViewDetails As ViewDetailPage) As String
        Function GetNavigationUrl(ByVal oContext As ResultContext, ByVal oDestinationView As IviewUsageResults.viewType) As String
        Function GetPrintUrl(ByVal oContext As ResultContext, ByVal oView As IviewUsageResults.viewType) As String
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
       Sub AddActionViewCommunities(ByVal CommunityID As Integer, ByVal PersonID As Integer)
       Sub AddActionViewReport(ByVal CommunityID As Integer, ByVal PersonID As Integer)
       Sub AddActionViewUsers(ByVal CommunityID As Integer, ByVal PersonID As Integer)
       Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
    End Interface
End Namespace