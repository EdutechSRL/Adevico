Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Domain
Imports lm.Notification.DataContract.Domain

Namespace Presentation
    <CLSCompliant(True)> Public Interface IViewCommunityNews
        Inherits iDomainView

        ReadOnly Property PortalPermission() As ModuleCommunityNews
        ReadOnly Property ModulePermission() As ModuleCommunityNews
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews))
        Property NewsContext() As CommunityNewsContext

        Property CommunityNameField() As String
        Property Pager() As PagerBase
        Property CurrentPageSize() As Integer

        ReadOnly Property CurrentView() As ViewModeType
        ReadOnly Property PreLoadedView() As ViewModeType
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedCommunityName() As String
        ReadOnly Property PreLoadedDayMode() As DayModeType
        ReadOnly Property PreLoadedDay() As Date
        ReadOnly Property PreLoadedDaySpecifyed() As Boolean
        ReadOnly Property PreLoadedPreviousView() As ViewModeType
        ReadOnly Property CurrentPageIndex() As Integer
        ReadOnly Property PreLoadedModuleID() As Integer
        ReadOnly Property PreLoadedViewFilter() As ViewModeFiler

        WriteOnly Property SetPreviousURL() As String

        Sub NavigationUrl(ByVal oContext As CommunityNewsContext)
        Function GetNavigationUrl(ByVal oContext As CommunityNewsContext) As String

        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oType As DayModeType, ByVal oDay As Date)
        Sub AddActionViewNews(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal NewsID As System.Guid)
        Sub AddAction(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oActionType As COL_BusinessLogic_v2.UCServices.Services_CommunityNews.ActionType, ByVal oDay As Date)

        Sub NoPermissionToAccess()
        Sub LoadTabs(ByVal oTabList As List(Of dtoTab))
        Sub LoadDays(ByVal oTabList As List(Of dtoTab))

        ReadOnly Property ToDayTranslated() As String
        ReadOnly Property YesterdayTranslated() As String

        Function GetTabUrl(ByVal CommunityID As Integer, ByVal CurrentView As ViewModeType, ByVal FromView As ViewModeType, ByVal CurrentDay As Date, ByVal DayView As DayModeType, Optional ByVal WithBaseUrl As Boolean = True) As String

        Sub LoadNotificationSummary(ByVal DayToShow As String, ByVal oList As List(Of dtoCommunitySummaryNews))
        Sub LoadNotifications(ByVal DayToShow As String, ByVal oList As List(Of dtoModuleNews))

        Function GetUrlDetails(ByVal oContext As CommunityNewsContext, Optional ByVal WithBaseUrl As Boolean = True)

        Sub LoadServices(ByVal oList As List(Of dtoRemoteModule))
        Property CurrentViewFilter() As ViewModeFiler
        Property CurrentModuleID() As Integer

        Sub LoadAllNews(ByVal oList As List(Of dtoMultipleNews))
        Sub ShowNoNewsFrom(ByVal oDate As DateTime, ByVal CommunityName As String)
    End Interface
End Namespace