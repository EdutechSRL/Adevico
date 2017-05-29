Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports lm.ActionDataContract
Imports lm.Comol.Services.WS.UserAction.Domain
Imports System.Transactions
Imports RefActionService

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSuserActions
	Inherits System.Web.Services.WebService

	<WebMethod(Description:="Create User working session")> _
	Public Sub CreateWorkingSession(ByVal WorkingID As System.Guid, ByVal PersonID As Integer)
		Dim oLoginAction As New LoginAction() With {.ActionNumber = 0, .isWorkingSessionClosed = False, .PersonID = PersonID, .WorkingSessionID = WorkingID, .LoginDate = Now, .LastActionDate = .LoginDate}

		ServiceAction.ProcessOpenWorkingSession(oLoginAction)
	End Sub
	<WebMethod(Description:="Close User working session")> _
	Public Sub DeleteWorkingSession(ByVal WorkingID As System.Guid, ByVal PersonID As Integer, ByVal EndDate As DateTime)
		ServiceAction.ProcessCloseWorkingSession(WorkingID, PersonID, EndDate)
	End Sub

	<WebMethod(Description:="Aggiunge dati browser")> _
	Public Sub AddBrowserInfo(ByVal oBrowser As BrowserInfo)
		ServiceAction.ProcessBrowserInfo(oBrowser)
	End Sub


	<WebMethod(Description:="Create User working session")> _
	Public Sub OpenWorkingSession(ByVal oAction As UserAction)
		ServiceAction.ProcessOpenWorkingSession(oAction)
	End Sub
	<WebMethod(Description:="Close User working session")> _
	Public Sub CloseWorkingSession(ByVal oAction As UserAction)
		ServiceAction.ProcessCloseWorkingSession(oAction)
	End Sub
	<WebMethod(Description:="Aggiunge un'azione")> _
	Public Sub AddAction(ByVal oAction As UserAction)
		ServiceAction.ProcessAction(oAction)
	End Sub

	<WebMethod(Description:="")> _
	Public Sub ClearCache()
		ServiceAction.ClearCacheItems()
	End Sub

	<WebMethod(Description:="")> _
	Public Sub LoginLogoutTest()
		Dim oAction As New UserAction
		With oAction
			.ID = System.Guid.NewGuid
			.ActionDate = Now
			.ClientIPadress = "0periodico"
			.CommunityID = 0
			.Interaction = InteractionType.Generic
			.ModuleID = 0
			.ObjectActions = Nothing
			.PersonID = 0
			.PersonRoleID = 0
			.ProxyIPadress = "0prova"
			.Type = 0
			.WorkingSessionID = System.Guid.NewGuid
		End With
		Me.OpenWorkingSession(oAction)
		oAction.ActionDate = Now.AddHours(1)
		Me.CloseWorkingSession(oAction)
	End Sub

	<WebMethod(Description:="")> _
	Public Sub Test()
		Dim o As BrowserInfo = BrowserInfo.Create(HttpContext.Current)
		o.PersonID = -1
		o.PersonTypeID = 0
		o.WorkingSessionID = System.Guid.NewGuid
		o.ClientIPAdress = "-1"
		o.ProxyIPAdress = "-1"
		Me.AddBrowserInfo(o)


        Dim oAction As New UserAction
        With oAction
            .ID = System.Guid.NewGuid
            .ActionDate = Now
            .ClientIPadress = "0periodico"
            .CommunityID = 0
            .Interaction = InteractionType.Generic
            .ModuleID = 0
            .ObjectActions = Nothing
            .PersonID = 1
            .PersonRoleID = 0
            .ProxyIPadress = "0prova"
            .Type = 0
            .WorkingSessionID = o.WorkingSessionID
        End With
        Me.OpenWorkingSession(oAction)
	End Sub

    <WebMethod(Description:="")> _
    Public Sub TestMultiplo(ByVal num As Integer)
        For I As Integer = 1 To num
            GenerateTest(I)
        Next
    End Sub
    Private Sub GenerateTest(ByVal PersonID As Integer)
        Dim o As BrowserInfo = BrowserInfo.Create(HttpContext.Current)
        o.PersonID = PersonID
        o.PersonTypeID = 0
        o.WorkingSessionID = System.Guid.NewGuid
        o.ClientIPAdress = "-1"
        o.ProxyIPAdress = "-1"
        Me.AddBrowserInfo(o)

        Dim oAction As New UserAction
        With oAction
            .ID = System.Guid.NewGuid
            .ActionDate = Now
            .ClientIPadress = "0periodico"
            .CommunityID = 0
            .Interaction = InteractionType.Generic
            .ModuleID = 0
            .ObjectActions = Nothing
            .PersonID = PersonID
            .PersonRoleID = 0
            .ProxyIPadress = "0prova"
            .Type = 0
            .WorkingSessionID = o.WorkingSessionID
        End With
        Me.OpenWorkingSession(oAction)

        With oAction
            .ID = System.Guid.NewGuid
            .ActionDate = Now
            .ClientIPadress = "0periodico0"
            .CommunityID = 0
            .Interaction = InteractionType.ModuleToModule
            .ModuleID = 0
            .ObjectActions = Nothing
            .PersonID = PersonID
            .PersonRoleID = 0
            .ProxyIPadress = "0periodico0"
            .Type = 0
            .WorkingSessionID = o.WorkingSessionID
        End With
        oAction.ActionDate = Now.AddMinutes(2) '.AddHours(1)
        Me.AddAction(oAction)


        'With oAction
        '    .ID = System.Guid.NewGuid
        '    .ActionDate = Now
        '    .ClientIPadress = "0close"
        '    .CommunityID = 0
        '    .Interaction = InteractionType.ModuleToModule
        '    .ModuleID = 0
        '    .ObjectActions = Nothing
        '    .PersonID = 0
        '    .PersonRoleID = 0
        '    .ProxyIPadress = "0close"
        '    .Type = 0
        '    .WorkingSessionID = o.WorkingSessionID
        'End With
        'oAction.ActionDate = Now.AddMinutes(3)
        'Me.CloseWorkingSession(oAction)

    End Sub
	'<WebMethod(Description:="UserOnlineCount For community", MessageName:="UserOnlineCount For community")> _
	'Public Function CommunityUserOnlineCount(ByVal CommunityID As Integer) As Integer

	'End Function
	'<WebMethod(Description:="UserOnlineCount For community and Module", MessageName:="UserOnlineCount For community and Module")> _
	'Public Function CommunityUserOnlineCount(ByVal CommunityID As Integer, ByVal ModuleID As Integer) As Integer

	'End Function

	'<WebMethod(Description:="")> _
	'Public Sub Message(ByVal testo As String)
	'	Dim SessionID_1 As System.Guid = System.Guid.NewGuid
	'	Dim SessionID_2 As System.Guid = System.Guid.NewGuid

	'	Me.ClearCache()
	'	For i As Integer = 1 To 7
	'		SessionID_1 = System.Guid.NewGuid

	'		Dim o As New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now, .CommunityID = 1, .ModuleID = 0, .PersonID = 1, .Interaction = InteractionType.None}

	'		Me.OpenWorkingSession(o)
	'		o = New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now.AddMinutes(10), .CommunityID = 1, .ModuleID = 0, .PersonID = 1, .Interaction = InteractionType.None}
	'		Me.AddAction(o)
	'		o = New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now.AddMinutes(20), .CommunityID = 1, .ModuleID = 0, .PersonID = 1, .Interaction = InteractionType.None}
	'		o.ObjectActions.Add(New ObjectAction() With {.ModuleID = 24, .ObjectTypeId = 12, .ValueID = "23"})
	'		o.ObjectActions.Add(New ObjectAction() With {.ModuleID = 24, .ObjectTypeId = 14, .ValueID = "25"})
	'		Me.AddAction(o)
	'		o = New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now.AddMinutes(30), .CommunityID = 1, .ModuleID = 1, .PersonID = 1, .Interaction = InteractionType.None}
	'		Me.AddAction(o)
	'		o = New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now.AddMinutes(40), .CommunityID = 1, .ModuleID = 1, .PersonID = 1, .Interaction = InteractionType.None}
	'		Me.AddAction(o)
	'		o = New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now.AddMinutes(50), .CommunityID = 1, .ModuleID = 2, .PersonID = 1, .Interaction = InteractionType.None}
	'		Me.AddAction(o)
	'		o = New UserAction With {.WorkingSessionID = SessionID_1, .ActionDate = Now.AddMinutes(60), .CommunityID = 2, .ModuleID = 2, .PersonID = 1, .Interaction = InteractionType.None}
	'		Me.CloseWorkingSession(o)
	'	Next
	'	'Me.OpenWorkingSession(System.Guid.NewGuid, 2)
	'	'For i = 5 To 10
	'	'	Me.OpenWorkingSession(System.Guid.NewGuid, i)
	'	'Next
	'	'For i = 5 To 10
	'	'	Me.AddBrowserInfo(New BrowserInfo() With {.PersonID = i, .WorkingSessionID = System.Guid.NewGuid})
	'	'Next
	'End Sub
End Class