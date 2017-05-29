Imports System.Data.SqlClient

Namespace Logging
	Public Class DBLogWriter
		Inherits GenericLogWriter


		Private _connectionstring As String
		Private _sql As String
		Private cnx As SqlConnection

		Public Property ConnectionString() As String
			Get
				Return _connectionstring
			End Get
			Set(ByVal value As String)
				_connectionstring = value
			End Set
		End Property

		Public Property Sql() As String
			Get
				Return _sql
			End Get
			Set(ByVal value As String)
				_sql = value
			End Set
		End Property

		Public Overrides Sub Write()

			cnx = New SqlConnection(Me.ConnectionString)
			cnx.Open()
			For Each m As LogMessage In Messages

				If (m.Level >= MinLevel) And (m.Level <= MaxLevel) Then
					append(m)
				End If
			Next
			cnx.Close()
		End Sub

		Private Sub append(ByVal m As LogMessage)
			Try
				Dim cmdtext As String
				cmdtext = String.Format(Me.Sql, m.FiredDate, m.Level, m.Context, m.Message)
				Dim cmd As New SqlCommand(cmdtext, cnx)
				cmd.ExecuteNonQuery()
			Catch ex As Exception

			End Try
		End Sub
	End Class
End Namespace