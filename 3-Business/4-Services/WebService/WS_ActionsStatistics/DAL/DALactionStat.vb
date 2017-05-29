Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data

Imports lm.WS.ActionStatistics.Domain
Imports lm.WS.ActionStatistics.Business
Imports lm.WS.UserAccessMonitor.DataContracts

'Imports lm.WS.ActionStatistics.Business
Namespace lm.WS.ActionStatistics.DAL
    Public Class DALactionStat

#Region "SQL"
#Region "getTotal"
        Public ReadOnly Property sp_getGlobalUsageTime() As String
            Get
                Return "SELECT sum(LGAC_UsageTime) as usageTime, count(*) as nAccesses FROM dbo.LOGIN_ACTION WHERE (LGAC_LoginDate >= @startDate AND LGAC_LoginDate <= @endDate) "
            End Get
        End Property
        Public ReadOnly Property sp_getGlobalUsageTimeByModule(ByVal ModuleIDList As List(Of Integer)) As String
            Get
                Return "SELECT sum(MDAC_UsageTime) AS usageTime, count( MDAC_UsageTime) AS nAccesses FROM dbo.MODULE_ACTION WHERE (" & ComposeSQLList("", "MDAC_ModuleID ", ModuleIDList) & " AND (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) "
            End Get
        End Property
        Public ReadOnly Property sp_getGlobalUsageTimeByCommunity(ByVal CommunityIdList As List(Of Integer)) As String
            Get
                Return "SELECT sum(CMAC_UsageTime) as usageTime, count(*) as nAccesses FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate AND CMAC_AccessDate <= @endDate) AND (" & ComposeSQLList("", "CMAC_CommunityID ", CommunityIdList)
            End Get
        End Property
#End Region
#Region "getGlobalPerson"
        Public ReadOnly Property sp_getUsersUTByCommunity(ByVal CommunityIdList As List(Of Integer)) As String
            Get
                Return "SELECT SUM(CMAC_UsageTime) AS usageTime, CMAC_PersonID as PersonID, count(CMAC_PersonID) as nAccesses FROM COMMUNITY_ACTION WHERE " & ComposeSQLList("(", "CMAC_CommunityID ", CommunityIdList) & "AND (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) GROUP BY CMAC_PersonID "
            End Get
        End Property
        Public ReadOnly Property sp_getUsersUTByModuleAndCommunity(ByVal CommunityIdList As List(Of Integer), ByVal ModuleIDList As List(Of Integer)) As String
            Get
                Return "SELECT SUM(MDAC_UsageTime) AS usageTime, MDAC_PersonID as PersonID, count(MDAC_PersonID) as nAccesses FROM MODULE_ACTION WHERE (" & ComposeSQLList("", "MDAC_ModuleID ", ModuleIDList) & ComposeSQLList("AND (", "MDAC_CommunityID ", CommunityIdList) & " AND (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) GROUP BY MDAC_PersonID "
            End Get
        End Property
        Public ReadOnly Property sp_getUsersUT() As String
            Get
                Return "SELECT SUM(LGAC_UsageTime) AS usageTime, LGAC_PersonID as PersonID, count(LGAC_PersonID) as nAccesses FROM LOGIN_ACTION WHERE (LGAC_LoginDate >= @startDate) AND (LGAC_LoginDate <= @endDate) GROUP BY LGAC_PersonID "
            End Get
        End Property
        Public ReadOnly Property sp_getUsersUTByModule(ByVal ModuleIDList As List(Of Integer)) As String
            Get
                Return "SELECT SUM(MDAC_UsageTime) AS usageTime, MDAC_PersonID as PersonID, count(MDAC_PersonID) as nAccesses FROM MODULE_ACTION  WHERE (" & ComposeSQLList("", "MDAC_ModuleID ", ModuleIDList) & " AND (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) GROUP BY MDAC_PersonID "
            End Get
        End Property
        Public Function ComposeSQLList(ByRef prefix As String, ByRef parameter As String, ByVal idList As List(Of Integer)) As String
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
#End Region
#Region "getGlobalCommunity"
        Public ReadOnly Property sp_getCommunitiesUsageTimeByCommunity(ByVal CommunityIDList As List(Of Integer))
            Get
                Return "SELECT sum(CMAC_UsageTime) as usageTime, count(CMAC_UsageTime) as nAccesses, CMAC_CommunityID as CommunityID FROM dbo.COMMUNITY_ACTION WHERE " & ComposeSQLList("(", " CMAC_CommunityID ", CommunityIDList) & " AND (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) group by CMAC_CommunityID ORDER BY usageTime"
            End Get
        End Property
        Public ReadOnly Property sp_getCommunitiesUsageTimeByPersonByCommunity(ByVal CommunityIDList As List(Of Integer), ByVal PersonIDList As List(Of Integer))
            Get
                Return "SELECT sum(CMAC_UsageTime) as usageTime, count(CMAC_UsageTime) as nAccesses, CMAC_CommunityID as CommunityID FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) " & ComposeSQLList("AND (", " CMAC_PersonID ", PersonIDList) & " group by CMAC_CommunityID " & ComposeSQLList("HAVING (", " CMAC_CommunityID ", CommunityIDList) & "ORDER BY usageTime"
            End Get
        End Property
        Public ReadOnly Property sp_getCommunitiesUsageTime()
            Get
                Return "SELECT sum(CMAC_UsageTime) as usageTime, CMAC_CommunityID AS communityID,  count(CMAC_CommunityID) as nAccesses FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) group by CMAC_CommunityID ORDER BY usageTime"
            End Get
        End Property
        Public ReadOnly Property sp_getCommunitiesByModule(ByVal CommunityIDList As List(Of Integer), ByVal PersonIDList As List(Of Integer), ByVal ModuleIDList As List(Of Integer))
            Get
                Return "SELECT sum(MDAC_UsageTime) as usageTime, count(MDAC_UsageTime) as nAccesses, MDAC_CommunityID as CommunityID FROM dbo.MODULE_ACTION WHERE (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) " & ComposeSQLList("AND (", " MDAC_PersonID ", PersonIDList) & ComposeSQLList("AND (", " MDAC_ModuleID ", ModuleIDList) & " group by MDAC_CommunityID " & ComposeSQLList("HAVING (", " MDAC_CommunityID ", CommunityIDList) & "ORDER BY usageTime desc"
            End Get
        End Property
        Public ReadOnly Property sp_getCommunitiesUsageTimeByPerson(ByVal PersonIDList As List(Of Integer))
            Get
                Return "SELECT sum(CMAC_UsageTime) as usageTime, CMAC_CommunityID AS CommunityID, count(CMAC_CommunityID) as nAccesses FROM dbo.COMMUNITY_ACTION WHERE " & ComposeSQLList("(", "CMAC_PersonID", PersonIDList) & " AND (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) group by CMAC_CommunityID ORDER BY usageTime"
            End Get
        End Property
#End Region
#Region "getGlobalModule"
        Public ReadOnly Property sp_getModules(ByVal PersonIDList As List(Of Integer), ByVal CommunityIDList As List(Of Integer), ByVal ModuleIDList As List(Of Integer))
            Get
                Return "SELECT sum(MDAC_UsageTime) as usageTime, count(MDAC_UsageTime) as nAccess, MDAC_ModuleID AS moduleID FROM dbo.MODULE_ACTION where MDAC_AccessDate >= @startDate AND MDAC_AccessDate <= @endDate " & ComposeSQLList("and (", " MDAC_PersonID ", PersonIDList) & ComposeSQLList(" AND (", " MDAC_CommunityID ", CommunityIDList) & " group by MDAC_ModuleID " & ComposeSQLList(" HAVING(", "MDAC_ModuleID", ModuleIDList) & " ORDER BY usageTime desc"
            End Get
        End Property
#End Region
#End Region

#Region "Global"
        Public Function getGlobalCommunity(ByRef PersonIDList As List(Of Integer), ByRef CommunityIDList As List(Of Integer), ByRef ModuleIDList As List(Of Integer), ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Try
                Dim checkCommunity As Boolean = False
                Dim checkPerson As Boolean = False
                Dim checkModule As Boolean = False
                If Not CommunityIDList Is Nothing Then
                    If CommunityIDList.Count > 0 Then
                        checkCommunity = True
                    End If
                End If
                If Not PersonIDList Is Nothing Then
                    If PersonIDList.Count > 0 Then
                        checkPerson = True
                    End If
                End If
                If Not ModuleIDList Is Nothing Then
                    If ModuleIDList.Count > 0 Then
                        checkModule = True
                    End If
                End If

                Dim db As Database = DatabaseFactory.CreateDatabase()
                Dim sqlCommand As String
                Dim dbCommand As DbCommand
                'caricamento lista 
                db = DatabaseFactory.CreateDatabase()
                If checkModule Then
                    sqlCommand = sp_getCommunitiesByModule(CommunityIDList, PersonIDList, ModuleIDList)
                Else
                    If checkCommunity Then
                        If checkPerson Then
                            sqlCommand = sp_getCommunitiesUsageTimeByPersonByCommunity(CommunityIDList, PersonIDList)
                        Else
                            sqlCommand = sp_getCommunitiesUsageTimeByCommunity(CommunityIDList)
                        End If
                    Else
                        If checkPerson Then
                            sqlCommand = sp_getCommunitiesUsageTimeByPerson(PersonIDList)
                        Else
                            sqlCommand = sp_getCommunitiesUsageTime
                        End If
                    End If
                End If

                dbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oActionStatistics As New BaseStatAction
                        oActionStatistics.CommunityID = sqlReader.Item("CommunityID")
                        oActionStatistics.nAccess = sqlReader.Item("nAccesses")
                        oActionStatistics.UsageTime = sqlReader.Item("usageTime")
                        oActionStatistics.ID = oActionStatistics.CommunityID
                        retval.Add(oActionStatistics)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try

            Return retval
        End Function
        Public Function getGlobalPerson(ByRef startDate As DateTime, ByRef endDate As DateTime, ByRef ModuleIDList As List(Of Integer), ByRef CommunityIdList As List(Of Integer), ByRef PersonIDList As List(Of Integer)) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                Dim checkCommunity As Boolean = False
                Dim checkModule As Boolean = False
                If Not CommunityIdList Is Nothing Then
                    If CommunityIdList.Count > 0 Then
                        checkCommunity = True
                    End If
                End If
                If Not ModuleIDList Is Nothing Then
                    If ModuleIDList.Count > 0 Then
                        checkModule = True
                    End If
                End If
                If checkCommunity Then
                    If checkModule Then
                        sqlCommand = sp_getUsersUTByModuleAndCommunity(CommunityIdList, ModuleIDList) & ComposeSQLList(" HAVING (", "MDAC_PersonID", PersonIDList)
                    Else
                        sqlCommand = sp_getUsersUTByCommunity(CommunityIdList) & ComposeSQLList(" HAVING (", "CMAC_PersonID", PersonIDList)
                    End If
                Else
                    If checkModule Then
                        sqlCommand = sp_getUsersUTByModule(ModuleIDList) & ComposeSQLList("HAVING (", "MDAC_PersonID", PersonIDList)
                    Else
                        sqlCommand = sp_getUsersUT & ComposeSQLList("HAVING (", "LGAC_PersonID", PersonIDList)
                    End If
                End If
                dbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oActionStatistics As New BaseStatAction
                        oActionStatistics.PersonID = sqlReader.Item("PersonID")
                        oActionStatistics.nAccess = sqlReader.Item("nAccesses")
                        oActionStatistics.UsageTime = sqlReader.Item("usageTime")
                        oActionStatistics.ID = oActionStatistics.PersonID
                        retval.Add(oActionStatistics)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getGlobalModule(ByRef startDate As DateTime, ByRef endDate As DateTime, ByRef ModuleIDList As List(Of Integer), ByRef CommunityIdList As List(Of Integer), ByRef PersonIDList As List(Of Integer)) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = sp_getModules(PersonIDList, CommunityIdList, ModuleIDList)
                dbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oActionStatistics As New BaseStatAction
                        oActionStatistics.ModuleID = sqlReader.Item("moduleID")
                        oActionStatistics.nAccess = sqlReader.Item("nAccess")
                        oActionStatistics.UsageTime = sqlReader.Item("usageTime")
                        oActionStatistics.ID = oActionStatistics.ModuleID
                        retval.Add(oActionStatistics)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function

        Public Function getTotal(ByRef startDate As DateTime, ByRef endDate As DateTime, ByRef ModuleIDList As List(Of Integer), ByRef CommunityIdList As List(Of Integer), ByRef PersonIDList As List(Of Integer)) As SummaryAction

            Dim retval As New SummaryAction
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Dim checkCommunity As Boolean = False
            Dim checkModule As Boolean = False
            If Not CommunityIdList Is Nothing Then
                If CommunityIdList.Count > 0 Then
                    checkCommunity = True
                End If
            End If
            If Not ModuleIDList Is Nothing Then
                If ModuleIDList.Count > 0 Then
                    checkModule = True
                End If
            End If
            Try
                If checkCommunity Then
                    If checkModule Then
                        sqlCommand = sp_getGlobalUsageTimeByModule(ModuleIDList) & ComposeSQLList(" AND (", "MDAC_CommunityID ", CommunityIdList) & ComposeSQLList(" AND (", "MDAC_PersonID", PersonIDList)
                    Else
                        sqlCommand = sp_getGlobalUsageTimeByCommunity(CommunityIdList) & ComposeSQLList(" AND (", "CMAC_PersonID", PersonIDList)
                    End If
                Else
                    If checkModule Then
                        sqlCommand = sp_getGlobalUsageTimeByModule(ModuleIDList) & ComposeSQLList(" AND (", "MDAC_PersonID", PersonIDList)
                    Else
                        sqlCommand = sp_getGlobalUsageTime & ComposeSQLList(" AND (", "LGAC_PersonID", PersonIDList)
                    End If
                End If

                dbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Dim sqlReader As SqlDataReader
                sqlReader = db.ExecuteReader(dbCommand)

                While sqlReader.Read()
                    retval.totalTime += (sqlReader.Item("usageTime"))
                    retval.nAccesses += (sqlReader.Item("nAccesses"))
                End While
            Catch ex As Exception
                Dim errorMessage As String = ex.Message
            End Try
            Return retval

        End Function
#End Region

#Region "Daily"
        Public Function getDailyModule(ByRef PersonID As Integer, ByRef CommunityId As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityId >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyModuleByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyModuleByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyModuleByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyModuleByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityId >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyModuleByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                        Else
                            sqlCommand = "sp_getDailyModuleByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyModuleByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getDailyModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If CommunityId <= 0 Then
                            oBaseStatAction.ModuleID = sqlReader.Item("ModuleID")
                        Else
                            oBaseStatAction.ModuleID = ModuleID
                        End If
                        oBaseStatAction.ID = oBaseStatAction.ModuleID
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getDailyCommunity(ByRef PersonID As Integer, ByRef CommunityId As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityId >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyCommunityByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyCommunityByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyCommunityByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityId >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyCommunityByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                        Else
                            sqlCommand = "sp_getDailyCommunityByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyCommunityByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getDailyCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If CommunityId <= 0 Then
                            oBaseStatAction.CommunityID = sqlReader.Item("CommunityID")
                        Else
                            oBaseStatAction.CommunityID = CommunityId
                        End If
                        oBaseStatAction.ID = oBaseStatAction.CommunityID
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getDailyPerson(ByRef PersonID As Integer, ByRef CommunityId As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityId >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyPersonByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyPersonByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityId >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyPersonByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                        Else
                            sqlCommand = "sp_getDailyPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyPersonByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getDailyPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If


                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        oBaseStatAction.PersonID = sqlReader.Item("PersonID")
                        oBaseStatAction.ID = oBaseStatAction.PersonID
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getDailyAccess(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyAccessByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyAccessByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyAccessByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getDailyAccessByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyAccessByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        Else
                            sqlCommand = "sp_getDailyAccessByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getDailyAccessByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getDailyAccess"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If PersonID > 0 Then
                            oBaseStatAction.PersonID = PersonID
                        End If
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
#End Region

#Region "Hourly"
        Public Function getHourlyPerson(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyPersonByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyPersonByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyPersonByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        Else
                            sqlCommand = "sp_getHourlyPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyPersonByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getHourlyPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If

                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If PersonID <= 0 Then
                            oBaseStatAction.PersonID = sqlReader.Item("PersonID")
                        Else
                            oBaseStatAction.PersonID = PersonID
                        End If
                        oBaseStatAction.ID = oBaseStatAction.PersonID
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getHourlyAccess(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyAccessByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyAccessByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyAccessByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyAccessByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyAccessByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        Else
                            sqlCommand = "sp_getHourlyAccessByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyAccessByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getHourlyAccess"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If

                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If PersonID > 0 Then
                            oBaseStatAction.PersonID = PersonID
                        End If
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getHourlyModule(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyModuleByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyModuleByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyModuleByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyModuleByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyModuleByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        Else
                            sqlCommand = "sp_getHourlyModuleByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyModuleByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getHourlyModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If

                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If ModuleID <= 0 Then
                            oBaseStatAction.ModuleID = sqlReader.Item("ModuleID")
                        Else
                            oBaseStatAction.ModuleID = ModuleID
                        End If
                        oBaseStatAction.ID = oBaseStatAction.ModuleID
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getHourlyCommunity(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
            Dim retval As New List(Of BaseStatAction)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                If ModuleID >= -1 Then
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyCommunityByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyCommunityByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyCommunityByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        Else
                            sqlCommand = "sp_getHourlyCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
                        End If
                    End If
                Else
                    If CommunityID >= 0 Then
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyCommunityByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        Else
                            sqlCommand = "sp_getHourlyCommunityByCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                        End If
                    Else
                        If PersonID > 0 Then
                            sqlCommand = "sp_getHourlyCommunityByPerson"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                        Else
                            sqlCommand = "sp_getHourlyCommunity"
                            dbCommand = db.GetStoredProcCommand(sqlCommand)
                        End If
                    End If
                End If

                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oBaseStatAction As New BaseStatAction
                        oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
                        oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
                        oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
                        oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
                        oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
                        If CommunityID <= 0 Then
                            oBaseStatAction.CommunityID = sqlReader.Item("CommunityID")
                        Else
                            oBaseStatAction.CommunityID = CommunityID
                        End If
                        oBaseStatAction.ID = oBaseStatAction.CommunityID
                        retval.Add(oBaseStatAction)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function

#End Region


#Region "Access Presence"
        Public Function getCommunityAccessResults(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
            Dim retval As New List(Of UserAccessResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_CommunityActionResult"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserAccessResult
                        oResult.UsageTime = sqlReader.Item("CMAC_UsageTime")
                        oResult.StartDate = sqlReader.Item("CMAC_AccessDate")
                        oResult.EndDate = sqlReader.Item("CMAC_LastActionDate")
                        oResult.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
                        If CommunityID <= 0 Then
                            oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
                        Else
                            oResult.CommunityID = CommunityID
                        End If
                        If PersonID <= 0 Then
                            oResult.PersonID = sqlReader.Item("CMAC_PersonID")
                        Else
                            oResult.PersonID = PersonID
                        End If
                        retval.Add(oResult)
                    End While
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getPortalAccessResults(ByRef PersonID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
            Dim retval As New List(Of UserAccessResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_PortalActionResult"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserAccessResult
                        oResult.UsageTime = sqlReader.Item("LGAC_UsageTime")
                        oResult.StartDate = sqlReader.Item("LGAC_LoginDate")
                        oResult.EndDate = sqlReader.Item("LGAC_LastActionDate")
                        oResult.IsClosed = sqlReader.Item("LGAC_SessionClosed")
                        If PersonID <= 0 Then
                            oResult.PersonID = sqlReader.Item("LGAC_PersonID")
                        Else
                            oResult.PersonID = PersonID
                        End If
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function


        Public Function getCommunityUserWithResults(ByVal PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As UserWithResult
            Dim iResponse As New UserWithResult With {.PersonID = PersonID, .CommunityID = CommunityID, .Result = 0}
            Dim iExec As Integer = 0
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand

            Try
                If CommunityID = -9999 Then
                    sqlCommand = "sp_getPortalUserWithResults"
                Else
                    sqlCommand = "sp_getCommunityUserWithResults"
                End If
                dbCommand = db.GetStoredProcCommand(sqlCommand)

                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                If Not CommunityID = -9999 Then
                    db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                End If
                iExec = db.ExecuteNonQuery(dbCommand)
                If iExec > 0 Then
                    iResponse.Result = 1
                End If
            Catch ex As Exception

            End Try
            Return iResponse
        End Function
        Public Function FindCommunitiesWithAccessResult(ByRef PersonID As Integer) As List(Of UserWithResult)
            Dim retval As New List(Of UserWithResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_GetCommuntyWithAccessResult"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserWithResult
                        oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
                        oResult.PersonID = PersonID
                        oResult.Result = 1
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function FindPortalUsersWithAccessResult() As List(Of UserWithResult)
            Dim retval As New List(Of UserWithResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_GetPortalUsersWithAccessResult"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserWithResult
                        oResult.CommunityID = -9999
                        oResult.PersonID = sqlReader.Item("LGAC_PersonID")
                        oResult.Result = 1
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function

        Public Function FindUsersWithAccessResult(ByRef CommunityID As Integer) As List(Of UserWithResult)
            Dim retval As New List(Of UserWithResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_GetCommunityUsersWithAccessResult"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserWithResult
                        oResult.CommunityID = CommunityID
                        oResult.PersonID = sqlReader.Item("CMAC_PersonID")
                        oResult.Result = 1
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function


#End Region

#Region "Search Between Date"
        Public Function getUsersCommunityResultsBetweenDate(ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
            Dim retval As New List(Of UserAccessResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_CommunityResultsBetweenDate"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserAccessResult
                        oResult.UsageTime = sqlReader.Item("CMAC_UsageTime")
                        oResult.StartDate = sqlReader.Item("CMAC_AccessDate")
                        oResult.EndDate = sqlReader.Item("CMAC_LastActionDate")
                        oResult.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
                        If CommunityID <= 0 Then
                            oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
                        Else
                            oResult.CommunityID = CommunityID
                        End If
                        oResult.PersonID = sqlReader.Item("CMAC_PersonID")
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getUsersPortalResultsBetweenDate(ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
            Dim retval As New List(Of UserAccessResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_PortalResultsBetweenDate"
                dbCommand = db.GetStoredProcCommand(sqlCommand)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserAccessResult
                        oResult.UsageTime = sqlReader.Item("LGAC_UsageTime")
                        oResult.StartDate = sqlReader.Item("LGAC_LoginDate")
                        oResult.EndDate = sqlReader.Item("LGAC_LastActionDate")
                        oResult.IsClosed = sqlReader.Item("LGAC_SessionClosed")
                        oResult.PersonID = sqlReader.Item("LGAC_PersonID")
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function

        Public Function getUsersCommunityResultsBetweenDate(ByVal PersonIDList As List(Of Integer), ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
            Dim retval As New List(Of UserAccessResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_CommunityResultsBetweenDate"
                dbCommand = db.GetSqlStringCommand(SQLcommunityResultsBetweenDate(PersonIDList))
                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserAccessResult
                        oResult.UsageTime = sqlReader.Item("CMAC_UsageTime")
                        oResult.StartDate = sqlReader.Item("CMAC_AccessDate")
                        oResult.EndDate = sqlReader.Item("CMAC_LastActionDate")
                        oResult.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
                        If CommunityID <= 0 Then
                            oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
                        Else
                            oResult.CommunityID = CommunityID
                        End If
                        oResult.PersonID = sqlReader.Item("CMAC_PersonID")
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function
        Public Function getUsersPortalResultsBetweenDate(ByVal PersonIDList As List(Of Integer), ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
            Dim retval As New List(Of UserAccessResult)
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim sqlCommand As String
            Dim dbCommand As DbCommand
            Try
                sqlCommand = "sp_PortalResultsBetweenDate"
                dbCommand = db.GetSqlStringCommand(SQLportalResultsBetweenDate(PersonIDList))
                db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim oResult As New UserAccessResult
                        oResult.UsageTime = sqlReader.Item("LGAC_UsageTime")
                        oResult.StartDate = sqlReader.Item("LGAC_LoginDate")
                        oResult.EndDate = sqlReader.Item("LGAC_LastActionDate")
                        oResult.IsClosed = sqlReader.Item("LGAC_SessionClosed")
                        oResult.PersonID = sqlReader.Item("LGAC_PersonID")
                        retval.Add(oResult)
                    End While
                    sqlReader.Close()
                End Using
            Catch ex As Exception
                Dim errore As String = ex.Message
            End Try
            Return retval
        End Function

        Private Function SQLcommunityResultsBetweenDate(ByVal PersonIDList As List(Of Integer)) As String
            Dim iResponse As String = "SELECT     CMAC_GuidSessionID, CMAC_AccessDate, CMAC_LastActionDate, CMAC_PersonID, CMAC_CommunityID, CMAC_ExitCommunity, CMAC_AccessDay,CMAC_AccessMonth, CMAC_AccessHour, CMAC_AccessYear, CMAC_UsageTime FROM dbo.COMMUNITY_ACTION "

            iResponse &= " where (CMAC_UsageTime > 0) AND (CMAC_CommunityID=@CommunityID OR @CommunityID=-1) AND ((CMAC_AccessDate >= @startDate AND CMAC_AccessDate < @endDate) OR (CMAC_LastActionDate >= @startDate AND CMAC_LastActionDate < @endDate)) "
            iResponse &= " and " & ComposeSQLList("(", "CMAC_PersonID", PersonIDList) & " "
            iResponse &= " order by CMAC_AccessDate asc"

            Return iResponse
        End Function
        Private Function SQLportalResultsBetweenDate(ByVal PersonIDList As List(Of Integer)) As String
            Dim iResponse As String = "SELECT LGAC_ID, LGAC_LoginDate, LGAC_LastActionDate, LGAC_PersonID, LGAC_SessionClosed, LGAC_LoginHour, LGAC_UsageTime FROM LOGIN_ACTION"

            iResponse &= " where(LGAC_UsageTime > 0) AND ((LGAC_LoginDate >= @startDate AND LGAC_LoginDate < @endDate) OR (LGAC_LastActionDate >= @startDate AND LGAC_LastActionDate < @endDate)) "
            iResponse &= " and " & ComposeSQLList("(", "LGAC_PersonID", PersonIDList) & " "
            iResponse &= " order by LGAC_LoginDate asc"

            Return iResponse
        End Function
#End Region


    End Class
End Namespace