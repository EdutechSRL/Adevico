Public Class OChtUtentiBlock

	Dim DSBlock As New DataSet
	'Questa funziona: inserisce e toglie e dati...

#Region " NEW - V 12/03/2004 "

	Public Sub New()
		' Declare DataColumn and DataRow variables.
		Dim DTBlock As New DataTable("DTBlock")
		Dim DCTemp As DataColumn

		' I colonna: Id dell'utente che effettua il blocco
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDFrom" ' ID Utente che blocca
		DTBlock.Columns.Add(DCTemp)

		' II colonna: Id dell'utente che subisce il blocco
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "IDTo"	' ID Utente che viene bloccato
		DTBlock.Columns.Add(DCTemp)

		DSBlock.Tables.Add(DTBlock)
		DSBlock.AcceptChanges()

		'Me.Blocca(0, 0) 'Blocco il sistema dal veedre sè stesso... Serve solo per essere sicuro di avere una riga...
	End Sub

#End Region

#Region " Proprietà, metodi e funzioni - V 12/03/2004 "

	Public Sub Blocca(ByVal IdFrom As Integer, ByVal IdTo As Integer)

		Dim DRIn, DrCtrl As DataRow
		Dim BCtrl As Boolean = False

		'Impedisco il blocco verso l'utente che mi ha bloccato...
		For Each DrCtrl In DSBlock.Tables(0).Rows
			If DrCtrl.Item("IDFrom") = IdTo And DrCtrl.Item("IDTo") = IdFrom Then
				BCtrl = True
			End If
		Next

		If Not BCtrl Then
			Try
				DRIn = DSBlock.Tables(0).NewRow
				DRIn.Item(0) = IdFrom
				DRIn.Item(1) = IdTo

				DSBlock.Tables(0).Rows.Add(DRIn)
				DSBlock.AcceptChanges()
			Catch ax As Exception
				'Eventuale codice nel caso in cui l'utente sia già stato inserito
			End Try
		End If

	End Sub

	Public Sub SBlocca(ByVal IdFrom As Integer, ByVal IDTo As Integer)
		Dim DRTemp As DataRow
		Dim i As Integer = 0

		For Each DRTemp In DSBlock.Tables("DTBlock").Rows
			If DRTemp.Item("IDFrom") = IdFrom And DRTemp.Item("IdTo") = IDTo Then
				Exit For
			End If
			i += 1
		Next
		DSBlock.Tables(0).Rows.RemoveAt(i)
		DSBlock.AcceptChanges()
	End Sub

	Public Function GetBlockFrom(ByVal IdFrom As Integer) As DataTable

		Dim DSTemp As New DataSet
		DSTemp = DSBlock.Clone 'Copia la struttura, ma è vuoto!!!
		Dim DRTemp, row As DataRow

		For Each row In DSBlock.Tables(0).Rows
			If row.Item("IdFrom") = IdFrom Then
				DRTemp = DSTemp.Tables(0).NewRow
				DRTemp.Item("IdFrom") = row.Item("IdFrom")
				DRTemp.Item("IdTo") = row.Item("IdTo")
				DSTemp.Tables(0).Rows.Add(DRTemp)
				DSTemp.AcceptChanges()
			End If
		Next

		Return DSTemp.Tables(0).Copy
	End Function

	Public Function GetBlockTo(ByVal IdTo As Integer) As DataTable

		Dim DSTemp As New DataSet
		DSTemp = DSBlock.Clone

		Dim DRTemp, row As DataRow



		For Each row In DSBlock.Tables(0).Rows
			If row.Item("IdTo") = IdTo Then
				DRTemp = DSTemp.Tables(0).NewRow
				DRTemp.Item("IdFrom") = row.Item("IdFrom")
				DRTemp.Item("IdTo") = row.Item("IdTo")
				'Try
				DSTemp.Tables(0).Rows.Add(DRTemp)
				'Catch ex As Exception
				'End Try
				DSTemp.AcceptChanges()
			End If
		Next

		Return DSTemp.Tables(0).Copy
	End Function

	'Mi sa che nn serve più, ma è lo stesso... Meglio lasciarlo per ora...
	Public Function GetAllBlock() As DataTable
		Return DSBlock.Tables(0).Copy
	End Function

#End Region

End Class
