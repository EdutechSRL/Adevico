Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.UserActions.DomainModel

Namespace lm.Comol.Modules.UserActions.BusinessLogic
    Public Class BaseStatisticsService
        Inherits lm.Comol.Core.Business.BaseCoreServices

        Public Sub New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
        End Sub
        Public Sub New(ByVal userContext As iUserContext, ByVal dataContext As iDataContext)
            MyBase.New(dataContext)
            MyBase.UC = userContext
        End Sub

        Public Function GetPerson(ByVal PersonID As Integer) As iPerson
            Dim oPerson As Person = Nothing

            Try
                oPerson = Manager.GetPerson(PersonID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oPerson) Then
                oPerson = New Person
            End If
            Return oPerson
        End Function
        Public Function GetCommunity(ByVal CommunityID As Integer) As Community
            Dim oCommunity As Community = Nothing

            Try
                oCommunity = Manager.GetCommunity(CommunityID)
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            If IsNothing(oCommunity) Then
                oCommunity = New Community
            End If
            Return oCommunity
        End Function

        Public Function GetSubscription(ByVal idCommunity As Integer, ByVal idPerson As Integer) As Subscription
            Return Manager.GetSubscription(idPerson, idCommunity)
        End Function
        Public Function GetDefaultIdRole(ByVal idCommunity As Integer) As Integer
            Dim idRole As Integer = 0
            Try
                Manager.BeginTransaction()
                Dim c As Community = Manager.Get(Of Community)(idCommunity)
                If Not IsNothing(c) Then
                    Dim r As Role = GetDefaultRole(c)
                    If Not IsNothing(r) Then
                        idRole = r.Id
                    End If
                 End If
                Manager.Commit()
            Catch ex As Exception
                Manager.RollBack()
            End Try
            Return idRole
        End Function
        Public Function GetDefaultRole(ByVal cm As Community) As Role
            Return (From ct In Manager.GetIQ(Of RoleCommunityTypeTemplate)()
                   Where ct.Type.Id.Equals(cm.TypeOfCommunity.Id) AndAlso ct.isDefault = True
                   Select ct).Skip(0).Take(1).ToList().Select(Function(c) c.Role).FirstOrDefault()
        End Function


        'Public Function GetCommunitiesList(ByVal items As List(Of Integer)) As List(Of Community)
        '    Dim result As List(Of Community) = New List(Of Community)

        '    Try
        '        result = (From c In Manager.GetIQ(Of Community)() Select c).ToList
        '    Catch ex As Exception
        '        Debug.Write(ex.ToString)
        '        result = New List(Of Community)
        '    End Try
        '    Return result
        'End Function
        Public Function GetCommunitiesById(ByVal items As List(Of Integer)) As List(Of Community)
            Dim result As List(Of Community) = New List(Of Community)

            Try
                Dim pageSize As Integer = 100
                Dim pageIndex As Integer = 0
                Dim paged As List(Of Integer) = items.Skip(pageIndex).Take(pageSize).ToList
                Dim query = (From c In Manager.GetIQ(Of Community)() Select c)

                While paged.Any
                    result.AddRange(query.Where(Function(c) paged.Contains(c.Id)).ToList())
                    pageIndex += 1
                    paged = items.Skip(pageIndex * pageSize).Take(pageSize).ToList
                End While
            Catch ex As Exception
                Debug.Write(ex.ToString)
                result = New List(Of Community)
            End Try
            Return result
        End Function
        Public Function GetSubscribedCommunityList(ByVal idPerson As Integer) As List(Of Community)
            Dim communities As New List(Of Community)
            'Not IsNothing(s.Role) AndAlso Not IsNothing(s.Person) AndAlso
            Try
                Dim person As Person = Manager.GetPerson(idPerson)
                communities = (From s In Manager.GetIQ(Of Subscription)() Where s.Person Is person AndAlso s.Role.Id > 0 _
                              Select DirectCast(s.Community, Community)).ToList()

            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return communities
        End Function
        Public Function GetAvailableCommunitiesBySubscription(ByVal idPerson As Integer) As List(Of Integer)
            Dim items As New List(Of Integer)
            Try
                items = (From s In Manager.GetIQ(Of LazySubscription)() Where s.IdPerson = idPerson AndAlso s.IdRole > 0 AndAlso s.Enabled _
                              Select s.IdCommunity).ToList()

            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return items
        End Function
        Public Function GetSubscribedCommunityCount(ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As Integer
            Dim count As Long = 0
            Try
                Dim c As Community = Manager.GetCommunity(idCommunity)

                If Not IsNothing(c) Then
                    count = (From s In Manager.GetIQ(Of Subscription)()
                        Where s.Community Is c AndAlso (status = SubscriptionStatus.all OrElse s.Status = status) AndAlso s.Role IsNot Nothing AndAlso s.Role.Id > 0 Select s.Id).Count()

                End If


            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return count
        End Function
        'Public Function GetSubscriptionsToCommunityCount(ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As Integer
        '    Dim count As Long = 0
        '    Try
        '        Dim person As Person = Manager.GetPerson(idPerson)

        '        If Not IsNothing(person) Then
        '            count = (From s In Manager.GetIQ(Of Subscription)()
        '                Where s.Person Is person AndAlso (status = SubscriptionStatus.all OrElse s.Status = status) AndAlso s.Role IsNot Nothing AndAlso s.Role.Id > 0 Select s.Id).Count()

        '        End If


        '    Catch ex As Exception
        '        Debug.Write(ex.ToString)
        '    End Try
        '    Return count
        'End Function
        Public Function GetPersonsByList(ByVal oListID As List(Of Integer)) As List(Of iPerson)
            Dim oList As List(Of iPerson) = New List(Of iPerson)

            Try
                For Each PersonID As Integer In oListID
                    Dim oPerson As iPerson = Me.GetPerson(PersonID)
                    If Not IsNothing(oPerson) Then : oList.Add(oPerson)

                    End If
                Next
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function

        Public Function GetSystemPersonList(ByVal surnameName As String, ByVal pager As PagerBase, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal alsoDisabled As Boolean) As List(Of Person)
            Dim result As List(Of Person)
            Try
                Dim query = (From o As Person In Manager.GetIQ(Of Person)() Where (alsoDisabled OrElse Not o.isDisabled) AndAlso (o.Name.Contains(surnameName) OrElse o.Surname.Contains(surnameName)) Select o)
                If order = StatisticOrder.Owner Then
                    If ascending Then
                        query = query.OrderBy(Function(p As Person) p.Surname).ThenBy(Function(p As Person) p.Name)
                    Else
                        query = query.OrderByDescending(Function(p As Person) p.Surname).ThenByDescending(Function(p As Person) p.Name)
                    End If
                End If
                result = query.Skip(pager.Skip).Take(pager.PageSize).ToList()

            Catch ex As Exception
                result = New List(Of Person)
                Debug.Write(ex.ToString)
            End Try

            Return result
        End Function
        Public Function GetSystemPersonList(ByVal pager As PagerBase, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal alsoDisabled As Boolean) As List(Of Person)
            Dim result As List(Of Person)
            Try
                Dim query = (From o As Person In Manager.GetIQ(Of Person)() Where (alsoDisabled OrElse Not o.isDisabled) Select o)
                If order = StatisticOrder.Owner Then
                    If ascending Then
                        query = query.OrderBy(Function(p As Person) p.Surname).ThenBy(Function(p As Person) p.Name)
                    Else
                        query = query.OrderByDescending(Function(p As Person) p.Surname).ThenByDescending(Function(p As Person) p.Name)
                    End If
                End If
                query = query.Skip(pager.Skip).Take(pager.PageSize).ToList()
                result = query.ToList

            Catch ex As Exception
                result = New List(Of Person)
                Debug.Write(ex.ToString)
            End Try

            Return result
        End Function
        Public Function GetSystemPersonList(idSearcher As Integer) As List(Of Person)
            Dim items As List(Of Person) = New List(Of Person)
            Try

                items = (From p In Manager.GetIQ(Of Person)() Where p.isDisabled = False Select p).ToList()
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try

            Return items
        End Function


        Public Function GetSystemPersonCount() As Integer
            Try
                Return (From p In Manager.GetIQ(Of Person)() Where p.isDisabled = False Select p.Id).Count
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return 0
        End Function

        Public Function GetCommunityPersonList(ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As List(Of Person)
            If idCommunity <= 0 Then
                Return GetSystemPersonList(UC.CurrentUserID)
            Else

                Try
                    Dim community As Community = Manager.GetCommunity(idCommunity)
                    Dim querySub = (From s In Manager.GetIQ(Of Subscription)() Where s.Community Is community AndAlso s.Role.Id > 0 Select s)

                    'select case sullo status
                    Select Case status
                        Case SubscriptionStatus.blocked
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.waiting
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso Not s.Accepted)
                        Case SubscriptionStatus.activemember
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.all
                            Exit Select
                        Case Else
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                    End Select
                    Return (From o In querySub Select DirectCast(o.Person, Person)).ToList
                Catch ex As Exception
                    Debug.Write(ex.ToString)
                End Try

                Return New List(Of Person)
            End If
        End Function
        Public Function GetCommunityPersonList(ByVal searchBy As String, ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As List(Of Person)
            If String.IsNullOrEmpty(searchBy) Then
                Return GetCommunityPersonList(idCommunity, status)
            Else
                If idCommunity <= 0 Then
                    Return GetSystemPersonList(UC.CurrentUserID)
                Else
                    Try
                        Dim querySub = (From s In Manager.GetIQ(Of LazySubscription)() Where s.IdCommunity.Equals(idCommunity) AndAlso s.IdRole > 0 Select s)

                        'select case sullo status
                        Select Case status
                            Case SubscriptionStatus.blocked
                                querySub = querySub.Where(Function(s) Not s.Enabled AndAlso s.Accepted)
                            Case SubscriptionStatus.waiting
                                querySub = querySub.Where(Function(s) Not s.Enabled AndAlso Not s.Accepted)
                            Case SubscriptionStatus.activemember
                                querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                            Case SubscriptionStatus.all
                                Exit Select
                            Case Else
                                querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                        End Select
                        Dim usersId As List(Of Integer) = GetUsersIdByName(searchBy, querySub.Select(Function(s) s.IdPerson))
                        Return GetUsersByName(searchBy, usersId)
                    Catch ex As Exception
                        Debug.Write(ex.ToString)
                    End Try

                    Return New List(Of Person)
                End If
            End If
           
        End Function
        Public Function GetCommunityPersonList(ByVal searchBy As String, ByVal pager As PagerBase, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As List(Of Person)
            Dim oList As IList(Of Subscription) = Nothing

            Try

                If Not String.IsNullOrEmpty(searchBy) Then
                    Dim querySub = (From s In Manager.GetIQ(Of LazySubscription)() Where s.IdCommunity.Equals(idCommunity) AndAlso s.IdRole > 0 Select s)

                    'select case sullo status
                    Select Case status
                        Case SubscriptionStatus.blocked
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.waiting
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso Not s.Accepted)
                        Case SubscriptionStatus.activemember
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.all
                            Exit Select
                        Case Else
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                    End Select
                    Dim usersId As List(Of Integer) = GetUsersIdByName(searchBy, querySub.Select(Function(s) s.IdPerson).ToList())
                    Dim users = GetUsersByName(searchBy, usersId)
                    If order = StatisticOrder.Owner Then
                        users = users.OrderBy(Function(s) s.SurnameAndName).ToList
                    End If
                    Return users.Skip(pager.Skip).Take(pager.PageSize).ToList
                Else
                    Dim community As Community = Manager.GetCommunity(idCommunity)
                    Dim querySub = (From s In Manager.GetIQ(Of Subscription)() Where s.Community Is community AndAlso s.Role.Id > 0 Select s)

                    'select case sullo status
                    Select Case status
                        Case SubscriptionStatus.blocked
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.waiting
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso Not s.Accepted)
                        Case SubscriptionStatus.activemember
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.all
                            Exit Select
                        Case Else
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                    End Select

                    If order = StatisticOrder.Owner Then
                        Dim usersId As List(Of Integer) = querySub.Select(Function(s) s.Person.Id).ToList
                        If ascending Then
                            Return GetUsersByName("", usersId).OrderBy(Function(p) p.SurnameAndName).Skip(pager.Skip).Take(pager.PageSize).ToList
                        Else
                            Return GetUsersByName("", usersId).OrderByDescending(Function(p) p.SurnameAndName).Skip(pager.Skip).Take(pager.PageSize).ToList
                        End If
                    Else
                        oList = querySub.Skip(pager.Skip).Take(pager.PageSize).ToList
                    End If
                End If
            Catch ex As Exception
                oList = New List(Of Subscription)
                Debug.Write(ex.ToString)
            End Try

            Return (From o In oList Select DirectCast(o.Person, Person)).ToList
        End Function
        Public Function GetSubscriptionsCount(ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As Integer
            Try

                Dim community As Community = Manager.GetCommunity(idCommunity)


                Return (From s In Manager.GetIQ(Of Subscription)() _
                        Where s.Community Is community AndAlso (s.Role IsNot Nothing AndAlso s.Role.Id > 0) AndAlso (status = SubscriptionStatus.all _
                                                                                                                    OrElse s.Status = status)
                                                                                                                Select s.Id).Count


            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return 0
        End Function
        Public Function GetSubscriptionsCount(ByVal searchBy As String, ByVal idCommunity As Integer, ByVal status As lm.Comol.Core.DomainModel.SubscriptionStatus) As Integer
            If String.IsNullOrEmpty(searchBy) Then
                Return GetSubscribedCommunityCount(idCommunity, status)
            Else
                Try
                    Dim querySub = (From s In Manager.GetIQ(Of LazySubscription)() Where s.IdCommunity.Equals(idCommunity) AndAlso s.IdRole > 0 Select s)

                    'select case sullo status
                    Select Case status
                        Case SubscriptionStatus.blocked
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.waiting
                            querySub = querySub.Where(Function(s) Not s.Enabled AndAlso Not s.Accepted)
                        Case SubscriptionStatus.activemember
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                        Case SubscriptionStatus.all
                            Exit Select
                        Case Else
                            querySub = querySub.Where(Function(s) s.Enabled AndAlso s.Accepted)
                    End Select
                    Return GetUsersIdByName(searchBy, querySub.Select(Function(s) s.IdPerson).ToList).Count

                Catch ex As Exception
                    Debug.Write(ex.ToString)
                End Try
            End If
           
            Return 0
        End Function

        Public Function GetSystemModuleList() As List(Of COL_BusinessLogic_v2.PlainService)
            Return GetGenericModuleList(-1, -1)
        End Function
        Public Function GetCommunityModuleList(ByVal CommunityID As Integer) As List(Of COL_BusinessLogic_v2.PlainService)
            Return GetGenericModuleList(CommunityID, -1)
        End Function

        Public Function GetUserModuleList(ByVal CommunityID As Integer, ByVal PersonID As Integer) As List(Of COL_BusinessLogic_v2.PlainService)
            Dim oSubscription As Subscription = GetSubscription(CommunityID, PersonID)
            Dim RoleID As Integer = -1
            If Not IsNothing(oSubscription) Then
                RoleID = oSubscription.Role.Id
            End If
            Return GetGenericModuleList(CommunityID, RoleID)
        End Function
        Public Function GetGenericModuleList(ByVal CommunityID As Integer, ByVal RoleID As Integer) As List(Of COL_BusinessLogic_v2.PlainService)
            Dim oList As New List(Of COL_BusinessLogic_v2.PlainService)

            Try
                If CommunityID < 0 Then
                    oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.ListSystemTranslated(UC.Language.Id)
                ElseIf RoleID < 0 Then
                    oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.ListCommunityTranslated(UC.Language.Id, CommunityID)
                Else
                    oList = COL_BusinessLogic_v2.Comol.Manager.ManagerService.RoleTranslated(RoleID, CommunityID, UC.Language.Id)
                End If

                Return oList
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try

            Return oList
        End Function

        Public Function GetPersonsByNameSurname(ByVal NameSurname As String, ByVal idCommunity As Integer) As List(Of Person)
            Dim items As List(Of Person)

            Try
                If idCommunity <= 0 Then
                    items = (From p In Manager.GetIQ(Of Person)() Where p.SurnameAndName.ToLower.Contains(NameSurname) Select p).ToList
                Else
                    Dim community As Community = Manager.GetCommunity(idCommunity)
                    Dim persons As List(Of Person)
                    persons = (From subs In Manager.GetIQ(Of Subscription)() Where subs.Community Is community AndAlso subs.Accepted = True AndAlso subs.Role.Id > 0 Select DirectCast(subs.Person, Person)).ToList
                    items = (From p In persons Where p.SurnameAndName.ToLower.Contains(NameSurname) Select p).ToList

                End If
            Catch ex As Exception
                items = New List(Of Person)
            End Try
            Return items
            'Return (From p In items Where p.SurnameAndName.ToLower.Contains(NameSurname) Select p).ToList
        End Function
        Public Function GetSystemPersonCount(ByVal searchBy As String) As Integer
            searchBy = searchBy.ToLower
            Return (From p In Manager.GetIQ(Of Person)() Where p.Name.Contains(searchBy) OrElse p.Surname.Contains(searchBy) Select p).Count
        End Function

        Public Function GetSystemPersonsByList(ByVal oListID As List(Of Integer)) As List(Of Person)
            Dim oList As List(Of Person) = New List(Of Person)

            Try
                For Each PersonID As Integer In oListID
                    Dim oPerson As iPerson = Me.GetPerson(PersonID)
                    If Not IsNothing(oPerson) AndAlso oPerson.Id > 0 Then : oList.Add(oPerson)

                    End If
                Next
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function

        Public Function GetPersonsByList(ByVal oListID As List(Of Integer), ByVal NameSurname As String) As List(Of iPerson)
            Dim oList As List(Of iPerson)

            Try
                oList = New List(Of iPerson)
                For Each PersonID As Integer In oListID
                    Dim oPerson As iPerson = Me.GetPerson(PersonID)
                    If Not IsNothing(oPerson) Then : oList.Add(oPerson)

                    End If
                Next
            Catch ex As Exception
                Debug.Write(ex.ToString)
                oList = New List(Of iPerson)
            End Try
            Return (From p In oList Where p.SurnameAndName.ToLower.Contains(NameSurname) Select p).ToList
        End Function
        Public Function GetPersonsByListCount(ByVal persons As List(Of Integer), ByVal NameSurname As String) As Integer
            Dim items As List(Of Person)

            Try
                items = (From p In Manager.GetIQ(Of Person)() Where persons.Contains(p.Id) Select p).ToList
            Catch ex As Exception
                items = New List(Of Person)
                Debug.Write(ex.ToString)
            End Try
            Return (From p In items Where p.SurnameAndName.ToLower.Contains(NameSurname) Select p.Id).Count
        End Function
        Public Function GetSystemCommunities(ByVal idPerson As Integer) As List(Of Community)
            Dim items As List(Of Community)

            Try
                Dim person As Person = Manager.GetPerson(idPerson)
                Dim idType As Integer = UserTypeStandard.Guest
                If Not IsNothing(person) Then
                    idType = person.TypeID
                End If

                If (idType = UserTypeStandard.Administrator OrElse idType = UserTypeStandard.Administrative OrElse idType = UserTypeStandard.SysAdmin) Then
                    items = (From c In Manager.GetIQ(Of Community)() Select c).ToList
                Else
                    items = (From s In Manager.GetIQ(Of Subscription)() Where Not IsNothing(s.Role) AndAlso s.Role.Id > 0 AndAlso s.Enabled AndAlso s.Person Is person Select DirectCast(s.Community, Community)).ToList
                End If
            Catch ex As Exception
                items = New List(Of Community)
                Debug.Write(ex.ToString)
            End Try
            Return items
        End Function
        Public Function GetSystemCommunitiesID(ByVal searchBy As String, ByVal idPerson As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of Integer)
            Dim items As New List(Of Integer)

            Try
                Dim person As Person = Manager.GetPerson(idPerson)
                Dim idType As Integer = UserTypeStandard.Guest
                If Not IsNothing(person) Then
                    idType = person.TypeID
                End If

                If (idType = UserTypeStandard.Administrator OrElse idType = UserTypeStandard.Administrative OrElse idType = UserTypeStandard.SysAdmin) Then
                    items = (From c In Manager.GetIQ(Of Community)() Where (String.IsNullOrEmpty(searchBy) OrElse c.Name.Contains(searchBy)) Select c.Id).ToList
                Else
                    items = GetCommunityIDByName(searchBy, (From s In Manager.GetIQ(Of Subscription)() Where s.Role.Id > 0 AndAlso s.Enabled AndAlso s.Person Is person Select s.Community.Id).ToList)
                    items.AddRange(GetCommunitiesWithStatistics(searchBy, idPerson, startDate, endDate))
                    'Dim tmp As List(Of Integer) = (From s In Manager.GetIQ(Of Subscription)() Where s.Role.Id > 0 AndAlso s.Enabled AndAlso s.Person Is person Select s.Community.Id).ToList
                    'tmp.AddRange(GetCommunitiesWithStatistics(idPerson, startDate, endDate))
                    'If Not String.IsNullOrEmpty(searchBy) Then
                    '    Dim pageSize As Integer = 100
                    '    Dim pageIndex As Integer = 0
                    '    Dim paged As List(Of Integer) = tmp.Skip(pageIndex).Take(pageSize).ToList

                    '    While paged.Any
                    '        items.AddRange((From c In Manager.GetIQ(Of Community)() Where (c.Name.Contains(searchBy) AndAlso paged.Contains(c.Id)) Select c.Id).ToList)
                    '        pageIndex += 1
                    '        paged = tmp.Skip(pageIndex * pageSize).Take(pageSize).ToList
                    '    End While


                    'Else
                    '    items = tmp
                    'End If
                End If
            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return items.Distinct().ToList()
        End Function

        Public Function GetCommunitiesWithStatistics(searchBy As String, ByVal idPerson As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of Integer)
            Dim persons As New List(Of Integer)
            persons.Add(idPerson)
            Return GetCommunityIDByName(searchBy, DALuserActions.GetCommunitiesWithStatistics(persons, startDate, endDate))

        End Function
        Public Function GetModulesIdWithStatistics(ByVal idPerson As Integer, ByVal idCommunity As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of Integer)
            Dim persons As New List(Of Integer)
            persons.Add(idPerson)
            Dim communities As New List(Of Integer)
            communities.Add(idCommunity)
            Return DALuserActions.GetModulesIdWithStatistics(persons, communities, startDate, endDate)
        End Function
        Public Function GetSubscribedCommunityIDList(searchBy As String, ByVal idPerson As Integer) As List(Of Integer)
            Dim oList As New List(Of Integer)

            Try
                Dim person As Person = Manager.GetPerson(idPerson)
                Dim tmp As List(Of Integer) = (From s In Manager.GetIQ(Of Subscription)() Where s.Person Is person AndAlso s.Role.Id > 0 AndAlso s.Enabled Select s.Community.Id).ToList
                oList = GetCommunityIDByName(searchBy, tmp)

            Catch ex As Exception
                Debug.Write(ex.ToString)
            End Try
            Return oList
        End Function

        Private Function GetCommunityIDByName(searchBy As String, idItems As List(Of Integer)) As List(Of Integer)
            Dim items As New List(Of Integer)

            Try
                If Not String.IsNullOrEmpty(searchBy) Then
                    Dim pageSize As Integer = 100
                    Dim pageIndex As Integer = 0
                    Dim paged As List(Of Integer) = idItems.Skip(pageIndex).Take(pageSize).ToList

                    While paged.Any
                        items.AddRange((From c In Manager.GetIQ(Of Community)() Where (c.Name.Contains(searchBy) AndAlso paged.Contains(c.Id)) Select c.Id).ToList)
                        pageIndex += 1
                        paged = items.Skip(pageIndex * pageSize).Take(pageSize).ToList
                    End While
                Else
                    items = idItems
                End If
            Catch ex As Exception

            End Try
            Return items
        End Function
        Private Function GetUsersIdByName(searchBy As String, idUsers As List(Of Integer)) As List(Of Integer)
            Dim items As New List(Of Integer)

            Try
                If Not String.IsNullOrEmpty(searchBy) Then
                    Dim pageSize As Integer = 100
                    Dim pageIndex As Integer = 0
                    Dim paged As List(Of Integer) = idUsers.Skip(pageIndex).Take(pageSize).ToList

                    While paged.Any
                        items.AddRange((From c In Manager.GetIQ(Of Person)() Where ((c.Name.Contains(searchBy) OrElse c.Surname.Contains(searchBy)) AndAlso paged.Contains(c.Id)) Select c.Id).ToList)
                        pageIndex += 1
                        paged = items.Skip(pageIndex * pageSize).Take(pageSize).ToList
                    End While
                Else
                    items = idUsers
                End If
            Catch ex As Exception

            End Try
            Return items
        End Function
        Private Function GetUsersByName(searchBy As String, idUsers As List(Of Integer)) As List(Of Person)
            Dim items As New List(Of Person)

            Try
                Dim pageSize As Integer = 100
                Dim pageIndex As Integer = 0
                Dim paged As List(Of Integer) = idUsers.Skip(pageIndex).Take(pageSize).ToList

                While paged.Any
                    If String.IsNullOrEmpty(searchBy) Then
                        items.AddRange((From c In Manager.GetIQ(Of Person)() Where paged.Contains(c.Id) Select c).ToList)
                    Else
                        items.AddRange((From c In Manager.GetIQ(Of Person)() Where ((c.Name.Contains(searchBy) OrElse c.Surname.Contains(searchBy)) AndAlso paged.Contains(c.Id)) Select c).ToList)
                    End If
                    pageIndex += 1
                    paged = idUsers.Skip(pageIndex * pageSize).Take(pageSize).ToList
                End While

            Catch ex As Exception

            End Try
            Return items
        End Function
    End Class
End Namespace