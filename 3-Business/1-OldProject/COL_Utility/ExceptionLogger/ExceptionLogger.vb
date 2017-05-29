Imports System.Reflection

Namespace Logging
	Public Class ExceptionLogger

		Private _writers As List(Of GenericLogWriter)
		Private _currentlevel As LogLevel
		Private _launcher As Type

		Public Property Writers() As List(Of GenericLogWriter)
			Get
				Return _writers
			End Get
			Set(ByVal value As List(Of GenericLogWriter))
				_writers = value
			End Set
		End Property

		Public Property CurrentLevel() As LogLevel
			Get
				Return _currentlevel
			End Get
			Set(ByVal value As LogLevel)
				_currentlevel = value
			End Set
		End Property

		Public Sub New(Optional ByVal sender As Type = Nothing)
			Me._launcher = sender
			Writers = New List(Of GenericLogWriter)
		End Sub

		Public Property Launcher() As Type
			Get
				Return _launcher
			End Get
			Set(ByVal value As Type)
				_launcher = value
			End Set
		End Property

		Private Function extractfilename(ByVal st As String) As String
			If st <> "" Then
				Dim sts() As String
				sts = st.Split("\".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
				Return sts(sts.Length - 2) + "\" + sts(sts.Length - 1)
			Else
				Return st
			End If
		End Function

		Private Function ContextAnalyzerExtracted(ByVal st As StackTrace, ByVal start As Integer) As String
			Dim methodname As String = ""
			Dim classname As String = ""
			Dim filename As String = ""
			Dim linenumber As String = ""

			Dim str As String = ""


			Dim sf As System.Diagnostics.StackFrame
			Dim idx As Integer = start

			While (idx < st.FrameCount) And linenumber <> "0"
				sf = st.GetFrame(idx)

				Dim caller As System.Reflection.MethodBase = sf.GetMethod()
				Dim location As StackFrame = sf
				If location IsNot Nothing Then
					Dim method As System.Reflection.MethodBase = location.GetMethod
					If method IsNot Nothing Then
						methodname = method.Name
					End If
					If method.DeclaringType IsNot Nothing Then
						classname = method.DeclaringType.FullName
					End If
				End If

				filename = extractfilename(location.GetFileName())
				linenumber = location.GetFileLineNumber().ToString(System.Globalization.NumberFormatInfo.InvariantInfo)

				If linenumber <> "0" Then
					If idx <> start Then
						str += " | " + classname + " " + methodname + " " + filename + " " + linenumber
					Else
						str += "{" + classname + " " + methodname + " " + filename + " " + linenumber
					End If

				End If
				idx += 1
			End While
			str += "}"
			Return str
		End Function
		Private Sub ContextAnalyzer(ByVal m As LogMessage)



			Dim st As New StackTrace(True)

			''Dim p As String = caller.Name

			'Dim st As New StackTrace(True)
			'Dim st As StackTrace = sf
			'Dim frameindex As Integer = 0
			'Dim trovato As Boolean = False
			'While (frameindex < st.FrameCount) And (Not trovato)
			'    Dim frame As StackFrame = st.GetFrame(frameindex)

			'    If (frame IsNot Nothing) And (frame.GetMethod.DeclaringType Is Me.Launcher) Then
			'        trovato = True
			'    Else
			'        frameindex += 1
			'    End If
			'End While

			'trovato = False
			'While (frameindex < st.FrameCount) And (Not trovato)
			'    Dim frame As StackFrame = st.GetFrame(frameindex)
			'    If (frame IsNot Nothing) And (frame.GetMethod.DeclaringType IsNot Me.Launcher) Then
			'        trovato = True
			'    Else
			'        frameindex += 1
			'    End If
			'End While

			'If frameindex < st.FrameCount Then
			'Dim location As StackFrame = st.GetFrame(frameindex - 1)


			'For i As Integer = 3 To max - 1
			'str += ContextAnalyzerExtracted(st, i) + vbCrLf
			'Next
			'End If

			m.Context = ContextAnalyzerExtracted(st, 3)

		End Sub

		Private Sub submit(ByVal m As LogMessage)

			If m.Context = "" Then
				ContextAnalyzer(m)
			End If

			For Each w As GenericLogWriter In Writers
				If w.IsEnabled Then
					w.AddMessage(m)
				End If
			Next
		End Sub


		Public Sub Generic(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "", Optional ByVal level As LogLevel = LogLevel.Info)
			Dim m As New LogMessage(message, context, usermessage, level)
			submit(m)
		End Sub

		Public Sub Generic(ByVal m As LogMessage)
			submit(m)
		End Sub

		Public Sub Info(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "")
			Dim m As New LogMessage(message, context, usermessage, LogLevel.Info)
			submit(m)
		End Sub

		Public Sub Info(ByVal m As LogMessage)
			m.Level = LogLevel.Info
			submit(m)
		End Sub

		Public Sub Debug(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "", Optional ByVal level As LogLevel = LogLevel.Debug)
			Dim m As New LogMessage(message, context, usermessage, LogLevel.Debug)
			submit(m)
		End Sub

		Public Sub Debug(ByVal m As LogMessage)
			m.Level = LogLevel.Debug
			submit(m)
		End Sub

		Public Sub SaveError(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "", Optional ByVal level As LogLevel = LogLevel.Debug)
			Dim m As New LogMessage(message, context, usermessage, LogLevel.CodeError)
			submit(m)
		End Sub

		Public Sub SaveError(ByVal m As LogMessage)
			m.Level = LogLevel.CodeError
			submit(m)
		End Sub

		Public Sub Fatal(ByVal message As String, Optional ByVal context As String = "", Optional ByVal usermessage As String = "", Optional ByVal level As LogLevel = LogLevel.Debug)
			Dim m As New LogMessage(message, context, usermessage, LogLevel.Fatal)
			submit(m)
		End Sub

		Public Sub Fatal(ByVal m As LogMessage)
			m.Level = LogLevel.Fatal
			submit(m)
		End Sub
	End Class
End Namespace