Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Domain

Namespace Presentation
    <CLSCompliant(True)> Public Interface IViewNewsFromLastAccess
        Inherits iDomainView

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
        ReadOnly Property PreLoadedPreviousView() As ViewModeType
        ReadOnly Property CurrentPageIndex() As Integer

        WriteOnly Property SetPreviousURL() As String

        Sub NavigationUrl(ByVal oContext As CommunityNewsContext)
        Function GetNavigationUrl(ByVal oContext As CommunityNewsContext) As String

        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
        Sub NoPermissionToAccess()
        Sub LoadTabs(ByVal oTabList As List(Of dtoTab))
        Sub LoadDays(ByVal oTabList As List(Of dtoTab))

        ReadOnly Property ToDayTranslated() As String
        ReadOnly Property YesterdayTranslated() As String

        Function GetTabUrl(ByVal CommunityID As Integer, ByVal CurrentView As ViewModeType, ByVal FromView As ViewModeType, ByVal CurrentDay As Date, ByVal DayView As DayModeType, Optional ByVal WithBaseUrl As Boolean = True) As String

        Sub LoadNotificationSummary(ByVal DayToShow As String, ByVal oList As List(Of dtoCommunitySummaryNews))
        Sub LoadNotifications(ByVal DayToShow As String, ByVal oList As List(Of dtoModuleNews))

        Function GetUrlDetails(ByVal oContext As CommunityNewsContext, Optional ByVal WithBaseUrl As Boolean = True)
    End Interface
End Namespace