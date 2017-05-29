Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
Imports WSstatistics

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewUsageDetails
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property StatisticContext() As UsageContext
        ReadOnly Property PreLoadedView() As viewType
        ReadOnly Property PreLoadedBackUrl() As String

        Property BackUrl() As String
        Property AvailableView() As List(Of viewType)
        Property CurrentView() As viewType

        Property CurrentStartDate(view As viewType) As DateTime
        ReadOnly Property CurrentEndDate(view As viewType) As DateTime

        Sub LoadAvailableYears(years As List(Of Integer))
        Sub LoadAvailableViews(items As List(Of viewType))
        Sub LoadStatistics(statistics As List(Of dtoDetailUsageStatistic), view As viewType)

        Sub DisplayInfo(moduleName As String)
        Sub DisplayInfo(moduleName As String, communityName As String)
        Sub DisplayUserInfo(moduleName As String, userName As String)
        Sub DisplayInfo(moduleName As String, communityName As String, userName As String)

        Sub DisplaySessionTimeout()
        Sub DisplayNoPermission()

        Enum viewType
            None = -100
			Day = 0
			Week = 1
			Month = 2
			Year = 3
			Range = 4
        End Enum
        Enum grouping
            None = 0
            CommunityID = 1
            ModuleID = 2
            PersonID = 3
        End Enum
        'Enum statisticsType
        '    GlobalType = 0
        '    CommunityType = 1
        '    ModuleType = 2
        'End Enum
    End Interface
End Namespace
