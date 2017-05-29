'Imports lm.Comol.Core.DomainModel.Common
'Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
'Imports WSstatistics

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewStatisticDetails
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property StatisticContext() As UsageContext
        Property AvailableDates As List(Of dtoYearItem)
        ReadOnly Property PreLoadedView() As DetailViewType
        ReadOnly Property PreLoadedBackUrl() As String
        Sub DisplayNoPermission()
        Sub DisplaySessionTimeout()


        Property BackUrl() As String
        Sub LoadAvailableViews(views As List(Of DetailViewType))
        Property CurrentView() As DetailViewType

        Property CurrentStartDate(view As DetailViewType) As DateTime
        ReadOnly Property CurrentEndDate(view As DetailViewType) As DateTime

        Sub LoadAvailableYears(years As List(Of dtoYearItem))
        Sub LoadStatistics(statistics As List(Of dtoDetailUsageStatistic), view As DetailViewType)

        Sub DisplayInfo(moduleName As String)
        Sub DisplayInfo(moduleName As String, communityName As String)
        Sub DisplayUserInfo(moduleName As String, userName As String)
        Sub DisplayInfo(moduleName As String, communityName As String, userName As String)

        Sub SendAction(ByVal idCommunity As Integer, ByVal idUser As Integer, ByVal action As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ActionType)
        Sub SendAction(ByVal idCommunity As Integer, ByVal idUser As Integer, ByVal statIdUser As Integer, ByVal statIdCommunity As Integer, ByVal action As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ActionType)

    End Interface
End Namespace
