Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewBasePageStatistics
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property StatisticContext() As UsageContext
        ReadOnly Property CurrentPageSize() As Integer
        ReadOnly Property Ascending() As Boolean
        ReadOnly Property CurrentPage() As Integer

        ReadOnly Property CurrentUrl() As String
        ReadOnly Property PortalName() As String

        Sub LoadView(ByVal url As String)

        Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String
        Function GetSummaryTranslatedString(ByVal summary As dtoSummary, ByVal type As SummaryType) As String

        Sub SendAction(ByVal idCommunity As Integer, ByVal idModule As Integer, ByVal action As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ActionType)
        Sub SendAction(ByVal idCommunity As Integer, ByVal idModule As Integer, ByVal statIdItem As Integer, ByVal type As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ObjectType, ByVal action As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ActionType)
        Sub SendAction(ByVal idCommunity As Integer, ByVal idModule As Integer, ByVal statIdUser As Integer, ByVal statIdCommunity As Integer, ByVal action As lm.Comol.Modules.UserActions.DomainModel.ModuleStatistics.ActionType)
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