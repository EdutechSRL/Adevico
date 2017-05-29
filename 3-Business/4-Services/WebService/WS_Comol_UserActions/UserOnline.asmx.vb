Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports lm.ActionDataContract
Imports lm.Comol.Services.WS.UserAction.Domain
Imports System.Transactions
Imports RefActionService
Imports lm.WS.UserAction.Domain

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class UserOnline
    Inherits System.Web.Services.WebService


#Region "OnLineUser"
    <WebMethod(Description:="")> _
      Public Function GetGenericUsersOnline(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of UserAction)
        Return OnLineAction.GetGenericUsersOnline(CommunityID, ModuleID, PageSize, PageIndex, Order, Ascending)
    End Function

    <WebMethod(Description:="GetSelectedUsersOnline", MessageName:="GetSelectedUsersOnline")> _
    Public Function GetSelectedUsersOnline(ByVal oDtoAccess As List(Of dtoAccess), ByVal oOrder As OrderItemsBy) As List(Of UserAction)
        If oDtoAccess.Count > 0 Then
            Select Case oOrder
                Case OrderItemsBy.CommunityName
                    Return OnLineAction.SelectedCommunityOnline(oDtoAccess)
                Case OrderItemsBy.ModuleName
                    Return OnLineAction.SelectedModulesOnline(oDtoAccess)
                Case OrderItemsBy.UserName
                    Return OnLineAction.SelectedUsersOnline(oDtoAccess)
            End Select
        End If
        Return New List(Of UserAction)
    End Function

    <WebMethod(Description:="GetUsersOnlineID", MessageName:="GetUsersOnlineID")> _
    Public Function GetUsersOnlineID(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
        Return OnLineAction.GetUsersOnlineID(CommunityID, ModuleID, Order, Ascending)
    End Function
    <WebMethod(Description:="GetUsersOnlineCommunityID", MessageName:="GetUsersOnlineCommunityID")> _
      Public Function GetUsersOnlineCommunitiesID(ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
        Return OnLineAction.GetCommunityOnlineID(ModuleID, Order, Ascending)
    End Function
    <WebMethod(Description:="GetUsersOnlineModulesID", MessageName:="GetUsersOnlineModulesID")> _
    Public Function GetUsersOnlineModulesID(ByVal CommunityID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
        Return OnLineAction.GetModuleOnlineID(CommunityID, Order, Ascending)
    End Function

    <WebMethod(Description:="UserOnlineCount", MessageName:="UsersOnlineCount")> _
    Public Function UserOnlineCount(ByVal CommunityID As Integer, ByVal ModuleID As Integer) As Integer
        Return OnLineAction.UserOnLineCount(CommunityID, ModuleID, True)
    End Function
    <WebMethod(Description:="WorkingSessionOnlineCount", MessageName:="WorkingSessionOnlineCount")> _
    Public Function WorkingSessionOnlineCount(ByVal CommunityID As Integer, ByVal ModuleID As Integer) As Integer
        Return OnLineAction.UserOnLineCount(CommunityID, ModuleID, False)
    End Function


    <WebMethod(Description:="GetOnLineActions", MessageName:="GetOnLineActions")> _
   Public Function GetOnLineActions(ByVal CommunityID As Integer, ByVal ModuleID As Integer) As List(Of OnLineUserAction)
        Return OnLineAction.GetOnLineActions(CommunityID, ModuleID)
    End Function

    <WebMethod(Description:="IsUserOnline", MessageName:="IsUserOnline")> _
    Public Function IsUserOnline(ByVal IdUser As Integer, ByVal workingSessionId As System.Guid) As Boolean
        Return OnLineAction.IsUserOnline(IdUser, 0, workingSessionId)
    End Function
    <WebMethod(Description:="IsUserOnlineIntoCommunity", MessageName:="IsUserOnlineIntoCommunity")> _
    Public Function IsUserOnlineIntoCommunity(ByVal IdUser As Integer, ByVal IdCommunity As Integer, ByVal workingSessionId As System.Guid) As Boolean
        Return OnLineAction.IsUserOnline(IdUser, IdCommunity, workingSessionId)
    End Function


    <WebMethod(Description:="Recupera dalla cache tutte le azioni.", MessageName:="Test_RecuperaTutteLeAzioniDallaCache")> _
 Public Function _Test_RecuperaTutteLeAzioniDallaCache() As List(Of lm.ActionDataContract.UserAction)
        Dim Prefix As String = String.Format(ServiceUtility.ActionKey, "_", "")
        Dim oList As List(Of lm.ActionDataContract.UserAction) = CacheActions(Of lm.ActionDataContract.UserAction).GetByPrefix(Prefix)

        Return oList
    End Function

    <WebMethod(Description:="Recupera dalla cache le ultime azioni entro l'ultima ora.", MessageName:="Test_RecuperaElementiMAxUnoraFa")> _
Public Function _Test_RecuperaElementiMAxUnoraFa() As List(Of lm.ActionDataContract.UserAction)
        Dim CurrentTime As DateTime = Now
        CurrentTime = CurrentTime.AddSeconds(-3600)

        Dim oListReturn As List(Of lm.ActionDataContract.UserAction)
        oListReturn = (From o In _Test_RecuperaTutteLeAzioniDallaCache() Where o.ActionDate >= CurrentTime Select o).ToList
        Return oListReturn
    End Function

    <WebMethod(Description:="Rimuovi gli elementi più vecchi della cache !", MessageName:="Test_RemoveNotRecent")> _
Public Sub _Test_RemoveNotRecent()
        Dim CurrentTime As DateTime = Now
        CurrentTime = CurrentTime.AddSeconds(-3600)

        Dim oListReturn As List(Of lm.ActionDataContract.UserAction)
        oListReturn = (From o In _Test_RecuperaTutteLeAzioniDallaCache() Where o.ActionDate < CurrentTime Select o).ToList

        For Each o In oListReturn
            Dim UserActionKey As String = String.Format("wsa_useraction{0}{1}", "_" & o.PersonID, "_" & o.WorkingSessionID.ToString)
            CacheActions(Of lm.ActionDataContract.UserAction).RemoveFromCache(UserActionKey)
        Next
    End Sub
#End Region
End Class