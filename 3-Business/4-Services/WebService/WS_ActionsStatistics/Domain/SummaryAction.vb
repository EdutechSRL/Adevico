Namespace lm.WS.ActionStatistics.Domain
    <Serializable(), CLSCompliant(True)> Public Class SummaryAction
        Public nAccesses As Integer
        Public totalTime As Integer
        Public Sub New()
            nAccesses = 0
            totalTime = 0
        End Sub
        Public Sub New(ByVal nAccess As Integer, ByVal totalT As Integer)
            nAccesses = nAccess
            totalTime = totalT
        End Sub
    End Class
End Namespace