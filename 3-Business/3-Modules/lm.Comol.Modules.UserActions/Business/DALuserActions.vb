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

Public Class DALuserActions
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
    'Public Shared Function isInvitato(ByVal idQuestionario As Integer, ByVal idUtenteInvitato As Integer) As Boolean
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim retVal As Integer

    '    Dim sqlCommand As String = "sp_Questionario_isInvitato"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.String, idQuestionario)
    '    db.AddInParameter(dbCommand, "idUtenteInvitato", DbType.String, idUtenteInvitato)
    '    retVal = db.ExecuteScalar(dbCommand)

    '    If retVal = 0 Then
    '        Return False
    '    Else
    '        Return True
    '    End If
    'End Function
    'Public Shared Function readInvitoByPersona(ByVal idQuestionario As Integer, ByVal idPersona As Integer) As UtenteInvitato
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim retVal As Integer

    '    Dim sqlCommand As String = "sp_Questionario_readIDInvitoByPersona"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.String, idQuestionario)
    '    db.AddInParameter(dbCommand, "idPersona", DbType.String, idPersona)
    '    retVal = db.ExecuteScalar(dbCommand)

    '    Dim oUI As New UtenteInvitato
    '    oUI.ID = retVal

    '    Return oUI

    'End Function

    'Private Shared Function readCampiQuestionario(ByVal idQuestionario As Integer, ByVal idLingua As Integer, ByVal db As Database, ByVal connection As DbConnection) As Questionario
    '    If db Is Nothing Then
    '        db = DatabaseFactory.CreateDatabase()
    '        connection = db.CreateConnection()
    '    ElseIf connection Is Nothing Then
    '        connection = db.CreateConnection()
    '    End If
    '    Dim oQuest As New Questionario

    '    oQuest.id = idQuestionario
    '    oQuest.idLingua = idLingua

    '    Dim sqlCommand As String = "sp_Questionario_QuestionarioByLingua_Select"
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '    dbCommand.CommandTimeout = 1200
    '    dbCommand.Connection = connection
    '    db.AddInParameter(dbCommand, "idQuestionario", DbType.Int32, oQuest.id)
    '    db.AddInParameter(dbCommand, "idLingua", DbType.Int32, oQuest.idLingua)
    '    '    Dim t As Integer = connection.ConnectionTimeout

    '    Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '        While sqlReader.Read()
    '            oQuest.id = isNullInt(sqlReader.Item("QSTN_Id"))
    '            oQuest.nome = isNullString(sqlReader.Item("QSML_Nome"))
    '            oQuest.descrizione = isNullString(sqlReader.Item("QSML_Descrizione"))
    '            oQuest.dataCreazione = isNullString(sqlReader.Item("QSTN_DataCreazione"))
    '            oQuest.idPersonaCreator = isNullInt(sqlReader.Item("QSTN_PRSN_Creator_Id"))
    '            oQuest.idGruppo = isNullInt(sqlReader.Item("QSTN_QSGR_Id"))
    '            oQuest.isCancellato = isNullBoolean(sqlReader.Item("QSML_IsCancellato"))
    '            oQuest.isBloccato = isNullBoolean(sqlReader.Item("QSML_IsBloccato"))
    '            oQuest.dataInizio = isNullDateMin(sqlReader.Item("QSTN_DataInizio"))
    '            oQuest.dataFine = isNullDateMax(sqlReader.Item("QSTN_DataFine"))
    '            oQuest.idLingua = isNullInt(sqlReader.Item("QSML_IdLingua"))
    '            oQuest.pesoTotale = isNullInt(sqlReader.Item("QSTN_pesoTotale"))
    '            oQuest.scalaValutazione = isNullInt(sqlReader.Item("QSTN_scalaValutazione"))
    '            oQuest.isReadOnly = isNullBoolean(sqlReader.Item("QSTN_IsChiuso"))
    '            oQuest.idQuestionarioMultilingua = isNullInt(sqlReader.Item("QSML_Id"))
    '            oQuest.isDefault = isNullBoolean(sqlReader.Item("QSML_IsDefault"))
    '            oQuest.durata = isNullInt(sqlReader.Item("QSTN_Durata"))
    '            oQuest.risultatiAnonimi = isNullBoolean(sqlReader.Item("QSTN_RisultatiAnonimi"))
    '            oQuest.forUtentiComunita = isNullBoolean(sqlReader.Item("QSTN_forUtentiComunita"))
    '            oQuest.forUtentiPortale = isNullBoolean(sqlReader.Item("QSTN_forUtentiPortale"))
    '            oQuest.forUtentiInvitati = isNullBoolean(sqlReader.Item("QSTN_forUtentiInvitati"))
    '            oQuest.forUtentiEsterni = isNullBoolean(sqlReader.Item("QSTN_forUtentiEsterni"))
    '            oQuest.visualizzaRisposta = isNullBoolean(sqlReader.Item("QSTN_visualizzaRisposta"))
    '            oQuest.visualizzaCorrezione = isNullBoolean(sqlReader.Item("QSTN_visualizzaCorrezione"))
    '            oQuest.visualizzaSuggerimenti = isNullBoolean(sqlReader.Item("QSTN_visualizzaSuggerimenti"))
    '            oQuest.editaRisposta = isNullBoolean(sqlReader.Item("QSTN_editaRisposta"))
    '            oQuest.tipoGrafico = isNullInt(sqlReader.Item("QSTN_TPGF_Id"))
    '            oQuest.tipo = isNullInt(sqlReader.Item("QSTN_Tipo"))
    '            oQuest.creator = isNullString(sqlReader.Item("creator"))
    '            oQuest.dataModifica = isNullString(sqlReader.Item("QSTN_DataModifica"))
    '            oQuest.idPersonaEditor = isNullInt(sqlReader.Item("QSTN_PRSN_Editor_Id"))
    '            oQuest.isRandomOrder = isNullBoolean(sqlReader.Item("QSTN_isRandomOrder"))
    '            oQuest.editor = isNullString(sqlReader.Item("editor"))
    '            oQuest.nDomandeDiffBassa = isNullInt(sqlReader.Item("QSTN_nDomandeDiffBassa"))
    '            oQuest.nDomandeDiffMedia = isNullInt(sqlReader.Item("QSTN_nDomandeDiffMedia"))
    '            oQuest.nDomandeDiffAlta = isNullInt(sqlReader.Item("QSTN_nDomandeDiffAlta"))
    '            oQuest.isRandomOrder_Options = isNullBoolean(sqlReader.Item("QSTN_isRandomOrder_Options"))
    '            oQuest.nQuestionsPerPage = isNullInt(sqlReader.Item("QSTN_nQuestionsPerPage"))
    '            oQuest.isPassword = isNullBoolean(sqlReader.Item("QSTN_isPassword"))
    '            oQuest.ownerType = isNullInt(sqlReader.Item("QSTN_ownerType"))
    '            oQuest.ownerId = isNullBigInt(sqlReader.Item("QSTN_ownerId"))
    '            oQuest.ownerGUID = isNullGuid(sqlReader.Item("QSTN_ownerGUID"))
    '        End While
    '        sqlReader.Close()
    '    End Using
    '    Return oQuest
    'End Function

#Region "SQL"

    Private Shared Function GetCondition(idCondition As Integer, cond As whereCondition, table As table) As String
        Dim name As String = ""
        Select Case cond
            Case whereCondition.person
                name = "PersonID"
            Case whereCondition.modules
                name = "ModuleID"
            Case whereCondition.community
                name = "CommunityID"
        End Select
        Return GetTablePrefix(table) & name & "=" & idCondition.ToString()
    End Function
    Private Shared Function GetTablePrefix(table As table) As String
        Select Case table
            Case DALuserActions.table.action
                Return "ACTN_}"
            Case DALuserActions.table.community
                Return "CMAC_"
            Case DALuserActions.table.login
                Return "LGAC_"
            Case DALuserActions.table.moduleAction
                Return "MDAC_"
            Case DALuserActions.table.usage
                Return "MDUT_"
            Case Else
                Return ""
        End Select
    End Function

#Region "Generic Procedure"
    Private Const _sp_YearsWithStatistic As String = "SELECT MDAC_AccessYear as Year FROM MODULE_ACTION where 1=1 {0} GROUP BY MDAC_AccessYear"
    Private Shared Function sp_YearsWithStatistic() As String
        Return String.Format(_sp_YearsWithStatistic, "")
    End Function
    Private Shared Function sp_YearsWithStatistic(idPerson As Integer, idCommunity As Integer, idModule As Integer) As String

        Return String.Format(_sp_YearsWithStatistic, IIf(idPerson > 0, "AND " & GetCondition(idPerson, whereCondition.person, table.moduleAction), "") _
                             & IIf(idCommunity > 0, "AND " & GetCondition(idCommunity, whereCondition.community, table.moduleAction), "") _
                             & IIf(idModule > 0, " AND " & GetCondition(idModule, whereCondition.modules, table.moduleAction), ""))
    End Function
    Private Shared Function sp_YearsMonthsWithStatistic(idPerson As Integer, idCommunity As Integer, idModule As Integer) As String
        Dim sql As String = "SELECT  MDAC_AccessYear AS Year, MDAC_AccessMonth as Month FROM MODULE_ACTION WHERE 1=1 {0} GROUP BY MDAC_AccessYear, MDAC_AccessMonth"

        Return String.Format(sql, IIf(idPerson > 0, "AND " & GetCondition(idPerson, whereCondition.person, table.moduleAction), "") _
                                & IIf(idCommunity > 0, "AND " & GetCondition(idCommunity, whereCondition.community, table.moduleAction), "") _
                                & IIf(idModule > 0, " AND " & GetCondition(idModule, whereCondition.modules, table.moduleAction), ""))
    End Function

    Private Shared Function sp_YearsMonthsDaysWithStatistic(idPerson As Integer, idCommunity As Integer, idModule As Integer) As String
        Dim sql As String = "SELECT     MDAC_AccessYear AS Year, MDAC_AccessMonth as Month ,MDAC_AccessDay as Day FROM MODULE_ACTION WHERE 1=1 {0} GROUP BY MDAC_AccessYear, MDAC_AccessMonth,MDAC_AccessDay"
        Return String.Format(sql, IIf(idPerson > 0, "AND " & GetCondition(idPerson, whereCondition.person, table.moduleAction), "") _
                             & IIf(idCommunity > 0, "AND " & GetCondition(idCommunity, whereCondition.community, table.moduleAction), "") _
                             & IIf(idModule > 0, " AND " & GetCondition(idModule, whereCondition.modules, table.moduleAction), ""))
    End Function

#End Region

#Region "getTotal"
    Private Shared Function sp_getGlobalUsageTime() As String
        Return "SELECT sum(LGAC_UsageTime) as usageTime, count(*) as nAccesses FROM dbo.LOGIN_ACTION WHERE (LGAC_LoginDate >= @startDate AND LGAC_LoginDate <= @endDate) "
    End Function
    Private Shared Function sp_getGlobalUsageTimeByModule(ByVal modules As List(Of Integer)) As String
        Return "SELECT sum(MDAC_UsageTime) AS usageTime, count( MDAC_UsageTime) AS nAccesses FROM dbo.MODULE_ACTION WHERE (" & ComposeSQLList("", "MDAC_ModuleID ", modules) & " AND (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) "
    End Function
    Private Shared Function sp_getGlobalUsageTimeByCommunity(ByVal communities As List(Of Integer)) As String
        Return "SELECT sum(CMAC_UsageTime) as usageTime, count(*) as nAccesses FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate AND CMAC_AccessDate <= @endDate) AND (" & ComposeSQLList("", "CMAC_CommunityID ", communities)
    End Function
#End Region
#Region "getGlobalPerson"
    Private Shared Function sp_getUsersUTByCommunity(ByVal communities As List(Of Integer)) As String
        Return "SELECT SUM(CMAC_UsageTime) AS usageTime, CMAC_PersonID as PersonID, count(CMAC_PersonID) as nAccesses FROM COMMUNITY_ACTION WHERE " & ComposeSQLList("(", "CMAC_CommunityID ", communities) & "AND (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) GROUP BY CMAC_PersonID "
    End Function
    Private Shared Function sp_getUsersUTByModuleAndCommunity(ByVal communities As List(Of Integer), ByVal modules As List(Of Integer)) As String
        Return "SELECT SUM(MDAC_UsageTime) AS usageTime, MDAC_PersonID as PersonID, count(MDAC_PersonID) as nAccesses FROM MODULE_ACTION WHERE (" & ComposeSQLList("", "MDAC_ModuleID ", modules) & ComposeSQLList("AND (", "MDAC_CommunityID ", communities) & " AND (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) GROUP BY MDAC_PersonID "
    End Function
    Private Shared Function sp_getUsersUT() As String
        Return "SELECT SUM(LGAC_UsageTime) AS usageTime, LGAC_PersonID as PersonID, count(LGAC_PersonID) as nAccesses FROM LOGIN_ACTION WHERE (LGAC_LoginDate >= @startDate) AND (LGAC_LoginDate <= @endDate) GROUP BY LGAC_PersonID "
    End Function
    Private Shared Function sp_getUsersUTByModule(ByVal modules As List(Of Integer)) As String
        Return "SELECT SUM(MDAC_UsageTime) AS usageTime, MDAC_PersonID as PersonID, count(MDAC_PersonID) as nAccesses FROM MODULE_ACTION  WHERE (" & ComposeSQLList("", "MDAC_ModuleID ", modules) & " AND (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) GROUP BY MDAC_PersonID "
    End Function

#End Region
#Region "getGlobalCommunity"
    Private Shared Function sp_getCommunitiesUsageTimeByCommunity(ByVal communities As List(Of Integer))
        Return "SELECT sum(CMAC_UsageTime) as usageTime, count(CMAC_UsageTime) as nAccesses, CMAC_CommunityID as CommunityID FROM dbo.COMMUNITY_ACTION WHERE " & ComposeSQLList("(", " CMAC_CommunityID ", communities) & " AND (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) group by CMAC_CommunityID ORDER BY usageTime"
    End Function
    Private Shared Function sp_getCommunitiesUsageTimeByPersonByCommunity(ByVal communities As List(Of Integer), ByVal persons As List(Of Integer))
        Return "SELECT sum(CMAC_UsageTime) as usageTime, count(CMAC_UsageTime) as nAccesses, CMAC_CommunityID as CommunityID FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) " & ComposeSQLList("AND (", " CMAC_PersonID ", persons) & " group by CMAC_CommunityID " & ComposeSQLList("HAVING (", " CMAC_CommunityID ", communities) & "ORDER BY usageTime"
    End Function
    Private Shared Function sp_getCommunitiesUsageTime()
        Return "SELECT sum(CMAC_UsageTime) as usageTime, CMAC_CommunityID AS communityID,  count(CMAC_CommunityID) as nAccesses FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) group by CMAC_CommunityID ORDER BY usageTime"
    End Function
    Private Shared Function sp_getCommunitiesByModule(ByVal communities As List(Of Integer), ByVal persons As List(Of Integer), ByVal modules As List(Of Integer))
        Return "SELECT sum(MDAC_UsageTime) as usageTime, count(MDAC_UsageTime) as nAccesses, MDAC_CommunityID as CommunityID FROM dbo.MODULE_ACTION WHERE (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) " & ComposeSQLList("AND (", " MDAC_PersonID ", persons) & ComposeSQLList("AND (", " MDAC_ModuleID ", modules) & " group by MDAC_CommunityID " & ComposeSQLList("HAVING (", " MDAC_CommunityID ", communities) & "ORDER BY usageTime desc"
    End Function
    Private Shared Function sp_getCommunitiesUsageTimeByPerson(ByVal persons As List(Of Integer))
        Return "SELECT sum(CMAC_UsageTime) as usageTime, CMAC_CommunityID AS CommunityID, count(CMAC_CommunityID) as nAccesses FROM dbo.COMMUNITY_ACTION WHERE " & ComposeSQLList("(", "CMAC_PersonID", persons) & " AND (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) group by CMAC_CommunityID ORDER BY usageTime"
    End Function
    Private Shared Function sp_getCommunitiesIdByPerson(ByVal persons As List(Of Integer))
        Return "SELECT distinct CMAC_CommunityID as CommunityID FROM dbo.COMMUNITY_ACTION WHERE (CMAC_AccessDate >= @startDate) AND (CMAC_AccessDate <= @endDate) " & ComposeSQLList("AND (", " CMAC_PersonID ", persons) '' & " group by CMAC_CommunityID "
    End Function
   
#End Region
#Region "getGlobalModule"
    Private Shared Function sp_getModules(ByVal persons As List(Of Integer), ByVal communities As List(Of Integer), ByVal modules As List(Of Integer))
        Return "SELECT sum(MDAC_UsageTime) as usageTime, count(MDAC_UsageTime) as nAccess, MDAC_ModuleID AS moduleID FROM dbo.MODULE_ACTION where MDAC_AccessDate >= @startDate AND MDAC_AccessDate <= @endDate " & ComposeSQLList("and (", " MDAC_PersonID ", persons) & ComposeSQLList(" AND (", " MDAC_CommunityID ", communities) & " group by MDAC_ModuleID " & ComposeSQLList(" HAVING(", "MDAC_ModuleID", modules) & " ORDER BY usageTime desc"
    End Function
    Private Shared Function sp_getModulesIdByPerson(ByVal persons As List(Of Integer), ByVal communities As List(Of Integer))
        Return "SELECT distinct MDAC_ModuleID as IdModule FROM dbo.MODULE_ACTION WHERE (MDAC_AccessDate >= @startDate) AND (MDAC_AccessDate <= @endDate) " & ComposeSQLList("AND (", " MDAC_PersonID ", persons) & ComposeSQLList(" AND (", " MDAC_CommunityID ", communities) & " group by MDAC_ModuleID "
    End Function
#End Region

#End Region

#Region "Common"
    Private Shared Function ComposeSQLList(ByRef prefix As String, ByRef parameter As String, ByVal idList As List(Of Integer)) As String
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
#Region "Global"
    Public Shared Function getAvailableYears(idPerson As Integer, idCommunity As Integer, idModule As Integer) As List(Of Integer)
        Dim results As New List(Of Integer)
        Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
        Try
            Dim connection As DbConnection = db.CreateConnection()
            Dim items As New List(Of Integer)

            Using connection
                connection.Open()
                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sp_YearsWithStatistic(idPerson, idCommunity, idModule))

                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        results.Add(CInt(sqlReader.Item("Year")))
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
        Return results
    End Function
    Public Shared Function getAvailableYearsMonth(idPerson As Integer, idCommunity As Integer, idModule As Integer) As List(Of dtoYearItem)
        Dim results As New List(Of dtoYearItem)
        Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
        Try
            Dim connection As DbConnection = db.CreateConnection()
            Dim items As New List(Of Integer)

            Using connection
                connection.Open()
                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sp_YearsMonthsDaysWithStatistic(idPerson, idCommunity, idModule))

                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim year As Integer = CInt(sqlReader.Item("Year"))
                        Dim month As Integer = CInt(sqlReader.Item("Month"))
                        Dim day As Integer = CInt(sqlReader.Item("Day"))
                        If results.Where(Function(r) r.Value = year).Any() Then
                            results.Where(Function(r) r.Value = year).FirstOrDefault.AddMonth(month, day)
                        Else
                            results.Add(New dtoYearItem(year, month, day))
                        End If
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
        Return results
    End Function
    Public Shared Function GetCommunityStatistics(ByVal context As UsageContext, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim persons As New List(Of Integer), communities As New List(Of Integer), modules As New List(Of Integer)
        If context.ModuleID <> -2 Then
            modules.Add(context.ModuleID)
        End If
        If context.CommunityID >= 0 Then
            communities.Add(context.CommunityID)
        End If
        If context.UserID > 0 Then
            persons.Add(context.UserID)
        End If
        Return GetCommunityStatistics(persons, communities, modules, startDate, endDate)
    End Function
    Public Shared Function GetCommunityStatistics(ByVal context As UsageContext, ByVal communities As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim persons As New List(Of Integer), modules As New List(Of Integer)
        If context.ModuleID <> -2 Then
            modules.Add(context.ModuleID)
        End If
        If context.UserID > 0 Then
            persons.Add(context.UserID)
        End If
        Return GetCommunityStatistics(persons, communities, modules, startDate, endDate)
    End Function
    Public Shared Function GetCommunityStatistics(ByVal persons As List(Of Integer), ByVal communities As List(Of Integer), ByVal modules As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim statistics As New List(Of dtoBaseStatistic)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Dim checkCommunity As Boolean = False, checkPerson As Boolean = False, checkModule As Boolean = False
            If Not IsNothing(communities) Then
                checkCommunity = (communities.Count > 0)
            End If
            If Not IsNothing(modules) Then
                checkModule = (modules.Count > 0)
            End If
            If Not IsNothing(persons) Then
                checkPerson = (persons.Count > 0)
            End If

            Using connection
                connection.Open()
                Dim sqlCommand As String = ""
                If checkModule Then
                    sqlCommand = sp_getCommunitiesByModule(communities, persons, modules)
                ElseIf checkCommunity AndAlso checkPerson Then
                    sqlCommand = sp_getCommunitiesUsageTimeByPersonByCommunity(communities, persons)
                ElseIf checkCommunity Then
                    sqlCommand = sp_getCommunitiesUsageTimeByCommunity(communities)
                ElseIf checkPerson Then
                    sqlCommand = sp_getCommunitiesUsageTimeByPerson(persons)
                Else
                    sqlCommand = sp_getCommunitiesUsageTime()
                End If

                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoBaseStatistic
                        statistic.Type = StatisticType.User
                        statistic.ID = sqlReader.Item("CommunityID")
                        statistic.nAccesses = sqlReader.Item("nAccesses")
                        statistic.UsageTime = sqlReader.Item("usageTime")
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

    Public Shared Function GetPersonStatistics(ByVal context As UsageContext, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim persons As New List(Of Integer), communities As New List(Of Integer), modules As New List(Of Integer)
        If context.ModuleID <> -2 Then
            modules.Add(context.ModuleID)
        End If
        If context.CommunityID >= 0 Then
            communities.Add(context.CommunityID)
        End If
        If context.UserID > 0 Then
            persons.Add(context.UserID)
        End If
        Return GetPersonStatistics(persons, communities, modules, startDate, endDate)
    End Function
    Public Shared Function GetPersonStatistics(ByVal context As UsageContext, ByVal persons As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim communities As New List(Of Integer), modules As New List(Of Integer)
        If context.ModuleID <> -2 Then
            modules.Add(context.ModuleID)
        End If
        If context.CommunityID >= 0 Then
            communities.Add(context.CommunityID)
        End If
        Return GetPersonStatistics(persons, communities, modules, startDate, endDate)
    End Function
    Public Shared Function GetPersonStatistics(ByVal persons As List(Of Integer), ByVal communities As List(Of Integer), ByVal modules As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim statistics As New List(Of dtoBaseStatistic)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Dim checkCommunity As Boolean = False
            Dim checkModule As Boolean = False
            If Not IsNothing(communities) Then
                checkCommunity = (communities.Count > 0)
            End If
            If Not IsNothing(modules) Then
                checkModule = (modules.Count > 0)
            End If

            Using connection
                connection.Open()
                Dim sqlCommand As String = ""
                If checkCommunity AndAlso checkModule Then
                    sqlCommand = sp_getUsersUTByModuleAndCommunity(communities, modules) & ComposeSQLList(" HAVING (", "MDAC_PersonID", persons)
                ElseIf checkCommunity Then
                    sqlCommand = sp_getUsersUTByCommunity(communities) & ComposeSQLList(" HAVING (", "CMAC_PersonID", persons)
                ElseIf checkModule Then
                    sqlCommand = sp_getUsersUTByModule(modules) & ComposeSQLList("HAVING (", "MDAC_PersonID", persons)
                Else
                    sqlCommand = sp_getUsersUT() & ComposeSQLList("HAVING (", "LGAC_PersonID", persons)
                End If

                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoBaseStatistic
                        statistic.Type = StatisticType.User
                        statistic.ID = sqlReader.Item("PersonID")
                        statistic.nAccesses = sqlReader.Item("nAccesses")
                        statistic.UsageTime = sqlReader.Item("usageTime")
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
    Public Shared Function GetModuleStatistics(ByVal context As UsageContext, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim persons As New List(Of Integer), communities As New List(Of Integer), modules As New List(Of Integer)
        If context.ModuleID <> -2 Then
            modules.Add(context.ModuleID)
        End If
        If context.CommunityID >= 0 Then
            communities.Add(context.CommunityID)
        End If
        If context.UserID > 0 Then
            persons.Add(context.UserID)
        End If
        Return GetModuleStatistics(persons, communities, modules, startDate, endDate)
    End Function
    Public Shared Function GetModuleStatistics(ByVal context As UsageContext, ByVal modules As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim persons As New List(Of Integer), communities As New List(Of Integer)
        If context.CommunityID >= 0 Then
            communities.Add(context.CommunityID)
        End If
        If context.UserID > 0 Then
            persons.Add(context.UserID)
        End If
        Return GetModuleStatistics(persons, communities, modules, startDate, endDate)
    End Function
    Public Shared Function GetModuleStatistics(ByVal persons As List(Of Integer), ByVal communities As List(Of Integer), ByVal modules As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoBaseStatistic)
        Dim statistics As New List(Of dtoBaseStatistic)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Dim checkCommunity As Boolean = False
            Dim checkModule As Boolean = False
            If Not IsNothing(communities) Then
                checkCommunity = (communities.Count > 0)
            End If
            If Not IsNothing(modules) Then
                checkModule = (modules.Count > 0)
            End If

            Using connection
                connection.Open()

                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sp_getModules(persons, communities, modules))
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim statistic As New dtoBaseStatistic
                        statistic.Type = StatisticType.User
                        statistic.ID = sqlReader.Item("moduleID")
                        statistic.nAccesses = sqlReader.Item("nAccess")
                        statistic.UsageTime = sqlReader.Item("usageTime")
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

    Public Shared Function GetCommunitiesWithStatistics(ByVal persons As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of Integer)
        Dim items As New List(Of Integer)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Dim checkPerson As Boolean = False
            If Not IsNothing(persons) Then
                checkPerson = (persons.Count > 0)
            End If

            Using connection
                connection.Open()
                Dim sqlCommand As String = sp_getCommunitiesIdByPerson(persons)

                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        items.Add(sqlReader.Item("CommunityID"))
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
        Return items.Distinct().ToList()
    End Function
    Public Shared Function GetModulesIdWithStatistics(ByVal persons As List(Of Integer), ByVal communities As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of Integer)
        Dim items As New List(Of Integer)

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Dim checkPerson As Boolean = False
            If Not IsNothing(persons) Then
                checkPerson = (persons.Count > 0)
            End If

            Using connection
                connection.Open()
                Dim sqlCommand As String = sp_getModulesIdByPerson(persons, communities)

                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        items.Add(sqlReader.Item("IdModule"))
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
        Return items.Distinct().ToList()
    End Function

    'Public Function getGlobalCommunity(ByRef PersonIDList As List(Of Integer), ByRef CommunityIDList As List(Of Integer), ByRef ModuleIDList As List(Of Integer), ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '    Dim retval As New List(Of BaseStatAction)
    '    Try
    '        Dim checkCommunity As Boolean = False
    '        Dim checkPerson As Boolean = False
    '        Dim checkModule As Boolean = False
    '        If Not CommunityIDList Is Nothing Then
    '            If CommunityIDList.Count > 0 Then
    '                checkCommunity = True
    '            End If
    '        End If
    '        If Not PersonIDList Is Nothing Then
    '            If PersonIDList.Count > 0 Then
    '                checkPerson = True
    '            End If
    '        End If
    '        If Not ModuleIDList Is Nothing Then
    '            If ModuleIDList.Count > 0 Then
    '                checkModule = True
    '            End If
    '        End If

    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        'caricamento lista 
    '        db = DatabaseFactory.CreateDatabase()
    '        If checkModule Then
    '            sqlCommand = sp_getCommunitiesByModule(CommunityIDList, PersonIDList, ModuleIDList)
    '        Else
    '            If checkCommunity Then
    '                If checkPerson Then
    '                    sqlCommand = sp_getCommunitiesUsageTimeByPersonByCommunity(CommunityIDList, PersonIDList)
    '                Else
    '                    sqlCommand = sp_getCommunitiesUsageTimeByCommunity(CommunityIDList)
    '                End If
    '            Else
    '                If checkPerson Then
    '                    sqlCommand = sp_getCommunitiesUsageTimeByPerson(PersonIDList)
    '                Else
    '                    sqlCommand = sp_getCommunitiesUsageTime
    '                End If
    '            End If
    '        End If

    '        dbCommand = db.GetSqlStringCommand(sqlCommand)
    '        db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '        db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '            While sqlReader.Read()
    '                Dim oActionStatistics As New BaseStatAction
    '                oActionStatistics.CommunityID = sqlReader.Item("CommunityID")
    '                oActionStatistics.nAccess = sqlReader.Item("nAccesses")
    '                oActionStatistics.UsageTime = sqlReader.Item("usageTime")
    '                oActionStatistics.ID = oActionStatistics.CommunityID
    '                retval.Add(oActionStatistics)
    '            End While
    '            sqlReader.Close()
    '        End Using
    '    Catch ex As Exception
    '        Dim errore As String = ex.Message
    '    End Try

    '    Return retval
    'End Function
    'Public Function getGlobalPerson(ByRef startDate As DateTime, ByRef endDate As DateTime, ByRef ModuleIDList As List(Of Integer), ByRef CommunityIdList As List(Of Integer), ByRef PersonIDList As List(Of Integer)) As List(Of BaseStatAction)
    '    Dim retval As New List(Of BaseStatAction)
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String
    '    Dim dbCommand As DbCommand
    '    Try
    '        Dim checkCommunity As Boolean = False
    '        Dim checkModule As Boolean = False
    '        If Not CommunityIdList Is Nothing Then
    '            If CommunityIdList.Count > 0 Then
    '                checkCommunity = True
    '            End If
    '        End If
    '        If Not ModuleIDList Is Nothing Then
    '            If ModuleIDList.Count > 0 Then
    '                checkModule = True
    '            End If
    '        End If
    '        If checkCommunity Then
    '            If checkModule Then
    '                sqlCommand = sp_getUsersUTByModuleAndCommunity(CommunityIdList, ModuleIDList) & ComposeSQLList(" HAVING (", "MDAC_PersonID", PersonIDList)
    '            Else
    '                sqlCommand = sp_getUsersUTByCommunity(CommunityIdList) & ComposeSQLList(" HAVING (", "CMAC_PersonID", PersonIDList)
    '            End If
    '        Else
    '            If checkModule Then
    '                sqlCommand = sp_getUsersUTByModule(ModuleIDList) & ComposeSQLList("HAVING (", "MDAC_PersonID", PersonIDList)
    '            Else
    '                sqlCommand = sp_getUsersUT & ComposeSQLList("HAVING (", "LGAC_PersonID", PersonIDList)
    '            End If
    '        End If
    '        dbCommand = db.GetSqlStringCommand(sqlCommand)
    '        db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '        db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '            While sqlReader.Read()
    '                Dim oActionStatistics As New BaseStatAction
    '                oActionStatistics.PersonID = sqlReader.Item("PersonID")
    '                oActionStatistics.nAccess = sqlReader.Item("nAccesses")
    '                oActionStatistics.UsageTime = sqlReader.Item("usageTime")
    '                oActionStatistics.ID = oActionStatistics.PersonID
    '                retval.Add(oActionStatistics)
    '            End While
    '            sqlReader.Close()
    '        End Using
    '    Catch ex As Exception
    '        Dim errore As String = ex.Message
    '    End Try
    '    Return retval
    'End Function
    'Public Function getGlobalModule(ByRef startDate As DateTime, ByRef endDate As DateTime, ByRef ModuleIDList As List(Of Integer), ByRef CommunityIdList As List(Of Integer), ByRef PersonIDList As List(Of Integer)) As List(Of BaseStatAction)
    '    Dim retval As New List(Of BaseStatAction)
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String
    '    Dim dbCommand As DbCommand
    '    Try
    '        sqlCommand = sp_getModules(PersonIDList, CommunityIdList, ModuleIDList)
    '        dbCommand = db.GetSqlStringCommand(sqlCommand)
    '        db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '        db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '            While sqlReader.Read()
    '                Dim oActionStatistics As New BaseStatAction
    '                oActionStatistics.ModuleID = sqlReader.Item("moduleID")
    '                oActionStatistics.nAccess = sqlReader.Item("nAccess")
    '                oActionStatistics.UsageTime = sqlReader.Item("usageTime")
    '                oActionStatistics.ID = oActionStatistics.ModuleID
    '                retval.Add(oActionStatistics)
    '            End While
    '            sqlReader.Close()
    '        End Using
    '    Catch ex As Exception
    '        Dim errore As String = ex.Message
    '    End Try
    '    Return retval
    'End Function

    'Public Function getTotal(ByRef startDate As DateTime, ByRef endDate As DateTime, ByRef ModuleIDList As List(Of Integer), ByRef CommunityIdList As List(Of Integer), ByRef PersonIDList As List(Of Integer)) As SummaryAction

    '    Dim retval As New SummaryAction
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String
    '    Dim dbCommand As DbCommand
    '    Dim checkCommunity As Boolean = False
    '    Dim checkModule As Boolean = False
    '    If Not CommunityIdList Is Nothing Then
    '        If CommunityIdList.Count > 0 Then
    '            checkCommunity = True
    '        End If
    '    End If
    '    If Not ModuleIDList Is Nothing Then
    '        If ModuleIDList.Count > 0 Then
    '            checkModule = True
    '        End If
    '    End If
    '    Try
    '        If checkCommunity Then
    '            If checkModule Then
    '                sqlCommand = sp_getGlobalUsageTimeByModule(ModuleIDList) & ComposeSQLList(" AND (", "MDAC_CommunityID ", CommunityIdList) & ComposeSQLList(" AND (", "MDAC_PersonID", PersonIDList)
    '            Else
    '                sqlCommand = sp_getGlobalUsageTimeByCommunity(CommunityIdList) & ComposeSQLList(" AND (", "CMAC_PersonID", PersonIDList)
    '            End If
    '        Else
    '            If checkModule Then
    '                sqlCommand = sp_getGlobalUsageTimeByModule(ModuleIDList) & ComposeSQLList(" AND (", "MDAC_PersonID", PersonIDList)
    '            Else
    '                sqlCommand = sp_getGlobalUsageTime & ComposeSQLList(" AND (", "LGAC_PersonID", PersonIDList)
    '            End If
    '        End If

    '        dbCommand = db.GetSqlStringCommand(sqlCommand)
    '        db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '        db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '        Dim sqlReader As SqlDataReader
    '        sqlReader = db.ExecuteReader(dbCommand)

    '        While sqlReader.Read()
    '            retval.totalTime += (sqlReader.Item("usageTime"))
    '            retval.nAccesses += (sqlReader.Item("nAccesses"))
    '        End While
    '    Catch ex As Exception
    '        Dim errorMessage As String = ex.Message
    '    End Try
    '    Return retval

    'End Function
#End Region

#Region "Details"
    Public Shared Function GetDailyAccessTime(idPerson As Integer, idCommunity As Integer, idModule As Integer, startDate As DateTime, ByRef endDate As DateTime) As List(Of dtoDetailsTime)
        Dim result As New List(Of dtoDetailsTime)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()
                Dim dbCommand As DbCommand
                Dim SqlCommand As String = ""
                If idModule >= -1 Then
                    If idCommunity >= 0 Then
                        If idPerson > 0 Then
                            SqlCommand = "sp_getDailyAccessByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        Else
                            SqlCommand = "sp_getDailyAccessByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        End If
                    Else
                        If idPerson > 0 Then
                            SqlCommand = "sp_getDailyAccessByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        Else
                            SqlCommand = "sp_getDailyAccessByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        End If
                    End If
                Else
                    If idCommunity >= 0 Then
                        If idPerson > 0 Then
                            SqlCommand = "sp_getDailyAccessByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                        Else
                            SqlCommand = "sp_getDailyAccessByCommunity"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                        End If
                    Else
                        If idPerson > 0 Then
                            SqlCommand = "sp_getDailyAccessByPerson"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                        Else
                            SqlCommand = "sp_getDailyAccess"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                        End If
                    End If
                End If
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim dto As New dtoDetailsTime
                        dto.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
                        dto.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        dto.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
                        dto.nAccesses = sqlReader.Item("nAccesses")
                        dto.UsageTime = sqlReader.Item("usageTime")
                        If idPerson > 0 Then
                            dto.IdPerson = idPerson
                        End If
                        result.Add(dto)
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
        Return result
    End Function
    Public Shared Function getHourlyAccess(ByVal idPerson As Integer, ByVal idCommunity As Integer, ByVal idModule As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As List(Of dtoDetailsTime)
        Dim result As New List(Of dtoDetailsTime)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
            Dim connection As DbConnection = db.CreateConnection()

            Using connection
                connection.Open()
                Dim dbCommand As DbCommand
                Dim SqlCommand As String = ""
                If idModule >= -1 Then
                    If idCommunity >= 0 Then
                        If idPerson > 0 Then
                            SqlCommand = "sp_getHourlyAccessByPersonByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        Else
                            SqlCommand = "sp_getHourlyAccessByCommunityByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        End If
                    Else
                        If idCommunity > 0 Then
                            SqlCommand = "sp_getHourlyAccessByPersonByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        Else
                            SqlCommand = "sp_getHourlyAccessByModule"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, idModule)
                        End If
                    End If
                Else
                    If idCommunity >= 0 Then
                        If idPerson > 0 Then
                            SqlCommand = "sp_getHourlyAccessByPersonByCommunity"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                        Else
                            SqlCommand = "sp_getHourlyAccessByCommunity"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, idCommunity)
                        End If
                    Else
                        If idPerson > 0 Then
                            SqlCommand = "sp_getHourlyAccessByPerson"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, idPerson)
                        Else
                            SqlCommand = "sp_getHourlyAccess"
                            dbCommand = db.GetStoredProcCommand(SqlCommand)
                        End If
                    End If
                End If

                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)

                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        Dim dto As New dtoDetailsTime
                        dto.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
                        dto.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
                        dto.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
                        dto.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
                        dto.nAccesses = sqlReader.Item("nAccesses")
                        dto.UsageTime = sqlReader.Item("usageTime")
                        If idPerson > 0 Then
                            dto.IdPerson = idPerson
                        End If
                        result.Add(dto)
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
        Return result
    End Function
    'Public Function getHourlyAccess(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '    Dim retval As New List(Of BaseStatAction)
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String
    '    Dim dbCommand As DbCommand
    '    Try
    '        If ModuleID >= -1 Then
    '            If CommunityID >= 0 Then
    '                If PersonID > 0 Then
    '                    sqlCommand = "sp_getHourlyAccessByPersonByCommunityByModule"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                Else
    '                    sqlCommand = "sp_getHourlyAccessByCommunityByModule"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                End If
    '            Else
    '                If PersonID > 0 Then
    '                    sqlCommand = "sp_getHourlyAccessByPersonByModule"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                Else
    '                    sqlCommand = "sp_getHourlyAccessByModule"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                End If
    '            End If
    '        Else
    '            If CommunityID >= 0 Then
    '                If PersonID > 0 Then
    '                    sqlCommand = "sp_getHourlyAccessByPersonByCommunity"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                Else
    '                    sqlCommand = "sp_getHourlyAccessByCommunity"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                End If
    '            Else
    '                If PersonID > 0 Then
    '                    sqlCommand = "sp_getHourlyAccessByPerson"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                Else
    '                    sqlCommand = "sp_getHourlyAccess"
    '                    dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                End If
    '            End If
    '        End If

    '        db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '        db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '        Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '            While sqlReader.Read()
    '                Dim oBaseStatAction As New BaseStatAction
    '                oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
    '                oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
    '                oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
    '                oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                If PersonID > 0 Then
    '                    oBaseStatAction.PersonID = PersonID
    '                End If
    '                retval.Add(oBaseStatAction)
    '            End While
    '            sqlReader.Close()
    '        End Using
    '    Catch ex As Exception
    '        Dim errore As String = ex.Message
    '    End Try
    '    Return retval
    'End Function
#End Region
    '#Region "Daily"
    '    Public Function getDailyModule(ByRef PersonID As Integer, ByRef CommunityId As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityId >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyModuleByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyModuleByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyModuleByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyModuleByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityId >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyModuleByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                    Else
    '                        sqlCommand = "sp_getDailyModuleByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyModuleByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getDailyModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If
    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If CommunityId <= 0 Then
    '                        oBaseStatAction.ModuleID = sqlReader.Item("ModuleID")
    '                    Else
    '                        oBaseStatAction.ModuleID = ModuleID
    '                    End If
    '                    oBaseStatAction.ID = oBaseStatAction.ModuleID
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getDailyCommunity(ByRef PersonID As Integer, ByRef CommunityId As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityId >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyCommunityByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyCommunityByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyCommunityByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityId >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyCommunityByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                    Else
    '                        sqlCommand = "sp_getDailyCommunityByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyCommunityByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getDailyCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If
    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If CommunityId <= 0 Then
    '                        oBaseStatAction.CommunityID = sqlReader.Item("CommunityID")
    '                    Else
    '                        oBaseStatAction.CommunityID = CommunityId
    '                    End If
    '                    oBaseStatAction.ID = oBaseStatAction.CommunityID
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getDailyPerson(ByRef PersonID As Integer, ByRef CommunityId As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityId >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyPersonByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyPersonByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityId >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyPersonByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                    Else
    '                        sqlCommand = "sp_getDailyPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityId)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyPersonByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getDailyPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If


    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    oBaseStatAction.PersonID = sqlReader.Item("PersonID")
    '                    oBaseStatAction.ID = oBaseStatAction.PersonID
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getDailyAccess(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyAccessByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyAccessByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyAccessByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getDailyAccessByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyAccessByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    Else
    '                        sqlCommand = "sp_getDailyAccessByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getDailyAccessByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getDailyAccess"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If
    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(4)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(0, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If PersonID > 0 Then
    '                        oBaseStatAction.PersonID = PersonID
    '                    End If
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '#End Region

    '#Region "Hourly"
    '    Public Function getHourlyPerson(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyPersonByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyPersonByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyPersonByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyPersonByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If

    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If PersonID <= 0 Then
    '                        oBaseStatAction.PersonID = sqlReader.Item("PersonID")
    '                    Else
    '                        oBaseStatAction.PersonID = PersonID
    '                    End If
    '                    oBaseStatAction.ID = oBaseStatAction.PersonID
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getHourlyAccess(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyAccessByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyAccessByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyAccessByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyAccessByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyAccessByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyAccessByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyAccessByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyAccess"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If

    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If PersonID > 0 Then
    '                        oBaseStatAction.PersonID = PersonID
    '                    End If
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getHourlyModule(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyModuleByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyModuleByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyModuleByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyModuleByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyModuleByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyModuleByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyModuleByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If

    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If ModuleID <= 0 Then
    '                        oBaseStatAction.ModuleID = sqlReader.Item("ModuleID")
    '                    Else
    '                        oBaseStatAction.ModuleID = ModuleID
    '                    End If
    '                    oBaseStatAction.ID = oBaseStatAction.ModuleID
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getHourlyCommunity(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of BaseStatAction)
    '        Dim retval As New List(Of BaseStatAction)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            If ModuleID >= -1 Then
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyCommunityByPersonByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyCommunityByCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyCommunityByPersonByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyCommunityByModule"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@ModuleID", DbType.Int32, ModuleID)
    '                    End If
    '                End If
    '            Else
    '                If CommunityID >= 0 Then
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyCommunityByPersonByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyCommunityByCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '                    End If
    '                Else
    '                    If PersonID > 0 Then
    '                        sqlCommand = "sp_getHourlyCommunityByPerson"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                        db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '                    Else
    '                        sqlCommand = "sp_getHourlyCommunity"
    '                        dbCommand = db.GetStoredProcCommand(sqlCommand)
    '                    End If
    '                End If
    '            End If

    '            db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oBaseStatAction As New BaseStatAction
    '                    oBaseStatAction.Hour = CStr(sqlReader.Item("loginDate")).Substring(11, 2)
    '                    oBaseStatAction.Day = CStr(sqlReader.Item("loginDate")).Substring(8, 2)
    '                    oBaseStatAction.Month = CStr(sqlReader.Item("loginDate")).Substring(5, 2)
    '                    oBaseStatAction.Year = CStr(sqlReader.Item("loginDate")).Substring(2, 2)
    '                    oBaseStatAction.nAccess = sqlReader.Item("nAccesses")
    '                    oBaseStatAction.UsageTime = sqlReader.Item("usageTime")
    '                    If CommunityID <= 0 Then
    '                        oBaseStatAction.CommunityID = sqlReader.Item("CommunityID")
    '                    Else
    '                        oBaseStatAction.CommunityID = CommunityID
    '                    End If
    '                    oBaseStatAction.ID = oBaseStatAction.CommunityID
    '                    retval.Add(oBaseStatAction)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function

    '#End Region


    '#Region "Access Presence"
    '    Public Function getCommunityAccessResults(ByRef PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
    '        Dim retval As New List(Of UserAccessResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_CommunityActionResult"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserAccessResult
    '                    oResult.UsageTime = sqlReader.Item("CMAC_UsageTime")
    '                    oResult.StartDate = sqlReader.Item("CMAC_AccessDate")
    '                    oResult.EndDate = sqlReader.Item("CMAC_LastActionDate")
    '                    oResult.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
    '                    If CommunityID <= 0 Then
    '                        oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
    '                    Else
    '                        oResult.CommunityID = CommunityID
    '                    End If
    '                    If PersonID <= 0 Then
    '                        oResult.PersonID = sqlReader.Item("CMAC_PersonID")
    '                    Else
    '                        oResult.PersonID = PersonID
    '                    End If
    '                    retval.Add(oResult)
    '                End While
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getPortalAccessResults(ByRef PersonID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
    '        Dim retval As New List(Of UserAccessResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_PortalActionResult"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserAccessResult
    '                    oResult.UsageTime = sqlReader.Item("LGAC_UsageTime")
    '                    oResult.StartDate = sqlReader.Item("LGAC_LoginDate")
    '                    oResult.EndDate = sqlReader.Item("LGAC_LastActionDate")
    '                    oResult.IsClosed = sqlReader.Item("LGAC_SessionClosed")
    '                    If PersonID <= 0 Then
    '                        oResult.PersonID = sqlReader.Item("LGAC_PersonID")
    '                    Else
    '                        oResult.PersonID = PersonID
    '                    End If
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function


    '    Public Function getCommunityUserWithResults(ByVal PersonID As Integer, ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As UserWithResult
    '        Dim iResponse As New UserWithResult With {.PersonID = PersonID, .CommunityID = CommunityID, .Result = 0}
    '        Dim iExec As Integer = 0
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand

    '        Try
    '            If CommunityID = -9999 Then
    '                sqlCommand = "sp_getPortalUserWithResults"
    '            Else
    '                sqlCommand = "sp_getCommunityUserWithResults"
    '            End If
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)

    '            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            If Not CommunityID = -9999 Then
    '                db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '            End If
    '            iExec = db.ExecuteNonQuery(dbCommand)
    '            If iExec > 0 Then
    '                iResponse.Result = 1
    '            End If
    '        Catch ex As Exception

    '        End Try
    '        Return iResponse
    '    End Function
    '    Public Function FindCommunitiesWithAccessResult(ByRef PersonID As Integer) As List(Of UserWithResult)
    '        Dim retval As New List(Of UserWithResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_GetCommuntyWithAccessResult"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            db.AddInParameter(dbCommand, "@PersonID", DbType.Int32, PersonID)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserWithResult
    '                    oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
    '                    oResult.PersonID = PersonID
    '                    oResult.Result = 1
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function FindPortalUsersWithAccessResult() As List(Of UserWithResult)
    '        Dim retval As New List(Of UserWithResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_GetPortalUsersWithAccessResult"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserWithResult
    '                    oResult.CommunityID = -9999
    '                    oResult.PersonID = sqlReader.Item("LGAC_PersonID")
    '                    oResult.Result = 1
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function

    '    Public Function FindUsersWithAccessResult(ByRef CommunityID As Integer) As List(Of UserWithResult)
    '        Dim retval As New List(Of UserWithResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_GetCommunityUsersWithAccessResult"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserWithResult
    '                    oResult.CommunityID = CommunityID
    '                    oResult.PersonID = sqlReader.Item("CMAC_PersonID")
    '                    oResult.Result = 1
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function


    '#End Region

    '#Region "Search Between Date"
    '    Public Function getUsersCommunityResultsBetweenDate(ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
    '        Dim retval As New List(Of UserAccessResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_CommunityResultsBetweenDate"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserAccessResult
    '                    oResult.UsageTime = sqlReader.Item("CMAC_UsageTime")
    '                    oResult.StartDate = sqlReader.Item("CMAC_AccessDate")
    '                    oResult.EndDate = sqlReader.Item("CMAC_LastActionDate")
    '                    oResult.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
    '                    If CommunityID <= 0 Then
    '                        oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
    '                    Else
    '                        oResult.CommunityID = CommunityID
    '                    End If
    '                    oResult.PersonID = sqlReader.Item("CMAC_PersonID")
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getUsersPortalResultsBetweenDate(ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
    '        Dim retval As New List(Of UserAccessResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_PortalResultsBetweenDate"
    '            dbCommand = db.GetStoredProcCommand(sqlCommand)
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserAccessResult
    '                    oResult.UsageTime = sqlReader.Item("LGAC_UsageTime")
    '                    oResult.StartDate = sqlReader.Item("LGAC_LoginDate")
    '                    oResult.EndDate = sqlReader.Item("LGAC_LastActionDate")
    '                    oResult.IsClosed = sqlReader.Item("LGAC_SessionClosed")
    '                    oResult.PersonID = sqlReader.Item("LGAC_PersonID")
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function

    '    Public Function getUsersCommunityResultsBetweenDate(ByVal PersonIDList As List(Of Integer), ByRef CommunityID As Integer, ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
    '        Dim retval As New List(Of UserAccessResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_CommunityResultsBetweenDate"
    '            dbCommand = db.GetSqlStringCommand(SQLcommunityResultsBetweenDate(PersonIDList))
    '            db.AddInParameter(dbCommand, "@CommunityID", DbType.Int32, CommunityID)
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserAccessResult
    '                    oResult.UsageTime = sqlReader.Item("CMAC_UsageTime")
    '                    oResult.StartDate = sqlReader.Item("CMAC_AccessDate")
    '                    oResult.EndDate = sqlReader.Item("CMAC_LastActionDate")
    '                    oResult.IsClosed = sqlReader.Item("CMAC_ExitCommunity")
    '                    If CommunityID <= 0 Then
    '                        oResult.CommunityID = sqlReader.Item("CMAC_CommunityID")
    '                    Else
    '                        oResult.CommunityID = CommunityID
    '                    End If
    '                    oResult.PersonID = sqlReader.Item("CMAC_PersonID")
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function
    '    Public Function getUsersPortalResultsBetweenDate(ByVal PersonIDList As List(Of Integer), ByRef startDate As DateTime, ByRef endDate As DateTime) As List(Of UserAccessResult)
    '        Dim retval As New List(Of UserAccessResult)
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim sqlCommand As String
    '        Dim dbCommand As DbCommand
    '        Try
    '            sqlCommand = "sp_PortalResultsBetweenDate"
    '            dbCommand = db.GetSqlStringCommand(SQLportalResultsBetweenDate(PersonIDList))
    '            db.AddInParameter(dbCommand, "@startDate", DbType.DateTime, startDate)
    '            db.AddInParameter(dbCommand, "@endDate", DbType.DateTime, endDate)
    '            Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
    '                While sqlReader.Read()
    '                    Dim oResult As New UserAccessResult
    '                    oResult.UsageTime = sqlReader.Item("LGAC_UsageTime")
    '                    oResult.StartDate = sqlReader.Item("LGAC_LoginDate")
    '                    oResult.EndDate = sqlReader.Item("LGAC_LastActionDate")
    '                    oResult.IsClosed = sqlReader.Item("LGAC_SessionClosed")
    '                    oResult.PersonID = sqlReader.Item("LGAC_PersonID")
    '                    retval.Add(oResult)
    '                End While
    '                sqlReader.Close()
    '            End Using
    '        Catch ex As Exception
    '            Dim errore As String = ex.Message
    '        End Try
    '        Return retval
    '    End Function

    '    Private Function SQLcommunityResultsBetweenDate(ByVal PersonIDList As List(Of Integer)) As String
    '        Dim iResponse As String = "SELECT     CMAC_GuidSessionID, CMAC_AccessDate, CMAC_LastActionDate, CMAC_PersonID, CMAC_CommunityID, CMAC_ExitCommunity, CMAC_AccessDay,CMAC_AccessMonth, CMAC_AccessHour, CMAC_AccessYear, CMAC_UsageTime FROM dbo.COMMUNITY_ACTION "

    '        iResponse &= " where (CMAC_UsageTime > 0) AND (CMAC_CommunityID=@CommunityID OR @CommunityID=-1) AND ((CMAC_AccessDate >= @startDate AND CMAC_AccessDate < @endDate) OR (CMAC_LastActionDate >= @startDate AND CMAC_LastActionDate < @endDate)) "
    '        iResponse &= " and " & ComposeSQLList("(", "CMAC_PersonID", PersonIDList) & " "
    '        iResponse &= " order by CMAC_AccessDate asc"

    '        Return iResponse
    '    End Function
    '    Private Function SQLportalResultsBetweenDate(ByVal PersonIDList As List(Of Integer)) As String
    '        Dim iResponse As String = "SELECT LGAC_ID, LGAC_LoginDate, LGAC_LastActionDate, LGAC_PersonID, LGAC_SessionClosed, LGAC_LoginHour, LGAC_UsageTime FROM LOGIN_ACTION"

    '        iResponse &= " where(LGAC_UsageTime > 0) AND ((LGAC_LoginDate >= @startDate AND LGAC_LoginDate < @endDate) OR (LGAC_LastActionDate >= @startDate AND LGAC_LastActionDate < @endDate)) "
    '        iResponse &= " and " & ComposeSQLList("(", "LGAC_PersonID", PersonIDList) & " "
    '        iResponse &= " order by LGAC_LoginDate asc"

    '        Return iResponse
    '    End Function
    '#End Region

#Region "Sumarry"
    Public Shared Function GetSummary(ByVal context As UsageContext, ByVal startDate As DateTime, ByVal endDate As DateTime) As dtoSummary
        Dim persons As New List(Of Integer), communities As New List(Of Integer), modules As New List(Of Integer)
        If context.ModuleID <> -2 Then
            modules.Add(context.ModuleID)
        End If
        If context.CommunityID >= 0 Then
            communities.Add(context.CommunityID)
        End If
        If context.UserID > 0 Then
            persons.Add(context.UserID)
        End If
        Return GetSummary(persons, communities, modules, startDate, endDate)
    End Function

    Public Shared Function GetSummary(ByVal persons As List(Of Integer), communities As List(Of Integer), modules As List(Of Integer), ByVal startDate As DateTime, ByVal endDate As DateTime) As dtoSummary
        Dim result As New dtoSummary

        Dim db As Database = DatabaseFactory.CreateDatabase(_databaseName)
        Try
            Dim connection As DbConnection = db.CreateConnection()

            Dim checkCommunity As Boolean = False
            Dim checkModule As Boolean = False
            If Not IsNothing(communities) Then
                checkCommunity = (communities.Count > 0)
            End If
            If Not IsNothing(modules) Then
                checkModule = (modules.Count > 0)
            End If

            Using connection
                connection.Open()

                Dim sqlCommand As String = ""
                If checkCommunity AndAlso checkModule Then
                    sqlCommand = sp_getGlobalUsageTimeByModule(modules) & ComposeSQLList(" AND (", "MDAC_CommunityID ", communities) & ComposeSQLList(" AND (", "MDAC_PersonID", persons)
                ElseIf checkCommunity Then
                    sqlCommand = sp_getGlobalUsageTimeByCommunity(communities) & ComposeSQLList(" AND (", "CMAC_PersonID", persons)
                ElseIf checkModule Then
                    sqlCommand = sp_getGlobalUsageTimeByModule(modules) & ComposeSQLList(" AND (", "MDAC_PersonID", persons)
                Else
                    sqlCommand = sp_getGlobalUsageTime() & ComposeSQLList(" AND (", "LGAC_PersonID", persons)
                End If

                Dim dbCommand As DbCommand = db.GetSqlStringCommand(sqlCommand)
                db.AddInParameter(dbCommand, "startDate", DbType.DateTime, startDate)
                db.AddInParameter(dbCommand, "endDate", DbType.DateTime, endDate)
                dbCommand.Connection = connection
                Using sqlReader As IDataReader = db.ExecuteReader(dbCommand)
                    While sqlReader.Read()
                        result.nAccesses += (sqlReader.Item("nAccesses"))
                        If Not IsDBNull(sqlReader.Item("usageTime")) Then
                            result.UsageTime += (sqlReader.Item("usageTime"))
                        End If
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
        Return result

    End Function
#End Region
End Class