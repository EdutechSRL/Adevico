Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel
Imports NHibernate
Imports System.Linq

Namespace lm.Comol.Modules.UserActions.BusinessLogic
	Public Class ManagerUsageStatistic
		Implements lm.Comol.Core.DomainModel.Common.iDomainManager

#Region "Private property"
		Private Shared WebService As New WSstatistics.WSactionStatisticsSoapClient
		Private _UserContext As iUserContext
        Private _Datacontext As iDataContext
        Private _BaseService As BaseStatisticsService
#End Region

#Region "Public property"
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
        Private ReadOnly Property BaseService() As BaseStatisticsService
            Get
                If IsNothing(_BaseService) Then
                    _BaseService = New BaseStatisticsService(_UserContext, _Datacontext)
                End If
                Return _BaseService
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

        Public Function GetPortalStatistics(ByVal portalName As String, ByVal context As UsageContext, ByVal startDate As Date, ByVal endDate As Date, ByVal pageIndex As Integer, ByRef pager As PagerBase) As dtoStatistic
            Dim items As New List(Of Integer)
            If context.CommunityID >= 0 Then
                items.Add(context.CommunityID)
            Else
                If context.UserID > 0 Then
                    items = Me.BaseService.GetSubscribedCommunityIDList(context.SearchBy, context.UserID)
                    items.AddRange(BaseService.GetCommunitiesWithStatistics(context.SearchBy, context.UserID, startDate, endDate))
                    items = items.Distinct().ToList()
                Else
                    items = Me.BaseService.GetSystemCommunitiesID(context.SearchBy, Me.CurrentUserContext.CurrentUserID, startDate, endDate)
                End If
                If Not items.Contains(0) AndAlso (String.IsNullOrEmpty(context.SearchBy) OrElse portalName.Contains(context.SearchBy)) Then
                    items.Add(0)
                End If
            End If

            Dim statistics As List(Of dtoBaseStatistic) = DALuserActions.GetCommunityStatistics(context, items, startDate, endDate)
            Dim result As New dtoStatistic
            Dim subscribed As New List(Of Integer)
            subscribed.Add(0)
            If Not IsNothing(statistics) Then
                result.Items = New List(Of dtoBaseStatistic)
                Dim communities As New List(Of Community)
                ' TUTTE LE COMUNITA O SOLO QUELLE A CUI SONO ISCRITTO ??
                If Me.CurrentUserContext.CurrentUser.Id = context.UserID Then
                    subscribed.AddRange(Me.BaseService.GetAvailableCommunitiesBySubscription(context.UserID))
                    communities = Me.BaseService.GetCommunitiesById((From o In statistics Where o.ID <> 0 Distinct Select o.ID).ToList)
                ElseIf Me.CurrentUserContext.UserTypeID = UserTypeStandard.Administrative OrElse Me.CurrentUserContext.UserTypeID = UserTypeStandard.Administrator OrElse Me.CurrentUserContext.UserTypeID = UserTypeStandard.SysAdmin Then
                    subscribed.AddRange(Me.BaseService.GetAvailableCommunitiesBySubscription(context.UserID))
                    communities = Me.BaseService.GetCommunitiesById(items.Where(Function(c) c > 0).Distinct().ToList)
                Else
                    subscribed.AddRange(Me.BaseService.GetAvailableCommunitiesBySubscription(context.UserID))
                    communities = Me.BaseService.GetCommunitiesById((From o In statistics Where o.ID <> 0 Distinct Select o.ID).ToList)
                End If
                communities.Add(New Community(0) With {.Name = portalName})

                pager.Count = communities.Count - 1
                pager.PageIndex = pageIndex

                'Dim ot As List(Of Integer)
                'Dim ot2 As List(Of Integer)

                'ot = (From c In oCommunityList Select c.Id Distinct Order By Id).ToList
                'ot2 = (From a In oListActions Select a.CommunityID Distinct Order By CommunityID).ToList

                Dim OrderedList As New List(Of dtoBaseStatistic)
                Dim oquery = (From oCommunity In communities Group Join stat In statistics On oCommunity.Id Equals stat.ID _
                   Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = oCommunity.Id, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = oCommunity.Id, .Subscribed = (context.UserID > 0 AndAlso subscribed.Contains(oCommunity.Id)), .Name = oCommunity.Name, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.Community})
                Select Case context.Order
                    Case StatisticOrder.Community
                        oquery = oquery.OrderBy(Function(o As dtoBaseStatistic) o.Name)
                    Case StatisticOrder.UsageTime
                        oquery = oquery.OrderBy(Function(o As dtoBaseStatistic) o.UsageTime)
                    Case StatisticOrder.AccessNumber
                        oquery = oquery.OrderBy(Function(o As dtoBaseStatistic) o.nAccesses)
                    Case Else
                        oquery = oquery.OrderBy(Function(o As dtoBaseStatistic) o.Name)
                End Select
                If Not context.Ascending Then
                    oquery = oquery.Reverse
                End If
                'OrderedList = oquery.ToList()
                oquery = oquery.Skip(pager.Skip).Take(pager.PageSize)
                OrderedList = oquery.ToList
                If Not IsNothing(OrderedList) Then
                    result.Items.AddRange(OrderedList)
                End If
            End If
            Return result
        End Function
        Public Function GetPortalUsersStatistics(ByVal context As UsageContext, ByRef startDate As Date, ByRef endDate As Date, ByRef pageIndex As Integer, ByRef pager As PagerBase) As dtoStatistic
            Dim oStatistics As New dtoStatistic
            Dim oUserList As List(Of Person) = New List(Of Person)
            Dim statistics As List(Of dtoBaseStatistic)
            Dim OrderedList As New List(Of dtoBaseStatistic)

            If String.IsNullOrEmpty(context.SearchBy) Then
                pager.Count = BaseService.GetSystemPersonCount - 1
            Else
                pager.Count = BaseService.GetSystemPersonCount(context.SearchBy) - 1
            End If

            pager.PageIndex = pageIndex
            If context.Order = StatisticOrder.AccessNumber OrElse context.Order = StatisticOrder.UsageTime Then
                ' Recupero i  dati statistici e poi ordino le persone....
                If String.IsNullOrEmpty(context.SearchBy) Then
                    statistics = DALuserActions.GetPersonStatistics(context, startDate, endDate)
                    oUserList = BaseService.GetSystemPersonList(CurrentUserContext.CurrentUserID)
                Else
                    oUserList = BaseService.GetSystemPersonList(context.SearchBy, pager, context.Order, context.Ascending, True)
                    statistics = DALuserActions.GetPersonStatistics(context, oUserList.Select(Function(p) p.Id).ToList, startDate, endDate)
                End If
                
                Dim oQuery = (From u In oUserList Group Join stat In statistics On u.Id Equals stat.ID _
                Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = u.Id, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = u.Id, .Name = u.SurnameAndName, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.User})

                If context.Order = StatisticOrder.AccessNumber Then
                    oQuery = oQuery.OrderBy(Function(o As dtoBaseStatistic) o.nAccesses)
                Else
                    oQuery = oQuery.OrderBy(Function(o As dtoBaseStatistic) o.UsageTime)
                End If
                If Not context.Ascending Then
                    oQuery = oQuery.Reverse
                End If
                oQuery = oQuery.Skip(pager.Skip).Take(pager.PageSize)
                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    oStatistics.Items.AddRange(OrderedList)
                End If
            Else
                'Recupero le persone e poi recupero i dati statistici...
                If String.IsNullOrEmpty(context.SearchBy) Then
                    oUserList = BaseService.GetSystemPersonList(pager, context.Order, context.Ascending, True)
                Else
                    oUserList = BaseService.GetSystemPersonList(context.SearchBy, pager, context.Order, context.Ascending, True)
                End If

                statistics = DALuserActions.GetPersonStatistics(context, oUserList.Select(Function(p) p.Id).ToList, startDate, endDate)

                Dim oQuery = (From u In oUserList Group Join stat In statistics On u.Id Equals stat.ID _
                Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = u.Id, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = u.Id, .Name = u.SurnameAndName, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.User})

                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    oStatistics.Items.AddRange(OrderedList)
                End If
            End If
            Return oStatistics
        End Function


        Public Function GetCommunityUsersStatistics(ByVal context As UsageContext, ByVal startDate As Date, ByVal endDate As Date, ByVal pageIndex As Integer, ByVal pager As PagerBase) As dtoStatistic
            Dim result As New dtoStatistic
            Dim users As List(Of Person) = New List(Of Person)
            Dim statistics As List(Of dtoBaseStatistic)
            Dim OrderedList As New List(Of dtoBaseStatistic)

            If context.CommunityID > 0 Then
                pager.Count = BaseService.GetSubscriptionsCount(context.SearchBy, context.CommunityID, SubscriptionStatus.all) - 1
            ElseIf String.IsNullOrEmpty(context.SearchBy) Then
                pager.Count = BaseService.GetSystemPersonCount - 1
            Else
                pager.Count = BaseService.GetSystemPersonCount(context.SearchBy) - 1
            End If

            pager.PageIndex = pageIndex
            If context.Order = StatisticOrder.AccessNumber OrElse context.Order = StatisticOrder.UsageTime Then
                ' Recupero i  dati statistici e poi ordino le persone....

                If String.IsNullOrEmpty(context.SearchBy) Then
                    statistics = DALuserActions.GetPersonStatistics(context, startDate, endDate)
                    users = BaseService.GetCommunityPersonList(context.CommunityID, SubscriptionStatus.all)
                Else
                    users = BaseService.GetCommunityPersonList(context.SearchBy, context.CommunityID, SubscriptionStatus.all)
                    statistics = DALuserActions.GetPersonStatistics(context, users.Select(Function(p) p.Id).ToList, startDate, endDate)
                End If

                'statistics = DALuserActions.GetPersonStatistics(context, startDate, endDate)
                'users = BaseService.GetCommunityPersonList(context.CommunityID, SubscriptionStatus.all)

                Dim oQuery = (From u In users Group Join stat In statistics On u.Id Equals stat.ID _
                Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = u.Id, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = u.Id, .Name = u.SurnameAndName, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.User})

                If context.Order = StatisticOrder.AccessNumber Then
                    oQuery = oQuery.OrderBy(Function(o As dtoBaseStatistic) o.nAccesses)
                Else
                    oQuery = oQuery.OrderBy(Function(o As dtoBaseStatistic) o.UsageTime)
                End If
                If Not context.Ascending Then
                    oQuery = oQuery.Reverse
                End If
                oQuery = oQuery.Skip(pager.Skip).Take(pager.PageSize)
                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    result.Items.AddRange(OrderedList)
                End If
            Else
                'Recupero le persone e poi recupero i dati statistici...
                If context.CommunityID > 0 Then
                    users = BaseService.GetCommunityPersonList(context.SearchBy, pager, context.Order, context.Ascending, context.CommunityID, SubscriptionStatus.all)
                Else
                    users = BaseService.GetSystemPersonList(context.SearchBy, pager, context.Order, context.Ascending, True)
                End If
                statistics = DALuserActions.GetPersonStatistics(context, users.Select(Function(p) p.Id).ToList(), startDate, endDate)

                Dim oQuery = (From u In users Group Join stat In statistics On u.Id Equals stat.ID _
                Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = u.Id, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = u.Id, .Name = u.SurnameAndName, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.User})

                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    result.Items.AddRange(OrderedList)
                End If
            End If
            Return result
        End Function

        Public Function GetUserModuleStatistics(ByVal context As UsageContext, ByVal startDate As Date, ByVal endDate As Date, ByVal pageIndex As Integer, ByVal pager As PagerBase) As dtoStatistic
            Return GetUserModuleStatistics(Nothing, context, startDate, endDate, pageIndex, pager)
        End Function
        Public Function GetUserModuleStatistics(ByVal p As ModuleStatistics, ByVal context As UsageContext, ByVal startDate As Date, ByVal endDate As Date, ByVal pageIndex As Integer, ByVal pager As PagerBase) As dtoStatistic
            Dim oStatistics As New dtoStatistic

            Dim oModuleList As List(Of COL_BusinessLogic_v2.PlainService) = Nothing
            Dim idRole As Integer = -1
            Dim oSubscription As Subscription = BaseService.GetSubscription(context.CommunityID, context.UserID)
            If Not IsNothing(oSubscription) Then
                idRole = oSubscription.Role.Id
            Else
                idRole = BaseService.GetDefaultIdRole(context.CommunityID)
            End If
            'If oUsageContext.CommunityID > 0 And oUsageContext.UserID > 0 Then
            '	RoleID = COL_BusinessLogic_v2.Main.TipoRuoloStandard.AdminComunità
            '	' DA RECUPERARE VIA NHIBERNATE !!!

            'End If
            Dim statistics As List(Of dtoBaseStatistic)
            Dim OrderedList As New List(Of dtoBaseStatistic)
            Dim allModules As List(Of COL_BusinessLogic_v2.PlainService) = BaseService.GetGenericModuleList(-1, -1)

            If context.CommunityID > 0 Then
                oModuleList = BaseService.GetGenericModuleList(context.CommunityID, idRole)
                '    If IsNothing(p) OrElse (Not p.Administration AndAlso (p.IsForHistory OrElse (Not p.ListOtherStatistic AndAlso CurrentUserContext.CurrentUserID <> context.UserID) OrElse (Not p.ListSelfStatistic AndAlso CurrentUserContext.CurrentUserID = context.UserID))) Then
                If context.UserID > 0 Then
                    oModuleList = oModuleList.Where(Function(m) m.Permessi.Contains("1")).ToList
                End If

                'End If
                Dim idModules As List(Of Integer) = BaseService.GetModulesIdWithStatistics(context.UserID, context.CommunityID, startDate, endDate)
                For Each idModule As Integer In idModules.Where(Function(m) Not oModuleList.Select(Function(o) o.ID).Contains(m)).ToList()
                    oModuleList.AddRange(allModules.Where(Function(m) m.ID = idModule).ToList())
                Next
            Else
                oModuleList = BaseService.GetCommunityModuleList(-1)
            End If

            pager.Count = oModuleList.Count - 1

            pager.PageIndex = pageIndex
            If context.Order = StatisticOrder.AccessNumber OrElse context.Order = StatisticOrder.UsageTime Then
                ' Recupero i  dati statistici e poi ordino i moduli....
                statistics = DALuserActions.GetModuleStatistics(context, startDate, endDate)

                Dim oQuery = (From m In oModuleList Group Join stat In statistics On m.ID Equals stat.ID _
                Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = m.ID, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = m.ID, .Name = m.Name, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.User})

                If context.Order = StatisticOrder.AccessNumber Then
                    oQuery = oQuery.OrderBy(Function(o As dtoBaseStatistic) o.nAccesses)
                Else
                    oQuery = oQuery.OrderBy(Function(o As dtoBaseStatistic) o.UsageTime)
                End If
                If Not context.Ascending Then
                    oQuery = oQuery.Reverse
                End If
                oQuery = oQuery.Skip(pager.Skip).Take(pager.PageSize)
                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    oStatistics.Items.AddRange(OrderedList)
                End If
            Else
                Dim modules As List(Of COL_BusinessLogic_v2.PlainService)
                If context.Ascending Then
                    modules = (From o In oModuleList Order By o.Name).Skip(pager.Skip).Take(pager.PageSize).ToList
                Else
                    modules = (From o In oModuleList Order By o.Name Descending).Skip(pager.Skip).Take(pager.PageSize).ToList
                End If

                statistics = DALuserActions.GetModuleStatistics(context, modules.Select(Function(m) m.ID).Distinct.ToList, startDate, endDate)

                Dim oQuery = (From m In modules Group Join stat In statistics On m.ID Equals stat.ID _
                Into children = Group From child In children.DefaultIfEmpty(New dtoBaseStatistic() With {.ID = m.ID, .nAccesses = 0, .UsageTime = 0}) Select _
                 New dtoBaseStatistic() With {.ID = m.ID, .Name = m.Name, .nAccesses = child.nAccesses, .UsageTime = child.UsageTime, .Type = StatisticType.User})

                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    oStatistics.Items.AddRange(OrderedList)
                End If
            End If
            Return oStatistics
        End Function

        Public Function GetSummary(ByVal usageContext As UsageContext, ByVal startDate As Date, ByVal endDate As Date) As dtoSummary
            Dim odtoSummary As dtoSummary = DALuserActions.GetSummary(usageContext, startDate, endDate)


            Dim community As Community = BaseService.GetCommunity(usageContext.CommunityID)
            Dim person As Person = BaseService.GetPerson(usageContext.UserID)
            Dim oModule As COL_BusinessLogic_v2.PlainService = (From o In Me.BaseService.GetSystemModuleList Where o.ID = usageContext.ModuleID).FirstOrDefault

            If IsNothing(community) Then
                odtoSummary.CommunityName = "       --      "
            Else
                odtoSummary.CommunityName = community.Name
            End If
            If IsNothing(person) Then
                odtoSummary.Owner = "       --      "
            Else
                odtoSummary.Owner = person.SurnameAndName
            End If
            If Not IsNothing(oModule) Then
                odtoSummary.ModuleName = oModule.Name
            End If

            Return odtoSummary
        End Function

        Public Function GetTralnsatedModules() As List(Of COL_BusinessLogic_v2.PlainService)
            Return BaseService.GetGenericModuleList(-1, -1)
        End Function

	End Class
End Namespace