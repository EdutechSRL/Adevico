Namespace lm.Comol.Modules.UserActions.DomainModel
	<Serializable(), CLSCompliant(True)> Public Class dtoSummary
		Private _UsageTime As Integer
		Private _Data As DateTime
		Private _ToTimeSpan As TimeSpan

		Public ID As Integer
		Public Owner As String
		Public ModuleName As String
		Public CommunityName As String
		Public nAccesses As Long
		Public NavigateTo As String
		Public NavigateToDetails As String

		Public Property UsageTime() As Integer
			Get
				Return _UsageTime
			End Get
			Set(ByVal value As Integer)
				_Data = Nothing
				_Data = _Data.AddSeconds(value)
				_UsageTime = value
                _ToTimeSpan = New TimeSpan(0, 0, UsageTime)
			End Set
		End Property
		Public ReadOnly Property ToTimeSpan() As TimeSpan
			Get
				Return _ToTimeSpan
			End Get
		End Property
		Public ReadOnly Property DataUsage() As DateTime
			Get
				Return _Data
			End Get
		End Property

		Sub New()
			UsageTime = 0
			nAccesses = 0
			Owner = ""
			ModuleName = ""
			CommunityName = ""
			ID = 0
			NavigateTo = ""
			NavigateToDetails = ""
		End Sub
	End Class
End Namespace