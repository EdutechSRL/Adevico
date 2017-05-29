Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class OnLineUsersContext
        Implements ICloneable

        Public UserID As Integer
        Public CommunityID As Integer
        Public ModuleID As Integer
        Public Order As StatisticOrder
        Public Ascending As Boolean
        Public CurrentPage As Integer
        Public LastUpdate As DateTime
        Public NameSurnameFilter As String

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New OnLineUsersContext
            o.UserID = UserID
            o.CommunityID = CommunityID
            o.ModuleID = ModuleID
            o.Order = Order
            o.Ascending = Ascending
            o.CurrentPage = CurrentPage
            o.NameSurnameFilter = NameSurnameFilter
            o.LastUpdate = LastUpdate
            Return o
        End Function
    End Class
End Namespace