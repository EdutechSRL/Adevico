Public Interface IViewMainStat

    ''' <summary>
    ''' Bind dell'elenco servizi
    ''' </summary>
    ''' <param name="ServiceList"></param>
    ''' <remarks></remarks>
    Sub BindService(ByVal ServiceList As IList(Of Object))

    '''' <summary>
    '''' Bind dell'elenco comunità
    '''' </summary>
    '''' <param name="CommunityID"></param>
    '''' <remarks>
    '''' Per ora TOLTO! Tengo SOLO la comunità corrente.
    '''' Successivamente pagina per accesso, scegliendo la comunità.
    '''' </remarks>
    'Sub BindCommunityList(ByVal CommunityID As Integer)

    '''' <summary>
    '''' Vedere se tenerlo o lasciarlo fare alla pagina.
    '''' Setta solamente il valore nella label comunità.
    '''' </summary>
    '''' <param name="CommunityName"></param>
    '''' <remarks></remarks>
    'Sub BindCommunity(ByVal CommunityName As String)

    ReadOnly Property CommunityID() As Integer
    ReadOnly Property ServiceID() As Integer

    Property IsGlobal() As Boolean
    Property ShowGlobal() As Boolean

    Sub LoadControl(ByVal ControlPath As String)
    Sub BindControl()



End Interface
