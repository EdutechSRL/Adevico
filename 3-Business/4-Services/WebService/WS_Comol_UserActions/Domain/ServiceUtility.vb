Imports lm.ActionDataContract
Imports lm.WS.UserAction.Configuration
Imports lm.Comol.Core.Cache

Namespace lm.WS.UserAction.Domain
	Public Class ServiceUtility
		Public Shared Config As ServiceConfiguration = ServiceConfiguration.CreateConfigSettings
        Public Shared CurrentCache As lm.Comol.Core.Cache.iCache = CacheFactory.Cache(Config.DefaultTimeToLive)

		Public Shared ActionKey As String = Config.CacheKey(ServiceConfiguration.KeyType.Action)
		Public Shared LastActionKey As String = Config.CacheKey(ServiceConfiguration.KeyType.LastAction)
		Public Shared LoginKey As String = Config.CacheKey(ServiceConfiguration.KeyType.Login)
		Public Shared WebPresenceKey As String = Config.CacheKey(ServiceConfiguration.KeyType.WebPresence)
	End Class
End Namespace