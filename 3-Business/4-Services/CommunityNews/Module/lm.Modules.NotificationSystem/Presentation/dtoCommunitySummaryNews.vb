Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoCommunitySummaryNews
        Public ID As Integer
        Public Name As String
        Public DetailView As String
        Public News As List(Of dtoSummaryNews)

        Public Function UpdateDetailUrl(ByVal url As String) As dtoCommunitySummaryNews
            Me.DetailView = url
            Return Me
        End Function
    End Class
End Namespace