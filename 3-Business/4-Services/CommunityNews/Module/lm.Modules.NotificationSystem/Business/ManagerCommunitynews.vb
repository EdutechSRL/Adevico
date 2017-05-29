Imports lm.Comol.Core.DomainModel
Imports lm.Modules.NotificationSystem.Presentation
Imports NHibernate
Imports NHibernate.Linq
Imports lm.Modules.NotificationSystem.WSremoteManagement
Imports lm.Modules.NotificationSystem.Domain


Namespace Business
    Public Class ManagerCommunitynews
        Inherits ManagerBase

        Private _ServiceManagement As WSremoteManagement.NotificationManagementSoapClient
        Private ReadOnly Property ServiceManagement() As WSremoteManagement.NotificationManagementSoapClient
            Get
                If IsNothing(_ServiceManagement) Then
                    _ServiceManagement = New WSremoteManagement.NotificationManagementSoapClient
                End If
                Return _ServiceManagement
            End Get
        End Property

        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal oContext As iApplicationContext)
            MyBase.New(oContext)
        End Sub
        Public Sub New(ByVal oUserContext As iUserContext, ByVal oDatacontext As iDataContext)
            MyBase.New(oUserContext, oDatacontext)
        End Sub

        Public Function GetWeekDayNewsFromNowToPrevoius(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal TodayName As String) As List(Of dtoDay)
            Dim iResponse As List(Of dtoDay) = (From d In GetWeekDayNews(PersonID, CommunityID) Order By d.Day Descending Select d).ToList

            If iResponse.Count > 0 Then
                iResponse(0).DayName = TodayName
            End If
            Return iResponse
        End Function
        Public Function GetWeekDayNews(ByVal PersonID As Integer, ByVal CommunityID As Integer) As List(Of dtoDay)
            Dim Today As Date = Now.Date
            Dim iResponse As List(Of dtoDay) = CreateDays(Today, 7)

            Dim cacheKey As String = CachePolicy.WeekDays(PersonID, CommunityID)
            If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oRemoteDays As List(Of DateTime) = Me.ServiceManagement.GetWeekDaysWithNews(Today, PersonID, CommunityID)
                If oRemoteDays.Count > 0 Then
                    For Each oDate As Date In oRemoteDays
                        Dim oDay As dtoDay
                        Dim currentDate As Date = oDate
                        oDay = (From d In iResponse Where d.Day = currentDate Select d).FirstOrDefault
                        If Not IsNothing(oDay) Then
                            oDay.Enabled = True
                        End If
                    Next
                End If

                COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, iResponse, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                iResponse = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of dtoDay))
            End If
            Return iResponse
        End Function
        Public Function GetMonthDayNews(ByVal PersonID As Integer, ByVal CommunityID As Integer) As List(Of dtoDay)
            Dim Today As Date = Now.Date
            Dim iResponse As List(Of dtoDay) = CreateDays(Today, 30)

            Dim cacheKey As String = CachePolicy.MonthDays(PersonID, CommunityID)
            If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oRemoteDays As List(Of DateTime) = Me.ServiceManagement.GetMonthDaysWithNews(Today, PersonID, CommunityID)
                If oRemoteDays.Count > 0 Then
                    For Each oDate As Date In oRemoteDays
                        Dim oDay As dtoDay
                        Dim currentDate As Date = oDate
                        oDay = (From d In iResponse Where d.Day = currentDate Select d).FirstOrDefault
                        If Not IsNothing(oDay) Then
                            oDay.Enabled = True
                        End If
                    Next
                End If

                COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, iResponse, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                iResponse = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of dtoDay))
            End If
            Return iResponse
        End Function
        Private Function CreateDays(ByVal EndDate As Date, ByVal Daynumbers As Integer) As List(Of dtoDay)
            Dim iResponse As New List(Of dtoDay)
            If Daynumbers <= 1 Then
                iResponse.Add(New dtoDay(EndDate))
            Else
                For i = Daynumbers To 1 Step -1
                    iResponse.Add(New dtoDay(EndDate.AddDays(1 - i)))
                Next
            End If
            Return iResponse
        End Function

        Public Function GetSummaryNews(ByVal oDay As Date, ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer, ByRef oPager As PagerBase, ByVal CurrentPage As Integer) As List(Of dtoCommunitySummaryNews)
            Dim oList As List(Of dtoCommunitySummaryNews)

            Dim cacheKey As String = CachePolicy.CommunityNewsSummary(PersonID, CommunityID, oDay)
            If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oRemote As List(Of WSremoteManagement.dtoCommunitySummaryNotification) = Me.ServiceManagement.GetCommunitySummary(oDay, PersonID, CommunityID, LanguageID)
                oList = New List(Of dtoCommunitySummaryNews)
                If oRemote.Count > 0 Then
                    Dim oLocalCommunities As List(Of Community) = GetCommunitiesSubscribed(PersonID, (From rm As WSremoteManagement.dtoCommunitySummaryNotification In oRemote Where rm.ID > 0 Select rm.ID).Distinct.ToList, OrderSubscriptionBy.LastAccess)
                    Dim oLocalModules As List(Of COL_BusinessLogic_v2.PlainService) = MyBase.GetGenericModuleList

                    oList = (From c In oLocalCommunities Select New dtoCommunitySummaryNews() With {.ID = c.Id, .Name = c.Name, .News = Nothing}).ToList
                    If oList.Count > 0 Then
                        For Each t In oList
                            Dim p As dtoCommunitySummaryNews = t
                            p.News = (From rm In oRemote Join m In oLocalModules On rm.ModuleID Equals m.ID Where rm.ID = p.ID Select New dtoSummaryNews() With {.ModuleID = rm.ID, .NewsCount = rm.Count, .ModuleName = m.Name}).ToList
                        Next
                    End If

                    If oRemote.Where(Function(c) c.ID = 0).Count > 0 Then
                        Dim Portal As New dtoCommunitySummaryNews
                        Portal.ID = 0
                        Portal.Name = ""
                        Portal.News = (From rm In oRemote Join m In oLocalModules On rm.ModuleID Equals m.ID Where rm.ID = 0 Select New dtoSummaryNews() With {.ModuleID = rm.ID, .NewsCount = rm.Count, .ModuleName = m.Name}).ToList
                        oList.Insert(0, Portal)
                    End If
                End If
                If oDay = Now.Date OrElse oDay < Now.Date Then
                    COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
                Else
                    COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30secondi)
                End If
            Else
                oList = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of dtoCommunitySummaryNews))
            End If

            oPager.Count = oList.Count - 1
            oPager.PageIndex = CurrentPage
            If oList.Count > oPager.PageSize Then
                oList = oList.Skip(oPager.Skip).Take(oPager.PageSize)
            End If
            Return oList
        End Function

        Public Function GetCommunityNews(ByVal oDay As Date, ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer, ByRef oPager As PagerBase, ByVal CurrentPage As Integer) As List(Of dtoModuleNews)
            Dim oList As List(Of dtoModuleNews)
            Dim cacheKey As String = CachePolicy.CommunityNews(PersonID, CommunityID, oDay, CurrentPage, oPager.PageSize)

            If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oRemote As List(Of CommunityNews) = Me.ServiceManagement.GetCommunityNews(False, oDay, PersonID, CommunityID, LanguageID, LanguageID, oPager.PageSize, CurrentPage)

                If oRemote.Count > 0 Then
                    Dim ModuleID As Integer
                    Dim oLocalModules As List(Of COL_BusinessLogic_v2.PlainService) = MyBase.GetGenericModuleList
                    Dim oRemoteModules As List(Of COL_BusinessLogic_v2.PlainService)
                    oRemoteModules = (From rm In oRemote Join m In oLocalModules On rm.ModuleID Equals m.ID _
                      Select m Distinct).ToList

                    oList = New List(Of dtoModuleNews)

                    For Each rm In oRemoteModules
                        ModuleID = rm.ID
                        oList.Add(New dtoModuleNews(rm, CommunityID) With {.News = (From r In oRemote Where r.ModuleID = ModuleID Order By r.SentDate Descending Select New dtoModuleMessage(r.UniqueID, r.SentDate, r.Day, r.Message)).ToList})
                    Next
                Else
                    oList = New List(Of dtoModuleNews)
                End If

                If oDay = Now.Date Then
                    COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
                Else
                    COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza30minuti)
                End If
            Else
                oList = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of dtoModuleNews))
            End If
            Return oList
        End Function

        Public Function GetModulesWithNews(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal LanguageID As Integer) As List(Of dtoRemoteModule)
            Dim oList As New List(Of dtoRemoteModule)
            Dim oRemote As List(Of WSremoteManagement.dtoModule)

            If CommunityID > 0 Then
                oRemote = Me.ServiceManagement.GetCommunityModulesWithNews(PersonID, CommunityID, Now.AddDays(-30))
            Else
                oRemote = Me.ServiceManagement.GetModulesWithNews(PersonID, Now.AddDays(-30))
            End If
            If oRemote.Count > 0 Then
                Dim oLocalModules As List(Of COL_BusinessLogic_v2.PlainService) = MyBase.GetGenericModuleList
                oList = (From rm In oRemote Join lm In oLocalModules On rm.ID Equals lm.ID Select New dtoRemoteModule() With {.ModuleID = rm.ID, .ModuleName = lm.Name}).ToList
            End If
            Return oList
        End Function
        Public Function GetCommunityNewsCount(ByVal PersonID As Integer) As List(Of dtoCommunityNewsCount)
            Dim oList As List(Of dtoCommunityNewsCount) = Nothing

            Dim cacheKey As String = "CommunityNewsCount_" & PersonID.ToString
            If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                Try
                    oList = (From o In Me.ServiceManagement.GetPersonalCommunityWithNews(PersonID) Select New dtoCommunityNewsCount() With {.Count = o.ActionCount, .ID = o.CommunityID, .LastUpdate = o.LastUpdate}).ToList
                Catch ex As Exception
                    oList = New List(Of dtoCommunityNewsCount)
                End Try
                COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oList, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, CacheTime.Scadenza2minuti)
            Else
                oList = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of dtoCommunityNewsCount))
            End If

            Return oList
        End Function

        Public Function GetPortalNews(ByVal NewsFromDay As DateTime, ByVal ModuleID As Integer, ByVal oView As ViewModeFiler, ByVal PersonID As Integer, ByVal ForCommunityID As Integer, ByVal LanguageID As Integer, ByRef oPager As PagerBase, ByVal CurrentPage As Integer, ByVal PortalName As String) As List(Of dtoMultipleNews)
            Dim oList As New List(Of dtoMultipleNews)
            Dim cacheKey As String = CachePolicy.AllNewsinfo(PersonID, ForCommunityID)
            Dim FromDay As DateTime = Now.Date
            Dim oLocalInfo As List(Of lm.Comol.Modules.Base.DomainModel.dtoInfo)

            ' Recupero gli id univoci remoti per poi paginare l'enorme quantità di dati da visualizzare ....

            If NewsFromDay.Equals(DateTime.MinValue) Then
                NewsFromDay = FromDay.AddDays(-30)
            End If

            If COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey) Is Nothing Then
                Dim oRemoteInfos As List(Of dtoNewsInfo)
                oRemoteInfos = Me.ServiceManagement.GetCommunityAllNewsInfo(PersonID, ForCommunityID, NewsFromDay).ToList
                If IsNothing(oRemoteInfos) Then
                    oRemoteInfos = New List(Of dtoNewsInfo)
                End If

                Dim oLocalModules As List(Of COL_BusinessLogic_v2.PlainService) = MyBase.GetGenericModuleList
                Dim oLocalCommunities As List(Of Community) = GetCommunitiesSubscribed(PersonID, (From i As dtoNewsInfo In oRemoteInfos Where i.CommunityID > 0 Select i.CommunityID).Distinct.ToList, OrderSubscriptionBy.LastAccess)
                If oRemoteInfos.Where(Function(c) c.CommunityID = 0).Count > 0 Then
                    oLocalCommunities.Insert(0, New Community(0) With {.Name = PortalName})
                End If

                oLocalInfo = (From i As dtoNewsInfo In oRemoteInfos Join c In oLocalCommunities On i.CommunityID Equals c.Id _
                              Select New lm.Comol.Modules.Base.DomainModel.dtoInfo(i.Day, i.UniqueID, i.CommunityID, c.Name, i.ModuleID, (From m In oLocalModules Where m.ID = i.ModuleID Select m.Name).FirstOrDefault, i.SentDate)).ToList

                COL_BusinessLogic_v2.ObjectBase.Cache.Insert(cacheKey, oLocalInfo, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, IIf(ForCommunityID = -1, CacheTime.Scadenza2minuti, CacheTime.Scadenza2minuti))
            Else
                oLocalInfo = CType(COL_BusinessLogic_v2.ObjectBase.Cache(cacheKey), List(Of lm.Comol.Modules.Base.DomainModel.dtoInfo))
            End If

            If oLocalInfo.Count > 0 Then
                Dim oInfoToRetrieve As List(Of lm.Comol.Modules.Base.DomainModel.dtoInfo)
                Dim oQuery As IEnumerable(Of lm.Comol.Modules.Base.DomainModel.dtoInfo)
                If oView = ViewModeFiler.ByDate Then
                    oQuery = (From o In oLocalInfo Where (ModuleID = -1 OrElse o.ModuleID = ModuleID) AndAlso o.SentDate >= NewsFromDay).OrderByDescending(Function(t) t.Day).ThenBy(Function(t) t.CommunityName)
                Else
                    oQuery = (From o In oLocalInfo Where (ModuleID = -1 OrElse o.ModuleID = ModuleID) AndAlso o.SentDate >= NewsFromDay).OrderBy(Function(t) t.CommunityName).ThenByDescending(Function(t) t.Day)
                End If

                oPager.Count = oQuery.Count - 1
                oPager.PageIndex = CurrentPage

                If oLocalInfo.Count > oPager.PageSize Then
                    oInfoToRetrieve = (From i In oQuery Select i).Skip(oPager.Skip).Take(oPager.PageSize).ToList
                Else
                    oInfoToRetrieve = (From i In oQuery Select i).ToList
                End If

                Dim ToRetrieve As New ArrayOfGuid
                ToRetrieve.AddRange((From o In oInfoToRetrieve Select o.UniqueID).Distinct.ToArray)
                Dim oNotifications As List(Of CommunityNews) = Me.ServiceManagement.GetNotifications(ToRetrieve, LanguageID, LanguageID)

                If oNotifications.Count > 0 Then
                    If oView = ViewModeFiler.ByDate Then
                        Dim oAnonymousDate = (From info In oInfoToRetrieve Order By info.Day Descending Group info By Key = info.Day _
                                                   Into Group Where Group.Count > 0 Select New With {.Day = Key}).ToList

                        For Each obj In oAnonymousDate
                            Dim oDtoMultipleNews As New dtoMultipleNews
                            Dim Day As Date = obj.Day

                            oDtoMultipleNews.ID = -9
                            oDtoMultipleNews.Name = Day.ToLongDateString

                            Dim oQu = (From info In oInfoToRetrieve Where info.Day = Day Order By info.CommunityName Group info By Key = info.CommunityID Into Group Select New With {.CommunityID = Key, .Name = Group(0).CommunityName, .Messages = Group})
                            oDtoMultipleNews.Multiples = (From o In oQu Select New dtoNewsItem() With {.ID = o.CommunityID, .Name = o.Name, .News = (From m In o.Messages Join notification In oNotifications On m.UniqueID Equals notification.UniqueID Order By m.SentDate Descending Select New dtoModuleMessage(m.UniqueID, notification.SentDate, m.Day, notification.Message)).ToList()}).ToList
                            oList.Add(oDtoMultipleNews)
                        Next
                    Else
                        Dim oAnonymousCommunity = (From info In oInfoToRetrieve Order By info.CommunityName Group info By Key = info.CommunityID _
                                                   Into Group Where Group.Count > 0 Select New With {.ID = Key, .Name = Group(0).CommunityName}).ToList

                        'oAnonymousCommunity = (From info In oInfoToRetrieve Order By info.CommunityName Group info By Key = New With {info.CommunityID, info.CommunityName} _
                        '                           Into Group Where Group.Count > 0 Select New With {.ID = Key.CommunityID, .Name = Key.CommunityName}).ToList

                        For Each obj In oAnonymousCommunity
                            Dim oDtoMultipleNews As New dtoMultipleNews
                            Dim CommunityID As Integer = obj.ID

                            oDtoMultipleNews.ID = obj.ID
                            oDtoMultipleNews.Name = obj.Name

                            Dim oQu = (From info In oInfoToRetrieve Where info.CommunityID = CommunityID Order By info.Day Descending Group info By Key = info.Day Into Group Select New With {.Day = Key.Date, .Messages = Group})
                            oDtoMultipleNews.Multiples = (From o In oQu Select New dtoNewsItem() With {.ID = o.Day.Second, .Name = o.Day.Date, .News = (From m In o.Messages Join notification In oNotifications On m.UniqueID Equals notification.UniqueID Order By m.SentDate Descending Select New dtoModuleMessage(m.UniqueID, notification.SentDate, m.Day, notification.Message)).ToList()}).ToList
                            oList.Add(oDtoMultipleNews)
                        Next
                    End If
                End If
            End If
            Return oList
        End Function

        Public Sub ClearAllCacheItems(ByVal PersonID As Integer)

            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems("CommunityNewsCount_" & PersonID.ToString)
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.AllNewsinfo(PersonID))
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.CommunityNews(PersonID))
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.CommunityNewsSummary(PersonID))
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.MonthDays(PersonID))
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.WeekDays(PersonID))
        End Sub
        Public Sub ClearCommunityNewsCacheItems(ByVal PersonID As Integer)
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems("CommunityNewsCount_" & PersonID.ToString)
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.AllNewsinfo(PersonID))
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.CommunityNews(PersonID))
            COL_BusinessLogic_v2.ObjectBase.PurgeCacheItems(CachePolicy.CommunityNewsSummary(PersonID))
        End Sub
    End Class
End Namespace