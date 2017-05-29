Namespace lm.WS.ActionStatistics.Domain
    <Serializable(), CLSCompliant(True)> Public Class dtoDate
        Public Hour As Integer
        Public Day As Integer
        Public Month As Integer
        Public Year As Integer

        Public Sub New()
            Hour = 0
            Day = 1
            Month = 1
            Year = 2000
        End Sub
        Public Sub New(ByRef _year As Integer, ByRef _month As Integer, ByRef _day As Integer, Optional ByRef _hour As Integer = 0)
            Hour = _hour
            Day = _day
            Month = _month
            Year = _year
        End Sub
        Public Sub New(ByRef newDateTime As DateTime)
            Hour = newDateTime.Hour
            Day = newDateTime.Day
            Month = newDateTime.Month
            Year = newDateTime.Year
        End Sub
        Public Function ToDateTime() As DateTime
            Dim retVal As DateTime
            Try
                retVal = DateTime.Parse(Year.ToString & "-" & Month.ToString & "-" & Day.ToString & " " & Hour.ToString & ":0")
            Catch ex As Exception
                Return DateTime.MinValue
            End Try
            Return retVal
        End Function
    End Class
End Namespace