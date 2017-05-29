Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel
Imports NHibernate
Imports lm.Comol.Modules.UserActions.BusinessLogic
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Core.DomainModel.Common

Namespace lm.Comol.Modules.UsageResults.BusinessLogic
    Public Class ManagerUsageResults
        Implements iDomainManager

#Region "Private property"
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


        Public Function FindUserCommunities(ByVal oContext As ResultContext, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)

            Dim statistics As New List(Of dtoUserResult)
            Dim cacheKey As String = CachePolicy.FindUserCommunities(oContext.UserID)

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                'oRemoteCommunityList = WebService.FindCommunitiesWithAccessResult(oContext.UserID)
                statistics = DALuserResults.GetCommunitiesWithAccessResult(oContext.UserID)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoUserResult))
            End If

            If Not IsNothing(statistics) AndAlso statistics.Count > 0 Then
                Dim oCommunityList As New List(Of Community)
                oCommunityList = Me.BaseService.GetCommunitiesById((From o In statistics Where o.IdCommunity <> 0 Distinct Select o.IdCommunity).ToList)
                oPager.Count = oCommunityList.Count - 1
                oPager.PageIndex = CurrentPage

                Dim oQuery = (From c In oCommunityList Select New dtoAccessResult() With {.PersonID = oContext.UserID, .CommunityID = c.Id, .CommunityName = c.Name})
                Select Case oContext.Order
                    Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Community
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.CommunityName)
                End Select
                If Not oContext.Ascending Then
                    oQuery = oQuery.Reverse
                End If
                oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)

                Dim OrderedList As New List(Of dtoAccessResult)
                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    oList.AddRange(OrderedList)
                End If
            End If
            Return oList
        End Function
        Public Function FindPortalUsers(ByVal oContext As ResultContext, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)
            Dim oUserList As List(Of Person) = New List(Of Person)

            Dim statistics As New List(Of dtoUserResult)
            Dim cacheKey As String = CachePolicy.FindPortalUsers

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                'oRemoteUserList = WebService.FindPortalUsersWithAccessResult()
                statistics = DALuserResults.GetPortalUsersWithAccessResult()
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoUserResult))
            End If

            Dim OrderedList As New List(Of dtoAccessResult)

            oPager.Count = statistics.Count - 1
            oPager.PageIndex = CurrentPage

            oUserList = BaseService.GetSystemPersonList(CurrentUserContext.CurrentUserID)
            Dim oQuery = (From u In oUserList Join oRemote In statistics On u.Id Equals oRemote.IdPerson _
            Select New dtoAccessResult() With {.PersonID = u.Id, .PersonName = u.SurnameAndName})

            Select Case oContext.Order
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner
                    oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.PersonName)
            End Select
            If Not oContext.Ascending Then
                oQuery = oQuery.Reverse
            End If
            oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If

            Return oList
        End Function
        Public Function FindCommunityUsers(ByVal oContext As ResultContext, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)


            Dim oUserList As List(Of Person) = New List(Of Person)
            Dim statistics As New List(Of dtoUserResult)
            Dim cacheKey As String = CachePolicy.FindCommunityUsers(oContext.CommunityID)

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                ' oRemoteUserList = WebService.FindUsersWithAccessResult(oContext.CommunityID)
                statistics = DALuserResults.GetUsersWithAccessResult(oContext.CommunityID)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoUserResult))
            End If


            Dim OrderedList As New List(Of dtoAccessResult)

            If oContext.CommunityID > 0 Then
                oPager.Count = BaseService.GetSubscriptionsCount(oContext.CommunityID, SubscriptionStatus.all) - 1
            Else
                oPager.Count = BaseService.GetSystemPersonCount - 1
            End If

            oPager.PageIndex = CurrentPage


            If oContext.Order <> lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner Then
                oUserList = BaseService.GetCommunityPersonList(oContext.CommunityID, SubscriptionStatus.all)
            Else
                If oContext.CommunityID > 0 Then
                    oUserList = BaseService.GetCommunityPersonList("", oPager, UserActions.DomainModel.StatisticOrder.Owner, oContext.Ascending, oContext.CommunityID, SubscriptionStatus.all)
                Else
                    oUserList = BaseService.GetSystemPersonList(oPager, UserActions.DomainModel.StatisticOrder.Owner, oContext.Ascending, False)
                End If
            End If

            Dim oQuery = (From u In oUserList Group Join oRemote In statistics On u.Id Equals oRemote.IdPerson _
                Into children = Group From child In children.DefaultIfEmpty(New dtoUserResult() With {.IdCommunity = oContext.CommunityID, .IdPerson = u.Id, .UsageTime = 0}) Select _
                 New dtoAccessResult() With {.PersonID = u.Id, .PersonName = u.SurnameAndName, .UsageTime = child.UsageTime})

            Select Case oContext.Order
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.UsageTime
                    oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.UsageTime)
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner
                    oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.PersonName)
                Case Else
                    oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.PersonName)
            End Select
            If Not oContext.Ascending Then
                oQuery = oQuery.Reverse
            End If
            oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function


        Public Function FindUsersBetweenDate(ByVal oContext As ResultContext, ByVal startDate As Date, ByVal endDate As Date, ByVal CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)

            Dim communityName As String = ""
            Dim community As iCommunity = BaseService.GetCommunity(oContext.CommunityID)
            If Not IsNothing(community) Then
                communityName = community.Id
            End If


            'Dim oRemoteResultsList As New List(Of SRV_accessMonitor.AccessResult)
            Dim statistics As List(Of dtoAccessResult)
            Dim cacheKey As String = ""
            If oContext.CommunityID > 0 Then
                cacheKey = CachePolicy.FindCommunityUsersResultsBetweenDate(CurrentUserContext.CurrentUser.Id, oContext.CommunityID, startDate.ToString, endDate.ToString)
            Else : cacheKey = CachePolicy.FindPortalUsersResultsBetweenDate(CurrentUserContext.CurrentUser.Id, startDate.ToString, endDate.ToString)

            End If

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                'If oContext.CommunityID > 0 Then
                '    oRemoteResultsList = WebService.GetCommunityUsersAccessResultBetweenDate(oFromDate, oToDate, oContext.CommunityID)
                'Else
                '    oRemoteResultsList = WebService.GetPortalUsersAccessResultBetweenDate(oFromDate, oToDate)
                'End If
                statistics = DALuserResults.GetAccessResultsBetweenDate(oContext, startDate, endDate)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
            End If

            Dim OrderedList As New List(Of dtoAccessResult)

            oPager.Count = statistics.Count - 1
            oPager.PageIndex = CurrentPage

            Dim oPersonsId As List(Of Integer) = (From stat In statistics Select stat.PersonID).Distinct.ToList()
            Dim oPersonList As List(Of iPerson) = BaseService.GetPersonsByList(oPersonsId)


            Dim oQuery = (From c In statistics Group Join p In oPersonList On c.PersonID Equals p.Id _
                            Into children = Group From child In children.DefaultIfEmpty(New Person With {.Id = c.PersonID, .Name = "", .Surname = ""}) _
            Select dtoAccessResult.Create(c, child.SurnameAndName, communityName))


            oQuery = GetQueryOrdered(oContext.Order, oContext.Ascending, oQuery)
            oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function

        'Public Function GetUsageResults(ByVal context As ContextBase, ByVal startDate As Date, ByVal endDate As Date, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
        '    Dim oList As New List(Of dtoAccessResult)

        '    Dim userName As String = ""
        '    Dim communityName As String = ""
        '    Dim community As iCommunity = BaseService.GetCommunity(context.CommunityID)
        '    Dim person As iPerson = BaseService.GetPerson(context.UserID)
        '    If Not IsNothing(community) Then
        '        communityName = community.Id
        '    End If
        '    If Not IsNothing(person) Then
        '        userName = person.SurnameAndName
        '    End If
        '    Dim results As List(Of dtoAccessResult) = GetUsageResults(context, startDate, endDate)
        '    'Dim cacheKey As String = ""
        '    'If context.CommunityID > 0 Then
        '    '    cacheKey = CachePolicy.GetCommunityUsageResults(context.CommunityID, context.UserID, context.ToString, context.ToString)
        '    'Else : cacheKey = CachePolicy.GetPortalUsageResults(context.UserID, context.ToString, context.ToString)

        '    'End If
        '    'Dim result As List(Of dtoAccessResult)
        '    'If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
        '    '    'If oContext.CommunityID > 0 Then
        '    '    '    oRemoteResultsList = 'WebService.GetCommunityAccess(oFromDate, oToDate, oContext.UserID, oContext.CommunityID)
        '    '    'Else
        '    '    '    oRemoteResultsList = 'WebService.GetPortalAccess(oFromDate, oToDate, oContext.UserID)
        '    '    'End If
        '    '    result = DALuserResults.GetAccessStatistics(context, startDate, endDate)
        '    '    lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, result, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
        '    'Else
        '    '    result = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
        '    'End If

        '    Dim OrderedList As New List(Of dtoAccessResult)

        '    oPager.Count = results.Count - 1
        '    oPager.PageIndex = CurrentPage

        '    Dim oQuery = (From c In results Select dtoAccessResult.Create(c, userName, communityName))

        '    oQuery = GetQueryOrdered(context.Order, context.Ascending, oQuery)
        '    oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
        '    OrderedList = oQuery.ToList
        '    If Not IsNothing(OrderedList) Then
        '        oList.AddRange(OrderedList)
        '    End If
        '    Return oList
        'End Function




        Private Function GetQueryOrdered(ByVal Order As lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder, ByVal Ascending As Boolean, ByVal oQuery As IEnumerable(Of dtoAccessResult)) As IEnumerable(Of dtoAccessResult)
            Select Case Order
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Community
                    oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.CommunityName)
                    If Not Ascending Then
                        oQuery = oQuery.Reverse
                    End If
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner
                    If Ascending Then
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.PersonName).ThenBy(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    Else
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.PersonName).ThenBy(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    End If
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.UsageTime
                    oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.UsageTime)
                    If Ascending Then
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.UsageTime).ThenBy(Function(o As dtoAccessResult) o.Day)
                    Else
                        oQuery = oQuery.OrderByDescending(Function(o As dtoAccessResult) o.UsageTime).ThenBy(Function(o As dtoAccessResult) o.Day)
                    End If
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Hour
                    If Ascending Then
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    Else
                        oQuery = oQuery.OrderByDescending(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    End If
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Day
                    If Ascending Then
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    Else
                        oQuery = oQuery.OrderByDescending(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    End If
                Case Else
                    If Ascending Then
                        oQuery = oQuery.OrderBy(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    Else
                        oQuery = oQuery.OrderByDescending(Function(o As dtoAccessResult) o.Day).ThenBy(Function(o As dtoAccessResult) o.Hour)
                    End If
            End Select
            Return oQuery
        End Function

        Public Function GetPerson(ByVal UserID As Integer) As iPerson
            Return BaseService.GetPerson(UserID)
        End Function
        Public Function GetCommunity(ByVal CommunityID As Integer) As iCommunity
            Return BaseService.GetCommunity(CommunityID)
        End Function

#Region "New"
        Public Function GetUsageResults(ByVal oContext As AccessResults.DomainModel.ContextBase, ByVal startDate As Date, ByVal endDate As Date, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)
            Dim userName As String = ""
            Dim communityName As String = ""
            Dim community As iCommunity = BaseService.GetCommunity(oContext.CommunityID)
            Dim person As iPerson = BaseService.GetPerson(oContext.UserID)
            If Not IsNothing(community) Then
                communityName = community.Id
            End If
            If Not IsNothing(person) Then
                userName = person.SurnameAndName
            End If
            Dim results As List(Of dtoAccessResult) = GetUsageResults(oContext, startDate, endDate)

            Dim OrderedList As New List(Of dtoAccessResult)

            If Not IsNothing(oPager) Then
                oPager.Count = results.Count - 1
                oPager.PageIndex = CurrentPage
            End If


            Dim oQuery = (From c In results Select dtoAccessResult.Create(c, userName, communityName))

            oQuery = GetQueryOrdered(oContext.Order, oContext.Ascending, oQuery)
            If Not IsNothing(oPager) Then
                oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            End If
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function

        Public Function FindUsersBetweenDate(ByVal context As ResultContextBase, ByVal startDate As Date, ByVal endDate As Date, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)
            Dim communityName As String = ""
            Dim community As iCommunity = BaseService.GetCommunity(context.CommunityID)
            If Not IsNothing(community) Then
                communityName = community.Id
            End If


            Dim statistics As New List(Of dtoAccessResult)
            Dim cacheKey As String = ""
            If context.CommunityID > 0 Then
                cacheKey = CachePolicy.FindCommunityUsersResultsBetweenDate(CurrentUserContext.CurrentUser.Id, context.CommunityID, startDate.ToString, endDate.ToString, context.NameSurnameFilter)
            Else : cacheKey = CachePolicy.FindPortalUsersResultsBetweenDate(CurrentUserContext.CurrentUser.Id, startDate.ToString, endDate.ToString, context.NameSurnameFilter)

            End If

            'If oContext.NameSurnameFilter = "" Then
            '    If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
            '        If oContext.CommunityID > 0 Then
            '            oRemoteResultsList = WebService.GetCommunityUsersAccessResultBetweenDate(oFromDate, oToDate, oContext.CommunityID)
            '        Else
            '            oRemoteResultsList = WebService.GetPortalUsersAccessResultBetweenDate(oFromDate, oToDate)
            '        End If
            '        lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, oRemoteResultsList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            '    Else
            '        oRemoteResultsList = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of SRV_accessMonitor.AccessResult))
            '    End If
            'End If



            Dim OrderedList As New List(Of dtoAccessResult)
            'Dim oPersonsId As List(Of Integer) = (From o As SRV_accessMonitor.AccessResult In oRemoteResultsList Select o.PersonID).Distinct.ToList()
            'Dim oPersonList As List(Of iPerson)
            'If oContext.NameSurnameFilter <> "" Then
            '    oPersonList = BaseService.GetPersonsByList(oPersonsId, oContext.NameSurnameFilter)
            'Else
            '    oPersonList = BaseService.GetPersonsByList(oPersonsId)
            'End If
            Dim persons As List(Of iPerson)
            If context.NameSurnameFilter = "" Then
                If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                    statistics = DALuserResults.GetAccessResultsBetweenDate(context, startDate, endDate)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
                Else
                    statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
                End If
            Else
                If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                    Dim idUsers As List(Of Integer) = (From p In BaseService.GetPersonsByNameSurname(context.NameSurnameFilter, context.CommunityID) Select p.Id).ToList
                    'If oContext.CommunityID > 0 Then
                    '    oRemoteResultsList = WebService.GetCommunityAccessResultBetweenDate(oPersonsId, oFromDate, oToDate, oContext.CommunityID)
                    'Else
                    '    oRemoteResultsList = WebService.GetPortalAccessResultBetweenDate(oPersonsId, oFromDate, oToDate)
                    'End If
                    statistics = DALuserResults.GetAccessResultsBetweenDate(context, idUsers, startDate, endDate)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
                Else
                    statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
                End If
            End If
            persons = BaseService.GetPersonsByList(statistics.Select(Function(s) s.PersonID).Distinct.ToList())

            Dim oQuery = (From c In statistics Group Join p In persons On c.PersonID Equals p.Id _
                            Into children = Group From child In children.DefaultIfEmpty(New Person With {.Id = c.PersonID, .Name = "", .Surname = ""}) _
            Select dtoAccessResult.Create(c, child.SurnameAndName, communityName))

            oQuery = GetQueryOrdered(context.Order, context.Ascending, oQuery)

            If Not IsNothing(oPager) Then
                oPager.Count = oQuery.Count - 1
                oPager.PageIndex = CurrentPage
                oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            End If

            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function

        Public Function GetUsageResults(ByRef TotalUsageTime As TimeSpan, ByVal context As AccessResults.DomainModel.ResultContextBase, ByVal startDate As Date, ByVal endDate As Date, ByRef pageIndex As Integer, ByRef pager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)
            Dim userName As String = ""
            Dim communityName As String = ""
            Dim community As iCommunity = BaseService.GetCommunity(context.CommunityID)
            Dim person As iPerson = BaseService.GetPerson(context.UserID)
            If Not IsNothing(community) Then
                communityName = community.Id
            End If
            If Not IsNothing(person) Then
                userName = person.SurnameAndName
            End If

            Dim results As List(Of dtoAccessResult) = GetUsageResults(context, startDate, endDate)
            Dim OrderedList As New List(Of dtoAccessResult)

            If Not IsNothing(pager) Then
                pager.Count = results.Count - 1
                pager.PageIndex = pageIndex
            End If


            Dim oQuery = (From c In results Select dtoAccessResult.Create(c, userName, communityName))
            If Not IsNothing(TotalUsageTime) Then
                Dim TotalTime As Long = (From o In oQuery Select o.UsageTime).Sum()
                TotalUsageTime = New TimeSpan(0, 0, TotalTime)
            End If
            oQuery = GetQueryOrdered(context.Order, context.Ascending, oQuery)
            If Not IsNothing(pager) Then
                oQuery = oQuery.Skip(pager.Skip).Take(pager.PageSize)
            End If
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function
        Public Function FindUsersBetweenDate(ByRef TotalUsageTime As TimeSpan, ByVal context As ResultContextBase, ByVal startDate As Date, ByVal endDate As Date, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoAccessResult)
            Dim oList As New List(Of dtoAccessResult)
            Dim communityName As String = ""
            Dim community As iCommunity = BaseService.GetCommunity(context.CommunityID)
            If Not IsNothing(community) Then
                communityName = community.Id
            End If


            Dim statistics As New List(Of dtoAccessResult)
            Dim cacheKey As String = ""
            If context.CommunityID > 0 Then
                cacheKey = CachePolicy.FindCommunityUsersResultsBetweenDate(CurrentUserContext.CurrentUser.Id, context.CommunityID, startDate.ToString, endDate.ToString, context.NameSurnameFilter)
            Else : cacheKey = CachePolicy.FindPortalUsersResultsBetweenDate(CurrentUserContext.CurrentUser.Id, startDate.ToString, endDate.ToString, context.NameSurnameFilter)

            End If

            Dim OrderedList As New List(Of dtoAccessResult)
            If context.NameSurnameFilter = "" Then
                If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                    'If oContext.CommunityID > 0 Then
                    '    oRemoteResultsList = WebService.GetCommunityUsersAccessResultBetweenDate(oFromDate, oToDate, oContext.CommunityID)
                    'Else
                    '    oRemoteResultsList = WebService.GetPortalUsersAccessResultBetweenDate(oFromDate, oToDate)
                    'End If
                    statistics = DALuserResults.GetAccessResultsBetweenDate(context, startDate, endDate)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
                Else
                    statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
                End If
            Else
                If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                    'oPersonsId = (From p In BaseService.GetPersonsByNameSurname(oContext.NameSurnameFilter, oContext.CommunityID) Select p.Id).ToList
                    'If oContext.CommunityID > 0 Then
                    '    oRemoteResultsList = WebService.GetCommunityAccessResultBetweenDate(oPersonsId, oFromDate, oToDate, oContext.CommunityID)
                    'Else
                    '    oRemoteResultsList = WebService.GetPortalAccessResultBetweenDate(oPersonsId, oFromDate, oToDate)
                    'End If
                    Dim idUsers As List(Of Integer) = (From p In BaseService.GetPersonsByNameSurname(context.NameSurnameFilter, context.CommunityID) Select p.Id).ToList
                    statistics = DALuserResults.GetAccessResultsBetweenDate(context, idUsers, startDate, endDate)
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
                Else
                    statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
                End If
            End If
            Dim persons As List(Of iPerson) = BaseService.GetPersonsByList(statistics.Select(Function(s) s.PersonID).Distinct.ToList())
            Dim oQuery = (From c In statistics Group Join p In persons On c.PersonID Equals p.Id _
                            Into children = Group From child In children.DefaultIfEmpty(New Person With {.Id = c.PersonID, .Name = "", .Surname = ""}) _
            Select dtoAccessResult.Create(c, child.SurnameAndName, communityName))

            oQuery = GetQueryOrdered(context.Order, context.Ascending, oQuery)
            If Not IsNothing(TotalUsageTime) Then
                Dim TotalTime As Long = (From o In oQuery Select o.UsageTime).Sum()
                TotalUsageTime = New TimeSpan(0, 0, TotalTime)
            End If
            If Not IsNothing(oPager) Then
                oPager.Count = oQuery.Count - 1
                oPager.PageIndex = CurrentPage
                oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            End If

            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function

        Public Function FindUserCommunitiesList(ByVal oContext As AccessResults.DomainModel.ResultContextBase, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoCommunityResult)
            Dim oList As New List(Of dtoCommunityResult)

            Dim statistics As New List(Of dtoUserResult)
            Dim cacheKey As String = CachePolicy.FindUserCommunities(oContext.UserID)

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                statistics = DALuserResults.GetCommunitiesWithAccessResult(oContext.UserID)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoUserResult))
            End If

            If Not IsNothing(statistics) AndAlso statistics.Count > 0 Then
                Dim oCommunityList As New List(Of Community)
                'MODIFICATO FEBBRAIO 2010
                oCommunityList = Me.BaseService.GetCommunitiesById((From o In statistics Where o.IdCommunity > 0 Distinct Select o.IdCommunity).ToList)

                oPager.Count = (From c In oCommunityList Where (oContext.NameSurnameFilter = "" OrElse c.Name.ToLower.Contains(oContext.NameSurnameFilter)) Select c.Id).Count - 1
                oPager.PageIndex = CurrentPage
                Dim oQuery = (From c In oCommunityList Where (oContext.NameSurnameFilter = "" OrElse c.Name.ToLower.Contains(oContext.NameSurnameFilter)) Select New dtoCommunityResult(c, oContext.UserID))
                Select Case oContext.Order
                    Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Community
                        oQuery = oQuery.OrderBy(Function(o As dtoCommunityResult) o.CommunityName)
                End Select
                If Not oContext.Ascending Then
                    oQuery = oQuery.Reverse
                End If
                oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)

                Dim OrderedList As New List(Of dtoCommunityResult)
                OrderedList = oQuery.ToList
                If Not IsNothing(OrderedList) Then
                    oList.AddRange(OrderedList)
                End If
            End If
            Return oList
        End Function

        Public Function FindPortalUsersList(ByVal oContext As ResultContextBase, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoUserAccessResult)
            Dim oList As New List(Of dtoUserAccessResult)
            Dim oUserList As List(Of Person) = New List(Of Person)

            Dim statistics As New List(Of dtoUserResult)
            Dim cacheKey As String = CachePolicy.FindPortalUsers

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                ' oRemoteUserList = WebService.FindPortalUsersWithAccessResult()
                statistics = DALuserResults.GetPortalUsersWithAccessResult()
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoUserResult))
            End If



            Dim OrderedList As New List(Of dtoUserAccessResult)
            If oContext.NameSurnameFilter <> "" Then
                oUserList = BaseService.GetPersonsByNameSurname(oContext.NameSurnameFilter, 0)
                oPager.Count = oUserList.Count - 1
                oPager.PageIndex = CurrentPage
            Else
                oUserList = BaseService.GetSystemPersonsByList((From o In statistics Select o.IdPerson Distinct).ToList)
                oPager.Count = (From o In oUserList Where o.Id > 0 Select o.Id Distinct).Count - 1
                oPager.PageIndex = CurrentPage
            End If
            Dim oQuery = (From u In oUserList Join oRemote In statistics On u.Id Equals oRemote.IdPerson _
            Select New dtoUserAccessResult() With {.PersonID = u.Id, .PersonName = u.SurnameAndName})

            Select Case oContext.Order
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner
                    oQuery = oQuery.OrderBy(Function(o As dtoUserAccessResult) o.PersonName)
            End Select
            If Not oContext.Ascending Then
                oQuery = oQuery.Reverse
            End If


            oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If

            Return oList
        End Function
        Public Function FindCommunityUsersList(ByVal oContext As ResultContextBase, ByRef CurrentPage As Integer, ByRef oPager As PagerBase) As List(Of dtoUserAccessResult)
            Dim oList As New List(Of dtoUserAccessResult)

            Dim oUserList As List(Of Person)
            Dim statistics As New List(Of dtoUserResult)
            Dim cacheKey As String = CachePolicy.FindCommunityUsers(oContext.CommunityID)

            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                ' oRemoteUserList = WebService.FindUsersWithAccessResult(oContext.CommunityID)
                statistics = DALuserResults.GetUsersWithAccessResult(oContext.CommunityID)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, statistics, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                statistics = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoUserResult))
            End If


            Dim OrderedList As New List(Of dtoUserAccessResult)
            If oContext.NameSurnameFilter <> "" Then
                oUserList = BaseService.GetPersonsByNameSurname(oContext.NameSurnameFilter, oContext.CommunityID)
                oPager.Count = oUserList.Count - 1
                oPager.PageIndex = CurrentPage
            Else
                oUserList = BaseService.GetSystemPersonsByList((From o In statistics Select o.IdPerson Distinct).ToList)
                oPager.Count = (From o In oUserList Where o.Id > 0 Select o.Id Distinct).Count - 1
                oPager.PageIndex = CurrentPage
            End If



            'If oContext.Order <> lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner Then
            '    oUserList = BaseService.GetCommunityPersonList(oContext.CommunityID, SubscriptionStatus.all)
            'Else
            '    If oContext.CommunityID > 0 Then
            '        oUserList = BaseService.GetCommunityPersonList(oPager, UserActions.DomainModel.StatisticOrder.Owner, oContext.Ascending, oContext.CommunityID, SubscriptionStatus.all)
            '    Else
            '        oUserList = BaseService.GetSystemPersonList(oPager, UserActions.DomainModel.StatisticOrder.Owner, oContext.Ascending)
            '    End If
            'End If

            Dim oQuery = (From u In oUserList Group Join oRemote In statistics On u.Id Equals oRemote.IdPerson _
                Into children = Group From child In children.DefaultIfEmpty(New dtoUserResult() With {.IdCommunity = oContext.CommunityID, .IdPerson = u.Id, .UsageTime = 0}) Select _
                 New dtoUserAccessResult() With {.PersonID = u.Id, .PersonName = u.SurnameAndName})

            Select Case oContext.Order
                Case lm.Comol.Modules.UsageResults.DomainModel.ResultsOrder.Owner
                    oQuery = oQuery.OrderBy(Function(o As dtoUserAccessResult) o.PersonName)
                Case Else
                    oQuery = oQuery.OrderBy(Function(o As dtoUserAccessResult) o.PersonName)
            End Select
            If Not oContext.Ascending Then
                oQuery = oQuery.Reverse
            End If
            oQuery = oQuery.Skip(oPager.Skip).Take(oPager.PageSize)
            OrderedList = oQuery.ToList
            If Not IsNothing(OrderedList) Then
                oList.AddRange(OrderedList)
            End If
            Return oList
        End Function
        Public Function GetPrintResults(ByRef oTotalTime As TimeSpan, ByVal oContext As ResultContextBase, ByRef oStartDate As Date, ByRef oEndDate As Date) As List(Of dtoAccessResult)
            Return GetUsageResults(oTotalTime, oContext, oStartDate, oEndDate, 0, Nothing)
        End Function

        Public Function GetPrintResultsBetweenDate(ByRef oTotalTime As TimeSpan, ByVal oContext As ResultContextBase, ByRef oStartDate As Date, ByRef oEndDate As Date) As List(Of dtoAccessResult)
            Return FindUsersBetweenDate(oTotalTime, oContext, oStartDate, oEndDate, 0, Nothing)
        End Function

        Private Function GetUsageResults(ByVal context As AccessResults.DomainModel.ContextBase, ByVal startDate As Date, ByVal endDate As Date) As List(Of dtoAccessResult)

            Dim cacheKey As String = ""
            If context.CommunityID > 0 Then
                cacheKey = CachePolicy.GetCommunityUsageResults(context.CommunityID, context.UserID, startDate.ToString, endDate.ToString)
            Else : cacheKey = CachePolicy.GetPortalUsageResults(context.UserID, startDate.ToString, endDate.ToString)

            End If
            Dim result As List(Of dtoAccessResult)
            If lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey) Is Nothing Then
                result = DALuserResults.GetAccessStatistics(context, startDate, endDate)
                lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache.Insert(cacheKey, result, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                result = CType(lm.Comol.Core.DomainModel.Helpers.CacheHelper.Cache(cacheKey), List(Of dtoAccessResult))
            End If
            Return result
        End Function
#End Region
    End Class
End Namespace