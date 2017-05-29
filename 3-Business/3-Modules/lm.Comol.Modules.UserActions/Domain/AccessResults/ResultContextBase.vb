Namespace lm.Comol.Modules.AccessResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class ResultContextBase
        Inherits ContextBase
        Implements ICloneable
        Public FromView As viewType

        Public Overridable Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New ResultContextBase
            o.UserID = UserID
            o.CommunityID = CommunityID
            o.Order = Order
            o.Ascending = Ascending
            o.CurrentPage = CurrentPage
            o.FromDate = FromDate
            o.ToDate = ToDate
            o.FromView = FromView
            o.NameSurnameFilter = NameSurnameFilter
            Return o
        End Function

        Sub New()
            FromView = viewType.None
        End Sub
    End Class
End Namespace