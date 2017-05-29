Namespace lm.Comol.Modules.UsageResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoCommunityWithResult
        Private _Adapter As dtoCommunityWithResultAdapter

        Public PersonID As Integer
        Public PersonName As String
        Public CommunityID As Integer
        Public CommunityName As String
        Public Result As Integer
        Public Sub New()

        End Sub

        Public Sub New(ByVal c As lm.Comol.Core.DomainModel.iCommunity)
            _Adapter = New dtoCommunityWithResultAdapter(c)
            _Adapter.Initialize(Me)
        End Sub

    End Class

    Friend Class dtoCommunityWithResultAdapter
        Private _Community As lm.Comol.Core.DomainModel.iCommunity

        Sub New(ByVal c As lm.Comol.Core.DomainModel.iCommunity)
            _Community = c
        End Sub

        Sub Initialize(ByVal dto As dtoCommunityWithResult)
            With dto
                .CommunityID = _Community.Id
                .CommunityName = _Community.Name

            End With
        End Sub
    End Class

End Namespace