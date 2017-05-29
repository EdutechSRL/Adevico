Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoNewsItem
        Public ID As Integer
        Public Name As String
        Public DefaultUrl As String
        Public News As List(Of dtoModuleMessage)
        Public Sub New()
            News = New List(Of dtoModuleMessage)
        End Sub
    End Class
End Namespace