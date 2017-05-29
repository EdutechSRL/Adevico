Imports System.Xml.Serialization

Namespace Logging
	Public Class XmlLogWriter
		Inherits GenericLogWriter


		Public Overrides Sub Write()
			Dim st As String = "<Logs>"

			For Each m As LogMessage In Messages

				If (m.Level >= MinLevel) And (m.Level <= MaxLevel) Then
					Dim oXS As XmlSerializer = New XmlSerializer(GetType(LogMessage))
					Dim sw As New IO.StringWriter()
					oXS.Serialize(sw, m)
					st += sw.ToString
				End If
			Next

			st += "</Logs>"

		End Sub

	End Class
End Namespace