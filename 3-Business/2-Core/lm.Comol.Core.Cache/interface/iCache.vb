Namespace lm.Comol.Core.Cache
	Public Interface iCache
		Property DefaultExpiration() As TimeSpan
		Function Add(ByVal Key As String, ByVal objValue As Object) As Boolean
		Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oTime As TimeSpan) As Boolean
		Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oMilliSeconds As Long) As Boolean
		Function Exsist(ByVal Key As String)
		Function GetByPrefix(ByVal Prefix As String) As IDictionary(Of String, Object)
		Function [Get](ByVal Keys As List(Of String)) As IDictionary(Of String, Object)
		Function [Get](ByVal Key As String) As Object
		Function myType() As String
		Sub Remove(ByVal Key As String)
		Sub RemoveByPrefix(ByVal Prefix As String)
		Sub RemoveAll()
	End Interface
End Namespace