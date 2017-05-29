Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports lm.WS.ActionStatistics.Domain
Imports lm.WS.ActionStatistics.Business
Imports lm.WS.ActionStatistics.DAL

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSactionStatistics
	Inherits System.Web.Services.WebService

#Region "Global"
    <WebMethod(Description:="Returns the usage time and number of accesses for each community")> _
    Public Function GetStatCommunity_global(ByVal PersonIDList As List(Of Integer), ByVal CommunityIDList As List(Of Integer), ByVal ModuleIDList As List(Of Integer), ByVal oStartDate As dtoDate, ByVal oEndDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        Return oDalActionStat.getGlobalCommunity(PersonIDList, CommunityIDList, ModuleIDList, oStartDate.ToDateTime, oEndDate.ToDateTime)
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses for each module")> _
    Public Function GetStatModule_global(ByVal PersonIDList As List(Of Integer), ByVal CommunityIDList As List(Of Integer), ByVal ModuleIDList As List(Of Integer), ByVal oStartDate As dtoDate, ByVal oEndDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        Return (oDalActionStat.getGlobalModule(oStartDate.ToDateTime, oEndDate.ToDateTime, ModuleIDList, CommunityIDList, PersonIDList))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses for each person")> _
    Public Function GetStatPerson_global(ByVal ModuleIDList As List(Of Integer), ByVal CommunityIDList As List(Of Integer), ByVal PersonIDList As List(Of Integer), ByVal oStartDate As dtoDate, ByVal oEndDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        Return (oDalActionStat.getGlobalPerson(oStartDate.ToDateTime, oEndDate.ToDateTime, ModuleIDList, CommunityIDList, PersonIDList))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses to the system")> _
    Public Function GetStatTotal(ByVal ModuleIDList As List(Of Integer), ByVal CommunityIDList As List(Of Integer), ByVal PersonIDList As List(Of Integer), ByVal oStartDate As dtoDate, ByVal oEndDate As dtoDate) As SummaryAction
        Dim oDalActionStat As New DALactionStat
        Return oDalActionStat.getTotal(oStartDate.ToDateTime, oEndDate.ToDateTime, ModuleIDList, CommunityIDList, PersonIDList)
    End Function
#End Region

#Region "Daily"
    <WebMethod(Description:="Returns the usage time and number of accesses for each module, for each day in interval")> _
    Public Function GetStatModule_daily(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'If endDate Is Nothing Then
        '    Dim newDateTime As DateTime = startDate.ToDateTime
        '    newDateTime.AddDays(1)
        '    Dim newEndDate As New dtoDate(newDateTime)
        '    endDate = newEndDate
        'End If
        Return (oDalActionStat.getDailyModule(PersonID, CommunityId, ModuleID, startDate.ToDateTime, endDate.ToDateTime))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses for each community, for each day in interval")> _
    Public Function GetStatCommunity_daily(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'If endDate Is Nothing Then
        '    Dim newDateTime As DateTime = startDate.ToDateTime
        '    newDateTime.AddDays(1)
        '    Dim newEndDate As New dtoDate(newDateTime)
        '    endDate = newEndDate
        'End If
        Return (oDalActionStat.getDailyCommunity(PersonID, CommunityId, ModuleID, startDate.ToDateTime, endDate.ToDateTime))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses for each person, for each day in interval")> _
    Public Function GetStatPerson_daily(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'If endDate Is Nothing Then
        '    Dim newDateTime As DateTime = startDate.ToDateTime
        '    newDateTime.AddDays(1)
        '    Dim newEndDate As New dtoDate(newDateTime)
        '    endDate = newEndDate
        'End If
        Return (oDalActionStat.getDailyPerson(PersonID, CommunityID, ModuleID, startDate.ToDateTime, endDate.ToDateTime))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses to the system, for each day in interval")> _
Public Function GetStatAccess_daily(ByVal PersonID As Integer, ByRef CommunityID As Integer, ByRef ModuleID As Integer, ByVal startDate As dtoDate, ByRef endDate As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'If endDate Is Nothing Then
        '    Dim newDateTime As DateTime = startDate.ToDateTime
        '    newDateTime.AddDays(1)
        '    Dim newEndDate As New dtoDate(newDateTime)
        '    endDate = newEndDate
        'End If
        Return (oDalActionStat.getDailyAccess(PersonID, CommunityID, ModuleID, startDate.ToDateTime, endDate.ToDateTime))
    End Function
#End Region

#Region "Hourly"

    <WebMethod(Description:="Returns the usage time and number of accesses for each person, for each hour in interval")> _
Public Function GetStatPerson_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'dayend creato cosi' perche': -la data ha ora = 0, e il controllo della store viene fatto anche su quella, quindi il giorno di fine e' il seguente
        Dim dayEnd As New dtoDate(day.Year, day.Month, day.Day, 23)
        Return (oDalActionStat.getHourlyPerson(PersonID, CommunityID, ModuleID, day.ToDateTime, dayEnd.ToDateTime))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses to the system, for each hour in interval")> _
Public Function GetStatAccess_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'dayend creato cosi' perche': -la data ha ora = 0, e il controllo della store viene fatto anche su quella, quindi il giorno di fine e' il seguente
        Dim dayEnd As New dtoDate(day.Year, day.Month, day.Day, 23)
        Return (oDalActionStat.getHourlyAccess(PersonID, CommunityID, ModuleID, day.ToDateTime, dayEnd.ToDateTime))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses for each Module, for each hour in interval")> _
Public Function GetStatModule_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'dayend creato cosi' perche': -la data ha ora = 0, e il controllo della store viene fatto anche su quella, quindi il giorno di fine e' il seguente
        Dim dayEnd As New dtoDate(day.Year, day.Month, day.Day, 23)
        Return (oDalActionStat.getHourlyModule(PersonID, CommunityID, ModuleID, day.ToDateTime, dayEnd.ToDateTime))
    End Function
    <WebMethod(Description:="Returns the usage time and number of accesses for each Community, for each hour in interval")> _
Public Function GetStatCommunity_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal day As dtoDate) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        'dayend creato cosi' perche': -la data ha ora = 0, e il controllo della store viene fatto anche su quella, quindi il giorno di fine e' il seguente
        Dim dayEnd As New dtoDate(day.Year, day.Month, day.Day, 23)
        Return (oDalActionStat.getHourlyCommunity(PersonID, CommunityID, ModuleID, day.ToDateTime, dayEnd.ToDateTime))
    End Function
#End Region

#Region "test"
    <WebMethod(Description:="global usage of system by a single user")> _
      Public Function testGetStatCommunity_global(ByVal isModuleList As Boolean, ByVal isCommunityList As Boolean, ByVal isPersonList As Boolean, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As List(Of BaseStatAction)

        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Dim oDalActionStat As New DALactionStat
        Dim PersonIDList As New List(Of Integer)
        Dim ModuleIDList As New List(Of Integer)
        Dim CommunityIDList As New List(Of Integer)
        If isPersonList Then
            PersonIDList.Add(1)
            PersonIDList.Add(11)
            PersonIDList.Add(149)
            PersonIDList.Add(3682)
        End If
        If isModuleList Then
            ModuleIDList.Add(-1)
            ModuleIDList.Add(28)
        End If
        If isCommunityList Then
            CommunityIDList.Add(0)
            CommunityIDList.Add(58)
        End If
        Return (GetStatCommunity_global(PersonIDList, CommunityIDList, ModuleIDList, oStartDate, oEndDate))

    End Function
    <WebMethod(Description:="global usage of a module by each user. ")> _
Public Function testGetStatPerson_global(ByVal isModuleList As Boolean, ByVal isCommunityList As Boolean, ByVal isPersonList As Boolean, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As BaseStatAction_List
        Dim oDalActionStat As New DALactionStat
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Dim PersonIDList As New List(Of Integer)
        Dim ModuleIDList As New List(Of Integer)
        Dim CommunityIDList As New List(Of Integer)
        If isPersonList Then
            PersonIDList.Add(1)
            PersonIDList.Add(11)
            PersonIDList.Add(149)
            PersonIDList.Add(3682)
        End If
        If isModuleList Then
            ModuleIDList.Add(-1)
            ModuleIDList.Add(28)
        End If
        If isCommunityList Then
            CommunityIDList.Add(0)
            CommunityIDList.Add(58)
        End If
        Dim retval As New BaseStatAction_List
        retval.statActionList = (oDalActionStat.getGlobalPerson(oStartDate.ToDateTime, oEndDate.ToDateTime, ModuleIDList, CommunityIDList, PersonIDList))
        Return retval
    End Function
    <WebMethod(Description:="global usage of a module by each user. ")> _
Public Function testGetStatModule_global(ByVal isCommunityList As Boolean, ByVal isPersonList As Boolean, ByVal isModuleList As Boolean, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As List(Of BaseStatAction)
        Dim oDalActionStat As New DALactionStat
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Dim PersonIDList As New List(Of Integer)
        Dim ModuleIDList As New List(Of Integer)
        Dim CommunityIDList As New List(Of Integer)
        If isPersonList Then
            PersonIDList.Add(1)
            PersonIDList.Add(11)
            PersonIDList.Add(149)
            PersonIDList.Add(3682)
        End If
        If isModuleList Then
            ModuleIDList.Add(-1)
            ModuleIDList.Add(28)
        End If
        If isCommunityList Then
            CommunityIDList.Add(0)
            CommunityIDList.Add(58)
        End If
        Return GetStatModule_global(PersonIDList, CommunityIDList, ModuleIDList, oStartDate, oEndDate)
    End Function

    <WebMethod(Description:="global usage of a module by each user. ")> _
Public Function testGetStatTotal(ByVal isCommunityList As Boolean, ByVal isPersonList As Boolean, ByVal isModuleList As Boolean, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As SummaryAction
        Dim oDalActionStat As New DALactionStat
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)

        Dim PersonIDList As New List(Of Integer)
        Dim ModuleIDList As New List(Of Integer)
        Dim CommunityIDList As New List(Of Integer)
        If isPersonList Then
            PersonIDList.Add(1)
            PersonIDList.Add(11)
            PersonIDList.Add(149)
            PersonIDList.Add(3682)
        End If
        If isModuleList Then
            ModuleIDList.Add(-1)
            ModuleIDList.Add(28)
        End If
        If isCommunityList Then
            CommunityIDList.Add(0)
            CommunityIDList.Add(58)
        End If
        Return GetStatTotal(ModuleIDList, CommunityIDList, PersonIDList, oStartDate, oEndDate)
    End Function

    '    <WebMethod(Description:="global usage of system by a single user, day by day")> _
    'Public Function testGetStatCommunity_hourly(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer) As BaseStatAction_List
    '        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
    '        Dim oDalActionStat As New DALactionStat
    '        Return GetStatCommunity_hourly(PersonID, CommunityId, ModuleID, oStartDate)
    '    End Function

    <WebMethod(Description:="global usage of system by a single user, day by day")> _
Public Function testGetStatModule_daily(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As List(Of BaseStatAction)
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Return GetStatModule_daily(PersonID, CommunityId, ModuleID, oStartDate, oEndDate)
    End Function
    <WebMethod(Description:="global usage of system by a single user, day by day")> _
  Public Function testGetStatCommunity_daily(ByVal PersonID As Integer, ByVal CommunityId As Integer, ByVal ModuleID As Integer, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As List(Of BaseStatAction)
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Return GetStatCommunity_daily(PersonID, CommunityId, ModuleID, oStartDate, oEndDate)
    End Function
    <WebMethod(Description:="global usage of system by a single user, day by day")> _
    Public Function testGetStatPerson_daily(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As List(Of BaseStatAction)
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Return GetStatPerson_daily(PersonID, CommunityID, ModuleID, oStartDate, oEndDate)
    End Function
    <WebMethod(Description:="global usage of system by a single user, day by day")> _
Public Function testGetStatAccess_daily(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal startYear As Integer, ByVal startMonth As Integer, ByVal startDay As Integer, ByVal endYear As Integer, ByVal endMonth As Integer, ByVal endDay As Integer) As List(Of BaseStatAction)
        Dim oStartDate As New dtoDate(startYear, startMonth, startDay)
        Dim oEndDate As New dtoDate(endYear, endMonth, endDay)
        Return GetStatAccess_daily(PersonID, CommunityID, ModuleID, oStartDate, oEndDate)
    End Function

    <WebMethod(Description:="global usage of system by a single user, day by day")> _
Public Function testGetStatPerson_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Year As Integer, ByVal Month As Integer, ByVal Day As Integer) As List(Of BaseStatAction)

        Dim oDay As New dtoDate(Year, Month, Day)
        Return GetStatPerson_hourly(PersonID, CommunityID, ModuleID, oDay)
    End Function
    <WebMethod(Description:="global usage of system by a single user, day by day")> _
Public Function testGetStatAccess_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Year As Integer, ByVal Month As Integer, ByVal Day As Integer) As List(Of BaseStatAction)
        Dim oDay As New dtoDate(Year, Month, Day)
        Return GetStatAccess_hourly(PersonID, CommunityID, ModuleID, oDay)
    End Function
    <WebMethod(Description:="global usage of system by a single user, day by day")> _
Public Function testGetStatCommunity_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Year As Integer, ByVal Month As Integer, ByVal Day As Integer) As List(Of BaseStatAction)
        Dim oDay As New dtoDate(Year, Month, Day)
        Return GetStatCommunity_hourly(PersonID, CommunityID, ModuleID, oDay)
    End Function
    <WebMethod(Description:="global usage of system by a single user, day by day")> _
Public Function testGetStatModule_hourly(ByVal PersonID As Integer, ByVal CommunityID As Integer, ByVal ModuleID As Integer, ByVal Year As Integer, ByVal Month As Integer, ByVal Day As Integer) As List(Of BaseStatAction)
        Dim oDay As New dtoDate(Year, Month, Day)
        Return GetStatModule_hourly(PersonID, CommunityID, ModuleID, oDay)
    End Function
    <WebMethod(Description:="UserOnlineCount")> _
      Public Function Test() As BaseStatAction_List
        Return Nothing
    End Function
#End Region
End Class