Imports System.Text
Imports COL_BusinessLogic_v2
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel

Public Class HTTPhandlerModuleUtility

#Region "Private Properties"
    Private _Impersonate As lm.Comol.Core.File.Impersonate
    Private _HttpContext As System.Web.HttpContext
    Private _ContextModule As ContextModule
    Private _ConfigurationFile As FileSettings.ConfigType
    Private _ObjectPath As ObjectFilePath
#End Region




#Region "Public Properties"
    Public ReadOnly Property HttpContext() As System.Web.HttpContext
        Get
            Return _HttpContext
        End Get
    End Property
    Public ReadOnly Property ConfigFilePath() As ObjectFilePath
        Get
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.GetSystemObjectPath(Me.SystemSettings.File.GetByCode(Me._ConfigurationFile))
            End If
            Return _ObjectPath
        End Get
    End Property
    'Public Property ConfigurationFile_() As FileSettings.ConfigType
    '	Get
    '		Return _ConfigurationFile
    '	End Get
    '	Set(ByVal value As FileSettings.ConfigType)
    '		_ConfigurationFile = value
    '	End Set
    'End Property

    Public ReadOnly Property LocalizedMail() As MailLocalized
        Get
            Try
                Return ManagerConfiguration.GetMailLocalized(Me.CurrentLanguage)
            Catch ex As Exception

            End Try
            Return Nothing
        End Get
    End Property


    Private ReadOnly Property ContextModule() As ContextModule
        Get
            If IsNothing(_ContextModule) Then
                _ContextModule = New ContextModule
                _ContextModule.UserID = Me.GetUserIdBySession
                _ContextModule.CommunityID = Me.GetComunityIdBySession
            End If
            Return _ContextModule
        End Get
    End Property

    Public ReadOnly Property CurrentLanguage() As Lingua
        Get
            If IsNothing(Me.ContextModule.Language) Then
                Me.ContextModule.Language = GetLanguageBySession()
            End If
            Return Me.ContextModule.Language
        End Get
    End Property
    Public ReadOnly Property CurrentCommunityID() As Integer
        Get
            Return Me.ContextModule.CommunityID
        End Get
    End Property
    Public ReadOnly Property CurrentUserID() As Integer
        Get
            Return Me.ContextModule.UserID
        End Get
    End Property
    Public ReadOnly Property NotificationSender() As NotificationService.iNotificationServiceClient
        Get
            Dim oSender As NotificationService.iNotificationServiceClient = Nothing
            Try
                oSender = New NotificationService.iNotificationServiceClient
            Catch ex As Exception

            End Try
            Return oSender
        End Get
    End Property
    Public Property Impersonation As lm.Comol.Core.File.Impersonate
        Get
            Return _Impersonate
        End Get
        Set(value As lm.Comol.Core.File.Impersonate)
            _Impersonate = value
        End Set
    End Property

#End Region

    Sub New(ByVal oHttpContext As System.Web.HttpContext)
        _HttpContext = oHttpContext
    End Sub
    Sub New(ByVal oHttpContext As System.Web.HttpContext, ByVal oConfig As FileSettings.ConfigType)
        _HttpContext = oHttpContext
        Me._ConfigurationFile = oConfig
    End Sub


#Region ""
    Private Function GetLanguageBySession() As Lingua
        Try
            Dim oSessionLanguage As Lingua = Nothing

            If Not IsNothing(_HttpContext.Session("LinguaID")) AndAlso TypeOf (_HttpContext.Session("LinguaID")) Is Integer Then
                oSessionLanguage = ManagerLingua.GetByID(_HttpContext.Session("LinguaID"))
            End If
            If Not IsNothing(oSessionLanguage) Then
                Return oSessionLanguage
            End If
        Catch ex As Exception

        End Try
        Dim oLingua As Lingua = ManagerLingua.GetDefault
        If IsNothing(oLingua) Then
            Return Me.SystemSettings.DefaultLanguage
        End If
        Return oLingua
    End Function
    Private Function GetComunityIdBySession() As Integer
        Try
            If IsNumeric(_HttpContext.Session("idComunita")) Then
                Return CInt(_HttpContext.Session("idComunita"))
            End If
        Catch ex As Exception
        End Try
        Return -1
    End Function
    Private Function GetUserIdBySession() As Integer
        Try
            If Not IsNothing(_HttpContext.Session("objPersona")) AndAlso TypeOf (_HttpContext.Session("objPersona")) Is COL_Persona Then
                Return DirectCast(_HttpContext.Session("objPersona"), COL_Persona).ID
            End If
        Catch ex As Exception

        End Try
        Return 0
    End Function
#End Region

#Region "Path Functions"
    Public ReadOnly Property BaseUrl() As String
        Get
            Dim url As String = _HttpContext.Request.ApplicationPath
            If url.EndsWith("/") Then
                Return url
            Else
                Return url + "/"
            End If
        End Get
    End Property
    Public ReadOnly Property BaseUrlDrivePath() As String
        Get
            Return _HttpContext.Server.MapPath(Me.BaseUrl())
        End Get
    End Property
#End Region

#Region "Decrypt Urls"
    Public Sub RedirectToEncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType)
        Me._HttpContext.Response.Redirect(Me.EncryptedUrl(UrlPage, UrlQuerystring, oTypeEnc), True)
    End Sub
    Private Function CryptQuerystring(ByVal data As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        Return Enc.ArmorURL(data)
    End Function
    Public Function EncryptedUrl(ByVal UrlPage As String, ByVal UrlQuerystring As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Redirect As String = Me.ApplicationUrlBase

        Redirect &= UrlPage & "?" & Me.CryptQuerystring(UrlQuerystring, oTypeEnc)
        Return Redirect
    End Function
    Public Function DecryptQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        If Enc.DecryptVerifyURL(_HttpContext.Request.QueryString) Then
            Return Enc.Querystring(Value)
        Else
            Return ""
        End If
    End Function
    Public Function DecryptSMARTQueryString(ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As Integer
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        Dim FileId As Integer = Enc.DecryptSMARTVerifyURL(_HttpContext.Request.QueryString)
        If FileId > 0 Then
            Return FileId
        Else
            Return 0
        End If
    End Function
    Public Shared Function DecryptQueryString(ByVal oHttpContext As System.Web.HttpContext, ByVal Value As String, ByVal oTypeEnc As SecretKeyUtil.EncType) As String
        Dim Enc As New EncryptQueryString(SecretKeyUtil.HashKey, SecretKeyUtil.CryptKey(oTypeEnc))
        If Enc.DecryptVerifyURL(oHttpContext.Request.QueryString) Then
            Return Enc.Querystring(Value)
        Else
            Return ""
        End If
    End Function
#End Region

#Region "NEW REPOSITORY"
    Public Sub DirectTrasmittFile(ByVal oImpersonate As lm.Comol.Core.File.Impersonate, ByVal pathFile As String, ByVal fileName As String, ByVal fileExtension As String, ByVal ContentType As String, ByVal cookie As HttpCookie)
        _Impersonate = oImpersonate
        If Not String.IsNullOrEmpty(pathFile) Then
            Dim Quote As String = """"
            Dim oFileInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(pathFile)
            Dim DirectWrite As Boolean = False
            Dim DownloadInline As Boolean = False
            If Not IsNothing(cookie) Then
                _HttpContext.Response.AppendCookie(cookie)
            End If
            If Not String.IsNullOrEmpty(fileExtension) Then
                fileExtension = fileExtension.ToLower
            End If
            Select Case fileExtension
                Case ".htm", ".html", ".asp", ".aspx"
                    DirectWrite = True
                Case ".xml"
                    Exit Select
                Case ".txt", ".ini", ".log"
                    _HttpContext.Response.ContentType = "text/plain"
                Case ".jpg"
                    _HttpContext.Response.ContentType = String.Format("image/JPEG;name={0}{1}{0}", Quote, fileName & fileExtension)
                Case ".gif", ".png", ".bmp"
                    _HttpContext.Response.ContentType = String.Format("image/{0};name={1}{2}{1}", fileExtension.TrimStart("."), Quote, fileName & fileExtension)
                Case ".tif"
                    _HttpContext.Response.ContentType = String.Format("image/tiff;name={0}{1}{0}", Quote, fileName & fileExtension)
                Case ".doc"
                    _HttpContext.Response.ContentType = "Application/msword"
                Case ".xls"
                    _HttpContext.Response.ContentType = "Application/x-msexcel"
                Case ".pdf"
                    _HttpContext.Response.ContentType = "Application/pdf"
                Case ".ppt", ".pps"
                    _HttpContext.Response.ContentType = "Application/vnd.ms-powerpoint"
                Case ".zip"
                    _HttpContext.Response.ContentType = "application/x-zip-compressed"
                Case ".exe"
                    _HttpContext.Response.ContentType = "application/octet-stream"
                Case ".docx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                Case ".dotx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                Case ".pptx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                Case ".ppsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow"
                Case ".potx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                Case ".xltx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.template"
                Case ".xlsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            End Select
            _HttpContext.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}{1}{0}", """", RecodeFileName(fileName)))
            Me.WriteFileToHttpRequest(oImpersonate, pathFile)
        End If
    End Sub
    Public Sub DownloadRepositoryFile(ByVal oImpersonate As lm.Comol.Core.File.Impersonate, ByVal Link As lm.Comol.Core.DomainModel.ModuleLink, saveExecution As Boolean, ByVal UserId As Integer, ByVal PathFile As String, ByVal ItemName As String, ByVal ItemExtension As String, ByVal ContentType As String, ByVal Query As String, Optional ByVal ToDisplayOnly As Boolean = False)
        _Impersonate = oImpersonate
        If Not String.IsNullOrEmpty(PathFile) Then
            Dim Quote As String = """"
            Dim oFileInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(PathFile)
            Dim DirectWrite As Boolean = False
            Dim DownloadInline As Boolean = False
            If Not String.IsNullOrEmpty(Query) Then
                If (Query.StartsWith("?")) Then
                    Query = Query.Remove(0, 1)
                End If
                Dim cookie = New HttpCookie("fileDownload", Query)
                cookie.Expires = Now.AddMinutes(5)
                _HttpContext.Response.AppendCookie(cookie)
            End If

            If Not String.IsNullOrEmpty(ItemExtension) Then
                ItemExtension = ItemExtension.ToLower
            End If
            Select Case ItemExtension
                Case ".htm", ".html", ".asp", ".aspx"
                    DirectWrite = True
                Case ".xml"
                    Exit Select
                Case ".txt", ".ini", ".log"
                    _HttpContext.Response.ContentType = "text/plain"
                Case ".jpg"
                    _HttpContext.Response.ContentType = String.Format("image/jpeg;name={0}{1}{0}", Quote, ItemName & ItemExtension)
                Case ".gif", ".png", ".bmp"
                    _HttpContext.Response.ContentType = String.Format("image/{0};name={1}{2}{1}", ItemExtension.TrimStart("."), Quote, ItemName & ItemExtension)
                Case ".tif"
                    _HttpContext.Response.ContentType = String.Format("image/tiff;name={0}{1}{0}", Quote, ItemName & ItemExtension)
                Case ".doc"
                    _HttpContext.Response.ContentType = "Application/msword"
                Case ".xls"
                    _HttpContext.Response.ContentType = "Application/x-msexcel"
                Case ".pdf"
                    _HttpContext.Response.ContentType = "Application/pdf"
                Case ".ppt", ".pps"
                    _HttpContext.Response.ContentType = "Application/vnd.ms-powerpoint"
                Case ".zip"
                    _HttpContext.Response.ContentType = "application/x-zip-compressed"
                Case ".exe"
                    _HttpContext.Response.ContentType = "application/octet-stream"
                Case ".docx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                Case ".dotx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                Case ".pptx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                Case ".ppsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow"
                Case ".potx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                Case ".xltx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.template"
                Case ".xlsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Case Else
                    If String.IsNullOrEmpty(ContentType) Then
                        Dim MimeType As String = ""
                        Try
                            If ItemExtension <> "" Then
                                MimeType = Me.SystemSettings.Extension.FindMimeType(ItemExtension.ToLower)
                            End If
                        Catch ex As Exception
                            MimeType = ""
                        End Try
                        If MimeType <> "" Then
                            _HttpContext.Response.ContentType = MimeType
                        End If
                    Else
                        _HttpContext.Response.ContentType = ContentType
                    End If
            End Select
            If DirectWrite Then
                Me.WriteFileToHttpRequest(oImpersonate, Link, UserId, PathFile, ToDisplayOnly, saveExecution)
            Else
                Me.Add_Disposition_Attachment(oImpersonate, Link, UserId, PathFile, ItemName, ToDisplayOnly, saveExecution)
            End If

        End If
    End Sub
    Public Sub SendToErrorPage(ByVal oError As dtoError, ByVal query As String)
        If oError.Settings.isSendingEnabled(ErrorsNotificationService.ErrorType.FileError) Then
            Dim oService As New ErrorsNotificationService.iErrorsNotificationServiceClient
            Dim oErrorMessage As New ErrorsNotificationService.FileError

            With oErrorMessage
                .ComolUniqueID = oError.Settings.ComolUniqueID
                .UniqueID = Guid.NewGuid
                .Type = ErrorsNotificationService.ErrorType.FileError
                .Persist = oError.Settings.FindPersistTo(ErrorsNotificationService.ErrorType.FileError)
                .SentDate = Now
                .Day = .SentDate.Date
                .Message = oError.ToString & " " & oError.FileName
                .CommunityID = oError.CommunityID
                .UserID = oError.UserID
            End With
            oService.sendFileError(oErrorMessage)
        End If

        Dim PreserveUrl As Boolean = False
        PreserveUrl = (oError.ErrorType = ItemRepositoryStatus.NotLoggedIn)

        Dim oRemotePost As New RemotePost
        oRemotePost.Url = oError.BaseUrl & "Modules/Repository/CommunityRepositoryItemError.aspx?PageSender=" & RepositoryPage.DownloadingPage.ToString & "&PreserveDownloadUrl=" & PreserveUrl.ToString

        oRemotePost.Add("FILE_C_Name_0", oError.FileName)
        oRemotePost.Add("FILE_C_isFile_0", "True")
        oRemotePost.Add("FILE_C_Status_0", oError.ErrorType.ToString)
        oRemotePost.Add("FILE_C_FolderId_0", oError.FolderID)
        oRemotePost.Add("FILE_C_SavedFilePath_0", "")
        oRemotePost.Add("FILE_C_SavedName_0", "")
        oRemotePost.Add("FILE_C_SavedExtension_0", oError.Extension)

        oRemotePost.Add("ForUserID", oError.ForUserID.ToString)
        oRemotePost.Add("PreloadedItemID", oError.FileID)
        oRemotePost.Add("FolderId", oError.FolderID)
        oRemotePost.Add("Language", oError.Language)
        oRemotePost.Add("CommunityID", oError.CommunityID)
        oRemotePost.Post(query)
    End Sub
    Public Sub SendToErrorPage(ByVal oError As dtoInternalError)
        If oError.Settings.isSendingEnabled(ErrorsNotificationService.ErrorType.FileError) Then
            Dim oService As New ErrorsNotificationService.iErrorsNotificationServiceClient
            Dim oErrorMessage As New ErrorsNotificationService.FileError

            With oErrorMessage
                .ComolUniqueID = oError.Settings.ComolUniqueID
                .UniqueID = Guid.NewGuid
                .Type = ErrorsNotificationService.ErrorType.FileError
                .Persist = oError.Settings.FindPersistTo(ErrorsNotificationService.ErrorType.FileError)
                .SentDate = Now
                .Day = .SentDate.Date
                .Message = oError.ToString & " " & oError.FileName
                .CommunityID = oError.CommunityId
                .UserID = oError.UserID
            End With
            oService.sendFileError(oErrorMessage)
        End If

        oError.ErrorType = ItemRepositoryStatus.NotLoggedIn

        Dim oRemotePost As New RemotePost
        oRemotePost.Url = oError.BaseUrl & lm.Comol.Core.BaseModules.Errors.Domain.RootObject.Default(oError.ErrorType <> ItemRepositoryStatus.NotLoggedIn, False, False, False, True)
        oRemotePost.Add("ForUserID", oError.ForUserID.ToString)
        oRemotePost.Add("Language", oError.Language)
        oRemotePost.Add("CommunityID", oError.CommunityId)
        oRemotePost.Post()
    End Sub


#End Region


#Region "NEw Download function"
    Private Sub Add_Disposition_Attachment(ByVal oImpersonate As lm.Comol.Core.File.Impersonate, ByVal Link As lm.Comol.Core.DomainModel.ModuleLink, ByVal UserId As Integer, ByVal FileFullPath As String, ByVal FileName As String, ByVal ToDisplayOnly As Boolean, saveExecution As Boolean)
        Dim Quote As String = """"
        _HttpContext.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}{1}{0}", """", RecodeFileName(FileName)))
        Me.WriteFileToHttpRequest(oImpersonate, Link, UserId, FileFullPath, ToDisplayOnly, saveExecution)
    End Sub
    Private Sub WriteFileToHttpRequest(ByVal oImpersonate As lm.Comol.Core.File.Impersonate, ByVal Link As lm.Comol.Core.DomainModel.ModuleLink, ByVal UserId As Integer, ByVal FileFullPath As String, ByVal ToDisplayOnly As Boolean, saveExecution As Boolean)
        Try
            If ToDisplayOnly Then
                _HttpContext.Response.WriteFile(FileFullPath)
            Else
                _HttpContext.Response.TransmitFile(FileFullPath)
            End If
            _HttpContext.Response.Flush()
        Catch ex As Exception
            '   oImpersonate.UndoImpersonation()
        Finally
            oImpersonate.UndoImpersonation()
            oImpersonate = Nothing
        End Try
        Dim oSender As PermissionService.IServicePermission = Nothing
        Try
            If Not IsNothing(Link) AndAlso Link.AutoEvaluable AndAlso saveExecution Then

                oSender = New PermissionService.ServicePermissionClient
                If Not IsNothing(oSender) Then
                    oSender.ExecutedActionForExternal(Link.Id, True, True, 100, True, 100, UserId, GetExternalUsers(), Nothing)
                    Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                    service.Close()
                    service = Nothing
                End If
            End If
        Catch ex As Exception
            If Not IsNothing(oSender) Then
                Dim service As System.ServiceModel.ClientBase(Of PermissionService.IServicePermission) = DirectCast(oSender, System.ServiceModel.ClientBase(Of PermissionService.IServicePermission))
                service.Abort()
                service = Nothing
            End If
        End Try
        '    Dim closeThread As New System.Threading.Thread(AddressOf HTTPhandlerModuleUtility.CloseImpersonation(oImpersonate))
        '   closeThread.Start()
        _HttpContext.Response.End()
    End Sub
    Private Sub WriteFileToHttpRequest(ByVal oImpersonate As lm.Comol.Core.File.Impersonate, ByVal fileFullPath As String)
        Try
            _HttpContext.Response.TransmitFile(fileFullPath)
            _HttpContext.Response.Flush()
        Catch ex As Exception
            '   oImpersonate.UndoImpersonation()
        Finally
            oImpersonate.UndoImpersonation()
            oImpersonate = Nothing
        End Try
        _HttpContext.Response.End()
    End Sub

#End Region
    Private Function GetExternalUsers() As Dictionary(Of String, Long)
        Dim result As New Dictionary(Of String, Long)
        If Not IsNothing(_HttpContext) AndAlso Not IsNothing(_HttpContext.Session) Then
            If (From k In _HttpContext.Session.Keys Where k = "TICKET.CurrentExtUser" Select k).Any() Then
                Dim tUser As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User = Nothing
                Try
                    tUser = DirectCast(_HttpContext.Session("TICKET.CurrentExtUser"), lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
                Catch ex As Exception
                End Try
                If Not IsNothing(tUser) Then
                    result.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode, tUser.UserId)
                End If

            End If
        End If
        Return result
    End Function
    Public Shared Sub CloseImpersonation(ByVal oImpersonate As lm.Comol.Core.File.Impersonate)
        System.Threading.Thread.Sleep(10000)
        If Not IsNothing(oImpersonate) Then
            oImpersonate.Dispose()

        End If
    End Sub

#Region "Download functions"
    Public Sub DownloadErrorMessage(ByVal oType As ErrorSettings.ErrorType, ByVal InternalDisplay As Boolean, Optional ByVal EmailMessage As String = "", Optional ByVal SendMail As Boolean = False)
        Try
            If Me.SystemSettings.Mail.isErrorSendingActivated AndAlso SendMail AndAlso EmailMessage <> "" Then
                Dim oLocalizedMail As MailLocalized = Me.LocalizedMail
                Dim oMail As New COL_E_Mail(oLocalizedMail)

                oMail.IndirizziTO.Add(Me.LocalizedMail.SendErrorTo)
                oMail.Mittente = Me.LocalizedMail.ErrorSender
                oMail.Oggetto = Me.LocalizedMail.ErrorSubject
                oMail.Body = EmailMessage & vbCrLf & "oType=" & oType.ToString
                oMail.InviaMailHTML()
            End If
            RedirectToEncryptedUrl("Errori/ShowErrors.aspx?", IIf(InternalDisplay, "InternalDisplay=true&", "") & "ErroreID=" & oType, SecretKeyUtil.EncType.Altro)
        Catch ex As Exception

        End Try


    End Sub

    'Public Sub DisplayFile(ByVal PathFile As String)
    '    Me.DownloadFile(0, PathFile, True)
    'End Sub



    Public Sub DownloadFile(ByVal LinkID As Long, ByVal PathFile As String, Optional ByVal ToDisplayOnly As Boolean = False) ' ByVal Query As String, ByVal AllowRefresh As Boolean, ByVal ToDisplayOnly As Boolean) '
        If Not String.IsNullOrEmpty(PathFile) Then
            Dim Quote As String = """"
            Dim oFileInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(PathFile)

            '	_HttpContext.Response.Charset = "ISO-8859-1"
            '= System.Text.Encoding.UTF8



            Select Case oFileInfo.Extension.ToLower
                Case ".htm", ".html", ".asp", ".aspx"
                    Me.WriteFileToHttpRequest(LinkID, PathFile, ToDisplayOnly)
                Case ".xml"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".txt", ".ini", ".log"
                    _HttpContext.Response.ContentType = "text/plain"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)
                Case ".jpg"
                    _HttpContext.Response.ContentType = String.Format("image/JPEG;name={0}{1}{0}", Quote, oFileInfo.Name)
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".gif", ".png", ".bmp"
                    _HttpContext.Response.ContentType = String.Format("image/{0};name={1}{2}{1}", oFileInfo.Extension.TrimStart("."), Quote, oFileInfo.Name)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)

                Case ".tif"
                    _HttpContext.Response.ContentType = String.Format("image/tiff;name={0}{1}{0}", Quote, oFileInfo.Name)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)

                Case ".doc"
                    _HttpContext.Response.ContentType = "Application/msword"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)

                Case ".xls"
                    _HttpContext.Response.ContentType = "Application/x-msexcel"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)

                Case ".pdf"
                    _HttpContext.Response.ContentType = "Application/pdf"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                    ''Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)

                Case ".ppt", ".pps"
                    _HttpContext.Response.ContentType = "Application/vnd.ms-powerpoint"
                    'Me.Add_Disposition_Inline(context, PathFile, oFileInfo.Name)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".zip"
                    _HttpContext.Response.ContentType = "application/x-zip-compressed"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".exe"
                    _HttpContext.Response.ContentType = "application/octet-stream"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".docx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".dotx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)

                Case ".pptx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".ppsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".potx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".xltx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.template"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case ".xlsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
                Case Else
                    Dim MimeType As String = ""
                    Try
                        MimeType = Me.SystemSettings.Extension.FindMimeType(oFileInfo.Extension.ToLower)
                    Catch ex As Exception
                        MimeType = ""
                    End Try
                    If MimeType <> "" Then
                        _HttpContext.Response.ContentType = MimeType
                    End If
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oFileInfo.Name, ToDisplayOnly)
            End Select
        End If
    End Sub

    Public Sub DownloadFile(ByVal LinkID As Long, ByVal PathFile As String, ByVal oBasefile As lm.Comol.Core.DomainModel.BaseFile, ByVal Query As String, Optional ByVal ToDisplayOnly As Boolean = False)
        If Not String.IsNullOrEmpty(PathFile) Then
            Dim Quote As String = """"
            'Dim oFileInfo As System.IO.FileInfo = lm.Comol.Core.File.ContentOf.File_Info(PathFile)
            Dim ClientFileExtension As String = oBasefile.Extension.ToLower
            Dim ClientFileName As String = oBasefile.DisplayName

            If Not String.IsNullOrEmpty(Query) Then
                If (Query.StartsWith("?")) Then
                    Query = Query.Remove(0, 1)
                End If
                _HttpContext.Response.AppendCookie(New HttpCookie("fileDownload", Query))
            End If

            Select Case ClientFileExtension
                Case ".htm", ".html", ".asp", ".aspx"
                    Me.WriteFileToHttpRequest(LinkID, PathFile, ToDisplayOnly)
                Case ".xml"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".txt", ".ini", ".log"
                    _HttpContext.Response.ContentType = "text/plain"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".jpg"
                    _HttpContext.Response.ContentType = String.Format("image/JPEG;name={0}{1}{0}", Quote, ClientFileName)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".gif", ".png", ".bmp"
                    _HttpContext.Response.ContentType = String.Format("image/{0};name={1}{2}{1}", ClientFileExtension.TrimStart("."), Quote, ClientFileName)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".tif"
                    _HttpContext.Response.ContentType = String.Format("image/tiff;name={0}{1}{0}", Quote, ClientFileName)
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".docx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".doc"
                    _HttpContext.Response.ContentType = "Application/msword"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)

                Case ".dotx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)

                Case ".pptx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".ppsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".potx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.template"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".xltx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.template"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".xlsx"
                    _HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".xls"
                    _HttpContext.Response.ContentType = "Application/x-msexcel"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".pdf"
                    _HttpContext.Response.ContentType = "Application/pdf"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".ppt", ".pps"
                    _HttpContext.Response.ContentType = "Application/vnd.ms-powerpoint"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".zip"
                    _HttpContext.Response.ContentType = "application/x-zip-compressed"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case ".exe"
                    _HttpContext.Response.ContentType = "application/octet-stream"
                    Me.Add_Disposition_Attachment(LinkID, PathFile, ClientFileName, ToDisplayOnly)
                Case Else
                    'Dim MimeType As String = ""
                    'Try
                    '	MimeType = Me.SystemSettings.Extension.FindMimeType(ClientFileExtension)
                    'Catch ex As Exception
                    '	MimeType = ""
                    'End Try
                    'If MimeType <> "" Then
                    '	_HttpContext.Response.ContentType = MimeType
                    'End If
                    _HttpContext.Response.ContentType = oBasefile.ContentType
                    Me.Add_Disposition_Attachment(LinkID, PathFile, oBasefile.DisplayName, ToDisplayOnly)
            End Select
        End If
    End Sub
    Private Sub Add_Disposition_Attachment(ByVal LinkID As Long, ByVal FileFullPath As String, ByVal FileName As String, ByVal ToDisplayOnly As Boolean)
        Dim Quote As String = """"
        '_HttpContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" & FileName)
        _HttpContext.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}{1}{0}", """", RecodeFileName(FileName)))
        Me.WriteFileToHttpRequest(LinkID, FileFullPath, ToDisplayOnly)
    End Sub
    Private Sub Add_Disposition_Inline(ByVal LinkID As Long, ByVal FileFullPath As String, ByVal FileName As String, ByVal ToDisplayOnly As Boolean)
        Dim Quote As String = """"
        '_HttpContext.Response.AppendHeader("Content-Disposition", "inline;filename=" & Quote & FileName & Quote)
        _HttpContext.Response.AppendHeader("Content-Disposition", String.Format("inline;filename={0}{1}{0}", """", RecodeFileName(FileName)))
        Me.WriteFileToHttpRequest(LinkID, FileFullPath, ToDisplayOnly)
    End Sub
    Private Sub WriteFileToHttpRequest(ByVal LinkID As Long, ByVal FileFullPath As String, ByVal ToDisplayOnly As Boolean)
        If ToDisplayOnly Then
            Try
                _HttpContext.Response.WriteFile(FileFullPath)
                _HttpContext.Response.End()
            Catch ex As Exception
                'Dim oMail As New MailDBerrori
                'oMail.Oggetto = "Errore  TransmitFile"
                'oMail.Body = "PathFile=" & ex.Message
                'oMail.InviaMail()
            End Try
        Else
            Try
                _HttpContext.Response.TransmitFile(FileFullPath)
                _HttpContext.Response.End()
            Catch ex As Exception
                'Dim oMail As New MailDBerrori
                'oMail.Oggetto = "Errore  TransmitFile"
                'oMail.Body = "PathFile=" & ex.Message
                'oMail.InviaMail()
            End Try
        End If

    End Sub

    'Private Shared Function ToHexString(ByVal chr As Char) As String
    '	Dim utf8 As New UTF8Encoding()
    '	Dim encodedBytes As Byte() = utf8.GetBytes(chr.ToString())
    '	Dim builder As New StringBuilder()
    '	For index As Integer = 0 To encodedBytes.Length - 1
    '		builder.AppendFormat("%{0}", Convert.ToString(encodedBytes(index), 16))
    '	Next

    '	Return builder.ToString()
    'End Function

    'Private Shared Function ToHexString2(ByVal chr As String) As String
    '	Dim utf8 As New UTF8Encoding()
    '	Dim encodedBytes As Byte() = utf8.GetBytes(chr.ToString())
    '	Dim builder As New StringBuilder()
    '	For index As Integer = 0 To encodedBytes.Length - 1
    '		builder.AppendFormat("%{0}", Convert.ToString(encodedBytes(index), 16))
    '	Next

    '	Return builder.ToString()
    'End Function

    Private Function RecodeFileName(ByVal Filename As String) As String
        Dim FileNameRecode As String = ""
        Dim oEncodeChar As New EncodeChar

        For i As Integer = 0 To Filename.Length - 1
            If oEncodeChar.Contains(Filename.Chars(i)) Then
                FileNameRecode &= oEncodeChar.Recode(Filename.Chars(i))
            Else
                FileNameRecode &= Filename.Chars(i).ToString
            End If
        Next
        Return FileNameRecode
    End Function

#End Region

    Public Sub ClearHTTPcontext()
        _HttpContext.Response.ClearHeaders()
        _HttpContext.Response.ClearContent()
    End Sub
    Public ReadOnly Property RequireSSL() As Boolean
        Get
            Dim RichiediSSL As Boolean = False
            Try
                RequireSSL = SystemSettings.Login.isSSLrequired
            Catch ex As Exception
                RequireSSL = False
            End Try
        End Get
    End Property
    Public ReadOnly Property ApplicationUrlBase() As String
        Get
            Dim Redirect As String = "http"

            If RequireSSL Then
                Redirect &= "s://" & Me._HttpContext.Request.Url.Host & Me.BaseUrl
            Else
                Redirect &= "://" & Me._HttpContext.Request.Url.Host & Me.BaseUrl
            End If
            ApplicationUrlBase = Redirect
        End Get
    End Property



#Region "Percorso"
    Public ReadOnly Property SystemSettings() As ComolSettings
        Get
            SystemSettings = ManagerConfiguration.GetInstance
        End Get
    End Property

    Public ReadOnly Property FileDrivePath(ByVal forCommunity As Boolean, ByVal CommunityID As Integer) As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.GetSystemObjectPath(Me.SystemSettings.File.GetByCode(Me._ConfigurationFile))
            End If
            If forCommunity Then
                Path = _ObjectPath.Drive & CommunityID & "\"
                'Path = _ObjectPath.Drive & Me.CurrentCommunityID & "\"
            Else
                Path = _ObjectPath.Drive
            End If
            Path = Replace(Path, "\/", "\")
            Return Path
        End Get
    End Property
    Public ReadOnly Property FileDrivePath(ByVal forCommunity As Boolean, ByVal oConfigType As FileSettings.ConfigType) As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.GetSystemObjectPath(Me.SystemSettings.File.GetByCode(oConfigType))
            End If
            If forCommunity Then
                Path = _ObjectPath.Drive & Me.CurrentCommunityID & "\"
            Else
                Path = _ObjectPath.Drive
            End If
            Path = Replace(Path, "\/", "\")
            Return Path
        End Get
    End Property


    Private Function GetSystemObjectPath(ByVal oPath As ConfigurationPath) As ObjectFilePath
        Dim oObjectPath As New ObjectFilePath(oPath.isOnThisServer)

        Try
            If oPath.isOnThisServer Then
                oObjectPath.Virtual = Me.BaseUrl & "/" & oPath.VirtualPath
                oObjectPath.Virtual = Replace(oObjectPath.Virtual, "//", "/")
                If oPath.DrivePath <> "" Then
                    oObjectPath.Drive = oPath.DrivePath 'Me.BaseUrlDrivePath & oPath.DrivePath
                Else
                    oObjectPath.Drive = Me.BaseUrlDrivePath & oPath.VirtualPath
                End If
                oObjectPath.Drive = Replace(oObjectPath.Drive, "/", "\")
                oObjectPath.Drive = Replace(oObjectPath.Drive, "\\", "\")
            Else
                If oPath.ServerVirtualPath <> "" Then
                    oObjectPath.Virtual = oPath.ServerVirtualPath & oPath.VirtualPath
                End If
                If oPath.DrivePath <> "" Then
                    oObjectPath.Drive = oPath.ServerPath & oPath.DrivePath
                Else
                    oObjectPath.Drive = oPath.ServerPath & oPath.VirtualPath
                End If
                oObjectPath.SharePath = oPath.ServerPath
            End If
            oObjectPath.RewritePath = oPath.RewritePath
        Catch ex As Exception

        End Try
        Return oObjectPath
    End Function





    Protected ReadOnly Property TipoComunitaCorrenteID() As Integer
        Get
            Try
                TipoComunitaCorrenteID = Me._HttpContext.Session("TPCM_ID")
            Catch ex As Exception
                TipoComunitaCorrenteID = Main.TipoComunitaStandard.Portale
            End Try
        End Get
    End Property
    Protected ReadOnly Property UtenteCorrente() As COL_Persona
        Get
            Try
                UtenteCorrente = Me._HttpContext.Session("objPersona")
            Catch ex As Exception
                UtenteCorrente = Nothing
            End Try
        End Get
    End Property
    'Public Sub RegistraEvento(ByVal statoAttività As String, Optional ByVal idAttività As Integer = 0, Optional ByVal classeAttività As String = "", Optional ByVal attributo1 As String = "", Optional ByVal attributo2 As String = "", Optional ByVal attributo3 As String = "")
    '	Try
    '		Me._HttpContext.Application.Lock()
    '		If Me._HttpContext.Application("KnowledgeTutor").SaveActivated = True Then
    '			Dim stringaMessaggio As String = ""
    '			Me._HttpContext.Application("KnowledgeTutor").AddToHashTable(Me._HttpContext, Me.CurrentLanguage.Codice, Me.UtenteCorrente, statoAttività, Me.CurrentCommunityID, Me.TipoComunitaCorrenteID, Me.AreaCorrenteID, idAttività, classeAttività, attributo1, attributo2)
    '		End If
    '	Catch ex As Exception
    '	Finally
    '		Me._HttpContext.Application.UnLock()
    '	End Try
    'End Sub
#End Region

End Class

Public Class EncodeChar
    Private _Hash As New Hashtable

    Public Function Contains(ByVal Carattere As String) As Boolean
        Return Me._Hash.ContainsKey(Carattere)
    End Function

    Public Function Recode(ByVal Carattere As String) As String
        Return Me._Hash.Item(Carattere).ToString
    End Function

    Sub New()
        Me._Hash.Add("–", "-")
        Me._Hash.Add("—", "-")
        Me._Hash.Add("«", "-")
        Me._Hash.Add("»", "-")
        Me._Hash.Add("£", "L")
        Me._Hash.Add("©", "c")
        Me._Hash.Add("®", "r")
        Me._Hash.Add("°", "_")
        Me._Hash.Add("µ", "u")
        Me._Hash.Add("·", "_")
        Me._Hash.Add("†", "_")
        Me._Hash.Add("•", "_")
        Me._Hash.Add("€", "E")
        Me._Hash.Add("ª", "a")
        Me._Hash.Add("á", "a")
        Me._Hash.Add("Á", "a")
        Me._Hash.Add("à", "a")
        Me._Hash.Add("À", "a")
        Me._Hash.Add("â", "a")
        Me._Hash.Add("Â", "a")
        Me._Hash.Add("ä", "a")
        Me._Hash.Add("Ä", "a")
        Me._Hash.Add("ă", "a")
        Me._Hash.Add("Ă", "a")
        Me._Hash.Add("ā", "a")
        Me._Hash.Add("Ā", "a")
        Me._Hash.Add("ã", "a")
        Me._Hash.Add("Ã", "a")
        Me._Hash.Add("å", "a")
        Me._Hash.Add("Å", "a")
        Me._Hash.Add("ą", "a")
        Me._Hash.Add("Ą", "a")
        Me._Hash.Add("æ", "ae")
        Me._Hash.Add("Æ", "AE")
        Me._Hash.Add("ć", "c")
        Me._Hash.Add("Ć", "C")
        Me._Hash.Add("ĉ", "c")
        Me._Hash.Add("Ĉ", "C")
        Me._Hash.Add("č", "c")
        Me._Hash.Add("Č", "C")
        Me._Hash.Add("ç", "c")
        Me._Hash.Add("Ç", "C")
        Me._Hash.Add("ď", "d")
        Me._Hash.Add("Ď", "D")
        Me._Hash.Add("đ", "d")
        Me._Hash.Add("Đ", "D")
        Me._Hash.Add("ð", "a")
        Me._Hash.Add("Ð", "D")
        Me._Hash.Add("é", "e")
        Me._Hash.Add("É", "e")
        Me._Hash.Add("è", "e")
        Me._Hash.Add("È", "E")
        Me._Hash.Add("ê", "e")
        Me._Hash.Add("Ê", "E")
        Me._Hash.Add("ë", "e")
        Me._Hash.Add("Ë", "E")
        Me._Hash.Add("ě", "e")
        Me._Hash.Add("Ě", "E")
        Me._Hash.Add("ē", "e")
        Me._Hash.Add("Ē", "E")
        Me._Hash.Add("ę", "e")
        Me._Hash.Add("Ę", "E")
        Me._Hash.Add("ĝ", "g")
        Me._Hash.Add("Ĝ", "G")
        Me._Hash.Add("ğ", "g")
        Me._Hash.Add("Ğ", "G")
        Me._Hash.Add("ģ", "g")
        Me._Hash.Add("Ģ", "G")
        Me._Hash.Add("ĥ", "h")
        Me._Hash.Add("Ĥ", "H")
        Me._Hash.Add("ı", "i")
        Me._Hash.Add("í", "i")
        Me._Hash.Add("Í", "I")
        Me._Hash.Add("ì", "i")
        Me._Hash.Add("Ì", "I")
        Me._Hash.Add("İ", "I")
        Me._Hash.Add("î", "i")
        Me._Hash.Add("Î", "I")
        Me._Hash.Add("ï", "i")
        Me._Hash.Add("Ï", "I")
        Me._Hash.Add("ī", "i")
        Me._Hash.Add("Ī", "I")
        Me._Hash.Add("ĵ", "j")
        Me._Hash.Add("Ĵ", "J")
        Me._Hash.Add("ķ", "k")
        Me._Hash.Add("Ķ", "K")
        Me._Hash.Add("ĺ", "i")
        Me._Hash.Add("Ĺ", "L")
        Me._Hash.Add("ľ", "l")
        Me._Hash.Add("Ľ", "L")
        Me._Hash.Add("ļ", "l")
        Me._Hash.Add("Ļ", "L")
        Me._Hash.Add("ł", "l")
        Me._Hash.Add("Ł", "L")
        Me._Hash.Add("ń", "n")
        Me._Hash.Add("Ń", "N")
        Me._Hash.Add("ň", "n")
        Me._Hash.Add("Ň", "N")
        Me._Hash.Add("ņ", "n")
        Me._Hash.Add("Ņ", "N")
        Me._Hash.Add("№", "n")
        Me._Hash.Add("º", "o")
        Me._Hash.Add("ó", "o")
        Me._Hash.Add("Ó", "O")
        Me._Hash.Add("ò", "o")
        Me._Hash.Add("Ò", "O")
        Me._Hash.Add("ô", "o")
        Me._Hash.Add("Ô", "O")
        Me._Hash.Add("ö", "o")
        Me._Hash.Add("Ö", "O")
        Me._Hash.Add("õ", "o")
        Me._Hash.Add("Õ", "O")
        Me._Hash.Add("ő", "o")
        Me._Hash.Add("Ő", "O")
        Me._Hash.Add("ø", "o")
        Me._Hash.Add("Ø", "O")
        Me._Hash.Add("œ", "ce")
        Me._Hash.Add("Œ", "CE")
        Me._Hash.Add("ŕ", "r")
        Me._Hash.Add("Ŕ", "R")
        Me._Hash.Add("ř", "r")
        Me._Hash.Add("Ř", "R")
        Me._Hash.Add("ŗ", "r")
        Me._Hash.Add("Ŗ", "R")
        Me._Hash.Add("ś", "s")
        Me._Hash.Add("Ś", "S")
        Me._Hash.Add("ŝ", "s")
        Me._Hash.Add("Ŝ", "S")
        Me._Hash.Add("š", "s")
        Me._Hash.Add("Š", "S")
        Me._Hash.Add("ş", "s")
        Me._Hash.Add("Ş", "S")
        Me._Hash.Add("ß", "b")
        Me._Hash.Add("ť", "t")
        Me._Hash.Add("Ť", "T")
        Me._Hash.Add("ţ", "t")
        Me._Hash.Add("Ţ", "T")
        Me._Hash.Add("þ", "_")
        Me._Hash.Add("Þ", "_")
        Me._Hash.Add("ú", "u")
        Me._Hash.Add("Ú", "U")
        Me._Hash.Add("ù", "u")
        Me._Hash.Add("Ù", "U")
        Me._Hash.Add("û", "u")
        Me._Hash.Add("Û", "U")
        Me._Hash.Add("ü", "u")
        Me._Hash.Add("Ü", "U")
        Me._Hash.Add("ŭ", "u")
        Me._Hash.Add("Ŭ", "U")
        Me._Hash.Add("ū", "u")
        Me._Hash.Add("Ū", "U")
        Me._Hash.Add("ů", "u")
        Me._Hash.Add("Ů", "U")
        Me._Hash.Add("ű", "u")
        Me._Hash.Add("Ű", "U")
        Me._Hash.Add("ý", "y")
        Me._Hash.Add("Ý", "Y")
        Me._Hash.Add("ÿ", "y")
        Me._Hash.Add("Ÿ", "Y")
        Me._Hash.Add("ź", "z")
        Me._Hash.Add("Ź", "Z")
        Me._Hash.Add("ż", "z")
        Me._Hash.Add("Ż", "Z")
        Me._Hash.Add("ž", "z")
        Me._Hash.Add("Ž", "Z")
    End Sub

End Class