'Imports System.IO

Namespace Logging


	Public Class TxtLogWriter
		Inherits GenericLogWriter

		Private _filename As String
		Private _maxsize As Integer
		Private _storeddir As String
		Private _storedname As String

		Public Property Filename() As String
			Get
				Return _filename
			End Get
			Set(ByVal value As String)
                _filename = value
                lm.Comol.Core.File.Create.TextFile(value, "", False, False)
            End Set
		End Property

		Public Property StoredName() As String
			Get
				Return _storedname
			End Get
			Set(ByVal value As String)
				_storedname = value
			End Set
		End Property

		Public Property MaxSize() As Integer
			Get
				Return _maxsize
			End Get
			Set(ByVal value As Integer)
				_maxsize = value
			End Set
		End Property

		Public Property StoredDir() As String
			Get
				Return _storeddir
			End Get
			Set(ByVal value As String)
				_storeddir = value
			End Set
		End Property

		Public Sub New()
			MyBase.new()
		End Sub

		Private Function newfilename() As String
			Return Format(Now.Year, "0000") + Format(Now.Month, "00") + Format(Now.Day, "00") + "-" + Now.ToShortTimeString + "-" + StoredName + ".config"
		End Function

		Public Overrides Sub Write()
			For Each m As LogMessage In Messages
				If (m.Level >= MinLevel) And (m.Level <= MaxLevel) Then
					appendStr(m.ToString)
				End If
			Next
		End Sub

		Protected Sub appendStr(ByVal message As String)
			Try
                If lm.Comol.Core.File.ContentOf.File_Size(Filename) > MaxSize Then
                    lm.Comol.Core.File.Create.CopyFile(Filename, StoredDir + newfilename())
                    lm.Comol.Core.File.Create.TextFile(Filename, message + vbCrLf, True, False)
                Else
                    lm.Comol.Core.File.Create.TextFile(Filename, message + vbCrLf, False, True)
                End If
			Catch ex As Exception

			End Try
		End Sub
	End Class

End Namespace