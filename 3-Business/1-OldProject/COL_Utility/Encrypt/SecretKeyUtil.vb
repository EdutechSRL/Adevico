Public Class SecretKeyUtil

	Enum EncType
		Mail
		Forum
		Questionario
		Iscrizione
		Account
		Altro
	End Enum
	Private Const _HashSecretKey As String = "maieutike_lab"
	Private Const _Questionario As String = "germanic"	'Gaio Giulio Cesare Claudiano Germanico
	Private Const _AttivazioneIscrizione As String = "publiova"	' Publio Quintilio Varo
	Private Const _AttivazioneAccount As String = "stilicon" 'Flavio Stilicone
	Private Const _CambioMail As String = "sulgalba" 'Servio Sulpicio Galba (pretore 54 a.C.)
	Private Const _AccessoForum As String = "agricola"	 ' Sesto Calpurnio Agricola
	Private Const _Altro As String = "decimobr"	 ' Decimo Bruto

	Public Shared Function HashKey() As String
		Return _HashSecretKey
	End Function

	Public Shared Function CryptKey(ByVal EncType As EncType) As String
		Select Case EncType
			Case SecretKeyUtil.EncType.Account
				Return _AttivazioneAccount
			Case SecretKeyUtil.EncType.Forum
				Return _AccessoForum
			Case SecretKeyUtil.EncType.Iscrizione
				Return _AttivazioneIscrizione
			Case SecretKeyUtil.EncType.Mail
				Return _CambioMail
			Case SecretKeyUtil.EncType.Questionario
				Return _Questionario
			Case SecretKeyUtil.EncType.Altro
				Return _Altro
			Case Else
				Return "fvalente" 'Fabio Valente"
		End Select
	End Function
End Class