Namespace lm.Comol.Modules.UsageResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoUserResult
        Public IdPerson As Integer
        Public IdCommunity As Integer
        Public UsageTime As Integer
        Public StartDate As DateTime
        Public EndDate As DateTime
        Public IsClosed As Boolean

        Public Sub New()

        End Sub

        Public Shared Function CreateToDate(dto As dtoUserResult, usageTime As Integer, endDate As DateTime) As dtoUserResult
            dto.UsageTime = usageTime
            dto.EndDate = endDate
            Return dto
        End Function

        Public Shared Function CreateFromDate(dto As dtoUserResult, usageTime As Integer, startDate As DateTime) As dtoUserResult
            dto.UsageTime = usageTime
            dto.StartDate = startDate
            Return dto
        End Function
    End Class
End Namespace