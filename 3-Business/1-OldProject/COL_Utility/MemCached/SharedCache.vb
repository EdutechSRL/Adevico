Public Class SharedCache
	Public Shared cache As iCache = CacheSetup()

	''' <summary>
	''' 
	''' </summary>
	''' <returns></returns>
	''' <remarks>
	'''   utilizzo:
	'''   imports TestCache.SharedCache
	'''   
	'''   una volta importato puoi usare direttamente l'oggetto shared cache
	''' </remarks>
	Public Shared Function CacheSetup() As iCache
		'Modificare utilizzando una stringa da configurazione...
		Dim classeCache As String = "UtilityLibrary.DotNetCache"
		'Possibili valori:
		'   UtilityLibrary.DotNetCache          -> utilizza HttpContext.Current.Application
		'   UtilityLibrary.DistribuitedCache    -> utilizza Enyim.Caching.MemcachedClient, tramite "MemCached.vb"
		Return FactoryBuilder.BuildFactory(Of iCache)(classeCache)
	End Function

End Class