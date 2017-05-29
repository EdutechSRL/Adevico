Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class ModuleUsageTime
        'Implements iModuleUsageTime

#Region "Private"
        Private _CommunityID As Integer
        Private _ActionDate As Date
        Private _UsageTime As Integer
        Private _ModuleID As Integer
		Private _PersonID As Integer
		Private _ActionNumber As Integer
#End Region

#Region "Public"
        <DataMember()> Property CommunityID() As Integer
            'Implements iModuleUsageTime.CommunityID
            Get
                Return _CommunityID
            End Get
            Set(ByVal value As Integer)
                _CommunityID = value
            End Set
        End Property
        <DataMember()> Public Property ActionDate() As Date
            'Implements iModuleUsageTime.Day
            Get
                Return _ActionDate
            End Get
            Set(ByVal value As Date)
                _ActionDate = value
            End Set
        End Property
        <DataMember()> Public Property ModuleID() As Integer
            'Implements iModuleUsageTime.ModuleID
            Get
                Return _ModuleID
            End Get
            Set(ByVal value As Integer)
                _ModuleID = value
            End Set
        End Property
        <DataMember()> Public Property PersonID() As Integer
            'Implements iModuleUsageTime.PersonID
            Get
                Return _PersonID
            End Get
            Set(ByVal value As Integer)
                _PersonID = value
            End Set
        End Property
        <DataMember()> Public Property UsageTime() As Integer
            'Implements iModuleUsageTime.UsageTime
            Get
                Return _UsageTime
            End Get
            Set(ByVal value As Integer)
                _UsageTime = value
            End Set
        End Property
        <DataMember()> Public Property UsageTimeToTimeSpan() As System.TimeSpan
            'Implements iModuleUsageTime.UsageTimeToTimeSpan
            Get
                Return TimeSpan.FromMinutes(_UsageTime)
            End Get
            Set(ByVal value As System.TimeSpan)
                _UsageTime = value.TotalMinutes
            End Set

		End Property
		<DataMember()> Public Property ActionNumber() As Integer
			Get
				Return _ActionNumber
			End Get
			Set(ByVal value As Integer)
				_ActionNumber = value
			End Set

		End Property
#End Region

        Sub New()

        End Sub
    End Class
End Namespace