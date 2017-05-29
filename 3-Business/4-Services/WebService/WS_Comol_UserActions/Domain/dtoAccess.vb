Imports lm.ActionDataContract
Imports lm.Comol.Core.Cache
Imports lm.WS.UserAction.Configuration
Imports lm.WS.UserAction.Domain


Namespace lm.Comol.Services.WS.UserAction.Domain
	<Serializable(), CLSCompliant(True)> Public Class dtoAccess
		Public ID As Integer
		Public WorkingSessionID As System.Guid
	End Class
End Namespace