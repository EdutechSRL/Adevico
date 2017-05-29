Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Domain

Namespace Presentation
    <CLSCompliant(True)> Public Interface IViewCommunityDayNews
        Inherits iDomainView

        ReadOnly Property PortalPermission() As ModuleCommunityNews
        ReadOnly Property ModulePermission() As ModuleCommunityNews
        ReadOnly Property CommunitiesPermission() As IList(Of ModuleCommunityPermission(Of ModuleCommunityNews))
        Property NewsContext() As DayNewsContext
        Property Pager() As PagerBase
        ReadOnly Property CurrentPageIndex() As Integer
        Property CurrentPageSize() As Integer
        WriteOnly Property SetPreviousURL() As String
        Sub SetAllNewsRead(ByVal CommunityID As Integer, ByVal url As String)

        ReadOnly Property PreLoadedCommunityID() As Integer
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedDay() As Date
        ReadOnly Property PreLoadedDaySpecifyed() As Boolean
        ReadOnly Property PreLoadedFromDay() As Date
        ReadOnly Property PreLoadedPreviousDay() As Date
        ReadOnly Property PreLoadedPreviousFromView() As ViewModeType
        ReadOnly Property PreLoadedPreviousView() As ViewModeType
        ReadOnly Property PreLoadedPreviousCommunityName() As String
        ReadOnly Property PreLoadedPreviousDayView() As DayModeType
        ReadOnly Property PreLoadedPreviousPageSize() As Integer
        ReadOnly Property PreLoadedPreviousPageIndex() As Integer
        ReadOnly Property PreLoadedPreviousUserID() As Integer
        ReadOnly Property PreviousCommunityID() As Integer
        WriteOnly Property CommunityName() As String

        Sub NavigationUrl(ByVal oContext As DayNewsContext)
        Function GetNavigationUrl(ByVal oContext As DayNewsContext, ByVal WithBaseUrl As Boolean) As String
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oType As DayModeType, ByVal oDay As Date)
        Sub AddActionViewNews(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal NewsID As System.Guid)
        Sub AddAction(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal oActionType As COL_BusinessLogic_v2.UCServices.Services_CommunityNews.ActionType, ByVal oDay As Date)


        Sub NoPermissionToAccess()
        Sub LoadNotifications(ByVal DayToShow As String, ByVal oList As List(Of dtoModuleNews))
        Sub LoadAllNews(ByVal oList As List(Of dtoMultipleNews))
        Sub ShowNoNewsFrom(ByVal oDate As DateTime, ByVal CommunityName As String)
        Sub SetNoNewsForComunity(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal UpdateDate As DateTime)
    End Interface
End Namespace