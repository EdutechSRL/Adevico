Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.AccessResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoCommunityResult
        Public PersonID As Integer
        Public CommunityID As Integer
        Public CommunityName As String
        Public NavigateTo As String

        Public Sub New()

        End Sub
        Public Sub New(ByVal c As Community, ByVal UserID As Integer)
            CommunityID = c.Id
            CommunityName = c.Name
            PersonID = UserID
        End Sub
        Public Sub New(ByVal c As Community, ByVal NavigateURL As String)
            CommunityID = c.Id
            CommunityName = c.Name
            NavigateTo = NavigateURL

        End Sub
    End Class
End Namespace