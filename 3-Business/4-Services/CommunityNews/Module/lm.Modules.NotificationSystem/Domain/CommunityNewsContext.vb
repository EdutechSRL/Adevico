Imports lm.Modules.NotificationSystem.Domain
Namespace Domain
    <CLSCompliant(True), Serializable()> Public Class CommunityNewsContext
        Implements ICloneable

        Public UserID As Integer
        Public CommunityID As Integer
        Public PageIndex As Integer
        'Public PageSize As Integer
        Public CurrentDay As Date
        Public CurrentView As ViewModeType
        Public FromView As ViewModeType
        Public DayView As DayModeType
        Public CommunityName As String

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim o As New CommunityNewsContext
            o.CommunityID = CommunityID
            o.UserID = UserID
            o.PageIndex = PageIndex
            ' o.PageSize = PageSize
            o.CurrentDay = CurrentDay
            o.CurrentView = CurrentView
            o.FromView = FromView
            o.DayView = DayView
            o.CommunityName = CommunityName
            Return o
        End Function

        Public Function UpdateCommunity(ByVal CommunityID As Integer) As CommunityNewsContext
            Me.CommunityID = CommunityID
            Return Me
        End Function
    End Class
End Namespace