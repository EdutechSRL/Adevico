Imports System.Data
Imports System.Web
'Imports System.IO

Public Class OChtMsgTemp
	Dim DSTemp As New DataSet 'Dataset contenente i messaggi

	' Da inserire nella Tabellina di Molinari...
	Private IMinRow, IMaxRow, IDCom As Integer 'Dimensioni della tabella "DTTemp" contenente i messaggi
	Private MaxDisconnect As Integer = 300 '120        'Numero di SECONDI in cui l'utente può rimanere disconnesso senza venir eliminato dalla lista...

	'Me.Path & Me.TmpFileDir & FileName
	'Dim path As String
	'Dim TmpXMLStoreDir As String = "BackupMessaggi"
	Dim FileName As String = "\LOG\WBS_CHAT_MESSAGGI_ID-"
	Dim dir As String

#Region " Metodi New - V 12/03/2004 "

	Public Sub New(ByVal SrvName As String, ByVal MaxMessaggi As Integer, ByVal MinMessaggi As Integer, ByVal IDComunita As Integer)
		Me.dir = SrvName
		' Inizializzazione variabili
		'Da valutare ed inserire nella tabella che diceva Molinari
		Me.MaxMsg = MaxMessaggi	'50 'Numero di righe massime della tabella
		Me.MinMsg = MinMessaggi	'25 'Numero di righe a cui viene ridimensionata la tabella
		Me.IDCom = IDComunita
		' Declare DataColumn and DataRow variables.
		Dim DTTemp As New DataTable("DTTemp")
		Dim DCTemp As DataColumn

		' I colonna: nome utente | Da eliminare se collego questa tabella con quella degli utenti...
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "Nome"
		DTTemp.Columns.Add(DCTemp)

		' II colonna: ora di inserimento del messaggio
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.DateTime")
		DCTemp.ColumnName = "Tempo"
		DTTemp.Columns.Add(DCTemp)
		DTTemp.Constraints.Add("KTime", DCTemp, False)

		' III colonna: corpo messaggio
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "Messaggio"
		DTTemp.Columns.Add(DCTemp)

		' IV colonna: ID mittente del messaggio | 0 riservato al sistema
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDMitt"
		DTTemp.Columns.Add(DCTemp)
		'DTTemp.Constraints.Add("KIdMitt", DCTemp, False)

		' V colonna: ID destinatario | 0 messaggio per tutti
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDDest" '0 per i messaggi inviati dal sistema
		DTTemp.Columns.Add(DCTemp)
		'DTTemp.Constraints.Add("KIdDest", DCTemp, False)

		' VI colonna: Tipo Bold
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Boolean")
		DCTemp.ColumnName = "TBold"
		DCTemp.DefaultValue = False
		DTTemp.Columns.Add(DCTemp)

		' VII colonna: Tipo Italic
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Boolean")
		DCTemp.ColumnName = "TItalic"
		DCTemp.DefaultValue = False
		DTTemp.Columns.Add(DCTemp)

		' VIII colonna: Tipo Underline
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Boolean")
		DCTemp.ColumnName = "TUnderline"
		DCTemp.DefaultValue = False
		DTTemp.Columns.Add(DCTemp)

		' IX colonna: Tipo BGColor
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "TBGColor"
		DCTemp.DefaultValue = "ffffff"
		DTTemp.Columns.Add(DCTemp)

		' X colonna: Tipo TextColor
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "TColor"
		DCTemp.DefaultValue = "000000"
		DTTemp.Columns.Add(DCTemp)

		DSTemp.Tables.Add(DTTemp)
		DSTemp.AcceptChanges()
		'###############################################################################################

		Dim keys(1) As DataColumn
		Dim DTTemp2 As New DataTable("DTUtenti")

		' I colonna: IDUtente | Da verificare con COL...
		DCTemp = New DataColumn
		With DCTemp
			.DataType = System.Type.GetType("System.Int32")
			.ColumnName = "ID"
			.Unique = True
			.AllowDBNull = False
		End With
		DTTemp2.Columns.Add(DCTemp)

		' II colonna: Nome | Nome utente
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "Nome"
		DTTemp2.Columns.Add(DCTemp)

		' III colonna: Last connection | DateTime -> serve per controllare l'inattività di un utente ed eliminarlo dalla lista...
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.DateTime")
		DCTemp.ColumnName = "LastCon"
		DTTemp2.Columns.Add(DCTemp)

		' IV colonna: UsLvl | Byte - UserLevel -> server per controllare il livello di accesso di una persona... (DA IMPLEMENTARE...)
		' Valori: 0 - Reader; 1 - Writer; 4 - Admin
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Byte")
		DCTemp.ColumnName = "UtLvl"
		DTTemp2.Columns.Add(DCTemp)

		'------------------------------------
		DSTemp.Tables.Add(DTTemp2)
		DSTemp.AcceptChanges()

		'Crea la riga di inizializzazione
		Me.AggiungiMsg("System", "Chat avviata correttamente", -1, 0, False, False, False, "ffffff", "000000") '-1 ID del sistema!!! ', Now()
	End Sub

#End Region

#Region " Metodi Aggiungi MESSAGGI - V 12/03/2004 "

	'Aggiunge un messaggio generico: ID=0 -> System
	Public Sub AggiungiMsg(ByVal Nome As String, ByVal Messaggio As String, ByVal IDMitt As Integer, ByVal IsBold As Boolean, ByVal IsItalic As Boolean, ByVal IsUnderline As Boolean, ByVal TxtBgColor As String, ByVal TxtColor As String) ', ByVal Tempo As DateTime
		If Me.GetLvlPers(IDMitt) > 0 Or IDMitt = -1 Then 'se -1 è il sistema...
			Me.Ridimensiona()
			Dim DRTemp As DataRow

			DRTemp = DSTemp.Tables("DTTemp").NewRow
			DRTemp("Nome") = Nome
			DRTemp("Tempo") = Now()	'Tempo
			DRTemp("Messaggio") = Messaggio
			DRTemp("IDMitt") = IDMitt
			DRTemp("IDDest") = 0
			DRTemp("TBold") = IsBold
			DRTemp("TItalic") = IsItalic
			DRTemp("TUnderline") = IsUnderline
			DRTemp("TColor") = TxtColor
			DRTemp("TBgColor") = TxtBgColor
			DSTemp.Tables("DTTemp").Rows.Add(DRTemp)
			DSTemp.AcceptChanges()
		End If
	End Sub

	'Aggiunge un messaggio per un utente specifico
	Public Sub AggiungiMsg(ByVal Nome As String, ByVal Messaggio As String, ByVal IDMitt As Integer, ByVal IDDest As Integer, ByVal IsBold As Boolean, ByVal IsItalic As Boolean, ByVal IsUnderline As Boolean, ByVal TxtBgColor As String, ByVal TxtColor As String) ', ByVal Tempo As DateTime
		If Me.GetLvlPers(IDMitt) > 0 Or IDMitt = -1 Then 'se -1 è il sistema...
			Me.Ridimensiona()
			Dim DRTemp As DataRow

			DRTemp = DSTemp.Tables("DTTemp").NewRow
			DRTemp("Nome") = Nome
			DRTemp("Tempo") = Now()	'Tempo
			DRTemp("Messaggio") = Messaggio
			DRTemp("IDMitt") = IDMitt
			DRTemp("IDDest") = IDDest
			DRTemp("TBold") = IsBold
			DRTemp("TItalic") = IsItalic
			DRTemp("TUnderline") = IsUnderline
			DRTemp("TColor") = TxtColor
			DRTemp("TBgColor") = TxtBgColor
			DSTemp.Tables("DTTemp").Rows.Add(DRTemp)
			DSTemp.AcceptChanges()
		End If
	End Sub

#End Region

#Region " Metodi Estrai MESSAGGI - V 17/02/2005 "

	'Ritorna tutto - V 17/02/2005
	Public Function Estrai() As DataTable
		Try
			Dim oDataset As New DataSet
			Dim oDataview As DataView
			oDataset = DSTemp
			oDataview = oDataset.Tables("DTTemp").DefaultView
			oDataview.Sort = "Tempo DESC"

			Return oDataview.Table.Copy
		Catch ex As Exception
			Return DSTemp.Tables("DTTemp").Copy
		End Try
	End Function

	'Ritorna gli ULTIMI messaggio di TUTTE le persone - V 27/12/04 - SOLO PER ADMIN
	Public Function Estrai(ByVal LastUpd As DateTime) As DataTable
		Dim oDataTable As New DataTable
		Dim oDataview As DataView

		Try
			oDataTable = DSTemp.Tables("DTTemp").Copy
			Dim oDataRow As DataRow
			For Each oDataRow In oDataTable.Rows
				If oDataRow.Item("Tempo") <= LastUpd Then
					oDataRow.Delete()
				End If
			Next
			oDataTable.AcceptChanges()
			oDataview = oDataTable.DefaultView
			oDataview.Sort = "Tempo DESC"

			Return oDataview.Table.Copy
		Catch ex As Exception
			Return oDataTable.Copy
		End Try
	End Function

	'Ritorna gli ULTIMI messaggi di UNA specifica persona assieme a quelli pubblici (ID=0)
	'con tanto di blocco utenti... - V 22/04/04
	Public Function Estrai(ByVal IDPersona As Integer, ByVal DtBlock As DataTable) As DataTable	'ByVal LastUpd As DateTime,
		Dim oDataTable As New DataTable
		Dim oDataview As DataView
		Dim CopyOk1, CopyOk2, CopyOk3 As Boolean

		Try
			oDataTable = DSTemp.Tables("DTTemp").Clone
			Dim oDataRow As DataRow


			For Each RowMsg As DataRow In DSTemp.Tables("DTTemp").Rows
				'Flag per poter separare le varie verifiche...
				CopyOk1 = False
				CopyOk2 = False
				CopyOk3 = True

				'Blocco utenti
				If DtBlock.Rows.Count > 0 Then
					Dim RowBlock As DataRow
					For Each RowBlock In DtBlock.Rows
						If RowBlock(1) = RowMsg.Item("IDMitt") Then
							CopyOk3 = False
						End If
					Next
				End If

				'Ultimi messaggi
				Dim LastConn As DateTime = Me.GetLastTime(IDPersona)
				Dim MsgTime As DateTime = RowMsg.Item("Tempo")
				If MsgTime >= LastConn Then	'LastUpd
					CopyOk1 = True
					'Lasciare in commento, che non si sa mai...
					'ElseIf MsgTime = LastConn Then
					'    If MsgTime.Millisecond() >= LastConn.Millisecond() Then
					'        CopyOk1 = True
					'    End If
				End If

				'   - Tutti quelli inviati da me -     Tutti i messagi per me         -    Tutti i messaggi per tutti
				If ((RowMsg.Item("IDMitt") = IDPersona) Or (RowMsg.Item("IDDest") = IDPersona) Or (RowMsg.Item("IDDest") = 0)) Then
					CopyOk2 = True
				End If

				If (CopyOk1 And CopyOk2 And CopyOk3) Then
					oDataRow = oDataTable.NewRow
					For i As Integer = 0 To 9
						oDataRow.Item(i) = RowMsg.Item(i)
					Next

					oDataTable.Rows.Add(oDataRow)
					oDataTable.AcceptChanges()
				End If
			Next
			Me.AggiornaPersona(IDPersona) 'Proviamo a metterlo qua... :rolleye:
			oDataview = oDataTable.DefaultView
			oDataview.Sort = "Tempo DESC" 'DESC

			Return oDataview.Table.Copy	'oDataTable.Copy 'oDataview.Table.Copy
		Catch ex As Exception
			Return oDataTable.Copy
		End Try



	End Function

	Public Function EstraiPalm(ByVal IDPersona As Integer, ByVal DtBlock As DataTable) As DataTable	'ByVal LastUpd As DateTime,

		Dim DTOut As New DataTable
		DTOut = DSTemp.Tables("DTTemp").Clone

		Dim drout As DataRow
		Dim CopyOk1, CopyOk2, CopyOk3 As Boolean

		For Each RowMsg As DataRow In DSTemp.Tables("DTTemp").Rows
			'Flag per poter separare le varie verifiche...

			CopyOk1 = False
			CopyOk2 = False
			CopyOk3 = True

			'Blocco utenti
			If DtBlock.Rows.Count > 0 Then
				Dim RowBlock As DataRow
				For Each RowBlock In DtBlock.Rows
					If RowBlock(1) = RowMsg.Item("IDMitt") Then
						CopyOk3 = False
					End If
				Next
			End If

			'Ultimi messaggi
			Dim LastConn As DateTime = Me.GetLastTime(IDPersona)
			Dim MsgTime As DateTime = RowMsg.Item("Tempo")
			If MsgTime >= LastConn Then	'LastUpd
				CopyOk1 = True
				'Lasciare in commento, che non si sa mai...
				'ElseIf MsgTime = LastConn Then
				'    If MsgTime.Millisecond() >= LastConn.Millisecond() Then
				'        CopyOk1 = True
				'    End If
			End If

			'   - Tutti quelli inviati da me -     Tutti i messagi per me         -    Tutti i messaggi per tutti
			If ((RowMsg.Item("IDMitt") = IDPersona) Or (RowMsg.Item("IDDest") = IDPersona) Or (RowMsg.Item("IDDest") = 0)) Then
				CopyOk2 = True
			End If

			If (CopyOk1 And CopyOk2 And CopyOk3) Then
				drout = DTOut.NewRow
				For i As Integer = 0 To 9
					drout.Item(i) = RowMsg.Item(i)
				Next
				drout.Item("Messaggio") = HttpUtility.HtmlDecode(drout.Item("Messaggio"))
				DTOut.Rows.Add(drout)
				DTOut.AcceptChanges()
			End If
		Next

		Me.AggiornaPersona(IDPersona) 'Proviamo a metterlo qua... :rolleye:
		Return DTOut.Copy
	End Function

#End Region

#Region " Metodi getione LISTA UTENTI - V 12/03/2004"

	'Ritorna la lista degli utenti connessi - FUNZIONA!!!! ColBlocco!!!
	Public Function EstraiUtenti(ByVal DtBlock As DataTable) As DataTable

		Dim DTOut As New DataTable

		'Parte di aggiornamento della lista: vengono tolti gli utenti "vecchi"...
		Dim i, j, k As Integer
		i = 0
		k = DSTemp.Tables("DTUtenti").Rows.Count - 1
		For j = 0 To k
			If DSTemp.Tables("DTUtenti").Rows(i).Item("Id") > 0 Then 'serve per saltare il System
				If (DateTime.Compare(DSTemp.Tables("DTUtenti").Rows(i).Item("LastCon").AddSeconds(MaxDisconnect), Now()) < 0) Then
					Me.AggiungiMsg(DSTemp.Tables("DTUtenti").Rows(i).Item("Nome"), "È uscito dalla chat", 0, False, False, False, "000000", "ffffff") 'From System
					DSTemp.Tables("DTUtenti").Rows.RemoveAt(i)
				Else
					i += 1
				End If
			End If
		Next
		DSTemp.AcceptChanges()
		' Fine parte aggiornamento

		'Parte per l'eliminazione delle righe contenente le informazioni di chi ha effettueto il blocco... (IDFROM)
		Dim TbUtenti As New DataTable '= DSTemp.Tables("DTUtenti").Copy()
		TbUtenti = DSTemp.Tables("DTUtenti").Copy()

		Dim DsBlockRowCount As Integer
		DsBlockRowCount = DtBlock.Rows.Count

		If DtBlock.Rows.Count > 0 Then	  'Al primo giro DEVE essere vuoto, quindi salatare questo blocco if!!!!!!!!

			Dim RowUt, RowBlock As DataRow
			'i = 0
			Dim IdBlock(TbUtenti.Rows.Count) As Integer	'Array contenente l'indice delle righe da eliminare
			i = 0 'Indice delle colonne da aliminare
			j = -1 'Indice dell'array
			For Each RowUt In TbUtenti.Rows
				For Each RowBlock In DtBlock.Rows
					If RowUt(0) = RowBlock(0) Then
						j += 1
						IdBlock(j) = New Integer
						IdBlock(j) = i
					End If
				Next
				i += 1
			Next

			If j > -1 Then
				For i = 0 To j
					TbUtenti.Rows.RemoveAt(IdBlock(i))
					TbUtenti.AcceptChanges()
				Next
			End If

		End If

		Return TbUtenti.Copy 'DSTemp.Tables("DTUtenti").Copy() 'TbUtenti.Copy 'DSTemp.Tables("DTUtenti").Copy()
	End Function

	Public Sub AggiungiPersona(ByVal Id As Integer, ByVal Nome As String, ByVal UsrLvl As Byte)
		Dim drtemp As DataRow
		drtemp = DSTemp.Tables("DTUtenti").NewRow
		Try		'prova ad inserire, se non ci riesce (utente già inserito), NON aggiorna il time
			drtemp.Item("ID") = Id 'E' univoco - controllare le collisioni: nel caso 2 utenti abbiano ID uguale...
			drtemp.Item("Nome") = Nome
			drtemp.Item("LastCon") = Now()
			drtemp.Item("UtLvl") = UsrLvl
			DSTemp.Tables("DTUtenti").Rows.Add(drtemp)
			DSTemp.AcceptChanges()

			'drtemp = DSTemp.Tables("DTTemp").NewRow
			'drtemp("Nome") = Nome
			'drtemp("Tempo") = Now().AddMilliseconds(1) 'Tempo
			'drtemp("Messaggio") = "E' entrato nella chat"
			'drtemp("IDMitt") = Id
			'drtemp("IDDest") = 0
			'drtemp("TBold") = False
			'drtemp("TItalic") = False
			'drtemp("TUnderline") = False
			'drtemp("TColor") = "ffffff"
			'drtemp("TBgColor") = "000000"
			'DSTemp.Tables("DTTemp").Rows.Add(drtemp)
			'DSTemp.AcceptChanges()

            Me.AggiungiMsg(Nome, "Is in the conversation", Id, False, False, False, "ffffff", "000000")
			'Me.AggiungiMsg(Nome, "E' entrato nella chat", Id, False, False, False, "ffffff", "000000") 'lo fa per tutti, anche se non possono inviare messaggi
		Catch ex As Exception
			Dim str As String = ex.ToString
			'Me.AggiornaPersona(Id)
		End Try
	End Sub

	'Necessario farlo su ID in quanto univoco!!!
	Public Sub EliminaPersona(ByVal Id As Integer)
		' DA VERIFICARE!!!! Prima con row as DataRow ->Diventa inutile...
		Dim i As Integer = 0

		For i = 0 To DSTemp.Tables("DTUtenti").Rows.Count - 1
			If DSTemp.Tables("DTUtenti").Rows(i).Item("Id") = Id Then
				DSTemp.Tables("DTUtenti").Rows.RemoveAt(i)
				DSTemp.AcceptChanges()
				Exit Sub
			End If
		Next
	End Sub

	Private Sub AggiornaPersona(ByVal Id As Integer)
		'Aggiorna l'ultimo accesso di persona...
		Dim row As DataRow
		For Each row In DSTemp.Tables("DTUtenti").Rows
			If row.Item("Id") = Id And row.Item("UtLvl") > 0 Then	'Un utente bloccato non viene aggiornato...
				row.Item("LastCon") = Now()
			End If
		Next
		DSTemp.AcceptChanges()
	End Sub

	Public Sub SetLvlPers(ByVal IdPers As Integer, ByVal NewLvl As Byte)
		'Aggiorna l'ultimo accesso di persona...
		'Da wbs controllare l'accesso a questa funzione...
		Dim nome As String = ""
		Dim row As DataRow
		For Each row In DSTemp.Tables("DTUtenti").Rows
			If row.Item("Id") = IdPers Then
				row.Item("UtLvl") = NewLvl
				nome = row.Item("Nome")
			End If
		Next
		DSTemp.AcceptChanges()
        Me.AggiungiMsg(nome, "Your user permissions have been changed.", -1, IdPers, False, False, False, "ffffff", "000000") ' Now(),
	End Sub

	Public Function GetLvlPers(ByVal Id As Integer) As Byte
		Dim ByOut As Byte = 0 'di default ritorna 0 -> utente non in lista...
		Dim row As DataRow
		For Each row In DSTemp.Tables("DTUtenti").Rows
			If row.Item("Id") = Id Then
				ByOut = row.Item("UtLvl")
				Exit For
			End If
		Next
		Return ByOut
	End Function

	Public Sub ResetMsg()
		Dim MinTmp As Integer = Me.IMinRow
		Dim MaxTmp As Integer = Me.IMaxRow
		IMinRow = 0
		IMaxRow = 0
		Me.Ridimensiona()
		IMinRow = MinTmp
		IMaxRow = MaxTmp
	End Sub

#End Region

#Region " Metodi privati - Da controllare se lo esegue... "

	'Elimina i dati in eccesso
	Private Sub Ridimensiona()
		Dim XmlStringOut As String
		Dim DsStoreMsg As New DataSet
		DsStoreMsg = DSTemp.Copy

		'If IMinRow <= IMaxRow Then  'Esegue solo se necessario...
		'da togliere, poi...
		Try


			If DSTemp.Tables("DTTemp").Rows.Count > IMaxRow Then
				For i As Integer = 0 To (DSTemp.Tables("DTTemp").Rows.Count - 1)
					If i < IMinRow Then
						DsStoreMsg.Tables("DTTemp").Rows.RemoveAt(DsStoreMsg.Tables("DtTemp").Rows.Count - 1)
						DsStoreMsg.AcceptChanges()
					Else
						DSTemp.Tables("DTTemp").Rows.RemoveAt(0)
						DSTemp.AcceptChanges() 'Verificare se è possibile mettere questo fuori dal ciclo...
					End If
				Next
				XmlStringOut = DsStoreMsg.GetXml
				If Not XmlStringOut = "" Then
					XmlStringOut &= vbCrLf
					'For i As Integer = 0 To Messaggio.Length - 1
					'    File(i) = Microsoft.VisualBasic.AscW(Messaggio.Chars(i))
					'Next
					'Dim Fs As New FileStream(Me.Path & Me.LogFileName, FileMode.Append)
                    'Dim Fs As StreamWriter = File.AppendText(Me.dir & Me.FileName & Me.IDCom.ToString & ".LOG")
                    'Try
                    '                   Fs.Write()
                    'Catch ex As Exception
                    'Finally
                    '	Fs.Close()
                    '               End Try
                    lm.Comol.Core.File.Create.TextFile(Me.dir & Me.FileName & Me.IDCom.ToString & ".LOG", XmlStringOut, False, True)
				End If
			End If
		Catch ex As Exception

		End Try

		'Impostare MIN e MAX a 0 e richiamare questa sub per archiviare TUTTI i messaggi rimasti prima di chiudere... RICONTROLLARE!!!


		'XmlStringOut = DsStoreMsg.GetXml
		'XmlString2 = DSTemp.GetXml
		'End If
	End Sub

	'Recupera il Time dell'ultima connessione
	Private Function GetLastTime(ByVal IdUtente As Integer) As DateTime
		For Each Row As DataRow In DSTemp.Tables("DTUtenti").Rows
			If Row.Item("ID") = IdUtente Then
				Return Row.Item("LastCon")
			End If
		Next
	End Function

#End Region

#Region " Proprietà "
	'Per ora ancora sono qui... E' possibile variare le dimensioni del buffer a seconda delle necessità...

	Public WriteOnly Property MaxMsg() As Integer
		Set(ByVal Value As Integer)
			If Not Value <= IMinRow Then
				IMaxRow = Value
			End If
		End Set
	End Property

	Public WriteOnly Property MinMsg() As Integer
		Set(ByVal Value As Integer)
			If Not Value >= IMaxRow Then
				IMinRow = Value
			End If
		End Set
	End Property

	Public WriteOnly Property TimDis() As Integer
		Set(ByVal Value As Integer)
			MaxDisconnect = Value
		End Set
	End Property

	Public ReadOnly Property NumUtenti() As Integer
		Get
			Return DSTemp.Tables("DTUtenti").Rows.Count
		End Get
	End Property
#End Region

#Region "Destroy"
	Public Sub Svuota()
        Me.AggiungiMsg("System", "Closing chat", -1, 0, False, False, False, "ffffff", "000000")
		Me.IMaxRow = 0
		Me.IMinRow = 0
		Me.Ridimensiona()
	End Sub
#End Region
End Class

' NOTE SULL'IMPLEMENTAZIONE
' La classe OChtMsgTemp memorizza i messaggi ricevuti dai client, tramite i metodi Aggiungi
' e li restituisce tramite i metodi Estrai.
' Puo' restituirli in base all'utente ed alla data di ultimo aggiornamento, cioè
' restituire solo i messaggi che non sono ancora stati inviati
'
' Inoltre questa classe tiene traccia degli utenti connessi ed elimina quelli che non richiamano
' i metodi di recupero messaggi per TimDis() -> MaxDisconnect in secondi (Default = 300)
' 
' E' stato inserito anche il discorso del blocco delle persone...
'
' MANCA:
'   - farne BackUp dei dati in eccesso...
'   - ottimizzare le ricerche sulla gestione utenti aggiungendo una primary key...