Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IviewOnLineUsers
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property OnLineContext() As OnLineUsersContext
        Property ViewAvailable() As IList(Of IviewUsageStatistic.viewType)
        Property CurrentView() As IviewUsageStatistic.viewType

        Property ShowIp As Boolean
        Property Pager() As lm.Comol.Core.DomainModel.PagerBase
        ReadOnly Property CurrentPage() As Integer
        ReadOnly Property Ascending() As Boolean
        Property CurrentPageSize() As Integer
        ReadOnly Property CurrentOrder() As StatisticOrder
        ReadOnly Property TranslatedContext() As dtoTranslatedContext

        Sub NavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As OnLineUsersContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType)
        Function GetNavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As OnLineUsersContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType) As String
        ReadOnly Property PreLoadedView() As IviewUsageStatistic.viewType
        ReadOnly Property PreLoadedLastUpdate() As DateTime
        ReadOnly Property PreLoadedPageSize() As Integer
        ReadOnly Property PreLoadedNameSurname() As String

        ReadOnly Property ReturnTo() As IviewUsageStatistic.viewType

        Property NameSurnameField() As String
        Sub NoPermissionToAccess()

        Sub UnLoadItems()
        Sub LoadItems(ByVal oList As List(Of dtoOnLineUser), viewIp As Boolean)
        WriteOnly Property SetPreviousURL() As String
        Sub LoadSummary(ByVal oDto As dtoSummary, ByVal oType As IviewUsageStatistic.SummaryType)
        Sub SendToView(ByVal oView As IviewUsageStatistic.viewType)
        Sub SendUserAction(ByVal idCommunity As Integer, idModule As Integer, action As ModuleOnLineUser.ActionType)
    End Interface
End Namespace