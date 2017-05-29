Namespace lm.Comol.Modules.UserActions.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoOnLineUser
        Public OwnerID As Integer
        Public Owner As String
        Public ModuleID As Integer
        Public ModuleName As String
        Public CommunityID As Integer
        Public CommunityName As String
        Public FirstAction As DateTime
        Public LastAction As TimeSpan
        Public ActionType As Integer
        Public ActionName As String
        Public ClientIP As String
        Public ProxyIP As String
        Public WorkingSessionID As System.Guid
        Public Sub New()

        End Sub

        Public Sub New(ByVal o As WS_OnLine.OnLineUserAction, ByVal Last As TimeSpan)
            OwnerID = o.PersonID
            FirstAction = o.AccessDate
            LastAction = Last
            ClientIP = o.ClientIPadress
            CommunityID = o.CommunityID
            ModuleID = o.ModuleID
            ProxyIP = o.ProxyIPadress
            ActionType = o.Type
            WorkingSessionID = o.WorkingSessionID

        End Sub
        Public Function IpToString() As String
            If String.IsNullOrEmpty(ClientIP) AndAlso String.IsNullOrEmpty(ProxyIP) Then
                Return "--"
            ElseIf String.IsNullOrEmpty(ClientIP) Then
                Return ProxyIP
            ElseIf String.IsNullOrEmpty(ProxyIP) Then
                Return ClientIP
            Else
                Return ProxyIP & " (" & ClientIP & ")"
            End If
        End Function
    End Class
End Namespace