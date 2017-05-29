Namespace lm.Comol.UI.Presentation.Utility
	Public Class EncryptionKey
#Region "Private"
		Private _Key As String
		Private _Type As EncType
#End Region

#Region "Const"
		Private Const _HashSecretKey As String = "maieutike_lab"
		Private Const _Questionario As String = "germanic"	'Gaio Giulio Cesare Claudiano Germanico
		Private Const _AttivazioneIscrizione As String = "publiova"	' Publio Quintilio Varo
		Private Const _AttivazioneAccount As String = "stilicon" 'Flavio Stilicone
		Private Const _CambioMail As String = "sulgalba" 'Servio Sulpicio Galba (pretore 54 a.C.)
		Private Const _AccessoForum As String = "agricola"	 ' Sesto Calpurnio Agricola
		Private Const _Common As String = "decimobr"	 ' Decimo Bruto
#End Region

#Region "Public"
		Public Property Key() As String
			Get
				Return _Key
			End Get
			Set(ByVal value As String)
				_Key = value
			End Set
		End Property
		Public Property Type() As EncType
			Get
				Return _Type
			End Get
			Set(ByVal value As EncType)
				_Type = value
			End Set
		End Property
#End Region

		Sub New()
			_Type = EncType.Common
		End Sub
		Sub New(ByVal iKey As String, ByVal iType As EncType)
			_Type = iType
			_Key = iKey
		End Sub

		Public Shared Function GetDefaults() As List(Of EncryptionKey)
			Dim oList As New List(Of EncryptionKey)
			oList.Add(New EncryptionKey(_AttivazioneAccount, EncType.Account))
			oList.Add(New EncryptionKey(_AccessoForum, EncType.Forum))
			oList.Add(New EncryptionKey(_AttivazioneIscrizione, EncType.Subscription))
			oList.Add(New EncryptionKey(_CambioMail, EncType.Mail))
			oList.Add(New EncryptionKey(_Questionario, EncType.Question))
			oList.Add(New EncryptionKey(_Common, EncType.Common))
			Return oList
		End Function
		Public Shared Function GetSecretHash() As String
			Return _HashSecretKey
		End Function

	End Class
End Namespace