Namespace lm.Comol.Modules.UsageResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ResultContext
        Inherits AccessResults.DomainModel.ContextBase
        Implements ICloneable
        Public SubView As ViewDetailPage

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New ResultContext
            o.UserID = UserID
            o.CommunityID = CommunityID
            o.Order = Order
            o.Ascending = Ascending
            o.CurrentPage = CurrentPage
            o.FromDate = FromDate
            o.ToDate = ToDate
            o.SubView = SubView
            o.NameSurnameFilter = NameSurnameFilter
            Return o
        End Function
    End Class
End Namespace