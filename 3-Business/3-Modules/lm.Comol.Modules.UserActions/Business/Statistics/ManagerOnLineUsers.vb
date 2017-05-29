Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
Imports NHibernate
Imports lm.Comol.Modules.UsageResults.BusinessLogic

Namespace lm.Comol.Modules.UserActions.BusinessLogic
    Public Class ManagerOnLineUsers
        Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
        Private _WebPresence As WS_OnLine.UserOnlineSoapClient
        Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
        Private _Common As BaseStatisticsService
#End Region

#Region "Public property"
        Private ReadOnly Property WebPresence() As WS_OnLine.UserOnlineSoapClient
            Get
                If IsNothing(_WebPresence) Then
                    _WebPresence = New WS_OnLine.UserOnlineSoapClient
                End If
                Return _WebPresence
            End Get
        End Property

        Private ReadOnly Property DC() As iDataContext
            Get
                Return _Datacontext
            End Get
        End Property
        Private ReadOnly Property CurrentUserContext() As iUserContext
            Get
                Return _UserContext
            End Get
        End Property
        Private ReadOnly Property Common() As BaseStatisticsService
            Get
                If IsNothing(_Common) Then
                    _Common = New BaseStatisticsService(_UserContext, _Datacontext)
                End If
                Return _Common
            End Get
        End Property
#End Region

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            Me._UserContext = oContext.UserContext
            Me._Datacontext = oContext.DataContext
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            Me._UserContext = oUserContext
            Me._Datacontext = oDatacontext
        End Sub


        Public Function RetrieveOnLineUsers(ByRef oContext As OnLineUsersContext, ByRef CurrentPage As Integer, ByRef oPager As PagerBase, ByRef oSummary As dtoSummary, ByVal DetailsOptions As Details, ByVal odtoTranslatedContext As dtoTranslatedContext) As List(Of dtoOnLineUser)
            Dim oOnLineUsers As New List(Of dtoOnLineUser)

            Dim cacheKey As String = CachePolicy.FindCommunityUsersOnLine(oContext.CommunityID)
            Dim oOnLineActions As New List(Of WS_OnLine.OnLineUserAction)
            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                oContext.LastUpdate = Now
                oOnLineActions = WebPresence.GetOnLineActions(oContext.CommunityID, oContext.ModuleID)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, oOnLineActions, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30secondi)
                DisposeWebPresence()
            Else
                oOnLineActions = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of WS_OnLine.OnLineUserAction))
            End If



            Dim LastUpdate As DateTime = oContext.LastUpdate
            Dim NameSurnameFilter As String = oContext.NameSurnameFilter
            oOnLineUsers = (From o In oOnLineActions Select New dtoOnLineUser(o, LastActionDiff(LastUpdate, o.ActionDate))).ToList
            oOnLineUsers = Me.FromIDtoCommunityName(oOnLineUsers, DetailsOptions, odtoTranslatedContext)
            oOnLineUsers = Me.FromIDtoModuleName(oOnLineUsers, DetailsOptions, odtoTranslatedContext)
            oOnLineUsers = Me.FromIDtoPerson(oOnLineUsers, DetailsOptions, odtoTranslatedContext)
            Dim oQuery = (From o In oOnLineUsers Select o)

            If oContext.NameSurnameFilter <> "" Then
                NameSurnameFilter = NameSurnameFilter.ToLower
                oQuery = oQuery.Where(Function(c) c.Owner.ToLower.Contains(NameSurnameFilter) = True)
            End If
            Select Case oContext.Order
                Case StatisticOrder.Community
                    oQuery = oQuery.OrderBy(Function(c) c.CommunityName)
                Case StatisticOrder.LastAction
                    oQuery = oQuery.OrderBy(Function(c) c.LastAction)
                Case StatisticOrder.ModuleName
                    oQuery = oQuery.OrderBy(Function(c) c.ModuleName)
                Case StatisticOrder.Owner
                    oQuery = oQuery.OrderBy(Function(c) c.Owner)
                Case StatisticOrder.FirstAction
                    oQuery = oQuery.OrderBy(Function(c) c.FirstAction)
                Case Else
                    oQuery = oQuery.OrderBy(Function(c) c.LastAction)
            End Select

            If Not oContext.Ascending Then
                oQuery = oQuery.Reverse
            End If

            oPager.Count = oQuery.Count - 1
            oPager.PageIndex = CurrentPage


            oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            oSummary = GetSummary(oContext, (From o In oOnLineActions Select o.WorkingSessionID Distinct).Count(), (From o In oOnLineActions Select o.PersonID Distinct).Count())
            oOnLineUsers = oQuery.ToList()

            Return oOnLineUsers
        End Function

        Private Function LastActionDiff(ByVal LastUpdate As DateTime, ByVal ActionDate As DateTime) As TimeSpan
            If LastUpdate <= ActionDate Then
                Return New TimeSpan(0, 0, 1)
            Else
                Return (LastUpdate - ActionDate)
            End If
        End Function

        'Public Function GetOnLineUsers(ByVal oContext As UsageContext, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoOnLineUser)
        '    Dim oOnLineUser As New List(Of dtoOnLineUser)

        '    oPager.Count = WebPresence.WorkingSessionOnlineCount(oContext.CommunityID, oContext.ModuleID) - 1
        '    oPager.PageIndex = CurrentPage
        '    Select Case oContext.Order
        '        Case StatisticOrder.Community
        '            oOnLineUser = Me.GetOnLineUserByCommunity(oContext, oContext.Ascending, oPager.PageIndex, oPager.PageSize, oPager.Skip)
        '        Case StatisticOrder.LastAction
        '            oOnLineUser = Me.GetOnLineUserByLastAction(oContext, oContext.Ascending, oPager.PageIndex, oPager.PageSize, oPager.Skip)
        '        Case StatisticOrder.ModuleName
        '            oOnLineUser = Me.GetOnLineUserByModule(oContext, oContext.Ascending, oPager.PageIndex, oPager.PageSize, oPager.Skip)
        '        Case StatisticOrder.Owner
        '            oOnLineUser = Me.GetOnLineUserByUserName(oContext, oContext.Ascending, oPager.PageIndex, oPager.PageSize, oPager.Skip)
        '        Case Else
        '            oOnLineUser = Me.GetOnLineUserByLastAction(oContext, oContext.Ascending, oPager.PageIndex, oPager.PageSize, oPager.Skip)
        '    End Select
        '    Return oOnLineUser
        'End Function

        'Private Function GetOnLineUserByModule(ByVal oContext As UsageContext, ByRef Ascending As Boolean, ByRef PageIndex As Integer, ByRef PageSize As Integer, ByRef PageSkip As Integer) As List(Of dtoOnLineUser)
        '    Dim oRemoteIDList As WS_OnLine.ArrayOfDtoAccess
        '    Dim oActionList As List(Of WS_OnLine.UserAction)
        '    Dim oRemoteSelectedID As New WS_OnLine.ArrayOfDtoAccess


        '    Dim oModuleList As List(Of COL_BusinessLogic_v2.PlainService) = Nothing
        '    oModuleList = Common.GetGenericModuleList(oContext.CommunityID, -1)

        '    oRemoteIDList = WebPresence.GetUsersOnlineModulesID(oContext.CommunityID, WS_OnLine.OrderItemsBy.ModuleName, oContext.Ascending)

        '    Dim oQuery = (From rm In oRemoteIDList Group Join m In oModuleList On rm.ID Equals m.ID _
        '      Into children = Group From child In children.DefaultIfEmpty(New COL_BusinessLogic_v2.PlainService With {.ID = rm.ID, .Name = ""}) Order By child.Name Select rm)
        '    If Not Ascending Then
        '        oQuery = oQuery.Reverse
        '    End If
        '    oQuery = oQuery.Skip(PageSkip).Take(PageSize)

        '    oRemoteSelectedID.AddRange(oQuery.AsEnumerable)
        '    oActionList = WebPresence.GetSelectedUsersOnline(oRemoteSelectedID, WS_OnLine.OrderItemsBy.ModuleName)

        '    Dim oOnLineUser As New List(Of dtoOnLineUser)
        '    oOnLineUser = (From o In oActionList Select New dtoOnLineUser() With {.ActionType = o.Type, .CommunityID = o.CommunityID, .LastAction = o.ActionDate, .ModuleID = o.ModuleID, .OwnerID = o.PersonID}).ToList
        '    oOnLineUser = Me.FromIDtoCommunityName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoModuleName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoPerson(oOnLineUser)
        '    Return oOnLineUser
        'End Function
        'Private Function GetOnLineUserByUserName(ByVal oContext As UsageContext, ByRef Ascending As Boolean, ByRef PageIndex As Integer, ByRef PageSize As Integer, ByRef PageSkip As Integer) As List(Of dtoOnLineUser)
        '    Dim oRemoteIDList As WS_OnLine.ArrayOfDtoAccess
        '    Dim oActionList As List(Of WS_OnLine.UserAction)
        '    Dim oRemoteSelectedID As New WS_OnLine.ArrayOfDtoAccess


        '    oRemoteIDList = WebPresence.GetUsersOnlineID(oContext.CommunityID, oContext.ModuleID, WS_OnLine.OrderItemsBy.UserName, oContext.Ascending)
        '    Dim oPersonList As List(Of iPerson) = Common.GetPersonsByList((From o In oRemoteIDList Select o.ID Distinct).ToList)

        '    Dim oQuery = (From rm In oRemoteIDList Group Join m In oPersonList On rm.ID Equals m.Id _
        '    Into children = Group From child In children.DefaultIfEmpty(New Person With {.Id = rm.ID, .Name = "", .Surname = ""}) Order By child.SurnameAndName Select rm)
        '    If Not Ascending Then
        '        oQuery = oQuery.Reverse
        '    End If
        '    oQuery = oQuery.Skip(PageSkip).Take(PageSize)

        '    oRemoteSelectedID.AddRange(oQuery.AsEnumerable)
        '    oActionList = WebPresence.GetSelectedUsersOnline(oRemoteSelectedID, WS_OnLine.OrderItemsBy.UserName)


        '    Dim oOnLineUser As New List(Of dtoOnLineUser)
        '    oOnLineUser = (From o In oActionList Select New dtoOnLineUser() With {.ActionType = o.Type, .CommunityID = o.CommunityID, .LastAction = o.ActionDate, .ModuleID = o.ModuleID, .OwnerID = o.PersonID, .ClientIP = o.ClientIPadress, .ProxyIP = o.ProxyIPadress, .WorkingSessionID = o.WorkingSessionID}).ToList
        '    oOnLineUser = Me.FromIDtoCommunityName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoModuleName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoPerson(oOnLineUser)
        '    Return oOnLineUser
        'End Function
        'Private Function GetOnLineUserByCommunity(ByVal oContext As UsageContext, ByRef Ascending As Boolean, ByRef PageIndex As Integer, ByRef PageSize As Integer, ByRef PageSkip As Integer) As List(Of dtoOnLineUser)
        '    Dim oRemoteIDList As WS_OnLine.ArrayOfDtoAccess
        '    Dim oActionList As List(Of WS_OnLine.UserAction)
        '    Dim oRemoteSelectedID As New WS_OnLine.ArrayOfDtoAccess

        '    oRemoteIDList = WebPresence.GetUsersOnlineCommunitiesID(oContext.ModuleID, WS_OnLine.OrderItemsBy.CommunityName, oContext.Ascending)
        '    Dim oCommunityList As List(Of Community) = Common.GetCommunitiesList((From o In oRemoteIDList Select o.ID Distinct).ToList)

        '    Dim oQuery = (From rm In oRemoteIDList Group Join m In oCommunityList On rm.ID Equals m.Id _
        '    Into children = Group From child In children.DefaultIfEmpty(New Community With {.Id = rm.ID, .Name = ""}) Order By child.Name Select rm)
        '    If Not Ascending Then
        '        oQuery = oQuery.Reverse
        '    End If
        '    oQuery = oQuery.Skip(PageSkip).Take(PageSize)
        '    oRemoteSelectedID.AddRange(oQuery.AsEnumerable)

        '    oActionList = WebPresence.GetSelectedUsersOnline(oRemoteSelectedID, WS_OnLine.OrderItemsBy.CommunityName)


        '    Dim oOnLineUser As New List(Of dtoOnLineUser)
        '    oOnLineUser = (From o In oActionList Select New dtoOnLineUser() With {.ActionType = o.Type, .CommunityID = o.CommunityID, .LastAction = o.ActionDate, .ModuleID = o.ModuleID, .OwnerID = o.PersonID, .ClientIP = o.ClientIPadress, .ProxyIP = o.ProxyIPadress, .WorkingSessionID = o.WorkingSessionID}).ToList
        '    oOnLineUser = Me.FromIDtoCommunityName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoModuleName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoPerson(oOnLineUser)
        '    Return oOnLineUser
        'End Function
        'Private Function GetOnLineUserByLastAction(ByVal oContext As UsageContext, ByRef Ascending As Boolean, ByRef PageIndex As Integer, ByRef PageSize As Integer, ByRef PageSkip As Integer) As List(Of dtoOnLineUser)
        '    Dim oActionList As List(Of WS_OnLine.UserAction)

        '    oActionList = WebPresence.GetGenericUsersOnline(oContext.CommunityID, oContext.ModuleID, PageSize, PageIndex, WS_OnLine.OrderItemsBy.LastAction, oContext.Ascending)


        '    Dim oOnLineUser As New List(Of dtoOnLineUser)
        '    oOnLineUser = (From o In oActionList Select New dtoOnLineUser() With {.ActionType = o.Type, .CommunityID = o.CommunityID, .LastAction = o.ActionDate, .ModuleID = o.ModuleID, .OwnerID = o.PersonID, .ClientIP = o.ClientIPadress, .ProxyIP = o.ProxyIPadress, .WorkingSessionID = o.WorkingSessionID}).ToList
        '    oOnLineUser = Me.FromIDtoCommunityName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoModuleName(oOnLineUser)
        '    oOnLineUser = Me.FromIDtoPerson(oOnLineUser)
        '    Return oOnLineUser
        'End Function

        Private Function FromIDtoModuleName(ByVal oList As List(Of dtoOnLineUser), ByVal DetailsOptions As Details, ByVal odtoTranslatedContext As dtoTranslatedContext) As List(Of dtoOnLineUser)
            If DetailsOptions And Details.NoModuleName Then
                For Each o As dtoOnLineUser In oList
                    o.ModuleName = odtoTranslatedContext.GenericModuleName
                Next
            Else
                Dim oModuleList As List(Of COL_BusinessLogic_v2.PlainService) = Nothing
                oModuleList = Common.GetGenericModuleList(-1, -1)

                For Each o As dtoOnLineUser In oList
                    Dim oDto As dtoOnLineUser = o

                    o.ModuleName = (From m In oModuleList Where m.ID = oDto.ModuleID Select m.Name).FirstOrDefault
                Next
            End If

            Return oList
        End Function
        Private Function FromIDtoCommunityName(ByVal oList As List(Of dtoOnLineUser), ByVal DetailsOptions As Details, ByVal odtoTranslatedContext As dtoTranslatedContext) As List(Of dtoOnLineUser)
            If DetailsOptions And Details.NoCommunityName Then
                For Each o As dtoOnLineUser In oList
                    If o.CommunityID = 0 Then
                        o.CommunityName = odtoTranslatedContext.PortalName
                    Else
                        o.CommunityName = odtoTranslatedContext.GenericCommunityName
                    End If
                Next
            Else
                If oList.Count > 0 Then
                    Dim oCommunityList As List(Of Community) = Common.GetCommunitiesById((From o In oList Select o.CommunityID Distinct).ToList)
                    Dim oHome As Community = (From c In oCommunityList Where c.Id = 0 Select c).FirstOrDefault
                    If IsNothing(oHome) Then
                        oCommunityList.Add(New Community(0) With {.Name = "Home"})
                    Else
                        oHome.Name = "Home"
                    End If

                    For Each o As dtoOnLineUser In oList
                        Dim oDto As dtoOnLineUser = o
                        o.CommunityName = (From c In oCommunityList Where c.Id = oDto.CommunityID Select c.Name).FirstOrDefault
                    Next
                Else
                    Return oList
                End If
            End If
           
            Return oList
        End Function
        Private Function FromIDtoPerson(ByVal oList As List(Of dtoOnLineUser), ByVal DetailsOptions As Details, ByVal odtoTranslatedContext As dtoTranslatedContext) As List(Of dtoOnLineUser)
            If oList.Count > 0 Then
                Dim CurrentID As Integer = Me.CurrentUserContext.CurrentUser.Id
                Dim oPersonList As List(Of iPerson) = Common.GetPersonsByList((From o In oList Select o.OwnerID Distinct).ToList)
                For Each o As dtoOnLineUser In oList
                    Dim oDto As dtoOnLineUser = o
                    Dim oPerson As Person = (From p In oPersonList Where p.Id = oDto.OwnerID Select p).FirstOrDefault

                    If IsNothing(oPerson) Then
                        o.Owner = " -- -- "
                    Else
                        If DetailsOptions And Details.NoUserName Then
                            If Not IsNothing(oPerson) AndAlso oPerson.Id = CurrentID Then
                                o.Owner = oPerson.SurnameAndName
                            Else
                                o.Owner = String.Format(odtoTranslatedContext.GenericPerson, oPerson.Surname.Chars(0), oPerson.Name.Chars(0))
                            End If
                        Else
                            o.Owner = oPerson.SurnameAndName
                        End If
                    End If
                Next
            Else
                Return oList
            End If
            Return oList
        End Function

        Public Function GetSummary(ByVal oContext As OnLineUsersContext, ByVal WorkingSession As Integer, ByVal UserOnlineCount As Integer) As dtoSummary
            Dim oDtoSummary As New dtoSummary

            'SUMMARY

            Dim oCommunity As Community = Common.GetCommunity(oContext.CommunityID)
            Dim oPerson As Person = Common.GetPerson(oContext.UserID)
            Dim oModule As COL_BusinessLogic_v2.PlainService = (From o In Me.Common.GetSystemModuleList Where o.ID = oContext.ModuleID).FirstOrDefault

            oDtoSummary.CommunityName = oCommunity.Name
            oDtoSummary.Owner = oPerson.SurnameAndName
            If Not IsNothing(oModule) Then
                oDtoSummary.ModuleName = oModule.Name
            End If
            oDtoSummary.UsageTime = WorkingSession
            oDtoSummary.nAccesses = UserOnlineCount
            Return oDtoSummary
        End Function
        Public Function GetWorkingSessionOnlineCount(Optional ByVal CommunityID = -1) As Integer
            Try
                Dim SessionCount As Integer = WebPresence.WorkingSessionOnlineCount(CommunityID, -1)
                DisposeWebPresence()
                Return SessionCount
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Function GetUserOnlineCount(Optional ByVal CommunityID = -1) As Integer
            Try
                Dim OnlineCount As Integer = WebPresence.UserOnlineCount(CommunityID, -1)
                DisposeWebPresence()
                Return OnlineCount
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Private Sub DisposeWebPresence()
            If Not IsNothing(_WebPresence) Then
                Dim service As System.ServiceModel.ClientBase(Of WS_OnLine.UserOnlineSoap) = DirectCast(_WebPresence, System.ServiceModel.ClientBase(Of WS_OnLine.UserOnlineSoap))
                If service.State <> System.ServiceModel.CommunicationState.Closing AndAlso service.State <> System.ServiceModel.CommunicationState.Closed Then
                    service.Close()
                    service = Nothing
                End If
            End If
        End Sub
        <Flags()> Enum Details
            None = 0
            NoUserName = 1
            NoCommunityName = 2
            NoModuleName = 4
            NoActionName = 8
        End Enum
    End Class
End Namespace