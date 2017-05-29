'Lista dei file
'1 oggetto per ogni comunita NO!!! RIVEDERE IL CONCETTO!!! VEDI CANCELLZIONE TUTTI I FILE...
'Lista virtuale
'Dovrebbe corrispondere ai file su disco, ma non è sicuro!!!

Public Class oChtWbsFmList
	Dim DTFile As New DataTable("DTFile")

	Public Sub New()

		' Declare DataColumn and DataRow variables.
		'Dim DTTemp As New DataTable("DTTemp")
		Dim DCTemp As DataColumn

		' I colonna: nome file
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "Nome"
		DTFile.Columns.Add(DCTemp)

		' II colonna: ora di inserimento del messaggio
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.DateTime")
		DCTemp.ColumnName = "Tempo"
		DTFile.Columns.Add(DCTemp)
		DTFile.Constraints.Add("KTime", DCTemp, False)

		' III colonna: Nome Utente
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "Uploder"
		DTFile.Columns.Add(DCTemp)

		' IV colonna: ID mittente del File
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDMitt"
		DTFile.Columns.Add(DCTemp)
		'DTTemp.Constraints.Add("KIdMitt", DCTemp, False)

		' V colonna: Note del File
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "Note"
		DTFile.Columns.Add(DCTemp)

		' VI colonna: Stato del file
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDStato"
		DTFile.Columns.Add(DCTemp)

		' VII colonna: ID comunita
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDCom"
		DTFile.Columns.Add(DCTemp)

		' VIII colonna: Grandezza File
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "ByteSize"
		DTFile.Columns.Add(DCTemp)
		'###############################################################################################
	End Sub

	'Rimuove 1 elemento dalla lista
	'Public Sub RemListFile(ByVal nome As String, ByVal IDComunita As Integer)

	'    For Each TmpRow As DataRow In DTFile.Rows
	'        If TmpRow.Item("IDCom") = IDComunita And TmpRow.Item("Nome") = nome Then
	'            TmpRow.Delete()
	'        End If
	'    Next
	'End Sub

	Public Sub RemListFile(ByVal nome As String)
		'Dim i As Integer = 0
		For Each TmpRow As DataRow In DTFile.Rows
			If TmpRow.Item("Nome") = nome Then
				'DTFile.Rows.RemoveAt(i)
				TmpRow.Delete()
				DTFile.AcceptChanges()
				Exit For
			End If
			'i += 1
		Next
	End Sub

	'Elimina la lista
	Public Sub ClearList()
		Me.DTFile.Clear()
	End Sub

	'Recupera la lista
	Public Function GetList() As DataTable
		Return Me.DTFile.Copy
	End Function
	Public Function GetList(ByVal Id As Integer) As DataTable
		Dim DtTemp As New DataTable("DTTemp")
		DtTemp = Me.DTFile.Clone
		Dim DrTempNew As DataRow
		For Each DrTemp As DataRow In Me.DTFile.Rows
			If DrTemp.Item("IDCom") = Id Then
				DrTempNew = DtTemp.NewRow
				For i As Integer = 0 To 7
					DrTempNew.Item(i) = DrTemp.Item(i)
				Next
				DtTemp.Rows.Add(DrTempNew)
			End If
		Next
		Return DtTemp.Copy
	End Function

	'Inserisce un elemento nella lista
	Public Function Insert(ByVal Nome As String, ByVal UplName As String, ByVal IDMitt As Integer, ByVal Note As String, ByVal IdComunita As Integer, ByVal ByteSize As Integer) As Integer
		'VEDERE COSA SUCCEDE SE INESRISCO 2 VOLTE LO STESSO FILE...
		Dim DRTemp As DataRow
		Dim Errore As Integer = 0

		For Each DRTemp In Me.DTFile.Rows
			If DRTemp.Item("Nome") = Nome Then
				Errore = 1
				Exit For
			End If
		Next

		If Errore = 0 Then
			DRTemp = Me.DTFile.NewRow
			DRTemp("Nome") = Nome
			DRTemp("Tempo") = Now()	'Tempo
			DRTemp("Uploder") = UplName
			DRTemp("IDMitt") = IDMitt
			DRTemp("Note") = Note
			DRTemp("IDStato") = 1
			DRTemp("IDCom") = IdComunita
			DRTemp("ByteSize") = ByteSize

			Me.DTFile.Rows.Add(DRTemp)
			Me.DTFile.AcceptChanges()
		End If

		Return Errore

	End Function

	'Verifica se un elemento è già nella lista
	Public Function IsInsert(ByVal Nome As String) As Boolean
		Dim BIns As Boolean = False
		For Each TmpRow As DataRow In DTFile.Rows
			If TmpRow.Item("Nome") = Nome Then
				BIns = True
				Exit For
			End If
		Next
		Return BIns
	End Function

	Public Function GetNumFile(ByVal IdCom As Integer) As Integer
		Dim NumF As Integer = 0
		For Each Row As DataRow In Me.DTFile.Rows
			If Row.Item("IDCom") = IdCom Then
				NumF += 1
			End If
		Next
		Return NumF
	End Function
End Class
