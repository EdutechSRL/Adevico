Namespace lm.WS.UserAction.Configuration
	Public Class ServiceConfiguration

#Region "Private"
		Private _PoisonQueueName As String
		Private _ActionQueue As String
		Private _OnLinePresence As Boolean
		Private _UsageTime As Boolean
		Private _LogonAction As Boolean
		Private _UserAction As Boolean
		Private _PersistBrowser As Boolean
		Private _DefaultTimeToLive As TimeSpan
		Private _ActionQueueTransaction As Messaging.MessageQueueTransactionType
		Private _CacheTimeToLive As IList(Of KeyComposite(Of KeyType, TimeSpan))
		Private _CacheKeys As IList(Of KeyComposite(Of KeyType, String))
        Private _MessageQueueServiceClass As String
        Private _OnLineUserTimeToLive As TimeSpan


#End Region

#Region "Public"
		Public Property ActionQueueTransaction() As Messaging.MessageQueueTransactionType
			Get
				Return _ActionQueueTransaction
			End Get
			Set(ByVal value As Messaging.MessageQueueTransactionType)
				_ActionQueueTransaction = value
			End Set
		End Property
		Public Property ActionQueueName() As String
			Get
				Return _ActionQueue
			End Get
			Set(ByVal value As String)
				_ActionQueue = value
			End Set
		End Property
		Public Property PoisonQueueName() As String
			Get
				Return _PoisonQueueName
			End Get
			Set(ByVal value As String)
				_PoisonQueueName = value
			End Set
		End Property
		Public Property OnLinePresence() As Boolean
			Get
				Return _OnLinePresence
			End Get
			Set(ByVal value As Boolean)
				_OnLinePresence = value
			End Set
		End Property
		Public Property PersistUsageTime() As Boolean
			Get
				Return _UsageTime
			End Get
			Set(ByVal value As Boolean)
				_UsageTime = value
			End Set
		End Property
		Public Property PersistBrowser() As Boolean
			Get
				Return _PersistBrowser
			End Get
			Set(ByVal value As Boolean)
				_PersistBrowser = value
			End Set
		End Property
		Public Property PersistLogonAction() As Boolean
			Get
				Return _LogonAction
			End Get
			Set(ByVal value As Boolean)
				_LogonAction = value
			End Set
		End Property
		Public Property PersistUserAction() As Boolean
			Get
				Return _UserAction
			End Get
			Set(ByVal value As Boolean)
				_UserAction = value
			End Set
		End Property
		Public Property CacheTimesToLive() As IList(Of KeyComposite(Of KeyType, TimeSpan))
			Get
				Return _CacheTimeToLive
			End Get
			Set(ByVal value As IList(Of KeyComposite(Of KeyType, TimeSpan)))
				_CacheTimeToLive = value
			End Set
		End Property
		Public Property CacheKeys() As IList(Of KeyComposite(Of KeyType, String))
			Get
				Return _CacheKeys
			End Get
			Set(ByVal value As IList(Of KeyComposite(Of KeyType, String)))
				_CacheKeys = value
			End Set
		End Property
		Public ReadOnly Property CacheKey(ByVal oType As KeyType) As String
			Get
				Return (From o In _CacheKeys Where o.Key = oType Select o.Value).FirstOrDefault
			End Get
		End Property
		Public ReadOnly Property CacheTimeToLive(ByVal oType As KeyType) As TimeSpan
			Get
				Dim oTimeSpan As TimeSpan = (From o In _CacheTimeToLive Where o.Key = oType Select o.Value).FirstOrDefault
				If IsNothing(oTimeSpan) Then
					oTimeSpan = _DefaultTimeToLive
				End If
				Return oTimeSpan
			End Get
		End Property
		Public Property DefaultTimeToLive() As TimeSpan
			Get
				Return _DefaultTimeToLive
			End Get
			Set(ByVal value As TimeSpan)
				_DefaultTimeToLive = value
			End Set
		End Property
		Private Shared ReadOnly Property Config(ByVal KeyName As String) As String
			Get
				Return System.Configuration.ConfigurationManager.AppSettings(KeyName)
			End Get
		End Property
		Public Property MessageQueueServiceClass() As String
			Get
				Return _MessageQueueServiceClass
			End Get
			Set(ByVal value As String)
				_MessageQueueServiceClass = value
			End Set
        End Property

        Public Property OnLineUserTimeToLive() As TimeSpan
            Get
                Return _OnLineUserTimeToLive
            End Get
            Set(ByVal value As TimeSpan)
                _OnLineUserTimeToLive = value
            End Set
        End Property

#End Region

		Sub New()
			Me._LogonAction = True
			Me._OnLinePresence = True
			Me._PoisonQueueName = ""
			Me._ActionQueue = ""
			Me._UsageTime = True
			Me._UserAction = False
			Me._DefaultTimeToLive = New TimeSpan(0, 20, 0)
			Me._CacheKeys = New List(Of KeyComposite(Of KeyType, String))
            Me._CacheTimeToLive = New List(Of KeyComposite(Of KeyType, TimeSpan))
            Me._OnLineUserTimeToLive = New TimeSpan(0, 20, 0)
		End Sub

		Public Shared Function CreateConfigSettings() As ServiceConfiguration
			Dim oConfig As New ServiceConfiguration

			oConfig.PersistLogonAction = Config("LogonAction")
			oConfig.PoisonQueueName = Config("PoisonQueueName")
			oConfig.ActionQueueName = Config("QueueName")
			oConfig.PersistUsageTime = Config("UsageTime")
			oConfig.PersistUserAction = Boolean.TryParse(Config("UserAction"), True)
			oConfig.PersistBrowser = Boolean.TryParse(Config("PersistBrowser"), True)
			oConfig.ActionQueueTransaction = IIf(Boolean.TryParse(Config("PersistBrowser"), False), Messaging.MessageQueueTransactionType.Single, Messaging.MessageQueueTransactionType.None)


            oConfig.OnLineUserTimeToLive = New TimeSpan(0, 0, Integer.Parse(Config("OnLineUserTimeToLive")))

			Dim oSeconds As Integer = Integer.Parse(Config("DefaultTimeToLive"))
			oConfig.DefaultTimeToLive = New TimeSpan(0, 0, oSeconds)

            oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.Login, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CacheLoginTimeToLive")))})

			oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.LastAction, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CacheLastActionTimeToLive")))})

			oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.Action, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CacheActionTimeToLive")))})

            oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.WebPresence, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CachePresenceTimeToLive")))})

			oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.Action, .Value = Config("CacheActionKey")})

			oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.LastAction, .Value = Config("CacheLastActionKey")})

			oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.Login, .Value = Config("CacheLoginKey")})

			oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.WebPresence, .Value = Config("CachePresenceKey")})

			oConfig.MessageQueueServiceClass = Config("MessageQueueServiceClass")
			Return oConfig
		End Function

		Enum KeyType
			LastAction
			Login
			Action
			WebPresence
		End Enum
	End Class
End Namespace