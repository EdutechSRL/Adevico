Imports System.Data
Imports System.Data.OleDb

Public Class COL_OleDbFactory : Inherits COL_AbstractFactory

    Private m_conOLEDB As New OleDbConnection()

    Protected Overrides Sub Finalize()
        If Not (m_conOLEDB.State = ConnectionState.Closed) Or _
           Not (m_conOLEDB.State = ConnectionState.Broken) Then
            m_conOLEDB.Close()
        End If
        MyBase.Finalize()
    End Sub

    Public Overrides Function ExecuteDataReader(ByRef Request As COL_Request) As COL_DataReader
        ' Returns a COL_DataReader object, which wraps an object of type IDataReader
        ' Uses OleDb .NET Server data provider, hence the wrapped object is a OleDbDataReader object 
        ' An OleDbDataReader object is a read-only, forward-only data stream. 
        ' NOTE: DataReaders won't be used in queries that perform transactions.

        Dim cmdOLEDB As New OleDbCommand()
        Dim prmOLEDB As OleDbParameter
        Dim oParam As COL_Request.Parameter
        Dim drOLEDB As OleDbDataReader
        Dim oDataReaderOLEDB As New COL_OleDbDataReader()

        Try
            m_conOLEDB.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)

            ' open connection, and begin to set properties of command
            m_conOLEDB.Open()
            cmdOLEDB.Connection = m_conOLEDB
            cmdOLEDB.CommandText = Request.Command
            cmdOLEDB.CommandType = Request.CommandType

            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmOLEDB = cmdOLEDB.Parameters.AddWithValue(oParam.Name, oParam.Value)
                Next
            End If

            drOLEDB = cmdOLEDB.ExecuteReader()

            oDataReaderOLEDB.ReturnedDataReader = drOLEDB
            Return oDataReaderOLEDB

        Catch exOLEDB As OleDbException
            Debug.WriteLine(exOLEDB.Message)
            Request.Exception = exOLEDB

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex

        Finally

        End Try
        Return Nothing
    End Function

    Public Overrides Function ExecuteDataSet(ByRef Request As COL_Request) As COL_DataSet
        ' Returns a COL_DataSet object, which wraps an object of type DataSet
        ' Uses OLE DB .NET data provider if a data provider is necessary 
        ' - hence uses a OleDbDataAdapter to fill the DataSet
        Dim conOLEDB As New OleDbConnection()
        Dim cmdOLEDB As New OleDbCommand()
        Dim prmOLEDB As OleDbParameter
        Dim oParam As COL_Request.Parameter
        Dim daOLEDB As OleDbDataAdapter
        Dim oDataSetOLEDB As New COL_OleDbDataSet()
        Dim tranOLEDB As OleDbTransaction

        Try
            conOLEDB.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)

            ' open connection, and begin to set properties of command            
            conOLEDB.Open()
            cmdOLEDB.Connection = conOLEDB
            cmdOLEDB.CommandText = Request.Command
            cmdOLEDB.CommandType = Request.CommandType

            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                'ICommand.Parameters is readonly
                For Each oParam In Request.Parameters
                    prmOLEDB = cmdOLEDB.Parameters.AddWithValue(oParam.Name, oParam.Value)
                Next
            End If

            If Request.Transactional Then
                tranOLEDB = conOLEDB.BeginTransaction()
            End If

            daOLEDB = New OleDbDataAdapter(cmdOLEDB)

            ' allow generic naming - NewDataSet
            daOLEDB.Fill(oDataSetOLEDB.ReturnedDataSet)

            Return oDataSetOLEDB

        Catch exOLEDB As OleDbException
            Debug.WriteLine(exOLEDB.Message)
            Request.Exception = exOLEDB
            If Request.Transactional Then
                tranOLEDB.Rollback()
            End If

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex
            If Request.Transactional Then
                tranOLEDB.Rollback()
            End If

        Finally
            If Request.Transactional Then
                tranOLEDB.Commit()
            End If
            If conOLEDB.State = ConnectionState.Open Then
                conOLEDB.Close()
            End If
        End Try
        Return Nothing
    End Function

    Public Overrides Function ExecuteNotQuery(ByRef Request As COL_Request) As COL_ExecuteNotQuery
        ' Returns a COL_DataReader object, which wraps an object of type IDataReader
        ' Uses OleDb Server .NET data provider, hence the wrapped object is a SqlDataReader object 
        ' A SqlDataReader object is a read-only, forward-only data stream. 
        ' NOTE: DataReaders won't be used in queries that perform transactions.
        Dim cmdOleDb As New OleDbCommand()
        Dim prmOleDb As OleDbParameter
        Dim oParam As COL_Request.Parameter
        Dim oResponse As COL_OleDbExecuteNotQuery

        Try
            m_conOLEDB.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conOLEDB.Open()
            cmdOleDb.Connection = m_conOLEDB
            cmdOleDb.CommandText = Request.Command
            cmdOleDb.CommandType = Request.CommandType
            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmOleDb = New OleDbParameter()
                    prmOleDb.Direction = oParam.Direction
                    prmOleDb.ParameterName = oParam.Name
                    prmOleDb.Value = oParam.Value
                    prmOleDb.IsNullable = oParam.IsNullable
                    prmOleDb.DbType = oParam.DBType

                    If oParam.AdvancedDataType Then
                        If oParam.DBType <> DbType.DateTime Then
                            prmOleDb.OleDbType = Me.GetOleDBType(oParam.SqlDbType)
                        End If
                    End If

                    If oParam.Size > 0 Then
                        prmOleDb.Size = oParam.Size
                    End If
                    cmdOleDb.Parameters.Add(prmOleDb)
                Next
            End If
            oResponse = New COL_OleDbExecuteNotQuery()
            oResponse.ReturnedExecuteNotQuery = cmdOleDb.ExecuteNonQuery()

            If Request.Parameters.Count > 0 Then
                Dim i As Integer
                For i = 0 To cmdOleDb.Parameters.Count - 1
                    prmOleDb = New OleDbParameter()
                    prmOleDb = cmdOleDb.Parameters.Item(i)
                    If Not (prmOleDb.Direction = ParameterDirection.Input) And Not (IsDBNull(prmOleDb.Value)) Then
                        Request.Parameters.Item(i + 1).Value() = prmOleDb.Value
                    End If
                Next
            End If

            Return oResponse

        Catch exOleDb As OleDbException
            Debug.WriteLine(exOleDb.Message)
            Request.Exception = exOleDb
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex
        Finally
            If m_conOLEDB.State = ConnectionState.Open Then
                m_conOLEDB.Close()
            End If
        End Try
        Return Nothing
    End Function

    Public Overrides Function HasDBconnection(ByRef Request As COL_Request) As Boolean
        Try
            m_conOLEDB.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conOLEDB.Open()

            Return True
        Catch exOleDb As OleDbException
            Return False
        Catch ex As Exception
            Return False
        Finally
        End Try
    End Function

    Private Function GetOleDBType(ByVal oSqlType As SqlDbType) As OleDbType
        Select Case oSqlType
            Case SqlDbType.BigInt
                Return OleDbType.BigInt
            Case SqlDbType.Binary
                Return OleDbType.Binary
                'Case SqlDbType.Bit
            Case SqlDbType.Char
                Return OleDbType.Char
                '    Case SqlDbType.DateTime
                '
                ' Case SqlDbType.Float
                '    Return OleDbType.Floa
            Case SqlDbType.Float
                Return OleDbType.Double
                'Case SqlDbType.Image
            Case SqlDbType.Int
                Return OleDbType.Integer

                '   Case SqlDbType.SqlDbType.Money

                '  Case SqlDbType.Money
                ' Case SqlDbType.NChar
                ' Return OleDbType.NCh
                ' Case SqlDbType.NText
                '  Case SqlDbType.NVarChar

                'Case SqlDbType.Real
                '   Case SqlDbType.SmallDateTime
            Case SqlDbType.SmallInt
                Return OleDbType.SmallInt
                '    Case SqlDbType.SmallMoney
            Case SqlDbType.Text
                Return OleDbType.LongVarChar
            Case SqlDbType.VarChar
                Return OleDbType.VarChar

            Case SqlDbType.Decimal
                Return OleDbType.Decimal
            Case SqlDbType.TinyInt
                Return OleDbType.TinyInt
            Case SqlDbType.Real
                Return OleDbType.Single
        End Select
    End Function
End Class