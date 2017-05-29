Imports System.Net.Mail

Namespace Logging
	Public Class MailLogWriter
		Inherits GenericLogWriter


		Private _smtp As String
		Private _from As String
		Private _to As String
		Private _subjectfrmt As String
		Private _messagefrmt As String
		Private _subject As String

		Private _mail As MailMessage

		Public Property FromAddress() As String
			Get
				Return _from
			End Get
			Set(ByVal value As String)
				_from = value
			End Set
		End Property

		Public Property ToAddress() As String
			Get
				Return _to
			End Get
			Set(ByVal value As String)
				_to = value
			End Set
		End Property

		Public Property SMTP() As String
			Get
				Return _smtp
			End Get
			Set(ByVal value As String)
				_smtp = value
			End Set
		End Property

		Public Property FrmtMessage() As String
			Get
				Return _messagefrmt
			End Get
			Set(ByVal value As String)
				_messagefrmt = value
			End Set
		End Property

		Public Property FrmtSubject() As String
			Get
				Return _subjectfrmt
			End Get
			Set(ByVal value As String)
				_subjectfrmt = value
			End Set
		End Property

		Public Property subject() As String
			Get
				Return _subject
			End Get
			Set(ByVal value As String)
				_subject = value
			End Set
		End Property

		Public Overrides Sub Write()
            Try
                _mail = New MailMessage
                _mail.IsBodyHtml = True

                _mail.Sender = New MailAddress(FromAddress)
                _mail.To.Add(New MailAddress(ToAddress))

                _mail.From = _mail.Sender

                _mail.Subject = subject

                Dim smtpcln As New System.Net.Mail.SmtpClient
                smtpcln.Host = SMTP
                smtpcln.Port = 25

                For Each m As LogMessage In Messages
                    If (m.Level >= MinLevel) And (m.Level <= MaxLevel) Then
                        appendstr(m.ToString)
                    End If
                Next

                If _mail.Body <> "" Then
                    smtpcln.Send(_mail)
                End If

            Catch ex As Exception

            End Try

		End Sub

		Private Sub appendstr(ByVal message As String)
			_mail.Body += message + "<br />"
		End Sub


	End Class
End Namespace