Imports System.Configuration

Public Class COL_DataAccess
    Public Sub VerificaAccesso()
        Dim oFactory As COL_AbstractFactory
        oFactory = IstanziaFactory()
    End Sub
    Public Function GetdataSet(ByRef Request As COL_Request) As DataSet
        Dim oFactory As COL_AbstractFactory
        Dim oDataSet As COL_DataSet
        oFactory = IstanziaFactory()
        oDataSet = oFactory.ExecuteDataSet(Request)
        GetdataSet = oDataSet.ReturnedDataSet
    End Function

    Public Function GetdataReader(ByRef Request As COL_Request) As IDataReader
        Dim oFactory As COL_AbstractFactory
        Dim oDataReader As COL_DataReader
        oFactory = IstanziaFactory()
        oDataReader = oFactory.ExecuteDataReader(Request)
        GetdataReader = oDataReader.ReturnedDataReader
    End Function

    Public Function GetExecuteNotQuery(ByRef Request As COL_Request) As Integer
        Dim oFactory As COL_AbstractFactory
        Dim oResponse As COL_ExecuteNotQuery

        oFactory = IstanziaFactory()
        oResponse = oFactory.ExecuteNotQuery(Request)
        GetExecuteNotQuery = oResponse.ReturnedExecuteNotQuery
        oFactory = Nothing
    End Function
    Public Function HasDBConnection(ByRef Request As COL_Request) As Boolean
        Dim oFactory As COL_AbstractFactory
        Dim iReturn As Boolean = False

        oFactory = IstanziaFactory()
        iReturn = oFactory.HasDBconnection(Request)
        oFactory = Nothing
        Return iReturn
    End Function
    Private Function IstanziaFactory() As COL_AbstractFactory
        Dim oFactory As COL_AbstractFactory
        Dim Provider As String = LCase(System.Configuration.ConfigurationManager.AppSettings("Provider"))
        Select Case Provider
            Case "sqlclient"
                oFactory = New COL_SqlFactory
            Case "oledb"
                oFactory = New COL_OleDbFactory
            Case "oracle"
                oFactory = New COL_OracleFactory
            Case Else
                oFactory = New COL_SqlFactory
        End Select
        Return oFactory
    End Function

    Public Function GetParameter(ByVal ParamName As String, ByVal Value As String, Optional ByVal ParameterDirection As ParameterDirection = ParameterDirection.Input, Optional ByVal oDBtype As DbType = DbType.String, Optional ByVal IsNullable As Boolean = True, Optional ByVal Size As Integer = 0) As COL_Request.Parameter
        Dim oParam As COL_Request.Parameter
        oParam = New COL_Request.Parameter

        oParam.Name = ParamName
        oParam.Value = Value
        oParam.Direction = ParameterDirection
        'oParam.DBType = DBType
        oParam.IsNullable = IsNullable
        oParam.DBType = oDBtype
        oParam.AdvancedDataType = False
        If Size > 0 Then
            oParam.Size = Size
        End If
        Return oParam
    End Function
    Public Function GetAdvancedParameter(ByVal ParamName As String, ByVal Value As String, ByVal ParameterDirection As ParameterDirection, ByVal oSqlDbType As SqlDbType, Optional ByVal IsNullable As Boolean = True, Optional ByVal Size As Integer = 0) As COL_Request.Parameter
        Dim oParam As COL_Request.Parameter
        oParam = New COL_Request.Parameter

        oParam.Name = ParamName
        oParam.Value = Value
        oParam.Direction = ParameterDirection
        'oParam.DBType = DBType
        oParam.IsNullable = IsNullable
        oParam.SqlDbType = oSqlDbType
        oParam.AdvancedDataType = True
        If Size >= -1 Then
            oParam.Size = Size
        End If
        Return oParam
    End Function

End Class
