Public Class COL_OracleDataReader : Inherits COL_DataReader
    Dim m_oReturnedDataReader As IDataReader

    Public Overrides Property ReturnedDataReader() As IDataReader
        Get
            ReturnedDataReader = m_oReturnedDataReader
        End Get
        Set(ByVal Value As IDataReader)
            m_oReturnedDataReader = Value
        End Set
    End Property
End Class



Public Class COL_OracleDataSet : Inherits COL_DataSet
    Dim m_oReturnedDataSet As DataSet

    Public Sub New()
        If m_oReturnedDataSet Is Nothing Then
            m_oReturnedDataSet = New DataSet()
        End If
    End Sub

    Public Overrides Property ReturnedDataSet() As DataSet
        Get
            ReturnedDataSet = m_oReturnedDataSet
        End Get
        Set(ByVal Value As DataSet)
            m_oReturnedDataSet = Value
        End Set
    End Property
End Class

Public Class COL_OracleExecuteNotQuery : Inherits COL_ExecuteNotQuery
    Dim m_ReturnedExecuteNotQuery As Integer
    Public Sub New()
        m_ReturnedExecuteNotQuery = -1
    End Sub

    Public Overrides Property ReturnedExecuteNotQuery() As Integer
        Get
            ReturnedExecuteNotQuery = m_ReturnedExecuteNotQuery
        End Get
        Set(ByVal Value As Integer)
            m_ReturnedExecuteNotQuery = Value
        End Set
    End Property
End Class
