﻿Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Interface IViewUsersCommunityStatistics
        Inherits IViewBaseStatistics

        ReadOnly Property PreloadedFromView() As StatisticView
        Sub SetBackUrl(url As String)
        ReadOnly Property PreloadedSearchBy() As String
        Property CurrentSearchBy As String
    End Interface
End Namespace