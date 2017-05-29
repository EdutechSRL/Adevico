Imports System.Data
Imports System.Data.OracleClient

Public Class COL_OracleFactory : Inherits COL_AbstractFactory
    Private m_conORACLE As New OracleConnection()

    Protected Overrides Sub Finalize()
        If Not (m_conORACLE.State = ConnectionState.Closed) Or _
           Not (m_conORACLE.State = ConnectionState.Broken) Then
            m_conORACLE.Close()
        End If
        MyBase.Finalize()
    End Sub

    Public Overrides Function ExecuteDataReader(ByRef Request As COL_Request) As COL_DataReader
        ' Returns a COL_DataReader object, which wraps an object of type IDataReader
        ' Uses ORACLE .NET data provider, hence the wrapped object is a OdbcDataReader object 
        ' A OdbcDataReader object is a read-only, forward-only data stream. 
        ' NOTE: DataReaders won't be used in queries that perform transactions.

        Dim cmdORACLE As New OracleCommand()
        Dim prmORACLE As OracleParameter
        Dim oParam As COL_Request.Parameter
        Dim drORACLE As OracleDataReader
        Dim oDataReaderORACLE As New COL_OracleDataReader()

        Try
            m_conORACLE.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)

            ' open connection, and begin to set properties of command
            m_conORACLE.Open()
            cmdORACLE.Connection = m_conORACLE
            cmdORACLE.CommandType = Request.CommandType

            ' Check for parameters, and set Command property accordingly
            Dim iCounter As Integer
            If Request.Parameters.Count > 0 Then
                'ORACLE data provider requires something of the form "{call CustOrdersOrders(?, ?, ?)}" for parameterised stored procedures
                'see http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q309486
                cmdORACLE.CommandText = "{call " & Request.Command & "("
                For iCounter = 1 To Request.Parameters.Count
                    cmdORACLE.CommandText &= "?"
                    If (iCounter < Request.Parameters.Count) Then cmdORACLE.CommandText &= ", "
                Next
                cmdORACLE.CommandText &= ")}"
            Else
                cmdORACLE.CommandText = Request.Command
            End If

            ' Add parameters to Parameters property if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmORACLE = cmdORACLE.Parameters.AddWithValue(oParam.Name, oParam.Value)
                Next
            End If

            drORACLE = cmdORACLE.ExecuteReader()

            oDataReaderORACLE.ReturnedDataReader = drORACLE
            Return oDataReaderORACLE

        Catch exORACLE As OracleException
            Debug.WriteLine(exORACLE.Message)
            Request.Exception = exORACLE

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex

        Finally

        End Try
        Return Nothing
    End Function

    Public Overrides Function ExecuteDataSet(ByRef Request As COL_Request) As COL_DataSet
        ' Returns a COL_DataSet object, which wraps an object of type DataSet
        ' Uses ORACLE .NET data provider if a data provider is necessary 
        ' - hence uses a OdbcDataAdapter to fill the DataSet

        Dim conORACLE As New OracleConnection()
        Dim cmdORACLE As New OracleCommand()
        Dim prmORACLE As OracleParameter
        Dim oParam As COL_Request.Parameter
        Dim daORACLE As OracleDataAdapter
        Dim oDataSetORACLE As New COL_OracleDataSet()
        Dim tranORACLE As OracleTransaction

        Try
            conORACLE.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)

            ' open connection, and begin to set properties of command            
            conORACLE.Open()
            cmdORACLE.Connection = conORACLE
            cmdORACLE.CommandType = Request.CommandType

            ' Check for parameters, and set Command property accordingly
            Dim iCounter As Integer
            If Request.Parameters.Count > 0 Then
                'ORACLE data provider requires something of the form "{call CustOrdersOrders(?, ?, ?)}" for parameterised stored procedures
                'see http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q309486
                cmdORACLE.CommandText = "{call " & Request.Command & "("
                For iCounter = 1 To Request.Parameters.Count
                    cmdORACLE.CommandText &= "?"
                    If (iCounter < Request.Parameters.Count) Then cmdORACLE.CommandText &= ", "
                Next
                cmdORACLE.CommandText &= ")}"
                Debug.WriteLine("cmdORACLE.CommandText = """ & cmdORACLE.CommandText & """")
            Else
                cmdORACLE.CommandText = Request.Command
            End If

            ' Add parameters to Parameters property if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmORACLE = cmdORACLE.Parameters.AddWithValue(oParam.Name, oParam.Value)
                Next
            End If

            If Request.Transactional Then
                tranORACLE = conORACLE.BeginTransaction()
            End If

            daORACLE = New OracleDataAdapter(cmdORACLE)

            daORACLE.Fill(oDataSetORACLE.ReturnedDataSet)
            Return oDataSetORACLE

        Catch exORACLE As OracleException
            Debug.WriteLine(exORACLE.Message)
            Request.Exception = exORACLE
            If Request.Transactional Then
                tranORACLE.Rollback()
            End If

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex
            If Request.Transactional Then
                tranORACLE.Rollback()
            End If

        Finally
            If Request.Transactional Then
                tranORACLE.Commit()
            End If
            If conORACLE.State = ConnectionState.Broken Then
                conORACLE.Close()
            End If
        End Try
        Return Nothing
    End Function

    Public Overrides Function ExecuteNotQuery(ByRef Request As COL_Request) As COL_ExecuteNotQuery
        ' Returns a COL_DataReader object, which wraps an object of type IDataReader
        ' Uses Oracle Server .NET data provider, hence the wrapped object is a OracleDataReader object 
        ' A OracleDataReader object is a read-only, forward-only data stream. 
        ' NOTE: DataReaders won't be used in queries that perform transactions.
        Dim cmdOracle As New OracleCommand()
        Dim prmOracle As OracleParameter
        Dim oParam As COL_Request.Parameter
        Dim oResponse As COL_OracleExecuteNotQuery
        Try
            m_conORACLE.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conORACLE.Open()
            cmdOracle.Connection = m_conORACLE
            cmdOracle.CommandText = Request.Command
            cmdOracle.CommandType = Request.CommandType
            ' add parameters if they exist
            If Request.Parameters.Count > 0 Then
                For Each oParam In Request.Parameters
                    prmOracle = New OracleParameter()
                    prmOracle.Direction = oParam.Direction
                    prmOracle.ParameterName = oParam.Name
                    prmOracle.Value = oParam.Value
                    prmOracle.IsNullable = oParam.IsNullable
                    prmOracle.DbType = oParam.DBType
                    If oParam.AdvancedDataType Then
                        If oParam.DBType <> DbType.Boolean Then
                            prmOracle.OracleType = Me.GetOracleType(oParam.SqlDbType)
                        End If
                    End If

                    If oParam.Size > 0 Then
                        prmOracle.Size = oParam.Size
                    End If
                    cmdOracle.Parameters.Add(prmOracle)
                Next
            End If
            oResponse = New COL_OracleExecuteNotQuery()
            oResponse.ReturnedExecuteNotQuery = cmdOracle.ExecuteNonQuery()

            If Request.Parameters.Count > 0 Then
                Dim i As Integer
                For i = 0 To cmdOracle.Parameters.Count - 1
                    prmOracle = New OracleParameter()
                    prmOracle = cmdOracle.Parameters.Item(i)
                    If Not (prmOracle.Direction = ParameterDirection.Input) And Not (IsDBNull(prmOracle.Value)) Then
                        Request.Parameters.Item(i + 1).Value() = prmOracle.Value
                    End If
                Next
            End If

            Return oResponse

        Catch exOracle As OracleException
            Debug.WriteLine(exOracle.Message)
            Request.Exception = exOracle
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Request.Exception = ex
        Finally
        End Try
        Return Nothing
    End Function

    Public Overrides Function HasDBconnection(ByRef Request As COL_Request) As Boolean
        Try
            m_conORACLE.ConnectionString = COL_ConnStrings.GetInstance.GetConnectStringByRole(Request.Role)
            m_conORACLE.Open()

            Return True
        Catch exOracle As OracleException
            Return False
        Catch ex As Exception
            Return False
        Finally
            m_conORACLE.Close()
        End Try
    End Function

    Private Function GetOracleType(ByVal oSqlType As SqlDbType) As OracleType
        Select Case oSqlType
            ' Case SqlDbType.BigInt
            'case SqlDbType.Binary
            'Case SqlDbType.Bit
            'return 
        Case SqlDbType.Char
                Return OracleType.Char
            Case SqlDbType.DateTime
                Return OracleType.DateTime
            Case SqlDbType.Float
                Return OracleType.Float
                'Case SqlDbType.Image
            Case SqlDbType.Int
                Return OracleType.Int32
                '  Case SqlDbType.Money
            Case SqlDbType.NChar
                Return OracleType.NChar
                ' Case SqlDbType.NText
            Case SqlDbType.NVarChar
                Return OracleType.NVarChar
                'Case SqlDbType.Real
                '   Case SqlDbType.SmallDateTime
            Case SqlDbType.SmallInt
                Return OracleType.Int16
                '    Case SqlDbType.SmallMoney
            Case SqlDbType.Text
                Return OracleType.LongVarChar
            Case SqlDbType.VarChar
                Return OracleType.VarChar

            Case SqlDbType.Decimal
                Return OracleType.Number

        End Select
    End Function
End Class