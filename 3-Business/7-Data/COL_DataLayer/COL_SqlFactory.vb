Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.l
Public Class COL_SqlFactory : Inherits COL_AbstractFactory

    Private m_conSQL As New SqlConnection()

    Protected Overrides Sub Finalize()
        If Not (m_conSQL.State = ConnectionState.Closed) And _
           Not (m_conSQL.State = ConnectionState.Broken) Then
            Try
                m_conSQL.Close()
            Catch exSql As SqlException

            Catch ex As Exception

            End Try

        End If
        MyBase.Finalize()
    End Sub

    Public Overrides Function ExecuteDataReader(ByRef Request As COL_Request) As COL_DataReader
        ' Returns a COL_DataReader object, which wraps an object of type IDataReader
        ' Uses SQL Server .NET data provider, hence the wrapped object is a SqlDataReader object 
        ' A SqlDataReader object is a read-only, forward-only data stream. 
        ' NOTE: DataReaders won't be used in queries that perform transactions.
        Dim cmdSQL As New SqlCommand()
        Dim prmSQL As SqlParameter
        Dim oParam As COL_Request.Parameter
        Dim drSQL As SqlDataReader
        Dim oDataReaderSQL As New COL_SqlDataReader()
        Try
            m_conSQL.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conSQL.Open()
            cmdSQL.Connection = m_conSQL
            cmdSQL.CommandText = Request.Command
            cmdSQL.CommandType = Request.CommandType
            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmSQL = cmdSQL.Parameters.AddWithValue(oParam.Name, oParam.Value)
                Next
            End If
            drSQL = cmdSQL.ExecuteReader()
            oDataReaderSQL.ReturnedDataReader = drSQL
            Return oDataReaderSQL
        Catch exSQL As SqlException
            Debug.WriteLine(exSQL.Message)
            Request.Exception = exSQL

            'Dim oMail As New MailDBerrori
            'oMail.Oggetto = "SqlException"
            'oMail.Body = "Message=" & exSQL.Message & vbCrLf & "StackTrace=" & exSQL.StackTrace & vbCrLf & "SQL=" & Request.Command
            'oMail.InviaMail()
            Dim parameters As List(Of String)
            parameters = (From o In Request.Parameters Select o.Name & "= " & o.Value).ToList
            GenerateDBError.SendError(exSQL.Message, Request.Command, exSQL.StackTrace, parameters)

        Catch ex As Exception
            'Dim oMail As New MailDBerrori
            'oMail.Oggetto = "SqlException"
            'oMail.Body = "Message=" & ex.Message & vbCrLf & "StackTrace=" & ex.StackTrace & vbCrLf & "SQL=" & Request.Command
            'oMail.InviaMail()
            Dim parameters As List(Of String)
            parameters = (From o In Request.Parameters Select o.Name & "= " & o.Value).ToList
            GenerateDBError.SendError(ex.Message, Request.Command, ex.StackTrace, parameters)

            Debug.WriteLine(ex.Message)
            Request.Exception = ex
        Finally
        End Try
        Return Nothing
    End Function

    Public Overrides Function ExecuteDataSet(ByRef Request As COL_Request) As COL_DataSet
        ' Returns a COL_DataSet object, which wraps an object of type DataSet
        ' Uses SQL Server .NET data provider if a data provider is necessary 
        ' - hence uses a SqlDataAdapter to fill the DataSet
        Dim conSQL As New SqlConnection()
        Dim cmdSQL As New SqlCommand()
        Dim prmSQL As SqlParameter
        Dim oParam As COL_Request.Parameter
        Dim daSQL As SqlDataAdapter
        Dim oDataSetSQL As New COL_SqlDataSet()
        Dim tranSQL As SqlTransaction
        Try
            conSQL.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            ' open connection, and begin to set properties of command
            conSQL.Open()
            cmdSQL.Connection = conSQL
            cmdSQL.CommandText = Request.Command
            cmdSQL.CommandType = Request.CommandType
            cmdSQL.CommandTimeout = 90
            Dim gh As Integer
            gh = cmdSQL.CommandTimeout
            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmSQL = cmdSQL.Parameters.AddWithValue(oParam.Name, oParam.Value)
                Next
            End If

            If Request.transactional Then
                tranSQL = conSQL.BeginTransaction()
            End If

            daSQL = New SqlDataAdapter(cmdSQL)

            ' allow generic naming - NewDataSet
            daSQL.Fill(oDataSetSQL.ReturnedDataSet)

            Return oDataSetSQL

        Catch exSQL As SqlException
            Debug.WriteLine(exSQL.Message)
            Request.Exception = exSQL
            If Request.transactional Then
                tranSQL.Rollback()
            End If
            cmdSQL.Dispose()
            daSQL.Dispose()

            'Dim oMail As New MailDBerrori
            'oMail.Oggetto = "SqlException"
            'oMail.Body = "Message=" & exSQL.Message & vbCrLf & "StackTrace=" & exSQL.StackTrace & vbCrLf & "SQL=" & Request.Command

            'If Request.Parameters.Count > 0 Then
            '    oMail.Body &= vbCrLf & "Parameters:" & vbCrLf
            '    For Each oParam In Request.Parameters
            '        oMail.Body &= oParam.Name & "= " & oParam.Value & vbCrLf
            '    Next
            'End If

            'oMail.InviaMail()
            Dim parameters As List(Of String)
            parameters = (From o In Request.Parameters Select o.Name & "= " & o.Value).ToList
            GenerateDBError.SendError(exSQL.Message, Request.Command, exSQL.StackTrace, parameters)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex
            If Request.transactional Then
                tranSQL.Rollback()
            End If
            cmdSQL.Dispose()
            daSQL.Dispose()

            Dim parameters As List(Of String)
            parameters = (From o In Request.Parameters Select o.Name & "= " & o.Value).ToList
            GenerateDBError.SendError(ex.Message, Request.Command, ex.StackTrace, parameters)

            Debug.WriteLine(ex.Message)
            Request.Exception = ex
        Finally
            If Request.transactional Then
                tranSQL.Commit()
            End If
            If conSQL.State = ConnectionState.Open Then
                conSQL.Close()
            End If

        End Try
        Return Nothing
    End Function

    Public Overrides Function ExecuteNotQuery(ByRef Request As COL_Request) As COL_ExecuteNotQuery
        ' Returns a COL_DataReader object, which wraps an object of type IDataReader
        ' Uses SQL Server .NET data provider, hence the wrapped object is a SqlDataReader object 
        ' A SqlDataReader object is a read-only, forward-only data stream. 
        ' NOTE: DataReaders won't be used in queries that perform transactions.
        Dim cmdSQL As New SqlCommand()
        Dim prmSQL As SqlParameter
        Dim oParm As New COL_Request.Parameter()
        Dim oResponse As COL_SQLExecuteNotQuery

        Try
            m_conSQL.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conSQL.Open()
            cmdSQL.Connection = m_conSQL
            cmdSQL.CommandText = Request.Command
            cmdSQL.CommandType = Request.CommandType
            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParm In Request.Parameters
                    prmSQL = New SqlParameter()
                    prmSQL.Direction = oParm.Direction
                    prmSQL.ParameterName = oParm.Name
                    prmSQL.IsNullable = oParm.IsNullable
                    prmSQL.DbType = oParm.DBType

                    If oParm.DBType = DbType.Guid Or oParm.SqlDbType = SqlDbType.UniqueIdentifier Then
                        prmSQL.Value = New Guid(oParm.Value)
                    Else
                        prmSQL.Value = oParm.Value
                    End If

                    If oParm.AdvancedDataType Then
                        prmSQL.SqlDbType = oParm.SqlDbType
                    Else
                        prmSQL.DbType = oParm.DBType
                    End If

                    If oParm.Size > 0 Then
                        prmSQL.Size = oParm.Size
                    End If
                    cmdSQL.Parameters.Add(prmSQL)
                Next
            End If
            oResponse = New COL_SQLExecuteNotQuery()
            oResponse.ReturnedExecuteNotQuery = cmdSQL.ExecuteNonQuery()

            If Request.Parameters.Count > 0 Then
                Dim Query As IEnumerable(Of SqlParameter) = (From p As SqlParameter In cmdSQL.Parameters Where p.Direction = ParameterDirection.Output AndAlso Not IsDBNull(p.Value))
                For Each oParameter As COL_Request.Parameter In (From p In Request.Parameters Where (From sp As SqlParameter In Query Select sp.ParameterName).Contains(p.Name)).ToList
                    oParameter.Value = (From p In Query Where p.ParameterName = oParameter.Name Select p.Value).FirstOrDefault
                Next
                'Dim i As Integer
                'For i = 0 To cmdSQL.Parameters.Count - 1
                '    prmSQL = New SqlParameter()
                '    prmSQL = cmdSQL.Parameters.Item(i)
                '    If Not (prmSQL.Direction = ParameterDirection.Input) And Not (IsDBNull(prmSQL.Value)) Then
                '        Request.Parameters.Item(i + 1).Value() = prmSQL.Value
                '    End If
                'Next
            End If
            Return oResponse

        Catch exSQL As SqlException
            Debug.WriteLine(exSQL.Message)
            'Dim omail As New MailDBerrori
            'omail.Oggetto = "Errore"
            'omail.Body = exSQL.Message
            'omail.InviaMail()
            Request.Exception = exSQL

            Dim parameters As List(Of String)
            parameters = (From o In Request.Parameters Select o.Name & "= " & o.Value).ToList
            GenerateDBError.SendError(exSQL.Message, Request.Command, exSQL.StackTrace, parameters)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex

            Dim parameters As List(Of String)
            parameters = (From o In Request.Parameters Select o.Name & "= " & o.Value).ToList
            GenerateDBError.SendError(ex.Message, Request.Command, ex.StackTrace, parameters)
        Finally
            m_conSQL.Close()

        End Try
        Return Nothing
    End Function

    Public Overrides Function HasDBconnection(ByRef Request As COL_Request) As Boolean
        Dim sConnectStr As String = ""
        Try
            m_conSQL.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conSQL.Open()
            Return True
        Catch exSQL As SqlException
            Return False
        Catch ex As Exception
            Return False
        Finally
            m_conSQL.Close()
        End Try
    End Function


    'Private Sub SendDBmailError(ByVal Message As String, ByVal StackTrace As String, ByVal Command As String, ByVal Request As COL_Request)
    '    Dim oMail As New MailDBerrori
    '    oMail.Oggetto = "SqlException"
    '    oMail.Body = "SQL=" & Command & vbCrLf

    '    Dim ParametersString As String = ""
    '    For Each oParameter As COL_Request.Parameter In Request.Parameters
    '        ParametersString &= oParameter.Name & " = " & oParameter.Value.ToString & vbCrLf
    '    Next
    '    oMail.Body &= ParametersString
    '    oMail.Body &= "Message=" & Message & vbCrLf & "StackTrace=" & StackTrace & vbCrLf

    '    oMail.InviaMail()
    'End Sub
End Class