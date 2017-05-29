Imports lm.ActionDataContract
Imports lm.Comol.Core.Cache
Imports lm.WS.UserAction.Configuration
Imports lm.WS.UserAction.Domain
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common


Namespace lm.Comol.Services.WS.UserAction.Domain
    Public Class OnLineAction

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
        Protected Shared ReadOnly Property OnLineUsers() As List(Of ActionDataContract.UserAction)
            Get
                Dim Prefix As String = String.Format(ServiceUtility.ActionKey, "_", "")
                Dim oList As List(Of ActionDataContract.UserAction) = CacheActions(Of ActionDataContract.UserAction).GetByPrefix(Prefix)

                Dim oCount As Integer = oList.Count
                Dim LastActionUsefullTime As DateTime = Now
                LastActionUsefullTime = LastActionUsefullTime.AddSeconds(-Config.OnLineUserTimeToLive.TotalSeconds)

                Dim oListReturn As List(Of ActionDataContract.UserAction)
                oListReturn = (From o In oList Where o.ActionDate >= LastActionUsefullTime Select o).ToList
                'If IsNothing(oList) Then
                '	oList = New List(Of ActionDataContract.UserAction)
                '	CacheActions(Of List(Of ActionDataContract.UserAction)).SendToCache(oList, ServiceUtility.WebPresenceKey)
                'End If
                Return oListReturn
            End Get
        End Property
#End Region

#Region "Caching"
        Private Shared Function ActionKey(ByVal PersonID As Integer, ByVal WorkingSessionID As System.Guid) As String
            Dim _ActionKey As String = Config.CacheKey(ServiceConfiguration.KeyType.Action)
            Return String.Format(_ActionKey, "_" & PersonID, "_" & WorkingSessionID.ToString)
        End Function
        'Private Shared Function LastActionKey(ByVal PersonID As Integer) As String
        '	Dim _ActionKey As String = Config.CacheKey(ServiceConfiguration.KeyType.LastAction)
        '	Return String.Format(_ActionKey, "_" & PersonID)
        'End Function
#End Region

        Public Shared Sub ClearCacheItems()
            Dim oCache As iCache = ServiceUtility.CurrentCache
            oCache.RemoveByPrefix(String.Format(ServiceUtility.LastActionKey, ""))
            oCache.RemoveByPrefix(String.Format(ServiceUtility.ActionKey, "", ""))
            oCache.RemoveByPrefix(ServiceUtility.WebPresenceKey)
        End Sub

#Region "Presence"
        'Private Shared Sub AddToWebPresence(ByVal oAction As ActionDataContract.UserAction)
        '	Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers
        '	If IsNothing(oList) Then
        '		oList = New List(Of ActionDataContract.UserAction)
        '		oList.Add(oAction)
        '	Else
        '		Dim oTemp As ActionDataContract.UserAction = (From o In oList Where o.WorkingSessionID = oAction.WorkingSessionID And o.PersonID = oAction.PersonID).FirstOrDefault
        '		If IsNothing(oTemp) Then
        '			oList.Add(oAction)
        '		Else
        '			oTemp = oAction
        '		End If
        '	End If
        'End Sub
        'Private Shared Sub RemoveFromWebPresence(ByVal oAction As ActionDataContract.UserAction)
        '	Dim oList As List(Of ActionDataContract.UserAction) = ServiceAction.OnLineUsers

        '	If Not IsNothing(oList) Then
        '		Dim oTemp As ActionDataContract.UserAction = (From o In oList Where o.WorkingSessionID = oAction.WorkingSessionID And o.PersonID = oAction.PersonID).FirstOrDefault
        '		If Not IsNothing(oTemp) Then
        '			oList.Remove(oTemp)
        '		End If
        '	End If
        'End Sub
        'Private Shared Function CreateWebPresence(ByVal oAction As ActionDataContract.UserAction) As WebPresence
        '	Dim oResult As New WebPresence

        '	With oResult
        '		.ClientIPadress = oAction.ClientIPadress
        '		.CommunityID = oAction.CommunityID
        '		.LastDate = oAction.ActionDate
        '		.ModuleID = oAction.ModuleID
        '		.PersonID = oAction.PersonID
        '		.ProxyIPadress = oAction.ProxyIPadress
        '		.WorkingSessionID = oAction.WorkingSessionID
        '		.PersonID = oAction.PersonID
        '		.PersonRoleID = oAction.PersonRoleID
        '	End With
        '	Return oResult
        'End Function

        Public Shared Function SelectedUsersOnline(ByVal oDtoAccess As List(Of dtoAccess)) As List(Of ActionDataContract.UserAction)
            Dim oActionList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers

            Dim oQuery = (From oa In oDtoAccess Group Join oAction In oActionList On oa.WorkingSessionID Equals oAction.WorkingSessionID And oa.ID Equals oAction.PersonID _
              Into children = Group From child In children.DefaultIfEmpty(New ActionDataContract.UserAction() With {.WorkingSessionID = System.Guid.Empty}) Select child)

            Return (From o In oQuery Where o.WorkingSessionID <> System.Guid.Empty).ToList
        End Function
        Public Shared Function SelectedCommunityOnline(ByVal oDtoAccess As List(Of dtoAccess)) As List(Of ActionDataContract.UserAction)
            Dim oActionList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers

            Dim oQuery = (From oa In oDtoAccess Group Join oAction In oActionList On oa.WorkingSessionID Equals oAction.WorkingSessionID And oa.ID Equals oAction.CommunityID _
              Into children = Group From child In children.DefaultIfEmpty(New ActionDataContract.UserAction() With {.WorkingSessionID = System.Guid.Empty}) Select child)

            Return (From o In oQuery Where o.WorkingSessionID <> System.Guid.Empty).ToList
        End Function
        Public Shared Function SelectedModulesOnline(ByVal oDtoAccess As List(Of dtoAccess)) As List(Of ActionDataContract.UserAction)
            Dim oActionList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers

            Dim oQuery = (From oa In oDtoAccess Group Join oAction In oActionList On oa.WorkingSessionID Equals oAction.WorkingSessionID And oa.ID Equals oAction.ModuleID _
              Into children = Group From child In children.DefaultIfEmpty(New ActionDataContract.UserAction() With {.WorkingSessionID = System.Guid.Empty}) Select child)

            Return (From o In oQuery Where o.WorkingSessionID <> System.Guid.Empty).ToList
        End Function

        Public Shared Function UserOnLineCount(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal DistinctValue As Boolean)
            Dim Totale As Integer = 0
            Try
                Dim oList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
                If DistinctValue Then
                    Totale = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o.PersonID Distinct).Count
                Else
                    Totale = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o.PersonID).Count
                End If

            Catch ex As Exception

            End Try

            Return Totale
        End Function

        Public Shared Function PortalUsersOnline(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)
            Return GetUsersOnline(PageSize, PageIndex, -1, -1, Order, Ascending)
        End Function
        Public Shared Function CommunityUserOnline(ByVal CommunityID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)
            Return GetUsersOnline(PageSize, PageIndex, CommunityID, -1, Order, Ascending)
        End Function

        Public Shared Function GetGenericUsersOnline(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)

            Dim oList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
            Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Distinct Select o)

            If PageSize > 0 Then
                oQuery = oQuery.Skip(PageIndex).Take(PageSize)
            End If
            If Order = OrderItemsBy.LastAction Then
                oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
            End If
            If Not Ascending Then
                oQuery = oQuery.Reverse
            End If
            Return oQuery.ToList
        End Function

        Private Shared Function GetUsersOnline(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of ActionDataContract.UserAction)

            Return GetGenericUsersOnline(CommunityID, ModuleID, PageSize, PageIndex, Order, Ascending)
        End Function

        Public Shared Function GetUsersOnlineID(ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
            Dim oList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
            Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o)
            Dim oReturnList As New List(Of dtoAccess)
            If Order = OrderItemsBy.LastAction Then
                oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
                If Not Ascending Then
                    oQuery = oQuery.Reverse
                End If
            End If
            oReturnList = (From o In oQuery Distinct Select New dtoAccess With {.ID = o.PersonID, .WorkingSessionID = o.WorkingSessionID}).ToList
            If IsNothing(oReturnList) Then
                oReturnList = New List(Of dtoAccess)
            End If
            Return oReturnList
        End Function
        Public Shared Function GetCommunityOnlineID(ByVal ModuleID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
            Dim oList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
            Dim oQuery = (From o In oList Where (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o)
            Dim oReturnList As New List(Of dtoAccess)

            If Order = OrderItemsBy.LastAction Then
                oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
                If Not Ascending Then
                    oQuery = oQuery.Reverse
                End If
            End If
            oReturnList = (From o In oQuery Distinct Select New dtoAccess With {.ID = o.CommunityID, .WorkingSessionID = o.WorkingSessionID}).ToList
            If IsNothing(oReturnList) Then
                oReturnList = New List(Of dtoAccess)
            End If
            Return oReturnList
        End Function
        Public Shared Function GetModuleOnlineID(ByVal CommunityID As Integer, ByVal Order As OrderItemsBy, ByVal Ascending As Boolean) As List(Of dtoAccess)
            Dim oList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
            Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) Select o)
            Dim oReturnList As New List(Of dtoAccess)

            If Order = OrderItemsBy.LastAction Then
                oQuery = oQuery.OrderBy(Function(o As ActionDataContract.UserAction) o.ActionDate)
                If Not Ascending Then
                    oQuery = oQuery.Reverse
                End If
            End If

            oReturnList = (From o In oQuery Distinct Select New dtoAccess With {.ID = o.ModuleID, .WorkingSessionID = o.WorkingSessionID}).ToList
            If IsNothing(oReturnList) Then
                oReturnList = New List(Of dtoAccess)
            End If
            Return oReturnList
        End Function


        Public Shared Function GetOnLineActions(ByVal CommunityID As Integer, ByVal ModuleID As Integer) As List(Of OnLineUserAction)

            Dim oList As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
            Dim oQuery = (From o In oList Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) _
                          AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Distinct Select New OnLineUserAction(o, GetFirstActionDate(o.WorkingSessionID, o.CommunityID, o.PersonID, o.ActionDate)))

            'Dim olista As List(Of OnLineUserAction) = oQuery.ToList()
            'For i As Integer = 1 To 20
            '    olista.Add(New OnLineUserAction() With {.AccessDate = olista(0).AccessDate, .ActionDate = olista(0).ActionDate, .ClientIPadress = olista(0).ClientIPadress, .CommunityID = 0, .ID = System.Guid.NewGuid, .ModuleID = olista(0).ModuleID, .PersonID = i, .ProxyIPadress = olista(0).ProxyIPadress, .Type = olista(0).Type, .WorkingSessionID = System.Guid.NewGuid})
            'Next
            'olista = (From o In olista Where (CommunityID < 0 OrElse CommunityID = o.CommunityID) _
            '              AndAlso (ModuleID < 0 OrElse ModuleID = o.ModuleID) Select o).ToList()
            Return oQuery.ToList()
        End Function

        Public Shared Function IsUserOnline(ByVal IdUser As Integer, ByVal IdCommunity As Integer, ByVal workingSessionId As System.Guid) As Boolean
            Dim list As List(Of ActionDataContract.UserAction) = OnLineAction.OnLineUsers
            Return (From u In list Where (IdCommunity = 0 OrElse u.CommunityID = IdCommunity) AndAlso u.PersonID = IdUser AndAlso u.WorkingSessionID = workingSessionId Select u.ID).Any()
        End Function

#End Region

        Public Shared Function GetSeconds(ByVal oDataI As DateTime, ByVal oDataF As DateTime) As Integer
            Dim SerialI As DateTime = New DateTime(oDataI.Year, oDataI.Month, oDataI.Day, oDataI.Hour, oDataI.Minute, oDataI.Second)
            Dim SerialF As DateTime = New DateTime(oDataF.Year, oDataF.Month, oDataF.Day, oDataF.Hour, oDataF.Minute, oDataF.Second)
            Return SerialF.Second - SerialI.Second
        End Function


        Private Shared Function GetFirstActionDate(ByVal UniqueID As System.Guid, ByVal CommunityID As Integer, ByVal PersonId As Integer, ByVal defaultDate As DateTime) As DateTime
            Dim oDatabase As Database = DatabaseFactory.CreateDatabase()

            Using oConnection As DbConnection = oDatabase.CreateConnection
                oConnection.Open()
                Try
                    Dim oCommand As DbCommand = oDatabase.GetStoredProcCommand("sp_CommunityFirstAction")

                    oDatabase.AddInParameter(oCommand, "@CommunityID", DbType.Int32, CommunityID)
                    oDatabase.AddInParameter(oCommand, "@PersonId", DbType.Int32, PersonId)
                    oDatabase.AddInParameter(oCommand, "@uniqueID", DbType.Guid, UniqueID)
                    oDatabase.AddOutParameter(oCommand, "@data", DbType.DateTime, 4)
                    oCommand.Connection = oConnection
                    Dim oResult As Object = oCommand.ExecuteScalar()
                    If IsNothing(oResult) Then
                        Return defaultDate
                    ElseIf Equals(oCommand.Parameters("@data").Value, New DateTime) Then
                        Return defaultDate
                    Else
                        If oCommand.Parameters("@data").Value > defaultDate Then
                            Return defaultDate
                        Else
                            Return oCommand.Parameters("@data").Value
                        End If
                    End If
                Catch ex As Exception
                    Return defaultDate
                End Try
            End Using
            Return defaultDate
        End Function

    End Class
End Namespace