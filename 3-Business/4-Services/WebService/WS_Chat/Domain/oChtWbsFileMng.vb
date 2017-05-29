Imports System.IO
Imports System.Threading
Imports lm.Comol.Core.File

Public Class oChtWbsFileMng

	'In XML config file
	Private MaxByte As Integer = 2097152 '2 Mb
	Private Path As String 'V
	Private TmpFileDir As String = "/TmpFile/" '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
	Private ConfFileName As String = "/FileConf.xml" 'V
	Private HTimDel As Double = 12 'Indica ogni quante ORE avviene il controllo per l'eliminazione dei file
	Private TimeDel As Double = 48 'Ore in cui viene garantita l'esistenza del file su server
	Private LogFileName As String = "/WBS_CHAT_FILE.LOG"
	'Altri interni
	'Private ThDelFile As New System.Threading.Thread(AddressOf ThrDelOldFile)
	Private Kill As Boolean = False
	Private FileList As New oChtWbsFmList
	Private IsRW As Boolean = False	'Flag per la verifica dello stato di lettura/scrittura...

#Region "Metodi new e di inizializzazione"

	'Inizializzazione di default
	Public Sub New(ByVal Path As String)
		If Not Path = "" Then
			Me.Path = Path
		Else
			Me.Path = "C:\Temp\"
		End If

		Me.Initialize()

		'Creazione temp dir contenente i file (se non esiste)
		'Inizializzazione ed avvio thread di controllo x cancellazione file

		'Varie ed eventuali
	End Sub


	'Inizializzazione di default
	Public Sub New(ByVal Path As String, ByVal IDComunita As Integer)
		If Not Path = "" Then
			Me.Path = Path
		Else
			Me.Path = "C:\Temp\"
		End If

		If Not IDComunita = 0 Then
			Me.TmpFileDir &= IDComunita.ToString
		End If

		Me.Initialize()

		'Creazione temp dir contenente i file (se non esiste)
		'Inizializzazione ed avvio thread di controllo x cancellazione file

		'Varie ed eventuali
	End Sub

	'Inizializzazione con nome dir o nome file
	Public Sub New(ByVal Path As String, ByVal Name As String, ByVal IsDir As Boolean)

		If Not Path = "" Then
			Me.Path = Path
		Else
			Me.Path = "C:\Temp\"
		End If

		If Not Name = "" Then
			If IsDir Then
				Me.TmpFileDir = Name
			Else
				Me.ConfFileName = Name
			End If
		End If
		Me.Initialize()
	End Sub

	'Inizializzazione con nome dir e nome file
	Public Sub New(ByVal Path As String, ByVal ConFileName As String, ByVal TmpFileDir As String)

		If Not Path = "" Then
			Me.Path = Path
		Else
			Me.Path = "C:\Temp\"
		End If

		If Not TmpFileDir = "" Then
			Me.TmpFileDir = TmpFileDir
		End If
		If Not TmpFileDir = "" Then
			Me.ConfFileName = ConfFileName
		End If
		Me.Initialize()
	End Sub

	'Creazione della directory temporanea per i file e del file di configurazione
	Private Function CreateFileDir() As Integer	'Ricontrollare Il discorso ByVal Path as String

		Dim DTTemp As New DataTable("DTTemp")
		Dim DCTemp As DataColumn
		Dim DRTemp As DataRow

		'### Creo struttura
		' I colonna: nome directory temporanea
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "NameDir"
		DTTemp.Columns.Add(DCTemp)

		' II colonna: nome File di configurazione
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "ConfFileName"
		DTTemp.Columns.Add(DCTemp)

		' III colonna: dimensione massima del file upload
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "Byte"
		DTTemp.Columns.Add(DCTemp)

		' IV colonna: Indica ogni quante ore avviene il controllo dei file obsoleti
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "TimDel"
		DTTemp.Columns.Add(DCTemp)

		' V colonna: indica quante ore un file può sopravvire sul server prima di venir cancellato
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.Int32")
		DCTemp.ColumnName = "HTimDel"
		DTTemp.Columns.Add(DCTemp)

		' V colonna: Nome del file di log
		DCTemp = New DataColumn
		DCTemp.DataType = System.Type.GetType("System.String")
		DCTemp.ColumnName = "LogFileName"
		DTTemp.Columns.Add(DCTemp)

		'### Inserisco dati
		DRTemp = DTTemp.NewRow
		DRTemp("NameDir") = TmpFileDir
		DRTemp("ConfFileName") = Me.ConfFileName
		DRTemp("Byte") = Me.MaxByte
		DRTemp("TimDel") = Me.TimeDel
		DRTemp("HTimDel") = Me.HTimDel
		DRTemp("LogFileName") = Me.LogFileName

		DTTemp.Rows.Add(DRTemp)
		DTTemp.AcceptChanges()

		Dim DsTemp As New DataSet
		DsTemp.Tables.Add(DTTemp)

		Try
			DsTemp.WriteXml(Path & ConfFileName)
		Catch ex As Exception
			Dim strerr As String = ex.ToString
		End Try
        'Creo la directory temporanea
        Dim fMessage As FileMessage = lm.Comol.Core.File.Create.Directory_FM(Path & TmpFileDir)
        If Not (fMessage = FileMessage.DirectoryExist OrElse fMessage = FileMessage.FolderCreated) Then
            'Metodi controllo e gestione errori di IO...
            Dim strerr As String = fMessage.ToString
        End If
    End Function

	'Inizializzazione: lettura file di configurazione e resto...
	Public Sub Initialize()

        If Not IO.File.Exists(ConfFileName) Or IO.Directory.Exists(Me.TmpFileDir) Then
            Me.CreateFileDir()
        Else
            Dim DsTemp As New DataSet
            Try
                DsTemp.ReadXml(ConfFileName)
            Catch ex As Exception

            End Try
            Me.ConfFileName = DsTemp.Tables(0).Rows(0).Item("ConfFileName")
            Me.TmpFileDir = DsTemp.Tables(0).Rows(0).Item("NameDir")
            Me.MaxByte = DsTemp.Tables(0).Rows(0).Item("Byte")
            'Me.Path=
            Me.TimeDel = DsTemp.Tables(0).Rows(0).Item("TimDel")
            Me.HTimDel = DsTemp.Tables(0).Rows(0).Item("HTimDel")

            CreateFileDir()
        End If

		'Inizio creazione thread
		'With Me.ThDelFile
		'    .IsBackground = True
		'    .Priority = ThreadPriority.Lowest
		'    .Start()
		'End With

	End Sub

#End Region

#Region "Metodi lettura e scrittura file"

	Public Function WriteFile(ByVal InputFile() As Byte, ByVal FileName As String, ByVal UplName As String, ByVal IDUpl As Integer, ByVal note As String, ByVal IDCom As Integer) As String
		Me.IsRW = True

		Dim ErrorString As String = ""
		Dim ErrorNumber As Integer
		If Me.FileList.IsInsert(FileName) Then
			ErrorString = "Nome file esistente o non valido"
		End If
        Dim fs As New FileStream(Me.Path & Me.TmpFileDir & "/" & FileName, FileMode.Create)
		Try
			fs.Write(InputFile, 0, InputFile.Length)
			ErrorNumber = Me.FileList.Insert(FileName, UplName, IDUpl, note, IDCom, InputFile.Length)
			If Not ErrorNumber = 0 Then
				ErrorString = "FL" & ErrorNumber.ToString
			End If
		Catch ex As Exception
			ErrorString = ex.ToString 'Umanizzare l'EX.TOSTRING!!!
		Finally
			fs.Close()
			Me.IsRW = False
		End Try
		If ErrorString = "" Then
			ErrorString = "File caricato con successo!"
		End If

		'File di log:
		Dim LogString As String = ""
		LogString = vbCrLf & Now() & vbCrLf & "Nome utente: " & UplName
		LogString &= vbCrLf & "ID: " & IDUpl.ToString
		LogString &= vbCrLf & "Nome File: " & FileName
		LogString &= vbCrLf & "ID Comunita': " & IDCom
		LogString &= vbCrLf & "Action: UPLOAD"
		LogString &= vbCrLf & "______________________________"
		Me.WriteLog(LogString)

		Return ErrorString
	End Function

#End Region

	Public ReadOnly Property WRState() As Boolean
		Get
			Return IsRW
		End Get
	End Property

	Public Property TmpDir() As String
		Get
			Return Me.TmpFileDir
		End Get
		Set(ByVal Value As String)
			Me.TmpFileDir = Value
		End Set
	End Property

	Public Sub EraseALLFile(ByVal IdCom As Integer)
        Delete.File(Me.Path & Me.TmpFileDir & "*.*")
    End Sub

	Public Function EraseFile(ByVal FileName As String, ByVal IdUser As Integer, ByVal IDCom As Integer) As String
		Me.IsRW = True
		Dim errMsg As String = ""
		Try
            Delete.File(Me.Path & Me.TmpFileDir & FileName)
			Me.FileList.RemListFile(FileName)
		Catch ex As Exception
			errMsg = ex.ToString
		Finally
			Me.IsRW = False
		End Try

		'File di log:
		Dim LogString As String = ""
		LogString = vbCrLf & Now() & vbCrLf & "Nome utente: " & "nd"
		LogString &= vbCrLf & "ID: " & IdUser.ToString
		LogString &= vbCrLf & "Nome File: " & FileName
		LogString &= vbCrLf & "ID Comunità: " & IDCom
		LogString &= vbCrLf & "Action: REMOVE"
		LogString &= vbCrLf & "______________________________"
		Me.WriteLog(LogString)

		Return errMsg
	End Function

#Region "Metodi gestione lista file"

	Public Function getListFile(ByVal id As Integer) As DataTable
		Return Me.FileList.GetList(id).Copy
	End Function

#End Region

	Public Function GetNumFile(ByVal IdCom As Integer) As Integer
		Return Me.FileList.GetNumFile(IdCom)
	End Function

	Private Sub WriteLog(ByVal Messaggio As String)
		Messaggio &= vbCrLf
		'For i As Integer = 0 To Messaggio.Length - 1
		'    File(i) = Microsoft.VisualBasic.AscW(Messaggio.Chars(i))
		'Next
		'Dim Fs As New FileStream(Me.Path & Me.LogFileName, FileMode.Append)
		Dim Fs As StreamWriter = File.AppendText(Me.Path & Me.LogFileName)
		Try
			Fs.Write(Messaggio)
		Catch ex As Exception
		Finally
			Fs.Close()
		End Try
	End Sub

	Public Function IsFileExist(ByVal FileName As String) As Boolean
		Return IO.File.Exists(Me.Path & Me.TmpFileDir & FileName)
	End Function

End Class

#Region "Thread"

''Thread per la cancellazione dei file obsoleti...
'Private Sub ThrDelOldFile() 'OK Funziona e cancella i file! :D ;)
'    Dim FileList As String()
'    Do
'        System.Threading.Thread.CurrentThread.Sleep(Me.HTimDel * 60 * 60 * 1000) 'Sleep di HTimDel ore
'        'System.Threading.Thread.CurrentThread.Sleep(15 * 1000)
'        'Do
'        'Loop While Me.IsRW
'        'Me.IsRW = True
'        ChDir(Me.Path & Me.TmpFileDir) 'Entra nella cartella specificata... (RIVEDERE IL DISTORSO DEL PATH ASSOLUTO E RELATIVO....)
'        FileList = System.IO.Directory.GetFiles(Me.Path & Me.TmpFileDir) 'Recupera l'elenco dei file in un Array tipo Stringa
'        For Each File As String In FileList 'Scorre l'array
'            If System.IO.File.GetCreationTime(File).AddHours(Me.TimeDel) < Now() Then 'Se il file è stato creato da troppo tempo viene eliminato
'                'If System.IO.File.GetLastAccessTime(File) > Now().AddHours(Me.HTimDel) Then 'Se il file è stato creato da troppo tempo viene eliminato
'                System.IO.File.Delete(File) 'Elimina il file
'                Me.FileList.RemListFile(File) 'Elimina l'elemento dalla lista...
'                'File di log:
'                Dim LogString As String = ""
'                LogString = vbCrLf & Now() & vbCrLf & "Nome utente: " & "System"
'                LogString &= vbCrLf & "Nome File: " & File
'                LogString &= vbCrLf & "Action: System delete old files"
'                LogString &= vbCrLf & "______________________________"
'                Me.WriteLog(LogString)
'            End If
'        Next
'        'Me.IsRW = False
'    Loop Until Kill
'End Sub

'Public Sub KillThread()
'    Kill = True 'Dovrebbe bastare questo: ma è necessario ricrearlo e riavviarlo se si usa questo metodo...
'End Sub
'Public Sub SuspThread()
'    Me.ThDelFile.Suspend()
'End Sub
'Public Sub ResumeThread()
'    Me.ThDelFile.Resume()
'End Sub

#End Region
