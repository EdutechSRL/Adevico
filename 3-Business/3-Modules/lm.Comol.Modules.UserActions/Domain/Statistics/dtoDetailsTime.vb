Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoDetailsTime
        Public Day As Integer
        Public Month As Integer
        Public Year As Integer
        Public Hour As Integer
        Public nAccesses As Long
        Public UsageTime As Integer
        Public IdPerson As Integer
        Public Function ToTimeSpan() As TimeSpan
            Return New TimeSpan(0, 0, UsageTime)
        End Function
        Public Function ToDate() As DateTime
            Return DateSerial(Year, Month, Day)
        End Function
        Public Function ToDateHour() As DateTime
            Return DateSerial(Year, Month, Day).AddHours(Hour)
        End Function
        Public Function ToDate(ByVal withHour As Boolean) As DateTime
            Return IIf(withHour, ToDateHour(), ToDateHour())
        End Function
    End Class
End Namespace