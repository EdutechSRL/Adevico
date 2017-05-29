Namespace lm.Comol.Modules.UsageResults.BusinessLogic
    Public Class CachePolicy

#Region "Const Wiki"
        Private Const _FindUserCommunities As String = "FindUserCommunities{0}"
        Private Const _FindPortalUsers As String = "FindPortalUsers"
        Private Const _FindCommunityUsers As String = "FindCommunityUsers{0}"
        Private Const _GetPortalUsageResults As String = "GetPortalUsageResults{0}{1}{2}"
        Private Const _GetCommunityUsageResults As String = "GetCommunityUsageResults{0}{1}{2}{3}"

        Private Const _FindCommunityUsersResultsBetweenDate As String = "FindCommunityUsersResultsBetweenDate{0}{1}{2}{3}{4}"
        Private Const _FindPortalUsersResultsBetweenDate As String = "FindPortalUsersResultsBetweenDate{0}{1}{2}{3}"

        Private Const _FindCommunityUsersOnLine As String = "OLU_FindCommunityUsersOnLine{0}"
#End Region


#Region "New"
        Public Shared Function FindCommunityUsersOnLine() As String
            Return String.Format(_FindCommunityUsersOnLine, "")
        End Function
        Public Shared Function FindCommunityUsersOnLine(ByVal CommunityID As Integer) As String
            Return String.Format(_FindCommunityUsersOnLine, "_" & CommunityID.ToString)
        End Function

        Public Shared Function FindUserCommunities() As String
            Return String.Format(_FindUserCommunities, "")
        End Function
        Public Shared Function FindUserCommunities(ByVal PersonID As Integer) As String
            Return String.Format(_FindUserCommunities, "_" & PersonID.ToString)
        End Function
        Public Shared Function FindPortalUsers() As String
            Return _FindPortalUsers
        End Function
        Public Shared Function FindCommunityUsers() As String
            Return String.Format(_FindCommunityUsers, "")
        End Function
        Public Shared Function FindCommunityUsers(ByVal CommunityID As Integer) As String
            Return String.Format(_FindCommunityUsers, "_" & CommunityID.ToString)
        End Function

        Public Shared Function GetCommunityUsageResults() As String
            Return String.Format(_GetCommunityUsageResults, "", "", "", "")
        End Function
        Public Shared Function GetCommunityUsageResults(ByVal CommunityID As Integer) As String
            Return String.Format(_GetCommunityUsageResults, "_" & CommunityID.ToString, "", "", "")
        End Function
        Public Shared Function GetCommunityUsageResults(ByVal CommunityID As Integer, ByVal PersonID As Integer) As String
            Return String.Format(_GetCommunityUsageResults, "_" & CommunityID.ToString, "_" & PersonID.ToString, "", "")
        End Function
        Public Shared Function GetCommunityUsageResults(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal From As Date) As String
            Return String.Format(_GetCommunityUsageResults, "_" & CommunityID.ToString, "_" & PersonID.ToString, "_" & From.ToString, "")
        End Function
        Public Shared Function GetCommunityUsageResults(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal From As Date, ByVal ToDate As Date) As String
            Return String.Format(_GetCommunityUsageResults, "_" & CommunityID.ToString, "_" & PersonID.ToString, "_" & From.ToString, "_" & ToDate.ToString)
        End Function


        Public Shared Function GetPortalUsageResults() As String
            Return String.Format(_GetPortalUsageResults, "", "", "")
        End Function
        Public Shared Function GetPortalUsageResults(ByVal PersonID As Integer) As String
            Return String.Format(_GetPortalUsageResults, "_" & PersonID.ToString, "", "")
        End Function
        Public Shared Function GetPortalUsageResults(ByVal PersonID As Integer, ByVal FromDate As Date) As String
            Return String.Format(_GetPortalUsageResults, "_" & PersonID.ToString, "_" & FromDate.ToString, "")
        End Function
        Public Shared Function GetPortalUsageResults(ByVal PersonID As Integer, ByVal FromDate As Date, ByVal EndDate As Date) As String
            Return String.Format(_GetPortalUsageResults, "_" & PersonID.ToString, "_" & FromDate.ToString, "_" & EndDate.ToString)
        End Function


        Public Shared Function FindCommunityUsersResultsBetweenDate() As String
            Return String.Format(_FindCommunityUsersResultsBetweenDate, "", "", "", "", "")
        End Function
        Public Shared Function FindCommunityUsersResultsBetweenDate(ByVal SearcherID As Integer) As String
            Return String.Format(_FindCommunityUsersResultsBetweenDate, "_" & SearcherID.ToString, "", "", "", "")
        End Function
        Public Shared Function FindCommunityUsersResultsBetweenDate(ByVal SearcherID As Integer, ByVal CommunityID As Integer) As String
            Return String.Format(_FindCommunityUsersResultsBetweenDate, "_" & SearcherID.ToString, "_" & CommunityID.ToString, "", "", "")
        End Function

        Public Shared Function FindCommunityUsersResultsBetweenDate(ByVal SearcherID As Integer, ByVal CommunityID As Integer, ByVal From As Date, ByVal ToDate As Date) As String
            Return String.Format(_FindCommunityUsersResultsBetweenDate, "_" & SearcherID.ToString, "_" & CommunityID.ToString, "_" & From.ToString, "_" & ToDate.ToString, "")
        End Function
        Public Shared Function FindCommunityUsersResultsBetweenDate(ByVal SearcherID As Integer, ByVal CommunityID As Integer, ByVal From As Date, ByVal ToDate As Date, ByVal Name As String) As String
            Return String.Format(_FindCommunityUsersResultsBetweenDate, "_" & SearcherID.ToString, "_" & CommunityID.ToString, "_" & From.ToString, "_" & ToDate.ToString, "_" & Name)
        End Function

        Public Shared Function FindPortalUsersResultsBetweenDate() As String
            Return String.Format(_FindPortalUsersResultsBetweenDate, "", "", "", "")
        End Function
        Public Shared Function FindPortalUsersResultsBetweenDate(ByVal SearcherID As Integer) As String
            Return String.Format(_FindPortalUsersResultsBetweenDate, "_" & SearcherID.ToString, "", "", "")
        End Function
        Public Shared Function FindPortalUsersResultsBetweenDate(ByVal SearcherID As Integer, ByVal From As Date, ByVal ToDate As Date) As String
            Return String.Format(_FindPortalUsersResultsBetweenDate, "_" & SearcherID.ToString, "_" & From.ToString, "_" & ToDate.ToString, "")
        End Function
        Public Shared Function FindPortalUsersResultsBetweenDate(ByVal SearcherID As Integer, ByVal From As Date, ByVal ToDate As Date, ByVal Name As String) As String
            Return String.Format(_FindPortalUsersResultsBetweenDate, "_" & SearcherID.ToString, "_" & From.ToString, "_" & ToDate.ToString, "_" & Name)
        End Function
#End Region

    End Class
End Namespace