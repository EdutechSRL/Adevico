Imports AdamTibi.Web.Security
Imports System.Web

Public Class SecuredCookie
    Public Shared Function encode_cookie(ByVal name As String, ByVal domain As String, ByVal expireMin As Integer, ByVal values As Hashtable) As HttpCookie
        Dim cookie As New HttpCookie(name)
        cookie.Domain = domain
        cookie.Expires = Now.AddMinutes(expireMin)

        For Each key As String In values.Keys
            cookie.Values(key) = values(key)
        Next
        '  MachineKeyCryptography.Encode(name, 
        ' Dim enc_cookie As HttpCookie = HttpSecureCookie.Encode(cookie)
        Return cookie
    End Function

    Public Shared Function decode_cookie(ByVal cookie As HttpCookie) As Hashtable
        'Dim dec_cookie As HttpCookie
        'dec_cookie = HttpSecureCookie.Decode(cookie)
        Dim hash As New Hashtable

        For Each key As String In cookie.Values.Keys
            hash(key) = cookie.Values(key)
        Next

        Return hash
    End Function
End Class
