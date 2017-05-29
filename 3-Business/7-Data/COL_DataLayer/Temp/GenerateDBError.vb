Imports Comol.Entity.Configuration
Imports Comol.Entity
Imports Comol.Entity.Configuration.Facility
Imports System.Collections.Generic

Public Class GenerateDBError
    Public Shared Sub SendError(ByVal message As String, ByVal command As String, ByVal StackTrace As String, ByVal parameters As List(Of String))
        Dim oSettings As NotificationErrorSettings = ManagerConfigurationSettings.GetInstance

        If oSettings.isSendingEnabled(ErrorsNotificationService.ErrorType.DBerror) Then
            Dim oService As New ErrorsNotificationService.iErrorsNotificationServiceClient
            Dim oError As New ErrorsNotificationService.DBerror

            With oError
                .ComolUniqueID = oSettings.ComolUniqueID
                .UniqueID = Guid.NewGuid
                .Type = ErrorsNotificationService.ErrorType.DBerror
                .Persist = oSettings.FindPersistTo(ErrorsNotificationService.ErrorType.DBerror)
                .StackTrace = StackTrace
                .SQLcommand = command
                .SQLparameters = parameters
                .SentDate = Now
                .Day = .SentDate.Date
                .Message = message

            End With
            oService.sendDBerror(oError)
        End If
    End Sub
End Class