Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoDay
        Public Day As Date
        Public DayNumber As Integer
        Public DayName As String
        Public AbbreviateName As String
        Public Month As String
        Public Url As String
        Public Enabled As Boolean
        'Public Selected As Boolean
        Public Sub New()

        End Sub
        Public Sub New(ByVal day As Date)
            Me.Day = day
            Me.DayNumber = day.Day
            If day.DayOfWeek = 0 Then
                Me.DayName = WeekdayName(7, False, FirstDayOfWeek.Monday)
                Me.AbbreviateName = WeekdayName(7, True, FirstDayOfWeek.Monday)
            Else
                Me.DayName = WeekdayName(day.DayOfWeek, False, FirstDayOfWeek.Monday)
                Me.AbbreviateName = WeekdayName(day.DayOfWeek, True, FirstDayOfWeek.Monday)
            End If
            Me.Month = MonthName(day.Month, True)
            Url = ""
            Enabled = False
            '  Selected = False
        End Sub
    End Class
End Namespace