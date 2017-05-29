Imports System.Web

Namespace lm.Comol.UI.Utility
	Public Class RemoteClientUtility
		Public Shared Function ClientAddress() As String
			Dim IpForwarded, IpVia, IpRemote, ClientIP As String
			IpForwarded = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
			IpVia = HttpContext.Current.Request.ServerVariables("HTTP_VIA")
			IpRemote = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
			ClientIP = ""
			If Not IpForwarded = "" Then
				If IpRemote = IpForwarded Then '2: VIA=FOR=Ip proxy
					'IpClient = "x.x.x.x"
					'IpProxy = IpRemote
					ClientIP = ""
				Else
					'1: VIA Ip del Proxy e x_for Ip del client
					'3: VIA Ip del proxy e x_for random IP
					ClientIP = IpForwarded
				End If
			Else
				'No Proxy
				'4: no proxy data. Remote_Addr Ip del proxy o IP del cliente senza proxy
				'  IpProxy = "x.x.x.x"
				ClientIP = IpRemote
			End If
			Return ClientIP
		End Function
		Public Shared Function ProxyAddress() As String
			Dim IpForwarded, IpVia, IpRemote, ProxyIP As String
			IpForwarded = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
			IpVia = HttpContext.Current.Request.ServerVariables("HTTP_VIA")
			IpRemote = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
			ProxyIP = ""
			If Not IpForwarded = "" Then
				ProxyIP = IpRemote
			Else
				'No Proxy
				'4: no proxy data. Remote_Addr Ip del proxy o IP del cliente senza proxy
				'  IpProxy = "x.x.x.x"
				ProxyIP = ""
			End If
			Return ProxyIP
		End Function

	End Class
End Namespace