Imports System.Collections.Generic

Public Class COL_Request
    Public Enum UserRole
        External = 1
        Internal = 2
        SuperUser = 3
        Admin = 4
        trusted = 5
    End Enum

    Public Class Parameter

#Region "Private Property"
        Private n_Name As String
        Private n_Value As String
        Private n_Direction As ParameterDirection
        Private n_DBType As DbType
        Private n_SqlDbType As SqlDbType
        Private n_IsNullable As Boolean
        Private n_Size As Integer
        Private n_AdvancedDataType As Boolean
#End Region

#Region "Public Property"
        Public Property Name() As String
            Get
                Name = n_Name
            End Get
            Set(ByVal Value As String)
                n_Name = Value
            End Set
        End Property
        Public Property Value() As String
            Get
                Value = n_Value
            End Get
            Set(ByVal Value As String)
                n_Value = Value
            End Set
        End Property
        Public Property Direction() As ParameterDirection
            Get
                Direction = n_Direction
            End Get
            Set(ByVal Value As ParameterDirection)
                n_Direction = Value
            End Set
        End Property
        Public Property DBType() As DbType
            Get
                DBType = n_DBType
            End Get
            Set(ByVal Value As DbType)
                n_DBType = Value
            End Set
        End Property
        Public Property IsNullable() As Boolean
            Get
                IsNullable = n_IsNullable
            End Get
            Set(ByVal Value As Boolean)
                n_IsNullable = Value
            End Set
        End Property
        Public Property Size() As Integer
            Get
                Size = n_Size
            End Get
            Set(ByVal Value As Integer)
                n_Size = Value
            End Set
        End Property
        Public Property SqlDbType() As SqlDbType
            Get
                SqlDbType = n_SqlDbType
            End Get
            Set(ByVal Value As SqlDbType)
                n_SqlDbType = Value
            End Set
        End Property
        Public Property AdvancedDataType() As Boolean
            Get
                AdvancedDataType = n_AdvancedDataType
            End Get
            Set(ByVal Value As Boolean)
                n_AdvancedDataType = Value
            End Set
        End Property
        Sub New()
            Me.n_Direction = ParameterDirection.Input
            Me.n_DBType = DBType.String
            Me.n_SqlDbType = SqlDbType.VarChar
            Me.n_IsNullable = True
            Me.n_AdvancedDataType = False
            ' Da aggiungere altri parametri da iniziallizzare di default
            ' Nullable e DBType
        End Sub
#End Region

    End Class

    Private m_lUserRole As UserRole
    Private m_lCommandType As CommandType
    Private m_sCommand As String
    Private m_btransactional As Boolean
    Private _Parameters As New List(Of COL_Request.Parameter)
    Private m_oException As Exception

#Region "Proprietà"
    Public Property Role() As UserRole
        Get
            Role = m_lUserRole
        End Get
        Set(ByVal Value As UserRole)
            m_lUserRole = Value
        End Set
    End Property
    Public Property CommandType() As CommandType
        Get
            CommandType = m_lCommandType
        End Get
        Set(ByVal Value As CommandType)
            m_lCommandType = Value
        End Set
    End Property
    Public Property Command() As String
        Get
            Command = m_sCommand
        End Get
        Set(ByVal Value As String)
            m_sCommand = Value
        End Set
    End Property
    Public Property Parameters() As List(Of COL_Request.Parameter)
        Get
            Parameters = _Parameters
        End Get
        Set(ByVal Value As List(Of COL_Request.Parameter))
            _Parameters = Value
        End Set
    End Property
    Public Property transactional() As Boolean
        Get
            transactional = m_btransactional
        End Get
        Set(ByVal Value As Boolean)
            m_btransactional = Value
        End Set
    End Property
    Public Property Exception() As Exception
        Get
            Exception = m_oException
        End Get
        Set(ByVal Value As Exception)
            m_oException = Value
        End Set
    End Property
#End Region

    ' Dato un indice intero, restituisce il valore del relativo parametro
    Public Function GetValueFromParameter(ByVal index As Integer) As String
        Dim oParam As New COL_Request.Parameter()
        Dim oResponse As String
        If Me.Parameters.Count > 0 And Me.Parameters.Count >= index Then
            Try
                oParam = Me.Parameters.Item(index - 1)
                oResponse = oParam.Value()
            Catch ex As Exception
                oResponse = Nothing
            End Try
        Else
            oResponse = Nothing
        End If

        Return oResponse
    End Function

End Class
