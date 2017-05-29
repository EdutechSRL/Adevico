Namespace Logging
	Public MustInherit Class GenericLogWriter

#Region "Private Property"
		Private _IsEnabled As Boolean = True
		Private _Minlevel As LogLevel
		Private _Maxlevel As LogLevel
		Private _Maxmessage As Integer
		Private _Messages As List(Of LogMessage)
#End Region

#Region "Public Property"
		Public Property IsEnabled() As Boolean
			Get
				Return _IsEnabled
			End Get
			Set(ByVal value As Boolean)
				_IsEnabled = value
			End Set
		End Property
		Public Property MinLevel() As LogLevel
			Get
				Return _Minlevel
			End Get
			Set(ByVal value As LogLevel)
				_Minlevel = value
			End Set
		End Property
		Public Property MaxLevel() As LogLevel
			Get
				Return _Maxlevel
			End Get
			Set(ByVal value As LogLevel)
				_Maxlevel = value
			End Set
		End Property
		Public Property MaxMessages() As Integer
			Get
				Return _Maxmessage
			End Get
			Set(ByVal value As Integer)
				_Maxmessage = value
			End Set
		End Property
		Public Property Messages() As List(Of LogMessage)
			Get
				Return _Messages
			End Get
			Set(ByVal value As List(Of LogMessage))
				_Messages = value
			End Set
		End Property
#End Region

		Public Sub New()
			Messages = New List(Of LogMessage)
		End Sub

		Public Sub AddMessage(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "", Optional ByVal level As LogLevel = LogLevel.Debug)
			Dim m As New LogMessage(message, context, usermessage, level)
			AddMessage(m)
		End Sub

		Public Sub AddMessage(ByVal oLogMessage As LogMessage)
			Messages.Add(oLogMessage)
			If Messages.Count >= MaxMessages Then
				Write()
				Messages.Clear()
			End If
		End Sub

		Public MustOverride Sub Write()
	End Class
End Namespace