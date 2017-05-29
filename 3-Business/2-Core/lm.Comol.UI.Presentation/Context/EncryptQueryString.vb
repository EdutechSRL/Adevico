Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography


Namespace lm.Comol.UI.Presentation
	Public Class EncryptQueryString

#Region "Private property"
		Private key() As Byte = {}
		Private IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
		Private keycryptlength As Object = 8
		Private RandomGenerator As New RandomKeyGenerator
		Private keyhash As String
		Private keycrypt As String
		Private queryhashed As New Hashtable
		Private metod As HashMethod
#End Region

#Region "Property"
		Public Property HashingMethod() As HashMethod
			Get
				Return metod
			End Get
			Set(ByVal value As HashMethod)
				metod = value
			End Set
		End Property

		Public WriteOnly Property Hashing_Key() As String
			Set(ByVal value As String)
				keyhash = value
			End Set
		End Property

		Public WriteOnly Property Crypto_Key() As String
			Set(ByVal value As String)
				If value.Length > keycryptlength Then
					keycrypt = Left(value, keycryptlength)
				Else
					If value.Length < keycryptlength Then
						keycrypt = value + StrDup(keycryptlength - value.Length, "A")
					Else
						keycrypt = value
					End If

				End If
			End Set
		End Property

		Public ReadOnly Property Querystring(ByVal queryname As String) As String
			Get
				Dim ret As String = queryhashed(queryname.ToLower)
				Return ret
			End Get
		End Property

		Public ReadOnly Property Querystring() As Hashtable
			Get
				Return queryhashed
			End Get
		End Property
#End Region

		Public Function ArmorURL(ByVal data As String) As String
			Dim datacrpt As String = Me.Encrypt(data, keycrypt)
			Dim hash As String = Me.GenerateHashDigest(keyhash + datacrpt + keyhash, HashingMethod)

			Dim datakey As String = Me.rndgen(6) + "="
			Dim hashkey As String = Me.rndgen(6) + "="

			Return datakey + datacrpt + "&" + hashkey + hash + "&vvv=63"
		End Function

		Public Function DecryptVerifyURL(ByVal querystring As System.Collections.Specialized.NameValueCollection) As Boolean
			Dim hash As String = "1"
			Dim data As String = "2"
			Dim ok As Boolean = True

            If querystring.HasKeys AndAlso querystring.Keys.Count > 1 Then
                Try
                    data = querystring(0).Replace(" ", "+")
                    hash = querystring(1).Replace(" ", "+")
                Catch ex As Exception
                    ok = False
                End Try

                If hash <> Me.GenerateHashDigest(Me.keyhash + data + Me.keyhash, HashMethod.MD5) Then
                    ok = False
                Else
                    If data <> "" Then
                        data = Me.Decrypt(data, Me.keycrypt)
                        Dim x() As String
                        x = data.Split("&")
                        Try
                            For Each st As String In x
                                Dim y() As String
                                y = st.Split("=")
                                queryhashed.Add(y(0).ToLower, y(1))

                            Next
                        Catch ex As Exception
                            ok = False
                        End Try
                    End If
                End If
            Else
                ok = False
            End If
         
			Return ok
		End Function

		Enum HashMethod
			MD5
			SHA1
			SHA384
		End Enum

		Public Sub New()
			MyBase.new()
			RandomGenerator.KeyChars = 6
			RandomGenerator.KeyLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			RandomGenerator.KeyNumbers = "0123456789"

			HashingMethod = HashMethod.MD5
		End Sub
		Public Sub New(ByVal hash As String, ByVal crypt As String)
			Me.new()

			Me.Crypto_Key = crypt
			Me.Hashing_Key = hash
		End Sub

		Private Function rndgen(ByVal numcar As Integer) As String
			RandomGenerator.KeyChars = numcar
			Return RandomGenerator.Generate()
		End Function
		Private Function rndgen() As String
			Return RandomGenerator.Generate()
		End Function
		Private Function GenerateHashDigest(ByVal source As String, ByVal algorithm As HashMethod) As String
			Dim hashAlgorithm As HashAlgorithm = Nothing
			Select Case algorithm
				Case HashMethod.MD5
					hashAlgorithm = New MD5CryptoServiceProvider
				Case HashMethod.SHA1
					hashAlgorithm = New SHA1CryptoServiceProvider
				Case HashMethod.SHA384
					hashAlgorithm = New SHA384Managed
				Case Else
					' Error case.
			End Select

			Dim byteValue() As Byte = Encoding.UTF8.GetBytes(source)
			Dim hashValue() As Byte = hashAlgorithm.ComputeHash(byteValue)
			Return Convert.ToBase64String(hashValue)
		End Function
		Private Function Decrypt(ByVal stringToDecrypt As String, _
		 ByVal sEncryptionKey As String) As String
			Dim inputByteArray(stringToDecrypt.Length) As Byte
			Try
				key = System.Text.Encoding.UTF8.GetBytes(Left(sEncryptionKey, keycryptlength))
				Dim des As New DESCryptoServiceProvider()
				inputByteArray = Convert.FromBase64String(stringToDecrypt)
				Dim ms As New MemoryStream()
				Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV), _
				 CryptoStreamMode.Write)
				cs.Write(inputByteArray, 0, inputByteArray.Length)
				cs.FlushFinalBlock()
				Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
				Return encoding.GetString(ms.ToArray())
			Catch e As Exception
				Return e.Message
			End Try
		End Function
		Private Function Encrypt(ByVal stringToEncrypt As String, _
		 ByVal SEncryptionKey As String) As String
			Try
				key = System.Text.Encoding.UTF8.GetBytes(Left(SEncryptionKey, 8))
				Dim des As New DESCryptoServiceProvider()
				Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes( _
				 stringToEncrypt)
				Dim ms As New MemoryStream()
				Dim cs As New CryptoStream(ms, des.CreateEncryptor(key, IV), _
				 CryptoStreamMode.Write)
				cs.Write(inputByteArray, 0, inputByteArray.Length)
				cs.FlushFinalBlock()
				Return Convert.ToBase64String(ms.ToArray())
			Catch e As Exception
				Return e.Message
			End Try
        End Function
    End Class
End Namespace