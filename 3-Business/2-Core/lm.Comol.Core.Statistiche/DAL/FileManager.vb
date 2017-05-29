Imports COL_DataLayer
Imports COL_BusinessLogic_v2

Public Class FileManager
    Public Shared Function GetFilePersStat(ByVal UserId As Integer, ByVal CommunityId As Integer)
        Dim oUserList As New List(Of FilePersonalStat)

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataReader As IDataReader
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_FileDownload_GetUserInfo"
            .CommandType = CommandType.StoredProcedure

            '@CommunityID int,
            oParam = objAccesso.GetAdvancedParameter("@CommunityID", CommunityId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            '@PersonID int
            oParam = objAccesso.GetAdvancedParameter("@PersonID", UserId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            oDataReader = objAccesso.GetdataReader(oRequest)
            Dim DataNull As New DateTime(2000, 1, 1)
            While oDataReader.Read
                Dim oFilePersStat As New FilePersonalStat
                With oFilePersStat
                    .FileID = oDataReader("FileID")
                    Try
                        .FileGuid = New System.Guid(oDataReader("FileGUID").ToString)
                    Catch ex As Exception
                        .FileGuid = System.Guid.Empty
                    End Try
                    .FileName = oDataReader("FileNome")
                    .FilePath = oDataReader("FilePath")
                    .NumDownload = GenericValidator.ValInteger(oDataReader("Download"), 0)
                    .LastDownload = GenericValidator.ValData(oDataReader("LastDown"), DataNull)
                End With

                oUserList.Add(oFilePersStat)
            End While
            oDataReader.Close()
        Catch ex As Exception
        End Try

        Return oUserList
    End Function

    Public Shared Function CountaDownload(ByVal UserId As Integer, ByVal CommunityId As Integer) As Integer
        Dim Count As Integer

        Dim oUserList As New List(Of FilePersonalStat)

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataReader As IDataReader
        Dim objAccesso As New COL_DataAccess

        With oRequest
            .Command = "sp_FileDownload_CountPersCom"
            .CommandType = CommandType.StoredProcedure

            '@CommunityID int,
            oParam = objAccesso.GetAdvancedParameter("@CommunityID", CommunityId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            '@PersonID int
            oParam = objAccesso.GetAdvancedParameter("@PersonID", UserId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False
        End With

        Try
            oDataReader = objAccesso.GetdataReader(oRequest)

            While oDataReader.Read
                Count = GenericValidator.ValInteger(oDataReader("TotaleDownload"), 0)
            End While
            oDataReader.Close()
        Catch ex As Exception
        End Try
        Return Count
    End Function

    Public Shared Function GetUserListCom(ByVal CommunityID As Integer, ByVal SearchBy As Integer, ByVal Value As String)
        Dim oUserList As List(Of StatFileUsr) = GetUserListCom(CommunityID)
        Dim Query = From oUF As StatFileUsr In oUserList Select oUF

        Select Case SearchBy
            Case 1 'nome
                Query = From oUF As StatFileUsr In oUserList Select oUF Where oUF.Nome Like "*" & Value & "*"
            Case 2 'cognome
                Query = From oUF As StatFileUsr In oUserList Select oUF Where oUF.Cognome Like "*" & Value & "*"
            Case 3 'all
                Query = From oUF As StatFileUsr In oUserList Select oUF Where ((oUF.Nome Like "*" & Value & "*") Or (oUF.Cognome Like "*" & Value & "*"))
        End Select

        oUserList = Query.ToList
        Return oUserList
    End Function
    Public Shared Function GetUserListCom(ByVal CommunityId As Integer) As List(Of StatFileUsr)
        Dim oList As New List(Of StatFileUsr)

        Dim oRequest As New COL_Request
        Dim oParam As New COL_Request.Parameter
        Dim oDataReader As IDataReader
        Dim objAccesso As New COL_DataAccess

        With oRequest
            'sp_SCORM_tinyStat_GetUserNumPlay 
            .Command = "sp_FileDownloadUserList"
            .CommandType = CommandType.StoredProcedure

            '@CMNT_Id int
            oParam = objAccesso.GetAdvancedParameter("@CommunityID", CommunityId, ParameterDirection.Input, SqlDbType.Int)
            .Parameters.Add(oParam)

            .Role = COL_Request.UserRole.Admin
            .transactional = False

        End With
        Dim DataNull As New DateTime(2000, 1, 1)

        Try
            oDataReader = objAccesso.GetdataReader(oRequest)
            While oDataReader.Read
                Dim oStFU As New StatFileUsr
                With oStFU
                    .Id = oDataReader("UserID")
                    .Nome = oDataReader("UserNome")
                    .Cognome = oDataReader("UserCognome")
                    .Ruolo = oDataReader("Ruolo")
                    Try
                        .NumDown = oDataReader("NumDown")
                    Catch ex As Exception
                        .NumDown = 0
                    End Try
                    Try
                        .KbDown = oDataReader("DimDown")
                    Catch ex As Exception
                        .KbDown = 0
                    End Try
                    .LastDown = COL_BusinessLogic_v2.GenericValidator.ValData(oDataReader("LastDown"), Nothing)
                End With

                oList.Add(oStFU)
            End While
            oDataReader.Close()
        Catch ex As Exception
        End Try

        Return oList
    End Function
End Class
