Imports lm.Comol.Core.DomainModel.Helpers


Namespace lm.Comol.Core.Cache
	Public Class CacheFactory
		Public Shared ReadOnly Property Cache(ByVal DefaultSliding As TimeSpan) As iCache
			Get
				Return CacheSetup(DefaultSliding)
			End Get
		End Property
		'Public Shared Cache(byval DefaulsT as TimeSpan) As iCache = CacheSetup()

		''' <summary>
		''' 
		''' </summary>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function CacheSetup(ByVal DefaultSliding As TimeSpan) As iCache
			Dim oCache As iCache = Nothing

			Try
				oCache = FactoryBuilder.BuildFactory(Of iCache)(Configuration.ConfigurationManager.AppSettings("CacheClassName"))
			Catch ex As Exception

			End Try
			If IsNothing(oCache) Then
				oCache = New FakeCache
			End If
			oCache.DefaultExpiration = DefaultSliding
			Return oCache
		End Function
	End Class
End Namespace