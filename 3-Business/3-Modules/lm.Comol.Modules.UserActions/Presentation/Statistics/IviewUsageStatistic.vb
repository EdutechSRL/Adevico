Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
	<CLSCompliant(True)> Public Interface IviewUsageStatistic
		Inherits lm.Comol.Core.DomainModel.Common.iDomainView

		'WriteOnly Property (ByVal Usage As TimeSpan,Access As Integer,
		Property StatisticContext() As UsageContext
        Property ViewAvailable() As IList(Of viewType)
		Property CurrentView() As viewType

		Property Pager() As lm.Comol.Core.DomainModel.PagerBase
		ReadOnly Property CurrentPageSize() As Integer
        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentPage() As Integer
		ReadOnly Property CurrentOrder() As StatisticOrder

		Sub NavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType)
		Function NavigationUrlToDetails(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oFromView As IviewUsageStatistic.viewType, ByVal oViewDetails As IViewUsageDetails.viewType) As String

        Function GetNavigationUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType, ByVal startFrom As IviewUsageStatistic.viewType) As String
		ReadOnly Property PreLoadedView() As viewType
		ReadOnly Property ReturnTo() As viewType
        ReadOnly Property StartFrom() As viewType
		Sub NoPermissionToAccess()
		Enum viewType
			None = -100
			Personal = 0
			SystemUsers = 1
			CommunityUsers = 2
			GenericUser = 3
			PersonalCommunity = 4
			GenericSystem = 5
			GenericCommunity = 6
			UserOnLine = 7
			CommunityUserOnLine = 8
        End Enum
        Enum grouping
            None = 0
            CommunityId = 1
            ModuleId = 2
            PersonId = 3
        End Enum
		Sub LoadItems(ByVal oStatistic As dtoStatistic, ByVal oContext As UsageContext, ByVal oType As viewType, ByVal oViewDetails As IViewUsageDetails.viewType)
        WriteOnly Property SetPreviousURL(view As viewType) As String
		Sub LoadSummary(ByVal oSummary As dtoSummary, ByVal oType As SummaryType)
		Sub SetFirstColumHeader(ByVal oType As IviewUsageStatistic.viewType)
		Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String
		Function GetSummaryTranslatedString(ByVal oSummary As dtoSummary, ByVal oType As SummaryType) As String

		Enum SummaryType
			None = -100
			Personal = 0
			Community = 1
			Modules = 2
			PersonalCommunity = 3
			PersonalModules = 4
			CommunityModules = 5
			PersonalCommunityModules = 6
			Portal = 7
			OnLineSystem = 8
			OnLineCommunity = 9
		End Enum
	End Interface
End Namespace