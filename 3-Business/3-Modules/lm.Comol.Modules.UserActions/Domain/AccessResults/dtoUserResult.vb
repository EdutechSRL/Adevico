Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.AccessResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoUserAccessResult
        Public PersonID As Integer
        Public PersonName As String
        Public NavigateTo As String

        Public Sub New()

        End Sub
        Public Sub New(ByVal p As Person)
            PersonID = p.Id
            PersonName = p.Name
        End Sub
        Public Sub New(ByVal p As Person, ByVal NavigateURL As String)
            PersonID = p.Id
            PersonName = p.Name
            NavigateTo = NavigateURL

        End Sub
    End Class
End Namespace