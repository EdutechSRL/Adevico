Imports System.Runtime.Serialization

Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True), DataContract()> Public Class BrowserInfo
        'Implements iBrowserinfo

#Region "Private"
        Private _ActiveXControls As Boolean
        Private _ClientIPadress As String
        Private _ProxyIPadress As String
        Private _CanInitiateVoiceCall As Boolean
        Private _Cookies As Boolean
        Private _Frames As Boolean
        Private _IsMobileDevice As Boolean
        Private _JavaApplets As Boolean
        Private _JScriptVersion As String
        Private _Language As String
        Private _PersonID As Integer
        Private _PersonTypeID As Integer
        Private _Platform As String
        Private _ScreenCharactersWidth As Integer
        Private _ScreenPixelsHeight As Integer
        Private _Version As String
        Private _Tables As Boolean
        Private _W3CDomVersion As String
        Private _WorkingSessionID As System.Guid
        Private _BrowserType As String
#End Region

#Region "Public"
        <DataMember()> Property ActiveXControls() As Boolean
            'Implements iBrowserinfo.ActiveXControls
            Get
                Return _ActiveXControls
            End Get
            Set(ByVal value As Boolean)
                _ActiveXControls = value
            End Set
        End Property
        <DataMember()> Public Property ClientIPAdress() As String
            'Implements iBrowserinfo.ClientIDAdress
            Get
                Return _ClientIPadress
            End Get
            Set(ByVal value As String)
                _ClientIPadress = value
            End Set
        End Property
        <DataMember()> Public Property ProxyIPAdress() As String
            'Implements iBrowserinfo.ProxyIDAdress
            Get
                Return _ProxyIPadress
            End Get
            Set(ByVal value As String)
                _ProxyIPadress = value
            End Set
        End Property
        <DataMember()> Property CanInitiateVoiceCall() As Boolean
            'Implements iBrowserinfo.CanInitiateVoiceCall
            Get
                Return _CanInitiateVoiceCall
            End Get
            Set(ByVal value As Boolean)
                _CanInitiateVoiceCall = value
            End Set
        End Property
        <DataMember()> Property Cookies() As Boolean
            'Implements iBrowserinfo.Cookies
            Get
                Return _Cookies
            End Get
            Set(ByVal value As Boolean)
                _Cookies = value
            End Set
        End Property
        <DataMember()> Public Property Frames() As Boolean
            'Implements iBrowserinfo.Frames
            Get
                Return _Frames
            End Get
            Set(ByVal value As Boolean)
                _Frames = value
            End Set
        End Property
        <DataMember()> Public Property IsMobileDevice() As Boolean
            'Implements iBrowserinfo.IsMobileDevice
            Get
                Return _IsMobileDevice
            End Get
            Set(ByVal value As Boolean)
                _IsMobileDevice = value
            End Set
        End Property
        <DataMember()> Public Property JavaApplets() As Boolean
            'Implements iBrowserinfo.JavaApplets
            Get
                Return _JavaApplets
            End Get
            Set(ByVal value As Boolean)
                _JavaApplets = value
            End Set
        End Property
        <DataMember()> Public Property JScriptVersion() As String
            'Implements iBrowserinfo.JScriptVersion
            Get
                Return _JScriptVersion
            End Get
            Set(ByVal value As String)
                _JScriptVersion = value
            End Set
        End Property
        <DataMember()> Public Property Language() As String
            'Implements iBrowserinfo.Language
            Get
                Return _Language
            End Get
            Set(ByVal value As String)
                _Language = value
            End Set
        End Property
        <DataMember()> Public Property PersonID() As Integer
            'Implements iBrowserinfo.PersonID
            Get
                Return _PersonID
            End Get
            Set(ByVal value As Integer)
                _PersonID = value
            End Set
        End Property
        <DataMember()> Public Property PersonTypeID() As Integer
            'Implements iBrowserinfo.PersonTypeID
            Get
                Return _PersonTypeID
            End Get
            Set(ByVal value As Integer)
                _PersonTypeID = value
            End Set
        End Property
        <DataMember()> Public Property Platform() As String
            'Implements iBrowserinfo.Platform
            Get
                Return _Platform
            End Get
            Set(ByVal value As String)
                _Platform = value
            End Set
        End Property
        <DataMember()> Public Property ScreenCharactersWidth() As Integer
            'Implements iBrowserinfo.ScreenCharactersWidth
            Get
                Return _ScreenCharactersWidth
            End Get
            Set(ByVal value As Integer)
                _ScreenCharactersWidth = value
            End Set
        End Property
        <DataMember()> Public Property ScreenPixelsHeight() As Integer
            'Implements iBrowserinfo.ScreenPixelsHeight
            Get
                Return _ScreenPixelsHeight
            End Get
            Set(ByVal value As Integer)
                _ScreenPixelsHeight = value
            End Set
        End Property
        <DataMember()> Public Property Tables() As Boolean
            'Implements iBrowserinfo.Tables
            Get
                Return _Tables
            End Get
            Set(ByVal value As Boolean)
                _Tables = value
            End Set
        End Property
        <DataMember()> Public Property Version() As String
            'Implements iBrowserinfo.Version
            Get
                Return _Version
            End Get
            Set(ByVal value As String)
                _Version = value
            End Set
        End Property
        <DataMember()> Public Property W3CDomVersion() As String
            'Implements iBrowserinfo.W3CDomVersion
            Get
                Return _W3CDomVersion
            End Get
            Set(ByVal value As String)
                _W3CDomVersion = value
            End Set
        End Property
        <DataMember()> Public Property WorkingSessionID() As System.Guid
            'Implements iBrowserinfo.WorkingSessionID
            Get
                Return _WorkingSessionID
            End Get
            Set(ByVal value As System.Guid)
                _WorkingSessionID = value
            End Set
        End Property
        <DataMember()> Public Property BrowserType() As String
            'Implements iBrowserinfo.WorkingSessionID
            Get
                Return _BrowserType
            End Get
            Set(ByVal value As String)
                _BrowserType = value
            End Set
        End Property
#End Region

        Sub New()

        End Sub
        Public Shared Function Create(ByVal oCapabilities As System.Web.HttpBrowserCapabilities) As BrowserInfo
            Dim oBrowserInfo As New BrowserInfo
            With oBrowserInfo
                .ActiveXControls = oCapabilities.ActiveXControls
                .CanInitiateVoiceCall = oCapabilities.CanInitiateVoiceCall
                .Cookies = oCapabilities.Cookies
                .Frames = oCapabilities.Frames
                .IsMobileDevice = oCapabilities.IsMobileDevice
                .JavaApplets = oCapabilities.JavaApplets
                .JScriptVersion = oCapabilities.JScriptVersion.Major
                .Platform = oCapabilities.Platform
                .ScreenCharactersWidth = oCapabilities.ScreenCharactersWidth
                .ScreenPixelsHeight = oCapabilities.ScreenCharactersHeight
                .Tables = oCapabilities.Tables
                .Version = oCapabilities.Version
				.W3CDomVersion = oCapabilities.W3CDomVersion.Minor
				.BrowserType = oCapabilities.Browser
			End With
            Return oBrowserInfo
        End Function
		Public Shared Function Create(ByVal oContext As System.Web.HttpContext) As BrowserInfo
			Dim oBrowserInfo As New BrowserInfo
			Dim oCapabilities As System.Web.HttpBrowserCapabilities = oContext.Request.Browser
			With oBrowserInfo
				.ActiveXControls = oCapabilities.ActiveXControls
				.CanInitiateVoiceCall = oCapabilities.CanInitiateVoiceCall
				.Cookies = oCapabilities.Cookies
				.Frames = oCapabilities.Frames
				.IsMobileDevice = oCapabilities.IsMobileDevice
				.JavaApplets = oCapabilities.JavaApplets
				.JScriptVersion = oCapabilities.JScriptVersion.Major
				.Platform = oCapabilities.Platform
				.ScreenCharactersWidth = oCapabilities.ScreenCharactersWidth
				.ScreenPixelsHeight = oCapabilities.ScreenCharactersHeight
				.Tables = oCapabilities.Tables
				.Version = oCapabilities.Version
				.W3CDomVersion = oCapabilities.W3CDomVersion.Minor
				.BrowserType = oCapabilities.Browser
				If oContext.Request.UserLanguages.Length = 0 Then
					.Language = ""
				Else
					.Language = oContext.Request.UserLanguages(0)
				End If
			End With
			Return oBrowserInfo
		End Function

    End Class
End Namespace