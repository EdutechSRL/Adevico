Namespace Business
    Public Class CachePolicy

#Region "Const Wiki"
        Private Const _AvailableModules As String = "RMNT_AvailableModules{0}"
        Private Const _WeekDays As String = "RMNT_WeekDays{0}{1}"
        Private Const _MonthDays As String = "RMNT_MonthDays{0}{1}"
        Private Const _CommunityNewsSummary As String = "RMNT_CommunityNewsSummary{0}{1}{2}"
        Private Const _CommunityNews As String = "RMNT_CommunityNews{0}{1}{2}{3}{4}"
        Private Const _AllNewsinfo As String = "RMNT_AllNewsinfo{0}{1}"
#End Region


#Region "New"
        Public Shared Function AllNewsinfo() As String
            Return String.Format(_AllNewsinfo, "", "")
        End Function
        Public Shared Function AllNewsinfo(ByVal PersonID As Integer) As String
            Return String.Format(_AllNewsinfo, "_" & PersonID.ToString, "")
        End Function
        Public Shared Function AllNewsinfo(ByVal PersonID As Integer, ByVal CommunityID As Integer) As String
            Return String.Format(_AllNewsinfo, "_" & PersonID.ToString, "_" & CommunityID.ToString)
        End Function
        Public Shared Function AvailableModules() As String
            Return String.Format(_AvailableModules, "")
        End Function
        Public Shared Function AvailableModules(ByVal LanguageID As Integer) As String
            Return String.Format(_AvailableModules, "_" & LanguageID.ToString)
        End Function
        Public Shared Function WeekDays() As String
            Return String.Format(_WeekDays, "", "")
        End Function
        Public Shared Function WeekDays(ByVal PersonID As Integer) As String
            Return String.Format(_WeekDays, "_" & PersonID.ToString, "")
        End Function
        Public Shared Function WeekDays(ByVal PersonID As Integer, ByVal CommunityID As Integer) As String
            Return String.Format(_WeekDays, "_" & PersonID.ToString, "_" & CommunityID.ToString)
        End Function

        Public Shared Function MonthDays() As String
            Return String.Format(_MonthDays, "", "")
        End Function
        Public Shared Function MonthDays(ByVal PersonID As Integer) As String
            Return String.Format(_MonthDays, "_" & PersonID.ToString, "")
        End Function
        Public Shared Function MonthDays(ByVal PersonID As Integer, ByVal CommunityID As Integer) As String
            Return String.Format(_MonthDays, "_" & PersonID.ToString, "_" & CommunityID.ToString)
        End Function

        Public Shared Function CommunityNewsSummary() As String
            Return String.Format(_CommunityNewsSummary, "", "", "")
        End Function
        Public Shared Function CommunityNewsSummary(ByVal PersonID As Integer) As String
            Return String.Format(_CommunityNewsSummary, "_" & PersonID.ToString, "", "")
        End Function
        Public Shared Function CommunityNewsSummary(ByVal PersonID As Integer, ByVal CommunityID As Integer) As String
            Return String.Format(_CommunityNewsSummary, "_" & PersonID.ToString, "_" & CommunityID.ToString, "")
        End Function
        Public Shared Function CommunityNewsSummary(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal Day As Date) As String
            Return String.Format(_CommunityNewsSummary, "_" & PersonID.ToString, "_" & CommunityID.ToString, "_" & Day.ToString)
        End Function


        Public Shared Function CommunityNews() As String
            Return String.Format(_CommunityNews, "", "", "", "", "")
        End Function
        Public Shared Function CommunityNews(ByVal PersonID As Integer) As String
            Return String.Format(_CommunityNews, "_" & PersonID.ToString, "", "", "", "")
        End Function
        Public Shared Function CommunityNews(ByVal PersonID As Integer, ByVal CommunityID As Integer) As String
            Return String.Format(_CommunityNews, "_" & PersonID.ToString, "_" & CommunityID.ToString, "", "", "")
        End Function
        Public Shared Function CommunityNews(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal Day As Date) As String
            Return String.Format(_CommunityNews, "_" & PersonID.ToString, "_" & CommunityID.ToString, "_" & Day.ToString)
        End Function
        Public Shared Function CommunityNews(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal Day As Date, ByVal PageIndex As Integer, ByVal PageSize As Integer) As String
            Return String.Format(_CommunityNews, "_" & PersonID.ToString, "_" & CommunityID.ToString, "_" & Day.ToString, "_" & PageIndex.ToString, "_" & PageSize.ToString)
        End Function
#End Region

    End Class
End Namespace