Namespace lm.WS.UserAction.Configuration
	Public Class KeyComposite(Of T, Q)
		Private _Key As T
		Private _Value As Q

		Public Property Key() As T
			Get
				Return _Key
			End Get
			Set(ByVal value As T)
				_Key = value
			End Set
		End Property
		Public Property Value() As Q
			Get
				Return _Value
			End Get
			Set(ByVal value As Q)
				_Value = value
			End Set
		End Property

		Sub New()

		End Sub
		Sub New(ByVal oKey As T, ByVal oValue As Q)
			Me._Key = oKey
			Me._Value = oValue
		End Sub
	End Class
End Namespace