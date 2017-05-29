Imports System.Reflection

Namespace Logging
	Public Class SharedLogger

		Public Shared Logger As ExceptionLogger = getLogger(System.Reflection.MethodBase.GetCurrentMethod.DeclaringType)

		Public Shared Sub LoggerSetup(ByVal path As String)
			'Dim log As GenericLogWriter

			'log = New TxtLogWriter
			'CType(log, TxtLogWriter).Filename = path + "general.logs"
			'CType(log, TxtLogWriter).MaxSize = 100 * SizeUtil.Kilo
			'CType(log, TxtLogWriter).StoredDir = path + "backup\"
			'CType(log, TxtLogWriter).StoredName = "general"
			'log.MaxMessages = 1
			'log.MinLevel = LogLevel.Info
			'log.MaxLevel = LogLevel.Fatal
			'log.IsEnabled = True

			'Logger.Writers.Add(log)

			'log = New CsvLogWriter
			'CType(log, CsvLogWriter).Filename = path + "general.csv"
			'CType(log, CsvLogWriter).MaxSize = 1 * SizeUtil.Mega
			'CType(log, CsvLogWriter).StoredDir = path + "backup\"
			'CType(log, CsvLogWriter).StoredName = "general.csv"
			'log.MaxMessages = 1
			'log.MinLevel = LogLevel.Info
			'log.MaxLevel = LogLevel.Fatal
			'log.IsEnabled = True

			'Logger.Writers.Add(log)

			'log = New TxtLogWriter
			'CType(log, TxtLogWriter).Filename = path + "errors.logs"
			'CType(log, TxtLogWriter).MaxSize = 1 * SizeUtil.Mega
			'CType(log, TxtLogWriter).StoredDir = path + "backup\"
			'CType(log, TxtLogWriter).StoredName = "errors"
			'log.MaxMessages = 1
			'log.MinLevel = LogLevel.CodeError
			'log.MaxLevel = LogLevel.Fatal
			'log.IsEnabled = True

			'Logger.Writers.Add(log)

			'log = New MailLogWriter
			'CType(log, MailLogWriter).SMTP = "mail.economia.unitn.it"
			'CType(log, MailLogWriter).subject = "[L3 Online]"
			'CType(log, MailLogWriter).FromAddress = "rmaschio@economia.unitn.it"
			'CType(log, MailLogWriter).ToAddress = "rmaschio@economia.unitn.it"
			'log.MaxMessages = 10
			'log.MinLevel = LogLevel.CodeError
			'log.MaxLevel = LogLevel.Fatal
			'log.IsEnabled = False

			'Logger.Writers.Add(log)

			'log = New DebugLogWriter
			'log.MaxMessages = 1
			'log.MinLevel = LogLevel.Info
			'log.MaxLevel = LogLevel.Fatal
			'log.IsEnabled = True

			'Logger.Writers.Add(log)

			'log = New DBLogWriter
			'CType(log, DBLogWriter).Sql = "INSERT INTO [L3_Logs].[dbo].[Logs] ([LOGS_Date],[LOGS_Level],[LOGS_Context],[LOGS_Message]) VALUES ('{0}','{1}','{2}','{3}')"
			'CType(log, DBLogWriter).ConnectionString = "Data Source=DOMAINGALAXY;Initial Catalog=L3_Logs;Persist Security Info=True;User ID=legge6;Password=lmadm1"
			'log.MaxMessages = 1
			'log.MinLevel = LogLevel.CodeError
			'log.MaxLevel = LogLevel.Fatal
			'log.IsEnabled = True

			'Logger.Writers.Add(log)

		End Sub

		Public Shared Sub LoggerContext(ByVal t As Type)
			Logger.Launcher = t
		End Sub

		Public Shared Function getLogger(ByVal t As Type) As ExceptionLogger
			Return New ExceptionLogger(t)
		End Function

	End Class
End Namespace