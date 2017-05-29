Imports lm.Comol.Modules.UserActions.Presentation

Namespace lm.Comol.Modules.UserActions.DomainModel
    Public Class RootObject
        Private Shared _BasePath As String = "Modules/ModulesStatistic/"
       
        Public Shared ReadOnly Property MySystemStatistics() As String
            Get
                Return _BasePath & "MySystemStatistics.aspx"
            End Get
        End Property
        Public Shared ReadOnly Property MySystemStatistics(idPageIndex As Integer, order As StatisticOrder, ascending As Boolean) As String
            Get
                Return String.Format(MySystemStatistics(order, ascending), IIf(idPageIndex > 0, idPageIndex.ToString, "0"))
            End Get
        End Property
        Public Shared ReadOnly Property MySystemStatistics(order As StatisticOrder, ascending As Boolean) As String
            Get
                If order = StatisticOrder.None Then
                    order = StatisticOrder.UsageTime
                End If
                Return MySystemStatistics() & "?Page={0}&Order=" & order.ToString & "&Ascending=" & ascending.ToString
            End Get
        End Property

        Public Shared ReadOnly Property UsersSystemStatistics() As String
            Get
                Return _BasePath & "UsersSystemStatistics.aspx"
            End Get
        End Property
        Public Shared ReadOnly Property UsersSystemStatistics(idPageIndex As Integer, searchBy As String, order As StatisticOrder, ascending As Boolean) As String
            Get
                Return String.Format(UsersSystemStatistics(searchBy, order, ascending), IIf(idPageIndex > 0, idPageIndex.ToString, "0"))
            End Get
        End Property
        Public Shared ReadOnly Property UsersSystemStatistics(searchBy As String, order As StatisticOrder, ascending As Boolean) As String
            Get
                If order = StatisticOrder.None Then
                    order = StatisticOrder.UsageTime
                End If
                Return UsersSystemStatistics() & "?Page={0}&Order=" & order.ToString & "&Ascending=" & ascending.ToString & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy)
            End Get
        End Property

        Public Shared ReadOnly Property SystemStatistics() As String
            Get
                Return _BasePath & "CommunitySystemStatistics.aspx"
            End Get
        End Property
        Public Shared ReadOnly Property SystemStatistics(idPageIndex As Integer, searchBy As String, order As StatisticOrder, ascending As Boolean) As String
            Get
                If idPageIndex < 0 Then
                    idPageIndex = 0
                End If
                Return String.Format(SystemStatistics(searchBy, order, ascending), IIf(idPageIndex > 0, idPageIndex.ToString, "0"))
            End Get
        End Property
        Public Shared ReadOnly Property SystemStatistics(searchBy As String, order As StatisticOrder, ascending As Boolean) As String
            Get
                If order = StatisticOrder.None Then
                    order = StatisticOrder.UsageTime
                End If
                Return SystemStatistics() & "?Page={0}&Order=" & order.ToString & "&Ascending=" & ascending.ToString & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy)
            End Get
        End Property

        'Public Shared ReadOnly Property MyCommunityStatistics() As String
        '    Get
        '        Return _BasePath & "MyCommunityStatistics.aspx"
        '    End Get
        'End Property
        'Public Shared ReadOnly Property MyCommunityStatistics(idPageIndex As Integer, ByVal idUser As Integer, idCommunity As Integer, order As StatisticOrder, ascending As Boolean, ByVal fromV As StatisticView) As String
        '    Get
        '        If order = StatisticOrder.None Then
        '            order = StatisticOrder.ModuleName
        '        End If
        '        Return String.Format(MyCommunityStatistics(idUser, idCommunity, order, ascending, fromV), IIf(idPageIndex > 0, idPageIndex.ToString, "0"))
        '    End Get
        'End Property
        'Public Shared ReadOnly Property MyCommunityStatistics(ByVal idUser As Integer, idCommunity As Integer, order As StatisticOrder, ascending As Boolean, ByVal fromV As StatisticView) As String
        '    Get
        '        If order = StatisticOrder.None Then
        '            order = StatisticOrder.ModuleName
        '        End If
        '        Return MyCommunityStatistics() & "?Page={0}&Order=" & order.ToString & "&Ascending=" & ascending.ToString & IIf(idUser > 0, "&IdUser=" & idUser.ToString, "") & "&idCommunity=" & idCommunity & "&from=" & fromV.ToString
        '    End Get
        'End Property

        Public Shared ReadOnly Property UserSystemStatistics(ByVal idUser As Integer, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                Dim url As String = _BasePath & "UserSystemStatistics.aspx"
                If order = StatisticOrder.None Then
                    order = StatisticOrder.UsageTime
                    ascending = False
                End If
                Return url & "?Page={0}&IdUser=" & idUser.ToString & "&Order=" & order.ToString & "&Ascending=" & ascending.ToString & "&from=" & fromV.ToString & "&BackUrl=" & backUrl
            End Get
        End Property
        Public Shared ReadOnly Property UserSystemStatistics(ByVal idUser As Integer, searchBy As String, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                Return UserSystemStatistics(idUser, order, ascending, backUrl, fromV) & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy)
            End Get
        End Property
        Public Shared ReadOnly Property UserSystemStatistics(idPageIndex As Integer, ByVal idUser As Integer, searchBy As String, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                If idPageIndex < 0 Then
                    idPageIndex = 0
                End If
                Return String.Format(UserSystemStatistics(idUser, order, ascending, backUrl, fromV) & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy), idPageIndex.ToString())
            End Get
        End Property

        Public Shared ReadOnly Property UserSystemCommunityStatistics(ByVal idUser As Integer, ByVal idCommunity As Integer, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                Dim url As String = _BasePath & "UserSystemCommunityStatistics.aspx"
                If order = StatisticOrder.None Then
                    order = StatisticOrder.ModuleName
                End If
                Return url & "?Page={0}&IdUser=" & idUser.ToString & "&idCommunity=" & idCommunity.ToString & "&Order=" & order.ToString & "&Ascending=" & ascending.ToString & "&from=" & fromV.ToString & "&BackUrl=" & backUrl
            End Get
        End Property
        Public Shared ReadOnly Property UserSystemCommunityStatistics(ByVal idPageIndex As Integer, ByVal idUser As Integer, ByVal idCommunity As Integer, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                If idPageIndex < 0 Then
                    idPageIndex = 0
                End If

                Return String.Format(UserSystemCommunityStatistics(idUser, idCommunity, order, ascending, backUrl, fromV), idPageIndex.ToString)
            End Get
        End Property
        Public Shared ReadOnly Property UsageDetails() As String
            Get
                Return _BasePath & "StatisticDetails.aspx"
            End Get
        End Property
        Public Shared ReadOnly Property UsageDetails(ByVal idUser As Integer, idCommunity As Integer, detailType As DetailViewType, ByVal from As StatisticView, ByVal backUrl As String) As String
            Get
                Dim url As String = UsageDetails()
                If detailType = DetailViewType.None Then
                    detailType = DetailViewType.Month
                End If

                Return url & "?&Show=" & detailType.ToString & IIf(idUser > 0, "&IdUser=" & idUser.ToString, "") & "&idCommunity=" & idCommunity & "&from=" & from.ToString & "&BackUrl=" & backUrl
            End Get
        End Property
        Public Shared ReadOnly Property UsageDetails(ByVal idModule As Integer, ByVal idUser As Integer, idCommunity As Integer, detailType As DetailViewType, ByVal from As StatisticView, ByVal backUrl As String) As String
            Get
                Return UsageDetails(idUser, idCommunity, detailType, from, backUrl) & IIf(idModule > 0, "&IdModule=" & idModule.ToString, "")
            End Get
        End Property


        Public Shared ReadOnly Property MyCommunityStatistics() As String
            Get
                Return _BasePath & "MyCommunityStatistics.aspx"
            End Get
        End Property
        Public Shared ReadOnly Property MyCommunityStatistics(ByVal idPageIndex As Integer, ByVal idCommunity As Integer, order As StatisticOrder, ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                Return String.Format(MyCommunityStatistics(idCommunity, order, ascending, fromV), IIf(idPageIndex > 0, idPageIndex.ToString, "0"))
            End Get
        End Property
        Public Shared ReadOnly Property MyCommunityStatistics(ByVal idCommunity As Integer, order As StatisticOrder, ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                If order = StatisticOrder.None Then
                    order = StatisticOrder.ModuleName
                End If
                Return MyCommunityStatistics() & "?Page={0}&Order=" & order.ToString & "&Ascending=" & ascending.ToString & IIf(idCommunity > 0, "&IdCommunity=" & idCommunity.ToString, "") & "&from=" & fromV.ToString
            End Get
        End Property

        Public Shared ReadOnly Property UsersCommunity(ByVal idCommunity As Integer, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                Dim url As String = _BasePath & "UsersCommunity.aspx"
                If order = StatisticOrder.None Then
                    order = StatisticOrder.Owner
                    ascending = False
                End If
                Return url & "?Page={0}&IdCommunity=" & idCommunity.ToString & "&Order=" & order.ToString & "&Ascending=" & ascending.ToString & "&from=" & fromV.ToString
            End Get
        End Property
        Public Shared ReadOnly Property UsersCommunity(ByVal idCommunity As Integer, searchBy As String, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                Return UsersCommunity(idCommunity, order, ascending, fromV) & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy)
            End Get
        End Property
        Public Shared ReadOnly Property UsersCommunity(idPageIndex As Integer, ByVal idCommunity As Integer, searchBy As String, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                If idPageIndex < 0 Then
                    idPageIndex = 0
                End If
                Return String.Format(UsersCommunity(idCommunity, order, ascending, fromV) & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy), idPageIndex.ToString())
            End Get
        End Property
        Public Shared ReadOnly Property CommunityStatistics() As String
            Get
                Return _BasePath & "CommunityStatistics.aspx"
            End Get
        End Property
        Public Shared ReadOnly Property CommunityStatistics(idPageIndex As Integer, ByVal idCommunity As Integer, searchBy As String, order As StatisticOrder, ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                If idPageIndex < 0 Then
                    idPageIndex = 0
                End If
                Return String.Format(CommunityStatistics(idCommunity, searchBy, order, ascending, fromV), IIf(idPageIndex > 0, idPageIndex.ToString, "0"))
            End Get
        End Property
        Public Shared ReadOnly Property CommunityStatistics(ByVal idCommunity As Integer, searchBy As String, order As StatisticOrder, ascending As Boolean, ByVal fromV As StatisticView) As String
            Get
                If order = StatisticOrder.None Then
                    order = StatisticOrder.ModuleName
                End If
                Return CommunityStatistics() & "?Page={0}&Order=" & order.ToString & "&Ascending=" & ascending.ToString & IIf(String.IsNullOrEmpty(searchBy), "", "&SearchBy=" & searchBy) & IIf(idCommunity > 0, "&IdCommunity=" & idCommunity.ToString, "") & "&from=" & fromV.ToString
            End Get
        End Property

        Public Shared ReadOnly Property UserCommunity(ByVal idUser As Integer, ByVal idCommunity As Integer, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                Dim url As String = _BasePath & "UserCommunity.aspx"
                If order = StatisticOrder.None Then
                    order = StatisticOrder.UsageTime
                    ascending = False
                End If
                Return url & "?Page={0}&IdUser=" & idUser.ToString & "&Order=" & order.ToString & "&Ascending=" & ascending.ToString & "&from=" & fromV.ToString & "&BackUrl=" & backUrl
            End Get
        End Property
        Public Shared ReadOnly Property UserCommunity(idPageIndex As Integer, ByVal idUser As Integer, ByVal idCommunity As Integer, ByVal order As StatisticOrder, ByVal ascending As Boolean, ByVal backUrl As String, ByVal fromV As StatisticView) As String
            Get
                If idPageIndex < 0 Then
                    idPageIndex = 0
                End If
                Return String.Format(UserCommunity(idUser, idCommunity, order, ascending, backUrl, fromV), idPageIndex.ToString())
            End Get
        End Property

        'Private Function GetBaseStatisticUrl(ByVal oDestinationPage As ViewPage, ByVal oContext As UsageContext, ByVal oDestinationView As IviewUsageStatistic.viewType, ByVal oFromView As IviewUsageStatistic.viewType, ByVal startFrom As IviewUsageStatistic.viewType) As String
        '    Dim url As String = "?"
        '    If oContext.UserID > 0 Then
        '        url &= "&UserID=" & oContext.UserID.ToString
        '    End If
        '    If oContext.CommunityID > 0 Then
        '        url &= "&CommunityID=" & oContext.CommunityID
        '    End If
        '    If oContext.ModuleID > 0 Then
        '        url &= "&ModuleID=" & oContext.ModuleID
        '    End If
        '    If oContext.Order <> StatisticOrder.None Then
        '        url &= "&Order=" & oContext.Order.ToString
        '    End If
        '    If oContext.Ascending Then
        '        url &= "&Dir=asc"
        '    Else
        '        url &= "&Dir=desc"
        '    End If
        '    If oDestinationView <> IviewUsageStatistic.viewType.None Then
        '        url &= "&View=" & oDestinationView.ToString
        '    End If
        '    If oFromView <> IviewUsageStatistic.viewType.None Then
        '        url &= "&From=" & oFromView.ToString
        '    End If
        '    If startFrom <> IviewUsageStatistic.viewType.None Then
        '        url &= "&StartFrom=" & startFrom.ToString
        '    Else
        '        url &= "&StartFrom=" & CurrentView.ToString
        '    End If
        '    If url.StartsWith("?&") Then
        '        url = url.Replace("?&", "?")
        '    End If
        '    If oDestinationPage <> ViewPage.None Then
        '        Select Case oDestinationPage
        '            Case ViewPage.TimeDetails
        '                url = Me.BaseUrl & "Statistiche_Servizi/UsageDetails.aspx" & url & "&BackUrl=" & DestinationUrl
        '            Case ViewPage.Community
        '                url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
        '            Case ViewPage.System
        '                url = Me.BaseUrl & "Statistiche_Servizi/UsageGlobal.aspx" & url
        '            Case ViewPage.CurrentPage
        '                url = Me.Request.Url.AbsolutePath & url
        '            Case ViewPage.OnLineUsers
        '                url = Me.BaseUrl & "Statistiche_Servizi/OnLineUsers.aspx" & url
        '        End Select
        '    End If

        '    Return url
        'End Function
    End Class
End Namespace