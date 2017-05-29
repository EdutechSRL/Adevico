'Imports System.IO
'Imports System.IO.IsolatedStorage
Imports System.Net
Imports System.Drawing
Imports lm.Comol.Core.File

Public Class FileHelpers

    'Public Enum FileMessage As Integer
    '	None = 0			' nessun errore
    '	Exsist = 1			' file già inserito
    '	NotUploaded = 2		' mancato upload
    '	ZeroByte = 3		' lunghezza file nulla
    '	NotFound = 4	' file non trovato
    '	MismatchType = 5 ' tipo 
    '	NotDeleted = 6
    '	NotCreated = 7
    '   End Enum

	Public Shared Function FileExists(ByVal filename As String) As Boolean
        Return Exists.File(filename)
	End Function
	Public Shared Function DirectoryExists(ByVal DirectoryName As String) As Boolean
        Return Exists.Directory(DirectoryName)
	End Function
	Public Shared Function DeleteFile(ByVal filename As String) As FileMessage
        Return Delete.File_FM(filename)
	End Function
	Public Shared Function DeleteDirectory(ByVal DirectoryName As String) As FileMessage
        Delete.Directory(DirectoryName, True)
    End Function
    Public Shared Function DirectorySize(ByVal DirectoryName As String, Optional ByVal AlsoSubfolders As Boolean = False) As Int64
        Return ContentOf.Directory_Size(DirectoryName, AlsoSubfolders)
    End Function
    Public Shared Function DirectoryIsEmpty(ByVal DirectoryName As String) As Boolean
        ContentOf.Directory_isEmpty(DirectoryName)
    End Function

	Public Shared Function RetrieveFileFromCompleteName(ByVal oCompleteName As String) As BaseFile
		Dim oBaseFile As BaseFile
		oCompleteName = Replace(oCompleteName, "/", "\")

		Try
			If oCompleteName.EndsWith("\") Then
				oBaseFile = New BaseFile("", oCompleteName)
			ElseIf oCompleteName.LastIndexOf("\") <> -1 Then
				oBaseFile = New BaseFile(oCompleteName.Substring(oCompleteName.LastIndexOf("\") + 1), Left(oCompleteName, oCompleteName.LastIndexOf("\") + 1))
			Else
				oBaseFile = New BaseFile(oCompleteName, "")
			End If
			Return oBaseFile
		Catch ex As Exception

		End Try
		Return Nothing
	End Function
	Public Shared Function CreateDirectoryByFileName(ByVal FileName As String) As Boolean
		Dim oBaseFile As BaseFile = RetrieveFileFromCompleteName(FileName)

		If oBaseFile.Path <> "" Then
			Return CreateDirectoryByPath(oBaseFile.Path)
		Else
			Return True
		End If
	End Function

    Public Shared Function CreateDirectoryByPath(ByVal DirectoryName As String) As Boolean
        Return Create.Directory(DirectoryName)
    End Function

	Public Shared Function ReadTextFile(ByVal filename As String) As String
        Return ContentOf.TextFile(filename)
	End Function
End Class

Public Class BaseFile

#Region "Private Property"
	Private _Path As String
	Private _Name As String
	Private _Size As Long
#End Region

#Region "Public Property"
	Public ReadOnly Property Name() As String
		Get
			Name = _Name
		End Get
	End Property
	Public ReadOnly Property Path() As String
		Get
			Path = _Path
		End Get
	End Property
	Public ReadOnly Property Size() As Long
		Get
			Size = _Size
		End Get
	End Property
	Public ReadOnly Property CompleteName() As String
		Get
			CompleteName = _Path & _Name
		End Get
	End Property
#End Region

#Region "Metodi New"
	Sub New(ByVal oCompleteName As String)
		'	Dim oBaseFile As BaseFile = RetrieveFileFromCompleteName()
	End Sub
	Sub New(ByVal oName As String, ByVal oPath As String)
		_Name = oName
		_Path = oPath
	End Sub
	Sub New(ByVal oName As String, ByVal oPath As String, ByVal oSize As Long)
		_Name = oName
		_Path = oPath
		_Size = oSize
	End Sub
#End Region


    Public Function Exists() As Boolean
        Return lm.Comol.Core.File.Exists.File(Me.CompleteName)
    End Function


#Region "Metodi"

	Public Shared Function FileNameOnly(ByVal strFileName As String) As String
		'funzione che passandogli la stringa del percorso ASSOLUTO o RELATIVO di un file restituisce
		'il solo nome del file (funzionante sia su IE che su Gecko)

		Dim intFileNameLength As Integer
		Dim strFileNameOnly As String
		If InStr(strFileName, "\") > 0 Or InStr(strFileName, "/") > 0 Then

			intFileNameLength = InStr(1, StrReverse(strFileName), "\")
			If intFileNameLength = 0 Then
				intFileNameLength = InStr(1, StrReverse(strFileName), "/")
			End If

			strFileNameOnly = Mid(strFileName, (Len(strFileName) - intFileNameLength) + 2)
		Else
			strFileNameOnly = strFileName
		End If

		Return strFileNameOnly
	End Function

#End Region
End Class

Public Class SizeHelpers
	Public Enum FileSizeCostants
		K = 10
		M = 20
		G = 30
		T = 40
		P = 50
	End Enum

	Public Shared Function FS(ByVal s As FileSizeCostants) As Long
		Return Math.Pow(2, s)
	End Function

	Public Shared Function Kilo() As Long
		Return FS(FileSizeCostants.K)
	End Function

	Public Shared Function Mega() As Long
		Return FS(FileSizeCostants.M)
	End Function

	Public Shared Function Giga() As Long
		Return FS(FileSizeCostants.G)
	End Function

	Public Shared Function Tera() As Long
		Return FS(FileSizeCostants.T)
	End Function

	Public Shared Function Peta() As Long
		Return FS(FileSizeCostants.P)
	End Function
End Class