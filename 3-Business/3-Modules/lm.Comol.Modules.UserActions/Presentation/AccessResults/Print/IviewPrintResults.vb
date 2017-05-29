Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation

Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Interface IviewPrintResults
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property CurrentView() As viewType
        ReadOnly Property ContextBase() As ResultContextBase
        Function GetTimeTranslatedString(ByVal oSpan As TimeSpan) As String
        Sub LoadItems(ByVal TotalTime As TimeSpan, ByVal oList As List(Of UsageResults.DomainModel.dtoAccessResult))
        WriteOnly Property PrintedOn() As String
        WriteOnly Property PrintedBy() As String
        Sub AddActionPrintReport(ByVal CommunityID As Integer, ByVal PersonID As Integer)
        Sub AddActionNoPermission(ByVal CommunityID As Integer, ByVal PersonID As Integer)
    End Interface
End Namespace