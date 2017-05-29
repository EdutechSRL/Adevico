Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class UserAction
        'Implements iAction

#Region "Private"
        Private _ActionDate As DateTime
        Private _CommunityID As Integer
        Private _InteractionType As InteractionType
        Private _ModuleID As Integer
        Private _Type As Integer
        Private _PersonID As Integer
		Private _WorkingSessionID As System.Guid
		Private _ID As System.Guid
		Private _ObjectActions As List(Of ObjectAction)
        Private _ClientIPadress As String
        Private _ProxyIPadress As String
        Private _PersonRoleID As Integer
#End Region

#Region "Public"
		<DataMember()> Public Property ID() As System.Guid
			Get
				Return _ID
			End Get
			Set(ByVal value As System.Guid)
				_ID = value
			End Set
		End Property
		<DataMember()> Public Property ActionDate() As DateTime
            Get
                Return _ActionDate
            End Get
			Set(ByVal value As DateTime)
				_ActionDate = value
			End Set
		End Property
		'<DataMember()> Public Property BaseIPaddress() As System.Collections.Generic.IList(Of iBaseIPaddress) Implements iAction.BaseIPaddress
		'	Get
		'		Return _BaseIPaddress
		'	End Get
		'	Set(ByVal value As System.Collections.Generic.IList(Of iBaseIPaddress))
		'		_BaseIPaddress = value
		'	End Set
		'End Property
		<DataMember()> Public Property CommunityID() As Integer
            Get
                Return _CommunityID
            End Get
			Set(ByVal value As Integer)
				_CommunityID = value
			End Set
		End Property
		<DataMember()> Public Property Interaction() As InteractionType
            Get
                Return _InteractionType
            End Get
			Set(ByVal value As InteractionType)
				_InteractionType = value
			End Set
		End Property
		<DataMember()> Public Property ModuleID() As Integer
            Get
                Return _ModuleID
            End Get
			Set(ByVal value As Integer)
				_ModuleID = value
			End Set
		End Property
		<DataMember()> Public Property ObjectActions() As System.Collections.Generic.List(Of ObjectAction)
			Get
				Return _ObjectActions
			End Get
			Set(ByVal value As System.Collections.Generic.List(Of ObjectAction))
				_ObjectActions = value
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
		<DataMember()> Public Property Type() As Integer
            Get
                Return _Type
            End Get
			Set(ByVal value As Integer)
				_Type = value
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
            Get
                Return _PersonRoleID
            End Get
            Set(ByVal value As Integer)
                _PersonRoleID = value
            End Set
        End Property
#End Region

		Sub New()
			_ID = System.Guid.NewGuid
			_ObjectActions = New List(Of ObjectAction)
		End Sub
    End Class
End Namespace