Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Imports System.Web.Services.Description
Imports System.Xml.Serialization
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.UCServices
Imports System.Configuration

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/WS_Chat/WS_Chat")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WS_Chat
	Inherits System.Web.Services.WebService

	Dim MaxNumFile As Integer = 4 '5 file massimo... :p
	Dim MaxMsg As Integer = 10 '100
	Dim MinMsg As Integer = 2 '20
	Dim MaxDisc As Integer = 300 '5 minuti...'Secondi in cui l'utente puo' fare a meno di accedere senza essere disconnesso

#Region " Codice generato da Progettazione servizi Web + NEW "
	Public Sub New()
		MyBase.New()

		'Chiamata richiesta da Progettazione servizi Web
		InitializeComponent()

		'Aggiungere il codice di inizializzazione dopo la chiamata a InitializeComponent()

		'Scadenza = 5 'Da mettere in Molinari's Table
		'servizio.PermessiAssociati = Permessi() 'Per la sicurezza...
	End Sub

	'Richiesto da Progettazione servizi Web
	Private components As System.ComponentModel.IContainer
	'NOTA: la procedura che segue è richiesta da Progettazione servizi Web.
	'Può essere modificata in Progettazione servizi Web.  
	'Non modificarla nell'editor del codice.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		'CODEGEN: questa procedura è richiesta da Progettazione servizi Web.
		'Non modificarla nell'editor del codice.
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

#End Region

#Region "Metodi di controllo Web Service"
	<WebMethod(Description:="Controlla se il WebService risponde.")> _
	Public Function IsAlive() As Boolean
		Return True
	End Function

	<WebMethod(Description:="Reset Application.")> _
		Public Sub Reset(ByVal SecureCode As String)
		If SecureCode = System.Configuration.ConfigurationManager.AppSettings("SecureCode") Then
			Application.Clear()
		End If
	End Sub

#End Region
#Region "Test e catch errori"
	'<WebMethod(Description:="Test salvataggio errori")> _
	'    Public Sub CatchTest(ByVal SecureCode As String)
	'    Try
	'        Dim a, b, c As Integer
	'        a = 1
	'        b = 0
	'        c = a / b
	'    Catch ex As Exception
	'        Me.LogError(ex.ToString)
	'    End Try
	'End Sub

	<WebMethod(Description:="Test salvataggio errori")> _
		Public Function GetErrorList(ByVal SecureCode As String) As String

		Dim ErrorString As String = ""
		If SecureCode = System.Configuration.ConfigurationManager.AppSettings("SecureCode") Then
			Dim FilePath As String
			FilePath = System.Configuration.ConfigurationManager.AppSettings("LogPath")
			FilePath &= System.Configuration.ConfigurationManager.AppSettings("ErrorLogFile")

			Try
				ErrorString = File.ReadAllText(FilePath)
			Catch ex As Exception
				ErrorString = ex.ToString
			Finally
			End Try
		End If
		Return ErrorString
	End Function
#End Region


#Region " Metodi Recupera Messaggi V 12/03/2004 "

	'Recupera ULTIMI messaggi di Una specifica persona - V 12/03/2004
	<WebMethod(Description:="Recupero messaggio")> _
	Public Function RecuperaMessaggio(ByVal IdUtente As Integer, ByVal IDComunita As Integer) As DataSet 'ByVal Tempo As DateTime,
		Dim DsMsgOut As New DataSet
		Try

			'Application.Lock()
			'DsMsgOut.Tables.Clear()
			DsMsgOut.Tables.Add(Application(IDComunita.ToString).Estrai(CInt(IdUtente), Application("oBlock").GetBlockFrom(IdUtente)).Copy)	'CDate(Tempo), 
			'Application.UnLock()
		Catch ex As Exception
			Me.LogError(ex.ToString)
		End Try
		Return DsMsgOut.Copy
	End Function

	'Recupera i messaggi di una persona in XML - V 12/03/2004
	<WebMethod(Description:="Recupero messaggio XML")> _
	Public Function RecuperaMessaggioXML(ByVal IDUtente As Integer, ByVal IDComunita As Integer) As String 'ByVal Tempo As DateTime,

		Dim dsMsgOut As New DataSet

		'Application.Lock()
		dsMsgOut.Tables.Add(Application(IDComunita.ToString).Estrai(CInt(IDUtente), Application("oBlock").GetBlockFrom(IDUtente)).Copy)	'CDate(Tempo),
		'Application.UnLock()

		Return dsMsgOut.GetXml
	End Function

	'DA VEDERE SE NECCESARI:
	'Mancano i metodi per il recupero dei messaggi archiviati (manca anche l'archiviazione)...

#End Region
#Region " Metodi di invio Messaggi V 12/03/2004 "

	<WebMethod(Description:="Invio messaggio")> _
	Public Sub InviaMessaggio(ByVal Nome As String, ByVal Messaggio As String, ByVal IDPersona As Integer, ByVal IDComunita As Integer, ByVal IsBold As Boolean, ByVal IsItalic As Boolean, ByVal IsUnderline As Boolean, ByVal TxtBgColor As String, ByVal TxtColor As String)	', ByVal Tempo As DateTime
		'controllo per i null...
		If Nome = "" Then Nome = "Utente sconosciuto" 'Da eliminare - (fisicamente...) ;) -> Lo tengo come controllo sulla sicurezza...
		If Messaggio = "" Then Messaggio = "No message"
		If IDPersona >= 0 And Me.GetLvl(IDPersona, IDComunita) > 1 Then	'Solo se ID valido e con diritto di scrittura...
			Try
				Application.Lock()
				Application(IDComunita.ToString).AggiungiMsg(Nome, Messaggio, IDPersona, IsBold, IsItalic, IsUnderline, TxtBgColor, TxtColor) ', CDate(Tempo)
			Catch ex As Exception
				Me.LogError(ex.ToString)
			Finally
				Application.UnLock()
			End Try
		End If
	End Sub

	<WebMethod(Description:="Invio messaggio privato")> _
		Public Sub InviaMessaggioPrivato(ByVal Nome As String, ByVal Messaggio As String, ByVal IDPersona As String, ByVal IDDest As String, ByVal IDComunita As Integer, ByVal IsBold As Boolean, ByVal IsItalic As Boolean, ByVal IsUnderline As Boolean, ByVal TxtBgColor As String, ByVal TxtColor As String) ', ByVal Tempo As DateTime
		'controllo per i null...
		If Nome = "" Then Nome = "Utente sconosciuto" 'Da eliminare - (fisicamente...) ;) 'Lo tengo per controllare la sicurezza... Al max fare controllo sul livello...
		If Messaggio = "" Then Messaggio = "No message"
		If IDPersona >= 0 And Me.GetLvl(IDPersona, IDComunita) > 1 Then	'Solo se ID valido e con diritto di scrittura...
			Try
				Application.Lock()
				Application(IDComunita.ToString).AggiungiMsg(Nome, Messaggio, IDPersona, IDDest, IsBold, IsItalic, IsUnderline, TxtBgColor, TxtColor) 'Aggiunto IDDest ', CDate(Tempo)
			Catch ex As Exception
				Me.LogError(ex.ToString)
			Finally
				Application.UnLock()
			End Try

		End If
	End Sub

#End Region

#Region " Metodi gestione utenti V 12/03/2004 "

	''ATTENZIONE: Me.Login richiama questa sub!!!
	<WebMethod(Description:="Utenti: Inserimento")> _
	Public Sub NuovoUtente(ByVal ID As Integer, ByVal Nome As String, ByVal IDComunita As Integer) ', ByVal Lvl As Byte) ', ByVal pwd as String)
		Try
			'Considerata una sorta di metodo NEW!!!

			Dim oChat As New Services_CHAT
			oChat.PermessiAssociati = Me.Permessi(ID, IDComunita)
			Dim Lvl As Integer = 0
			If oChat.Admin Or oChat.GestionePermessi Then 'servizio.Grant Or servizio.Admin Or servizio.Moderate Then
				Lvl = 4	'Admin
			ElseIf oChat.Write Then
				Lvl = 2	'Writer
			ElseIf oChat.Read Then
				Lvl = 1	'Reader
			Else
				Lvl = 0	'Out! (utente senza permesso)
			End If

			If Not Lvl = 0 Then	'Se l'utente non è autorizzato non viene nemmeno inserito...
				'Il codice sotto va bene qui: necessario averlo qui e quando controllo se l'utente c'è... -> vedi Col

				If (Application(IDComunita.ToString)) Is Nothing Then
					Me.CreaComunita(IDComunita)
				End If

				'If Application("FileMngmt" & IDComunita.ToString) Is Nothing Then
				'    Application.Add("FileMngmt" & IDComunita.ToString, New oChtWbsFileMng(Server.MapPath(".")))
				'End If
				If Application("FileMngmt" & IDComunita.ToString) Is Nothing Then
					Application.Add("FileMngmt" & IDComunita.ToString, New oChtWbsFileMng(Server.MapPath("."), IDComunita))
				End If
				Try
					Application.Lock()
					Application(IDComunita.ToString).AggiungiPersona(ID, Nome, Lvl)
				Catch ex As Exception
					Me.LogError(ex.ToString)
				Finally
					Application.UnLock()
				End Try

			End If

			If Application("oBlock") Is Nothing Then
				Application.Add("oBlock", New OChtUtentiBlock)
			End If

		Catch ex As Exception
			Me.LogError(ex.ToString)
		End Try

	End Sub

	<WebMethod(Description:="Utenti: Inserimento")> _
	Public Function NumeroUtenti(ByVal IDComunita As Integer) As Integer
		Try
			If Application(IDComunita.ToString) Is Nothing Then
				Return 0
			Else
				Return Application(IDComunita.ToString).NumUtenti()
			End If
		Catch ex As Exception
			Me.LogError(ex.ToString)
		End Try
	End Function

	<WebMethod(Description:="Utenti: Recupera lista")> _
	Public Function RecuperaListaUtenti(ByVal IdUtente As Integer, ByVal IDComunita As Integer) As DataSet
		Dim oDataSet As New DataSet
		Try
			'oDataSet.Tables.Clear() 'Probabilmente inutile...
			oDataSet.Tables.Add(Application(IDComunita.ToString).EstraiUtenti(Application("oBlock").GetBlockTo(IdUtente)).Copy)
		Catch ex As Exception
			oDataSet = New DataSet
		End Try
		'Application.Lock()

		'Application.UnLock()

		Return oDataSet.Copy
	End Function

	<WebMethod(Description:="Utenti: recupera livello")> _
	Public Function GetLvl(ByVal Id As Integer, ByVal IDComunita As Integer) As Byte
		Dim ByteLvl As Byte = 0
		If Application(IDComunita.ToString) Is Nothing Then
			Me.CreaComunita(IDComunita)
		End If

		'Application.Lock()
		ByteLvl = Application(IDComunita.ToString).GetLvlPers(Id)
		'Application.UnLock()
		Return ByteLvl
	End Function

	<WebMethod(Description:="Utenti: Rimuove utente")> _
	Public Function RemUtente(ByVal Id As Integer, ByVal IDComunita As Integer) As Byte
		'Da testare... :p
		Try
			Application(IDComunita.ToString).EliminaPersona(Id)
			If Application(IDComunita.ToString).NumUtenti() = 0 Then
				Me.EliminaComunita(IDComunita)
			End If
		Catch ex As Exception
		End Try

	End Function

	'<WebMethod(Description:="Test: Rimuove utente")> _
	'  Public Function ElencoComunita() As DataSet
	'	'Da testare... :p
	'	Try
	'		Dim oComunita As New COL_Comunita
	'		Dim oPeriodo As New COL_Periodo
	'		Dim oDataset As DataSet = oComunita.ElencaComunita(1, 1, Main.TipoComunitaStandard.CorsoDiLaurea)
	'		Dim oDataset1 As DataSet = oPeriodo.Elenca(1)
	'		Dim oDataTable As New DataTable
	'		oDataTable = oDataset1.Tables(0).Clone
	'		oDataTable.TableName = "SLOTS"
	'		oDataset.Tables(0).TableName = "Aule"
	'		oDataset.Tables.Add(oDataTable)
	'		Return oDataset

	'	Catch ex As Exception
	'	End Try

	'End Function

	'<WebMethod(Description:="Test: Rimuove utente")> _
	'  Public Function ElencoComunitaTesto() As String
	'	'Da testare... :p
	'	Try
	'		Dim oComunita As New COL_Comunita

	'		Dim oDataset As DataSet = oComunita.ElencaComunita(1, 1, Main.TipoComunitaStandard.CorsoDiLaurea)
	'		Dim oDataset1 As DataSet = oComunita.ElencaComunita(1, 1, Main.TipoComunitaStandard.CorsoDiLaurea)
	'		Dim oDataTable As New DataTable
	'		oDataTable = oDataset1.Tables(0).Clone
	'		oDataTable.TableName = "SLOTS"
	'		oDataset.Tables(0).TableName = "Aule"
	'		oDataset.Tables.Add(oDataTable)
	'		Return oDataset.GetXml()

	'	Catch ex As Exception
	'	End Try

	'End Function

	'<WebMethod(Description:="Test: Rimuove utente")> _
	' Public Function ElencoComunitaSchema() As String
	'	'Da testare... :p
	'	Try
	'		Dim oComunita As New COL_Comunita
	'		Dim oDataset As DataSet = oComunita.ElencaComunita(1, 1, Main.TipoComunitaStandard.CorsoDiLaurea)
	'		Dim oDataset1 As DataSet = oComunita.ElencaComunita(1, 1, Main.TipoComunitaStandard.CorsoDiLaurea)

	'		Dim oDataTable As New DataTable
	'		oDataTable = oDataset1.Tables(0).Clone
	'		oDataTable.TableName = "SLOTS"
	'		oDataset.Tables(0).TableName = "Aule"
	'		oDataset.Tables.Add(oDataTable)
	'		Return oDataset.GetXmlSchema

	'	Catch ex As Exception
	'	End Try

	'End Function
#End Region
#Region " Metodi Blocco utenti V 12/03/2004"
	'Rivedere la sicurezza di questi metodi...
	'ATTENZIONE: Questo metodi funzionano per tutte le comunità...
	'Ciò significa che non posso controllare il livello di un utente da qui, perchè
	'cambia con la comunità...
	'Devo controllare dall'applicazione che usa il Wbs_Chat...

	<WebMethod(Description:=" Blocco: imposta un blocco ")> _
	 Public Sub SetBlock(ByVal IdFrom As Integer, ByVal IdTo As Integer)
		If Not IdFrom = IdTo Then
			Try
				Application.Lock()
				Application("oBlock").Blocca(IdFrom, IdTo)
			Catch ex As Exception
				Me.LogError(ex.ToString)
			Finally
				Application.UnLock()
			End Try

		End If
	End Sub

	<WebMethod(Description:=" Blocco: rimuove un blocco")> _
	 Public Sub RemBlock(ByVal IdFrom As Integer, ByVal IdTo As Integer)
		Try
			Application.Lock()
			Application("oBlock").SBlocca(IdFrom, IdTo)
		Catch ex As Exception
			Me.LogError(ex.ToString)
		Finally
			Application.UnLock()
		End Try

	End Sub

	<WebMethod(Description:=" Blocco: lista in base a chi ha effettuato il blocco ")> _
  Public Function GetBlockFrom(ByVal IdFrom As Integer) As DataSet
		'Application.Lock()
		Dim DsBlock As New DataSet
		DsBlock.Clear()
		DsBlock.Tables.Clear()
		'Application.Lock()
		Try
			DsBlock.Tables.Add(Application("oBlock").GetBlockFrom(IdFrom).Copy)
		Catch ex As Exception
			Me.LogError(ex.ToString)
		End Try
		'Application.UnLock()
		Return DsBlock.Copy
	End Function

	<WebMethod(Description:=" Blocco: lista in base a chi subisce il blocco")> _
  Public Function GetBlockTo(ByVal IdTo As Integer) As DataSet
		Dim dsTemp As New DataSet
		dsTemp.Tables.Add(Application("oBlock").GetBlockTo(IdTo).Copy)
		Return dsTemp.Copy
	End Function

#End Region

#Region " Metodi di amministrazione - Protetti - V 12/03/2004 "	'Andranno protetti con pwd... (Vedere implementazione di Bussiness logic)
	'Per la SICUREZZA: verifica solamente il livello all'interno della tabella temporanea di oMsgTemp...
	'Attualmente il controllo è sull'ID, ma è facile recuperare l'ID di un amministratore...

	<WebMethod(Description:="AMMINISTRAZIONE: Recupera tutti i messaggi")> _
	Public Function RecuperaTutti(ByVal Id As Integer, ByVal IDComunita As Integer) As DataSet
		Dim DsMsgOut As New DataSet
		'GetLvlPers(ByVal Id As Integer)
		If Application(IDComunita.ToString).GetLvlPers(Id) > 1 Then	'Lo faccio solo se sono admin...
			Try
				Application.Lock()
				'Application(IDComunita.ToString).AggiornaBlock(Application("oBlock"))
				DsMsgOut.Tables.Add(Application(IDComunita.ToString).Estrai()) 'Application("WBSChat").Estrai().Copy
			Catch ex As Exception
				Me.LogError(ex.ToString)
			Finally
				Application.UnLock()
			End Try

		End If
		'End If
		Return DsMsgOut.Copy
	End Function

	<WebMethod(Description:="AMMINISTRAZIONE: Modifica il livello di accesso di un utente")> _
	Public Sub SetLvl(ByVal IDAmm As Integer, ByVal IDSet As Integer, ByVal NewLvl As Byte, ByVal IDComunita As Integer)
		If Application(IDComunita.ToString).GetLvlPers(IDAmm) > 1 Then
			Try
				Application.Lock()
				Application(IDComunita.ToString).SetLvlPers(IDSet, NewLvl)
			Catch ex As Exception
				Me.LogError(ex.ToString)
			Finally
				Application.UnLock()
			End Try

		End If
	End Sub

	<WebMethod(Description:="AMMINISTRAZIONE: Elimina tutti i messaggi di quella comunita")> _
	Public Sub ResetMsg(ByVal Id As Integer, ByVal IDComunita As Integer)
		If Application(IDComunita.ToString).GetLvlPers(Id) > 1 Then
			Try
				Application.Lock()
				Application(IDComunita.ToString).ResetMsg()
			Catch ex As Exception
				Me.LogError(ex.ToString)
			Finally
				Application.UnLock()
			End Try

		End If
	End Sub

#End Region
#Region "AMMINISTRAZIONE DI ALTO LIVELLO - SISTEMA"

	<WebMethod(Description:="AMMINISTRAZIONE ALTO LIVELLO: Elimina tutte le variabili correttamente: salvataggio dati e cancellazione")> _
		Public Sub DistruggiTutto(ByVal Credenziale As String)

		Dim condizione As Boolean = False
		If condizione Then
			For i As Integer = 0 To Application("ListaComunita").Count - 1
				Application(Application("ListaComunita").Item(i).ToString).svuota()
			Next
		End If
	End Sub

	<WebMethod(Description:="AMMINISTRAZIONE ALTO LIVELLO: Manda un messaggio a tutti gli utenti per dire che la chat verra' chiusa-riavviata...")> _
		Public Sub SendAllADM(ByVal Credenziale As String, ByVal messaggio As String)
		Dim condizione As Boolean = False
		If condizione Then
			For i As Integer = 0 To Application("ListaComunita").Count - 1
				Application(Application("ListaComunita").Item(i).ToString).AggiungiMsg("System", messaggio, -1, False, False, False, "ffffff", "000000")
			Next
		End If
	End Sub
	'<WebMethod(Description:="Cancello TUTTE le application")> _
	'     Public Sub RemApplication()
	'    Application.RemoveAll()
	'End Sub
#End Region

#Region "Metodi Upload/Download"

	'<WebMethod(Description:="Se True sono in corso operazioni di lettura/scrittura")> _
	'   Public Function IsUploadTime(ByVal IDComunita As Integer) As Boolean 'Da aggiornare... x compatibilita' all'indietro...
	'    Dim BoolTemp As Boolean = Application("FileMngmt" & IDComunita.ToString).WRState()
	'    'Dim BoolTemp As Boolean = Application("FileMngmt").WRState()
	'    Return BoolTemp 'True 
	'End Function

	<WebMethod(Description:="Se True sono in corso operazioni di lettura/scrittura")> _
		  Public Function IsUploadTime() As Boolean	'Da aggiornare... x compatibilita' all'indietro...
		'Dim BoolTemp As Boolean = Application("FileMngmt" & IDComunita.ToString).WRState()
		'Dim BoolTemp As Boolean = Application("FileMngmt").WRState()
		Return True	'BoolTemp
	End Function

	<WebMethod(Description:="Se True sono in corso operazioni di lettura/scrittura")> _
		  Public Function IsUploadTimeCom(ByVal IdCom As Integer) As Boolean 'Da aggiornare... x compatibilita' all'indietro...
		Dim BoolTemp As Boolean = Application("FileMngmt" & IdCom.ToString).WRState()
		Return BoolTemp	'True 'BoolTemp
	End Function

	<WebMethod(Description:="Effettua l'upload del file")> _
		   Public Function UploadFile(ByVal File() As Byte, ByVal FileName As String, ByVal UplName As String, ByVal IDUpl As Integer, ByVal note As String, ByVal IDComunita As Integer) As String
		Dim ErrorString As String
		If Me.GetFileNum(IDComunita) <= MaxNumFile Then
			If Me.GetLvl(IDUpl, IDComunita) > 1 Then
				'ErrorString = Application("FileMngmt").WriteFile(File, FileName, UplName, IDUpl, note, IDComunita)
				ErrorString = Application("FileMngmt" & IDComunita.ToString).WriteFile(File, FileName, UplName, IDUpl, note, IDComunita)
			Else
				ErrorString = "Non si dispongono di permessi sufficienti"
			End If
		Else
			ErrorString = "Non e' possibile aggiungere altri file. Cancellarne qualcuno e riprovare"
		End If
		Return ErrorString
    End Function

    '<WebMethod(Description:="Effettua l'upload del file")> _
    'Public Function GetFileBaseUrl() As String

    '    Dim str As String = "/TmpFile/"
    '    Return str
    'End Function

	<WebMethod(Description:="Restituisce l'elenco dei file disponibili")> _
		   Public Function GetFileList(ByVal IDUt As Integer, ByVal IDComunita As Integer) As DataSet
		Dim DsTemp As New DataSet
		Try
			'DsTemp.Tables.Add(Application("FileMngmt").GetListFile(IDComunita))
			DsTemp.Tables.Add(Application("FileMngmt" & IDComunita.ToString).GetListFile(IDComunita))
		Catch ex As Exception

		End Try
		Return DsTemp.Copy
	End Function

	<WebMethod(Description:="Restituisce la directory temporanea dei file")> _
		   Public Function GetTmpDir(ByVal IDUt As Integer, ByVal IDComunita As Integer) As String
		'GetTmpDir(Session("objPersona").Id, Session("IdComunita"))
		Dim Path As String
		If Me.GetLvl(IDUt, IDComunita) > 0 Then
			'Path = Application("FileMngmt").TmpDir
			Path = Application("FileMngmt" & IDComunita.ToString).TmpDir
		Else
			Path = "Non autorizzato"
		End If
		Return Path	'& IDComunita.ToString
	End Function


	<WebMethod(Description:="Elimina il file specificato")> _
	   Public Function RemFile(ByVal FileName As String, ByVal IdUser As Integer, ByVal IDComunita As Integer) As String
		Dim ErrMsg As String = ""
		If Me.GetLvl(IdUser, IDComunita) > 0 Then
			'ErrMsg = Application("FileMngmt").EraseFile(FileName, IdUser, IDComunita)
			ErrMsg = Application("FileMngmt" & IDComunita.ToString).EraseFile(FileName, IdUser, IDComunita)
		Else
			ErrMsg = "Non autorizzato"
		End If
		Return ErrMsg
	End Function

	<WebMethod(Description:="Recupera il numero di file uplodati per ogni comunita")> _
		   Public Function GetFileNum(ByVal IDComunita As Integer) As Integer
		'Return Application("FileMngmt").GetNumFile(IDComunita)
		Return Application("FileMngmt" & IDComunita.ToString).GetNumFile(IDComunita)
	End Function

	<WebMethod(Description:="Se True esiste già un file con quel nome")> _
	   Public Function IsFileExist(ByVal FileName As String, ByVal IDComunita As Integer) As Boolean
		'Dim BoolTemp As Boolean = Application("FileMngmt").IsFileExist(FileName)
		Dim BoolTemp As Boolean = Application("FileMngmt" & IDComunita.ToString).IsFileExist(FileName)
		Return BoolTemp
	End Function

#End Region

#Region " Metodi Palm - Andrà rivista la sicurezza - Da togliere da ARIES finchè non implementata! Non Usare Per ULPFILE "

	'    <WebMethod(Description:="Palm: Login - 0 Id Persona 1 Comunità 2 Lvl Utente")> _
	'    Public Function Login(ByVal Nome As String, ByVal Password As String) As Integer()

	'        Dim ID(3) As Integer

	'        'ID(0) = ID PERSONA
	'        Select Case Nome
	'            Case "Admin"
	'                ID(0) = 4
	'            Case "Writer"
	'                ID(0) = 2
	'            Case "Reader"
	'                ID(0) = 1
	'            Case Else
	'                ID(0) = 0
	'        End Select

	'        'ID(1) Comunità di default
	'        ID(1) = 1

	'        'ID(2) Livello
	'        ID(2) = ID(0) 'SOLO PER SVILUPPO FUORI LABMA

	'        'Codice commentato per lo sviluppo esterno a LABMA
	'        ''Mi devo creare un array monodimensionale contenente ID comunita ed ID persona
	'        '' ... magari anche con il nome della comunita... Uso il seguente metodo: COL_Comunita.EstraiNome(IdComunita)

	'        ''Login persona
	'        'Dim Persona As New COL_Persona
	'        'Persona.Login = Nome    'Ce li ho...
	'        'Persona.Pwd = Password  'Ce li ho...
	'        'Persona.Logon()
	'        'ID(0) = Persona.Id
	'        'Dim oOrganizzazione As New COL_Organizzazione
	'        ''mi carica nell'oggetto l'id dell'organizzazione di default dell'organizzazione

	'        'Persona.GetOrganizzazioneDefault()

	'        'oOrganizzazione.OrganizzazioneDefaultByPersona(Persona.Id)

	'        'ID(1) = oOrganizzazione.RitornaComunitaOrganizzazione
	'        'Dim int1 As Integer = ID(0)
	'        'Dim int2 As Integer = ID(1)
	'        'servizio.PermessiAssociati = Me.Permessi(int1, int2)

	'        ''Dim Lvl As Integer = 0
	'        ''Inserire qui il livello di utenza -> vedi bussiness logic...
	'        'If servizio.Admin Or servizio.GestionePermessi Then 'servizio.Grant Or servizio.Admin Or servizio.Moderate Then
	'        '    ID(2) = 4 'Admin
	'        'ElseIf servizio.Write Then
	'        '    ID(2) = 2 'Writer
	'        'ElseIf servizio.Read Then
	'        '    ID(2) = 1 'Reader
	'        'Else
	'        '    ID(2) = 0 'Out! (utente senza permesso)
	'        'End If

	'        ''Inserisce l'utente
	'        'If Not ID(2) = 0 Then
	'        '    Me.NuovoUtente(ID(0), Nome, ID(1))
	'        'End If

	'        Return ID

	'    End Function

	'    <WebMethod(Description:="PALM: recura l'elenco delle comunità")> _
	'Function GetComunita(ByVal id As Integer) As DataSet

	'        Dim dsComunitaOUT As New DataSet
	'        'TAGLIATO PER SVILUPPO FUORI LABMA
	'        'Dim oPersona As New COL_Persona

	'        '''Definizione dell'oggeto che mando in output '- Vedere se è il caso di farlo lato CLIENT!!!
	'        'Dim dsComunitaOut As New DataSet
	'        'Dim dtComunitaOut As New DataTable("Comunita")
	'        ''Dim dtComunitaIn As New DataTable
	'        'Dim dcComunitaOut As DataColumn
	'        'Dim drComunitaOut As DataRow
	'        ''Try

	'        'dcComunitaOut = New DataColumn
	'        'dcComunitaOut.DataType = System.Type.GetType("System.Int32")
	'        'dcComunitaOut.ColumnName = "ID"
	'        'dtComunitaOut.Columns.Add(dcComunitaOut)

	'        'dcComunitaOut = New DataColumn
	'        'dcComunitaOut.DataType = System.Type.GetType("System.String")
	'        'dcComunitaOut.ColumnName = "Nome"
	'        'dtComunitaOut.Columns.Add(dcComunitaOut)

	'        'dtComunitaOut.AcceptChanges()


	'        'oPersona.Id = id
	'        'dsComunita = oPersona.ElencaComunitaAppartenenza(0) 'Tiro su tutti i tipi di comunita...

	'        'For i As Integer = 0 To dsComunita.Tables.Count - 1
	'        '    For Each Row As DataRow In dsComunita.Tables(i).Rows
	'        '        drComunitaOut = dtComunitaOut.NewRow
	'        '        drComunitaOut("ID") = Row("CMNT_id") 'dsComunita.Tables(i).Rows.Item(1) 'Controllare!!!!
	'        '        drComunitaOut("Nome") = Row("CMNT_nome") 'dsComunita.Tables(i).Rows.Item(2) 'dsComunita.Tables(i).Rows("CMNT_nome")
	'        '        dtComunitaOut.Rows.Add(drComunitaOut)
	'        '        dtComunitaOut.AcceptChanges()
	'        '    Next
	'        'Next
	'        ''For Each dtComunitaIn In dsComunita.Tables
	'        ''    drComunitaOut = dtComunitaOut.NewRow
	'        ''    drComunitaOut("ID") = dtComunitaIn.Rows.Item("RLPC_CMNT_id") 'Controllare!!!!
	'        ''    drComunitaOut("Nome") = dtComunitaIn.Rows.Item("CMNT_nome")
	'        ''    dtComunitaOut.Rows.Add(drComunitaOut)
	'        ''    dtComunitaOut.AcceptChanges()
	'        ''Next

	'        ''Catch ex As Exception
	'        ''    Dim str As String = ex.ToString
	'        ''End Try
	'        'dsComunitaOut.Tables.Add(dtComunitaOut)
	'        Return dsComunitaOUT '.Copy 'ATTENZIONE: PER SVILUPPO RESTITUISCE OGGETTO VUOTO O NULL
	'    End Function

	'    <WebMethod(Description:="PALM: Recupero messaggio")> _
	' Public Function RecuperaMessaggioPalm(ByVal IdUtente As Integer, ByVal IDComunita As Integer) As DataSet 'ByVal Tempo As DateTime,

	'        Dim DsMsgOut As New DataSet
	'        'Application.Lock()
	'        'DsMsgOut.Tables.Clear()
	'        DsMsgOut.Tables.Add(Application(IDComunita.ToString).EstraiPalm(CInt(IdUtente), Application("oBlock").GetBlockFrom(IdUtente)).Copy) 'CDate(Tempo), 
	'        'Application.UnLock()
	'        Return DsMsgOut.Copy

	'    End Function

#End Region

#Region " Metodi interni - Permessi - Elimina comunita"

	Private Function Permessi(ByVal IdPersona As Integer, ByVal IdComunita As Integer) As String
		''Parte aggiunta per le modifiche esterne a LABMA
		''Elenco IdPersona e relativi permessi:
		''Id 1   Grant   
		'Dim PermessoUtente As String = "0"

		'Select Case IdPersona
		'    Case 4
		'        PermessoUtente = "4"
		'    Case 2
		'        PermessoUtente = "2"
		'    Case 1
		'        PermessoUtente = "1"
		'    Case Else
		'        PermessoUtente = "0"
		'End Select

		''Questa parte è commentata per lo sviluppo fuori dal laboratorio...
		''Byte '-> Ricavo i permessi direttamente dalla ColChat
		''passando l'IdPersona ed IdComunità...
		''dal nome del servizio ricavo i permessi (credenziali) che l'utente ha,

		''Creo l'array che NON posso avere in sessione
		Dim oRuolo As New COL_RuoloPersonaComunita
		Dim PermessoUtente As String ' stringa di 32 bit contenente il valore dei permessi con cui l'utente entra nella pagina

		'oRuolo.Persona.Id = IdPersona
		'oRuolo.Comunita.Id = IdComunita

		PermessoUtente = "00000000000000000000000000000000"

		Try
			'oRuolo.Estrai(IdComunita, IdPersona)  'IdPersona, IdComunita)
			'If oRuolo.Errore = Errori_Db.None And oRuolo.Abilitato = True And oRuolo.Attivato = True Then
			PermessoUtente = COL_Comunita.GetPermessiForServizioByPersona(IdPersona, IdComunita, Services_CHAT.Codex)
			'End If
		Catch ex As Exception
			PermessoUtente = "00000000000000000000000000000000"
		End Try
		''oRuolo.CaricaRuoloPersonaComunita() 'Importante, serve per caricare effettivamente i dati, altrimenti non ho nulla...
		Return PermessoUtente 'PermessoUtente
	End Function

	Private Sub EliminaComunita(ByVal IdComunita As Integer)

		Application(IdComunita.ToString).Svuota()
		Application.Remove(IdComunita.ToString)

		Application("ListaComunita").Remove(IdComunita.ToString)

		Try
			'Elimino tutti i file di quella comunita
			Application("FileMngmt" & IdComunita.ToString).EraseALL(IdComunita)
			Application.Remove("FileMngmt" & IdComunita.ToString)
		Catch ex As Exception
			'Se non sono presenti file, non esiste la relativa application
		End Try
	End Sub

	Private Sub CreaComunita(ByVal IdComunita As Integer)
		Application.Add(IdComunita.ToString, New OChtMsgTemp(Server.MapPath("."), Me.MaxMsg, Me.MinMsg, IdComunita))
		Application(IdComunita.ToString).MaxMsg = MaxMsg
		Application(IdComunita.ToString).MinMsg = MinMsg
		Application(IdComunita.ToString).TimDis = MaxDisc

		If Application("ListaComunita") Is Nothing Then
			Application("ListaComunita") = New ArrayList
		End If
		Application("ListaComunita").add(IdComunita.ToString)

	End Sub
#End Region

#Region "LOG ERRORI"
	Private Sub LogError(ByVal ErrorMessage As String)
		If Me.SaveException Then
			Dim FilePath As String
			FilePath = System.Configuration.ConfigurationManager.AppSettings("LogPath")
			FilePath &= System.Configuration.ConfigurationManager.AppSettings("ErrorLogFile")

			Dim StringOut As String = ""
			StringOut = Now().ToString
			StringOut &= vbCrLf
			StringOut &= ErrorMessage
			StringOut &= vbCrLf & vbCrLf

			Dim Fs As StreamWriter = File.AppendText(FilePath)
			Try
				Fs.Write(StringOut)
			Catch ex As Exception
			Finally
				Fs.Close()
			End Try
		End If
	End Sub
#End Region

	Private ReadOnly Property SaveException() As Boolean
		Get
			If IsNothing(Application("WBS_SaveException")) Then
				If System.Configuration.ConfigurationManager.AppSettings("SaveException") = "True" Then
					Application("WBS_SaveException") = True
				Else
					Application("WBS_SaveException") = False
				End If
			End If

			Return CBool(Application("WBS_SaveException"))
		End Get
	End Property

End Class

#Region "OLD"
'<WebMethod(Description:="Recupero messaggi")> _
'Public Function RecuperaTuttiXML(ByVal Id As Integer, ByVal IDComunita As Integer) As String

'    'SOLO PER DEBUG, DA TOGLIERE APPENA IMPLEMENTATO L'UTENTE
'    'If Application(IDComunita.ToString) Is Nothing Then
'    '    Application.Add(IDComunita.ToString, New OMsgTemp)
'    'End If

'    Dim dsTemp As New DataSet
'    'If Application(IDComunita.ToString).GetLvlPers(Id) = 4 Then 'Se è admin
'    Application.Lock()
'    dsTemp.Tables.Add(Application(IDComunita.ToString).Estrai()) 'Application("WBSChat").Estrai().Copy
'    Application.UnLock()
'    'End If
'    Return dsTemp.GetXml
'End Function

'Diventerà inutile...
' <WebMethod(Description:="Time di WbsChat")> _
'Public Function GetTime() As DateTime
'     'Da estendere anche al resto: altrimenti ci possono essere grossi problemi
'     'col recupero dei messaggi...
'     Return Now()
' End Function
#End Region

'NOTE:
'Rivedere gli application.lock!
' Metterli solo durante l'inserimento dei messaggi e verificare se funziona...
' Questo però potrebbe portare problemi durante il recupero... -> perdita di messaggi...


'Public Enum PermissionType
'    Read = 0
'    Write = 1
'    Change = 2
'    Delete = 3
'    Moderate = 4
'    Grant = 5
'    Admin = 6
'    Send = 7
'    Receive = 8
'    Synchronize = 9
'    Browse = 10
'    Print = 11
'    ChangeOwner = 12
'End Enum