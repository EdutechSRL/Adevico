Imports lm.ActionDataContract
Imports lm.WS.UserAction.Domain

Namespace lm.WS.UserAction.Domain
	Public Class CacheActions(Of T)
		'Implements iCacheAction(Of T)

		Public Shared ReadOnly Property CurrentCache() As Comol.Core.Cache.iCache 'Implements iCacheAction(Of T).CurrentCache
			Get
				Return ServiceUtility.CurrentCache
			End Get
		End Property

		Sub New()

		End Sub

		Public Shared Sub SendToCache(ByVal oAction As T, ByVal Key As String) 'Implements iCacheAction(Of T).SendToCache
			CurrentCache.Add(Key, oAction, CurrentCache.DefaultExpiration)
		End Sub
		Public Shared Function GetFromCache(ByVal Key As String) As T 'Implements iCacheAction(Of T).GetFromCache
			Return CurrentCache.Get(Key)
		End Function

		Public Shared Sub RemoveFromCache(ByVal Key As String) 'Implements iCacheAction(Of T).RemoveFromCache
			CurrentCache.Remove(Key)
		End Sub
		Public Shared Sub ClearCache(ByVal Prefix As String) ' Implements iCacheAction(Of T).ClearCache
			CurrentCache.RemoveByPrefix(Prefix)
		End Sub

		Public Shared Function GetByPrefix(ByVal Prefix As String) As List(Of T)	 ' Implements iCacheAction(Of T).ClearCache
			Return (From o In CurrentCache.GetByPrefix(Prefix) Select DirectCast(o.Value, T)).ToList
		End Function
	End Class
End Namespace