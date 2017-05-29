Public MustInherit Class COL_DataReader
    Public MustOverride Property ReturnedDataReader() As IDataReader
End Class

Public MustInherit Class COL_DataSet
    Public MustOverride Property ReturnedDataSet() As DataSet
End Class

Public MustInherit Class COL_ExecuteNotQuery
    Public MustOverride Property ReturnedExecuteNotQuery() As Integer
End Class