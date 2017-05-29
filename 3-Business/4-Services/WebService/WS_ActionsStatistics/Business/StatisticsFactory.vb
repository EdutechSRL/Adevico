Imports lm.WS.ActionStatistics.Domain

Namespace lm.WS.ActionStatistics.Business
	Public Class StatisticFactory

		Public Shared ReadOnly Property Service(ByVal oType As StatisticType) As IstatisticService
			Get
				Return StatisticSetup(oType)
			End Get
		End Property

		Private Shared Function StatisticSetup(ByVal oType As StatisticType) As IstatisticService
			Dim oService As IstatisticService = Nothing

			Try
				Select Case oType
					Case StatisticType.Community
						oService = New CommunityStat
					Case StatisticType.Modules
						oService = New ModuleStat
					Case StatisticType.Portal
						oService = New LoginStat
					Case Else
						oService = New FakeStatistics
				End Select

			Catch ex As Exception

			End Try
			If IsNothing(oService) Then
				oService = New FakeStatistics
			End If
			Return oService
		End Function

		Public Enum StatisticType
			None
			Portal
			Community
			Modules
		End Enum
	End Class
End Namespace