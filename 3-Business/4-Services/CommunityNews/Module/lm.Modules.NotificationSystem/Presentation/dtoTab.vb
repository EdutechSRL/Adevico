Imports lm.Modules.NotificationSystem.Domain
Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoTab
        Public TypeTab As DayModeType
        Public Name As String
        Public Url As String
        Public Enabled As Boolean
        Public Selected As Boolean
        Public isType As Boolean
        Public Day As Date
        Public Month As Integer
        Public Sub New()
            Enabled = True
            Selected = False
            TypeTab = DayModeType.None
            isType = False
        End Sub
    End Class
End Namespace