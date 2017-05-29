Namespace lm.Comol.Modules.UserActions.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class UsageContext
		Implements ICloneable

		Public UserID As Integer
		Public CommunityID As Integer
		Public ModuleID As Integer
		Public Order As StatisticOrder
		Public Ascending As Boolean
        Public CurrentPage As Integer
        Public UserIDList As New List(Of String)
        Public CommunityIDList As New List(Of Integer)
        Public ModuleIDList As New List(Of Integer)
        Public GroupBy As grouping
        Public LastUpdate As DateTime
        Public SearchBy As String

        Public Enum grouping
            None = 0
            CommunityID = 1
            ModuleID = 2
            UserID = 3
        End Enum
        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New UsageContext
            o.UserID = UserID
            o.CommunityID = CommunityID
            o.ModuleID = ModuleID
            o.Order = Order
            o.Ascending = Ascending
            o.CurrentPage = CurrentPage
            o.GroupBy = GroupBy
            o.LastUpdate = LastUpdate
            For Each id As Integer In UserIDList
                o.UserIDList.Add(id)
            Next
            For Each id As Integer In ModuleIDList
                o.ModuleIDList.Add(id)
            Next
            For Each id As Integer In CommunityIDList
                o.CommunityIDList.Add(id)
            Next
            Return o
        End Function
	End Class
End Namespace