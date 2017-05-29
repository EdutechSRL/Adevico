Namespace lm.Comol.Modules.UsageResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoUserWithResult
        Private _Adapter As dtoUserWithResultAdapter

        Public PersonID As Integer
        Public CommunityID As Integer
        Public Result As Integer

        Public Sub New(ByVal ur As SRV_accessMonitor.UserWithResult)
            _Adapter = New dtoUserWithResultAdapter(ur)
            _Adapter.Initialize(Me)
        End Sub
    End Class

    Friend Class dtoUserWithResultAdapter
        Private _UserWithResult As SRV_accessMonitor.UserWithResult

        Sub New(ByVal o As SRV_accessMonitor.UserWithResult)
            _UserWithResult = o
        End Sub

        Sub Initialize(ByVal dto As dtoUserWithResult)
            With dto
                .CommunityID = _UserWithResult.CommunityID
                .Result = _UserWithResult.Result
                .PersonID = _UserWithResult.PersonID
            End With
        End Sub
    End Class
End Namespace