
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports lm.Comol.Modules.AccessResults.Presentation
Imports lm.Comol.Modules.AccessResults.DomainModel
Namespace lm.Comol.Modules.AccessResults.Presentation
    <CLSCompliant(True)> Public Interface iViewBaseAccessResult
        Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        Property ResultsContext() As ResultContextBase



        Function GetNavigationUrl(ByVal oContext As ResultContextBase, ByVal oDestinationView As viewType) As String
    End Interface
End Namespace