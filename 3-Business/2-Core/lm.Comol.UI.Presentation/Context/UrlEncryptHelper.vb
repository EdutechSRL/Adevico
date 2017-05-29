Imports System.Web
Namespace lm.Comol.UI.Presentation.Utility
	Public Class UrlEncryptHelper
		Private Shared _EncryptionKeys As List(Of EncryptionKey)
		Private Shared _GetSecretHash As String

		Private Shared ReadOnly Property EncryptionKeys() As List(Of EncryptionKey)
			Get
				If IsNothing(_EncryptionKeys) Then
					_EncryptionKeys = EncryptionKey.GetDefaults
				End If
				Return _EncryptionKeys
			End Get
		End Property
		Private Shared ReadOnly Property GetSecretHash() As String
			Get
				If String.IsNullOrEmpty(_GetSecretHash) Then
					_GetSecretHash = EncryptionKey.GetSecretHash
				End If
				Return _GetSecretHash
			End Get
		End Property


		'Public Function EncryptedQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
		'	Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
		'	If Enc.DecryptVerifyURL(HttpContext.Current.Request.QueryString) Then
		'		Return Enc.Querystring(Value)
		'	Else
		'		Return ""
		'	End If
		'End Function
        Public Shared Function DecryptCommonUrl(ByVal KeyValue As String) As String
            If HttpContext.Current.Request.QueryString.HasKeys Then
                Dim Enc As New EncryptQueryString(GetSecretHash, GetCriptString(EncType.Common))
                If Enc.DecryptVerifyURL(HttpContext.Current.Request.QueryString) Then
                    Return Enc.Querystring(KeyValue)
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Function

		Private Shared Function GetCriptString(ByVal Type As EncType) As String
			Return (From o In UrlEncryptHelper.EncryptionKeys Where o.Type = Type).FirstOrDefault.Key
		End Function

	End Class
End Namespace