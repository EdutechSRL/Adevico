Imports System.Linq

Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoDetailUsageStatistic
        Public Id As Long
        Public DisplayName As String
        Public Details As New List(Of dtoDetailsTime)
        Public Day As DateTime
        Public ReadOnly Property nAccesses As Long
            Get
                Return Details.Select(Function(d) d.nAccesses).Sum
            End Get
        End Property
        Public ReadOnly Property UsageTime As Long
            Get
                Return Details.Select(Function(d) d.UsageTime).Sum
            End Get
        End Property
        Public Function ToTimeSpan() As TimeSpan
            Return New TimeSpan(0, 0, UsageTime)
        End Function


    End Class
End Namespace