Namespace Logging
	Public Class CsvLogWriter
		Inherits TxtLogWriter


		Private _overrideformat As String

		Public Property OverrideFormat() As String
			Get
				Return _overrideformat
			End Get
			Set(ByVal value As String)
				_overrideformat = value
			End Set
		End Property

		Public Sub New()
			OverrideFormat = "{0};{1};{2};{3};{4};{5}"
		End Sub

		Public Overrides Sub Write()
			For Each m As LogMessage In Messages
				m.Format = OverrideFormat
				If (m.Level >= MinLevel) And (m.Level <= MaxLevel) Then
					appendStr(m.ToString)
				End If
			Next
		End Sub
	End Class
End Namespace