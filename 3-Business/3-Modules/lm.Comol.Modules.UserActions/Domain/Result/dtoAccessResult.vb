Namespace lm.Comol.Modules.UsageResults.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoAccessResult
        Public PersonID As Integer
        Public PersonName As String
        Public CommunityID As Integer
        Public CommunityName As String
        Public Day As Date
        Public Hour As Integer
        Public UsageTime As Integer
        Public NavigateTo As String
        Public Sub New()

        End Sub

        Public Function ToTimeSpan() As TimeSpan
            Return New TimeSpan(0, 0, UsageTime)
        End Function
        Public Function HourToInterval() As String
            If Hour = 23 Then
                Return Hour.ToString & " - " & "24"
            ElseIf Hour > 9 Then
                Return Hour.ToString & " - " & (Hour + 1).ToString
            ElseIf Hour < 10 AndAlso Hour + 1 > 9 Then
                Return "0" & Hour.ToString & " - " & (Hour + 1).ToString
            Else
                Return "0" & Hour.ToString & " - " & "0" & (Hour + 1).ToString
            End If
        End Function
        Public Shared Function Create(dto As dtoAccessResult, userName As String, communityName As String) As dtoAccessResult
            dto.PersonName = userName
            dto.CommunityName = communityName
            Return dto
        End Function
    End Class

End Namespace