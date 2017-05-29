Imports System.Web

Public Class DotNetCache
    Implements iCache


    Public Function myType() As String Implements iCache.myType
        Return Me.GetType.Name
    End Function

    Public Function DeleteByKey(ByVal key As Object) As Boolean Implements iCache.DeleteByKey
        HttpContext.Current.Application(key.ToString) = Nothing
    End Function

    Public Function GetByKey(ByVal key As Object) As Object Implements iCache.GetByKey
        Return HttpContext.Current.Application(key.ToString)
    End Function

    Public Sub RemoveNamespace(ByVal key As Object) Implements iCache.RemoveNamespace

    End Sub

    Public Function SetByKey(ByVal key As Object, ByVal obj As Object) As Boolean Implements iCache.SetByKey
        HttpContext.Current.Application(key.ToString) = obj
    End Function
End Class
