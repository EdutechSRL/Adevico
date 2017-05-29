Public MustInherit Class COL_AbstractFactory
    Public MustOverride Function HasDBconnection(ByRef Request As COL_Request) As Boolean
    Public MustOverride Function ExecuteDataReader(ByRef Request As COL_Request) As COL_DataReader
    Public MustOverride Function ExecuteDataSet(ByRef Request As COL_Request) As COL_DataSet
    Public MustOverride Function ExecuteNotQuery(ByRef Request As COL_Request) As COL_ExecuteNotQuery
End Class