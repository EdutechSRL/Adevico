Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class CommunityAction

#Region "Private"
        Private _isExitCommunity As Boolean
        Private _LastActionDate As DateTime
        Private _AccessDate As DateTime
        Private _PersonID As Integer
        Private _CommunityID As Integer
        Private _WorkingSessionID As System.Guid
        Private _PersonRoleID As Integer
#End Region

#Region "Public"
        <DataMember()> Public Property isExitCommunity() As Boolean
            Get
                Return _isExitCommunity
            End Get
            Set(ByVal value As Boolean)
                _isExitCommunity = value
            End Set
        End Property
        <DataMember()> Public Property LastActionDate() As DateTime
            Get
                Return _LastActionDate
            End Get
			Set(ByVal value As DateTime)
				_LastActionDate = value
			End Set
        End Property
        <DataMember()> Public Property AccessDate() As DateTime
            Get
                Return _AccessDate
            End Get
			Set(ByVal value As DateTime)
				_AccessDate = value
			End Set
        End Property
        <DataMember()> Public Property PersonID() As Integer
            Get
                Return _PersonID
            End Get
            Set(ByVal value As Integer)
                _PersonID = value
            End Set
        End Property
        <DataMember()> Public Property CommunityID() As Integer
            Get
                Return _CommunityID
            End Get
            Set(ByVal value As Integer)
                _CommunityID = value
            End Set
        End Property
        <DataMember()> Public Property WorkingSessionID() As System.Guid
            Get
                Return _WorkingSessionID
            End Get
            Set(ByVal value As System.Guid)
                _WorkingSessionID = value
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
            Me._isExitCommunity = False
        End Sub
    End Class
End Namespace
