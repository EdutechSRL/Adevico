Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Namespace lm.Comol.Modules.AccessResults.Presentation
    Public Interface iViewCommunityList
        Inherits iViewBaseAccessResult

        Property isPersonal() As Boolean
        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        Property CurrentPageSize() As Integer
        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentPageIndex() As Integer
        ReadOnly Property CurrentOrder() As ResultsOrder
        WriteOnly Property SetPreviousURL() As String
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedFromView() As viewType
        ReadOnly Property PreLoadedCommunityName() As String
        Property CommunityName() As String
        ReadOnly Property CurrentView() As viewType

        Sub InitControl(ByVal oContext As ResultContextBase, ByVal CurrentView As viewType)

        Sub LoadCommunity(ByVal oResults As List(Of dtoCommunityResult))
        Sub NavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType)
        Sub ShowSummary(ByVal oSummaryType As SummaryType, ByVal UserName As String, ByVal CommunityName As String)

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
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonViewer As Integer, ByVal CommunityOwnerID As Integer)
        Sub AddActionViewCommunities(ByVal CommunityID As Integer, ByVal PersonViewer As Integer, ByVal CommunityOwnerID As Integer)

        Sub NoPermissionToAccess(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal OnPersonID As Integer)
    End Interface
End Namespace