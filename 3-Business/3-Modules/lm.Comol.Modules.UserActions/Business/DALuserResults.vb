Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports lm.Comol.Modules.UserActions.DomainModel
Imports lm.Comol.Modules.AccessResults.DomainModel
Imports lm.Comol.Modules.UsageResults.DomainModel

Public Class DALuserResults
    Private Const _databaseName As String = "userActions"
    Private Enum whereCondition
        none = 0
        modules = 1
        person = 2
        community = 4
    End Enum
    Private Enum table
        none = 0
        action = 1
        community = 2
        login = 4
        moduleAction = 8
        usage = 16
    End Enum

    'Public Shared Function GetAccessStatistics(ByVal context As ResultContext, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoAccessResult)
    '    Dim statistics As List(Of dtoUserResult)
    '    endDate = endDate.AddDays(1)
    '    If context.CommunityID > 0 Then
    '        statistics = GetCommunityAccessResults(context.UserID, context.CommunityID, startDate, endDate)
    '    Else
    '        statistics = GetPortalAccessResults(context.UserID, startDate, endDate)
    '    End If
    '    Return AnalyzeAccessStatistics(statistics, startDate, endDate)
    'End Function
    Public Shared Function GetAccessStatistics(ByVal context As ContextBase, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoAccessResult)
        Dim statistics As List(Of dtoUserResult)
        endDate = endDate.AddDays(1)
        If context.CommunityID > 0 Then
            statistics = GetCommunityAccessResults(context.UserID, context.CommunityID, startDate, endDate)
        Else
            statistics = GetPortalAccessResults(context.UserID, startDate, endDate)
        End If

        Return AnalyzeAccessStatistics(statistics, startDate, endDate)
    End Function


    Private Shared Function GetPortalAccessResults(ByVal idPerson As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoUserResult)
        Dim statistics As New List(Of dtoUserResult)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_PortalActionResult")
                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)

                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.UsageTime = sqlReader.Item("LGAC_UsageTime")
                        statistic.StartDate = sqlReader.Item("LGAC_LoginDate")
                        statistic.EndDate = sqlReader.Item("LGAC_LastActionDate")
                        statistic.IsClosed = sqlReader.Item("LGAC_SessionClosed")
                        If idPerson <= 0 Then
                            statistic.IdPerson = sqlReader.Item("LGAC_PersonID")
                        Else
                            statistic.IdPerson = idPerson
                        End If
                        statistics.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return statistics
    End Function
    Private Shared Function GetCommunityAccessResults(ByVal idPerson As Integer, ByVal idCommunity As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoUserResult)
        Dim statistics As New List(Of dtoUserResult)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CommunityActionResult")
                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)

                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.UsageTime = sqlReader.Item("CMAC_UsageTime")
                        statistic.StartDate = sqlReader.Item("CMAC_AccessDate")
                        statistic.EndDate = sqlReader.Item("CMAC_LastActionDate")
                        statistic.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
                        If idCommunity <= 0 Then
                            statistic.IdCommunity = sqlReader.Item("CMAC_CommunityID")
                        Else
                            statistic.IdCommunity = idCommunity
                        End If
                        If idPerson <= 0 Then
                            statistic.IdPerson = sqlReader.Item("CMAC_PersonID")
                        Else
                            statistic.IdPerson = idPerson
                        End If
                        statistics.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return statistics
    End Function

    Private Shared Function AnalyzeAccessStatistics(ByVal items As List(Of dtoUserResult), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoAccessResult)
        Dim result As New List(Of dtoAccessResult)

        Dim previousItems As New List(Of dtoUserResult)
        Dim afterItems As New List(Of dtoUserResult)
        Dim innerItems As New List(Of dtoUserResult)

        previousItems = (From o In items Where o.StartDate < startDate _
                Select dtoUserResult.CreateFromDate(o, GetUsageTimes(startDate, o.EndDate), New DateTime(startDate.Year, startDate.Month, startDate.Day))).ToList()

        For Each item In previousItems
            UpdateResult(item, result)
        Next

        afterItems = (From o In items Where o.EndDate >= endDate _
               Select dtoUserResult.CreateToDate(o, GetUsageTimes(o.StartDate, endDate), New DateTime(endDate.Year, endDate.Month, endDate.Day))).ToList()

        For Each item In afterItems
            UpdateResult(item, result)
        Next
        innerItems = (From o In items Where o.StartDate >= startDate AndAlso o.EndDate < endDate _
              Select o).ToList()

        For Each item In innerItems
            UpdateResult(item, result)
        Next

        Return (From o In result Order By o.Hour Select o).ToList()
    End Function
    Private Shared Sub UpdateResult(ByVal item As dtoUserResult, ByVal list As List(Of dtoAccessResult))
        Dim dto As dtoAccessResult

        dto = (From ar In list Where ar.Day.Day = item.StartDate.Day And ar.CommunityID = item.IdCommunity And ar.PersonID = item.IdPerson AndAlso ar.Hour = item.StartDate.Hour Select ar).FirstOrDefault()
        If IsNothing(dto) Then
            dto = New dtoAccessResult
            With dto
                .UsageTime = 0
                .PersonID = item.IdPerson
                .Hour = item.StartDate.Hour
                .Day = item.StartDate
                .CommunityID = item.IdCommunity
            End With
            list.Add(dto)
        End If


        If item.StartDate.Hour = item.EndDate.Hour Then
            dto.UsageTime += GetUsageTimes(item.StartDate, item.EndDate)
            If dto.UsageTime > 3600 Then
                dto.UsageTime = 3600
            End If
        Else
            Dim EndDate As DateTime = New DateTime(item.StartDate.Year, item.StartDate.Month, item.StartDate.Day, item.StartDate.Hour, 0, 0)
            EndDate = EndDate.AddHours(1)
            dto.UsageTime += GetUsageTimes(item.StartDate, EndDate)
            If dto.UsageTime > 3600 Then
                dto.UsageTime = 3600
            End If
            If EndDate < item.EndDate Then
                item.StartDate = EndDate
                UpdateResult(item, list)
            End If
        End If
    End Sub
    Private Shared Function GetUsageTimes(ByVal startDate As DateTime, ByVal endDate As DateTime) As Integer
        Dim SerialI As DateTime = New DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second)
        Dim SerialF As DateTime = New DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second)
        Return (SerialF - SerialI).TotalSeconds
    End Function


    Public Shared Function GetCommunitiesWithAccessResult(ByVal idPerson As Integer) As List(Of dtoUserResult)
        Dim items As New List(Of dtoUserResult)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_GetCommuntyWithAccessResult")
                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.IdPerson = idPerson
                        statistic.IdCommunity = sqlReader.Item("CMAC_CommunityID")
                        statistic.UsageTime = 1
                        items.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return items
    End Function
    Public Shared Function GetPortalUsersWithAccessResult() As List(Of dtoUserResult)
        Dim items As New List(Of dtoUserResult)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_GetPortalUsersWithAccessResult")
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.IdPerson = sqlReader.Item("LGAC_PersonID")
                        statistic.IdCommunity = -9999
                        statistic.UsageTime = 1
                        items.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return items
    End Function
    Public Shared Function GetUsersWithAccessResult(ByVal idCommunity As Integer) As List(Of dtoUserResult)
        Dim items As New List(Of dtoUserResult)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_GetCommunityUsersWithAccessResult")
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.IdPerson = sqlReader.Item("CMAC_PersonID")
                        statistic.IdCommunity = idCommunity
                        statistic.UsageTime = 1
                        items.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return items
    End Function


    Public Shared Function GetAccessResultsBetweenDate(ByVal context As ContextBase, ByVal startDate As DateTime, endDate As DateTime) As List(Of dtoAccessResult)
        Dim statistics As List(Of dtoUserResult)
        endDate = endDate.AddDays(1)
        If context.CommunityID > 0 Then
            statistics = GetCommunityAccessResultsBetweenDate(context.CommunityID, startDate, endDate)
        Else
            statistics = GetPortalAccessResultsBetweenDate(startDate, endDate)
        End If

        Return AnalyzeAccessStatistics(statistics, startDate, endDate)
    End Function
    Private Shared Function GetCommunityAccessResultsBetweenDate(ByVal idCommunity As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoUserResult)
        Dim statistics As New List(Of dtoUserResult)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CommunityResultsBetweenDate")
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.UsageTime = sqlReader.Item("CMAC_UsageTime")
                        statistic.StartDate = sqlReader.Item("CMAC_AccessDate")
                        statistic.EndDate = sqlReader.Item("CMAC_LastActionDate")
                        statistic.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
                        If idCommunity <= 0 Then
                            statistic.IdCommunity = sqlReader.Item("CMAC_CommunityID")
                        Else
                            statistic.IdCommunity = idCommunity
                        End If
                        statistic.IdPerson = sqlReader.Item("CMAC_PersonID")
                        statistics.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return statistics
    End Function
    Private Shared Function GetPortalAccessResultsBetweenDate(ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoUserResult)
        Dim statistics As New List(Of dtoUserResult)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_PortalResultsBetweenDate")
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.UsageTime = sqlReader.Item("LGAC_UsageTime")
                        statistic.StartDate = sqlReader.Item("LGAC_LoginDate")
                        statistic.EndDate = sqlReader.Item("LGAC_LastActionDate")
                        statistic.IsClosed = sqlReader.Item("LGAC_SessionClosed")
                        statistic.IdPerson = sqlReader.Item("LGAC_PersonID")
                        statistics.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return statistics
    End Function

    Public Shared Function GetAccessResultsBetweenDate(ByVal context As ContextBase, ByVal idUsers As List(Of Integer), ByVal startDate As DateTime, endDate As DateTime) As List(Of dtoAccessResult)
        Dim statistics As List(Of dtoUserResult)
        endDate = endDate.AddDays(1)
        If context.CommunityID > 0 Then
            statistics = GetCommunityAccessResultsBetweenDate(idUsers, context.CommunityID, startDate, endDate)
        Else
            statistics = GetPortalAccessResultsBetweenDate(idUsers, startDate, endDate)
        End If

        Return AnalyzeAccessStatistics(statistics, startDate, endDate)
    End Function
    Private Shared Function GetCommunityAccessResultsBetweenDate(ByVal idUsers As List(Of Integer), ByVal idCommunity As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoUserResult)
        Dim statistics As New List(Of dtoUserResult)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand(SQLcommunityResultsBetweenDate(idUsers))
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.UsageTime = sqlReader.Item("CMAC_UsageTime")
                        statistic.StartDate = sqlReader.Item("CMAC_AccessDate")
                        statistic.EndDate = sqlReader.Item("CMAC_LastActionDate")
                        statistic.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
                        If idCommunity <= 0 Then
                            statistic.IdCommunity = sqlReader.Item("CMAC_CommunityID")
                        Else
                            statistic.IdCommunity = idCommunity
                        End If
                        statistic.IdPerson = sqlReader.Item("CMAC_PersonID")
                        statistics.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return statistics
    End Function
    Private Shared Function GetPortalAccessResultsBetweenDate(ByVal idUsers As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoUserResult)
        Dim statistics As New List(Of dtoUserResult)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetStoredProcCommand(SQLportalResultsBetweenDate(idUsers))
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoUserResult
                        statistic.UsageTime = sqlReader.Item("LGAC_UsageTime")
                        statistic.StartDate = sqlReader.Item("LGAC_LoginDate")
                        statistic.EndDate = sqlReader.Item("LGAC_LastActionDate")
                        statistic.IsClosed = sqlReader.Item("LGAC_SessionClosed")
                        statistic.IdPerson = sqlReader.Item("LGAC_PersonID")
                        statistics.Add(statistic)
                    End While
                    sqlReader.Close()
                    sqlReader.Dispose()
                End Using
                dbCommand.Dispose()
                connection.Close()
                connection.Dispose()
            End Using
        Catch ex As Exception

        End Try
        Return statistics
    End Function
    Private Shared Function SQLcommunityResultsBetweenDate(ByVal idUsers As List(Of Integer)) As String
        Dim iResponse As String = "SELECT     CMAC_GuidSessionID, CMAC_AccessDate, CMAC_LastActionDate, CMAC_PersonID, CMAC_CommunityID, CMAC_ExitCommunity, CMAC_AccessDay,CMAC_AccessMonth, CMAC_AccessHour, CMAC_AccessYear, CMAC_UsageTime FROM dbo.COMMUNITY_ACTION "

        iResponse &= " where (CMAC_UsageTime > 0) AND (CMAC_CommunityID=@CommunityID OR @CommunityID=-1) AND ((CMAC_AccessDate >= @startDate AND CMAC_AccessDate < @endDate) OR (CMAC_LastActionDate >= @startDate AND CMAC_LastActionDate < @endDate)) "
        iResponse &= " and " & ComposeSQLList("(", "CMAC_PersonID", idUsers) & " "
        iResponse &= " order by CMAC_AccessDate asc"

        Return iResponse
    End Function
    Private Shared Function SQLportalResultsBetweenDate(ByVal idUsers As List(Of Integer)) As String
        Dim iResponse As String = "SELECT LGAC_ID, LGAC_LoginDate, LGAC_LastActionDate, LGAC_PersonID, LGAC_SessionClosed, LGAC_LoginHour, LGAC_UsageTime FROM LOGIN_ACTION"

        iResponse &= " where(LGAC_UsageTime > 0) AND ((LGAC_LoginDate >= @startDate AND LGAC_LoginDate < @endDate) OR (LGAC_LastActionDate >= @startDate AND LGAC_LastActionDate < @endDate)) "
        iResponse &= " and " & ComposeSQLList("(", "LGAC_PersonID", idUsers) & " "
        iResponse &= " order by LGAC_LoginDate asc"

        Return iResponse
    End Function
    Private Shared Function ComposeSQLList(ByVal prefix As String, ByVal parameter As String, ByVal idList As List(Of Integer)) As String
        Try
            If Not idList Is Nothing Then
                If idList.Count > 0 Then
                    Dim retval As String = prefix
                    For Each id As Integer In idList
                        retval &= " " & parameter & " = " & id.ToString & " OR "
                    Next
                    retval = retval.Substring(0, retval.Length - 3) & ")"
                    Return retval
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Dim errorMessage As String = ex.Message
            Return String.Empty
        End Try
    End Function

End Class