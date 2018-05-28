Imports System.Web.UI.WebControls.Expressions
Imports Adevico.Core.Presentation
Imports Adevico.Core.Presentation.View
Imports PresentationLayer
Imports System.Text
Imports Newtonsoft.Json

Public Class VoidAPI
    Inherits PageBase
    Implements IViewAPIWrapper

    Public Const QueryStringUpdate As String = "Upd"
    Protected Shared isForIframe As Boolean = False


#Region "Context"
    Private _Presenter As Adevico.Core.Presentation.ApiWrapperPresenter

    Private ReadOnly Property CurrentPresenter() As Adevico.Core.Presentation.ApiWrapperPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Adevico.Core.Presentation.ApiWrapperPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim thisUrl As String
        thisUrl = Request.Url.ToString
        If Not Request.IsSecureConnection Then
            thisUrl = thisUrl.Replace("http://", "https://")
            If thisUrl.StartsWith("https://") Then
                Response.Redirect(thisUrl)
                'Server.Transfer(thisUrl)
                'Server.TransferRequest()
            End If
        End If

        isForIframe = False
        If Request("iframe") & "" = "1" Then
            isForIframe = True
        End If
    End Sub
    Public Overrides Sub BindDati()

        If Not Page.IsPostBack Then
            Me.TMsession.Interval = If((Me.SystemSettings.Presenter.AjaxTimer <= 30000), 30000, Me.SystemSettings.Presenter.AjaxTimer)
            Me.TMsession.Interval = 600000
            Me.TMsession.Enabled = False
        End If


        Dim update As Boolean = False
        Try
            update = CBool(Request.QueryString(QueryStringUpdate))
        Catch ex As Exception

        End Try

        CurrentPresenter.InitView(update)
    End Sub

#Region "Base"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        Try
            MyBase.SetCulture(String.Format("pg_{0}", Me.LTlocalizationService.Text), "Modules", "APIwrapper")
        Catch ex As Exception
        End Try
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

        Dim sourceValue As String = LTlocalizationValue.Text

        'Dim varList As New List(Of KeyValuePair(Of String, String))
        Dim varDict As New Dictionary(Of String, String)

        Dim outValue As String = ""

        Dim isFirst = False

        If Not String.IsNullOrEmpty(sourceValue) Then
            Dim keys As String() = sourceValue.Replace(" ", "").Replace(vbCrLf, "").Split(",")

            For Each key As String In keys

                Dim value As String = Resource.getValue(key)
                If (String.IsNullOrEmpty(value)) Then
                    value = key
                End If

                If Not varDict.ContainsKey(key) Then
                    varDict.Add(key, value)
                End If

                'outValue = String.Format(outFormat, key, Resource.getValue(key))
            Next

        End If

        'Dim json As String = JsonConvert.SerializeObject(varList, System.Xml.Formatting.Indented)
        
        LTlocalizationValue.Text = JsonConvert.SerializeObject(varDict)

    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Public Function GetWorkingSessionId() As Guid Implements IViewAPIWrapper.GetWorkingSessionId
        Return Me.PageUtility.UniqueGuidSession
    End Function

    ''' <summary>
    ''' Imposta i cookie da presenter
    ''' </summary>
    ''' <param name="token">Il Token: da presenter</param>
    ''' <param name="workingSessionId">WorkingSessionIs</param>
    ''' <param name="comId">Comunità</param>
    ''' <remarks></remarks>
    Public Sub SetCookie(token As String, workingSessionId As String, comId As String, usrId As String) Implements IViewAPIWrapper.SetCookie

        If Not String.IsNullOrEmpty(Request.QueryString("ComId")) AndAlso IsNumeric(Request.QueryString("ComId")) Then

            Dim RequestComId As Integer = 0
            Try
                RequestComId = System.Convert.ToInt32(Request.QueryString("ComId"))
            Catch ex As Exception

            End Try

            If RequestComId > 0 Then
                comId = RequestComId.ToString()
            End If


        End If

        'Testiamo QUI se tutto funziona.
        Me.PageUtility.ApiTokenWriteCookie(
            token,
            workingSessionId,
            comId,
            usrId,
            Request.Url.ToString,
            MyBase.UserSessionLanguage.ID,
            MyBase.UserSessionLanguage.Codice
            )

        'Chiamate precedenti
        'SetResponseCookie(CookieHelper.CookieKeyToked, token)
        'SetResponseCookie(CookieHelper.CookieKeyDeviceId, workingSessionId)
        'SetResponseCookie(CookieHelper.CookieKeyCommunityId, comId)
        'SetResponseCookie(CookieHelper.CookieKeyPersonId, usrId)
        'SetResponseCookie(CookieHelper.CookieKeyLangId, MyBase.UserSessionLanguage.ID)
        'SetResponseCookie(CookieHelper.CookieKeyLangCode, MyBase.UserSessionLanguage.Codice)

    End Sub

    Private Sub SetResponseCookie(ByVal key As String, ByVal value As String)
        Dim myCookie As HttpCookie = New HttpCookie(key, value)
        myCookie.Expires = DateTime.Now.AddDays(1)
        Dim myurl As String = Request.Url.ToString
        If myurl.IndexOf("://localhost") > 0 Then
            myCookie.Domain = ""
            myCookie.Path = "/"
        End If

        Response.Cookies.Add(myCookie)
    End Sub


    ''' <summary>
    ''' Mostra i cookie - X TEST
    ''' </summary>
    ''' <returns>Stringa di test con i valori dei cookie</returns>
    ''' <remarks></remarks>
    Public Function ShowCookie() As String

        Return String.Format("Token={0};DeviceId={1};CommunityId={2};LangId={3};LangCode={4};PersonId={5}", _
                             GetCookieResponse(CookieHelper.CookieKeyToked), _
                             GetCookieResponse(CookieHelper.CookieKeyDeviceId), _
                             GetCookieResponse(CookieHelper.CookieKeyCommunityId), _
                             GetCookieResponse(CookieHelper.CookieKeyLangId), _
                             GetCookieResponse(CookieHelper.CookieKeyLangCode), _
                             GetCookieResponse(CookieHelper.CookieKeyPersonId))
    End Function

    ''' <summary>
    ''' Recupera il valore .ToString() di un cookie data la chiave.String
    ''' </summary>
    ''' <param name="key">Chiave del cookie</param>
    ''' <returns>Valore del cookie</returns>
    ''' <remarks></remarks>
    Public Function GetCookieRequest(ByVal key As String) As String Implements IViewAPIWrapper.GetCookieRequest
        Return Request.Cookies(key).Value.ToString()
    End Function

    Public Function GetCookieResponse(ByVal key As String) As String Implements IViewAPIWrapper.GetCookieResponse

        Return Response.Cookies(key).Value.ToString()

    End Function

    Public Sub UpdateCookie()
        CurrentPresenter.InitView(True)
    End Sub

    ''' <summary>
    ''' Recupera un valore dall'XML di internazionalizzazione
    ''' </summary>
    ''' <param name="value">Valore da recuperare (es: Title.text). NOTA: CASE SENSITIVE!</param>
    ''' <returns>Il valore internazionalizzato</returns>
    ''' <remarks></remarks>
    Public Function GetLocalization(ByVal value As String) As String
        Try
            Return Me.Resource.getValue(value)
        Catch ex As Exception

        End Try
        Return ""

    End Function


    Public Sub SessionTimeout(communityId As Integer) Implements IViewAPIWrapper.SessionTimeout
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow
        dto.DestinationUrl = Request.Url.PathAndQuery

        dto.IdCommunity = communityId
        webPost.Redirect(dto)
    End Sub

    '''' <summary>
    '''' Configurazione sistema
    '''' </summary>
    '''' <returns></returns>
    'Public ReadOnly Property SysConfig As DelaySubscriptionConfig
    '    Get
    '        Return SystemSettings.Features.DelaySubconf
    '    End Get
    'End Property

    Public Function GetConfig(ByVal ConfigurationType As String) As String
        'Dim serializer As New JavaScriptSerializer
        Dim values As String = ""


        Select Case ConfigurationType
            Case "DelaySubconf"
                values = JsonConvert.SerializeObject(SystemSettings.Features.DelaySubconf)
            Case "DelaySubconfLocal"
                values = JsonConvert.SerializeObject(SystemSettings.Features.DelaySubconf.GetValidity(MyBase.LinguaCode))
        End Select

        Return values
    End Function


End Class