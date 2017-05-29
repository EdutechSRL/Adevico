Public Interface iCache
    Function GetByKey(ByVal key As Object) As Object
    Function SetByKey(ByVal key As Object, ByVal obj As Object) As Boolean
    Function DeleteByKey(ByVal key As Object) As Boolean
    Function myType() As String

    Sub RemoveNamespace(ByVal key As Object)
End Interface
