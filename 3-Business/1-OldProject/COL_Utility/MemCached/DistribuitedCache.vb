Public Class DistribuitedCache
    Implements iCache

    Public Function myType() As String Implements iCache.myType
        Return Me.GetType.Name
    End Function

    Public Function DeleteByKey(ByVal key As Object) As Boolean Implements iCache.DeleteByKey
        MemCached.Remove(key)
        Return Not MemCached.KeyExists(key)
    End Function

    Public Function GetByKey(ByVal key As Object) As Object Implements iCache.GetByKey
        Return MemCached.GetKey(key)
    End Function

    Public Sub RemoveNamespace(ByVal key As Object) Implements iCache.RemoveNamespace
        MemCached.Remove(key)
    End Sub

    Public Function SetByKey(ByVal key As Object, ByVal obj As Object) As Boolean Implements iCache.SetByKey
        MemCached.AddKey(key, obj)
        Return MemCached.KeyExists(key)
    End Function
End Class