Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class BaseIPaddress
        'Implements iBaseIPaddress
#Region "Private"
        Private _Name As String
        Private _IP As String
        Private _isProxy As Boolean
#End Region

#Region "Public"
        <DataMember()> Property Name() As String
            'Implements iBaseIPaddress.Name
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        <DataMember()> Public Property IP() As String
            'Implements iBaseIPaddress.IP
            Get
                Return _IP
            End Get
            Set(ByVal value As String)
                _IP = value
            End Set
        End Property
        <DataMember()> Public Property isProxy() As Boolean
            'Implements iBaseIPaddress.isProxy
            Get
                Return _isProxy
            End Get
            Set(ByVal value As Boolean)
                _isProxy = value
            End Set
        End Property
#End Region

        Sub New()
            Me._isProxy = False
        End Sub
    End Class
End Namespace