'Namespace lm.Comol.Services.WS.UserAction.Domain
'	Public Class ServiceCache(Of T)

'		Public Shared Sub SendActionToCache(ByVal oAction As T, ByVal oKey As String)
'			ServiceAction.CurrentLoginCache.Add(oKey, oAction)
'		End Sub
'		Public Shared Function GetActionFromCache(ByVal oKey As String) As T
'			Dim oKey As String = String.Format(_CacheKey, WorkingSessionID, PersonID)
'			Return ServiceAction.CurrentLoginCache.Get(oKey)
'		End Function
'		Public Shared Sub RemoveActionToCache(ByVal oKey As String)
'			ServiceAction.CurrentLoginCache.Remove(oKey)
'		End Sub
'	End Class
'End Namespace