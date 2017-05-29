Namespace lm.Comol.Core.Cache
	Public Class FakeCache
		Implements iCache
		Private _DefaultExpiration As TimeSpan

		Public Function Add(ByVal Key As String, ByVal objValue As Object) As Boolean Implements iCache.Add
			Return False
		End Function
		Public Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oMilliSeconds As Long) As Boolean Implements iCache.Add
			Return False
		End Function
		Public Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oTime As System.TimeSpan) As Boolean Implements iCache.Add
			Return False
		End Function
		Public Function [Get](ByVal Key As String) As Object Implements iCache.Get
			Return Nothing
		End Function
		Public Function [Get](ByVal Keys As System.Collections.Generic.List(Of String)) As System.Collections.Generic.IDictionary(Of String, Object) Implements iCache.Get
			Return New Dictionary(Of String, Object)
		End Function
		Public Function myType() As String Implements iCache.myType
			Return Me.GetType.Name
		End Function
		Public Sub Remove(ByVal Key As String) Implements iCache.Remove

		End Sub
		Public Sub RemoveAll() Implements iCache.RemoveAll

		End Sub
		Public Sub RemoveByPrefix(ByVal Prefix As String) Implements iCache.RemoveByPrefix

		End Sub

		Public Function Exsist(ByVal Key As String) As Object Implements iCache.Exsist
			Return False
		End Function
		Public Property DefaultExpiration() As System.TimeSpan Implements iCache.DefaultExpiration
			Get
				If IsNothing(_DefaultExpiration) Then
					_DefaultExpiration = New TimeSpan(0, 20, 0)
				End If
				Return _DefaultExpiration
			End Get
			Set(ByVal value As System.TimeSpan)
				_DefaultExpiration = value
			End Set
		End Property

		Public Function GetByPrefix(ByVal Prefix As String) As System.Collections.Generic.IDictionary(Of String, Object) Implements iCache.GetByPrefix
			Return New Dictionary(Of String, Object)
		End Function
	End Class
End Namespace