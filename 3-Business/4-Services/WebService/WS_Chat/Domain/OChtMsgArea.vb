'Uguale a oChtMsgTemp.vb, con la differenza che è stato tolto il blocco per gli utenti. Per ripristinarlo basta togliere questa classe e nel codice che utlizza questa classe utilizzare oChtMsgTemp.vb...

Imports System.Data
Imports System.Web

Public Class OChtMsgArea

	Dim DSTemp As New DataSet 'Dataset contenente i messaggi

	' Da inserire nella Tabellina di Molinari...
	Private IMinRow, IMaxRow As Integer	'Dimensioni della tabella "DTTemp" contenente i messaggi
	Private MaxDisconnect As Integer = 300 '120        'Numero di SECONDI in cui l'utente può rimanere disconnesso senza venir eliminato dalla lista...

#Region " Metodi New - V 12/03/2004 "

	Public Sub New()

		' Inizializzazione variabili
		'Da valutare ed inserire nella tabella che diceva Molinari
		Me.MaxMsg = 50 'Numero di righe massime della tabella
		Me.MinMsg = 25 'Numero di righe a cui viene ridimensionata la tabella

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

#Region " Metodi Estrai MESSAGGI - V 12/03/2004 "

	'Ritorna TUTTI i messaggi di TUTTE le persone - V 27/12/04 - SOLO PER ADMIN
	Public Function Estrai() As DataTable

		'Dim DTOut As New DataTable
		'DTOut = DSTemp.Tables("DTTemp").Copy
		'Dim dr As DataRow
		'For Each dr In DTOut.Rows
		'    If dr.Item("Tempo") <= LastUpd Then
		'        dr.Delete()
		'    End If
		'Next
		'DTOut.AcceptChanges()
		Return DSTemp.Tables("DTTemp").Copy
	End Function

	'Ritorna gli ULTIMI messaggi di UNA specifica persona assieme a quelli pubblici (ID=0)
	'senza di blocco utenti...
	Public Function Estrai(ByVal IDPersona As Integer) As DataTable	'ByVal LastUpd As DateTime,

		Dim DTOut As New DataTable
		DTOut = DSTemp.Tables("DTTemp").Clone

		Dim drout As DataRow
		Dim CopyOk1, CopyOk2 As Boolean

		Dim LastConn As DateTime = Me.GetLastTime(IDPersona)

		For Each RowMsg As DataRow In DSTemp.Tables("DTTemp").Rows
			'Flag per poter separare le varie verifiche...

			CopyOk1 = False
			CopyOk2 = False

			'Ultimi messaggi
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

			If (CopyOk1 And CopyOk2) Then
				drout = DTOut.NewRow
				For i As Integer = 0 To 9
					drout.Item(i) = RowMsg.Item(i)
				Next

				DTOut.Rows.Add(drout)
				DTOut.AcceptChanges()
			End If
		Next

		Me.AggiornaPersona(IDPersona) 'Proviamo a metterlo qua... :rolleye:
		Return DTOut.Copy
	End Function

#End Region

#Region " Metodi getione LISTA UTENTI - No Block"

	'Ritorna la lista degli utenti connessi - tolto il blocco...
	Public Function EstraiUtenti() As DataTable
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

		Dim TbUtenti As New DataTable '= DSTemp.Tables("DTUtenti").Copy()
		TbUtenti = DSTemp.Tables("DTUtenti").Copy()

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

			Me.AggiungiMsg(Nome, "E' entrato nella chat", Id, False, False, False, "ffffff", "000000") 'lo fa per tutti, anche se non possono inviare messaggi

		Catch ex As Exception
			Dim str As String = ex.ToString
			'Me.AggiornaPersona(Id)
		End Try
	End Sub

	'Necessario farlo su ID in quanto univoco!!!
	Public Sub EliminaPersona(ByVal Id As Integer)

		Dim i As Integer = 0

		For i = 0 To DSTemp.Tables("DTUtenti").Rows.Count - 1
			If DSTemp.Tables("DTUtenti").Rows(i).Item("Id") = Id Then Exit For
		Next
		If Not i = 0 Then
			DSTemp.Tables("DTUtenti").Rows.RemoveAt(i)
			DSTemp.AcceptChanges()
		End If
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
		Me.AggiungiMsg(nome, "Il tuo livello di utenza è stato cambiato", -1, IdPers, False, False, False, "ffffff", "000000") ' Now(),
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
		DSTemp.Tables("DTTemp").Rows.Clear()
	End Sub

#End Region

#Region " Metodi privati - Da controllare se lo esegue... "

	'Elimina i dati in eccesso
	Private Sub Ridimensiona()
		If IMinRow <= IMaxRow Then	'Esegue solo se necessario...
			If DSTemp.Tables("DTTemp").Rows.Count > IMaxRow Then
				For i As Integer = IMinRow To (DSTemp.Tables("DTTemp").Rows.Count)
					DSTemp.Tables("DTTemp").Rows.RemoveAt(0)
					DSTemp.AcceptChanges() 'Verificare se è possibile mettere questo fuori dal ciclo...
				Next
			End If
		End If
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

#End Region

#Region "Metodi tolti..."

	'Public Function EstraiPalm(ByVal IDPersona As Integer, ByVal DtBlock As DataTable) As DataTable 'ByVal LastUpd As DateTime,

	'    Dim DTOut As New DataTable
	'    DTOut = DSTemp.Tables("DTTemp").Clone

	'    Dim drout As DataRow
	'    Dim CopyOk1, CopyOk2, CopyOk3 As Boolean

	'    For Each RowMsg As DataRow In DSTemp.Tables("DTTemp").Rows
	'        'Flag per poter separare le varie verifiche...

	'        CopyOk1 = False
	'        CopyOk2 = False
	'        CopyOk3 = True

	'        'Blocco utenti
	'        If DtBlock.Rows.Count > 0 Then
	'            Dim RowBlock As DataRow
	'            For Each RowBlock In DtBlock.Rows
	'                If RowBlock(1) = RowMsg.Item("IDMitt") Then
	'                    CopyOk3 = False
	'                End If
	'            Next
	'        End If

	'        'Ultimi messaggi
	'        Dim LastConn As DateTime = Me.GetLastTime(IDPersona)
	'        Dim MsgTime As DateTime = RowMsg.Item("Tempo")
	'        If MsgTime >= LastConn Then 'LastUpd
	'            CopyOk1 = True
	'            'Lasciare in commento, che non si sa mai...
	'            'ElseIf MsgTime = LastConn Then
	'            '    If MsgTime.Millisecond() >= LastConn.Millisecond() Then
	'            '        CopyOk1 = True
	'            '    End If
	'        End If

	'        '   - Tutti quelli inviati da me -     Tutti i messagi per me         -    Tutti i messaggi per tutti
	'        If ((RowMsg.Item("IDMitt") = IDPersona) Or (RowMsg.Item("IDDest") = IDPersona) Or (RowMsg.Item("IDDest") = 0)) Then
	'            CopyOk2 = True
	'        End If

	'        If (CopyOk1 And CopyOk2 And CopyOk3) Then
	'            drout = DTOut.NewRow
	'            For i As Integer = 0 To 9
	'                drout.Item(i) = RowMsg.Item(i)
	'            Next
	'            drout.Item("Messaggio") = HttpUtility.HtmlDecode(drout.Item("Messaggio"))
	'            DTOut.Rows.Add(drout)
	'            DTOut.AcceptChanges()
	'        End If
	'    Next

	'    Me.AggiornaPersona(IDPersona) 'Proviamo a metterlo qua... :rolleye:
	'    Return DTOut.Copy
	'End Function

	'Ritorna tutto - V 27/12/04

	'Public Function Estrai() As DataTable
	'    Return DSTemp.Tables("DTTemp").Copy
	'End Function

#End Region

End Class
