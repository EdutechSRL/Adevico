Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class ObjectAction

#Region "Private"
		Private _ModuleID As Integer
        Private _ObjectTypeId As Integer
		Private _ValueID As String
#End Region

#Region "Public"
		<DataMember()> Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal value As Integer)
				_ModuleID = value
			End Set
		End Property
        <DataMember()> Public Property ObjectTypeId() As Integer
            Get
                Return _ObjectTypeId
            End Get
            Set(ByVal value As Integer)
                _ObjectTypeId = value
            End Set
        End Property
		<DataMember()> Public Property ValueID() As String
			Get
				Return _ValueID
			End Get
			Set(ByVal value As String)
				_ValueID = value
			End Set
		End Property
#End Region

        Sub New()

        End Sub

    End Class
End Namespace