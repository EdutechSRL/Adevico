Namespace Logging
	Public Class DebugLogWriter
		Inherits GenericLogWriter

		Public Overrides Sub Write()
			For Each oLogMessage As LogMessage In Messages
				If (oLogMessage.Level >= MinLevel) And (oLogMessage.Level <= MaxLevel) Then
					AppendStr(oLogMessage.ToString)
				End If
			Next
		End Sub

		Private Sub AppendStr(ByVal message As String)
			Try
				Debug.Write(message + vbCrLf)
			Catch ex As Exception

			End Try
		End Sub
	End Class
End Namespace