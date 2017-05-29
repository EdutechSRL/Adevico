Imports System.Web

Namespace lm.Comol.Core.Cache
	Public Class HttpCache
		Implements iCache

		Private _DefaultExpiration As TimeSpan
		Private Shared Cache As System.Web.Caching.Cache = System.Web.HttpContext.Current.Cache

		Public Function Add(ByVal Key As String, ByVal objValue As Object) As Boolean Implements iCache.Add
			Cache.Insert(Key, objValue)
		End Function
		Public Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oMilliSeconds As Long) As Boolean Implements iCache.Add
            Cache.Insert(Key, objValue, Nothing, Cache.NoAbsoluteExpiration, New TimeSpan(oMilliSeconds))
		End Function
		Public Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oTime As System.TimeSpan) As Boolean Implements iCache.Add
            Cache.Insert(Key, objValue, Nothing, Cache.NoAbsoluteExpiration, oTime)
		End Function
		Public Function [Get](ByVal Key As String) As Object Implements iCache.Get
			Return Cache.Get(Key)
		End Function
		Public Function [Get](ByVal Keys As System.Collections.Generic.List(Of String)) As System.Collections.Generic.IDictionary(Of String, Object) Implements iCache.Get
			Dim oDictionary As IDictionary(Of String, Object) = New Dictionary(Of String, Object)
			For Each Key In Keys
				oDictionary.Add(Key, Cache.Get(Key))
			Next
			Return oDictionary
		End Function
		Public Function myType() As String Implements iCache.myType
			Return Me.GetType.Name
		End Function
		Public Sub Remove(ByVal Key As String) Implements iCache.Remove
			Cache.Remove(Key)
		End Sub
		Public Sub RemoveAll() Implements iCache.RemoveAll
			Dim itemsToRemove As New List(Of String)
			Dim enumerator As IDictionaryEnumerator = System.Web.HttpContext.Current.Cache.GetEnumerator()
			While enumerator.MoveNext
				itemsToRemove.Add(enumerator.Key.ToString)
			End While
			Me.RemoveItems(itemsToRemove)
		End Sub
		Public Sub RemoveByPrefix(ByVal Prefix As String) Implements iCache.RemoveByPrefix
			Prefix = Prefix.ToLower
			Dim itemsToRemove As New List(Of String)
			Dim enumerator As IDictionaryEnumerator = Cache.GetEnumerator()
			While enumerator.MoveNext
				If enumerator.Key.ToString.ToLower.StartsWith(Prefix) Then
					itemsToRemove.Add(enumerator.Key.ToString)
				End If
			End While
			Me.RemoveItems(itemsToRemove)
		End Sub
		Private Sub RemoveItems(ByVal oList As List(Of String))
			For Each itemToRemove As String In oList
				Cache.Remove(itemToRemove)
			Next
		End Sub
		Public Function Exsist(ByVal Key As String) As Object Implements iCache.Exsist
			Return Not IsNothing(Cache.Get(Key))
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
			Dim oDictionary As IDictionary(Of String, Object) = New Dictionary(Of String, Object)
			For Each Key In FindItemsByKey(Prefix)
				Dim o As Object = Cache.Get(Key)
				If Not IsNothing(o) Then
					oDictionary.Add(Key, Cache.Get(Key))
				End If
			Next
			Return oDictionary
		End Function

		Private Function FindItemsByKey(ByVal Prefix As String) As List(Of String)
			Prefix = Prefix.ToLower
			Dim items As New List(Of String)
			Dim enumerator As IDictionaryEnumerator = Cache.GetEnumerator()
			While enumerator.MoveNext
				If enumerator.Key.ToString.ToLower.StartsWith(Prefix) Then
					items.Add(enumerator.Key.ToString)
				End If
			End While
			Return items
		End Function

	End Class
End Namespace