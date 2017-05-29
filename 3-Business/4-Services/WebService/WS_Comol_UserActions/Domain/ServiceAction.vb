Imports lm.ActionDataContract
Imports lm.Comol.Core.Cache
Imports lm.WS.UserAction.Configuration
Imports lm.WS.UserAction.Domain


Namespace lm.Comol.Services.WS.UserAction.Domain
	Public Class ServiceAction

#Region "Shared"
        Private Shared _Config As ServiceConfiguration
        Protected Shared ReadOnly Property Config() As ServiceConfiguration
            Get
                If IsNothing(_Config) Then
                    _Config = ServiceConfiguration.CreateConfigSettings
                End If
                Return _Config
            End Get
		End Property
		'Protected Shared ReadOnly Property OnLineUsers() As List(Of ActionDataContract.UserAction)
		'	Get
		'		'	Dim oList As List(Of ActionDataContract.UserAction) = CacheActions(Of List(Of ActionDataContract.UserAction)).GetFromCache(ServiceUtility.WebPresenceKey)
		'		Dim Prefix As String = String.Format(ServiceUtility.ActionKey, "_", "")
		'		Dim oList As List(Of ActionDataContract.UserAction) = CacheActions(Of ActionDataContract.UserAction).GetByPrefix(Prefix)
		'		If IsNothing(oList) Then
		'			oList = New List(Of ActionDataContract.UserAction)
		'			CacheActions(Of List(Of ActionDataContract.UserAction)).SendToCache(oList, ServiceUtility.WebPresenceKey)
		'		End If
		'		Return oList
		'	End Get
		'End Property
#End Region

#Region "Caching"
		Private Shared Function ActionKey(ByVal PersonID As Integer, ByVal WorkingSessionID As System.Guid) As String
			Dim _ActionKey As String = Config.CacheKey(ServiceConfiguration.KeyType.Action)
            Return String.Format(_ActionKey, "_" & PersonID, "_" & WorkingSessionID.ToString)
		End Function
		Private Shared Function LastActionKey(ByVal PersonID As Integer) As String
			Dim _ActionKey As String = Config.CacheKey(ServiceConfiguration.KeyType.LastAction)
			Return String.Format(_ActionKey, "_" & PersonID)
		End Function
		Private Shared Function LoginKey(ByVal PersonID As Integer, ByVal WorkingSessionID As System.Guid) As String
			Dim _LoginKey As String = Config.CacheKey(ServiceConfiguration.KeyType.Login)
			Return String.Format(_LoginKey, "_" & PersonID, "_" & WorkingSessionID.ToString)
		End Function
#End Region

#Region "Message Queue"
		Private Shared _MessageQueue As iActionService = MessageQueueFactory.Service
#End Region

#Region "Process Actions"
		Public Shared Sub ProcessOpenWorkingSession(ByVal oLoginAction As LoginAction)
			If Config.PersistLogonAction Then
				ServiceAction._MessageQueue.InsertLoginAction(oLoginAction)
			End If
			CacheActions(Of LoginAction).SendToCache(oLoginAction, LoginKey(oLoginAction.PersonID, oLoginAction.WorkingSessionID))
		End Sub
		Public Shared Sub ProcessCloseWorkingSession(ByVal WorkingSessionID As System.Guid, ByVal PersonID As Integer, ByVal EndDate As DateTime)
			Dim oLoginAction As ActionDataContract.LoginAction = GetUpdatedLoginAction(WorkingSessionID, PersonID, EndDate, True)
			If Not IsNothing(oLoginAction) Then
				ServiceAction._MessageQueue.UpdateLoginAction(oLoginAction)
			End If
		End Sub

		Public Shared Sub ProcessOpenWorkingSession(ByVal oAction As ActionDataContract.UserAction)
			AnalyzeAction(oAction)
			Dim oLoginAction As New LoginAction With {.isWorkingSessionClosed = False, .ActionNumber = 1, .LastActionDate = oAction.ActionDate, .LoginDate = oAction.ActionDate, .PersonID = oAction.PersonID, .WorkingSessionID = oAction.WorkingSessionID, .ClientIPadress = oAction.ClientIPadress, .ProxyIPadress = oAction.ProxyIPadress, .PersonRoleID = oAction.PersonRoleID}

			If Config.PersistLogonAction Then
				ServiceAction._MessageQueue.InsertLoginAction(oLoginAction)
			End If
			CacheActions(Of LoginAction).SendToCache(oLoginAction, LoginKey(oLoginAction.PersonID, oLoginAction.WorkingSessionID))

		End Sub
		Public Shared Sub ProcessCloseWorkingSession(ByVal oAction As ActionDataContract.UserAction)
			AnalyzeAction(oAction, True)
			Dim oLoginAction As LoginAction = GetUpdatedLoginAction(oAction.WorkingSessionID, oAction.PersonID, oAction.ActionDate, True)
			If Not IsNothing(oLoginAction) Then
				ServiceAction._MessageQueue.UpdateLoginAction(oLoginAction)
			End If
			CacheActions(Of ActionDataContract.LoginAction).RemoveFromCache(LoginKey(oAction.PersonID, oAction.WorkingSessionID))
			CacheActions(Of ActionDataContract.UserAction).RemoveFromCache(LastActionKey(oAction.PersonID))
			CacheActions(Of ActionDataContract.UserAction).RemoveFromCache(ActionKey(oAction.PersonID, oAction.WorkingSessionID))

			'RemoveFromWebPresence(oAction)
		End Sub

		Public Shared Sub ProcessBrowserInfo(ByVal oBrowserInfo As BrowserInfo)
			If Config.PersistBrowser Then
				ServiceAction._MessageQueue.InsertBrowserInfo(oBrowserInfo)
			End If
		End Sub
		Public Shared Sub ProcessAction(ByVal oAction As ActionDataContract.UserAction)
			AnalyzeAction(oAction)
			Dim oLoginAction As LoginAction = GetUpdatedLoginAction(oAction.WorkingSessionID, oAction.PersonID, oAction.ActionDate, False)
			If Not IsNothing(oLoginAction) Then
				ServiceAction._MessageQueue.UpdateLoginAction(oLoginAction)
			End If
		End Sub

		Private Shared Sub AnalyzeAction(ByVal oAction As ActionDataContract.UserAction, Optional ByVal ForLogout As Boolean = False)
			If Config.PersistUserAction Then
				ServiceAction._MessageQueue.InsertUserAction(oAction)
			End If

			Dim UserActionKey As String = ActionKey(oAction.PersonID, oAction.WorkingSessionID)
			If Config.PersistLogonAction Then
				AnalyzeCommunityAction(oAction, ForLogout)
			End If
			If Config.PersistUsageTime Then
				Dim LastKey As String = LastActionKey(oAction.PersonID)
				Dim oLastAction As ActionDataContract.UserAction = CacheActions(Of ActionDataContract.UserAction).GetFromCache(LastKey)

				AnalyzeModuleAction(oAction, ForLogout)
				AnalyzeUsageTime(oAction, oLastAction)

                CacheActions(Of ActionDataContract.UserAction).SendToCache(oAction, LastKey)
			End If
			'AddToWebPresence(oAction)
			CacheActions(Of ActionDataContract.UserAction).SendToCache(oAction, UserActionKey)
		End Sub
		Private Shared Function GetUpdatedLoginAction(ByVal WorkingSessionID As System.Guid, ByVal PersonID As Integer, ByVal LastDate As DateTime, Optional ByVal isClosed As Boolean = False) As LoginAction
			Dim oLoginAction As LoginAction = Nothing
			If Config.PersistLogonAction Then
				Dim oKey As String = LoginKey(PersonID, WorkingSessionID)
				oLoginAction = CacheActions(Of LoginAction).GetFromCache(oKey)
				If IsNothing(oLoginAction) Then
					oLoginAction = New LoginAction() With {.WorkingSessionID = WorkingSessionID, .PersonID = PersonID, .ActionNumber = 0, .LoginDate = LastDate}
				End If
				oLoginAction.isWorkingSessionClosed = isClosed
				oLoginAction.ActionNumber += 1
				If oLoginAction.LastActionDate < LastDate Then
					oLoginAction.LastActionDate = LastDate
				End If
			End If

			Return oLoginAction
		End Function

		'http://forums.asp.net/p/955145/1173230.aspx#1173230


#Region "Analisi accessi comunità"
		Private Shared Sub AnalyzeCommunityAction(ByVal oAction As ActionDataContract.UserAction, Optional ByVal ForLogout As Boolean = False)
			Dim UserActionKey As String = ActionKey(oAction.PersonID, oAction.WorkingSessionID)
			If Config.PersistLogonAction Then
				Dim oPreviousAction As ActionDataContract.UserAction = CacheActions(Of ActionDataContract.UserAction).GetFromCache(UserActionKey)
				Dim oCurrentCommunityAction As CommunityAction = CreateCommunityActionFromAction(oAction)
				If ForLogout Then
					oCurrentCommunityAction.isExitCommunity = True
				End If
				If IsNothing(oPreviousAction) Then
					ServiceAction._MessageQueue.InsertCommunityAction(oCurrentCommunityAction)
				ElseIf oPreviousAction.CommunityID = oAction.CommunityID Then
					ServiceAction._MessageQueue.UpdateCommunityAction(oCurrentCommunityAction)
				Else
					Dim oPreviousCommunityAction As CommunityAction = CreateCommunityActionFromAction(oPreviousAction)
					oPreviousCommunityAction.isExitCommunity = True
					oPreviousCommunityAction.LastActionDate = oCurrentCommunityAction.LastActionDate

					ServiceAction._MessageQueue.UpdateCommunityAction(oPreviousCommunityAction)
					ServiceAction._MessageQueue.InsertCommunityAction(oCurrentCommunityAction)
				End If
			End If
		End Sub
		Private Shared Function CreateCommunityActionFromAction(ByVal oAction As ActionDataContract.UserAction) As CommunityAction
			Dim oResult As New CommunityAction

			With oResult
				.AccessDate = oAction.ActionDate
				.CommunityID = oAction.CommunityID
				.LastActionDate = oAction.ActionDate
				.PersonID = oAction.PersonID
				.WorkingSessionID = oAction.WorkingSessionID
				.PersonRoleID = oAction.PersonRoleID
			End With
			Return oResult
		End Function
#End Region

#Region "Analisi accessi modulo"
		Private Shared Sub AnalyzeUsageTime(ByVal oAction As ActionDataContract.UserAction, ByVal oLastAction As ActionDataContract.UserAction)
			Dim oResult As ModuleUsageTime = Nothing
            Dim oPreviousUsage As ModuleUsageTime = Nothing
			If IsNothing(oLastAction) Then
				oResult = CreateUsageTimeFromAction(oAction)
				oResult.ActionNumber = 1
			ElseIf oAction.CommunityID = oLastAction.CommunityID AndAlso oAction.ModuleID = oLastAction.ModuleID Then
				oResult = CreateUsageTimeFromAction(oAction)
				'oResult.UsageTime = DateDiff(DateInterval.Second, oLastAction.ActionDate, oAction.ActionDate) ' Convert.ToUInt32(oAction.ActionDate.Subtract(oLastAction.ActionDate).TotalSeconds)
				'oPreviousUsage.UsageTime = CInt(oAction.ActionDate.Subtract(oLastAction.ActionDate).TotalMilliseconds / 1000)
				oResult.UsageTime = GetSeconds(oLastAction.ActionDate, oAction.ActionDate)


				If oResult.UsageTime < 0 Then : oResult.UsageTime *= -1
				End If
				If oAction.WorkingSessionID <> oLastAction.WorkingSessionID Then : oResult.ActionNumber = 1
				End If
			Else
				oPreviousUsage = CreateUsageTimeFromAction(oLastAction)
				'oPreviousUsage.UsageTime = DateDiff(DateInterval.Second, oLastAction.ActionDate, oAction.ActionDate)
				'	oPreviousUsage.UsageTime = CInt(oAction.ActionDate.Subtract(oLastAction.ActionDate).TotalMilliseconds / 1000)

				oPreviousUsage.UsageTime = GetSeconds(oLastAction.ActionDate, oAction.ActionDate)
				If oPreviousUsage.UsageTime < 0 Then : oPreviousUsage.UsageTime *= -1
				End If
				oResult = CreateUsageTimeFromAction(oAction)
                oResult.ActionNumber = 1
            End If
			If Not IsNothing(oResult) Then
				ServiceAction._MessageQueue.UpdateModuleUsageTime(oResult)
            End If
            If Not IsNothing(oPreviousUsage) Then
                ServiceAction._MessageQueue.UpdateModuleUsageTime(oPreviousUsage)
            End If
        End Sub

		Private Shared Function CreateUsageTimeFromAction(ByVal oAction As ActionDataContract.UserAction) As ModuleUsageTime
			Dim oResult As New ModuleUsageTime

			With oResult
				.CommunityID = oAction.CommunityID
				.ActionDate = oAction.ActionDate.Date
				.ModuleID = oAction.ModuleID
				.UsageTime = 0
				.PersonID = oAction.PersonID
			End With
			Return oResult
		End Function
		Private Shared Sub AnalyzeModuleAction(ByVal oAction As ActionDataContract.UserAction, Optional ByVal ForLogout As Boolean = False)
			Dim UserActionKey As String = ActionKey(oAction.PersonID, oAction.WorkingSessionID)
			Dim oPreviousAction As ActionDataContract.UserAction = CacheActions(Of ActionDataContract.UserAction).GetFromCache(UserActionKey)
			Dim oCurrentModuleAction As ModuleAction = CreateModuleActionFromAction(oAction)

			If ForLogout Then
				oCurrentModuleAction.isExitModule = True
			End If
			If IsNothing(oPreviousAction) Then
				ServiceAction._MessageQueue.InsertModuleAction(oCurrentModuleAction)
			ElseIf oPreviousAction.CommunityID = oAction.CommunityID AndAlso oPreviousAction.ModuleID = oAction.ModuleID Then
				ServiceAction._MessageQueue.UpdateModuleAction(oCurrentModuleAction)
			Else
				Dim oPreviousModuleAction As ModuleAction = CreateModuleActionFromAction(oPreviousAction)
				oPreviousModuleAction.isExitModule = True
				oPreviousModuleAction.LastActionDate = oCurrentModuleAction.LastActionDate

				ServiceAction._MessageQueue.UpdateModuleAction(oPreviousModuleAction)
				ServiceAction._MessageQueue.InsertModuleAction(oCurrentModuleAction)
			End If
		End Sub
		Private Shared Function CreateModuleActionFromAction(ByVal oAction As ActionDataContract.UserAction) As ModuleAction
			Dim oResult As New ModuleAction

			With oResult
				.AccessDate = oAction.ActionDate
				.CommunityID = oAction.CommunityID
				.LastActionDate = oAction.ActionDate
				.PersonID = oAction.PersonID
				.WorkingSessionID = oAction.WorkingSessionID
				.PersonRoleID = oAction.PersonRoleID
				.ModuleID = oAction.ModuleID
			End With
			Return oResult
		End Function
#End Region


#End Region

		Public Shared Sub ClearCacheItems()
			Dim oCache As iCache = ServiceUtility.CurrentCache
			oCache.RemoveByPrefix(String.Format(ServiceUtility.LastActionKey, ""))
			oCache.RemoveByPrefix(String.Format(ServiceUtility.LoginKey, "", ""))
            oCache.RemoveByPrefix(String.Format(ServiceUtility.ActionKey, "", ""))
            oCache.RemoveByPrefix(ServiceUtility.WebPresenceKey)
		End Sub

		'#Region "Presence"
		'		'Private Shared Sub AddToWebPresence(ByVal oAction As ActionDataContract.UserAction)
		'		'	Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
		'		'	If IsNothing(oList) Then
		'		'		oList = New List(Of ActionDataContract.UserAction)
		'		'		oList.Add(oAction)
		'		'	Else
		'		'		Dim oTemp As ActionDataContract.UserAction = (From o In oList Where o.WorkingSessionID = oAction.WorkingSessionID And o.PersonID = oAction.PersonID).FirstOrDefault
		'		'		If IsNothing(oTemp) Then
		'		'			oList.Add(oAction)
		'		'		Else
		'		'			oTemp = oAction
		'		'		End If
		'		'	End If
		'		'End Sub
		'		'Private Shared Sub RemoveFromWebPresence(ByVal oAction As ActionDataContract.UserAction)
		'		'	Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers

		'		'	If Not IsNothing(oList) Then
		'		'		Dim oTemp As ActionDataContract.UserAction = (From o In oList Where o.WorkingSessionID = oAction.WorkingSessionID And o.PersonID = oAction.PersonID).FirstOrDefault
		'		'		If Not IsNothing(oTemp) Then
		'		'			oList.Remove(oTemp)
		'		'		End If
		'		'	End If
		'		'End Sub
		'		'Private Shared Function CreateWebPresence(ByVal oAction As ActionDataContract.UserAction) As WebPresence
		'		'	Dim oResult As New WebPresence

		'		'	With oResult
		'		'		.ClientIPadress = oAction.ClientIPadress
		'		'		.CommunityID = oAction.CommunityID
		'		'		.LastDate = oAction.ActionDate
		'		'		.ModuleID = oAction.ModuleID
		'		'		.PersonID = oAction.PersonID
		'		'		.ProxyIPadress = oAction.ProxyIPadress
		'		'		.WorkingSessionID = oAction.WorkingSessionID
		'		'		.PersonID = oAction.PersonID
		'		'		.PersonRoleID = oAction.PersonRoleID
		'		'	End With
		'		'	Return oResult
		'		'End Function

		'		Public Shared Function SelectedUsersOnline(ByVal oDtoAccess As List(Of dtoAccess)) As List(Of ActionDataContract.UserAction)
		'			Dim oActionList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers

		'			Dim oQuery = (From oa In oDtoAccess Group Join oAction In oActionList On oa.WorkingSessionID Equals oAction.WorkingSessionID And oa.ID Equals oAction.PersonID _
		'			  Into children = Group From child In children.DefaultIfEmpty(New ActionDataContract.UserAction() With {.WorkingSessionID = System.Guid.Empty}) Select child)

		'			Return (From o In oQuery Where o.WorkingSessionID <> System.Guid.Empty).ToList
		'		End Function
		'		Public Shared Function SelectedCommunityOnline(ByVal oDtoAccess As List(Of dtoAccess)) As List(Of ActionDataContract.UserAction)
		'			Dim oActionList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers

		'			Dim oQuery = (From oa In oDtoAccess Group Join oAction In oActionList On oa.WorkingSessionID Equals oAction.WorkingSessionID And oa.ID Equals oAction.CommunityID _
		'			  Into children = Group From child In children.DefaultIfEmpty(New ActionDataContract.UserAction() With {.WorkingSessionID = System.Guid.Empty}) Select child)

		'			Return (From o In oQuery Where o.WorkingSessionID <> System.Guid.Empty).ToList
		'		End Function
		'		Public Shared Function SelectedModulesOnline(ByVal oDtoAccess As List(Of dtoAccess)) As List(Of ActionDataContract.UserAction)
		'			Dim oActionList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers

		'			Dim oQuery = (From oa In oDtoAccess Group Join oAction In oActionList On oa.WorkingSessionID Equals oAction.WorkingSessionID And oa.ID Equals oAction.ModuleID _
		'	 Into children = Group From child In children.DefaultIfEmpty(New ActionDataContract.UserAction() With {.WorkingSessionID = System.Guid.Empty}) Select child)

		'			Return (From o In oQuery Where o.WorkingSessionID <> System.Guid.Empty).ToList
		'		End Function


		'		'Public Shared Function CommunityUserOnlineCount(Optional ByVal CommunityID As Integer = -1, Optional ByVal ModuleID As Integer = -1) As Integer
		'		'	Dim Totale As Integer = 0
		'		'	Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers

		'		'	If DistinctValue Then
		'		'		Totale = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o.PersonID Distinct).Count
		'		'	Else
		'		'		Totale = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o.PersonID).Count
		'		'	End If


		'		'	Return Totale
		'		'End Function
		'		Public Shared Function UserOnLineCount(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal DistinctValue As Boolean)
		'			Dim Totale As Integer = 0
		'			Try
		'				Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
		'				If DistinctValue Then
		'					Totale = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o.PersonID Distinct).Count
		'				Else
		'					Totale = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o.PersonID).Count
		'				End If

		'			Catch ex As Exception

		'			End Try

		'			Return Totale
		'		End Function

		'		Public Shared Function PortalUsersOnline(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)
		'			Return GetUsersOnline(PageSize, PageIndex, -1, -1, Order, Ascending)
		'		End Function
		'		Public Shared Function CommunityUserOnline(ByVal CommunityID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)
		'			Return GetUsersOnline(PageSize, PageIndex, CommunityID, -1, Order, Ascending)
		'		End Function

		'		Public Shared Function GetGenericUsersOnline(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)

		'			Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
		'			Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Distinct Select o)

		'			If PageSize > 0 Then
		'				oQuery = oQuery.Skip(PageIndex).Take(PageSize)
		'			End If
		'			If Order = OrderItemsBy.LastAction Then
		'				oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
		'			End If
		'			If Not Ascending Then
		'				oQuery = oQuery.Reverse
		'			End If
		'			Return oQuery.ToList
		'		End Function

		'		Private Shared Function GetUsersOnline(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)

		'			Return GetGenericUsersOnline(CommunityID, ModuleID, PageSize, PageIndex, Order, Ascending)
		'		End Function

		'		Public Shared Function GetUsersOnlineID(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
		'			Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
		'			Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o)
		'			Dim oReturnList As New List(Of dtoAccess)
		'			If Order = OrderItemsBy.LastAction Then
		'				oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
		'				If Not Ascending Then
		'					oQuery = oQuery.Reverse
		'				End If
		'			End If
		'			oReturnList = (From o In oQuery Distinct Select New dtoAccess With {.ID = o.PersonID, .WorkingSessionID = o.WorkingSessionID}).ToList
		'			If IsNothing(oReturnList) Then
		'				oReturnList = New List(Of dtoAccess)
		'			End If
		'			Return oReturnList
		'		End Function
		'		Public Shared Function GetCommunityOnlineID(ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
		'			Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
		'			Dim oQuery = (From o In oList Where (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o)
		'			Dim oReturnList As New List(Of dtoAccess)

		'			If Order = OrderItemsBy.LastAction Then
		'				oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
		'				If Not Ascending Then
		'					oQuery = oQuery.Reverse
		'				End If
		'			End If
		'			oReturnList = (From o In oQuery Distinct Select New dtoAccess With {.ID = o.CommunityID, .WorkingSessionID = o.WorkingSessionID}).ToList
		'			If IsNothing(oReturnList) Then
		'				oReturnList = New List(Of dtoAccess)
		'			End If
		'			Return oReturnList
		'		End Function
		'		Public Shared Function GetModuleOnlineID(ByVal CommunityID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
		'			Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
		'			Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) Select o)
		'			Dim oReturnList As New List(Of dtoAccess)

		'			If Order = OrderItemsBy.LastAction Then
		'				oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
		'				If Not Ascending Then
		'					oQuery = oQuery.Reverse
		'				End If
		'			End If

		'			oReturnList = (From o In oQuery Distinct Select New dtoAccess With {.ID = o.ModuleID, .WorkingSessionID = o.WorkingSessionID}).ToList
		'			If IsNothing(oReturnList) Then
		'				oReturnList = New List(Of dtoAccess)
		'			End If
		'			Return oReturnList
		'		End Function
		'#End Region

		Public Shared Function GetSeconds(ByVal oDataI As DateTime, ByVal oDataF As DateTime) As Integer
			Dim SerialI As DateTime = New DateTime(oDataI.Year, oDataI.Month, oDataI.Day, oDataI.Hour, oDataI.Minute, oDataI.Second)
			Dim SerialF As DateTime = New DateTime(oDataF.Year, oDataF.Month, oDataF.Day, oDataF.Hour, oDataF.Minute, oDataF.Second)
			Return SerialF.Second - SerialI.Second
		End Function
	End Class
End Namespace