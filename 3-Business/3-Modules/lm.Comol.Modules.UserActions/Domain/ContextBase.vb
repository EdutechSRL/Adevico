Namespace lm.Comol.Modules.AccessResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ContextBase
        Public UserID As Integer
        Public CommunityID As Integer
        Public Order As ResultsOrder
        Public Ascending As Boolean
        Public CurrentPage As Integer
        Public FromDate As Date
        Public ToDate As Date
        Public NameSurnameFilter As String


        Sub New()

        End Sub
    End Class
End Namespace