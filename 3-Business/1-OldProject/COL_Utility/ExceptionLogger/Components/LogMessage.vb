Imports System.Xml.Serialization

Namespace Logging
	<XmlRoot(ElementName:="LogMessage")> _
  Public Class LogMessage

		Private _fired As DateTime
		Private _message As String
		Private _context As String
		Private _level As LogLevel


		Private _format As String = "{0} - {1} {2} - {3} - {4} - {5}"
		Private _guid As Guid
		Private _usermessage As String

		Public Sub New()
			Guid = System.Guid.NewGuid
		End Sub

		Public Sub New(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "", Optional ByVal level As LogLevel = LogLevel.Debug)
			Me.New()
			Me.Message = message
			Me.Context = context
			Me.UserMessage = usermessage
			Me.FiredDate = Now
			Me.Level = level
		End Sub

		<XmlElement(ElementName:="Level")> _
		Public Property Level() As LogLevel
			Get
				Return _level
			End Get
			Set(ByVal value As LogLevel)
				_level = value
			End Set
		End Property

		<XmlElement(ElementName:="Date")> _
		Public Property FiredDate() As DateTime
			Get
				Return _fired
			End Get
			Set(ByVal value As DateTime)
				_fired = value
			End Set
		End Property

		<XmlElement(ElementName:="Message")> _
		Public Property Message() As String
			Get
				Return _message
			End Get
			Set(ByVal value As String)
				_message = value
			End Set
		End Property

		<XmlElement(ElementName:="Context")> _
		Public Property Context() As String
			Get
				Return _context
			End Get
			Set(ByVal value As String)
				_context = value
			End Set
		End Property


		Public Property Format() As String
			Get
				Return _format
			End Get
			Set(ByVal value As String)
				_format = value
			End Set
		End Property

		Public Property Guid() As Guid
			Get
				Return _guid
			End Get
			Set(ByVal value As Guid)
				_guid = value
			End Set
		End Property

		Public Property UserMessage() As String
			Get
				Return _usermessage
			End Get
			Set(ByVal value As String)
				_usermessage = value
			End Set
		End Property

		Public Overrides Function ToString() As String
			'_format As String="{0} - {1} {2} - {3} - {4} - {5}"
			Return String.Format(Format, "Log", FiredDate.ToShortDateString, FiredDate.ToShortTimeString, Level, Context, Message)
		End Function

	End Class

End Namespace