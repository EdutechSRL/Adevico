Imports System.Web

Namespace lm.Comol.Core.Cache
	Public Class ApplicationCache
		Implements iCache

		Private _DefaultExpiration As TimeSpan
		Private Shared KeysList As List(Of String)

		Public Function Add(ByVal Key As String, ByVal objValue As Object) As Boolean Implements iCache.Add
			HttpContext.Current.Application(Key) = objValue
			Return True
		End Function
		Public Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oMilliSeconds As Long) As Boolean Implements iCache.Add
			HttpContext.Current.Application(Key) = objValue
			Return True
		End Function
		Public Function Add(ByVal Key As String, ByVal objValue As Object, ByVal oTime As System.TimeSpan) As Boolean Implements iCache.Add
			HttpContext.Current.Application(Key) = objValue
			Return True
		End Function
		Public Function [Get](ByVal Key As String) As Object Implements iCache.Get
			Return HttpContext.Current.Application(Key)
		End Function
		Public Function [Get](ByVal Keys As System.Collections.Generic.List(Of String)) As System.Collections.Generic.IDictionary(Of String, Object) Implements iCache.Get
			Dim oDictionary As New Dictionary(Of String, Object)
			For Each Key In Keys
				oDictionary.Add(Key, HttpContext.Current.Application(Key))
			Next
			Return oDictionary
		End Function

		Public Function myType() As String Implements iCache.myType
			Return Me.GetType.Name
		End Function

		Public Sub Remove(ByVal Key As String) Implements iCache.Remove
			HttpContext.Current.Application(Key) = Nothing
		End Sub
		Public Sub RemoveAll() Implements iCache.RemoveAll
			Dim KeyToRemove As New List(Of String)
			For Each Key In KeysList
				HttpContext.Current.Application(Key) = Nothing
				KeyToRemove.Add(Key)
			Next
			For Each Key In KeyToRemove
				HttpContext.Current.Application(Key) = Nothing
				KeysList.Remove(Key)
			Next
		End Sub
		Public Sub RemoveByPrefix(ByVal Prefix As String) Implements iCache.RemoveByPrefix
			Prefix = Prefix.ToLower
			Dim oList As List(Of String) = (From o In HttpContext.Current.Application.AllKeys Where o.ToLower.StartsWith(Prefix)).ToList
			For Each Key In oList
				HttpContext.Current.Application(Key) = Nothing
			Next
		End Sub
		Public Function Exsist(ByVal Key As String) As Object Implements iCache.Exsist
			Return Not IsNothing(HttpContext.Current.Application(Key))
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
			Dim oDictionary As New Dictionary(Of String, Object)
			For Each Key In HttpContext.Current.Application.AllKeys
				If Key.StartsWith(Prefix) Then
					oDictionary.Add(Key, HttpContext.Current.Application(Key))
				End If
			Next
			Return oDictionary
		End Function
	End Class
End Namespace