Namespace Presentation
    <CLSCompliant(True), Serializable()> Public Class dtoMultipleNews
        Implements IEqualityComparer(Of dtoMultipleNews)

        Public ID As Long
        Public Name As String
        Public DefaultUrl As String
        Public Multiples As List(Of dtoNewsItem)
        Public Sub New()
            Multiples = New List(Of dtoNewsItem)
        End Sub
        Public Sub New(ByVal oId As Long, ByVal oName As String, ByVal oUrl As String)
            ID = oId
            Name = oName
            DefaultUrl = oUrl
            Multiples = New List(Of dtoNewsItem)
        End Sub


        Public Overloads Function Equals(ByVal x As dtoMultipleNews, ByVal y As dtoMultipleNews) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of dtoMultipleNews).Equals
            Return x.ID.Equals(y.ID) AndAlso x.Name = y.Name
        End Function

        Public Overloads Function GetHashCode(ByVal obj As dtoMultipleNews) As Integer Implements System.Collections.Generic.IEqualityComparer(Of dtoMultipleNews).GetHashCode
            Return (obj.ID.ToString & " " & obj.ToString).GetHashCode
        End Function
    End Class
End Namespace