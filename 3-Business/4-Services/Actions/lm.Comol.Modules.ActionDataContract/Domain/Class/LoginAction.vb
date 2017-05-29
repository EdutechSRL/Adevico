Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class LoginAction
        'Implements iLoginAction

#Region "Private"
        Private _ActionNumber As Integer
        Private _isWorkingSessionClosed As Boolean
        Private _LastActionDate As DateTime
        Private _LoginDate As DateTime
        Private _PersonID As Integer
		Private _WorkingSessionID As System.Guid
        Private _ClientIPadress As String
        Private _ProxyIPadress As String
        Private _PersonRoleID As Integer
#End Region

#Region "Public"
        <DataMember()> Public Property ActionNumber() As Integer
            'Implements iLoginAction.ActionNumber
            Get
                Return _ActionNumber
            End Get
            Set(ByVal value As Integer)
                _ActionNumber = value
            End Set
        End Property
        <DataMember()> Public Property isWorkingSessionClosed() As Boolean
            'Implements iLoginAction.isWorkingSessionClosed
            Get
                Return _isWorkingSessionClosed
            End Get
            Set(ByVal value As Boolean)
                _isWorkingSessionClosed = value
            End Set
        End Property
        <DataMember()> Public Property LastActionDate() As DateTime
            'Implements iLoginAction.LastActionDate
            Get
                Return _LastActionDate
            End Get
			Set(ByVal value As DateTime)
				_LastActionDate = value
			End Set
        End Property
        <DataMember()> Public Property LoginDate() As DateTime
            'Implements iLoginAction.LoginDate
            Get
                Return _LoginDate
            End Get
			Set(ByVal value As DateTime)
				_LoginDate = value
			End Set
        End Property
        <DataMember()> Public Property PersonID() As Integer
            'Implements iLoginAction.PersonID
            Get
                Return _PersonID
            End Get
            Set(ByVal value As Integer)
                _PersonID = value
            End Set
        End Property
        <DataMember()> Public Property WorkingSessionID() As System.Guid
            'Implements iLoginAction.WorkingSessionID
            Get
                Return _WorkingSessionID
            End Get
            Set(ByVal value As System.Guid)
                _WorkingSessionID = value
            End Set
		End Property
        <DataMember()> Public Property ClientIPadress() As String
            Get
                Return _ClientIPadress
            End Get
            Set(ByVal value As String)
                _ClientIPadress = value
            End Set
        End Property
        <DataMember()> Public Property ProxyIPadress() As String
            Get
                Return _ProxyIPadress
            End Get
            Set(ByVal value As String)
                _ProxyIPadress = value
            End Set
        End Property
        <DataMember()> Public Property PersonRoleID() As Integer
            'Implements iLoginAction.PersonID
            Get
                Return _PersonRoleID
            End Get
            Set(ByVal value As Integer)
                _PersonRoleID = value
            End Set
        End Property

#End Region

        Sub New()
            Me._isWorkingSessionClosed = False
        End Sub
    End Class
End Namespace