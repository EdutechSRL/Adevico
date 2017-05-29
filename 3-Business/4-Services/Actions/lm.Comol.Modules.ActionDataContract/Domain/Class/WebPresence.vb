Namespace lm.ActionDataContract
	Public Class WebPresence

#Region "Private"
		Private _ModuleID As Integer
		Private _PersonID As Integer
		Private _PersonRoleID As Integer
		Private _CommunityID As Integer
		Private _LastDate As DateTime
		Private _WorkingSessionID As System.Guid
		Private _ClientIPadress As Long
		Private _ProxyIPadress As Long
#End Region

#Region "Public"
		Public Property LastDate() As DateTime
			'Implements iAction.ActionDate
			Get
				Return _LastDate
			End Get
			Set(ByVal value As DateTime)
				_LastDate = value
			End Set
		End Property
		Public Property CommunityID() As Integer
			Get
				Return _CommunityID
			End Get
			Set(ByVal value As Integer)
				_CommunityID = value
			End Set
		End Property
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal value As Integer)
				_ModuleID = value
			End Set
		End Property
		Public Property PersonID() As Integer
			Get
				Return _PersonID
			End Get
			Set(ByVal value As Integer)
				_PersonID = value
			End Set
		End Property
		Public Property PersonRoleID() As Integer
			Get
				Return _PersonRoleID
			End Get
			Set(ByVal value As Integer)
				_PersonRoleID = value
			End Set
		End Property
		Public Property WorkingSessionID() As System.Guid
			Get
				Return _WorkingSessionID
			End Get
			Set(ByVal value As System.Guid)
				_WorkingSessionID = value
			End Set
		End Property
		Public Property ClientIPadress() As Long
			Get
				Return _ClientIPadress
			End Get
			Set(ByVal value As Long)
				_ClientIPadress = value
			End Set
		End Property
		Public Property ProxyIPadress() As Long
			Get
				Return _ProxyIPadress
			End Get
			Set(ByVal value As Long)
				_ProxyIPadress = value
			End Set
		End Property
#End Region

		Sub New()

		End Sub

	End Class
End Namespace