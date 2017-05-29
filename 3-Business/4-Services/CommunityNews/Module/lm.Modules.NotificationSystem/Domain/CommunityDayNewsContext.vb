Imports lm.Modules.NotificationSystem.Domain
Namespace Domain
    <CLSCompliant(True), Serializable()> Public Class DayNewsContext
        Implements ICloneable
        Public UserID As Integer
        Public CommunityID As Integer
        Public CurrentDay As Date
        Public FromDay As DateTime
        Public PageIndex As Integer

        Public PreviousView As ViewModeType
        Public PreviousDayView As DayModeType
        Public PreviousCommunityName As String
        Public PreviousDay As Date
        Public PreviousPageSize As Integer
        Public PreviousPageIndex As Integer
        Public PreviousUserID As Integer
        Public PreviousFromView As ViewModeType
        Public PreviousCommunityID As Integer


        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New DayNewsContext
            o.CommunityID = CommunityID
            o.CurrentDay = CurrentDay
            o.PageIndex = PageIndex
            o.FromDay = FromDay
            o.PreviousView = PreviousView
            o.PreviousDayView = PreviousDayView
            o.PreviousCommunityName = PreviousCommunityName
            o.PreviousDay = PreviousDay
            o.PreviousPageSize = PreviousPageSize
            o.PreviousPageIndex = PreviousPageIndex
            o.PreviousFromView = PreviousFromView
            o.PreviousCommunityID = PreviousCommunityID

            Return o
        End Function
    End Class
End Namespace